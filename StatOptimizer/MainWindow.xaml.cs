using TorPlayground.DamageModel.Data;
using TorPlayground.StatOptimizer.ViewModel;

namespace TorPlayground.StatOptimizer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public MainWindow()
		{
			DataContext = new MainViewModel(DataManager.Profile);
			InitializeComponent();
		}
	}
}
