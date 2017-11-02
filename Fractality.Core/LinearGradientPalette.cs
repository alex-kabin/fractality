using System;
using System.Collections.Generic;
using System.Linq;

namespace Fractality.Core
{
	public class LinearGradientPalette : GradientPaletteBase
	{
		public LinearGradientPalette(IEnumerable<KeyValuePair<double, ImageColor>> keyColors)
		{
			foreach (var keyColor in keyColors)
			{
				ColorIntervals.Add(keyColor.Key, keyColor.Value);
			}
		}

		static LinearGradientPalette()
		{
			var rainbowColors = CreateRainbowColors();
			Rainbow = new LinearGradientPalette(rainbowColors);
		}

		private static IEnumerable<KeyValuePair<double, ImageColor>> CreateRainbowColors()
		{
			const int COUNT = 7;

			double offset = 1.0 / COUNT;
			double key = offset / 2;
			var colors = new List<KeyValuePair<double, ImageColor>>(COUNT);
			colors.Add(new KeyValuePair<double, ImageColor>(key, new ImageColor() { A = 255, R = 0xFF, G = 0x0, B = 0x0 })); // Red
			key += offset;
			colors.Add(new KeyValuePair<double, ImageColor>(key, new ImageColor() { A = 255, R = 0xFF, G = 0xA5, B = 0x00 })); // Orange
			key += offset;
			colors.Add(new KeyValuePair<double, ImageColor>(key, new ImageColor() { A = 255, R = 0xFF, G = 0xFF, B = 0x00 })); // Yellow
			key += offset;
			colors.Add(new KeyValuePair<double, ImageColor>(key, new ImageColor() { A = 255, R = 0x00, G = 0xFF, B = 0x00 })); // Green
			key += offset;
			colors.Add(new KeyValuePair<double, ImageColor>(key, new ImageColor() { A = 255, R = 0x00, G = 0xFF, B = 0xFF })); // Cyan
			key += offset;
			colors.Add(new KeyValuePair<double, ImageColor>(key, new ImageColor() { A = 255, R = 0x00, G = 0x00, B = 0xFF })); // Blue
			key += offset;
			colors.Add(new KeyValuePair<double, ImageColor>(key, new ImageColor() { A = 255, R = 0x80, G = 0x00, B = 0x80 })); // Purple
			
			return colors;
		}

		public static IPalette Rainbow { get; private set; }

		protected override ImageColor GetInterpolatedColor(ImageColor color1, ImageColor color2, double length, double offset)
		{
			var ratio = offset/length;
			return new ImageColor()
			       	{
			       		A = 255,
			       		R = (byte)(color1.R + ratio*(color2.R - color1.R)),
			       		G = (byte)(color1.G + ratio*(color2.G - color1.G)),
			       		B = (byte)(color1.B + ratio*(color2.B - color1.B)),
			       	};
		}
	}
}