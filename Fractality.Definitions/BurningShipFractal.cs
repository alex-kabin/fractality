using System;
using System.ComponentModel.Composition;
using System.Numerics;
using Fractality.Core;

namespace Fractality.Definitions
{
	[Export(typeof(IFractalDefinition))]
	[ExportMetadata("Name", "Burning Ship")]
	public class BurningShipFractal : IFractalDefinition
	{
		public double Power { get; set; }

		public double BailoutRadius { get; set; }

		public BurningShipFractal()
		{
			Power = 2;
			BailoutRadius = 100000;
		}

		public ComplexVector Init(IPointContext c)
		{
			return new ComplexVector(Complex.Zero);
		}

		public void Iterate(IPointContext c)
		{
			c.Z[0] = Complex.Pow(new Complex(Math.Abs(c.Z[0].Real), Math.Abs(c.Z[0].Imaginary)), Power) + c.Point;
		}

		public bool Bailout(IPointContext c)
		{
			return Complex.Abs(c.Z[0]) > BailoutRadius;
		}

		public Complex InitialMin
		{
			get { return new Complex(-2, -2); }
		}

		public Complex InitialMax
		{
			get { return new Complex(1.2, 1.2); }
		}	
	}
}