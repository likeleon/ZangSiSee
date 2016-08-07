using Android.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ZangSiSee.Droid.Gestures;
using ZangSiSee.Droid.Providers;
using ZangSiSee.Renderers;

[assembly: ExportRenderer(typeof(SwipeableImage), typeof(SwipeableImageRenderer))]
namespace ZangSiSee.Droid.Providers
{
    class SwipeableImageRenderer : ImageRenderer
    {
        SwipeableImage SwipeableImage => Element as SwipeableImage;

        readonly SwipeGestureListener _listener = new SwipeGestureListener();
        readonly GestureDetector _detector;

        public SwipeableImageRenderer()
        {
            _detector = new GestureDetector(_listener);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
            {
                GenericMotion -= HandleGenericMotion;
                Touch -= HandleTouch;
                _listener.SwipeLeft -= HandleSwipeLeft;
                _listener.SwipeRight -= HandleSwipeRight;
                _listener.SwipeUp -= HandleSwipeUp;
                _listener.SwipeDown -= HandleSwipeDown;
            }

            if (e.OldElement == null)
            {
                GenericMotion += HandleGenericMotion;
                Touch += HandleTouch;
                _listener.SwipeLeft += HandleSwipeLeft;
                _listener.SwipeRight += HandleSwipeRight;
                _listener.SwipeUp += HandleSwipeUp;
                _listener.SwipeDown += HandleSwipeDown;
            }
        }

        void HandleGenericMotion(object sender, GenericMotionEventArgs e) => _detector.OnTouchEvent(e.Event);
        void HandleTouch(object sender, TouchEventArgs e) => _detector.OnTouchEvent(e.Event);
        void HandleSwipeLeft(object sender, EventArgs e) => SwipeableImage.RaiseSwipeLeft();
        void HandleSwipeRight(object sender, EventArgs e) => SwipeableImage.RaiseSwipeRight();
        void HandleSwipeUp(object sender, EventArgs e) => SwipeableImage.RaiseSwipeUp();
        void HandleSwipeDown(object sender, EventArgs e) => SwipeableImage.RaiseSwipeDown();
    }
}