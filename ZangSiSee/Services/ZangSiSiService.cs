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

            var document = await GetDocument();
            var comics = ParseComics(document).ToArray();
            comics.Do(c => DataManager.Instance.Comics.AddOrUpdate(c));
            return comics;
        }

        async Task<IDocument> GetDocument()
        {
            var config = Configuration.Default.WithDefaultLoader();
            return await BrowsingContext.New(config).OpenAsync(Site).ConfigureAwait(false);
        }

        IEnumerable<Comic> ParseComics(IDocument doc)
        {
            foreach (var a in doc.QuerySelector("#recent-post").QuerySelectorAll("a.intro"))
                yield return new Comic(a.TextContent);
            foreach (var a in doc.QuerySelector("#manga-list").QuerySelectorAll("a.lists").Skip(3))
                yield return new Comic(a.TextContent);
        }
    }
}
