using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Fractality;
using Fractality.Common;
using Fractality.Windows;

namespace Fractality
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	[Export("RootWindow", typeof(IWindow))]
	[Export(typeof(IDialogService))]
	[Export(typeof(IUIMessageService))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public partial class MainWindow : IWindow, IDialogService, IUIMessageService
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		public void Show(IWindow window, object dataContext, Action callback)
		{
			Window childWindow = window as Window;
			if (childWindow != null)
			{
				childWindow.Owner = this;
				childWindow.Closed += delegate { callback(); };
				childWindow.Show();
			}
		}

		public bool? ShowModal(IWindow window, object dataContext)
		{
			Window childWindow = window as Window;
			if (childWindow != null)
			{
				childWindow.Owner = this;
				childWindow.DataContext = dataContext;
				return childWindow.ShowDialog();
			}

			return null;
		}

		public void ShowError(string text)
		{
			MessageBox.Show(this, text, Properties.Resources.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
		}

		public void ShowWarning(string text)
		{
			MessageBox.Show(this, text, Properties.Resources.WarningTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
		}

		public void ShowInfo(string text)
		{
			MessageBox.Show(this, text, Properties.Resources.InfoTitle, MessageBoxButton.OK, MessageBoxImage.Information);
		}

		public bool? ShowQuestion(string text, MessageBoxButton buttons)
		{
			var result = MessageBox.Show(this, text, Properties.Resources.QuestionTitle, buttons, MessageBoxImage.Question);
			if (result == MessageBoxResult.OK || result == MessageBoxResult.Yes)
				return true;
			if (result == MessageBoxResult.Cancel || result == MessageBoxResult.No)
				return false;

			return null;
		}

		private void btnRefresh_Click(object sender, RoutedEventArgs e)
		{
			fractalView.Refresh();
		}
	}
}
