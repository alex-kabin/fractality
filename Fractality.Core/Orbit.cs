using System.Collections.Generic;
using System.Numerics;

namespace Fractality.Core
{
	public class Orbit<T> : List<T>
	{
		public Orbit()
		{
		}

		public Orbit(int maxSize) : base(maxSize)
		{
		}

		public T Last
		{
			get { return this[Count - 1]; }
		}

		public bool IsConvergentToPoint(IMetric<T> metric, double threshold = 1e-06)
		{
			if (Count < 2)
				return false;

			var zn = Last;
			var zn_1 = this[Count - 2];

			var distance = metric.GetDistance(zn, zn_1);
			return distance < threshold;
		}

		public bool IsConvergentToCycle(IMetric<T> metric, double threshold = 1e-06)
		{
			if (Count < 2)
				return false;

			var zn = Last;

			for (int i = 0; i < Count - 1; i++)
			{
				var z = this[i];
				var distance = metric.GetDistance(z, zn);
				if(distance < threshold)
					return true;
			}
			return false;
		}

		public bool IsDivergent(IMetric<T> metric, double bailout = 1e+06)
		{
			if (Count == 0)
				return false;

			var zn = Last;
			var zero = default(T);
			var distance = metric.GetDistance(zn, zero);
			return distance > bailout;
		}
	}
}