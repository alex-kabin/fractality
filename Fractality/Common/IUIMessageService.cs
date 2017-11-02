using System.Windows;

namespace Fractality.Common
{
	public interface IUIMessageService
	{
		void ShowError(string text);
		void ShowWarning(string text);
		void ShowInfo(string text);
		bool? ShowQuestion(string text, MessageBoxButton buttons);
	}
}