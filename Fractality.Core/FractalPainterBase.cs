using System;

namespace Fractality.Core
{
	public abstract class FractalPainterBase : IFractalPainter
	{
		public virtual void OnInit(IPointContext context)
		{
			
		}

		public virtual void OnIteration(IPointContext context)
		{
			
		}

		public abstract ImageColor GetPointColor(IPointContext context, int maxIterationsCount);
	}
}