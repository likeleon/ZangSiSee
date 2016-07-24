namespace ZangSiSee.Models
{
    public class BaseModel : BaseNotify, IDirty
    {
        public bool IsDirty { get; set; }
    }
}
