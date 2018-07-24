using QuranMp3TagEditor.Models;

namespace QuranMp3TagEditor.ViewModels
{
    public class MainViewModel
    {
        public MainModel Main { get; set; }

        public MainViewModel()
        {
            Main = new MainModel();
        }
    }
}