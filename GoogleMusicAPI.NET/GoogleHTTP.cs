using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

namespace GoogleMusicAPI
{
    public class GoogleHTTP
    {
        public static String AuthroizationToken = null;
        public static CookieContainer AuthorizationCookieCont = new CookieContainer();
        public static CookieCollection AuthorizationCookies = new CookieCollection();

        public class GoogleResponse
        {
            public HttpWebRequest Request;
            public HttpWebResponse Response;
            public String Data;
        }

        public static void GET(string url, FormBuilder form, Action<GoogleResponse> googleResponseCallback)
        {
            
        }

        public static void POST(string url, FormBuilder form, Action<GoogleResponse> googleResponseCallback, String method = "POST")
        {
            GoogleResponse googleResponse = new GoogleResponse();

            if (url.StartsWith("https://play.google.com/music/services/"))
            {
                url += String.Format("?u=0&xt={0}", GoogleHTTP.GetCookieValue("xt"));
            }

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = form.ContentType;
            httpWebRequest.Method = method;
            httpWebRequest.CookieContainer = AuthorizationCookieCont;

            if (AuthroizationToken != null)
                httpWebRequest.Headers[HttpRequestHeader.Authorization] = String.Format("GoogleLogin auth={0}", AuthroizationToken);

            byte[] requestBytes = form.GetBytes();

            googleResponse.Request = httpWebRequest;

            Task.Factory.FromAsync<Stream>(httpWebRequest.BeginGetRequestStream, httpWebRequest.EndGetRequestStream, null).ContinueWith(task =>
            {
                task.Result.Write(requestBytes, 0, requestBytes.Length);

                Task.Factory.FromAsync<WebResponse>(httpWebRequest.BeginGetResponse, httpWebRequest.EndGetResponse, null).ContinueWith(task2 =>
                {
                    var resp = task2.Result;

                    googleResponse.Response = (HttpWebResponse)resp;

                    using (var responseStream = resp.GetResponseStream())
                    {
                        var reader = new StreamReader(responseStream);

                        googleResponse.Data = reader.ReadToEnd();

                        if (googleResponseCallback != null)
                        {
                            googleResponseCallback(googleResponse);
                        }
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public static void SetCookieData(CookieContainer cont, CookieCollection coll)
        {
            AuthorizationCookieCont = cont;
            AuthorizationCookies = coll;
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
    }
}
