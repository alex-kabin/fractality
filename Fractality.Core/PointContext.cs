using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Numerics;

namespace Fractality.Core
{
	internal class PointContext : IPointContext
	{
		private VariablesSet<double> _real;
		private VariablesSet<Complex> _complex;

		public Complex Point { get; set; }
		public int N { get; set; }
		public ComplexVector Z { get; set; }
		public ComplexVector DZ { get; set; }

		public VariablesSet<double> Real { get { return _real ?? (_real = new VariablesSet<double>()); } }
		public VariablesSet<Complex> Complex { get { return _complex ?? (_complex = new VariablesSet<Complex>()); } }
	}
}