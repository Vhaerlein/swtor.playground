using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using TorSimulator.StatOptimizer.Annotations;

namespace Swtor.Dps.StatOptimizer.Controls
{
	/// <summary>
	/// Delayed text box, updates its value with specified delay.
	/// </summary>
	public class DelayedTextBox : TextBox, INotifyPropertyChanged
	{
		private static readonly Regex IntegerRegex = new Regex("^[0-9]*$", RegexOptions.Compiled);
		private static readonly Regex DoubleRegex = new Regex(@"^[0-9]*(\.)?[0-9]*$", RegexOptions.Compiled);

		public const int DefaultDelayTime = 500;

		private readonly Timer _delayTimer;
		private bool _timerElapsed;
		private bool _keysPressed;
		private TextChangedEventArgs _previousTextChangedEventArgs;

		/// <summary>
		/// Update value delay timer.
		/// Default value is 250ms.
		/// </summary>
		public int Timer
		{
			get { return (int) GetValue(TimerProperty); }
			set { SetValue(TimerProperty, value); }
		}
		public static readonly DependencyProperty TimerProperty =
			DependencyProperty.Register("Timer", typeof(int), typeof(DelayedTextBox), new UIPropertyMetadata(DefaultDelayTime));


		public DataType DataType
		{
			get { return (DataType) GetValue(DataTypeProperty); }
			set { SetValue(DataTypeProperty, value); }
		}
		public static readonly DependencyProperty DataTypeProperty = 
			DependencyProperty.Register("DataType", typeof (DataType), typeof (DelayedTextBox), new UIPropertyMetadata (DataType.String));

		public DelayedTextBox()
		{
			_delayTimer = new Timer(Timer);
			_delayTimer.Elapsed += DelayTimerElapsed;

			PreviewTextInput += OnPreviewTextInput;
			DataObject.AddPastingHandler(this, OnPasting);

			DependencyPropertyDescriptor prop = DependencyPropertyDescriptor.FromProperty(TimerProperty, typeof(DelayedTextBox));
			prop.AddValueChanged(this, delegate
			{
				_delayTimer.Interval = Timer;
			});

			AddHandler(PreviewKeyDownEvent, new KeyEventHandler(PreviewKeyDownHandler));
		}

		private delegate void DelayedTextChangedHandler();
		private void DelayTimerElapsed(object sender, ElapsedEventArgs e)
		{
			_delayTimer.Enabled = false;
			_timerElapsed = true;
			Dispatcher.Invoke(new DelayedTextChangedHandler(DelayTextChanged), null);
		}

		private void PreviewKeyDownHandler(object sender, KeyEventArgs e)
		{
			if (!_delayTimer.Enabled)
				_delayTimer.Enabled = true;
			else
			{
				_delayTimer.Enabled = false;
				_delayTimer.Enabled = true;
			}

			_keysPressed = true;
		}

		private void DelayTextChanged()
		{
			if (_previousTextChangedEventArgs != null)
				OnTextChanged(_previousTextChangedEventArgs);
		}

		protected override void OnTextChanged(TextChangedEventArgs e)
		{
			if (_timerElapsed || !_keysPressed)
			{
				_timerElapsed = false;
				_keysPressed = false;
				base.OnTextChanged(e);

				BindingExpression bindingExpression = GetBindingExpression(TextProperty);
				if (bindingExpression != null && bindingExpression.Status == BindingStatus.Active)
					bindingExpression.UpdateSource();

				var handler = ValueIsUpdated;
				handler?.Invoke();
			}

			_previousTextChangedEventArgs = e;
		}

		private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (!ValidateInput(e.Text))
				e.Handled = true;
		}

		private bool ValidateInput(string input)
		{
			return !(DataType == DataType.Double && !DoubleRegex.IsMatch(input) || DataType == DataType.Integer && !IntegerRegex.IsMatch(input));
		}

		private void OnPasting(object sender, DataObjectPastingEventArgs e)
		{
			if (e.DataObject.GetDataPresent(typeof (string)))
			{
				if (!ValidateInput((string) e.DataObject.GetData(typeof (string))))
					e.CancelCommand();
			}
			else
				e.CancelCommand();
		}

		#region INotifyPropertyChanged

		public delegate void ValueIsUpdatedEventHandler();
		public event ValueIsUpdatedEventHandler ValueIsUpdated;

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}
