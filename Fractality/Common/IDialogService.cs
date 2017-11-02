using System;

namespace Fractality.Common
{
	public interface IDialogService
	{
		void Show(IWindow window, object dataContext, Action callback);

		bool? ShowModal(IWindow window, object dataContext);
	}
}