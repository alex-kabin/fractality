using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Fractality.Core;

namespace Fractality.Painters
{
	[Export(typeof(IFractalPainter))]
	[ExportMetadata("Name", "Gradient Escape Time")]
	public class GradientPainter : FractalPainterBase
	{
		[DisplayName("Red")]
		[Range(0, 1)]
		public double RedWeight { get; set; }

		[DisplayName("Green")]
		[Range(0, 1)]
		public double GreenWeight { get; set; }

		[DisplayName("Blue")]
		[Range(0, 1)]
		public double BlueWeight { get; set; }

		[DisplayName("Invert")]
		public bool Invert { get; set; }

		public GradientPainter()
		{
			RedWeight = 1;
			GreenWeight = 1;
			BlueWeight = 1;
		}

		public override ImageColor GetPointColor(IPointContext context, int maxIterationsCount)
		{
			var iterationRatio = (double)context.N/maxIterationsCount;
			double t = 255*iterationRatio;
			if (Invert)
				t = 255 - t;
			return new ImageColor()
			       	{
			       		A = 255,
			       		R = (byte)(RedWeight*t),
			       		G = (byte)(GreenWeight*t),
			       		B = (byte)(BlueWeight*t),
			       	};
		}
	}
}