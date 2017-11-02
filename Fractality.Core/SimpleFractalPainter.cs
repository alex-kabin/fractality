using System;
using System.Collections.Generic;
using System.Numerics;

namespace Fractality.Core
{
	public class SimpleFractalPainter : FractalPainterBase
	{
		public override ImageColor GetPointColor(IPointContext context, int maxIterationsCount)
		{
			if(context.N < maxIterationsCount)
				return ImageColor.White;

			return ImageColor.Black;
		}
	}
}