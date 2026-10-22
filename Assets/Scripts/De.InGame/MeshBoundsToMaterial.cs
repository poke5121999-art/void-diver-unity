using UnityEngine;

public class MeshBoundsToMaterial : MonoBehaviour
{
	[SerializeField]
	private bool _visibleDebug;

	private Renderer _targetRenderer;

	private MaterialPropertyBlock _propertyBlock;

	private static readonly int _isSingleObjectID;

	private static readonly int _objectBoundSizeID;

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void OnDestroy()
	{
	}

	public void UpdateMaterialProperties()
	{
	}

	private void OnDrawGizmosSelected()
	{
	}
}
