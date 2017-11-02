using System;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Fractality.Core;

namespace Fractality.Windows
{
	public class WriteableBitmapImage : IImage
	{
		private readonly WriteableBitmap _bitmap;
		private bool _locked;
		private IntPtr _backBuffer;
		private readonly int _bytesPerPixel;
		private int _backBufferStride;
		private readonly object _syncRoot = new object();

		public WriteableBitmapImage(int width, int height)
		{
			_bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);
			_bytesPerPixel = _bitmap.Format.BitsPerPixel/8;
			Width = width;
			Height = height;
		}

		public int Width { get; private set; }

		public int Height { get; private set; }

		public void Lock()
		{
			lock (_syncRoot)
			{
				if (!_locked)
				{
					_locked = true;
					_bitmap.Lock();
					_backBuffer = _bitmap.BackBuffer;
					_backBufferStride = _bitmap.BackBufferStride;
				}
			}
		}

		public void Unlock()
		{
			if (_locked)
			{
				lock (_syncRoot)
				{
					if (_locked)
					{
						_bitmap.AddDirtyRect(new Int32Rect(0, 0, Width, Height));
						_bitmap.Unlock();
						_locked = false;
					}
				}
			}
		}

		private byte[] ColorToBytes(ImageColor color)
		{
			return new byte[] { color.B, color.G, color.R, color.A };
		}

		private ImageColor ColorFromBytes(params byte[] data)
		{
			return new ImageColor { B = data[0], G = data[1], R = data[2], A = data[3] };
		}

		private ImageColor GetPixelColor(ImagePoint point)
		{
			if (_locked)
			{
				int offset = point.Y * _backBufferStride + point.X * _bytesPerPixel;
				unsafe
				{
					byte* pbuff = (byte*)_backBuffer.ToPointer();
					return ColorFromBytes(pbuff[offset], pbuff[offset + 1], pbuff[offset + 2], pbuff[offset + 3]);
				}
			}
			else
			{
				byte[] pixelBuffer = new byte[_bytesPerPixel];
				Int32Rect rect = new Int32Rect(point.X, point.Y, 1, 1);
				_bitmap.CopyPixels(rect, pixelBuffer, _bytesPerPixel, 0);
				return ColorFromBytes(pixelBuffer);
			}
		}

		private void SetPixelColor(ImagePoint point, ImageColor color)
		{
			var colorBytes = ColorToBytes(color);
			if (_locked)
			{
				int offset = point.Y * _backBufferStride + point.X * _bytesPerPixel;
				unsafe
				{
					byte* pbuff = (byte*)_backBuffer.ToPointer();
					pbuff[offset] = colorBytes[0];
					pbuff[offset + 1] = colorBytes[1];
					pbuff[offset + 2] = colorBytes[2];
					pbuff[offset + 3] = colorBytes[3];
				}
			}
			else
			{
				Int32Rect rect = new Int32Rect(point.X, point.Y, 1, 1);
				_bitmap.WritePixels(rect, colorBytes, _bytesPerPixel, 0);
			}
		}

		public ImageColor this[ImagePoint point]
		{
			get { return GetPixelColor(point); }
			set { SetPixelColor(point, value); }
		}

		public BitmapSource BitmapSource
		{
			get { return _bitmap; }
		}

		public void Dispose()
		{
			
		}
	}
}