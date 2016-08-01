using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Xamarin.Forms;
using ZangSiSee.Interfaces;
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

        public static void ToToast(this string message, ToastNotificationType type = ToastNotificationType.Info, string title = null)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var toaster = DependencyService.Get<IToastNotifier>();
                toaster.Notify(type, title ?? type.ToString().ToUpper(), message, TimeSpan.FromSeconds(2.5f));
            });
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
    }
}
