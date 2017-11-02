using System;
using System.Numerics;

namespace Fractality.Core
{
	public static class ClassicFractals
	{
		public static IFractalDefinition Mandelbrot =
			new FractalDefinition(new Complex(-2, -1), new Complex(1, 1))
				{
					Init = (data) => new ComplexVector(Complex.Zero),
					Iterate = (data) => { data.Z[0] = data.Z[0]*data.Z[0] + data.Point; },
					Bailout = (data) => data.Z[0].Real * data.Z[0].Real + data.Z[0].Imaginary * data.Z[0].Imaginary > 4
				};

		public static IFractalDefinition Julia =
			new FractalDefinition(new Complex(-1, -1), new Complex(1, 1))
				{
					Init = (data) => new ComplexVector(data.Point),
					Iterate = (data) => { data.Z[0] = data.Z[0]*data.Z[0] + Complex.ImaginaryOne; },
					Bailout = (data) => data.Z[0].Real*data.Z[0].Real + data.Z[0].Imaginary*data.Z[0].Imaginary > 16
				};

		public static IFractalDefinition BarnsleyTree =
			new FractalDefinition(new Complex(-2, -2), new Complex(2, 2))
			{
				Init = (data) => new ComplexVector(data.Point),
				Iterate = (data) => { data.Z[0] = new Complex(0.6, 1.1)*(data.Z[0] - (data.Z[0].Real >= 0 ? 1 : -1)); },
				Bailout = (data) => data.Z[0].Real * data.Z[0].Real + data.Z[0].Imaginary * data.Z[0].Imaginary > 4
			};
	}
}