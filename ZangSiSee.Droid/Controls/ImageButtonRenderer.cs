using Android.Graphics;
using Android.Views;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ZangSiSee.Controls;
using ZangSiSee.Droid.Controls;

[assembly: ExportRenderer(typeof(ImageButton), typeof(ImageButtonRenderer))]
namespace ZangSiSee.Droid.Controls
{
    public class ImageButtonRenderer : ViewRenderer<ImageButton, Android.Widget.ImageButton>
    {
        Bitmap _image;

        protected async override void OnElementChanged(ElementChangedEventArgs<ImageButton> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null && Control == null)
            {
                var button = new Android.Widget.ImageButton(Context);
                button.Touch += OnButtonTouch;
                button.SetBackgroundColor(Android.Graphics.Color.Transparent);
                button.Tag = this;
                SetNativeControl(button);
            }

            await UpdateImage();
        }

        protected async override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == ImageButton.ImageSourceProperty.PropertyName)
            {
                _image = null;
                await UpdateImage();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && Control != null)
                Control.Touch -= OnButtonTouch;
            base.Dispose(disposing);
        }

        void OnButtonTouch(object sender, TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Down)
                Control.Alpha = (float)Element.Opacity / 2;
            else if (e.Event.Action == MotionEventActions.Up)
            {
                Control.Alpha = (float)Element.Opacity;
                if (Element?.Command?.CanExecute(null) == true)
                    Element?.Command?.Execute(null);
            }
        }

        async Task UpdateImage()
        {
            using (var bitmap = await GetBitmapAsync(Element.ImageSource).ConfigureAwait(false))
                Control.SetImageBitmap(bitmap);
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
    }
}
