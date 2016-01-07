using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Hangups.Views;
using Hangups.Services;
using Windows.UI.Xaml.Controls;

namespace Hangups.ViewModels
{
    public class ViewModelLocator
    {
        private Services.NavigationService _navigationService;
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IDialogService, DialogService>();
            _navigationService = new Services.NavigationService();
            InitNavigation();
            SimpleIoc.Default.Register<INavigationService>(() => _navigationService);
            SimpleIoc.Default.Register<SettingsService>();
            SimpleIoc.Default.Register<NotificationsService>();
            SimpleIoc.Default.Register<HangoutsService>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
        }

        public void SetNavigationFrame(Frame frame)
        {
            _navigationService.SetNavigationFrame(frame);
        }

        private void InitNavigation()
        {
            _navigationService.Configure("Home", typeof(MainPage));
            _navigationService.Configure("Login", typeof(LoginPage));
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public LoginViewModel Login => ServiceLocator.Current.GetInstance<LoginViewModel>();

        public HangoutsService HangoutsService => ServiceLocator.Current.GetInstance<HangoutsService>();
        public SettingsService SettingsService => ServiceLocator.Current.GetInstance<SettingsService>();
        public INavigationService NavigationService => ServiceLocator.Current.GetInstance<INavigationService>();

        public static void Cleanup()
        {

        }
    }
}