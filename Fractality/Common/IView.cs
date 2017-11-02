using System.Windows.Input;

namespace Fractality.Common
{
	public interface IView
	{
		object DataContext { get; set; }

		Cursor Cursor { get; set; }
	}
}