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

namespace GoogleMusicTest
{
    public partial class GoogleTest : Form
    {
        API api = new API();
        public GoogleTest()
        {
            InitializeComponent();

            api.OnLoginComplete += OnGMLoginComplete;
            api.OnGetAllSongsComplete += GetAllSongsDone;
        }

        void OnGMLoginComplete(object s, EventArgs e)
        {
            api.GetAllSongs(String.Empty);
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

                this.Invoke(new MethodInvoker( delegate {
                    lvSongs.Items.Add(lvi);
                }));
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            api.Login(tbEmail.Text, tbPass.Text);
        }
    }
}
