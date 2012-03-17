using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;


namespace GoogleMusicAPI
{
    public class GoogleHTTP : HTTP
    {
        public static String AuthroizationToken = null;
        public static CookieContainer AuthorizationCookieCont = new CookieContainer();
        public static CookieCollection AuthorizationCookies = new CookieCollection();

        public GoogleHTTP()
        {

        }

        public override HttpWebRequest SetupRequest(Uri address)
        {
            if (address.ToString().StartsWith("https://play.google.com/music/services/"))
            {
                address = new Uri(address.OriginalString + String.Format("?u=0&xt={0}", GoogleHTTP.GetCookieValue("xt")));
            }

            HttpWebRequest request = base.SetupRequest(address);

            request.CookieContainer = AuthorizationCookieCont;
            
            if (AuthroizationToken != null)
                request.Headers[HttpRequestHeader.Authorization] = String.Format("GoogleLogin auth={0}", AuthroizationToken);

            return request;
        }

        public static String GetCookieValue(String cookieName)
        {
            foreach (Cookie cookie in AuthorizationCookies)
            {
                if (cookie.Name.Equals(cookieName))
                    return cookie.Value;
            }

            return null;
        }

        public static void SetCookieData(CookieContainer cont, CookieCollection coll)
        {
            AuthorizationCookieCont = cont;
            AuthorizationCookies = coll;
        }
    }
}
