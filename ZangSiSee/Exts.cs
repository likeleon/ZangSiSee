using System;

namespace ZangSiSee
{
    public static class Exts
    {
        public static Lazy<T> Lazy<T>(Func<T> func)
        {
            return new Lazy<T>(func);
        }
    }
}
