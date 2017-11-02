using System.Collections.Generic;
using System.Numerics;

namespace Fractality.Core
{
	public interface IPointContext
	{
		Complex Point { get; }
		int N { get; }
		ComplexVector Z { get; }
		ComplexVector DZ { get; }

		VariablesSet<double> Real { get; }
		VariablesSet<Complex> Complex { get; }
	}
}