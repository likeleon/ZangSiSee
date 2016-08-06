using System;
using System.Linq;
using System.Threading.Tasks;
using ZangSiSee.Models;
using ZangSiSee.Services;

namespace ZangSiSee.ViewModels
{
    public class BookImagesViewModel : BaseViewModel
    {
        public event EventHandler<bool> IsFullScreenChanged;

        public Book Book { get; set; }
        public Uri ImageUri
        {
            get { return _imageUri; }
            private set { SetPropertyChanged(ref _imageUri, value); }
        }
        public bool IsFullScreen
        {
            get { return _isFullScreen; }
            private set
            {
                if (SetPropertyChanged(ref _isFullScreen, value))
                    IsFullScreenChanged?.Invoke(this, value);
            }
        }

        Uri _imageUri;
        bool _isFullScreen;

        public BookImagesViewModel()
        {
            IsFullScreen = true;
        }

        public async Task GetImages()
        {
            if (Book.Images == null)
            {
                using (new Busy(this))
                    Book.Images = await ExceptionSafe(ZangSiSiService.Instance.GetImages(Book));
            }

            ImageUri = Book.Images.FirstOrDefault();
        }
    }
}
