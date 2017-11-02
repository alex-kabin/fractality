using System.Collections.Generic;
using System.Numerics;

namespace Fractality.Core
{
	public interface IFractalPainter
	{
		void OnInit(IPointContext context);

		void OnIteration(IPointContext context);

		ImageColor GetPointColor(IPointContext context, int maxIterationsCount);
	}
} 