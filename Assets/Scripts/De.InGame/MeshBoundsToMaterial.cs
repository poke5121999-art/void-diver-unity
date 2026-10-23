using UnityEngine;

public class MeshBoundsToMaterial : MonoBehaviour
{
	[SerializeField]
	private bool _visibleDebug;

	private Renderer _targetRenderer;

	private MaterialPropertyBlock _propertyBlock;

	private static readonly int _isSingleObjectID = Shader.PropertyToID("_IsSingleObject");

	private static readonly int _objectBoundSizeID = Shader.PropertyToID("_ObjectBoundSize");

	private void Awake()
	{
		_targetRenderer = GetComponent<Renderer>();
		_propertyBlock = new MaterialPropertyBlock();
	}

	private void Start()
	{
		UpdateMaterialProperties();
	}

	private void OnDestroy()
	{
		_targetRenderer.GetPropertyBlock(_propertyBlock);
		_propertyBlock.SetFloat(_isSingleObjectID, 0f);
		_propertyBlock.SetVector(_objectBoundSizeID, Vector3.zero);
		_targetRenderer.SetPropertyBlock(_propertyBlock);
	}

	public void UpdateMaterialProperties()
	{
		if (_targetRenderer == null)
		{
			Debug.LogWarning("Renderer가 없습니다.");
			return;
		}
		_targetRenderer.GetPropertyBlock(_propertyBlock);
		Vector3 vector = _targetRenderer.bounds.extents * 2f;
		_propertyBlock.SetFloat(_isSingleObjectID, 1f);
		_propertyBlock.SetVector(_objectBoundSizeID, vector);
		_targetRenderer.SetPropertyBlock(_propertyBlock);
	}

	private void OnDrawGizmosSelected()
	{
		if (_visibleDebug)
		{
			Renderer component = GetComponent<Renderer>();
			if (component != null)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireCube(component.bounds.center, component.bounds.extents * 2f);
			}
		}
	}
}
