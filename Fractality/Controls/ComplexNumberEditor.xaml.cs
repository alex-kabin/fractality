using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using Fractality.Common;

namespace Fractality.Controls
{
	/// <summary>
	/// Interaction logic for ComplexNumberEditor.xaml
	/// </summary>
	public partial class ComplexNumberEditor : UserControl
	{
		public class ComplexViewModel : ObservableObject
		{
			private double _real;
			public double Real
			{
				get { return _real; }
				set
				{
					if (value != _real)
					{
						_real = value;
						RaisePropertyChanged(() => Real);
					}
				}
			}

			private double _imaginary;
			public double Imaginary
			{
				get { return _imaginary; }
				set
				{
					if (value != _imaginary)
					{
						_imaginary = value;
						RaisePropertyChanged(() => Imaginary);
					}
				}
			}

			public Complex Number
			{
				get { return new Complex(Real, Imaginary); }
				set
				{
					Real = value.Real;
					Imaginary = value.Imaginary;
				}
			}
		}

		private ComplexViewModel ViewModel { get; set; }

		public ComplexNumberEditor()
		{
			InitializeComponent();
			ViewModel = new ComplexViewModel();
			ViewModel.PropertyChanged += viewModel_PropertyChanged;
			DataContext = ViewModel;
		}

		private void viewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			ComplexNumber = ViewModel.Number;
		}
		
		public Complex ComplexNumber
		{
			get { return (Complex)GetValue(ComplexNumberProperty); }
			set { SetValue(ComplexNumberProperty, value); }
		}

		public static readonly DependencyProperty ComplexNumberProperty =
			DependencyProperty.Register("ComplexNumber", typeof(Complex), typeof(ComplexNumberEditor), 
			new UIPropertyMetadata(Complex.Zero, OnComplexNumberPropertyChanged));

		private static void OnComplexNumberPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			var editor = sender as ComplexNumberEditor;
			if(editor == null)
				return;

			var number = (Complex)e.NewValue;
			var viewModel = editor.ViewModel;

			viewModel.PropertyChanged -= editor.viewModel_PropertyChanged;
			viewModel.Number = number;
			viewModel.PropertyChanged += editor.viewModel_PropertyChanged;
		}
	}
}
