using System;

namespace Yeol
{
    public abstract class TerminalBase : IDisposable // IDisposable: 관리되지 않은 리소스 해제를 위한 메커니즘 제공
    {
        // Dipose 체크 플래그 변수
        private bool _isDisposed;

        protected TerminalBase(string name)
        {
            Name = name;
        }

        public string Name { get; }

        /// <summary>
        ///     IDisposable을 구현
        ///     가상 Dispose 메서드를 호출하고 finalize를 호출X
        /// </summary>        
        public void Dispose()
        {
            Dispose(true); // 관리, 비관리 리소스 해제
            GC.SuppressFinalize(this); // finalizer가 호출되지 않도록 함
        }

        ~TerminalBase()
        {
            Dispose(false); // finalizer에서 비관리 리소스 해제
        }

        /// <summary>
        ///     리소스 정리를 위한 공통 작업을 수행하고 파생클래스에서도 리소스를 정리할 수 있도록 제공
        ///     ㄴ 상속받은 클래스에서도 리소스를 해제할 수 있도록 도와주는 코드
        ///     isDisposing: true - 관리/비관리 리소스 모두 헤제, false - 비관리 리소스만 정리
        ///     코드 마지막 부분에서 "반드시" base class에서 정의하는 Dipose를 호출
        /// </summary>
        /// <param name="isDisposing"></param>
        protected virtual void Dispose(bool isDisposing)
        {
            if (_isDisposed) return; // 중복 실행 방지
            if (isDisposing) 
            {
                // 관리 리소스 정리
            }
            // 비관리 리소스 정리

            _isDisposed = true;
            // Free native resources

            //if (!isDisposing) return;
            // Dispose managed resources
        }

        public event EventHandler<NetworkEventArgs<Exception>> Faulted;
        public event EventHandler<NetworkEventArgs<string>> Notified;


        protected void OnFaulted(Exception exception)
        {
            Faulted?.Invoke(this, new NetworkEventArgs<Exception>(exception));
        }

        protected void OnNotified(string message)
        {
            Notified?.Invoke(this, new NetworkEventArgs<string>(message));
        }
    }
}