using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace MyCSharpLibrary.Threading
{
    public class ThreadWorker : IDisposable
    {
        private bool _doWorkFlag;
        private int _interval = 1000;
        private DateTime _nextCanRunTime = DateTime.Now;
        private bool _runFlag;
        private Action _task;
        private Thread _worker = null;

        public ThreadWorker(Action task, int interval = 1000, bool startFlag = true)
        {
            Initial();

            _task = task;
            _interval = interval;
            _doWorkFlag = startFlag;
        }

        public int Interval
        {
            get => _interval;
            set => _interval = value < 10 ? 10 : value;
        }

        public bool IsRunning => _doWorkFlag;

        public void Pause()
        {
            _doWorkFlag = false;
        }

        public void Start()
        {
            _doWorkFlag = true;
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
                try
                {
                    if (_doWorkFlag && DateTime.Now > _nextCanRunTime)
                    {
                        try
                        {
                            _nextCanRunTime = DateTime.Now.AddMilliseconds(_interval);
                            _task?.Invoke();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"{ex.Message}-{ex.StackTrace}");
                        }
                    }
                    else if (_nextCanRunTime > DateTime.Now.AddMilliseconds(_interval))
                    {
                        _nextCanRunTime = DateTime.Now;
                        continue;
                    }

                    SpinWait.SpinUntil(() => false, 10);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"{ex.Message}-{ex.StackTrace}");
                }
            }
        }

        #region IDisposeable interface implementation

        private bool disposed = false;

        ~ThreadWorker()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

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
