using System;
using GalaSoft.MvvmLight;
using Hangups.View;
using Windows.UI.Xaml.Navigation;

namespace Hangups.ViewModel
{
    public class MainViewModel : ViewModelBase, INavigable
    {
        public bool AllowGoBack()
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationEventArgs e)
        {
            
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            
        }
    }
}