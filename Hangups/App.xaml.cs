using GalaSoft.MvvmLight.Views;
using Hangups.Services;
using Hangups.ViewModels;
using Hangups.Views;
using HangupsCore;
using HangupsCore.Helpers;
using HangupsCore.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Hangups
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            Current = this;

            Manager = new Manager();
            Manager.SettingsService = new SettingsService(Manager);
            Manager.LogService = new LogService(Manager);
            Manager.HangoutsService = new HangoutsService(Manager, Manager.SettingsService);

            //TODO: Create UI Manager for centrelize NavigationService and NotificationsService
            NavigationService = new Services.NavigationService();

            //TODO: mplement camera service for photo/video sharing
            //Manager.CameraService = new BasicCameraService(Manager);
        }


        public new static App Current { get; private set; }        

        public Services.NavigationService NavigationService { get; set; }
        public NotificationsService NotificationService { get; set; } = new NotificationsService();

        public Manager Manager { get; set; }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;
                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Restore navigation stack
                }
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                NavigationService.SetNavigationFrame(rootFrame);
                NavigationServiceInit();
                if (Manager.SettingsService.GetValueRoaming<string>("refresh_token") != null)
                    NavigationService.NavigateTo("Home");
                else
                    NavigationService.NavigateTo("Login");
            }
            Window.Current.Activate();

            Manager.Configure();
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }


        public void NavigationServiceInit()
        {
            NavigationService.Configure("Home", typeof(MainPage));
            NavigationService.Configure("Login", typeof(LoginPage));
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
