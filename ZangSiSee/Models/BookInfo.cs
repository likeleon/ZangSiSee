using SQLite;

namespace ZangSiSee.Models
{
    public class BookInfo
    {
        [PrimaryKey]
        public string BookTitle { get; set; }
        public byte[] CoverImage { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Translator { get; set; }
    }
}
