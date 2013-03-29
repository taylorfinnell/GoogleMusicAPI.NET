using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TestWPF
{
	public class RelayCommand : ICommand
	{
		public event EventHandler CanExecuteChanged;
		private Predicate<object> _canExecute;
		private Action<object> _execute;

		public RelayCommand(Action<object> execute)
		{
			_execute = execute;
		}

		public RelayCommand(Predicate<object> canExecute, Action<object> execute)
		{
			_canExecute = canExecute;
			_execute = execute;
		}

		public bool CanExecute(object parameter)
		{
			if (_canExecute != null)
			{
				return _canExecute(parameter);
			}
			return true;
		}
				
		public void Execute(object parameter)
		{
			_execute(parameter);
		}
	}
}
