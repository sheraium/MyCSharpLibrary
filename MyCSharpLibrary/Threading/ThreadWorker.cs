using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace MyCSharpLibrary.Threading
{
    public class ThreadWorker : IDisposable
    {
        private int _interval = 1000;
        private Action _task;
        private bool _doWorkFlag;
        private bool _runFlag;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private Thread _worker = null;
        public bool IsRunning => _doWorkFlag;

        public ThreadWorker(Action task, int interval = 1000, bool startFlag = true)
        {
            Initial();

            _task = task;
            _interval = interval;
            _doWorkFlag = startFlag;
        }

        public void Start()
        {
            _doWorkFlag = true;
        }

        public void Pause()
        {
            _doWorkFlag = false;
        }

        private void Initial()
        {
            _runFlag = true;
            _worker = new Thread(new ThreadStart(WorkProcess));
            _worker.IsBackground = true;
            _worker.Start();
        }

        private void WorkProcess()
        {
            while (_runFlag)
            {
                _stopwatch.Start();
                if (_doWorkFlag)
                {
                    try
                    {
                        _task?.Invoke();
                    }
                    catch (Exception ex)
                    {
                    }
                }
                _stopwatch.Stop();
                var haveToWait = HaveToWait(_stopwatch);
                _stopwatch.Reset();
                SpinWait.SpinUntil(() => haveToWait < 0, haveToWait);
            }
        }

        private int HaveToWait(Stopwatch stopwatch)
        {
            try
            {
                var haveToWait = _interval - stopwatch.ElapsedMilliseconds;
                return Convert.ToInt32(haveToWait > 0 ? haveToWait : 0);
            }
            catch (OverflowException ex) { }
            return 0;
        }

        #region IDisposeable interface implementation

        ~ThreadWorker()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources.

                }
                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.
                _runFlag = false;
                _worker = null;
                // Note disposing has been done.
                disposed = true;
            }
        }

        #endregion IDisposeable interface implementation
    }
}
