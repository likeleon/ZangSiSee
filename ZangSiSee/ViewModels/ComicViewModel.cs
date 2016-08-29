using ZangSiSee.Models;

namespace ZangSiSee.ViewModels
{
    public class ComicViewModel : BaseViewModel
    {
        public Comic Comic { get; set; }
        public BookInfo Info
        {
            get { return _info; }
            set { SetPropertyChanged(ref _info, value); }
        }

        BookInfo _info;

        public ComicViewModel(Comic comic)
        {
            Comic = comic;
        }
    }
}
