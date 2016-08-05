using System;
using System.Linq;
using System.Threading.Tasks;
using ZangSiSee.Models;
using ZangSiSee.Services;

namespace ZangSiSee.ViewModels
{
    public class BookImagesViewModel : BaseViewModel
    {
        public Book Book { get; set; }
        public Uri ImageUri
        {
            get { return _imageUri; }
            private set { SetPropertyChanged(ref _imageUri, value); }
        }
        public bool IsFullScreen => true;

        Uri _imageUri;

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
