using System;
using System.Numerics;

namespace Fractality.Core
{
	public class FractalDefinition : IFractalDefinition
	{
		public Func<IPointContext, ComplexVector> Init;

		public Action<IPointContext> Iterate;

		public Func<IPointContext, bool> Bailout;
		
		public FractalDefinition()
		{
		}

		public FractalDefinition(Complex initialMin, Complex initialMax)
		{
			InitialMin = initialMin;
			InitialMax = initialMax;
		}

		ComplexVector IFractalDefinition.Init(IPointContext c)
		{
			return Init(c);
		}
		
		void IFractalDefinition.Iterate(IPointContext c)
		{
			Iterate(c);
		}
		
		bool IFractalDefinition.Bailout(IPointContext c)
		{
			return Bailout(c);
		}

		public Complex InitialMin
		{
			get; private set;
		}

		public Complex InitialMax
		{
			get; private set;
		}
	}
}