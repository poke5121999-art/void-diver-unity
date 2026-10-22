public interface IHitable
{
	int HitLayer { get; }

	bool IsHitable { get; }

	float HitRadius { get; }
}
