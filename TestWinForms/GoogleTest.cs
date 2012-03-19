using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GoogleMusicAPI;
using System.Net;
using System.Threading.Tasks;

namespace GoogleMusicTest
{
    public partial class GoogleTest : Form
    {
        API api = new API();
        GoogleMusicPlaylists pls;

        public GoogleTest()
        {
            InitializeComponent();

            api.OnLoginComplete += OnGMLoginComplete;
            api.OnGetAllSongsComplete += GetAllSongsDone;
            api.OnCreatePlaylistComplete += api_OnCreatePlaylistComplete;
            api.OnGetPlaylistsComplete += new API._GetPlaylists(api_OnGetPlaylistsComplete);
            api.OnGetSongURL += new API._GetSongURL(api_OnGetSongURL);
            api.OnDeletePlaylist += new API._DeletePlaylist(api_OnDeletePlaylist);
            api.OnGetPlaylistComplete += new API._GetPlaylist(api_OnGetPlaylistComplete);
        }

        void api_OnGetPlaylistComplete(GoogleMusicPlaylist pls2)
        {
            int k = 0;
        }

        void api_OnDeletePlaylist(DeletePlaylistResp resp)
        {
            if (!String.IsNullOrEmpty(resp.ID))
            {
                MessageBox.Show("Deleted");
            }
        }

        void api_OnGetSongURL(GoogleMusicSongUrl songurl)
        {
            new WebClient().DownloadFile(songurl.URL, "C:\\test.mp3");
        }

        void api_OnGetPlaylistsComplete(GoogleMusicPlaylists pls)
        {
            this.pls = pls;

            this.Invoke(new MethodInvoker(delegate
            {
                foreach (GoogleMusicPlaylist pl in pls.UserPlaylists)
                {
                    lbPlaylists.Items.Add(pl.Title);
                }

                foreach (GoogleMusicPlaylist pl in pls.InstantMixes)
                {
                    lbPlaylists.Items.Add(pl.Title);
                }
            }));
        }

        void OnGMLoginComplete(object s, EventArgs e)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                this.Text += " -> Logged in";
            }));
        }

        void GetAllSongsDone(List<GoogleMusicSong> songs)
        {
            int num = 1;
            foreach (GoogleMusicSong song in songs)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = (num++).ToString();
                lvi.SubItems.Add(song.Title);
                lvi.SubItems.Add(song.Artist);
                lvi.SubItems.Add(song.Album);
                lvi.SubItems.Add(song.ID);
                this.Invoke(new MethodInvoker( delegate {
                    lvSongs.Items.Add(lvi);
                }));

                if (num >= 100)
                    break;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            api.Login(tbEmail.Text, tbPass.Text);
        }

        void api_OnCreatePlaylistComplete(AddPlaylistResp resp)
        {
            if (resp.Success)
                MessageBox.Show("Created pl");
        }

        private void btnCreatePl_Click(object sender, EventArgs e)
        {
            api.AddPlaylist("Testing");
        }

        private void btnFetchSongs_Click(object sender, EventArgs e)
        {
            api.GetAllSongs(String.Empty);
        }

        private void btnGetPlaylists_Click(object sender, EventArgs e)
        {
            api.GetPlaylist();
        }

        private void btnSongURL_Click(object sender, EventArgs e)
        {
            String id = lvSongs.SelectedItems[0].SubItems[4].Text;
            api.GetSongURL(id);
        }

        private void btnDeletePl_Click(object sender, EventArgs e)
        {
            String id = "";
            foreach (GoogleMusicPlaylist pl in pls.UserPlaylists)
            {
                if(pl.Title.Equals(lbPlaylists.SelectedItem.ToString()))
                {
                    id = pl.PlaylistID;
                    break;
                }
            }

            api.DeletePlaylist(id);
        }

        private void btnGetPlaylistSongs_Click(object sender, EventArgs e)
        {
            String id = "";
            foreach (GoogleMusicPlaylist pl in pls.UserPlaylists)
            {
                if (pl.Title.Equals(lbPlaylists.SelectedItem.ToString()))
                {
                    id = pl.PlaylistID;
                    break;
                }
            }

            api.GetPlaylist(id);
        }
    }
}
