using UnityEngine;

[ExecuteAlways]
public class DecorationObjectDissolveTarget : MonoBehaviour, IDissolveTarget
{
	private static readonly int _dissolveProgressId = Shader.PropertyToID("_DissolveProgress");

	private static readonly int _dissolveEdgeColorId = Shader.PropertyToID("_DissolveEdgeColor");

	private Renderer[] _renderers;

	private MaterialPropertyBlock _mpb;

	public float Progress { get; private set; }

	private void OnEnable()
	{
		_renderers = GetComponentsInChildren<Renderer>(true);
		_mpb = new MaterialPropertyBlock();
		Progress = 0f;
		if (_renderers != null)
		{
			for (int i = 0; i < _renderers.Length; i++)
			{
				_renderers[i].SetPropertyBlock(null);
			}
		}
		if (DecorationVisualManager.Instance != null)
		{
			DecorationVisualManager.Instance.Register(this);
		}
	}

	private void OnDisable()
	{
		if (DecorationVisualManager.Instance != null)
		{
			DecorationVisualManager.Instance.Unregister(this);
		}
	}

	public void SetProgress(float value, Color edgeColor)
	{
		Progress = value;
		if (_renderers == null)
		{
			return;
		}
		for (int i = 0; i < _renderers.Length; i++)
		{
			Renderer renderer = _renderers[i];
			if (value > 0f)
			{
				renderer.GetPropertyBlock(_mpb);
				_mpb.SetFloat(_dissolveProgressId, value);
				_mpb.SetColor(_dissolveEdgeColorId, edgeColor);
				renderer.SetPropertyBlock(_mpb);
			}
			else
			{
				renderer.SetPropertyBlock(null);
			}
		}
	}
}
