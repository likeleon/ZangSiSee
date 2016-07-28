using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ZangSiSee.Models;

namespace ZangSiSee
{
    public static class Exts
    {
        public static Lazy<T> Lazy<T>(Func<T> func)
        {
            return new Lazy<T>(func);
        }

        public static void Do<T>(this IEnumerable<T> e, Action<T> a)
        {
            foreach (var ee in e)
                a(ee);
        }

        public static void AddOrUpdate<T>(this ConcurrentDictionary<string, T> dict, T model) where T : BaseModel
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (dict.ContainsKey(model.Id))
            {
                if (!model.Equals(dict[model.Id]))
                    dict[model.Id] = model;
            }
            else
                dict.TryAdd(model.Id, model);
        }
    }
}
