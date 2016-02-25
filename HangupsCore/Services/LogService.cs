using HangupsCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace HangupsCore.Services
{
    public class LogService : ServiceBase, ILogService
    {
        public string LogText { get; set; } = "";

        public LogService(Manager man) : base(man)
        {

        }

        public void Log(string text, params string[] textM)
        {
            if (string.IsNullOrEmpty(text))
                return;
            LogText += DateTime.Now.ToString() + ": " + text + "\r\n";
            foreach (var item in textM)
            {
                if (!string.IsNullOrEmpty(text))
                    LogText += DateTime.Now.ToString() + ": " + item + "\r\n";
            }
        }

        public async Task WriteLog(string filename = "log")
        {
            // Create sample file; replace if exists.
            StorageFolder folder = Windows.Storage.ApplicationData.Current.TemporaryFolder;
            StorageFile logfile = await folder.CreateFileAsync(string.Format("{0}.txt", filename), CreationCollisionOption.OpenIfExists);
            await Windows.Storage.FileIO.WriteTextAsync(logfile, LogText);

        }
    }
}
