using HangupsCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Graphics.Display;
using Windows.System.Profile;
using Windows.UI.Xaml.Controls;

namespace HangupsCore.Services
{
    public enum DeviceTypes
    {
        Desktop,
        Mobile,
        Other
    }

    public enum AppState
    {
        Tablet,
        Mobile
    }

    public class DeviceService : ServiceBase
    {
        IStateCondition stateCondition;
        public DeviceService(Manager man, IStateCondition stateCondition) : base(man)
        {
            this.stateCondition = stateCondition;
        }
        public DeviceTypes CurrentDevice()
        {
            string deviceFamily = AnalyticsInfo.VersionInfo.DeviceFamily;
            switch (deviceFamily)
            {
                case "Windows.Desktop":
                    return DeviceTypes.Desktop;
                case "Windows.Mobile":
                    return DeviceTypes.Mobile;
                default:
                    return DeviceTypes.Other;

            }
        }

        public string GetAppVersion()
        {
            PackageVersion pv = Package.Current.Id.Version;
            Version version = new Version(Package.Current.Id.Version.Major,
                Package.Current.Id.Version.Minor,
                Package.Current.Id.Version.Revision,
                Package.Current.Id.Version.Build);
            string appVersion = version.Major + "." + version.Minor + "." + version.MinorRevision + "." + version.Build;
            return appVersion;
        }

        public AppState GetAppState()
        {
            return stateCondition.GetCurrentState();
        }

        public void LockDisplayOrientations(bool displayLock = true)
        {
            if (displayLock)
                DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
            else
                DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait | DisplayOrientations.Landscape;
        }
    }
}
