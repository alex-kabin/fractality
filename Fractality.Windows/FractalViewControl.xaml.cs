using System;
using System.Collections.Generic;
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
using Fractality.Core;

namespace Fractality.Windows
{
	public partial class FractalViewControl
	{
		#region Cmin property
		public Complex Cmin
		{
			get { return (Complex)GetValue(CminProperty); }
			private set { SetValue(CminProperty, value); }
		}

		private static readonly DependencyProperty CminProperty =
			DependencyProperty.Register("Cmin",
			                            typeof(Complex),
			                            typeof(FractalViewControl),
			                            new UIPropertyMetadata(Complex.Zero, OnExtentPropertyChanged));
		#endregion

		#region Cmax property
		public Complex Cmax
		{
			get { return (Complex)GetValue(CmaxProperty); }
			private set { SetValue(CmaxProperty, value); }
		}

		private static readonly DependencyProperty CmaxProperty =
			DependencyProperty.Register("Cmax",
			                            typeof(Complex),
			                            typeof(FractalViewControl),
			                            new UIPropertyMetadata(Complex.Zero, OnExtentPropertyChanged));
		#endregion

		private static void OnExtentPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			var control = sender as FractalViewControl;
			if (control != null)
			{
				control._needExtentAdjustment = true;
			}
		}

		public double ZoomFactor { get; set; }

		public bool IsInteractive { get; set; }

		#region MaxIterationsCount property
		public int MaxIterationsCount
		{
			get { return (int)GetValue(MaxIterationsCountProperty); }
			set { SetValue(MaxIterationsCountProperty, value); }
		}

		public static readonly DependencyProperty MaxIterationsCountProperty =
			DependencyProperty.Register("MaxIterationsCount",
			                            typeof(int), 
										typeof(FractalViewControl),
			                            new UIPropertyMetadata(20, OnMaxIterationsCountPropertyChanged));
		
