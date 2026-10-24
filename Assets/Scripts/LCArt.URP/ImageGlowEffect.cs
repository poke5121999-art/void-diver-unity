using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[ExecuteAlways]
public class ImageGlowEffect : MonoBehaviour
{
	[Header("Image Glow Setting")]
	[SerializeField]
	private ImageGlowEffectSettings _settings;

	[SerializeField]
	private Color _glowColor = Color.white;

	private Image _targetImage;

	private RectTransform _targetRect;

	private RenderTexture _alphaTexture;

	private RawImage _alphaOverlay;

	private RectTransform _overlayRect;

	private GameObject _alphaOverlayObject;

	private Sprite _lastSprite;

	private Color _lastColor;

	private Vector2 _lastAnchoredPosition;

	private Vector2 _lastSizeDelta;

	private Quaternion _lastLocalRotation;

	private Vector3 _lastLocalScale;

	private int _lastPadding;

	private int _lastMaxDistance;

	private Color _lastGlowColor;

	private bool _needsOverlayUpdate;

	private RenderTexture _seedBuffer;

	private RenderTexture _tempAlphaTexture;

	private GameObject _tempCameraObject;

	private Camera _tempCamera;

	private GameObject _tempCanvasObject;

	private Canvas _tempCanvas;

	private GameObject _tempImageObject;

	private Image _tempImage;

	private const float ChangeEpsilonSqr = 9.9999994E-11f;

	private static void DestroyObject(Object obj)
	{
		if (Application.isPlaying)
		{
			Object.Destroy(obj);
		}
		else
		{
			Object.DestroyImmediate(obj);
		}
	}

	private static float SqrDiff(Color a, Color b)
	{
		float num = a.g - b.g;
		float num2 = a.r - b.r;
		float num3 = a.b - b.b;
		float num4 = a.a - b.a;
		return num * num + num2 * num2 + num3 * num3 + num4 * num4;
	}

	private void OnEnable()
	{
		_targetImage = GetComponent<Image>();
		_targetRect = ((_targetImage != null) ? _targetImage.rectTransform : null);
		if (_targetImage != null)
		{
			CacheImageProperties();
			CacheRectTransformProperties();
			CacheSdfProperties();
			CacheGlowProperties();
			if (_targetImage.enabled)
			{
				GenerateSDFTexture();
				CreateOrUpdateOverlay();
				UpdateRectTransform();
			}
		}
	}

	private void LateUpdate()
	{
		if (_needsOverlayUpdate)
		{
			_needsOverlayUpdate = false;
			CreateOrUpdateOverlay();
			CacheImageProperties();
			if (HasSdfPropertiesChanged() || HasImagePropertiesChanged() || _alphaTexture == null)
			{
				ReleaseTextures();
				GenerateSDFTexture();
				CreateOrUpdateOverlay();
				CacheSdfProperties();
			}
		}
		if (_targetRect != null && HasRectTransformChanged())
		{
			UpdateRectTransform();
			CacheRectTransformProperties();
		}
		if (HasGlowPropertiesChanged())
		{
			if (_alphaOverlay != null)
			{
				_alphaOverlay.color = _glowColor;
			}
			CacheGlowProperties();
		}
	}

	private void OnDisable()
	{
		DestroyOverlay();
		ReleaseTextures();
		CleanupTempObjects();
	}

	private void OnDestroy()
	{
		DestroyOverlay();
		ReleaseTextures();
		CleanupTempObjects();
	}

	private void OnValidate()
	{
		_needsOverlayUpdate = true;
	}

	private void CacheImageProperties()
	{
		if (!(_targetImage == null))
		{
			_lastSprite = _targetImage.sprite;
			_lastColor = _targetImage.color;
		}
	}

	private void CacheRectTransformProperties()
	{
		if (!(_targetRect == null))
		{
			_lastAnchoredPosition = _targetRect.anchoredPosition;
			_lastSizeDelta = _targetRect.sizeDelta;
			_lastLocalRotation = _targetRect.localRotation;
			_lastLocalScale = _targetRect.localScale;
		}
	}

	private void CacheSdfProperties()
	{
		if (_settings == null)
		{
			_lastPadding = 8;
			_lastMaxDistance = 8;
		}
		else
		{
			_lastPadding = _settings.Padding;
			_lastMaxDistance = _settings.MaxDistance;
		}
	}

	private void CacheGlowProperties()
	{
		_lastGlowColor = _glowColor;
	}

	private bool HasImagePropertiesChanged()
	{
		if (_targetImage == null)
		{
			return false;
		}
		if (_targetImage.sprite != _lastSprite)
		{
			return true;
		}
		return SqrDiff(_targetImage.color, _lastColor) >= ChangeEpsilonSqr;
	}

