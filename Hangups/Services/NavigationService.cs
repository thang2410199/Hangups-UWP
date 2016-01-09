using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace Hangups.Services
{
    public class NavigationService : INavigationService
    {
        public const string RootPageKey = "-- ROOT --";
        public const string UnknownPageKey = "-- UNKNOWN --";
        private Frame _navigationFrame;

        private readonly Dictionary<string, Type> _pagesByKey = new Dictionary<string, Type>();

        public string CurrentPageKey
        {
            get
            {
                lock (_pagesByKey)
                {
                    if (_navigationFrame.BackStackDepth == 0)
                        return RootPageKey;

                    if (_navigationFrame.Content == null)
                        return UnknownPageKey;

                    var currentType = _navigationFrame.Content.GetType();
                    if (_pagesByKey.All(p => p.Value != currentType))
                        return UnknownPageKey;

                    var item = _pagesByKey.FirstOrDefault(i => i.Value == currentType);

                    return item.Key;
                }
            }
        }

        public void GoBack()
        {
            if (_navigationFrame.CanGoBack)
                _navigationFrame.GoBack();
        }

        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, null);
        }
        public virtual void NavigateTo(string pageKey, object parameter)
        {
            lock (_pagesByKey)
            {
                if (!_pagesByKey.ContainsKey(pageKey))
                {
                    throw new ArgumentException(
                        string.Format(
                            "No such page: {0}. Did you forget to call NavigationService.Configure?",
                            pageKey),
                        "pageKey");
                }

                _navigationFrame.Navigate(_pagesByKey[pageKey], parameter);
            }
        }

        public void Configure(string key, Type pageType)
        {
            lock (_pagesByKey)
            {
                if (_pagesByKey.ContainsKey(key))
                    throw new ArgumentException("This key is already used: " + key);

                if (_pagesByKey.Any(p => p.Value == pageType))
                    throw new ArgumentException(
                        "This type is already configured with key " + _pagesByKey.First(p => p.Value == pageType).Key);

                _pagesByKey.Add(key, pageType);
            }
        }

        public void SetNavigationFrame(Frame frame)
        {
            _navigationFrame = frame;
        }
    }
}
