using SQLite;

namespace ZangSiSee.Models
{
    public class Bookmark
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string BookTitle { get; set; }
        public int PageNumber { get; set; }
    }
}
