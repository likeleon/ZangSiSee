using System;
using System.Threading.Tasks;
using ZangSiSee.Models;
using System.Linq;

namespace ZangSiSee.Services
{
    public class ZangSiSiService
    {
        public static ZangSiSiService Instance => _instance.Value;

        static readonly Lazy<ZangSiSiService> _instance = Exts.Lazy(() => new ZangSiSiService());

        public Task<Comic[]> GetAllComics()
        {
            return new Task<Comic[]>(() =>
            {
                DataManager.Instance.Comics.Clear();
                var comics = new string[] { "은혼", "오늘부터 우리는" }.Select(x => new Comic(x)).ToArray();
                comics.Do(c => DataManager.Instance.Comics.AddOrUpdate(c));
                return comics;
            });
        }
    }
}
