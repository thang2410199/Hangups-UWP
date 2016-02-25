using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Devices;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace HangupsCore.Interfaces
{
    public interface ICameraService : IDisposable
    {
        List<DeviceInformation> AudioDeviceList { get; }
        List<DeviceInformation> CameraDeviceList { get; }
        bool IsCameraAvailable { get; }
        bool IsBackCamera { get; set; }
        Task Dispose(bool threadSafe);
        Task DisposeLowLagAsync();
        void FocusOnTap(TappedRoutedEventArgs e);
        Task InitialAsync(CaptureElement captureElement, MediaEncodingProfile profile = null, bool BackCamera = false);
        bool IsFlashAvailable();
        Task PauseLowLagAsync(MediaCapturePauseBehavior b);
        Task PauseRecordingAsync(MediaCapturePauseBehavior b);
        Task PrepareInitAsync();
        Task PrepareLowLagRecordingToFileAsync(StorageFile file);
        Task ResumeLowLagAsync();
        Task ResumeRecordingAsync();
        void SetActiveProfile(MediaEncodingProfile profile);
        void SetFlashStatus(bool flashStatus);
        Task SetResolutionAsync(uint width = 1024, uint height = 768);
        Task StartLowLagRecordingAsync();
        Task StartPreviewAsync();
        Task StartRecordingToFileAsync(StorageFile file);
        Task StopLowLagRecordingAsync();
        Task StopPreviewAsync();
        Task StopRecordingAsync();
    }
}