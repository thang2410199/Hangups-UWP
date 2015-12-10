using System;

namespace Hangups.Service
{
    public class KeyboardService
    {
        KeyboardHelper _helper;

        public KeyboardService()
        {
            _helper = new KeyboardHelper();
            _helper.KeyDown = (e) => { KeyDown?.Invoke(e); };
            _helper.GoBackGestured = () => { AfterBackGesture?.Invoke(); };
            _helper.GoForwardGestured = () => { AfterForwardGesture?.Invoke(); };
        }

        public Action<KeyboardEventArgs> KeyDown { get; set; }
        public Action AfterBackGesture { get; set; }
        public Action AfterForwardGesture { get; set; }
    }

}
