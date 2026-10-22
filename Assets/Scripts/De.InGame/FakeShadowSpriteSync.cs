using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FakeShadowCaster))]
public class FakeShadowSpriteSync : MonoBehaviour
{
	[SerializeField]
	private Image _targetImage;

	[Tooltip("그림자 월드 위치의 소스 Transform. 비어있으면 자기 자신을 사용합니다.")]
	[SerializeField]
	private Transform _shadowTransform;

	private FakeShadowCaster _caster;

	private Sprite _lastSprite;

	private void OnEnable()
	{
		_caster = GetComponent<FakeShadowCaster>();
		_caster.SetSpriteTransform((_shadowTransform != null) ? _shadowTransform : base.transform);
		SyncSprite();
	}

	private void LateUpdate()
	{
		SyncSprite();
	}

	private void SyncSprite()
	{
		if (!(_targetImage == null) && !(_caster == null))
		{
			Sprite sprite = _targetImage.sprite;
			if (sprite != _lastSprite)
			{
				_lastSprite = sprite;
				_caster.SetSprite(sprite);
			}
		}
	}
}
