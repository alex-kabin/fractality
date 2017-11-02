using System;
using System.Numerics;

namespace Fractality.Core
{
	public class EuclideanComplexMetric : IMetric<Complex>
	{
		public double GetDistance(Complex op1, Complex op2)
		{
			return Complex.Abs(op1 - op2);
		}
	}
}