using Hangups.Services;
using Hangups.ViewModels;
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
        }


        public new static App Current { get; private set; }

        public ViewModelLocator Locator { get { return (ViewModelLocator)Resources["Locator"]; } }

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
                Locator.SetNavigationFrame(rootFrame);
                if (Locator.SettingsService.GetValue<string>("refresh_token") != null)
                    Locator.NavigationService.NavigateTo("Home");
                else
                    Locator.NavigationService.NavigateTo("Login");
            }
            Window.Current.Activate();
            KeyboardService keyboard = new KeyboardService();
        }
        
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
