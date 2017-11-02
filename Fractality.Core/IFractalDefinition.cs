using System.Numerics;

namespace Fractality.Core
{
	public interface IFractalDefinition
	{
		ComplexVector Init(IPointContext c);

		void Iterate(IPointContext c);

		bool Bailout(IPointContext c);

		Complex InitialMin { get; }
		Complex InitialMax { get; }
	}
}