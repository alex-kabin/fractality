using System;

namespace Fractality.Core
{
	public struct Interval<T> where T : IComparable<T>
	{
		private readonly T _min;
		public T Min
		{
			get { return _min; }
		}

		private readonly T _max;
		public T Max
		{
			get { return _max; }
		}

		public Interval(T min, T max)
		{
			if (min.CompareTo(max) <= 0)
			{
				_min = min;
				_max = max;
			}
			else
			{
				_max = min;
				_min = max;
			}
		}

		public bool Contains(Interval<T> other)
		{
			return Min.CompareTo(other.Min) <= 0 && Max.CompareTo(other.Max) >= 0;
		}
	}
}