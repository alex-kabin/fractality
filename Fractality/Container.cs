using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using Fractality.Common;

namespace Fractality
{
	public class Container
	{
		private static readonly IContainer Instance;

		public static IContainer Global
		{
			get { return Instance; }
		}

		static Container()
		{
			AggregateCatalog catalog = new AggregateCatalog();
			catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
			catalog.Catalogs.Add(new DirectoryCatalog(".", "Fractality.Definitions.dll"));
			catalog.Catalogs.Add(new DirectoryCatalog(".", "Fractality.Painters.dll"));
			
			var composition = new CompositionContainer(catalog);
			Instance = new MEFContainer(composition);
		}
	}
}