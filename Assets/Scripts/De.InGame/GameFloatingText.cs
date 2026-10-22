using NemoLib.Log;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class GameFloatingText : MonoBase
{
	[SerializeField]
	private TextMeshProUGUI _text;

	[SerializeField]
	private Image _iconFront;

	[SerializeField]
	private Image _iconBack;

	[SerializeField]
	private float _duration;

	[SerializeField]
	[Range(0f, 3f)]
	private float _randomOffsetY;

	[SerializeField]
	[Range(0f, 3f)]
	private float _randomOffsetX;

	[Inject]
	private SeriLogger _logger;

	public void Init(string text, Vector3 position, bool showFrontIcon = false, bool showBackwardIcon = false)
	{
	}

	private Vector3 GetRandomPositionOffset()
	{
		return default;
	}
}
