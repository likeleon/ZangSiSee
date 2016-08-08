using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using ZangSiSee.Models;
using ZangSiSee.Services;

namespace ZangSiSee.ViewModels
{
    public class BookImagesViewModel : BaseViewModel
    {
        public event EventHandler<bool> IsFullScreenChanged;

        public Book Book { get; set; }        

        public ImageSource Image
        {
            get { return _image; }
            private set { SetPropertyChanged(ref _image, value); }
        }

        public int PageNumber
        {
            get { return _pageNumber; }
            private set { SetPropertyChanged(ref _pageNumber, value); }
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

        public ICommand NextImageCommand => new Command(_ => ShowPage(PageNumber + 1));
        public ICommand PrevImageCommand => new Command(_ => ShowPage(PageNumber - 1));

        readonly ConcurrentDictionary<Uri, ImageSource> _imageSources = new ConcurrentDictionary<Uri, ImageSource>();
        ImageSource _image;
        int _pageNumber;
        bool _isFullScreen;

        public BookImagesViewModel()
        {
            IsFullScreen = true;
        }

        public async Task GetImages()
        {
            if (Book.ImageUris == null)
            {
                using (new Busy(this))
                    Book.ImageUris = await ExceptionSafe(ZangSiSiService.Instance.GetImages(Book));
            }

            ShowPage(PageNumber, true);

            await RunSafe(LoadImages()).ConfigureAwait(false);
        }

        void ShowPage(int pageNumber, bool force = false)
        {
            if (Book.ImageUris.IsNullOrEmpty())
                return;

            if (!force && PageNumber == pageNumber)
                return;

            PageNumber = pageNumber;
            var uri = Book.ImageUris[pageNumber.Clamp(0, Book.ImageUris.Length)];
            Image = GetImageSource(uri);
        }

        ImageSource GetImageSource(Uri uri)
        {
            ImageSource imageSource;
            if (_imageSources.TryGetValue(uri, out imageSource))
                return imageSource;

            return ImageSource.FromUri(uri);
        }

        Task LoadImages()
        {
            return new Task(() =>
            {
                if (Book == null)
                    throw new InvalidOperationException("Book should not null");

                foreach (var uri in Book.ImageUris)
                    _imageSources.TryAdd(uri, ImageSource.FromUri(uri));
            });
        }
    }
}
