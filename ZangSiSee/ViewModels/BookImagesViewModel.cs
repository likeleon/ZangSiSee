﻿using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using ZangSiSee.Interfaces;
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
            private set
            {
                if (SetPropertyChanged(ref _pageNumber, value))
                    SliderPageNumber = value;
            }
        }

        public int SliderPageNumber
        {
            get { return _slidingPageNumber; }
            set { SetPropertyChanged(ref _slidingPageNumber, value); }
        }

        public int MinPageNumber { get; } = 1;
        public int MaxPageNumber => Book.ImageUris?.Length ?? MinPageNumber + 1;

        public bool IsFullScreen
        {
            get { return _isFullScreen; }
            set
            {
                if (SetPropertyChanged(ref _isFullScreen, value))
                    IsFullScreenChanged?.Invoke(this, value);
            }
        }

        public ICommand NextImageCommand => new Command(async _ => await ShowPage(PageNumber + 1));
        public ICommand PrevImageCommand => new Command(async _ => await ShowPage(PageNumber - 1));
        public ICommand AddBookmarkCommand => new Command(_ => AddBookmark());
        public ICommand EnterFullScreenCommand => new Command(_ => IsFullScreen = true);
        public ICommand ExitFullScreenCommand => new Command(_ => IsFullScreen = false);

        readonly ConcurrentDictionary<Uri, byte[]> _imageCaches = new ConcurrentDictionary<Uri, byte[]>();
        ImageSource _image;
        int _pageNumber = 1; // starts from 1
        int _slidingPageNumber;
        bool _isFullScreen;

        public BookImagesViewModel()
        {
            SliderPageNumber = _pageNumber;
        }

        public async Task Initialize(int startingPage)
        {
            await ShowPage(startingPage, true);
        }

        public async Task ShowPage(int pageNumber, bool force = false)
        {
            if (!await EnsureBookImageUrisGot())
                return;

            int clampedPage = pageNumber.Clamp(1, MaxPageNumber);
            if (!force && PageNumber == clampedPage)
                return;

            PageNumber = clampedPage;
            Image = GetImageSource(Book.ImageUris[PageNumber - 1]);

            var urisToCache = Book.ImageUris
                .Skip(PageNumber)
                .Take(3)
                .Where(uri => !_imageCaches.ContainsKey(uri)).ToArray();
            if (urisToCache.Length > 0)
                await RunSafe(CacheImages(urisToCache)).ConfigureAwait(false);
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
                    SetPropertyChanged(nameof(MaxPageNumber));
                    return true;
                }
                catch (Exception e)
                {
                    "이미지 목록을 가져오지 못했습니다.".ToToast(ToastNotificationType.Warning);
                    HandleException(e, false);
                    return false;
                }
            }
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
                        _imageCaches.TryUpdate(uri, await uri.DownloadAsBytes(), null);
                    }
                    catch (Exception ex)
                    {
                        HandleException(ex, false);
                        byte[] bytes;
                        _imageCaches.TryRemove(uri, out bytes);
                    }
                }
            });
        }

        public void AddBookmark()
        {
            if (DataManager.Instance.GetBookmark(Book, PageNumber) != null)
            {
                "이미 북마크되어 있습니다.".ToToast(ToastNotificationType.Info, "북마크");
                return;
            }

            try
            {
                DataManager.Instance.AddBookmark(Book, PageNumber);
                "북마크가 추가되었습니다.".ToToast(ToastNotificationType.Success, "북마크");
            }
            catch (Exception e)
            {
                ("북마크를 추가하지 못했습니다. " + e.Message).ToToast(ToastNotificationType.Error, "북마크");
                HandleException(e, false);
            }
        }
    }
}