	private bool HasRectTransformChanged()
	{
		if (_targetRect == null)
		{
			return false;
		}
		if ((_targetRect.anchoredPosition - _lastAnchoredPosition).sqrMagnitude >= ChangeEpsilonSqr)
		{
			return true;
		}
		if ((_targetRect.sizeDelta - _lastSizeDelta).sqrMagnitude >= ChangeEpsilonSqr)
		{
			return true;
		}
		if (Quaternion.Dot(_targetRect.localRotation, _lastLocalRotation) <= 0.999999f)
		{
			return true;
		}
		return (_targetRect.localScale - _lastLocalScale).sqrMagnitude >= ChangeEpsilonSqr;
	}

	private bool HasSdfPropertiesChanged()
	{
		if (_settings == null)
		{
			return false;
		}
		return _settings.Padding != _lastPadding || _settings.MaxDistance != _lastMaxDistance;
	}

	private bool HasGlowPropertiesChanged()
	{
		return SqrDiff(_glowColor, _lastGlowColor) >= ChangeEpsilonSqr;
	}

	private void CreateOrUpdateOverlay()
	{
		if (_settings == null || _alphaTexture == null || _targetImage == null)
		{
			return;
		}
		if (_alphaOverlayObject == null)
		{
			_alphaOverlayObject = new GameObject(_targetImage.gameObject.name + "_GlowEffect");
			_alphaOverlayObject.transform.SetParent(base.transform.parent, worldPositionStays: false);
			_alphaOverlayObject.hideFlags = HideFlags.DontSave;
			_alphaOverlay = _alphaOverlayObject.AddComponent<RawImage>();
			_overlayRect = _alphaOverlayObject.GetComponent<RectTransform>();
			_overlayRect.anchorMin = _targetRect.anchorMin;
			_overlayRect.anchorMax = _targetRect.anchorMax;
			_overlayRect.pivot = _targetRect.pivot;
			_overlayRect.anchoredPosition = _targetRect.anchoredPosition;
			_overlayRect.sizeDelta = _targetRect.sizeDelta;
			_overlayRect.localRotation = _targetRect.localRotation;
			_overlayRect.localScale = _targetRect.localScale;
			_alphaOverlayObject.transform.SetSiblingIndex(base.transform.GetSiblingIndex());
			_alphaOverlay.raycastTarget = false;
		}
		Sprite sprite = _targetImage.sprite;
		float num = Mathf.Abs(_targetRect.rect.width) / sprite.rect.width * (float)_settings.Padding;
		float num2 = Mathf.Abs(_targetRect.rect.height) / sprite.rect.height * (float)_settings.Padding;
		Vector2 sizeDelta = _targetRect.sizeDelta;
		Vector2 pivot = _overlayRect.pivot;
		_overlayRect.sizeDelta = new Vector2(num + num + sizeDelta.x, num2 + num2 + sizeDelta.y);
		Vector2 anchoredPosition = _targetRect.anchoredPosition;
		_overlayRect.anchoredPosition = new Vector2((1f - pivot.x * 2f) * (0f - num) + anchoredPosition.x, (1f - pivot.y * 2f) * (0f - num2) + anchoredPosition.y);
		_alphaOverlay.texture = _alphaTexture;
		_alphaOverlay.uvRect = new Rect(0f, 0f, 1f, 1f);
		if (_settings.GlowEffectMaterial != null)
		{
			_alphaOverlay.material = _settings.GlowEffectMaterial;
		}
		_alphaOverlay.color = _glowColor;
	}

	private void UpdateRectTransform()
	{
		if (_overlayRect == null || _targetRect == null)
		{
			return;
		}
		Vector2 vector = CalculatePaddingOffset();
		_overlayRect.anchoredPosition = vector + _targetRect.anchoredPosition;
		_overlayRect.localRotation = _targetRect.localRotation;
		_overlayRect.localScale = _targetRect.localScale;
		if ((_targetRect.sizeDelta - _lastSizeDelta).sqrMagnitude >= ChangeEpsilonSqr)
		{
			CreateOrUpdateOverlay();
		}
	}

	private Vector2 CalculatePaddingOffset()
	{
		if (_settings == null || _targetRect == null || _overlayRect == null || _alphaTexture == null)
		{
			return Vector2.zero;
		}
		float width = _targetRect.rect.width;
		float height = _targetRect.rect.height;
		Rect rect = _targetImage.sprite.rect;
		int padding = _settings.Padding;
		Vector2 pivot = _overlayRect.pivot;
		Vector2 result = default(Vector2);
		result.x = (0f - Mathf.Abs(width) / rect.width * (float)padding) * (1f - pivot.x * 2f);
		result.y = (1f - pivot.y * 2f) * (0f - Mathf.Abs(height) / rect.height * (float)padding);
		return result;
	}

