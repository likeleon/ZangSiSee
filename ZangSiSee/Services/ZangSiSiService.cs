using AngleSharp;
using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZangSiSee.Models;

namespace ZangSiSee.Services
{
    public class ZangSiSiService
    {
        public static ZangSiSiService Instance => _instance.Value;
        public static string Site { get; } = "http://zangsisi.net/";

        static readonly Lazy<ZangSiSiService> _instance = Exts.Lazy(() => new ZangSiSiService());

        public async Task<Comic[]> GetAllComics()
        {
            var document = await GetDocument(Site);
            var comics = ParseComics(document).ToArray();
            DataManager.Instance.ReplaceAllComics(comics);
            return comics;
        }

        async Task<IDocument> GetDocument(string address)
        {
            var config = Configuration.Default.WithDefaultLoader();
            return await BrowsingContext.New(config).OpenAsync(address).ConfigureAwait(false);
        }

        IEnumerable<Comic> ParseComics(IDocument doc)
        {
            foreach (var a in doc.QuerySelector("#recent-post").QuerySelectorAll("a.tx-link"))
            {
                yield return new Comic()
                {
                    Title = a.TextContent,
                    Url = a.GetAttribute("href"),
                    Concluded = true
                };
            }

            foreach (var a in doc.QuerySelector("#manga-list").QuerySelectorAll("a.lists").Skip(3))
            {
                yield return new Comic()
                {
                    Title = a.TextContent,
                    Url = a.GetAttribute("href")
                };
            }
        }

        public async Task<Book[]> GetBooks(Comic comic)
        {
            var document = await GetDocument(comic.Url);
            var books = ParseBooks(document, comic).ToArray();
            DataManager.Instance.ReplaceBooks(comic, books);
            return books;
        }

        IEnumerable<Book> ParseBooks(IDocument doc, Comic comic)
        {
            var element = doc.QuerySelector("#recent-post") ?? doc.QuerySelector("span.contents");
            if (element == null)
                yield break;

            var elems = element.QuerySelectorAll("a.tx-link");
            if (elems.IsNullOrEmpty())
                elems = element.QuerySelectorAll("a");
            if (elems.IsNullOrEmpty())
                yield break;

            int order = 0;
            foreach (var a in elems)
            {
                yield return new Book()
                {
                    ComicTitle = comic.Title,
                    Title = a.TextContent,
                    Order = order++,
                    Url = a.GetAttribute("href")
                };
            }
        }

        public async Task<Uri[]> GetImages(Book book)
        {
            var document = await GetDocument(book.Url);
            var x = document.QuerySelector("span.contents");
            if (x == null)
                throw new Exception("Nothing found with selector 'span.contents'");

            return x.QuerySelectorAll("img").Select(img => new Uri(img.GetAttribute("src"))).ToArray();
        }
    }
}
