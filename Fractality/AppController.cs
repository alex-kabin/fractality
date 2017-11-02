using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using Fractality.Core;
using Fractality.Common;
using NLog;

namespace Fractality
{
	[Export("RootController", typeof(IController))]
	public class AppController : IController, IPartImportsSatisfiedNotification
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		[ImportMany]
		private Lazy<IFractalDefinition, IComponentMetadata>[] ImportedDefinitions { get; set; }
		private IDictionary<string, Lazy<IFractalDefinition>> _definitions;

		[ImportMany]
		private Lazy<IFractalPainter, IComponentMetadata>[] ImportedPainters { get; set; }
		private IDictionary<string, Lazy<IFractalPainter>> _painters;

		private readonly IWindow _window;
		private readonly IDialogService _dialogService;
		private readonly IUIMessageService _uiMessageService;
		private readonly AppViewModel _viewModel;
		
		[ImportingConstructor]
		public AppController(
			[Import("RootWindow")] IWindow window,
			[Import] IUIMessageService uiMessageService,
			[Import] IDialogService dialogService)
		{
			_window = window;
			_dialogService = dialogService;
			_uiMessageService = uiMessageService;

			_viewModel = new AppViewModel() { Controller = this };
			_viewModel.PropertyChanged += viewModel_PropertyChanged;

			_window.DataContext = _viewModel;
			_window.Closing += window_Closing;
			_window.Show();
		}

		private void viewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
		}

		private void window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			
		}

		#region Exit
		public void Exit()
		{
			Application.Current.Shutdown();
		}

		private ICommand _exitCommand;
		public ICommand ExitCommand
		{
			get { return _exitCommand ?? (_exitCommand = new DelegateCommand(Exit, () => true)); }
		}
		#endregion // Exit

		#region Реализация IPartImportsSatisfiedNotification
		void IPartImportsSatisfiedNotification.OnImportsSatisfied()
		{
			_definitions = new Dictionary<string, Lazy<IFractalDefinition>>();
			foreach (var definition in ImportedDefinitions)
				_definitions.Add(definition.Metadata.Name, definition);

			_viewModel.Fractals = ImportedDefinitions;

			_painters = new Dictionary<string, Lazy<IFractalPainter>>();
			foreach (var painter in ImportedPainters)
				_painters.Add(painter.Metadata.Name, painter);

			_viewModel.Painters = ImportedPainters;
		}
		#endregion
	}
}