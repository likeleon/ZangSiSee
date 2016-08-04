using System;
using System.Collections.Generic;

namespace ZangSiSee.Models
{
    public class Book : BaseModel
    {
        public Comic Comic { get; }
        public string Title { get; }
        public int Order { get; }
        public string Url { get; }
        public Uri[] Images { get; set; }
        public override string Id => Title;

        public Book(Comic comic, string title, int order, string url)
        {
            Comic = comic;
            Title = title;
            Order = order;
            Url = url;
        }

        public override bool Equals(object obj)
        {
            return new BookComparer().Equals(this, obj as Book);
        }

        public override int GetHashCode()
        {
            return new BookComparer().GetHashCode(this);
        }
    }

    public class BookComparer : IEqualityComparer<Book>
    {
        public bool Equals(Book x, Book y)
        {
            if (x == null || y == null)
                return false;

            return x.Comic == y.Comic && x.Id == y.Id;
        }

        public int GetHashCode(Book obj)
        {
            return obj.Id != null ? obj.Id.GetHashCode() : base.GetHashCode();
        }
    }
}
