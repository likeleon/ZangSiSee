// from https://github.com/kyurkchyan/ImageButton

using System.Windows.Input;
using Xamarin.Forms;

namespace ZangSiSee.Controls
{
    public class ImageButton : View
    {
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ImageButton));

        public static readonly BindableProperty ImageSourceProperty =
            BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(ImageButton));

        public static readonly BindableProperty PaddingProperty =
            BindableProperty.Create(nameof(Padding), typeof(int), typeof(ImageButton), 10);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        [TypeConverter(typeof(ImageSourceConverter))]
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public int Padding
        {
            get { return (int)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }
    }
}
