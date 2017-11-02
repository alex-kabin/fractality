using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fractality.Core
{
	public class Vector<T> : IEnumerable<T>
	{
		private readonly T[] _array;

		public Vector(int size)
		{
			_array = new T[size];
		}

		public Vector(int size, T initialValue)
		{
			_array = new T[size];
			for (int i = 0; i < size; i++)
				_array[i] = initialValue;
		}

		public Vector(Vector<T> v)
		{
			_array = new T[v.Size];
			for (int i = 0; i < _array.Length; i++)
				_array[i] = v[i];
		}

		public Vector(params T[] values)
		{
			_array = new T[values.Length];
			for (int i = 0; i < _array.Length; i++)
				_array[i] = values[i];
		}

		public bool IsEmpty
		{
			get { return _array.Length == 0; }
		}

		public int Size { get { return _array.Length; } }
		
		public T this[int index]
		{
			get { return _array[index]; }
			set { _array[index] = value; }
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _array.ToList().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}