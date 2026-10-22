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
	private Color _glowColor;

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

	private void OnEnable()
	{
	}

	private void LateUpdate()
	{
	}

	private void OnDisable()
	{
	}

	private void OnDestroy()
	{
	}

	private void OnValidate()
	{
	}

	private void CacheImageProperties()
	{
	}

	private void CacheRectTransformProperties()
	{
	}

	private void CacheSdfProperties()
	{
	}

	private void CacheGlowProperties()
	{
	}

	private bool HasImagePropertiesChanged()
	{
		return false;
	}

	private bool HasRectTransformChanged()
	{
		return false;
	}

	private bool HasSdfPropertiesChanged()
	{
		return false;
	}

	private bool HasGlowPropertiesChanged()
	{
		return false;
	}

	private void CreateOrUpdateOverlay()
	{
	}

	private void UpdateRectTransform()
	{
	}

	private Vector2 CalculatePaddingOffset()
	{
		return default;
	}

	private void UpdateGlowColor()
	{
	}

	private void DestroyOverlay()
	{
	}

	private void GenerateSDFTexture()
	{
	}

	private RenderTexture RenderSpriteWithCamera(Sprite sprite, int width, int height, int padding)
	{
		return null;
	}

	private void SetupTempCamera(int width, int height)
	{
	}

	private void SetupTempImage(Sprite sprite, int width, int height, int padding)
	{
	}

	private void CleanupTempObjects()
	{
	}

	private RenderTexture GenerateSDF(RenderTexture source, int width, int height)
	{
		return null;
	}

	private void ReleaseTextures()
	{
	}
}
