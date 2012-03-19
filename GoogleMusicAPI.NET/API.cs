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

        public delegate void _GetAllSongs(List<GoogleMusicSong> songList);
        public  _GetAllSongs OnGetAllSongsComplete;

        public delegate void _AddPlaylist(AddPlaylistResp resp);
        public  _AddPlaylist OnCreatePlaylistComplete;

        public delegate void _Error(Exception e);
        public _Error OnError;

        public delegate void _GetPlaylists(GoogleMusicPlaylists pls);
        public  _GetPlaylists OnGetPlaylistsComplete;

        public delegate void _GetPlaylist(GoogleMusicPlaylist pls);
        public _GetPlaylist OnGetPlaylistComplete;

        public delegate void _GetSongURL(GoogleMusicSongUrl songurl);
        public  _GetSongURL OnGetSongURL;

        public delegate void _DeletePlaylist(DeletePlaylistResp resp);
        public  _DeletePlaylist OnDeletePlaylist;

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

            FormBuilder form = new FormBuilder();
            form.AddFields(fields);
            form.Close();

            Action<GoogleMusicAPI.GoogleHTTP.GoogleResponse> ActionLogin = (g) =>
            {
                string CountTemplate = @"Auth=(?<AUTH>(.*?))$";
                Regex CountRegex = new Regex(CountTemplate, RegexOptions.IgnoreCase);
                GoogleHTTP.AuthroizationToken = CountRegex.Match(g.Data).Groups["AUTH"].ToString();

                Action<GoogleMusicAPI.GoogleHTTP.GoogleResponse> action2 = (gr) =>
               {
                   GoogleHTTP.SetCookieData(gr.Request.CookieContainer, gr.Response.Cookies);

                   if (OnLoginComplete != null)
                       OnLoginComplete(this, null);
               };

                GoogleHTTP.POST("https://play.google.com/music/listen?hl=en&u=0", FormBuilder.Empty, action2);
            };

            GoogleHTTP.POST("https://www.google.com/accounts/ClientLogin", form, ActionLogin);
        }
        #endregion

        #region GetAllSongs
        /// <summary>
        /// Gets all the songs
        /// </summary>
        /// <param name="continuationToken"></param>
        public void GetAllSongs(String continuationToken = "")
        {
            List<GoogleMusicSong> library = new List<GoogleMusicSong>();

            String jsonString = "{\"continuationToken\":\"" + continuationToken +  "\"}";

            JsonConvert.SerializeObject(jsonString);

            Dictionary<String, String> fields = new Dictionary<String, String>
            {
               {"json", jsonString}
            };

            FormBuilder form = new FormBuilder();
            form.AddFields(fields);
            form.Close();

            Action<GoogleMusicAPI.GoogleHTTP.GoogleResponse> ActionGetAllSongs = (g) =>
            {
                GoogleMusicPlaylist pl = JsonConvert.DeserializeObject<GoogleMusicPlaylist>(g.Data);

                trackContainer.AddRange(pl.Songs);

                if (!String.IsNullOrEmpty(pl.ContToken))
                {
                    GetAllSongs(pl.ContToken);
                }
                else
                {
                    if (OnGetAllSongsComplete != null)
                        OnGetAllSongsComplete(trackContainer);

                    return;
                }
            };

            GoogleHTTP.POST("https://play.google.com/music/services/loadalltracks", form, ActionGetAllSongs);
        }
        #endregion

        #region AddPaylist
        /// <summary>
        /// Creates a playlist with given name
        /// </summary>
        /// <param name="playlistName">Name of playlist</param>
        public void AddPlaylist(String playlistName)
        {
            String jsonString = "{\"title\":\"" + playlistName + "\"}";

            Dictionary<String, String> fields = new Dictionary<String, String>
            {
               {"json", jsonString}
            };

            Action<GoogleMusicAPI.GoogleHTTP.GoogleResponse> ActionAddPlaylist = (gr) =>
            {
                AddPlaylistResp resp = JsonConvert.DeserializeObject<AddPlaylistResp>(gr.Data);

                 if (OnCreatePlaylistComplete != null)
                     OnCreatePlaylistComplete(resp);
            };

            FormBuilder form = new FormBuilder();
            form.AddFields(fields);
            form.Close();

            GoogleHTTP.POST("https://play.google.com/music/services/addplaylist", form, ActionAddPlaylist);
        }
        #endregion

        #region GetPlaylist
        /// <summary>
        /// Returns all user and instant playlists
        /// </summary>
        public void GetPlaylist(String plID = "all")
        {
            String jsonString = (plID.Equals("all")) ? "{}" : "{\"id\":\"" + plID + "\"}";

            Dictionary<String, String> fields = new Dictionary<String, String>() { };

            fields.Add("json", jsonString);

            FormBuilder form = new FormBuilder();
            form.AddFields(fields);
            form.Close();

            Action<GoogleMusicAPI.GoogleHTTP.GoogleResponse> ActionPlaylistRecv = (gr) =>
            {
                GoogleMusicPlaylists playlists = JsonConvert.DeserializeObject<GoogleMusicPlaylists>(gr.Data);

                if (OnGetPlaylistsComplete != null)
                    OnGetPlaylistsComplete(playlists);
            };

            Action<GoogleMusicAPI.GoogleHTTP.GoogleResponse> ActionPlaylistRecvSingle = (gr) =>
            {
                GoogleMusicPlaylist pl = JsonConvert.DeserializeObject<GoogleMusicPlaylist>(gr.Data);

                if (OnGetPlaylistComplete != null)
                    OnGetPlaylistComplete(pl);
            };

            if (plID.Equals("all"))
                GoogleHTTP.POST("https://play.google.com/music/services/loadplaylist", form, ActionPlaylistRecv);
            else
                GoogleHTTP.POST("https://play.google.com/music/services/loadplaylist", form, ActionPlaylistRecvSingle);
        }
        #endregion

        #region GetSongURL
        public void GetSongURL(String id)
        {
            Action<GoogleMusicAPI.GoogleHTTP.GoogleResponse> ActionSongUrlRecv = (gr) =>
           {
               GoogleMusicSongUrl url = JsonConvert.DeserializeObject<GoogleMusicSongUrl>(gr.Data);

               if (OnGetSongURL != null)
                   OnGetSongURL(url);
           };

            GoogleHTTP.GET(String.Format("https://play.google.com/music/play?u=0&songid={0}&pt=e", id), FormBuilder.Empty, ActionSongUrlRecv);
        }
        #endregion

        #region DeletePlaylist
        //{"deleteId":"c790204e-1ee2-4160-9e25-7801d67d0a16"}
        public void DeletePlaylist(String id)
        {
            String jsonString = "{\"id\":\"" + id + "\"}";

            JsonConvert.SerializeObject(jsonString);

            Dictionary<String, String> fields = new Dictionary<String, String>
            {
               {"json", jsonString}
            };

            FormBuilder builder = new FormBuilder();
            builder.AddFields(fields);
            builder.Close();

            Action<GoogleMusicAPI.GoogleHTTP.GoogleResponse> ActionDeletePlaylist = (gr) =>
            {
                DeletePlaylistResp resp = JsonConvert.DeserializeObject<DeletePlaylistResp>(gr.Data);

                if (OnDeletePlaylist != null)
                    OnDeletePlaylist(resp);
            };

            GoogleHTTP.POST("https://play.google.com/music/services/deleteplaylist", builder,ActionDeletePlaylist);
        }

        #endregion
    }
}
