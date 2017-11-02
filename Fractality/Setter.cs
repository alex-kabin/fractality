using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Markup;

namespace Fractality
{
	public class Setter : TargetedTriggerAction<DependencyObject>
	{
		[Ambient]
		public DependencyProperty Property
		{
			get { return (DependencyProperty)GetValue(PropertyProperty); }
			set { SetValue(PropertyProperty, value); }
		}

		public static readonly DependencyProperty PropertyProperty =
			 DependencyProperty.Register("Property", typeof(DependencyProperty), typeof(Setter), new UIPropertyMetadata(null));




		public object Value
		{
			get { return (object)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		public static readonly DependencyProperty ValueProperty =
			 DependencyProperty.Register("Value", typeof(object), typeof(Setter), new UIPropertyMetadata(null));


		
		protected override void Invoke(object parameter)
		{
			Target.SetValue(Property, Value);
		}
	}
}
