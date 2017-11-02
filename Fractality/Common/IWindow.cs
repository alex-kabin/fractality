using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Fractality.Common
{
	public interface IWindow : IView
	{
		void Show();
		void Close();

		event EventHandler Closed;
		event CancelEventHandler Closing;
	}
}