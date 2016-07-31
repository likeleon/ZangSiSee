using System;
using System.Collections.Concurrent;
using ZangSiSee.Models;

namespace ZangSiSee.Services
{
    public class DataManager
    {
        public static DataManager Instance => _instance.Value;

        public ConcurrentDictionary<string, Comic> Comics { get; } = new ConcurrentDictionary<string, Comic>();
        public ConcurrentDictionary<string, Book> Books { get; } = new ConcurrentDictionary<string, Book>();

        static readonly Lazy<DataManager> _instance = Exts.Lazy(() => new DataManager());
    }
}
