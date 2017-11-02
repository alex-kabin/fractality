using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Markup;

namespace Fractality
{
	public class InvokeMethod : TargetedTriggerAction<object>
	{
		public string MethodName
		{
			get { return (string)GetValue(MethodNameProperty); }
			set { SetValue(MethodNameProperty, value); }
		}

		public static readonly DependencyProperty MethodNameProperty =
			 DependencyProperty.Register("MethodName", typeof(string), typeof(InvokeMethod), new UIPropertyMetadata(null));


		public object Parameter
		{
			get { return (object)GetValue(ParameterProperty); }
			set { SetValue(ParameterProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Parameter.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ParameterProperty =
			 DependencyProperty.Register("Parameter", typeof(object), typeof(InvokeMethod), new UIPropertyMetadata(null));


		protected override void Invoke(object parameter)
		{
			var methodInfo = Target.GetType().GetMethods().FirstOrDefault(mi => mi.Name == MethodName);
			if (methodInfo != null)
			{
				var parameters = methodInfo.GetParameters();
				if(parameters.Length == 1)
					methodInfo.Invoke(Target, new[] {Parameter});
				else
					methodInfo.Invoke(Target, null);
			}
		}
	}
}