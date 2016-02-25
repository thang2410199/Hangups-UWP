using System.Threading.Tasks;
using UniversalRateReminder;

namespace HangupsCore.Interfaces
{
    public interface IRateUsService
    {
        void Configurate(int LaunchLimit, bool ResetCountOnNewVersion, string RateButtonText, string CancelButtonText, string Title, string Content);
        void ResetCounter();
        Task ShowFeedbackPopup();
        Task ShowRatePopup(int LaunchLimit = 5, bool ResetCountOnNewVersion = false, string RateButtonText = "rate 5 stars", string CancelButtonText = "no, thanks", string Title = "Rate us!", string Content = "Your feedback helps us improve this app. If you like it, please take a minute and rate it with five stars so we can continue working on new features and updates.");
    }
}