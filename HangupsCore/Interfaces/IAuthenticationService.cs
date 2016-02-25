using System.Threading.Tasks;

namespace HangupsCore.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> WindowsHelloAuthen();

        AuthenticationResult PasswordAuthen(string input);

        Task<AuthenticationResult> RequestUserConsent();

        void CreatePassword(string password);

        void DeleteAccount();
    }

    public enum AuthenticationResult
    {
        NotAvailable,
        Success,
        Fail
    }
}