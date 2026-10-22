public class HitableObstacle : MonoBase, IHitable
{
	public int HitLayer => 0;

	public bool IsHitable => false;

	public float HitRadius => 0f;
}
