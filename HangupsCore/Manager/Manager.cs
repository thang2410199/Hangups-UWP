using HangupsCore.Interfaces;
using HangupsCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangupsCore
{
    public class Manager
    {
        public static Manager Current { get; set; }
        public IIAPService IAPService { get; set; }
        public IKeyboardService KeyboardService { get; set; }
        public IWebService WebService { get; set; }
        public ILocalizationService LocalizationService { get; set; }
        public ILogService LogService { get; set; }
        public IAuthenticationService AuthenticationService { get; set; }
        public IRateUsService RateUsService { get; set; }
        public IDatabase Database { get; set; }
        public IImageService ImageService { get; set; }
        public ITrackingService TrackingService { get; set; }
        public DeviceService DeviceService { get; set; }
        public ICameraService CameraService { get; set; }
        public ISettingsService SettingsService { get; set; }
        public HangoutsService HangoutsService { get; set; }

        public Manager()
        {
            Current = this;
        }
        public void Configure()
        {
            KeyboardService = new KeyboardService(this);
            //LocalizationService = new LocalizationService(this);
        }
    }
}
