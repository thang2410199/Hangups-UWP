using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangupsCore.Services
{
    public class HangoutsEndpoints
    {
        public const string ORIGIN_URL = "https://talkgadget.google.com";
        public const string IMAGE_UPLOAD_URL = "http://docs.google.com/upload/photos/resumable";
        public const string PVT_TOKEN_URL = "https://hangouts.google.com/webchat/extension-start";
        public const string CHAT_INIT_URL = "https://hangouts.google.com/webchat/u/0/load";
        public const string CHANNEL_URL = "https://0.client-channel.google.com/client-channel/";
        public const string CHAT_SERVER_URL = "https://clients6.google.com/chat/v1/";

        public const string COOKIE_URI = "https://google.com/";

        public const string OAUTH_SCOPE = "email profile https://www.google.com/accounts/OAuthLogin";
        public const string CLIENT_SECRET = "KWsJlkaMn1jGLxQpWxMnOox-";
        public const string CLIENT_ID = "936475272427.apps.googleusercontent.com";
        public const string REDIRECT_URL = "urn:ietf:wg:oauth:2.0:oob";
        public const string TOKEN_REQUEST_URL = "https://accounts.google.com/o/oauth2/token";

        /// <summary>
        /// Request "Authorization" header: Bearer {ACCESS_TOKEN}, use /"me" to get my profile OR /{USER_ID}
        /// </summary>
        public const string GET_USER_PROFILE = "https://www.googleapis.com/plus/v1/people/";
    }
}
