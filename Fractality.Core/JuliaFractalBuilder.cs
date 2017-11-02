using System.Drawing;
using System.Numerics;
using System;

namespace Fractality
{
	public class JuliaFractalBuilder :  FractalBuilderBase
	{
		private Func<Complex, Complex> _function;
		
		public JuliaFractalBuilder(Size imageSize, BitmapFactory bitmapFactory, FractalColorizerBase colorizer,
										Func<Complex, Complex> func)
			: base(imageSize, colorizer, bitmapFactory)
		{
			SetFunction(func);
			BailOutTest = DefaultTestBailOut;
		}

		public JuliaFractalBuilder(Size imageSize, BitmapFactory bitmapFactory)
			: this(imageSize, bitmapFactory, DefaultGetNextValue)
		{
		}

		public JuliaFractalBuilder(Size imageSize, BitmapFactory bitmapFactory, Func<Complex, Complex> func)
			: this(imageSize, bitmapFactory, new SimpleFractalColorizer(), func)
		{
		}

		public void SetFunction(Func<Complex, Complex> func)
		{
			_function = func;
		}

		private static Complex DefaultGetNextValue(Complex z)
		{
			return z * z + new Complex(0,1);
		}

		private static bool DefaultTestBailOut(Complex z)
		{
			return Math.Sqrt(z.Real) + Math.Sqrt(z.Imaginary) > 16;
		}

		protected override Complex GetNextValue(Complex z, Complex c, int iteration)
		{
			return _function(z);
		}

		protected override Complex GetInitialValue(Complex c)
		{
			return c;
		}
	}
}