using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Swtor.Dps.DamageModel;
using Swtor.Dps.DamageModel.Data;
using Swtor.Dps.LogParser.Combat;
using Swtor.Dps.StatOptimizer.Utils;

namespace Swtor.Dps.StatOptimizer.ViewModel
{
	internal class MainViewModel : NotifyPropertyChangedObject
	{
		private string CurrentProfilePath
		{
			get => _currentProfilePath ?? (_currentProfilePath = Properties.Settings.Default.ProfilePath);
			set
			{
				_currentProfilePath = value;
				Properties.Settings.Default.ProfilePath = value;
				Properties.Settings.Default.Save();
			}
		}

		private string _currentProfilePath;

		public ConfigurationViewModel ConfigurationViewModel
		{
			get => _configurationViewModel;
			private set
			{
				_configurationViewModel = value;
				OnPropertyChanged();
			}
		}

		private ConfigurationViewModel _configurationViewModel;

		public ObservableCollection<SessionViewModel> SessionViewModels { get; }

		public SessionViewModel ActiveSessionViewModel
		{
			get => _activeSessionViewModel;
			set
			{
				if (_activeSessionViewModel != value)
				{
					HasUnsavedChanges = true;

					_profile.Sessions.ForEach(s => s.Active = false);
					_activeSessionViewModel = value;

					if (_activeSessionViewModel != null)
						_activeSessionViewModel.Active = true;

					ConfigurationViewModel.CorrectionViewModel.IsOutdated = true;

					CalculateDpsAndCorrection();
					OnPropertyChanged();
				}
			}
		}

		private SessionViewModel _activeSessionViewModel;

		public double Dps
		{
			get => _dps;
			set
			{
				_dps = value;
				OnPropertyChanged();
			}
		}

		private double _dps;

		public ValueCorrectionViewModel NewDps { get; private set; }

		public ICommand RecalculateCommand { get; }
		public ICommand RecalculateAllCommand { get; }
		public ICommand ParseCombatLogCommand { get; }
		public ICommand LoadProfileCommand { get; }
		public ICommand SaveProfileCommand { get; }
		public ICommand SaveProfileAsCommand { get; }
		public ICommand DeleteSessionCommand { get; }

		public string Title => $"SWtOR 5.2+: DPS Stat Optimization{(HasUnsavedChanges ? "*" : "")}{(Resources.DataManager.ExternalXml ? " (external xml)" : "")}";

		public bool HasUnsavedChanges
		{
			get => _profile.HasUnsavedChanges;
			private set
			{
				_profile.HasUnsavedChanges = value;
				OnPropertyChanged(nameof(Title));
			}
		}

		private Profile _profile;

