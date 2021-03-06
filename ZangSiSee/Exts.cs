﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZangSiSee.Interfaces;
using ZangSiSee.Primitives;

namespace ZangSiSee
{
    public static class Exts
    {
        public static Lazy<T> Lazy<T>(Func<T> func) => new Lazy<T>(func);

        public static void Do<T>(this IEnumerable<T> e, Action<T> a)
        {
            foreach (var ee in e)
                a(ee);
        }

        public static void ToToast(this string message, ToastNotificationType type = ToastNotificationType.Info, string title = null)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var toaster = DependencyService.Get<IToastNotifier>();
                toaster.Notify(type, title ?? type.ToString().ToUpper(), message, TimeSpan.FromSeconds(2.5f));
            });
        }

        public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);

        public static string F(this string format, params object[] args) => string.Format(format, args);

        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0)
                return min;
            else if (val.CompareTo(max) > 0)
                return max;
            else
                return val;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> e) => e == null || !e.Any();

        public static EventHandler<T> Throttle<T>(this EventHandler<T> handler, TimeSpan dueTime) where T : EventArgs 
            => new ThrottledEventHandler<T>(handler, dueTime);

        public static V GetOrAdd<K, V>(this Dictionary<K, V> d, K k) where V : new()
        {
            return GetOrAdd(d, k, _ => new V());
        }

        public static V GetOrAdd<K, V>(this Dictionary<K, V> d, K k, Func<K, V> createFn)
        {
            V ret;
            if (!d.TryGetValue(k, out ret))
                d.Add(k, ret = createFn(k));
            return ret;
        }

        public static string JoinWith(this IEnumerable<string> values, string separator) => string.Join(separator, values);

        public static async Task<byte[]> DownloadAsBytes(this Uri uri)
        {
            using (var httpClient = new HttpClient())
                return await httpClient.GetByteArrayAsync(uri).ConfigureAwait(false);
        }

        public static async void Forget(this Task task)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch
            {
            }
        }
    }
}
