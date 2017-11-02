using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Numerics;
using Fractality.Core;

namespace Fractality.Painters
{
	[Export(typeof(IFractalPainter))]
	[ExportMetadata("Name", "Exponent Smooth")]
	public class ExponentSmoothPainter : IFractalPainter
	{
		private const string VAR_PREFIX = "PAINTER";

		[DisplayName("ColorCycle")]
		public int ColorCycle { get; set; }

		public IPalette Palette { get; set; }

		public bool Inverse { get; set; }

		public ExponentSmoothPainter()
		{
			ColorCycle = 10;
			Palette = LinearGradientPalette.Rainbow;
		}

		private double GetValue(IPointContext c)
		{
			Complex p = Inverse ? (c.N > 0 ?  1/c.DZ[0] : 0) : c.Z[0];
			return Math.Exp(-Complex.Abs(p));
		}

		public void OnInit(IPointContext context)
		{
			context.Real[VAR_PREFIX, "sum"] = GetValue(context);
		}

		public void OnIteration(IPointContext context)
		{
			context.Real[VAR_PREFIX, "sum"] = context.Real[VAR_PREFIX, "sum"] + GetValue(context);
		}

		public ImageColor GetPointColor(IPointContext context, int maxIterationsCount)
		{
			var i = context.N;
			var color = new ImageColor() { A = 255 };

			if (i < maxIterationsCount)
			{
				var distance = context.Real[VAR_PREFIX, "sum"];
				double hue = 1 - (distance % ColorCycle) / ColorCycle;
				color = Palette.GetColor(hue);
			}
			return color;
		}
	}
}