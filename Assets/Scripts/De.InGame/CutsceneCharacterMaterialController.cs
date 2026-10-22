using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CutsceneCharacterMaterialController : MonoBehaviour
{
	private struct OriginalValues
	{
		public Color EffectColor;

		public float Effect;

		public Color DissolveColor;

		public float DissolveProgress;

		public float GhostEnable;
	}

	private static readonly int _effectColorID;

	private static readonly int _effectID;

	private static readonly int _dissolveColorID;

	private static readonly int _dissolveProgressID;

	private static readonly int _ghostEnableID;

	private List<Material> _materials;

	private OriginalValues[] _originals;

	private Renderer[] _renderers;

	private MaterialPropertyBlock _mpb;

	private bool _usePropertyBlock;

	private bool _initialized;

	public bool Initialize()
	{
		return false;
	}

	public void ApplyEffect(Color color, float intensity)
	{
	}

	public void ApplyDeath(Color color, float progress)
	{
	}

	public void ApplyGhost(float enable)
	{
	}

	public void ResetEffect()
	{
	}

	public void ResetDeath()
	{
	}

	public void ResetGhost()
	{
	}

	private void ApplyMaterial(int colorId, Color color, int floatId, float value)
	{
	}

	private void ApplyMaterialFloat(int floatId, float value)
	{
	}

	private void ApplyPropertyBlock(int colorId, Color color, int floatId, float value)
	{
	}

	private void ApplyPropertyBlockFloat(int floatId, float value)
	{
	}

	private void ResetPropertyBlockPair(int colorId, Color defaultColor, int floatId, float defaultValue)
	{
	}

	private void ResetPropertyBlockFloat(int floatId, float defaultValue)
	{
	}
}