	private void UpdateGlowColor()
	{
		if (!(_alphaOverlay == null))
		{
			_alphaOverlay.color = _glowColor;
		}
	}

	private void DestroyOverlay()
	{
		if (_alphaOverlayObject != null)
		{
			DestroyObject(_alphaOverlayObject);
			_alphaOverlayObject = null;
			_alphaOverlay = null;
		}
	}

	private void GenerateSDFTexture()
	{
		if (_settings == null)
		{
			return;
		}
		if (_targetImage == null || _targetImage.sprite == null)
		{
			Debug.LogWarning("Image or Sprite is missing!");
			return;
		}
		if (_settings.GlowEffectComputeShader == null)
		{
			Debug.LogError("SDF ComputeShader is not assigned!");
			return;
		}
		if (_alphaTexture != null)
		{
			ReleaseTextures();
		}
		Sprite sprite = _targetImage.sprite;
		// FUN_180069d40 (rect width/height) is Mathf.FloorToInt (calls Math.Floor @0x184a51804), FUN_180069e90 (padding) is Mathf.RoundToInt (machine code checked)
		int num = Mathf.Max(1, Mathf.FloorToInt(sprite.rect.width));
		int num2 = Mathf.Max(1, Mathf.FloorToInt(sprite.rect.height));
		int num3 = Mathf.RoundToInt(_settings.Padding);
		int num4 = num + num3 * 2;
		int num5 = num2 + num3 * 2;
		RenderTexture renderTexture = new RenderTexture(num4, num5, 24, RenderTextureFormat.ARGB32);
		renderTexture.enableRandomWrite = true;
		renderTexture.Create();
		SetupTempCamera(num4, num5);
		SetupTempImage(sprite, num, num2, num3);
		_tempCamera.targetTexture = renderTexture;
		_tempCamera.Render();
		_tempCamera.targetTexture = null;
		CleanupTempObjects();
		_tempAlphaTexture = renderTexture;
		if (_tempAlphaTexture == null)
		{
			Debug.LogError("Failed to render sprite!");
			return;
		}
		_alphaTexture = GenerateSDF(_tempAlphaTexture, num + num3 * 2, num2 + num3 * 2);
		if (_tempAlphaTexture != null)
		{
			_tempAlphaTexture.Release();
			DestroyObject(_tempAlphaTexture);
			_tempAlphaTexture = null;
		}
	}

	private RenderTexture RenderSpriteWithCamera(Sprite sprite, int width, int height, int padding)
	{
		int num = width + padding * 2;
		int num2 = height + padding * 2;
		RenderTexture renderTexture = new RenderTexture(num, num2, 24, RenderTextureFormat.ARGB32);
		renderTexture.enableRandomWrite = true;
		renderTexture.Create();
		SetupTempCamera(num, num2);
		SetupTempImage(sprite, width, height, padding);
		_tempCamera.targetTexture = renderTexture;
		_tempCamera.Render();
		_tempCamera.targetTexture = null;
		CleanupTempObjects();
		return renderTexture;
	}

	private void SetupTempCamera(int width, int height)
	{
		_tempCameraObject = new GameObject("TempGlowCamera");
		_tempCameraObject.hideFlags = HideFlags.HideAndDontSave;
		_tempCamera = _tempCameraObject.AddComponent<Camera>();
		_tempCamera.orthographic = true;
		_tempCamera.orthographicSize = (float)height * 0.5f;
		_tempCamera.clearFlags = CameraClearFlags.SolidColor;
		_tempCamera.backgroundColor = new Color(0f, 0f, 0f, 0f);
		_tempCamera.cullingMask = int.MinValue;
		_tempCamera.nearClipPlane = 0.1f;
		_tempCamera.farClipPlane = 100f;
		_tempCamera.depth = -100f;
		_tempCamera.enabled = false;
	}

