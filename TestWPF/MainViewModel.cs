using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TestWPF
{
	public class MainViewModel : INotifyPropertyChanged
	{
		public string Email
		{
			get { return _email; }
			set
			{
				if (_email != value)
				{
					_email = value;
					RaisePropertyChanged("Email");
				}
			}
		}

		public ICommand FetchTracksCommand
		{
			get
			{
				if (_fetchTracks == null)
				{
					_fetchTracks = new RelayCommand(OnFetchTracks);
				}
				return _fetchTracks;
			}
		}

		private void OnFetchTracks(object parameter)
		{
		}

		private void RaisePropertyChanged(string property)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(property));
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;
		private string _email;
		private ICommand _fetchTracks;
	}
}
