using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DisallowMultipleComponent]
[ExecuteAlways]
[RequireComponent(typeof(Image))]
public sealed class UISlicedVFXMaterialPropertyBinder : UIBehaviour, IMaterialModifier
{
	private enum OriginalSizeMode
	{
		SpriteRectPixels = 0,
		ImageNativeSize = 1
	}

	[SerializeField]
	private string uiRectPropertyName;

	[SerializeField]
	private string originalSizePropertyName;

	[SerializeField]
	private bool usePixelAdjustedRect;

	[SerializeField]
	private OriginalSizeMode originalSizeMode;

	[SerializeField]
	private Vector2 fallbackOriginalSize;

	[SerializeField]
	private bool watchForRuntimeChanges;

	[NonSerialized]
	private Image _image;

	[NonSerialized]
	private Material _material;

	[NonSerialized]
	private Material _lastBaseMaterial;

	[NonSerialized]
	private Rect _lastRect;

	[NonSerialized]
	private Vector2 _lastOriginalSize;

	[NonSerialized]
	private Sprite _lastSprite;

	private Image image => null;

	protected override void OnEnable()
	{
	}

	protected override void OnDisable()
	{
	}

	protected override void OnRectTransformDimensionsChange()
	{
	}

	protected override void OnDidApplyAnimationProperties()
	{
	}

	private void LateUpdate()
	{
	}

	public Material GetModifiedMaterial(Material baseMaterial)
	{
		return null;
	}

	private void ApplyProperties(Material targetMaterial)
	{
	}

	private Rect GetMaterialRect()
	{
		return default;
	}

	private Vector2 GetOriginalSize()
	{
		return default;
	}

	private static Vector2 ClampSize(Vector2 size)
	{
		return default;
	}

	private void CacheState(Rect rect, Vector2 originalSize)
	{
	}

	private void SetDirty()
	{
	}

	private void CleanUpMaterial()
	{
	}
}
