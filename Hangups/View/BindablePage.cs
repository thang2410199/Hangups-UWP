using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Hangups.View
{
    public class BindablePage : Page
    {
        public virtual void NavigatedTo(NavigationEventArgs e) { }
        public virtual void NavigatedFrom(NavigationEventArgs e) { }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NavigatedTo(e);
            var navigableViewModel = this.DataContext as INavigable;
            if (navigableViewModel != null)
            {
                navigableViewModel.OnNavigatedTo(e);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            NavigatedFrom(e);
            var navigableViewModel = this.DataContext as INavigable;
            if (navigableViewModel != null)
                navigableViewModel.OnNavigatedFrom(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);

            var navigableViewModel = this.DataContext as INavigable;
            if (navigableViewModel != null)
                navigableViewModel.OnNavigatingFrom(e);
        }

    }

    public interface INavigable
    {
        void OnNavigatedTo(NavigationEventArgs e);
        void OnNavigatedFrom(NavigationEventArgs e);
        void OnNavigatingFrom(NavigatingCancelEventArgs e);
        bool AllowGoBack();
    }
}
