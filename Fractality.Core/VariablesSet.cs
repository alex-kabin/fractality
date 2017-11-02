using System.Collections.Generic;
using Fractality.Utils;

namespace Fractality.Core
{
	public class VariablesSet<T>
	{
		private readonly IDictionary<string, IDictionary<string, T>> _variables = new Dictionary<string, IDictionary<string, T>>();

		private IDictionary<string, T> GetScope(string scope)
		{
			IDictionary<string, T> dict;
			if(_variables.ContainsKey(scope))
				dict = _variables[scope];
			else
			{
				dict = new Dictionary<string, T>();
				_variables.Add(scope, dict);
			}
			return dict;
		}

		public T this[string name]
		{
			get { return this[string.Empty, name]; }
			set { this[string.Empty, name] = value; }
		}

		public T this[string scope, string name]
		{
			get
			{
				T result;
				GetScope(scope).TryGetValue(name, out result);
				return result;
			}
			set
			{
				GetScope(scope).AddOrSet(name, value);
			}
		}
	}
}