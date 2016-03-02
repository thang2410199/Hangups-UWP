using HangupsCore.Helpers;
using HangupsCore.Interfaces;
using HangupsCore.Models;
using HangupsCore.ProtoJson;
using HangupsCore.ProtoJson.Schema;
using HangupsCore.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;

namespace HangupsCore.Services
{
    public class HangoutsService : ServiceBase
    {
        public AccessToken Token = new AccessToken();
        Dictionary<string, string> _initParams = new Dictionary<string, string>();
        Channel _channel;
        public CookieContainer cookieContainer = new CookieContainer();
        CancellationTokenSource CancelToken;
        HttpClient _client;


        private DateTime _last_response_date = DateTime.UtcNow;

        string _api_key;
        string _email;
        string _header_date;
        string _header_version;
        string _header_id;
        string _client_id;

        bool _wasConnected = false;

        double _timestamp = 0;
        public Entity CurrentUser { get; private set; }

        internal List<string> _contact_ids = new List<string>();
        internal List<string> _active_conversation_ids = new List<string>();

        #region events
        public event EventHandler<List<ConversationState>> ConversationHistoryLoaded;
        public event EventHandler<List<Entity>> ContactListLoaded;
        public event EventHandler ConnectionEstablished;
        public event EventHandler<ConversationState> NewConversationCreated;
        public event EventHandler<Event> NewConversationEventReceived;
        public event EventHandler<Entity> UserInformationReceived;
        public event EventHandler<Entity> ContactInformationReceived;

        public event EventHandler<PresenceResult> PresenceInformationReceived;
        #endregion

        private RequestHeader _requestHeader;

        RequestHeader RequestHeaderBody
        {
            get
            {
                if (_requestHeader == null)
                {
                    _requestHeader = new RequestHeader();
                    _requestHeader.client_identifier = new ClientIdentifier() { header_id = _header_id, resource = _client_id };
                    _requestHeader.client_version = new ClientVersion()
                    {
                        client_id = ClientId.CLIENT_ID_WEB_GMAIL,
                        build_type = ClientBuildType.BUILD_TYPE_PRODUCTION_APP,
                        major_version = _header_version,
                        version_timestamp = long.Parse(_header_date)
                    };
                    _requestHeader.language_code = "en";

                }
                return _requestHeader;
            }
        }


        private ISettingsService _settings;

        public HangoutsService(Manager man, ISettingsService settingsService) : base(man)
        {
            _settings = settingsService;

            //_initParams.Add("prop", "StartPage");
            _initParams.Add("client", "sm");
            _initParams.Add("stime", DateTime.Now.TimeIntervalSince1970().TotalSeconds.ToString("0"));
            _initParams.Add("nav", "true");
            _initParams.Add("prop", "ChromeApp");
            _initParams.Add("fid", "gtn-roster-iframe-id");
            _initParams.Add("ec", "[\"ci:ec\",true,true,false]");
            _initParams.Add("pvt", "");


            _channel = new Channel(this.cookieContainer);

            _channel.OnDataReceived += _channel_OnDataReceived;

            CancelToken = new CancellationTokenSource();

            _client = new HttpClient(new SigningMessageHandler(true, new CookieContainer()));
            _client.Timeout = new TimeSpan(0, 0, 30);
        }

        void _channel_OnDataReceived(object sender, JArray rawdata)
        {

            //Parse channel array and call the appropriate events.
            if (rawdata[0].ToString() == "noop")
            {
                // set active client if more than 120 sec of inactivity
                if ((DateTime.UtcNow - _last_response_date).TotalSeconds > 120)
                {
                    SetActiveClient();
                    _last_response_date = DateTime.UtcNow;
                }
            }
            else if (rawdata[0]["p"] != null)
            {
                JObject wrapper = JObject.Parse(rawdata[0]["p"].ToString());
                if (wrapper["3"] != null && wrapper["3"]["2"] != null)
                {
                    _client_id = wrapper["3"]["2"].ToString();
                    _requestHeader = null;

                    _channel.SendAck(0);

                    if (_channel.Connected && !_wasConnected)
                    {
                        _wasConnected = _channel.Connected;
                        if (ConnectionEstablished != null)
                            ConnectionEstablished(this, null);
                    }
                }

                if (wrapper["2"] != null && wrapper["2"]["2"] != null)
                {
                    JArray cbu = JArray.Parse(wrapper["2"]["2"].ToString());
                    cbu.RemoveAt(0);
                    BatchUpdate batchUpdate = ProtoJsonSerializer.Deserialize<BatchUpdate>(cbu as JArray);

                    foreach (StateUpdate state_update in batchUpdate.state_update)
                    {

                        if (state_update.event_notification != null)
                            if (state_update.event_notification.current_event.event_type == EventType.EVENT_TYPE_REGULAR_CHAT_MESSAGE)
                            {
                                if (_active_conversation_ids.Contains(state_update.event_notification.current_event.conversation_id.id))
                                {
                                    if (NewConversationEventReceived != null)
                                        NewConversationEventReceived(this, state_update.event_notification.current_event);
                                }
                                else
                                {
                                    if (NewConversationCreated != null)
                                    {
                                        ConversationState s = new ConversationState()
                                        {
                                            conversation_id = state_update.event_notification.current_event.conversation_id,
                                            conversation = state_update.conversation,
                                            events = new List<Event>() { state_update.event_notification.current_event }
                                        };
                                        NewConversationCreated(this, s);
                                    }
                                }
                            }

                        if (state_update.presence_notification != null)
                            foreach (var presence in state_update.presence_notification.presence)
                                PresenceInformationReceived(this, presence);

                        if (state_update.self_presence_notification != null)
                            PresenceInformationReceived(this, new PresenceResult()
                            {
                                user_id = CurrentUser.id,
                                presence = new Presence()
                                {
                                    available = state_update.self_presence_notification.client_presence_state.state == ClientPresenceStateType.CLIENT_PRESENCE_STATE_DESKTOP_ACTIVE
                                }
                            });

                    }



                    this._timestamp = long.Parse(wrapper["1"]["4"].ToString());
                }

            }



        }


