using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace Fractality.Core
{
	public class FractalBuilder
	{
		private Complex _origin;
		private double _xratio;
		private double _yratio;

		private int _maxIterationsCount = 20;
		public int MaxIterationsCount
		{
			get { return _maxIterationsCount; }
			set
			{
				Contract.Requires(value > 0);
				_maxIterationsCount = value;
			}
		}

		public IImage Image { get; set; }

		public IFractalDefinition Fractal { get; set; }

		public FractalBuilder()
		{
		}

		public FractalBuilder(IFractalPainter painter)
		{
			Painter = painter;
		}

		private IFractalPainter _painter;
		public IFractalPainter Painter
		{
			get { return _painter; }
			set
			{
				Contract.Requires(value != null);
				_painter = value;
			}
		}

		protected static IEnumerable<ImagePoint> EnumerateAreaBlocks(ImageArea area, int blockSize, bool isFirstPass)
		{
			Contract.Requires(blockSize > 0);

			for (int y = area.YMin; y < area.YMax; y += blockSize)
				for (int x = area.XMin; x < area.XMax; x += blockSize)
				{
					if (isFirstPass)
						yield return new ImagePoint(x, y);
					else
					{
						var bs = blockSize*2;
						if ((x - area.XMin)%bs > 0 || (y-area.YMin)%bs > 0)
							yield return new ImagePoint(x, y);
					}
				}
		}

		private void DrawPoint(ImagePoint point, CancellationToken cancellationToken, int blockSize = 1)
		{
			var color = CalculatePointColor(point, cancellationToken);

			if(blockSize <= 1)
				Image[point] = color;
			else
			{
				int maxY = point.Y + blockSize;
				int maxX = point.X + blockSize;
				for (int y = point.Y; y < maxY && y < Image.Height; y++)
					for (int x = point.X; x < maxX && x < Image.Width; x++)
						Image[new ImagePoint(x, y)] = color;
			}
		}

		protected virtual ImageColor CalculatePointColor(ImagePoint point, CancellationToken cancellationToken)
		{
			Complex c = _origin + new Complex(_xratio * point.X, _yratio * point.Y);
			var pointContext = new PointContext() { N = 0, Point = c };

			pointContext.Z = Fractal.Init(pointContext); // Z0
			
			int vectorSize = pointContext.Z.Size;
			pointContext.DZ = new ComplexVector(vectorSize);

			Painter.OnInit(pointContext);

			ComplexVector temp;
			do
			{
				cancellationToken.ThrowIfCancellationRequested();

				temp = new ComplexVector(pointContext.Z);
				Fractal.Iterate(pointContext);

				for (int i = 0; i < vectorSize; i++)
					pointContext.DZ[i] = pointContext.Z[i] - temp[i];
				
				pointContext.N += 1;

				Painter.OnIteration(pointContext);
			}
			while (pointContext.N <= MaxIterationsCount && !Fractal.Bailout(pointContext));

			return Painter.GetPointColor(pointContext, MaxIterationsCount);
		}

		protected virtual void Initialize(Complex cmin, Complex cmax)
		{
			Contract.Requires(cmin.Real < cmax.Real && cmin.Imaginary < cmax.Imaginary);

			_origin = cmin;
			_xratio = (cmax.Real - cmin.Real) / Image.Width;
			_yratio = (cmax.Imaginary - cmin.Imaginary) / Image.Height;
		}

		private void ThrowIfNotInitialized()
		{
			if (_xratio == 0 || _yratio == 0)
				throw new InvalidOperationException("Builder is not initialized");
		}

		#region Build
		public void Build()
		{
			Build(Fractal.InitialMin, Fractal.InitialMax, CancellationToken.None);
		}

		public void Build(Complex cmin, Complex cmax)
		{
			Build(cmin, cmax, CancellationToken.None);
		}

		public void Build(Complex cmin, Complex cmax, CancellationToken cancellationToken)
		{
			BuildArea(cmin, cmax, new ImageArea(0, 0, Image.Width, Image.Height), cancellationToken);
		}

		public void BuildArea(Complex cmin, Complex cmax, ImageArea area, CancellationToken cancellationToken)
		{
			Initialize(cmin, cmax);
			Image.Lock();
			try
			{
				Parallel.ForEach(EnumerateAreaBlocks(area, 1, true),
				                 new ParallelOptions()
				                 	{
				                 		MaxDegreeOfParallelism = Environment.ProcessorCount,
				                 		CancellationToken = cancellationToken
				                 	},
				                 point => DrawPoint(point, cancellationToken));
			}
			finally
			{
				Image.Unlock();
			}
		}
		#endregion // Build

		#region BuildAsync
		public void BuildAsync(Complex cmin, Complex cmax, CancellationToken cancellationToken, Action<Exception> callback)
		{
			BuildLevelAsync(cmin, cmax, 0, true, cancellationToken, callback);
		}

		public void BuildLevelAsync(Complex cmin, Complex cmax, int level, bool isFirstPass, CancellationToken cancellationToken, Action<Exception> callback)
		{
			BuildLevelAreaAsync(cmin, cmax, new ImageArea(0, 0, Image.Width, Image.Height), level, isFirstPass, cancellationToken,
			                    callback);
		}

		public void BuildAreaAsync(Complex cmin, Complex cmax, ImageArea area, CancellationToken cancellationToken, Action<Exception> callback)
		{
			BuildLevelAreaAsync(cmin, cmax, area, 0, true, cancellationToken, callback);
		}

		public void BuildLevelAreaAsync(Complex cmin, Complex cmax, ImageArea area, int level, bool isFirstPass, CancellationToken cancellationToken, Action<Exception> callback)
		{
			Contract.Requires(level >= 0);

			Initialize(cmin, cmax);

			var asyncOperation = AsyncOperationManager.CreateOperation(null);

			int blockSize = 1 << level;
			Action action = delegate
			                	{
			                		Parallel.ForEach(EnumerateAreaBlocks(area, blockSize, isFirstPass),
			                		                 new ParallelOptions
			                		                 	{
			                		                 		MaxDegreeOfParallelism = Environment.ProcessorCount,
			                		                 		CancellationToken = cancellationToken
			                		                 	},
			                		                 point => DrawPoint(point, cancellationToken, blockSize));
			                	};

			Image.Lock();
			action.BeginInvoke(delegate(IAsyncResult result)
			                   	{
			                   		Exception exception = null;
			                   		try
			                   		{
			                   			action.EndInvoke(result);
			                   		}
			                   		catch (Exception ex)
			                   		{
			                   			exception = ex;
			                   		}

			                   		asyncOperation.PostOperationCompleted(
			                   			delegate
			                   				{
			                   					Image.Unlock();
			                   					callback(exception);
			                   				},
			                   			null);
			                   	},
			                   null);
		}
		#endregion // BuildAsync
	}
}