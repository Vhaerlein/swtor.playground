using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TorPlayground.DamageModel;
using TorPlayground.DamageModel.Data;
using TorPlayground.StatOptimizer.Utils;

namespace TorPlayground.StatOptimizer.ViewModel
{
	public class MainViewModel : NotifyPropertyChangedObject
	{
		public ConfigurationViewModel ConfigurationViewModel { get; }
		public ObservableCollection<SessionViewModel> SessionViewModels { get; }

		public SessionViewModel ActiveSessionViewModel
		{
			get { return _activeSessionViewModel; }
			set
			{
				if (_activeSessionViewModel != value)
				{
					_profile.Sessions.ForEach(s => s.Active = false);
					_activeSessionViewModel = value;
					_activeSessionViewModel.Active = true;
					ConfigurationViewModel.CorrectionViewModel.IsOutdated = true;
					UpdateDps();
					OnPropertyChanged();
				}
			}
		}

		private SessionViewModel _activeSessionViewModel;

		public double Dps
		{
			get { return _dps; }
			set
			{
				_dps = value;
				OnPropertyChanged();
			}
		}
		private double _dps;

		public ValueCorrectionViewModel NewDps { get; }

		public ICommand RecalculateCommand { get; private set; }
		public MainViewModel() : this(DataManager.Profile){}

		private readonly Profile _profile;

		public MainViewModel(Profile profile)
		{
			_profile = profile;
			ConfigurationViewModel = new ConfigurationViewModel(profile.Configuration);
			ConfigurationViewModel.ConfigurationUpdated += () =>
			{
				ConfigurationViewModel.CorrectionViewModel.IsOutdated = true;
				UpdateDps();
			};
			SessionViewModels = new ObservableCollection<SessionViewModel>();
			foreach (var session in profile.Sessions)
			{
				var sessionViewModel = new SessionViewModel(session, profile.Configuration);
				sessionViewModel.SessionUpdated += () =>
				{
					ConfigurationViewModel.CorrectionViewModel.IsOutdated = true;
					UpdateDps();
				};
				SessionViewModels.Add(sessionViewModel);
			}

			ActiveSessionViewModel = SessionViewModels.FirstOrDefault(s => s.Active);

			NewDps = new ValueCorrectionViewModel(ValueType.Double);

			RecalculateCommand = new CommandHandler(CalculateDpsAndCorrection);
			CalculateDpsAndCorrection();
		}

		private void UpdateDps()
		{
			_profile.UpdateDpsEstimation();
			Dps = _profile.Dps;
		}

		private async void CalculateDpsAndCorrection()
		{
			UpdateDps();
			Mouse.OverrideCursor = Cursors.Wait;
			await Task.Run(() =>
			{
				var correction = DpsUtils.FindHighestDpsCorrection(_profile);
				ConfigurationViewModel.CorrectionViewModel.UpdateCorrection(correction);
				NewDps.NewValue = correction.Dps;
				NewDps.Difference = correction.Dps - _profile.Dps;
				OnPropertyChanged(nameof(NewDps));
			});
			Mouse.OverrideCursor = null;
		}
	}
}
