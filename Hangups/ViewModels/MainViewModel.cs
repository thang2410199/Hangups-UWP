using System;
using GalaSoft.MvvmLight;
using Hangups.Views;
using Windows.UI.Xaml.Navigation;
//using Hangups.Core;
using Hangups.Interfaces;
using Hangups.Services;
using HangupsCore.Services;

namespace Hangups.ViewModels
{
    public class MainViewModel : ViewModelBase, INavigable
    {
        private HangoutsService _hangoutsService;
        public MainViewModel(HangoutsService hangoutsService)
        {
            _hangoutsService = hangoutsService;
        }

        public bool AllowGoBack()
        {
            return false;
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