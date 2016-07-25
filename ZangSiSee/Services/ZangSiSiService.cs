using System;
using System.Threading.Tasks;

namespace ZangSiSee.Services
{
    public class ZangSiSiService
    {
        public static ZangSiSiService Instance => _instance.Value;

        static readonly Lazy<ZangSiSiService> _instance = Exts.Lazy(() => new ZangSiSiService());

        public Task GetAllComics()
        {
            return new Task(() =>
            {
            });
        }
    }
}
