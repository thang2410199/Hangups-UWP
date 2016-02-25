using System.IO;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace HangupsCore.Interfaces
{
    public interface IWebService
    {
        void CancelCurrentRequests();
        Task<string> GetString(string url);
        Task<Stream> MakeRawGetRequest(string url);
    }
}