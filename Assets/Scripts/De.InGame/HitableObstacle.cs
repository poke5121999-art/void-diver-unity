public class HitableObstacle : MonoBase, IHitable
{
	public int HitLayer => 3000;

	public bool IsHitable => true;

	public float HitRadius => 0f;
}
