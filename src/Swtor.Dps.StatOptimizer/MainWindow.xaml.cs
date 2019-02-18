using System.ComponentModel;
using System.Windows;
using Swtor.Dps.StatOptimizer.ViewModel;

namespace Swtor.Dps.StatOptimizer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		private readonly MainViewModel _viewModel = new MainViewModel();
		public MainWindow()
		{
			DataContext = _viewModel;
			InitializeComponent();
		}

		private void MainWindow_OnClosing(object sender, CancelEventArgs e)
		{
			if (_viewModel.HasUnsavedChanges)
			{
				var result = MessageBox.Show("Do you want to save your changes before exit?", "Unsaved data", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
				if (result == MessageBoxResult.Yes)
					_viewModel.SaveProfile();
				else if (result == MessageBoxResult.Cancel)
					e.Cancel = true;
			}
		}
	}
}
