namespace Fractality.Common
{
	public abstract class ViewModel : ObservableObject
	{
		private IController _controller;
		public virtual IController Controller
		{
			get { return _controller; }
			set { _controller = value; RaisePropertyChanged(() => Controller); }
		}
	}

	public abstract class ViewModel<T> : ViewModel
	{
		protected ViewModel(T data)
		{
			Data = data;
		}

		public T Data { get; private set; }
	}
}