	private void SetupTempImage(Sprite sprite, int width, int height, int padding)
	{
		_tempCanvasObject = new GameObject("TempGlowCanvas");
		_tempCanvasObject.hideFlags = HideFlags.HideAndDontSave;
		_tempCanvasObject.layer = 31;
		_tempCanvas = _tempCanvasObject.AddComponent<Canvas>();
		_tempCanvas.renderMode = RenderMode.WorldSpace;
		_tempCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(width + padding * 2, height + padding * 2);
		_tempCanvasObject.transform.position = new Vector3(0f, 0f, 10f);
		_tempCanvasObject.transform.localScale = Vector3.one;
		_tempImageObject = new GameObject("TempGlowImage");
		_tempImageObject.hideFlags = HideFlags.HideAndDontSave;
		_tempImageObject.transform.SetParent(_tempCanvasObject.transform, worldPositionStays: false);
		_tempImageObject.layer = 31;
		_tempImage = _tempImageObject.AddComponent<Image>();
		_tempImage.sprite = sprite;
		_tempImage.color = Color.white;
		_tempImage.raycastTarget = false;
		RectTransform rectTransform = _tempImage.rectTransform;
		rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
		rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
		rectTransform.pivot = new Vector2(0.5f, 0.5f);
		rectTransform.anchoredPosition = Vector2.zero;
		rectTransform.sizeDelta = new Vector2(width, height);
	}

	private void CleanupTempObjects()
	{
		if (_tempImageObject != null)
		{
			_tempImageObject.SetActive(value: false);
			DestroyObject(_tempImageObject);
			_tempImageObject = null;
			_tempImage = null;
		}
		if (_tempCanvasObject != null)
		{
			_tempCanvasObject.SetActive(value: false);
			DestroyObject(_tempCanvasObject);
			_tempCanvasObject = null;
			_tempCanvas = null;
		}
		if (_tempCameraObject != null)
		{
			_tempCameraObject.SetActive(value: false);
			DestroyObject(_tempCameraObject);
			_tempCameraObject = null;
			_tempCamera = null;
		}
	}

	private RenderTexture GenerateSDF(RenderTexture source, int width, int height)
	{
		if (_settings == null)
		{
			return null;
		}
		RenderTexture renderTexture = new RenderTexture(width, height, 0, _settings.GlowEffectFormat);
		renderTexture.enableRandomWrite = true;
		renderTexture.filterMode = _settings.GlowEffectFilterMode;
		renderTexture.Create();
		_seedBuffer = new RenderTexture(width, height, 0, RenderTextureFormat.RGFloat);
		_seedBuffer.enableRandomWrite = true;
		_seedBuffer.Create();
		ComputeShader computeShader = _settings.GlowEffectComputeShader;
		int kernelIndex = computeShader.FindKernel("InitSeeds");
		int kernelIndex2 = computeShader.FindKernel("JumpFlood");
		int kernelIndex3 = computeShader.FindKernel("FinalizeDistance");
		computeShader.SetInt("ResolutionX", width);
		computeShader.SetInt("ResolutionY", height);
		computeShader.SetFloat("MaxDistance", _settings.MaxDistance);
		computeShader.SetTexture(kernelIndex, "Source", source);
		computeShader.SetTexture(kernelIndex, "SeedBuffer", _seedBuffer);
		int threadGroupsX = Mathf.CeilToInt((float)width * 0.125f);
		int threadGroupsY = Mathf.CeilToInt((float)height * 0.125f);
		computeShader.Dispatch(kernelIndex, threadGroupsX, threadGroupsY, 1);
		int num = Mathf.CeilToInt((float)System.Math.Log(Mathf.Max(width, height), 2.0));
		computeShader.SetTexture(kernelIndex2, "Source", source);
		computeShader.SetTexture(kernelIndex2, "SeedBuffer", _seedBuffer);
		for (int i = 0; i < num; i++)
		{
			computeShader.SetInt("StepSize", (int)Mathf.Pow(2f, num - 1 - i));
			computeShader.Dispatch(kernelIndex2, threadGroupsX, threadGroupsY, 1);
		}
		computeShader.SetTexture(kernelIndex3, "Source", source);
		computeShader.SetTexture(kernelIndex3, "SeedBuffer", _seedBuffer);
		computeShader.SetTexture(kernelIndex3, "Result", renderTexture);
		computeShader.Dispatch(kernelIndex3, threadGroupsX, threadGroupsY, 1);
		if (_seedBuffer != null)
		{
			_seedBuffer.Release();
			DestroyObject(_seedBuffer);
			_seedBuffer = null;
		}
		return renderTexture;
	}

	private void ReleaseTextures()
	{
		if (_alphaTexture != null)
		{
			_alphaTexture.Release();
			DestroyObject(_alphaTexture);
			_alphaTexture = null;
		}
		if (_tempAlphaTexture != null)
		{
			_tempAlphaTexture.Release();
			DestroyObject(_tempAlphaTexture);
			_tempAlphaTexture = null;
		}
		if (_seedBuffer != null)
		{
			_seedBuffer.Release();
			DestroyObject(_seedBuffer);
			_seedBuffer = null;
		}
	}
}