		private static void OnMaxIterationsCountPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			var control = sender as FractalViewControl;
			if (control != null)
			{
				control._fractalBuilder.MaxIterationsCount = (int)e.NewValue;
			}
		}
		#endregion
		
		#region Definition property
		public IFractalDefinition Definition
		{
			get { return (IFractalDefinition)GetValue(DefinitionProperty); }
			set { SetValue(DefinitionProperty, value); }
		}

		public static readonly DependencyProperty DefinitionProperty =
			DependencyProperty.Register("Definition",
			                            typeof(IFractalDefinition),
			                            typeof(FractalViewControl),
			                            new UIPropertyMetadata(null, OnDefinitionPropertyChanged));

		private static void OnDefinitionPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			var control = sender as FractalViewControl;
			if (control != null)
			{
				var fractalDefinition = e.NewValue as IFractalDefinition;
				control._fractalBuilder.Fractal = fractalDefinition;
				if (fractalDefinition != null)
				{
					control.Cmin = fractalDefinition.InitialMin;
					control.Cmax = fractalDefinition.InitialMax;
				}
			}
		}
		#endregion
		
		#region Painter property
		public IFractalPainter Painter
		{
			get { return (IFractalPainter)GetValue(PainterProperty); }
			set { SetValue(PainterProperty, value); }
		}

		public static readonly DependencyProperty PainterProperty =
			DependencyProperty.Register("Painter", typeof(IFractalPainter), typeof(FractalViewControl),
			new UIPropertyMetadata(null, OnPainterPropertyChanged));

		private static void OnPainterPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			var control = sender as FractalViewControl;
			if (control != null)
				control._fractalBuilder.Painter = e.NewValue as IFractalPainter;
		} 
		#endregion

		private bool IsBusy;
		private bool _needExtentAdjustment;

		private readonly FractalBuilder _fractalBuilder;
		private CancellationTokenSource _cancellationTokenSource;

		private WriteableBitmapImage _bitmap1;
		private WriteableBitmapImage _bitmap2;

		public FractalViewControl()
		{
			InitializeComponent();
			_fractalBuilder = new FractalBuilder();
			ZoomFactor = 1.3;
			IsInteractive = true;
		}

		private Image BackImage
		{
			get { return image1.Visibility == Visibility.Visible ? image2 : image1; }
		}

		private Image FrontImage
		{
			get { return image1.Visibility == Visibility.Visible ? image1 : image2; }
		}

		private WriteableBitmapImage BackBitmap
		{
			get { return image1.Visibility == Visibility.Visible ? _bitmap2: _bitmap1; }
		}

		private WriteableBitmapImage FrontBitmap
		{
			get { return image1.Visibility == Visibility.Visible ? _bitmap1 : _bitmap2; }
		}

		private void AdjustBitmapSizeIfNecessary()
		{
			var imageWidth = (int)canvas.ActualWidth;
			var imageHeight = (int)canvas.ActualHeight;

			if (_bitmap1 == null || _bitmap1.Width != imageWidth || _bitmap1.Height != imageHeight)
			{
				_bitmap1 = new WriteableBitmapImage(imageWidth, imageHeight);
				image1.Source = _bitmap1.BitmapSource;
				_bitmap2 = new WriteableBitmapImage(imageWidth, imageHeight);
				image2.Source = _bitmap2.BitmapSource;

				_needExtentAdjustment = true;
			}
		}

		private void SwapImages()
		{
			var backImage = BackImage;
			var frontImage = FrontImage;

			Canvas.SetTop(backImage, 0);
			Canvas.SetLeft(backImage, 0);
			frontImage.Visibility = Visibility.Hidden;
			backImage.Visibility = Visibility.Visible;
		}

		private void AdjustExtentIfNecessary()
		{
			if (_needExtentAdjustment)
			{
				var cw = Cmax.Real - Cmin.Real;
				var ch = Cmax.Imaginary - Cmin.Imaginary;
				var d = cw/canvas.ActualWidth - ch/canvas.ActualHeight;

				Complex o = new Complex(d < 0 ? -canvas.ActualWidth*d/2 : 0, d > 0 ? canvas.ActualHeight*d/2 : 0);

				Cmin = Cmin - o;
				Cmax = Cmax + o;
				_needExtentAdjustment = false;
			}
		}

		public void Refresh()
		{
			AdjustBitmapSizeIfNecessary();
			AdjustExtentIfNecessary();
			
			_fractalBuilder.Image = BackBitmap;
			_fractalBuilder.Build(Cmin, Cmax, CancellationToken.None);

			SwapImages();
		}

		private int _level;

		public void RefreshAsync()
		{
			AdjustBitmapSizeIfNecessary();
			AdjustExtentIfNecessary();
			
			IsBusy = true;
			_level = 0;
			_fractalBuilder.Image = BackBitmap;
			_fractalBuilder.BuildLevelAsync(Cmin, Cmax, _level, true, CancellationToken.None,
			                                OnLevelBuilt);
		}

		private void OnLevelBuilt(Exception ex)
		{
			SwapImages();
			//if (!(ex is OperationCanceledException))
			//{
			//    if(_level == 4)
			//    {
			//        SwapImages();
			//    }
			//    _level--;
			//    if (_level >= 0)
			//    {
			//        //Thread.Sleep(2000);
			//        _fractalBuilder.BuildLevelAsync(Cmin, Cmax, _level, false, CancellationToken.None, OnLevelBuilt);
			//        return;
			//    }
			//}
			IsBusy = false;
		}

		public void Reset()
		{
			Cmin = _fractalBuilder.Fractal.InitialMin;
			Cmax = _fractalBuilder.Fractal.InitialMax;
			//AdjustExtent(_fractalBuilder.Fractal.InitialMin, _fractalBuilder.Fractal.InitialMax);
			RefreshAsync();
		}

		private void Zoom(Point centerPoint, int power)
		{
			var extentWidth = Cmax.Real - Cmin.Real;
			var extentHeight = Cmax.Imaginary - Cmin.Imaginary;
			var center = new Complex(Cmin.Real + centerPoint.X*extentWidth/canvas.ActualWidth,
			                        Cmin.Imaginary + centerPoint.Y*extentHeight/canvas.ActualHeight);

			var z = Math.Pow(ZoomFactor, power);
			Complex offset = new Complex(extentWidth/z/2, extentHeight/z/2);

			Cmin = center - offset;
			Cmax = center + offset;
			
			RefreshAsync();
		}

		private void Pan(Vector v)
		{
			var extentWidth = Cmax.Real - Cmin.Real;
			var extentHeight = Cmax.Imaginary - Cmin.Imaginary;
			var offset = new Complex(v.X * extentWidth / canvas.ActualWidth, v.Y * extentHeight / canvas.ActualHeight);

			Cmin += offset;
			Cmax += offset;

			RefreshAsync();
		}

		private void ZoomIn(Point centerPoint)
		{
			Zoom(centerPoint, 1);
		}

		private void ZoomOut(Point centerPoint)
		{
			Zoom(centerPoint, -1);
		}

		private Point _clickPoint;
		private bool _moved;
		private void image_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (IsInteractive)
			{
				_clickPoint = e.GetPosition(canvas);
				_moved = false;
			}
		}

		private void image_MouseMove(object sender, MouseEventArgs e)
		{
			if (IsInteractive)
			{
				if (e.LeftButton == MouseButtonState.Pressed)
				{
					var currentPoint = e.GetPosition(canvas);
					Canvas.SetLeft(FrontImage, currentPoint.X - _clickPoint.X);
					Canvas.SetTop(FrontImage, currentPoint.Y - _clickPoint.Y);
					_moved = true;
				}
			}
		}

		private void image_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if (IsInteractive)
			{
				var currentPoint = e.GetPosition(canvas);
				if (_moved)
				{
					Pan(Point.Subtract(_clickPoint, currentPoint));
				}
				else
				{
					if (e.ChangedButton == MouseButton.Left)
						ZoomIn(currentPoint);
					else
						ZoomOut(currentPoint);
				}
			}
		}
	}
}