		public MainViewModel()
		{
			SessionViewModels = new ObservableCollection<SessionViewModel>();

			RecalculateCommand = new CommandHandler(CalculateDpsAndCorrection);
			RecalculateAllCommand = new CommandHandler(CalculateDpsAndCorrectionAllSessions);

			ParseCombatLogCommand = new CommandHandler(ParseCombatLog);

			LoadProfileCommand = new CommandHandler(LoadProfile);
			SaveProfileCommand = new CommandHandler(SaveProfile);
			SaveProfileAsCommand = new CommandHandler(SaveProfileAs);
			DeleteSessionCommand = new CommandHandler(DeleteSession);

			if (Formulas.HasErrors)
				MessageBox.Show($"{string.Join("\n", Formulas.Errors)}\n\nDefault formula{(Formulas.Errors.Count > 1 ? "s" : "")} will be used.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

			try
			{
				LoadProfile
				(
					!string.IsNullOrEmpty(CurrentProfilePath)
						? DataManager.LoadProfile(CurrentProfilePath)
						: DataManager.DefaultProfile
				);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error loading profile \"{CurrentProfilePath}\":\n\n {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				LoadProfile(DataManager.DefaultProfile);
			}
		}

		private void DeleteSession()
		{
			if (ActiveSessionViewModel != null)
			{
				if (MessageBox.Show
					(
						$"Press [Ok] to delete \"{ActiveSessionViewModel.Name}\" combat session.", "Confirm delete",
						MessageBoxButton.OKCancel, MessageBoxImage.Warning
					)
					!= MessageBoxResult.OK)
					return;

				ActiveSessionViewModel.SessionUpdated -= OnConfigurationChanged;
				_profile.Sessions.Remove(ActiveSessionViewModel.Session);
				SessionViewModels.Remove(ActiveSessionViewModel);
				ActiveSessionViewModel = SessionViewModels.FirstOrDefault();
			}
		}

		public void SaveProfile()
		{
			if (!string.IsNullOrEmpty(CurrentProfilePath))
				SaveProfile(CurrentProfilePath);
			else
				SaveProfileAs();
		}

		private void SaveProfile(string filePath)
		{
			if (!DataManager.SaveProfile(_profile, filePath))
				MessageBox.Show($"Error saving profile \"{filePath}\".", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			else
			{
				CurrentProfilePath = filePath;
				MessageBox.Show($"Profile saved to \"{filePath}\".", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
				HasUnsavedChanges = false;
			}
		}

		private void SaveProfileAs()
		{
			var filePicker = new SaveFileDialog {Filter = "Json files (*.json)|*.json"};
			if (filePicker.ShowDialog() == true)
				SaveProfile(filePicker.FileName);
		}

		private void LoadProfile()
		{
			var filePicker = new OpenFileDialog {Filter = "Json files (*.json)|*.json"};

			if (filePicker.ShowDialog() == true)
			{
				try
				{
					LoadProfile(DataManager.LoadProfile(filePicker.FileName));
					CurrentProfilePath = filePicker.FileName;
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Error loading profile \"{filePicker.FileName}\":\n\n {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		private void LoadProfile(Profile profile)
		{
			_profile = profile ?? throw new Exception("Profile is null (most likely file is corrupted).");

			if (ConfigurationViewModel != null)
			{
				ConfigurationViewModel.ConfigurationUpdated -= OnConfigurationChanged;
				foreach (var sessionViewModel in SessionViewModels)
					sessionViewModel.SessionUpdated -= OnConfigurationChanged;
			}

			ConfigurationViewModel = new ConfigurationViewModel(profile.Configuration);
			ConfigurationViewModel.ConfigurationUpdated += OnConfigurationChanged;

			SessionViewModels.Clear();
			foreach (var session in profile.Sessions)
			{
				var sessionViewModel = new SessionViewModel(session, profile.Configuration, profile.DefaultValues);
				sessionViewModel.SessionUpdated += OnConfigurationChanged;
				SessionViewModels.Add(sessionViewModel);
			}

			ActiveSessionViewModel = SessionViewModels.FirstOrDefault(s => s.Active) ?? SessionViewModels.First();

			NewDps = new ValueCorrectionViewModel(ValueType.Double);
			HasUnsavedChanges = false;
		}

		private void OnConfigurationChanged()
		{
			HasUnsavedChanges = true;
			ConfigurationViewModel.CorrectionViewModel.IsOutdated = true;
			foreach (var a in _activeSessionViewModel.Abilities)
				a.UpdateNumbers();
			UpdateDps();
		}

		private void UpdateDps()
		{
			_profile.UpdateDpsEstimation();
			Dps = _profile.Dps;
		}

		private void UpdateDpsAllSessions()
		{
			_profile.UpdateDpsEstimationAllSessions();
			Dps = _profile.Dps;
		}

		private async void CalculateDpsAndCorrection()
		{
			Mouse.OverrideCursor = Cursors.Wait;
			await Task.Run
			(
				() =>
				{
					UpdateDps();
					var correction = DpsUtils.FindHighestDpsCorrection(_profile);
					ConfigurationViewModel.CorrectionViewModel.UpdateCorrection(correction);
					NewDps.NewValue = correction.Dps;
					NewDps.Difference = correction.Dps - _profile.Dps;
					OnPropertyChanged(nameof(NewDps));
				}
			);
			Mouse.OverrideCursor = null;
		}

		private async void CalculateDpsAndCorrectionAllSessions()
		{
			UpdateDpsAllSessions();
			Mouse.OverrideCursor = Cursors.Wait;
			await Task.Run
			(
				() =>
				{
					var corrections = _profile.Sessions.Select(session => DpsUtils.FindHighestDpsCorrection(_profile, session)).ToList();

					var correction = new ConfigurationCorrection
					{
						AccuracyPoints = (int) Math.Round(corrections.Average(c => c.AccuracyPoints)),
						AlacrityPoints = (int) Math.Round(corrections.Average(c => c.AlacrityPoints)),
						CriticalPoints = (int) Math.Round(corrections.Average(c => c.CriticalPoints)),
						MasteryPoints = (int) Math.Round(corrections.Average(c => c.MasteryPoints)),
						PowerPoints = (int) Math.Round(corrections.Average(c => c.PowerPoints)),
						Dps = corrections.Average(c => c.Dps)
					};

					ConfigurationViewModel.CorrectionViewModel.UpdateCorrection(correction);
					NewDps.NewValue = correction.Dps;
					NewDps.Difference = correction.Dps - _profile.Dps;
					OnPropertyChanged(nameof(NewDps));
				}
			);
			Mouse.OverrideCursor = null;
		}

		private void ParseCombatLog()
		{
			var filePicker = new OpenFileDialog
			{
				Filter = "Text files (*.txt)|*.txt",
				Multiselect = true
			};

			if (filePicker.ShowDialog() == true)
			{
				var combats = new List<Combat>();

				Mouse.OverrideCursor = Cursors.Wait;
				foreach (var filePath in filePicker.FileNames)
				{
					try
					{
						combats.AddRange(CombatLog.Load(filePath).Combats);
					}
					catch (Exception e)
					{
						MessageBox.Show($"Error parsing combat log: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					}
				}

				Mouse.OverrideCursor = null;

				if (combats.Count == 0)
				{
					MessageBox.Show("File doesn't contain any combat.", "Parse combat log", MessageBoxButton.OK, MessageBoxImage.Information);
					return;
				}

				try
				{
					var sessions = combats.Where(c => c.Duration > 120 || c.Targets.Max(t => t.DamageDone > 1000000)).Select(c => c.ToSession(_profile.DefaultValues)).ToList();

					if (sessions.Count == 0)
					{
						MessageBox.Show("File doesn't contain any suitable combat session (duration >= 120s and/or damage >= 1 000 000).", "Parse combat log", MessageBoxButton.OK, MessageBoxImage.Information);
						return;
					}

					HasUnsavedChanges = true;

					foreach (var session in sessions)
					{
						_profile.Sessions.Add(session);
						var sessionViewModel = new SessionViewModel(session, _profile.Configuration, _profile.DefaultValues);
						sessionViewModel.SessionUpdated += () =>
						{
							ConfigurationViewModel.CorrectionViewModel.IsOutdated = true;
							UpdateDps();
						};
						SessionViewModels.Add(sessionViewModel);
					}

					if (ActiveSessionViewModel == null)
						ActiveSessionViewModel = SessionViewModels.FirstOrDefault();
				}
				catch (Exception e)
				{
					MessageBox.Show($"Error parsing combat log: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}
	}
}
