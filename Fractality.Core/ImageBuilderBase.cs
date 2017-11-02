using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Fractality
{
	public abstract class ImageBuilderBase : IDisposable
	{
		protected IBitmap Image { get; private set; }
		
		protected ImageBuilderBase(int width, int height, BitmapFactory bitmapFactory)
		{
			Image = bitmapFactory.CreateBitmap(width, height);
		}

		private IEnumerable<Point> EnumerateImagePoints()
		{
			for (int y = 0; y < Image.Height; y++)
				for (int x = 0; x < Image.Width; x++)
					yield return new Point() { X = x, Y = y };
		}

		protected abstract Color CalculatePointColor(Point point, CancellationToken cancellationToken);

		private void ProcessPoint(Point point, CancellationToken cancellationToken)
		{
			var color = CalculatePointColor(point, cancellationToken);
			Image[point.X, point.Y] = color;
		}

		protected virtual IBitmap Build()
		{
			var cancellationToken = CancellationToken.None;
			foreach (Point point in EnumerateImagePoints())
				ProcessPoint(point, cancellationToken);

			return Image;
		}

		protected virtual IBitmap BuildParallel(CancellationToken cancellationToken)
		{
			Parallel.ForEach(EnumerateImagePoints(), point => ProcessPoint(point, cancellationToken));
		
			return Image;
		}

		public virtual void Dispose()
		{
			Image.Dispose();
		}

	}
}