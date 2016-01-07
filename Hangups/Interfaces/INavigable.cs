using Windows.UI.Xaml.Navigation;

namespace Hangups.Interfaces
{
    public interface INavigable
    {
        void OnNavigatedTo(NavigationEventArgs e);
        void OnNavigatedFrom(NavigationEventArgs e);
        void OnNavigatingFrom(NavigatingCancelEventArgs e);
        bool AllowGoBack();
    }
}
