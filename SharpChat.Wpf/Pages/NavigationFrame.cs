using System.Windows.Controls;

namespace SharpChat.Wpf.Pages
{
    public class NavigationFrame : Frame
    {
        public NavigationFrame()
        {
            JournalOwnership = System.Windows.Navigation.JournalOwnership.OwnsJournal;
            NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }
    }
}
