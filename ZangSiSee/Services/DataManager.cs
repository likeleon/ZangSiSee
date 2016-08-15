using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using ZangSiSee.Interfaces;
using ZangSiSee.Models;

namespace ZangSiSee.Services
{
    public class DataManager
    {
        public static DataManager Instance => _instance.Value;

        static readonly Lazy<DataManager> _instance = Exts.Lazy(() => new DataManager());

        const string _localDbName = "zangsisee.db3";
        readonly SQLiteConnection _database;
        readonly object _lock = new object();

        DataManager()
        {
            _database = DependencyService.Get<ISQLite>().GetConnection(_localDbName);
            _database.CreateTable<Comic>();
            _database.CreateTable<Book>();
        }

        public IEnumerable<Comic> AllComics()
        {
            lock (_lock)
            {
                var counts = _database.Table<Comic>().Count();
                return _database.Table<Comic>().OrderBy(c => c.Title);
            }
        }

        public Comic GetComic(string id)
        {
            lock (_lock)
                return _database.Find<Comic>(id);
        }

        public void ReplaceAllComics(IEnumerable<Comic> comics)
        {
            lock (_lock)
            {
                _database.DropTable<Comic>();
                _database.CreateTable<Comic>();
                _database.InsertAll(comics);
            }
        }

        public IEnumerable<Book> GetBooks(Comic comic)
        {
            lock (_lock)
                return _database.Table<Book>().Where(b => b.ComicTitle == comic.Title);
        }
        
        public void ReplaceBooks(Comic comic, IEnumerable<Book> books)
        {
            if (books.Any(b => b.ComicTitle != comic.Title))
                throw new ArgumentException("Different comic title found", nameof(books));

            lock (_lock)
            {
                _database.Table<Book>().Delete(b => b.ComicTitle == comic.Title);
                _database.InsertAll(books);
            }
        }
    }
}
