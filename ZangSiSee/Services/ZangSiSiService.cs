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
            DataManager.Instance.Comics.Clear();

            var document = await GetDocument(Site);
            var comics = ParseComics(document).ToArray();
            comics.Do(c => DataManager.Instance.Comics.AddOrUpdate(c));
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
                yield return new Comic(a.TextContent, a.GetAttribute("href"), true);
            foreach (var a in doc.QuerySelector("#manga-list").QuerySelectorAll("a.lists").Skip(3))
                yield return new Comic(a.TextContent, a.GetAttribute("href"), false);
        }

        public async Task<Book[]> GetBooks(Comic comic)
        {
            DataManager.Instance.Books.Clear();

            var document = await GetDocument(comic.Url);
            var books = ParseBooks(document, comic).ToArray();
            books.Do(b => DataManager.Instance.Books.AddOrUpdate(b));
            return books;
        }

        IEnumerable<Book> ParseBooks(IDocument doc, Comic comic)
        {
            var element = doc.QuerySelector("#recent-post") ?? doc.QuerySelector("span.contents");
            if (element == null)
                yield break;

            int order = 0;
            foreach (var a in element.QuerySelectorAll("a"))
                yield return new Book(comic, a.TextContent, order++, a.GetAttribute("href"));
        }

        public async Task<Uri[]> GetImages(Book book)
        {
            var document = await GetDocument(book.Url);
            return document.QuerySelector("span.contents").QuerySelectorAll("img")
                .Select(img => new Uri(img.GetAttribute("src"))).ToArray();
        }
    }
}
