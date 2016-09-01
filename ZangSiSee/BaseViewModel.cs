using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ZangSiSee
{
    public class BaseViewModel : BaseNotify
    {
        public string Title
        {
            get { return _title; }
            set { SetPropertyChanged(ref _title, value); }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetPropertyChanged(ref _isBusy, value); }
        }

        public CancellationToken CancellationToken => _cancellationTokenSource.Token;

        string _title = string.Empty;
        bool _isBusy;
        CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public async Task RunSafe(Task task, bool notifyOnError = true)
        {
            Exception exception = null;
            try
            {
                await Task.Run(() =>
                {
                    CancellationToken.ThrowIfCancellationRequested();

                    task.Start();
                    task.Wait(CancellationToken);
                }, CancellationToken);
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Task Cancelled");
            }
            catch (AggregateException e)
            {
                var ex = e.InnerException;
                while (ex.InnerException != null)
                    ex = ex.InnerException;

                exception = ex;
            }
            catch (Exception e)
            {
                exception = e;
            }

            if (exception != null)
                HandleException(exception, notifyOnError);
        }

        public void HandleException(Exception exception, bool notify = true)
        {
            Debug.WriteLine(exception);

            if (notify)
                MessagingCenter.Send(this, Messages.ExceptionOccured, exception);
        }

        public virtual void CancelTasks()
        {
            if (_cancellationTokenSource.IsCancellationRequested || !CancellationToken.CanBeCanceled)
                return;

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
        }
    }

    public class Busy : IDisposable
    {
        readonly object _sync = new object();
        readonly BaseViewModel _viewModel;

        public Busy(BaseViewModel viewModel)
        {
            _viewModel = viewModel;
            lock(_sync)
                _viewModel.IsBusy = true;
        }

        public void Dispose()
        {
            lock (_sync)
                _viewModel.IsBusy = false;
        }
    }
}
