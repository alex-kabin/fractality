using System;

namespace Fractality.Core
{
	public class DoubleVector : Vector<double>
	{
		public DoubleVector(int size)
			: base(size)
		{

		}

		public DoubleVector(int size, double initialValue)
			: base(size, initialValue)
		{

		}

		public static DoubleVector operator +(DoubleVector op1, DoubleVector op2)
		{
			if (op1.Size != op2.Size)
				throw new InvalidOperationException("Vectors should have the same size");

			var size = op1.Size;
			var newVector = new DoubleVector(size);
			for (int i = 0; i < size; i++)
				newVector[i] = op1[i] + op2[i];
			return newVector;
		}

		public static DoubleVector operator -(DoubleVector op1, DoubleVector op2)
		{
			if (op1.Size != op2.Size)
				throw new InvalidOperationException("Vectors should have the same size");

			var size = op1.Size;
			var newVector = new DoubleVector(size);
			for (int i = 0; i < size; i++)
				newVector[i] = op1[i] - op2[i];
			return newVector;
		}
	}
}