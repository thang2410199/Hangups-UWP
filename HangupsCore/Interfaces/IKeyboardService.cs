using System;

namespace HangupsCore.Helpers
{
    public interface IKeyboardService
    {
        event EventHandler<KeyboardEventArgs> KeyDown;
        event EventHandler<KeyboardEventArgs> RefreshRequest;
    }
}