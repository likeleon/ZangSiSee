using System;
using System.Threading.Tasks;

namespace ZangSiSee.Primitives
{
    public class ThrottledEventHandler<TArgs> where TArgs : EventArgs
    {
        readonly EventHandler<TArgs> _innerHandler;
        readonly EventHandler<TArgs> _outerHandler;
        readonly TimeSpan _dueTime;
        readonly object _lock = new object();

        Task _task;
        Tuple<object, TArgs> _delayed;

        public ThrottledEventHandler(EventHandler<TArgs> handler, TimeSpan dueTime)
        {
            _innerHandler = handler;
            _outerHandler = HandleIncomingEvent;
            _dueTime = dueTime;
        }

        void HandleIncomingEvent(object sender, TArgs args)
        {
            lock (_lock)
            {
                if (_task != null)
                    _delayed = Tuple.Create(sender, args);
                else
                    InvokeInnerHandler(sender, args);
            }
        }

        async void InvokeInnerHandler(object sender, TArgs args)
        {
            _innerHandler?.Invoke(sender, args);

            if (_task != null)
                return;

            await DelayDueTime();
            InvokeDelayed();
        }

        async Task DelayDueTime()
        {
            _task = Task.Delay(_dueTime);
            await _task.ConfigureAwait(false);
        }

        void InvokeDelayed()
        {
            lock (_lock)
            {
                _task = null;
                if (_delayed == null)
                    return;

                InvokeInnerHandler(_delayed.Item1, _delayed.Item2);
                _delayed = null;
            }
        }

        public static implicit operator EventHandler<TArgs>(ThrottledEventHandler<TArgs> h) => h._outerHandler;
    }
}
