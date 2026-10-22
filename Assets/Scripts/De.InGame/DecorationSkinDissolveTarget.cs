using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(SkinnedAreaObject))]
public class DecorationSkinDissolveTarget : MonoBehaviour, IDissolveTarget
{
	private static readonly int _dissolveProgressId;

	private static readonly int _dissolveEdgeColorId;

	private MeshRenderer _meshRenderer;

	private SkinnedAreaObject _skinnedAreaObject;

	private MaterialPropertyBlock _mpb;

	public float Progress { get; private set; }

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void OnAfterApply()
	{
	}

	public void SetProgress(float value, Color edgeColor)
	{
	}
}
