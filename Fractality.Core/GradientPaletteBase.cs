using System;
using System.Collections.Generic;
using System.Linq;

namespace Fractality.Core
{
	public abstract class GradientPaletteBase : IPalette
	{
		protected readonly SortedDictionary<double, ImageColor> ColorIntervals = new SortedDictionary<double, ImageColor>();

		protected abstract ImageColor GetInterpolatedColor(ImageColor color1, ImageColor color2, double length, double offset);

		public ImageColor GetColor(double hue)
		{
			int count = ColorIntervals.Count;
			if (count == 0)
				return ImageColor.Black;

			var keysColors = ColorIntervals.Select(kv => Tuple.Create(kv.Key, kv.Value)).ToArray();

			ImageColor color1;
			ImageColor color2;

			double imax, length, offset;
			int index = 0;
			do
			{
				var prevIndex = index - 1;
				if(prevIndex < 0)
					prevIndex = count - 1;

				var nextIndex = index % count;

				color1 = keysColors[prevIndex].Item2;
				color2 = keysColors[nextIndex].Item2;

				var imin = keysColors[prevIndex].Item1;
				imax = keysColors[nextIndex].Item1;

				length = imax - imin;
				if (length < 0) length += 1;
				
				offset = hue - imin;
				if(offset < 0) offset += 1;

				index++;
			}
			while (index <= count && hue > imax);

			return GetInterpolatedColor(color1, color2, length, offset);
		}
	}
}