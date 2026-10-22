using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(SkinnedAreaObject))]
public class DecorationSkinDissolveTarget : MonoBehaviour, IDissolveTarget
{
	private static readonly int _dissolveProgressId = Shader.PropertyToID("_DissolveProgress");

	private static readonly int _dissolveEdgeColorId = Shader.PropertyToID("_DissolveEdgeColor");

	private MeshRenderer _meshRenderer;

	private SkinnedAreaObject _skinnedAreaObject;

	private MaterialPropertyBlock _mpb;

	public float Progress { get; private set; }

	private void OnEnable()
	{
		_meshRenderer = GetComponent<MeshRenderer>();
		_skinnedAreaObject = GetComponent<SkinnedAreaObject>();
		_mpb = new MaterialPropertyBlock();
		_skinnedAreaObject.OnAfterApply += OnAfterApply;
		SetProgress(0f, Color.white);
		if (DecorationVisualManager.Instance != null)
		{
			DecorationVisualManager.Instance.Register(this);
		}
	}

	private void OnDisable()
	{
		if (_skinnedAreaObject != null)
		{
			_skinnedAreaObject.OnAfterApply -= OnAfterApply;
		}
		if (DecorationVisualManager.Instance != null)
		{
			DecorationVisualManager.Instance.Unregister(this);
		}
	}

	private void OnAfterApply()
	{
		DecorationVisualManager instance = DecorationVisualManager.Instance;
		if (!(instance == null))
		{
			bool flag = instance.IsRegistered(this);
			instance.Register(this);
			if (!flag)
			{
				SetProgress(1f, instance.EdgeColor);
			}
		}
	}

	public void SetProgress(float value, Color edgeColor)
	{
		Progress = value;
		if (value <= 0f)
		{
			_meshRenderer.SetPropertyBlock(null);
			return;
		}
		_meshRenderer.GetPropertyBlock(_mpb);
		_mpb.SetFloat(_dissolveProgressId, value);
		_mpb.SetColor(_dissolveEdgeColorId, edgeColor);
		_meshRenderer.SetPropertyBlock(_mpb);
	}
}
