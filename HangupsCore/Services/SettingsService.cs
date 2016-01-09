using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Newtonsoft.Json;

namespace HangupsCore.Services
{
    public class SettingsService
    {
        private ApplicationDataContainer roamingSettings;
        public SettingsService()
        {
            roamingSettings = ApplicationData.Current.RoamingSettings;
        }

        public T GetValue<T>(string key)
        {
            var value = roamingSettings.Values[key];
            var ret = default(T);
            try
            {
                ret = JsonConvert.DeserializeObject<T>(value as string);
            }
            catch (Exception e)
            {
                RemoveValue(key);
                return default(T);
            }
            return ret;
        }

        public void SetValue(string key, object value)
        {
            var jsonValue = JsonConvert.SerializeObject(value);
            roamingSettings.Values[key] = jsonValue;
        }

        public void RemoveValue(string key)
        {
            roamingSettings.Values.Remove(key);
        }
    }
}
