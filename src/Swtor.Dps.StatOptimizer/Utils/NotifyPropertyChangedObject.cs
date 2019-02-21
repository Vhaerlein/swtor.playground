using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Swtor.Dps.StatOptimizer.Utils
{
	public abstract class NotifyPropertyChangedObject : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		[TorSimulator.StatOptimizer.Annotations.NotifyPropertyChangedInvocator]
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
