using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ZangSiSee.Controls;
using View = Android.Views.View;

namespace ZangSiSee.Droid.Controls
{
    public class ImageButtonRenderer : Xamarin.Forms.Platform.Android.AppCompat.ButtonRenderer
    {
        ImageButton ImageButton => (ImageButton)Element;

        static float _density = float.MinValue;

        protected async override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            _density = Resources.DisplayMetrics.Density;

            var targetButton = Control;
            targetButton?.SetOnTouchListener(TouchListener.Instance);

            if (Element != null && ImageButton.Source != null)
                await SetImageSourceAsync(targetButton, ImageButton).ConfigureAwait(false);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
                Control?.Dispose();
        }

        async Task SetImageSourceAsync(Android.Widget.Button targetButton, ImageButton model)
        {
            if (targetButton == null || targetButton.Handle == IntPtr.Zero || model == null)
                return;

            using (var bitmap = await GetBitmapAsync(model.Source).ConfigureAwait(false))
            {
                if (bitmap == null)
                    targetButton.SetCompoundDrawables(null, null, null, null);
                else
                {
                    var drawable = new BitmapDrawable(bitmap);
                    using (var scaledDrawable = GetScaleDrawable(drawable, GetWidth(model.ImageWidthRequest), GetHeight(model.ImageHeightRequest)))
                    {
                        targetButton.CompoundDrawablePadding = RequestToPixels(model.Padding);
                        targetButton.Gravity = GravityFlags.Center;
                        targetButton.SetCompoundDrawables(null, scaledDrawable, null, null);
                    }
                }
            }
        }

        async Task<Bitmap> GetBitmapAsync(ImageSource source)
        {
            var handler = GetHandler(source);
            var returnValue = (Bitmap)null;

            if (handler != null)
                returnValue = await handler.LoadImageAsync(source, Context).ConfigureAwait(false);

            return returnValue;
        }

        static IImageSourceHandler GetHandler(ImageSource source)
        {
            if (source is UriImageSource)
                return new ImageLoaderSourceHandler();
            else if (source is FileImageSource)
                return new FileImageSourceHandler();
            else if (source is StreamImageSource)
                return new StreamImagesourceHandler();
            else
                return null;
        }

        Drawable GetScaleDrawable(Drawable drawable, int width, int height)
        {
            var returnValue = new ScaleDrawable(drawable, 0, 100, 100).Drawable;
            returnValue.SetBounds(0, 0, RequestToPixels(width), RequestToPixels(height));
            return returnValue;
        }

        int RequestToPixels(int sizeRequest)
        {
            if (_density == float.MinValue)
            {
                if (Resources.Handle == IntPtr.Zero || Resources.DisplayMetrics.Handle == IntPtr.Zero)
                    _density = 1.0f;
                else
                    _density = Resources.DisplayMetrics.Density;
            }
            return (int)(sizeRequest * _density);
        }

        int GetWidth(int requestetWidth) => requestetWidth <= 0 ? 50 : requestetWidth;
        int GetHeight(int requestedHeight) => requestedHeight <= 0 ? 50 : requestedHeight;
    }

    class TouchListener : Java.Lang.Object, View.IOnTouchListener
    {
        public static TouchListener Instance => _instance.Value;
        static readonly Lazy<TouchListener> _instance = Exts.Lazy(() => new TouchListener());

        TouchListener() { }

        public bool OnTouch(View v, MotionEvent e)
        {
            var buttonRenderer = v.Tag as ButtonRenderer;
            if (buttonRenderer != null && e.Action == MotionEventActions.Down)
                buttonRenderer.Control.Text = buttonRenderer.Element.Text;
            return false;
        }
    }
}
