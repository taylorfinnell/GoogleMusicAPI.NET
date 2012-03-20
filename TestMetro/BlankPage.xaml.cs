using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GoogleMusicAPI;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading;
using Windows.UI.Core;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TestMetro
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage : Page
    {
        GoogleMusicAPI.API api = new GoogleMusicAPI.API();

        public BlankPage()
        {
            this.InitializeComponent();
            api.OnLoginComplete += OnLoginComplete;
            api.OnGetAllSongsComplete += OnGetAllSongsCompleted;

            api.Login("", "");
        }

        void OnLoginComplete(object s, EventArgs e)
        {
            api.GetAllSongs();
        }

        void OnGetAllSongsCompleted(List<GoogleMusicSong> songs)
        {
            Dispatcher.Invoke(CoreDispatcherPriority.Normal, (x, y) =>
            {
                foreach (GoogleMusicSong song in songs)
                    itemListView.Items.Add(song);
            }, this, null);
        }
    }
}
