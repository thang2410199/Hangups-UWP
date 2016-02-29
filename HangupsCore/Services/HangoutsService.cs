using HangupsCore.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;

namespace HangupsCore.Services
{
    public class HangoutsService : ServiceBase
    {
        private string oAuth_Scope = "https://www.google.com/accounts/OAuthLogin";
        private string client_secret = "KWsJlkaMn1jGLxQpWxMnOox-";
        private string client_id = "936475272427.apps.googleusercontent.com";
        private string redirect_uri = "urn:ietf:wg:oauth:2.0:oob";
        private string token_request_url = "https://accounts.google.com/o/oauth2/token";

        private ISettingsService _settings;

        public HangoutsService(Manager man, ISettingsService settingsService) : base(man)
        {
            _settings = settingsService;
        }


        #region Interface calls
        public async Task Login()
        {
            var refreshToken = _settings.GetValueRoaming<String>("refresh_token");
            if (refreshToken != null)
                await RefreshTokenAuth(refreshToken);
            else
                await GoogleLogin();
        }

        #endregion

        #region Private methods
        private async Task GoogleLogin()
        {
            try
            {
                WebAuthenticationResult result =
                    await WebAuthenticationBroker.AuthenticateAsync(
                        WebAuthenticationOptions.UseTitle,
                        new Uri(LoginUrl),
                        new Uri("https://accounts.google.com/o/oauth2/approval?"));

                switch (result.ResponseStatus)
                {
                    case WebAuthenticationStatus.Success:
                        await TokenAuth(result.ResponseData.ToString().Split('=')[1]);
                        break;
                    case WebAuthenticationStatus.UserCancel:
                    case WebAuthenticationStatus.ErrorHttp:
                        break;
                }
            }
            catch (Exception e)
            {

            }
        }

        private async Task TokenAuth(string token)
        {
            HttpClient client = new HttpClient();
            var body = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", client_id),
                new KeyValuePair<string, string>("client_secret", client_secret),
                new KeyValuePair<string, string>("code", token),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("redirect_uri", redirect_uri),
            });

            var response = await client.PostAsync(token_request_url, body);
            response.EnsureSuccessStatusCode();

            var resultContent = await response.Content.ReadAsStringAsync();
            JObject returnedData = JObject.Parse(resultContent);
            _settings.SetValueRoaming("access_token", (string)returnedData.SelectToken("access_token"));
            _settings.SetValueRoaming("refresh_token", (string)returnedData.SelectToken("refresh_token"));
            await GetCookies((string)returnedData.SelectToken("access_token"));
        }

        private async Task RefreshTokenAuth(string refreshToken)
        {
            HttpClient client = new HttpClient();
            var body = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", client_id),
                new KeyValuePair<string, string>("client_secret", client_secret),
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refreshToken)
            });
            var response = await client.PostAsync(token_request_url, body);
            response.EnsureSuccessStatusCode();

            var resultContent = await response.Content.ReadAsStringAsync();
            JObject returnedData = JObject.Parse(resultContent);
            _settings.SetValueRoaming("access_token", (string)returnedData.SelectToken("access_token"));
            await GetCookies((string)returnedData.SelectToken("access_token"));
        }


        private async Task GetCookies(string accessToken)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", accessToken));
            var response = await client.GetAsync("https://accounts.google.com/accounts/OAuthLogin?source=hangups&issueuberauth=1");
            response.EnsureSuccessStatusCode();
            var uberAuth = await response.Content.ReadAsStringAsync();
            response = await client.GetAsync(String.Format("https://accounts.google.com/MergeSession?service=mail&continue=http://www.google.com&uberauth={0}", uberAuth));
            response.EnsureSuccessStatusCode();
            IEnumerable<string> cookies;
            var responseCookies = new CookieContainer();
            if (response.Headers.TryGetValues("set-cookie", out cookies))
                foreach (var c in cookies)
                    responseCookies.SetCookies(response.RequestMessage.RequestUri, c);
        }
        #endregion

        private string LoginUrl
        {
            get
            {
                return string.Format("https://accounts.google.com/o/oauth2/auth?client_id={0}&redirect_uri={1}&response_type=code&scope={2}",
                    Uri.EscapeDataString(client_id),
                    Uri.EscapeDataString(redirect_uri),
                    Uri.EscapeDataString(oAuth_Scope));
            }
        }
    }
}
