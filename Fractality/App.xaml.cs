using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Fractality.Common;

namespace Fractality
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		[Import("RootController")]
		private IController Controller { get; set; }

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			Container.Global.Inject(this);
		}
	}
}
