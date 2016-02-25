using Newtonsoft.Json;
using System;
using Windows.Storage;

namespace HangupsCore.Interfaces
{
    public interface ISettingsService
    {
        T GetValueLocal<T>(string key);
        T GetValueRoaming<T>(string key);
        void RemoveValueLocal(string key);
        void RemoveValueRoaming(string key);
        void SetValueLocal(string key, object value);
        void SetValueRoaming(string key, object value);
    }
}
