using SQLite;

namespace ZangSiSee.Models
{
    public class Comic
    {
        [PrimaryKey]
        public string Title { get; set; }
        public string Url { get; set; }
        public bool Concluded { get; set; }
    }
}
