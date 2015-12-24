using Hangups.Helpers;
using System;

namespace Hangups.Services
{
    public class KeyboardService
    {
        KeyboardHelper _helper;

        public KeyboardService()
        {
            _helper = new KeyboardHelper();
            _helper.KeyDown = (e) => { KeyDown?.Invoke(e); };
            _helper.RefreshRequest = (e) => { RefreshRequest?.Invoke(e); };
        }

        public Action<KeyboardEventArgs> KeyDown { get; set; }
        public Action<KeyboardEventArgs> RefreshRequest { get; set; }
    }

}
