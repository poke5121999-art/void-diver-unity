using UnityEngine;

[ExecuteAlways]
public class ShadowMonsterVisualEffect : MonoBehaviour
{
	private readonly int _propertyGhostEnable;

	private readonly int _propertyIsLocalGhost;

	private readonly int _propertyIsReflected;

	[Header("Shadow Monster Ghost Effect Setting")]
	[SerializeField]
	private bool _isLocalGhost;

	[SerializeField]
	private SkeletonMaterialOverrider _skeletonMaterialOverrider;

	private bool _ghostEnable;

	private bool _isInitialized;

	private void Start()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void OnDestroy()
	{
	}

	private void UpdateShadowMonsterVisualEffect()
	{
	}

	private void ApplyEditorPreview()
	{
	}

	private void ClearEditorPreview()
	{
	}

	public void SetIsLocalGhost(bool isLocal)
	{
	}

	[ContextMenu("Assign Renderer Components")]
	private void GetRendererComponents()
	{
	}
}
