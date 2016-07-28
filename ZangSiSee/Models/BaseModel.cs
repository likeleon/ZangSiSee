namespace ZangSiSee.Models
{
    public abstract class BaseModel : BaseNotify, IDirty
    {
        public bool IsDirty { get; set; }
        public abstract string Id { get; }
    }
}
