using HangupsCore.Helpers;
using HangupsCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace HangupsCore.Services
{
    public class KeyboardService : ServiceBase, IKeyboardService
    {
        public KeyboardService(Manager manager) : base(manager)
        {
            Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated += CoreDispatcher_AcceleratorKeyActivated;
        }

        private void CoreDispatcher_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs e)
        {
            if ((e.EventType == CoreAcceleratorKeyEventType.SystemKeyDown ||
                e.EventType == CoreAcceleratorKeyEventType.KeyDown))
            {
                var coreWindow = Windows.UI.Xaml.Window.Current.CoreWindow;
                var downState = CoreVirtualKeyStates.Down;
                var virtualKey = e.VirtualKey;
                bool winKey = ((coreWindow.GetKeyState(VirtualKey.LeftWindows) & downState) == downState || (coreWindow.GetKeyState(VirtualKey.RightWindows) & downState) == downState);
                bool altKey = (coreWindow.GetKeyState(VirtualKey.Menu) & downState) == downState;
                bool controlKey = (coreWindow.GetKeyState(VirtualKey.Control) & downState) == downState;
                bool shiftKey = (coreWindow.GetKeyState(VirtualKey.Shift) & downState) == downState;

                // raise keydown actions
                var keyDown = new KeyboardEventArgs
                {
                    AltKey = altKey,
                    Character = ToChar(virtualKey, shiftKey),
                    ControlKey = controlKey,
                    EventArgs = e,
                    ShiftKey = shiftKey,
                    VirtualKey = virtualKey
                };

                try { _KeyDown?.Raise(this, keyDown); }
                catch { }

                // Handle F5 to refresh content
                if (virtualKey == VirtualKey.F5)
                {
                    bool noModifiers = !altKey && !controlKey && !shiftKey;
                    _RefreshRequest?.Raise(this, keyDown);
                }
            }
        }

        public event EventHandler<KeyboardEventArgs> KeyDown
        {
            add { _KeyDown.Add(value); }
            remove { _KeyDown.Remove(value); }
        }
        SmartWeakEvent<EventHandler<KeyboardEventArgs>> _KeyDown = new SmartWeakEvent<EventHandler<KeyboardEventArgs>>();

        public event EventHandler<KeyboardEventArgs> RefreshRequest
        {
            add { _RefreshRequest.Add(value); }
            remove { _RefreshRequest.Remove(value); }
        }
        SmartWeakEvent<EventHandler<KeyboardEventArgs>> _RefreshRequest = new SmartWeakEvent<EventHandler<KeyboardEventArgs>>();

        private static char? ToChar(VirtualKey key, bool shift)
        {
            // convert virtual key to char
            if (32 == (int)key)
                return ' ';

            VirtualKey search;

            // look for simple letter
            foreach (var letter in "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
            {
                if (Enum.TryParse<VirtualKey>(letter.ToString(), out search) && search.Equals(key))
                    return (shift) ? letter : letter.ToString().ToLower()[0];
            }

            // look for simple number
            foreach (var number in "1234567890")
            {
                if (Enum.TryParse<VirtualKey>("Number" + number.ToString(), out search) && search.Equals(key))
                    return number;
            }

            // not found
            return null;
        }
    }

    enum VKeyClass_EnUs
    {
        Control, // 0-31, 33-47, 91-95, 144-165
        Character, // 32, 48-90
        NumPad, // 96-111
        Function // 112 - 135
    }

    public enum VKeyCharacterClass
    {
        Space,
        Numeric,
        Alphabetic
    }

}
