using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace ZangSiSee.Renderers
{
    public class SwipeableImage : Image
    {
        public static readonly BindableProperty SwipeLeftCommandProperty =
            BindableProperty.Create(nameof(SwipeLeftCommand), typeof(ICommand), typeof(SwipeableImage));
        public static readonly BindableProperty SwipeRightCommandProperty =
            BindableProperty.Create(nameof(SwipeRightCommand), typeof(ICommand), typeof(SwipeableImage));
        public static readonly BindableProperty SwipeUpCommandProperty =
            BindableProperty.Create(nameof(SwipeUpCommand), typeof(ICommand), typeof(SwipeableImage));
        public static readonly BindableProperty SwipeDownCommandProperty =
            BindableProperty.Create(nameof(SwipeDownCommand), typeof(ICommand), typeof(SwipeableImage));

        public ICommand SwipeLeftCommand
        {
            get { return (ICommand)GetValue(SwipeLeftCommandProperty); }
            set { SetValue(SwipeLeftCommandProperty, value); }
        }

        public ICommand SwipeRightCommand
        {
            get { return (ICommand)GetValue(SwipeRightCommandProperty); }
            set { SetValue(SwipeRightCommandProperty, value); }
        }

        public ICommand SwipeUpCommand
        {
            get { return (ICommand)GetValue(SwipeUpCommandProperty); }
            set { SetValue(SwipeUpCommandProperty, value); }
        }

        public ICommand SwipeDownCommand
        {
            get { return (ICommand)GetValue(SwipeDownCommandProperty); }
            set { SetValue(SwipeDownCommandProperty, value); }
        }

        public event EventHandler SwipeLeft;
        public event EventHandler SwipeRight;
        public event EventHandler SwipeUp;
        public event EventHandler SwipeDown;


        public void RaiseSwipeLeft()
        {
            SwipeLeft?.Invoke(this, EventArgs.Empty);
            if (SwipeLeftCommand?.CanExecute(null) == true)
                SwipeLeftCommand.Execute(null);
        }

        public void RaiseSwipeRight()
        {
            SwipeRight?.Invoke(this, EventArgs.Empty);
            if (SwipeRightCommand?.CanExecute(null) == true)
                SwipeRightCommand.Execute(null);
        }

        public void RaiseSwipeUp()
        {
            SwipeUp?.Invoke(this, EventArgs.Empty);
            if (SwipeUpCommand?.CanExecute(null) == true)
                SwipeUpCommand.Execute(null);
        }

        public void RaiseSwipeDown()
        {
            SwipeDown?.Invoke(this, EventArgs.Empty);
            if (SwipeDownCommand?.CanExecute(null) == true)
                SwipeDownCommand.Execute(null);
        }
    }
}
