using NotificationsExtensions.Toasts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace Hangups.Services
{
    public class NotificationsService
    {
        ToastNotifier _notifier;
        public NotificationsService()
        {
            _notifier = ToastNotificationManager.CreateToastNotifier();
        }


        #region Public methods
        public void ShowErrorNotification(string title, string message)
        {
            ShowNotification(new ToastContent
            {
                Visual = new ToastVisual
                {
                    TitleText = new ToastText { Text = title },
                    BodyTextLine1 = new ToastText { Text = message }
                },
                Scenario = ToastScenario.Default
            });
        }
        #endregion


        #region Private methods
        private void ShowNotification(ToastContent toast)
        {
            _notifier.Show(new ToastNotification(toast.GetXml()));
        }
        #endregion
    }
}
