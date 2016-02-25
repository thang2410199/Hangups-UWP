using HangupsCore.Helpers;
using System;

namespace HangupsCore.Interfaces
{
    public interface IKeyboardService
    {
        event EventHandler<KeyboardEventArgs> KeyDown;
        event EventHandler<KeyboardEventArgs> RefreshRequest;
    }
}