using System;
using System.Threading;
using RJCP.IO.Ports;

namespace Yeol
{
    /// <summary>
    ///    < 포인트 >
    ///    1. 실체 클래스들의 공통된 필드와 메소드의 이름을 통할할 목적 + 사용시간 절약
    ///    2. 객체를 생성할 수 없다. 실체성X 구체적X
    ///    3. 추상클래스와 실체클래스 = 상속관계
    ///    
    ///     < 용도 >
    ///     1. 공통된 필드와 매서드를 통일할 목적
    ///     2. 규격(추상메소드, 필드 등)에 맞는 실체클래스 구현
    /// </summary>
    public abstract class SerialPortBase : TerminalBase
    {
        // 필드
        public static int BufferSize = ushort.MaxValue; // 데이터 저장

        private bool _isDisposed;
        private SerialPortStream _serialPortStream;  // SerialPortStream: 직렬포트에서 데이터를 버퍼링(데이터 임시 저장공간)하는 Stream의 기본 구현

        /// <summary>
        ///    < Poperty >
        ///    정보은닉된 클래스의 변수(필드, 메소드 등)를 불러오기 위해서 get/set 함수사용
        ///    set 접근자 내부의 value는 전달받은 인자값을 의미
        ///    자동구현 프로퍼티 = get(읽기 전용); set(쓰기 전용);  ~ 초기화 가능
        /// </summary>
        public static int MoveNextLength { get; set; } = 1;
        public static int ThreadSleepTime { get; set; } = 0;
        public static int ValueArraySize { get; set; } = 1;

        /// <summary>
        ///    ByteBuffer는 바이트 데이터를 저장하고 읽는 저장소
        /// </summary>
        protected ByteBuffer ByteBuffer { get; } = new ByteBuffer(BufferSize);
        protected int IgnoredByteCount { get; set; }

        /// <summary>
        ///     => 람다식: 접근자, 함수이름, return 문이 없는 익명함수 - 간단함
        ///     a ?? b : null X = a, null = b
        ///     _serialPortStream?.IsOpen = _serialPortStream가 null도 받을 수 있음(Nullable)
        ///     null : 어떤 객체도 참조하지 않음을 의미할 수 있다.
        /// </summary>
        public bool IsOpen => _serialPortStream?.IsOpen ?? false;

        /// <summary>
        ///     생성자: new 연산자를 통해서 인스턴스를 생성할 때 반드시 호출되고 먼저 실행되는 메소드
        ///     인스턴스 변수(필드값 등)를 초기화시키는 역할
        ///     추상클래스의 생성자는 클래스에 필요한 제약을 줄 때 사용
        /// </summary>
        protected SerialPortBase(string name) : base(name)  // protected: 자식클래스만 사용
        {
        }

        public int BaudRate
        {
            set
            {
                if (!IsOpen) return;
                _serialPortStream.BaudRate = value;
            }
        }

        // EvnetArgs: 이벤트를 보내는 객체
        public event EventHandler<NetworkEventArgs<LinkErrors>> ErrorReceived; // 에러
        public event EventHandler<NetworkEventArgs<int>> ByteIgnored; // 특정값을 전달받으면 이벤트 시행

        /// <summary>
        ///     오버라이딩: 부모클래스의 메소드를 자식클래스의 메소드가 덮은 상황
        ///     ㄴ virtual(부모), override(자식)로 명시
        ///     
        ///     Dispose
        ///     일반적으로 .NET에서 생성되는 개체는 모두 Managed Heap 메모리 공간에 생성되고 관리된다.
        ///     메모리 관리는 GC의 역할(프로그램 컨트롤X), Dispose는 메모리가 아닌 다른 리소스(파일 핸들, GDI 리소스, DB 커넥션 등)를 해제하기 위함
        ///     즉, GC가 제어해주지 못하는 메모리 이외의 리소스에 해당하는 것
        /// </summary>
        protected override void Dispose(bool isDisposing)
        {
            if (_isDisposed) return;
            _isDisposed = true;
            // Free native resources
            _serialPortStream?.Dispose();

            //if (!isDisposing) return;
            // Dispose managed resources
            base.Dispose(isDisposing); // base: TerminalBase
        }

        public static string[] GetPortNames()
        {
            return SerialPortStream.GetPortNames();
        }

        /// <summary>
        ///     포트 연결 - 포트번호, 통신속도 설정
        /// </summary>
        public void Open(string portName, int baudRate)
        {
            _serialPortStream = new SerialPortStream(portName, baudRate);
            //_serialPortStream.DiscardInBuffer();
            //_serialPortStream.DiscardOutBuffer();
            _serialPortStream.DataReceived += SerialPortStream_DataReceived;    // DataReceived: SerialPort 개체가 나타내는 포트를 통해 데이터를 수신했음을 나타냄
            _serialPortStream.ErrorReceived += SerialPortStream_ErrorReceived;  // ErrorReceived: SerialPort 개체가 나타내는 포트에서 오류가 발생했음을 나타냄
            _serialPortStream.Open();
        }

        public void Close()
        {
            ByteBuffer.Clear(); // 버퍼에서 유효한 데이터의 시작지점과 끝지점을 0으로 만든다.
            _serialPortStream?.Close(); // 포트 연결을 닫고, IsOpen 속성을 false로 설정하고, 내부 stream 개체를 삭제
            _serialPortStream?.Dispose(); // SerialPort에서 사용하는 관리되지 않는 리소스를 해제하고, 관리되는 리소스를 선택적으로 해제
            _serialPortStream = null;
        }

