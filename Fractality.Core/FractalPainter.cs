using System;
using System.Collections.Generic;
using System.Numerics;

namespace Fractality.Core
{
	public class FractalPainter : IFractalPainter
	{
		public Action<IPointContext> Init;
		public Action<IPointContext> Iteration;
		public Func<IPointContext, int, ImageColor> GetColor;

		public void OnInit(IPointContext context)
		{
			Init(context);
		}

		public void OnIteration(IPointContext context)
		{
			Iteration(context);
		}

		public ImageColor GetPointColor(IPointContext context, int maxIterationsCount)
		{
			return GetColor(context, maxIterationsCount);
		}
	}
}