using System;
using System.IO;

namespace Yeol
{
    public class ByteBuffer
    {
        /// <summary>
        ///     The buffer: 데이터 읽기
        /// </summary>
        private readonly byte[] _buffer;

        /// <summary>
        ///     머리, 꼬리, 버퍼의 배타적 접근
        /// </summary>
        private readonly object _syncObject;

        /// <summary>
        ///     데이터의 시작지점
        /// </summary>
        private int _head;

        /// <summary>
        ///     데이터의 끝지점
        /// </summary>
        private int _tail;

        public ByteBuffer(int capacity) // 생성자
        {
            _syncObject = new object();
            // _buffer.length = ushort.MaxValue;
            _buffer = capacity > 0 ? new byte[capacity] : new byte[ushort.MaxValue];
        }

        public int Length { get; private set; }

        /// <summary>
        ///     버퍼에서 유효한 데이터의 시작지점과 끝지점을 0으로 만든다.
        /// </summary>
        public void Clear()
        {
            // 시작과 끝 위치가 리셋되니 굳이 버퍼의 모든 값을 0으로 초기화하지는 않는다.
            lock (_syncObject)
            {
                _head = 0;
                _tail = 0;
            }
        }

        // lock 블럭 내에서 호출하기
        private void UpdateLength()
        {
            if (_tail > _head) Length = _tail - _head;
            else if (_tail < _head) Length = _buffer.Length - _head + _tail;
            else Length = 0; // tail == head
        }

        /// <summary>
        ///     Buffer에 Data 쓰기
        ///     기록할 때는 tail 변경 - buffer에 데이터 이어붙이기 위함
        /// </summary>
        /// <param name="value"></param> 읽어드린 데이터
        public void Write(byte[] value)
        {
            if (value == null || value.Length <= 0) return;
        
            lock (_syncObject) // lock: 특정 블럭의 코드(Critical Section이라 부른다)를 한번에 하나의 쓰레드만 실행할 수 있도록 해준다.
            {
                if (Length + value.Length > _buffer.Length) throw new InternalBufferOverflowException("There was not enough buffer space.");

                if (_tail + value.Length < _buffer.Length)
                {
                    /// <summary>
                    ///     BlockCopy(원본배열, 원본 배열의 복사 시작 위치, 복사될 배열, 복사될배열의 시작위치, 복사 개수)
                    /// </summary>
                    Buffer.BlockCopy(value, 0, _buffer, _tail, value.Length);
                    _tail += value.Length; // 뒤로 데이터 이어붙이기위해 tail값 변경
                }
                else
                {
                    var freeLength = _buffer.Length - _tail;
                    var lackLength = value.Length - freeLength;
                    Buffer.BlockCopy(value, 0, _buffer, _tail, freeLength);
                    Buffer.BlockCopy(value, freeLength, _buffer, 0, lackLength);
                    _tail = lackLength;
                }

                // 데이터를 기록했으니 버퍼길이 조정
                UpdateLength();
            }
        }

        public byte[] ReadToEnd(bool isReadOnly)
        {
            return Read(Length, isReadOnly);
        }

        // 읽어갈 때는 head 변경(Readonly false)
        public byte[] Read(int length, bool isReadOnly)
        {
            return Read(0, length, isReadOnly);
        }

        /// <summary>
        ///     Buffer Data 읽기
        /// </summary>
        /// <param name="offset"></param> 시작시점
        /// <param name="length"></param> 데이터 길이
        /// <param name="isReadOnly"></param> 반환종류 선택
        /// <returns></returns>
        public byte[] Read(int offset, int length, bool isReadOnly)
        {
            lock (_syncObject)
            {
                if (Length < length + offset) throw new ArgumentOutOfRangeException(nameof(length));

                // Length는 0보다 크거나 같으니, length가 0보다 작은 경우가 올 수 없다.             
                var buffer = new byte[length];
                var head = _head + offset;

                /// <summary>
                ///     데이터 버퍼 return 전에 위치 조정
                /// </summary>
                // 처음(head 0)부터 차례로 읽기 위해서 head 값 조정
                if (head >= _buffer.Length) head -= _buffer.Length;

                // 데이터 크기가 맞지 않는 경우 buffer 위치 조정
                if (head + length > _buffer.Length)
                {
                    var tailLength = _buffer.Length - head;
                    var headLength = length - tailLength;
                    if (tailLength < 0 || headLength < 0) tailLength = 0;
                    Buffer.BlockCopy(_buffer, head, buffer, 0, tailLength);
                    Buffer.BlockCopy(_buffer, 0, buffer, tailLength, headLength);
                    head = headLength;
                }
                else
                {
                    Buffer.BlockCopy(_buffer, head, buffer, 0, length);
                    head += length;
                }

                if (isReadOnly) return buffer;

                // 헤더를 옮기고 버퍼 길이를 업데이트
                _head = head;
                UpdateLength();
                return buffer;
            }
        }

        /// <summary>
        ///     Chart 데이터 왼쪽으로 한 칸 씩 이동
        /// </summary>
        /// <param name="number"></param> MoveNextLength = 1
        public void MoveNext(int number)
        {
            lock (_syncObject)
            {
                if (number < 0 || number > Length) throw new ArgumentOutOfRangeException(nameof(number));

                var head = _head + number; // number칸 씩 이동시키기 위해 head 값 변경
                if (head > _buffer.Length) head -= _buffer.Length;
                _head = head;
                UpdateLength();
            }
        }
    }
}