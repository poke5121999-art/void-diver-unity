using UnityEngine;

[RequireComponent(typeof(FakeShadowCaster))]
[ExecuteAlways]
public class FakeShadowCasterVisibility : MonoBehaviour
{
	private static readonly int _propDeathProgress;

	private static readonly int _propGhostEnable;

	private static readonly int _propFakeShadowEnable;

	private static readonly int _propVisibility;

	private FakeShadowCaster _caster;

	private MaterialPropertyBlock _tempMPB;

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void SetVisibilityProperties(MaterialPropertyBlock mpb, Renderer renderer)
	{
	}

	private float GetRendererFloat(Renderer renderer, int propId, float defaultValue)
	{
		return 0f;
	}
}
