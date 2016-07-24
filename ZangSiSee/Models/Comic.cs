namespace ZangSiSee.Models
{
    public class Comic : BaseModel
    {
        public string Title { get; }

        public Comic(string title)
        {
            Title = title;
        }
    }
}
