using System.ComponentModel.Composition;
using System.Numerics;
using Fractality.Core;
using Fractality.Utils;

namespace Fractality.Definitions
{
	[Export(typeof(IFractalDefinition))]
	[ExportMetadata("Name", "Newton")]
	public class NewtonFractal : IFractalDefinition
	{
		public double BailoutRadius { get; set; }

		public int Power { get; set; }

		public Complex A { get; set; }

		public NewtonFractal()
		{
			Power = 3;
			BailoutRadius = 0.0000001;
			A = 1;
		}

		public ComplexVector Init(IPointContext c)
		{
			return new ComplexVector(c.Point);
		}

		public void Iterate(IPointContext c)
		{
			c.Z[0] = c.Z[0] - A*((Complex.Pow(c.Z[0], Power) - 1)/(Power*Complex.Pow(c.Z[0], Power - 1)));
		}

		public bool Bailout(IPointContext c)
		{
			return Complex.Abs(c.DZ[0]) < BailoutRadius;
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