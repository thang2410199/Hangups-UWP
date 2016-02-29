using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Hangups.Views;
using Hangups.Services;
using Windows.UI.Xaml.Controls;
using HangupsCore.Services;

namespace Hangups.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IDialogService, DialogService>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public LoginViewModel Login => ServiceLocator.Current.GetInstance<LoginViewModel>();

        //public HangoutsService HangoutsService => ServiceLocator.Current.GetInstance<HangoutsService>();
        public INavigationService NavigationService => ServiceLocator.Current.GetInstance<INavigationService>();

        public static void Cleanup()
        {

        }
    }
}