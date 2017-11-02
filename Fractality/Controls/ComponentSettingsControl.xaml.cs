using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Fractality.Core;

namespace Fractality.Controls
{
	/// <summary>
	/// Interaction logic for ComponentSettingsControl.xaml
	/// </summary>
	public partial class ComponentSettingsControl
	{
		public class ParameterViewModel
		{
			public string Label { get; set; }
			public string Description { get; set; }
			public Control Editor { get; set; }
		}

		public ComponentSettingsControl()
		{
			InitializeComponent();
		}
		
		public object Component
		{
			get { return GetValue(ComponentProperty); }
			set { SetValue(ComponentProperty, value); }
		}

		public static readonly DependencyProperty ComponentProperty =
			DependencyProperty.Register("Component", typeof(object), typeof(ComponentSettingsControl), 
			new UIPropertyMetadata(null, OnComponentPropertyChanged));

		private static void OnComponentPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			var control = sender as ItemsControl;
			if(control == null)
				return;

			var component = e.NewValue;
			if (component == null)
			{
				control.DataContext = null;
				return;
			}

			var parameters = new List<ParameterViewModel>();

			var properties = component.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(pi => pi.CanWrite);
			foreach (var propertyInfo in properties)
			{
				var parameterViewModel = new ParameterViewModel();

				var propertyLabel = propertyInfo.Name;

				var displayNameAttribute =
					Attribute.GetCustomAttribute(propertyInfo, typeof(DisplayNameAttribute)) as DisplayNameAttribute;
				if (displayNameAttribute != null)
					propertyLabel = displayNameAttribute.DisplayName;

				var descriptionAttribute =
					Attribute.GetCustomAttribute(propertyInfo, typeof(DescriptionAttribute)) as DescriptionAttribute;
				if (descriptionAttribute != null)
					parameterViewModel.Description = descriptionAttribute.Description;
				
				parameterViewModel.Label = String.Format("{0}:", propertyLabel);

				var binding = new Binding(propertyInfo.Name) { Source = component, Mode = BindingMode.TwoWay };
				if (propertyInfo.PropertyType == typeof(Complex))
				{
					parameterViewModel.Editor = new ComplexNumberEditor();
					parameterViewModel.Editor.SetBinding(ComplexNumberEditor.ComplexNumberProperty, binding);
				}
				else if(propertyInfo.PropertyType == typeof(bool))
				{
					parameterViewModel.Editor = new CheckBox() { IsThreeState = false };
					parameterViewModel.Editor.SetBinding(CheckBox.IsCheckedProperty, binding);
				}
				else if (new[] { typeof(int), typeof(string), typeof(double) }.Contains(propertyInfo.PropertyType))
				{
					var rangeAttribute = Attribute.GetCustomAttribute(propertyInfo, typeof(RangeAttribute)) as RangeAttribute;
					if (rangeAttribute != null)
					{
						parameterViewModel.Editor = new Slider()
						                            	{
						                            		Minimum = Convert.ToDouble(rangeAttribute.Minimum),
						                            		Maximum = Convert.ToDouble(rangeAttribute.Maximum)
						                            	};
						parameterViewModel.Editor.SetBinding(RangeBase.ValueProperty, binding);
					}
					else
					{
						parameterViewModel.Editor = new TextBox();
						parameterViewModel.Editor.SetBinding(TextBox.TextProperty, binding);
					}
				}

				if(parameterViewModel.Editor != null)
					parameters.Add(parameterViewModel);
			}

			control.ItemsSource = parameters;
		}
	}
}
