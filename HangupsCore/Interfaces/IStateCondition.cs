using HangupsCore.Services;
using Windows.UI.Xaml.Controls;

namespace HangupsCore.Interfaces
{
    public interface IStateCondition
    {
        AppState GetCurrentState();
        void Configurate(Frame rootFrame, double width);
    }
}