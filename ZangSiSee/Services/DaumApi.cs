using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using ZangSiSee.Models;

namespace ZangSiSee.Services
{
    public class DaumApi
    {
        public static DaumApi Instance => _instance.Value;
        static Uri ServiceUri => new Uri("https://apis.daum.net/search/book");

        public string ApiKey { get; set; }

        static readonly Lazy<DaumApi> _instance = Exts.Lazy(() => new DaumApi());

        public async Task<BookInfo> GetBookInfo(Comic comic, int volume)
        {
            CheckApiKeySet();
            var keyword = MakeSearchKeyword(comic.Title, volume);
            return await GetBookInfoInternal(keyword, comic.Title);
        }

        public async Task<BookInfo> GetBookInfo(Book book)
        {
            CheckApiKeySet();
            var keyword = MakeSearchKeyword(book.ComicTitle, GuessVolume(book.Title)) ?? book.Title;
            return await GetBookInfoInternal(keyword, book.ComicTitle, book);
        }

        string MakeSearchKeyword(string comicTitle, int? volume)
        {
            if (volume == null)
                return null;

            return "{0}. {1}".F(comicTitle, volume);
        }

        async Task<BookInfo> GetBookInfoInternal(string searchKeyword, string comicTitle, Book book = null)
        {
            var response = await GetAsync(MakeUriSearching(searchKeyword));
            var items = response.Elements("item");
            if (items.IsNullOrEmpty())
                return null;

            var item = await FirstItemContainsCoverImage(items);
            return new BookInfo()
            {
                BookTitle = book?.Title ?? GetResponseField(item.Item1, "title"),
                ComicTitle = comicTitle,
                CoverImage = item.Item2,
                Description = GetResponseField(item.Item1, "description"),
                Author = GetResponseField(item.Item1, "author"),
                Translator = GetResponseField(item.Item1, "translator")
            };
        }

        async Task<Tuple<XElement, byte[]>> FirstItemContainsCoverImage(IEnumerable<XElement> items)
        {
            foreach (var item in items)
            {
                var coverImage = await DownloadCoverImage(item);
                if (coverImage != null)
                    return Tuple.Create(item, coverImage);
            }
            return Tuple.Create<XElement, byte[]>(items.First(), null);
        }

        async Task<byte[]> DownloadCoverImage(XElement e)
        {
            try
            {
                var imageUrl = GetResponseField(e, "cover_l_url") ?? GetResponseField(e, "cover_s_url");
                if (imageUrl.IsNullOrEmpty())
                    return null;

                return await new Uri(imageUrl).DownloadAsBytes();
            }
            catch
            {
                return null;
            }
        }

        int? GuessVolume(string bookTitle)
        {
            try
            {
                var match = Regex.Match(bookTitle, @"[^\d]*(\d+)\s*권");
                if (!match.Success)
                    return null;

                return int.Parse(match.Groups[1].Value);
            }
            catch
            {
                return null;
            }
        }

        string MakeUriSearching(string keyword)
        {
            var args = new Dictionary<string, string>
            {
                { "apikey", ApiKey },
                { "q", keyword },
                { "result", "20" },
                { "sort", "accu" },
                { "searchType", "title" },
                { "output", "xml" }
            };
            return new UriBuilder(ServiceUri)
            {
                Query = args.Select(x => "{0}={1}".F(WebUtility.UrlDecode(x.Key), Uri.EscapeDataString(x.Value))).JoinWith("&")
            }.ToString();
        }

        async Task<XElement> GetAsync(string uri)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                return XElement.Load(await response.Content.ReadAsStreamAsync());
            }
        }

        void CheckApiKeySet()
        {
            if (ApiKey == null)
                throw new InvalidOperationException("{0} should be set".F(nameof(ApiKey)));
        }

        string GetResponseField(XElement e, string childName)
        {
            return Uri.UnescapeDataString(e.Element(childName).Value);
        }
    }

    public class DaumBook
    {
        public string Title { get; set; }
        public Uri Link { get; set; }
        public Uri SmallCover { get; set; }
        public Uri LargeCover { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Translator { get; set; }
    }
}
