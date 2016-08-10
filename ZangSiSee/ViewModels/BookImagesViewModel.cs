using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net.Http;
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

        public ICommand NextImageCommand => new Command(async _ => await ShowPage(PageNumber + 1));
        public ICommand PrevImageCommand => new Command(async _ => await ShowPage(PageNumber - 1));

        readonly ConcurrentDictionary<Uri, byte[]> _imageCaches = new ConcurrentDictionary<Uri, byte[]>();
        readonly HttpClient _httpClient = new HttpClient();
        ImageSource _image;
        int _pageNumber;
        bool _isFullScreen;

        public BookImagesViewModel()
        {
            IsFullScreen = true;
        }

        public async Task GetImages()
        {
            if (!await EnsureBookImageUrisGot())
                return;

            await ShowPage(PageNumber, true).ConfigureAwait(false);
        }

        async Task<bool> EnsureBookImageUrisGot()
        {
            if (Book.ImageUris != null)
                return true;

            using (new Busy(this))
            {
                try
                {
                    Book.ImageUris = await ZangSiSiService.Instance.GetImages(Book);
                    return true;
                }
                catch (Exception e)
                {
                    e.Message.ToToast(Interfaces.ToastNotificationType.Warning, "이미지 리스트 획득 실패");
                    HandleException(e);
                    return false;
                }
            }
        }

        async Task ShowPage(int pageNumber, bool force = false)
        {
            if (Book.ImageUris.IsNullOrEmpty())
                return;

            int clampedPage = pageNumber.Clamp(0, Book.ImageUris.Length);
            if (!force && PageNumber == clampedPage)
                return;

            PageNumber = clampedPage;
            Image = GetImageSource(Book.ImageUris[PageNumber]);

            var urisToCache = Book.ImageUris
                .Skip(PageNumber + 1)
                .Take(3)
                .Where(uri => !_imageCaches.ContainsKey(uri)).ToArray();
            if (urisToCache.Length > 0)
                await RunSafe(CacheImages(urisToCache)).ConfigureAwait(false);
        }

        ImageSource GetImageSource(Uri uri)
        {
            byte[] bytes;
            if (_imageCaches.TryGetValue(uri, out bytes) && bytes != null)
                return ImageSource.FromStream(() => new MemoryStream(bytes));

            return ImageSource.FromUri(uri);
        }

        Task CacheImages(Uri[] uris)
        {
            return new Task(async () =>
            {
                if (uris.Length <= 0)
                    throw new ArgumentException("uri list should not be empty", nameof(uris));

                foreach (var uri in uris)
                {
                    if (!_imageCaches.TryAdd(uri, null))
                        continue;

                    try
                    {
                        var bytes = await _httpClient.GetByteArrayAsync(uri).ConfigureAwait(false);
                        _imageCaches.TryUpdate(uri, bytes, null);
                    }
                    catch
                    {
                        byte[] bytes;
                        _imageCaches.TryRemove(uri, out bytes);
                    }
                }
            });
        }
    }
}
