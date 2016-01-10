using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Hangups.Interfaces;
using Hangups.Services;
using HangupsCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace Hangups.ViewModels
{
    public class LoginViewModel : ViewModelBase, INavigable
    {
        private HangoutsService _hangouts;
        private INavigationService _navgation;
        public LoginViewModel(HangoutsService hangouts, INavigationService navigation)
        {
            _hangouts = hangouts;
            _navgation = navigation;
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

        private RelayCommand _loginCommand;
        public RelayCommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                    _loginCommand = new RelayCommand(async () =>
                    {
                        await _hangouts.Login();
                        _navgation.NavigateTo("Home");
                    });

                return _loginCommand;
            }
        }
    }
}
