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
        public event _GetAllSongs OnGetAllSongsComplete;

        public delegate void _AddPlaylist(AddPlaylistResp resp);
        public event _AddPlaylist OnCreatePlaylistComplete;

        public delegate void _Error(Exception e);
        public _Error OnError;

        public delegate void _GetPlaylists(GoogleMusicPlaylists pls);
        public event _GetPlaylists OnGetPlaylistsComplete;

        public delegate void _GetSongURL(GoogleMusicSongUrl songurl);
        public event _GetSongURL OnGetSongURL;

        public delegate void _DeletePlaylist(DeletePlaylistResp resp);
        public event _DeletePlaylist OnDeletePlaylist;

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
            if (error != null)
            {
                OnError(error);
                return;
            }

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
            if (error != null)
            {
                OnError(error);
                return;
            }

            GoogleHTTP.SetCookieData(request.CookieContainer, response.Cookies);

            if (OnLoginComplete != null)
                OnLoginComplete(this, EventArgs.Empty);
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

            FormBuilder builder = new FormBuilder();
            builder.AddFields(fields);
            builder.Close();

            client.UploadDataAsync(new Uri("https://play.google.com/music/services/loadalltracks"), builder, TrackListChunkRecv);
        }

        private void TrackListChunkRecv(HttpWebRequest request, HttpWebResponse response, String jsonData, Exception error)
        {
            if (error != null)
            {
                OnError(error);
                return;
            }

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

        #region AddPaylist
        /// <summary>
        /// Creates a playlist with given name
        /// </summary>
        /// <param name="playlistName">Name of playlist</param>
        public void AddPlaylist(String playlistName)
        {
            String jsonString = "{\"title\":\"" + playlistName + "\"}";

            JsonConvert.SerializeObject(jsonString);

            Dictionary<String, String> fields = new Dictionary<String, String>
            {
               {"json", jsonString}
            };

            FormBuilder builder = new FormBuilder();
            builder.AddFields(fields);
            builder.Close();

            client.UploadDataAsync(new Uri("https://play.google.com/music/services/addplaylist"), builder, PlaylistCreated);
        }

        private void PlaylistCreated(HttpWebRequest request, HttpWebResponse response, String jsonData, Exception error)
        {
            if (error != null)
            {
                ThrowError(error);
                return;
            }

            AddPlaylistResp resp = null;

            try
            {
                resp = JsonConvert.DeserializeObject<AddPlaylistResp>(jsonData);
            }
            catch (Exception e)
            {
                ThrowError(error);
                return;
            }

            if (OnCreatePlaylistComplete != null)
                OnCreatePlaylistComplete(resp);
        }
        #endregion

        #region GetPlaylist
        /// <summary>
        /// Returns all user and instant playlists
        /// </summary>
        public void GetPlaylists()
        {
            client.UploadDataAsync(new Uri("https://play.google.com/music/services/loadplaylist"), FormBuilder.Empty, PlaylistRecv);
        }

        private void PlaylistRecv(HttpWebRequest request, HttpWebResponse response, String jsonData, Exception error)
        {
            if (error != null)
            {
                ThrowError(error);
            }

            GoogleMusicPlaylists playlists = null;
            try
            {
                playlists = JsonConvert.DeserializeObject<GoogleMusicPlaylists>(jsonData);
            }
            catch (Exception e)
            {
                ThrowError(error);
            }

            if (OnGetPlaylistsComplete != null)
                OnGetPlaylistsComplete(playlists);
        }
        #endregion

        #region GetSongURL
        public void GetSongURL(String id)
        {
            client.DownloadStringAsync(new Uri(String.Format("https://play.google.com/music/play?u=0&songid={0}&pt=e", id)), SongUrlRecv);
        }

        private void SongUrlRecv(HttpWebRequest request, HttpWebResponse response, String jsonData, Exception error)
        {
            if (error != null)
            {
                ThrowError(error);
                return;
            }

            GoogleMusicSongUrl url = null;
            try
            {
                url = JsonConvert.DeserializeObject<GoogleMusicSongUrl>(jsonData);
            }
            catch (Exception e)
            {
                OnError(e);
            }

            if (OnGetSongURL != null)
                OnGetSongURL(url);

        }

        private void ThrowError(Exception error)
        {
            if (OnError != null)
                OnError(error);
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

            client.UploadDataAsync(new Uri("https://play.google.com/music/services/deleteplaylist"), builder, PlaylistDeleted);
        }

        private void PlaylistDeleted(HttpWebRequest request, HttpWebResponse response, String jsonData, Exception error)
        {
            if (error != null)
            {
                ThrowError(error);
                return;
            }

            DeletePlaylistResp resp = null;
            try
            {
                resp = JsonConvert.DeserializeObject<DeletePlaylistResp>(jsonData);
            }
            catch (System.Exception ex)
            {
                ThrowError(ex);
                return;
            }

            if (OnDeletePlaylist != null)
                OnDeletePlaylist(resp);
        }
        #endregion
    }
}
