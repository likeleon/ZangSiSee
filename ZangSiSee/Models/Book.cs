using SQLite;
using System;

namespace ZangSiSee.Models
{
    public class Book
    {
        [PrimaryKey]
        public string Title { get; set; }
        public string ComicTitle { get; set; }
        public int Order { get; set; }
        public string Url { get; set; }

        [Ignore]
        public Uri[] ImageUris { get; set; }
    }
}
