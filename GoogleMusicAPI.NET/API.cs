using System;
using System.Net;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace GoogleMusicAPI
{
    public class API
    {
        #region Events
        public EventHandler OnLoginComplete;

        public delegate void OnGetAllSongs(List<GoogleMusicSong> songList);
        public OnGetAllSongs OnGetAllSongsComplete;

        #endregion

        #region Members
        GoogleHTTP client;
        private List<GoogleMusicSong> trackContainer;
        #endregion

        #region Constructor
        public API()
        {
            client = new GoogleHTTP();
            trackContainer = new List<GoogleMusicSong>();
        }
        #endregion

        #region Login
        public void Login(String email, String password)
        {
            Dictionary<String, String> fields = new Dictionary<String, String>
            {
                {"service", "sj"},
                {"Email",  email},
                {"Passwd", password},
            };

            FormBuilder builder = new FormBuilder();
            builder.AddFields(fields);
            builder.Close();

            client.UploadDataAsync(new Uri("https://www.google.com/accounts/ClientLogin"), builder.ContentType, builder.GetBytes(), 10000, GetAuthTokenComplete, null);
        }

        public void Login(String authToken)
        {
            GoogleHTTP.AuthroizationToken = authToken;
            GetAuthCookies();
        }

        private void GetAuthTokenComplete(HttpWebRequest request, HttpWebResponse response, String jsonData, Exception error)
        {
            string CountTemplate = @"Auth=(?<AUTH>(.*?))$";
            Regex CountRegex = new Regex(CountTemplate, RegexOptions.IgnoreCase);
            string auth = CountRegex.Match(jsonData).Groups["AUTH"].ToString();

            GoogleHTTP.AuthroizationToken = auth;

            GetAuthCookies();
        }

        private void GetAuthCookies()
        {
            client.UploadDataAsync(new Uri("https://play.google.com/music/listen?hl=en&u=0"), 
                FormBuilder.Empty, GetAuthCookiesComplete);
        }

        private void GetAuthCookiesComplete(HttpWebRequest request, HttpWebResponse response, String jsonData, Exception error)
        {
            GoogleHTTP.SetCookieData(request.CookieContainer, response.Cookies);

            if (OnLoginComplete != null)
                OnLoginComplete(this, EventArgs.Empty);
        }
        #endregion

        #region GetAllSongs
        public void GetAllSongs(String continuationToken = "")
        {
            List<GoogleMusicSong> library = new List<GoogleMusicSong>();

            String jsonString = "{\"continuationToken\":\"" + continuationToken +  "\"}";

            Dictionary<String, String> fields = new Dictionary<String, String>
            {
               {"json", jsonString}
            };

            FormBuilder builder = new FormBuilder();
            builder.AddFields(fields);
            builder.Close();

            client.UploadDataAsync(new Uri("https://play.google.com/music/services/loadalltracks"), builder, TrackListChunkRecv);
        }

        private void TrackListChunkRecv(HttpWebRequest request, HttpWebResponse response, String jsonData, Exception error)
        {
            GoogleMusicPlaylist chunk = 
                Newtonsoft.Json.JsonConvert.DeserializeObject <GoogleMusicPlaylist>(jsonData);

            trackContainer.AddRange(chunk.Songs);

            if (!String.IsNullOrEmpty(chunk.ContToken))
            {
                GetAllSongs(chunk.ContToken);
            }
            else
            {
                if (OnGetAllSongsComplete != null)
                    OnGetAllSongsComplete(trackContainer);
            }
        }
        #endregion
    }
}
