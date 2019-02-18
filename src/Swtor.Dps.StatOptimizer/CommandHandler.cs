using System;
using System.Windows.Input;

namespace Swtor.Dps.StatOptimizer
{
	public class CommandHandler : ICommand
	{
		private readonly Action<object> _actionWithParameter;
		private readonly Action _action;

		public CommandHandler(Action<object> action)
		{
			_actionWithParameter = action;
		}

		public CommandHandler(Action action)
		{
			_action = action;
		}

		public bool CanExecute(object parameter) => true;

		public event EventHandler CanExecuteChanged;

		public void Execute(object parameter)
		{
			if (_action != null)
				_action();
			else
			{
				_actionWithParameter?.Invoke(parameter);
			}
		}
	}
}
