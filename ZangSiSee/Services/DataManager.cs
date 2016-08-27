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
            _database.CreateTable<Bookmark>();
        }

        public IEnumerable<Comic> AllComics()
        {
            lock (_lock)
                return _database.Table<Comic>().OrderBy(c => c.Title);
        }

        public Comic GetComic(string title)
        {
            lock (_lock)
                return _database.Find<Comic>(title);
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

        public Book GetBook(string title)
        {
            lock (_lock)
                return _database.Find<Book>(title);
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

        public IEnumerable<Bookmark> AllBookmarks()
        {
            lock (_lock)
                return _database.Table<Bookmark>();
        }

        public Bookmark GetBookmark(Book book, int pageNumber)
        {
            lock (_lock)
                return GetBookmarkInner(book, pageNumber);
        }

        Bookmark GetBookmarkInner(Book book, int pageNumber)
        {
            return _database.Table<Bookmark>().FirstOrDefault(m => m.BookTitle == book.Title && m.PageNumber == pageNumber);
        }

        public Bookmark AddBookmark(Book book, int pageNumber)
        {
            lock (_lock)
            {
                if (GetBookmarkInner(book, pageNumber) != null)
                    throw new InvalidOperationException("Bookmark(book:{}, page:{}) already exist!".F(book.Title, pageNumber));

                var bookmark = new Bookmark() { BookTitle = book.Title, PageNumber = pageNumber };
                _database.Insert(bookmark);
                return bookmark;
            }
        }

        public bool RemoveBookmark(Book book, int pageNumber)
        {
            lock (_lock)
                return _database.Table<Bookmark>().Delete(m => m.BookTitle == book.Title && m.PageNumber == pageNumber) > 0;
        }
    }
}
