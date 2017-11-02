using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Numerics;
using Fractality.Core;

namespace Fractality.Definitions
{
	[Export(typeof(IFractalDefinition))]
	[ExportMetadata("Name", "Julia")]
	public class JuliaFractal : IFractalDefinition
	{
		[DisplayName("Power")]
		public double Power { get; set; }

		public double EscapeRadius { get; set; }
		
		public Complex C { get; set; }

		public JuliaFractal()
		{
			Power = 2;
			EscapeRadius = 4;
			C = Complex.ImaginaryOne;
		}

		public ComplexVector Init(IPointContext c)
		{
			return new ComplexVector(c.Point);
		}

		public void Iterate(IPointContext c)
		{
			c.Z[0] = Complex.Pow(c.Z[0], Power) + C;
		}

		public bool Bailout(IPointContext c)
		{
			return c.Z[0].Real*c.Z[0].Real + c.Z[0].Imaginary*c.Z[0].Imaginary > EscapeRadius*EscapeRadius;
		}

		public Complex InitialMin
		{
			get { return new Complex(-1.4, -1.3); }
		}

		public Complex InitialMax
		{
			get { return new Complex(1.4, 1.3); }
		}
	}
}