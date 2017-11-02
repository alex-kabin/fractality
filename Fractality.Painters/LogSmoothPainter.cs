using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Numerics;
using Fractality.Core;

namespace Fractality.Painters
{
	[Export(typeof(IFractalPainter))]
	[ExportMetadata("Name", "Log Smooth")]
	public class LogSmoothPainter : FractalPainterBase
	{
		[DisplayName("Power")]
		public double Power { get; set; }

		[DisplayName("Radius")]
		public double BailoutRadius { get; set; }

		[DisplayName("ColorCycle")]
		public int ColorCycle { get; set; }

		public IPalette Palette { get; set; }

		public LogSmoothPainter()
		{
			Power = 2;
			ColorCycle = 10;
			BailoutRadius = 100000;
			Palette = LinearGradientPalette.Rainbow;
		}

		public override ImageColor GetPointColor(IPointContext context, int maxIterationsCount)
		{
			var i = context.N;
			
			var color = new ImageColor() { A = 255 };
			if (i < maxIterationsCount)
			{
				
				var zn = Complex.Abs(context.Z[0]);
				var distance = i - Math.Log(Math.Log(zn) / Math.Log(BailoutRadius), Power);
				
				// var logp = Math.Log(Power);
				//var distance = i - Math.Log(Math.Log(zn) / logp) / logp;

				double hue = 1 - (distance % ColorCycle) / ColorCycle;

				color = Palette.GetColor(hue);
			}
			return color;
		}
	}
}