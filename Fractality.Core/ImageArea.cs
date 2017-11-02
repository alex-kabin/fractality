using System.Diagnostics.Contracts;

namespace Fractality.Core
{
	public struct ImageArea
	{
		public int XMin;
		public int YMin;
		public int XMax;
		public int YMax;

		public ImagePoint Min { get { return new ImagePoint(XMin, YMin); } }
		public ImagePoint Max { get { return new ImagePoint(XMax, YMax); } }

		public ImageArea(int xmin, int ymin, int xmax, int ymax)
		{
			Contract.Requires(xmin <= xmax && ymin <= ymax);

			XMin = xmin;
			YMin = ymin;
			XMax = xmax;
			YMax = ymax;
		}
	}
}