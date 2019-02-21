using System;
using System.Windows;

namespace Swtor.Dps.StatOptimizer.Controls
{
	/// <summary>
	/// Interaction logic for LabeledTextBox.xaml
	/// </summary>
	public partial class LabeledTextBox
	{
		public bool IsReadOnly
		{
			get => (bool) GetValue(IsReadOnlyProperty);
			set => SetValue(IsReadOnlyProperty, value);
		}
		public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(LabeledTextBox), new FrameworkPropertyMetadata(false));

		public double LabelWidth
		{
			get => (double) GetValue(LabelWidthProperty);
			set => SetValue(LabelWidthProperty, value);
		}
		public static readonly DependencyProperty LabelWidthProperty = DependencyProperty.Register("LabelWidth", typeof(double), typeof(LabeledTextBox), new FrameworkPropertyMetadata(Double.NaN));

		public double InputWidth
		{
			get => (double) GetValue(InputWidthProperty);
			set => SetValue(InputWidthProperty, value);
		}
		public static readonly DependencyProperty InputWidthProperty = DependencyProperty.Register("InputWidth", typeof(double), typeof(LabeledTextBox), new FrameworkPropertyMetadata(Double.NaN));

		public int MaxLength
		{
			get => (int) GetValue(MaxLengthProperty);
			set => SetValue(MaxLengthProperty, value);
		}
		public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(int), typeof(LabeledTextBox), new FrameworkPropertyMetadata(0));

		public string Label
		{
			get => (string) GetValue(LabelProperty);
			set => SetValue(LabelProperty, value);
		}
		public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(LabeledTextBox));

		public string Text
		{
			get => (string) GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(LabeledTextBox));

		public DataType DataType
		{
			get => (DataType) GetValue(DataTypeProperty);
			set => SetValue(DataTypeProperty, value);
		}
		public static readonly DependencyProperty DataTypeProperty =
			DependencyProperty.Register("DataType", typeof(DataType), typeof(LabeledTextBox), new UIPropertyMetadata(DataType.String));

		public LabeledTextBox()
		{
			HorizontalAlignment = HorizontalAlignment.Left;
			InitializeComponent();
		}
	}
}
