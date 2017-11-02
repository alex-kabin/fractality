using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Numerics;
using Fractality.Core;

namespace Fractality.Painters
{
	[Export(typeof(IFractalPainter))]
	[ExportMetadata("Name", "Zebra")]
	public class ZebraPainter : FractalPainterBase
	{
		[DisplayName("Invert")]
		public bool Invert { get; set; }

		public override ImageColor GetPointColor(IPointContext context, int maxIterationsCount)
		{
			int t = 255*(context.N % 2);
			if (Invert)
				t = 255 - t;

			return new ImageColor()
			{
				A = 255,
				R = (byte)(t),
				G = (byte)(t),
				B = (byte)(t),
			};
		}
	}
}