using System;
using System.Collections.Generic;
using Fractality.Core;
using Fractality.Common;

namespace Fractality
{
	public class AppViewModel : ViewModel
	{
		public string AppTitle { get { return ApplicationInfo.ProductName; } }

		private IEnumerable<Lazy<IFractalDefinition, IComponentMetadata>> _fractals;
		public IEnumerable<Lazy<IFractalDefinition, IComponentMetadata>> Fractals
		{
			get { return _fractals; }
			set { _fractals = value; RaisePropertyChanged(() => Fractals); }
		}

		private IEnumerable<Lazy<IFractalPainter, IComponentMetadata>> _painters;
		public IEnumerable<Lazy<IFractalPainter, IComponentMetadata>> Painters
		{
			get { return _painters; }
			set { _painters = value; RaisePropertyChanged(() => Painters); }
		}

		private IFractalDefinition _selectedFractal;
		public IFractalDefinition SelectedFractal
		{
			get { return _selectedFractal; }
			set
			{
				_selectedFractal = value;
				RaisePropertyChanged(() => SelectedFractal);
				RaisePropertyChanged(() => CanBuild);
			}
		}

		private IFractalPainter _selectedPainter;
		public IFractalPainter SelectedPainter
		{
			get { return _selectedPainter; }
			set
			{
				_selectedPainter = value;
				RaisePropertyChanged(() => SelectedPainter);
				RaisePropertyChanged(() => CanBuild);
			}
		}

		public bool CanBuild
		{
			get { return _selectedPainter != null && _selectedFractal != null; }
		}
	}
}