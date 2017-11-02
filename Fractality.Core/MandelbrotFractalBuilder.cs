using System;
using System.Drawing;
using System.Numerics;

namespace Fractality
{
	public class MandelbrotFractalBuilder : FractalBuilderBase
	{
		private Func<Complex[], Complex, Complex[]> _function;

		public MandelbrotFractalBuilder(Size imageSize, BitmapFactory bitmapFactory, FractalColorizerBase colorizer,
										Func<Complex[], Complex, Complex[]> func)
			: base(imageSize, colorizer, bitmapFactory)
		{
			SetFunction(func);
			BailOutTest = DefaultTestBailOut;
		}

		public MandelbrotFractalBuilder(Size imageSize, BitmapFactory bitmapFactory)
			: this(imageSize, bitmapFactory, new SimpleFractalColorizer(), DefaultGetNextValue)
		{
		}

		public MandelbrotFractalBuilder(Size imageSize, BitmapFactory bitmapFactory, Func<Complex[], Complex, Complex[]> func)
			: this(imageSize, bitmapFactory, new SimpleFractalColorizer(), func)
		{
		}

		public void SetFunction(Func<Complex[], Complex, Complex[]> func)
		{
			_function = func;
		}

		private static Complex[] DefaultGetNextValue(Complex[] z, Complex c)
		{
			z[0] = z[0] * z[0] + c;
			return z;
		}

		private static bool DefaultTestBailOut(Complex[] z)
		{
			return z[0].Real * z[0].Real + z[0].Imaginary * z[0].Imaginary > 4;
		}

		protected override Complex[] GetNextValue(Complex[] z, Complex c, int iteration)
		{
			return _function(z, c);
		}

		protected override Complex[] GetInitialValue(Complex c)
		{
			return new Complex[] {0};
		}
	}
}