        protected void Send(byte[] buffer)
        {
            /// <summary>
            ///     public void Write(byte[] buffer, int offset, int count)
            ///     ㄴ buffer: 포트에 쓸 데이터가 포함된 바이트 배열, offset: 포트에 바이트 복사를 시작할 buffer 매개변수의 0부터 시작하는 바이트 오프셋(시작점), count: 쓸 바이트 수
            ///     버퍼의 데이터를 사용하여 지정된 수의 바이트를 직렬포트 출력 버퍼에 쓴다
            /// </summary>
            _serialPortStream.Write(buffer, 0, buffer.Length);
        }

        public void Receive(int threshold)
        {
            // ex) tmp = 조건 ? 참일 경우 return : 거짓일 경우 return
            var state = threshold > 0 ? threshold : 1;

            /// <summary>
            ///     System.Threading.ThreadPoll
            ///     작업실행, 작업 항목 게시, 비동기 I/O 처리, 다른 스레드 대신 기다리기 및 타이머 처리에 사용할 수 있는 스레드 풀 제공
            ///     
            ///     .QueueUserWorkItem(WaitCallback, Object): 실행을 위해 메서드를 큐에 대기시키고 메서드에서 사용할 데이터가 들어있는 개체를 지정
            ///     ㄴ WaitCallback: 실행할 메서드, state: 메서드에서 사용할 데이터가 들어 있는 개체, return: boolean(true: 성공적으로 대기)
            /// </summary>
            ThreadPool.QueueUserWorkItem(Receiver, state);
        }

        /// <summary>
        ///     object sender, EventArgs e
        ///     sender: 어떤 오브젝트가 이 이벤트를 유발시켰는가, e는 이벤트 발생과 관련된 정보(이벤트 핸들러 파라미터)
        /// </summary>
        private void SerialPortStream_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            if (e.EventType == SerialError.NoError) return; // 에러없으면 리턴

            /// <summary>
            ///     동시성이 있는 멀티스레드 프로그램 환경에서 특정스레드에서 생성된 Win Form Control(Textbox, ListView 등)을 다른스레드에서 접근할 때 충돌 발생
            ///     스레드에서 안전한 방식으로 컨트롤에 접근하려면 컨트롤을 생성한 스레드가 아닌 다른 스레드에서 WFC에 접근할때 invoke 사용
            ///     
            ///     delegate(대리자): c/c++의 포인터, 이벤트 관련된 처리에 사용, method의 포인터를 저장할뿐 내부 코드 기술X
            ///     ㄴ public delegate [반환형식][이름](매개변수)
            ///     invoke: 별도의 스레드에서 컨트롤박스에 접근하려고 하면 서로 다른 스레드가 하나의 컨트롤 객체에 접근하는 것을 방지하기 위해 사용(크로스 스레드 오류현상)
            /// 
            ///     invoke를 통해 delegate 함수인 EventHandler를 호출
            ///     this: 발상한 개채(본인), 두번째 인자: 이벤트데이터 클래스 이름
            /// </summary>
            ErrorReceived?.Invoke(this, new NetworkEventArgs<LinkErrors>(e.EventType.ToLinkErrors()));  // 어떤 이유로 error 났는지 event로 알려줌
        }

        private void SerialPortStream_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var length = _serialPortStream.BytesToRead; // 수신버퍼에 있는 데이터의 바이트 수
            if (length <= 0) return;    // 수신 데이터가 없으면 리턴
            var buffer = new byte[length];  // byte = 0~255
            // ReSharper disable once MustUseReturnValue

            /// <summary>
            ///     public int Read(byte[] buffer, int offset, int count);
            ///     ㄴ buffer: 입력내용을 쓸 바이트 배열, offset: 바이트를 쓸 buffer 오프셋, count: 읽을 최대 바이트 수
            ///     SerialPort 입력 버퍼에서 여러 바이트를 읽고 해당 바이트를 바이트 배열의 지정된 오프셋에 쓴다
            /// </summary>
            _serialPortStream.Read(buffer, 0, buffer.Length);
            ByteBuffer.Write(buffer); // 데이터 저장용 - ProcessBuffer를 통해서 저장된 데이터를 빼오는 용도
        }

        /// <summary>
        ///     EMG data receive
        /// </summary>
        private void SerialPortStream_EmgDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // 수신버퍼에 있는 데이터의 바이트 수
            var length = _serialPortStream.BytesToRead;

            if (length != 0)
            {
                var buffer = new byte[length];  // byte = 0~255
                try
                {
                    _serialPortStream.Read(buffer, 0, length);
                    ByteBuffer.Write(buffer); // 데이터 저장용 - ProcessBuffer를 통해서 저장된 데이터를 빼오는 용도
                }
                catch { }
            }
        }

        protected void OnByteIgnored()
        {   
            ByteIgnored?.BeginInvoke(this, new NetworkEventArgs<int>(IgnoredByteCount), null, null); // BeginInvoke: 비동기식 invoke
            IgnoredByteCount = 0;
        }

        private void Receiver(object state)
        {
            var threshold = (int)state;
            try
            {
                while (IsOpen)
                {
                    // ByteBuffer에 쓰인 데이터 길이만큼 데이터를 다 받으면 진행
                    if (ByteBuffer.Length >= threshold) ProcessBuffer();
                    Thread.Sleep(ThreadSleepTime);
                }
            }
            catch (ObjectDisposedException e)
            {
                OnNotified(e.ToString());
            }
            catch (Exception e)
            {
                OnFaulted(e);
            }
        }

        protected abstract void ProcessBuffer();
    }
}