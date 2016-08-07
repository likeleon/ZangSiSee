using System;
using Xamarin.Forms;

namespace ZangSiSee.Renderers
{
    public class SwipeableImage : Image
    {
        public event EventHandler SwipeLeft;
        public event EventHandler SwipeRight;
        public event EventHandler SwipeUp;
        public event EventHandler SwipeDown;

        public void RaiseSwipeLeft() => SwipeLeft?.Invoke(this, EventArgs.Empty);
        public void RaiseSwipeRight() => SwipeRight?.Invoke(this, EventArgs.Empty);
        public void RaiseSwipeUp() => SwipeUp?.Invoke(this, EventArgs.Empty);
        public void RaiseSwipeDown() => SwipeDown?.Invoke(this, EventArgs.Empty);
    }
}
