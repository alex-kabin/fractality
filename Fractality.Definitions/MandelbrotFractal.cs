using System;
using System.ComponentModel.Composition;
using System.Numerics;
using Fractality.Core;

namespace Fractality.Definitions
{
	[Export(typeof(IFractalDefinition))]
	[ExportMetadata("Name", "Mandelbrot")]
	public class MandelbrotFractal : IFractalDefinition
	{
		public double Power { get; set; }

		public double BailoutRadius { get; set; }

		public MandelbrotFractal()
		{
			Power = 2;
			BailoutRadius = 2;
		}

		public ComplexVector Init(IPointContext c)
		{
			return new ComplexVector(Complex.Zero);
		}

		public void Iterate(IPointContext c)
		{
			c.Z[0] = Complex.Pow(c.Z[0], Power) + c.Point;
		}

		public bool Bailout(IPointContext c)
		{
			return Complex.Abs(c.Z[0]) > BailoutRadius;
		}

		public Complex InitialMin
		{
			get { return new Complex(-2.2, -1.2); }
		}

		public Complex InitialMax
		{
			get { return new Complex(0.7, 1.2); }
		}
	}
}