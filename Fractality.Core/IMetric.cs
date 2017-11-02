namespace Fractality.Core
{
	public interface IMetric<in T>
	{
		double GetDistance(T op1, T op2);
	}
}