using Xamarin.Forms;

namespace ZangSiSee.Controls
{
    public class ImageButton : Button
    {
        public static readonly BindableProperty SourceProperty =
            BindableProperty.Create(nameof(Source), typeof(ImageSource), typeof(ImageButton));

        public static readonly BindableProperty PaddingProperty =
            BindableProperty.Create(nameof(Padding), typeof(int), typeof(ImageButton));

        [TypeConverter(typeof(ImageSourceConverter))]
        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public int Padding
        {
            get { return (int)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }
    }
}
