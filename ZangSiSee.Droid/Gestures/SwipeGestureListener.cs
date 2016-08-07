using Android.Views;
using System;

namespace ZangSiSee.Droid.Gestures
{
    class SwipeGestureListener : GestureDetector.SimpleOnGestureListener
    {
        public event EventHandler SwipeLeft;
        public event EventHandler SwipeRight;
        public event EventHandler SwipeUp;
        public event EventHandler SwipeDown;

        static readonly int SwipeThreshold = 100;
        static readonly int SwipeVelocityThreshold = 100;

        public override bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            var diffX = e2.GetX() - e1.GetX();
            var diffY = e2.GetY() - e1.GetY();

            if (Math.Abs(diffX) > Math.Abs(diffY))
                TryRaiseLeftOrRight(diffX, velocityX);
            else
                TryRaiseUpOrDown(diffY, velocityY);

            return base.OnFling(e1, e2, velocityX, velocityY);
        }

        void TryRaiseLeftOrRight(float diff, float velocity)
        {
            if (Math.Abs(diff) <= SwipeThreshold || Math.Abs(velocity) <= SwipeVelocityThreshold)
                return;

            if (diff > 0)
                SwipeRight?.Invoke(this, EventArgs.Empty);
            else
                SwipeLeft?.Invoke(this, EventArgs.Empty);
        }

        void TryRaiseUpOrDown(float diff, float velocity)
        {
            if (Math.Abs(diff) <= SwipeThreshold || Math.Abs(velocity) <= SwipeVelocityThreshold)
                return;

            if (diff > 0)
                SwipeDown?.Invoke(this, EventArgs.Empty);
            else
                SwipeUp?.Invoke(this, EventArgs.Empty);
        }
    }
}
