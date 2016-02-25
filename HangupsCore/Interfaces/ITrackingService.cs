using System.Collections.Generic;

namespace HangupsCore.Interfaces
{
    public interface ITrackingService
    {
        void AutoIntegrate();
        void TagEvent(string eventName);
        void TagEvent(string eventName, Dictionary<string, string> attribute);
        void TagScreen(string screenName);
    }
}