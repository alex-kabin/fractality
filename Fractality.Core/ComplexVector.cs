using System;
using System.Numerics;

namespace Fractality.Core
{
	public class ComplexVector : Vector<Complex>
	{
		public ComplexVector(int size) : base(size) { }

		public ComplexVector(int size, Complex initialValue) : base(size, initialValue) { }

		public ComplexVector(ComplexVector v) : base(v) { }

		public ComplexVector(params Complex[] values) : base(values) { }

		public static ComplexVector operator +(ComplexVector op1, ComplexVector op2)
		{
			if (op1.Size != op2.Size)
				throw new InvalidOperationException("Vectors should have the same size");

			var size = op1.Size;
			var newVector = new ComplexVector(size);
			for (int i = 0; i < size; i++)
				newVector[i] = op1[i] + op2[i];
			return newVector;
		}

		public static ComplexVector operator +(ComplexVector op1, DoubleVector op2)
		{
			if (op1.Size != op2.Size)
				throw new InvalidOperationException("Vectors should have the same size");

			var size = op1.Size;
			var newVector = new ComplexVector(size);
			for (int i = 0; i < size; i++)
				newVector[i] = op1[i] + op2[i];
			return newVector;
		}

		public static ComplexVector operator -(ComplexVector op1, ComplexVector op2)
		{
			if (op1.Size != op2.Size)
				throw new InvalidOperationException("Vectors should have the same size");

			var size = op1.Size;
			var newVector = new ComplexVector(size);
			for (int i = 0; i < size; i++)
				newVector[i] = op1[i] - op2[i];
			return newVector;
		}

		public static ComplexVector operator -(ComplexVector op1, DoubleVector op2)
		{
			if (op1.Size != op2.Size)
				throw new InvalidOperationException("Vectors should have the same size");

			var size = op1.Size;
			var newVector = new ComplexVector(size);
			for (int i = 0; i < size; i++)
				newVector[i] = op1[i] - op2[i];
			return newVector;
		}
	}
}