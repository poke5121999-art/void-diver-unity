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
	private string uiRectPropertyName = "_UIRect";

	[SerializeField]
	private string originalSizePropertyName = "_OriginalSize";

	[SerializeField]
	private bool usePixelAdjustedRect;

	[SerializeField]
	private OriginalSizeMode originalSizeMode;

	[SerializeField]
	private Vector2 fallbackOriginalSize = Vector2.one;

	[SerializeField]
	private bool watchForRuntimeChanges = true;

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

	private Image image
	{
		get
		{
			if (!_image)
			{
				_image = GetComponent<Image>();
			}
			return _image;
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		SetDirty();
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		CleanUpMaterial();
	}

	protected override void OnRectTransformDimensionsChange()
	{
		base.OnRectTransformDimensionsChange();
		SetDirty();
	}

	protected override void OnDidApplyAnimationProperties()
	{
		base.OnDidApplyAnimationProperties();
		SetDirty();
	}

	private void LateUpdate()
	{
		if (!watchForRuntimeChanges || !IsActive() || image == null)
		{
			return;
		}
		Rect materialRect = GetMaterialRect();
		Vector2 originalSize = GetOriginalSize();
		if (materialRect.x == _lastRect.x && materialRect.y == _lastRect.y && materialRect.width == _lastRect.width && materialRect.height == _lastRect.height && (originalSize - _lastOriginalSize).sqrMagnitude < 1E-10f && !(image.sprite != _lastSprite))
		{
			return;
		}
		CacheState(materialRect, originalSize);
		SetDirty();
	}

	public Material GetModifiedMaterial(Material baseMaterial)
	{
		if (!IsActive() || image == null || baseMaterial == null)
		{
			return baseMaterial;
		}
		if (_material == null || _lastBaseMaterial != baseMaterial || _material.shader != baseMaterial.shader)
		{
			CleanUpMaterial();
			_material = new Material(baseMaterial);
			_material.hideFlags = HideFlags.HideAndDontSave;
			_lastBaseMaterial = baseMaterial;
		}
		else
		{
			_material.CopyPropertiesFromMaterial(baseMaterial);
		}
		ApplyProperties(_material);
		CacheState(GetMaterialRect(), GetOriginalSize());
		return _material;
	}

	private void ApplyProperties(Material targetMaterial)
	{
		Rect materialRect = GetMaterialRect();
		Vector2 originalSize = GetOriginalSize();
		if (!string.IsNullOrEmpty(uiRectPropertyName) && targetMaterial.HasProperty(uiRectPropertyName))
		{
			targetMaterial.SetVector(uiRectPropertyName, new Vector4(materialRect.x, materialRect.y, materialRect.x + materialRect.width, materialRect.y + materialRect.height));
		}
		if (!string.IsNullOrEmpty(originalSizePropertyName) && targetMaterial.HasProperty(originalSizePropertyName))
		{
			targetMaterial.SetVector(originalSizePropertyName, new Vector4(originalSize.x, originalSize.y, 0f, 0f));
		}
	}

	private Rect GetMaterialRect()
	{
		if (usePixelAdjustedRect && image != null)
		{
			return image.GetPixelAdjustedRect();
		}
		RectTransform rectTransform = base.transform as RectTransform;
		if (!(rectTransform != null))
		{
			return Rect.zero;
		}
		return rectTransform.rect;
	}

	private Vector2 GetOriginalSize()
	{
		Sprite sprite = ((image != null) ? image.sprite : null);
		if (sprite == null)
		{
			return ClampSize(fallbackOriginalSize);
		}
		if (originalSizeMode == OriginalSizeMode.ImageNativeSize)
		{
			float num = Mathf.Max(0.001f, image.pixelsPerUnit);
			return ClampSize(new Vector2(sprite.rect.width / num, sprite.rect.height / num));
		}
		return ClampSize(new Vector2(sprite.rect.width, sprite.rect.height));
	}

	private static Vector2 ClampSize(Vector2 size)
	{
		return new Vector2(Mathf.Max(0.001f, size.x), Mathf.Max(0.001f, size.y));
	}

	private void CacheState(Rect rect, Vector2 originalSize)
	{
		_lastRect = rect;
		_lastOriginalSize = originalSize;
		_lastSprite = ((image != null) ? image.sprite : null);
	}

	private void SetDirty()
	{
		if (IsActive() && image != null)
		{
			image.SetMaterialDirty();
		}
	}

	private void CleanUpMaterial()
	{
		if (_material != null)
		{
			if (Application.isPlaying)
			{
				UnityEngine.Object.Destroy(_material);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(_material);
			}
		}
		_material = null;
		_lastBaseMaterial = null;
	}
}