        public void SetActiveClient()
        {

            SetActiveClientRequest request = new SetActiveClientRequest()
            {
                request_header = RequestHeaderBody,
                full_jid = string.Format("{0}/{1}", _email, _client_id),
                is_active = true,
                timeout_secs = 120,
                unknown = true
            };

            HttpResponseMessage message = _client.PostProtoJson(_api_key, "clients/setactiveclient", request);
        }

        #region Interface calls
        public async Task Login(bool forceNewToken = false)
        {
            var refreshToken = _settings.GetValueRoaming<String>("refresh_token");
            if (refreshToken != null && forceNewToken == false)
                await RefreshTokenAuth(refreshToken);
            else
                await GoogleLogin();


        }
        /// <summary>
        /// supply "me" if get my profile
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public async Task<GoogleUser> GetProfile(string user_id)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", Token.access_token));
            var response = await client.GetAsync(HangoutsEndpoints.GET_USER_PROFILE + "me");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<GoogleUser>(content);

            var checkResize = user.image.url.IndexOf("?sz=");
            if (checkResize != -1)
            {
                user.image.url = user.image.url.Remove(checkResize);
            }
            return user;
        }

        #endregion

        #region Private methods

        public async Task<HttpResponseMessage> MakeRequest(HttpClient client, HttpRequestMessage httpRequestMessage, CancellationTokenSource tokenSource = null)
        {
            try
            {
                DateTimeOffset startTime = DateTimeOffset.UtcNow;
                HttpResponseMessage response;
                if (tokenSource != null)
                {
                    response = await client.SendAsync(httpRequestMessage, tokenSource.Token).ConfigureAwait(false);
                }
                else
                {
                    response = await client.SendAsync(httpRequestMessage, CancelToken.Token).ConfigureAwait(false);
                }

                //RequestTelemetry request = new RequestTelemetry(httpRequestMessage.RequestUri.AbsoluteUri, startTime, startTime - DateTimeOffset.UtcNow, ((int)response.StatusCode).ToString(), response.IsSuccessStatusCode);
                //InSight.TrackRequest(request);

                return response;
            }
            catch (OperationCanceledException e)
            {
                Debug.WriteLine("Request cancelled");
                CancelToken = new CancellationTokenSource();
                return new HttpResponseMessage(HttpStatusCode.PreconditionFailed);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.RequestTimeout);
            }
        }

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
                new KeyValuePair<string, string>("client_id", HangoutsEndpoints.CLIENT_ID),
                new KeyValuePair<string, string>("client_secret", HangoutsEndpoints.CLIENT_SECRET),
                new KeyValuePair<string, string>("code", token),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("redirect_uri", HangoutsEndpoints.REDIRECT_URL),
            });

            var response = await client.PostAsync(HangoutsEndpoints.TOKEN_REQUEST_URL, body);
            response.EnsureSuccessStatusCode();

            var resultContent = await response.Content.ReadAsStringAsync();
            Token = JsonConvert.DeserializeObject<AccessToken>(resultContent);
            _settings.SetValueRoaming("access_token", Token.access_token);
            _settings.SetValueRoaming("refresh_token", Token.refresh_token);
            await GetCookies(Token.access_token);
        }

        private async Task RefreshTokenAuth(string refreshToken)
        {
            HttpClient client = new HttpClient();
            var body = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", HangoutsEndpoints.CLIENT_ID),
                new KeyValuePair<string, string>("client_secret", HangoutsEndpoints.CLIENT_SECRET),
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refreshToken)
            });
            var response = await client.PostAsync(HangoutsEndpoints.TOKEN_REQUEST_URL, body);
            response.EnsureSuccessStatusCode();

            var resultContent = await response.Content.ReadAsStringAsync();
            Token = JsonConvert.DeserializeObject<AccessToken>(resultContent);
            _settings.SetValueRoaming("access_token", Token.access_token);
            _settings.SetValueRoaming("refresh_token", Token.refresh_token);
            await GetCookies(Token.access_token);
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
            cookieContainer = new CookieContainer();
            if (response.Headers.TryGetValues("set-cookie", out cookies))
                foreach (var c in cookies)
                    cookieContainer.SetCookies(response.RequestMessage.RequestUri, c);
        }
        #endregion

        private string LoginUrl
        {
            get
            {
                return string.Format("https://accounts.google.com/o/oauth2/auth?client_id={0}&redirect_uri={1}&response_type=code&scope={2}",
                    Uri.EscapeDataString(HangoutsEndpoints.CLIENT_ID),
                    Uri.EscapeDataString(HangoutsEndpoints.REDIRECT_URL),
                    Uri.EscapeDataString(HangoutsEndpoints.OAUTH_SCOPE));
            }
        }
    }
}
