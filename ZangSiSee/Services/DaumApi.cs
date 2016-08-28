using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        public async Task<BookInfo> GetBookInfo(string bookTitle, int volume)
        {
            CheckApiKeySet();

            var response = await GetAsync(MakeUriSearching(bookTitle, volume));
            var item = response.Element("item");
            if (item == null)
                return null;

            return new BookInfo()
            {
                BookTitle = bookTitle,
                CoverImage = await new Uri(GetResponseField(item, "cover_l_url")).DownloadAsBytes(),
                Description = GetResponseField(item, "description"),
                Author = GetResponseField(item, "author"),
                Translator = GetResponseField(item, "translator")
            };
        }

        string MakeUriSearching(string bookTitle, int volume)
        {
            var args = new Dictionary<string, string>
            {
                { "apikey", ApiKey },
                { "q", "{0}. {1}".F(bookTitle, volume) },
                { "result", "1" },
                { "sort", "accu" },
                { "searchType", "title" },
                { "cate_id", "47" },
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
