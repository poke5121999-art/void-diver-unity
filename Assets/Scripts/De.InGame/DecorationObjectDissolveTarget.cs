using UnityEngine;

[ExecuteAlways]
public class DecorationObjectDissolveTarget : MonoBehaviour, IDissolveTarget
{
	private static readonly int _dissolveProgressId;

	private static readonly int _dissolveEdgeColorId;

	private Renderer[] _renderers;

	private MaterialPropertyBlock _mpb;

	public float Progress { get; private set; }

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	public void SetProgress(float value, Color edgeColor)
	{
	}
}
