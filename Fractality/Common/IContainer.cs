using System;

namespace Fractality.Common
{
	public interface IContainer : IDisposable
	{
		T Resolve<T>();
		T Resolve<T>(string name);
		void Inject(object target);
	}
}