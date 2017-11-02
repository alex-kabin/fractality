using System;
using System.Runtime.InteropServices;

namespace Fractality.Core
{
	[StructLayout(LayoutKind.Explicit)]
	public struct ImageColor
	{
		[FieldOffset(0)] public UInt32 ARGB;
		[FieldOffset(0)] public byte A;
		[FieldOffset(1)] public byte R;
		[FieldOffset(2)] public byte G;
		[FieldOffset(3)] public byte B;

		public ImageColor Invert()
		{
			return new ImageColor() { A = this.A, R = (byte)(255 - this.R), G = (byte)(255 - this.G), B = (byte)(255 - this.B) };
		}

		public static readonly ImageColor White = new ImageColor() { A = 255, R = 255, G = 255, B = 255 };
		public static readonly ImageColor Black = new ImageColor() { A = 255, R = 0, G = 0, B = 0 };
	}
}