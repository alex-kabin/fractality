using System;

namespace Fractality.Core
{
	public interface IImage : IDisposable
	{
		int Width { get; }
		int Height { get; }

		void Lock();
		void Unlock();

		ImageColor this[ImagePoint point] { get; set; }
	}
}