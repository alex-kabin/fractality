using System.ComponentModel.Composition;
using System.Numerics;
using Fractality.Core;
using Fractality.Utils;

namespace Fractality.Definitions
{
	[Export(typeof(IFractalDefinition))]
	[ExportMetadata("Name", "Nova")]
	public class NovaFractal : IFractalDefinition
	{
		public double BailoutRadius { get; set; }

		public Complex Power { get; set; }

		public Complex R { get; set; }

		public Complex Z0 { get; set; }

		public NovaFractal()
		{
			Power = 2;
			BailoutRadius = 0.0000001;
			R = 1;
			Z0 = -1;
		}

		public ComplexVector Init(IPointContext c)
		{
			return new ComplexVector(Z0);
		}

		public void Iterate(IPointContext c)
		{
			c.Z[0] = c.Z[0] - R*((Complex.Pow(c.Z[0], Power) - 1)/(Power*Complex.Pow(c.Z[0], Power - 1))) + c.Point;
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