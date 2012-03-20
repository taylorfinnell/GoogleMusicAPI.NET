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

            api.OnLoginComplete += OnGMLoginComplete;
            api.OnGetPlaylistsComplete += api_OnGetPlaylistsComplete;
            
        }

        void OnGMLoginComplete(object s, EventArgs e)
        {
            api.GetPlaylist();
        }

        void api_OnGetPlaylistsComplete(GoogleMusicPlaylists pls)
        {
            Dispatcher.Invoke(CoreDispatcherPriority.Normal, (x, y) => 
            { 
                foreach (GoogleMusicPlaylist p in pls.UserPlaylists)
                    lbPlaylists.Items.Add(p.Title); 
            }, this, null);
            
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            api.Login(tbEmail.Text, tbPass.Text);
        }
    }
}
