using UniRx;
using GameCommon.Enum;
using TMPro;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizationTextFont : MonoBase
{
	[Inject]
	private GamePlaySettingService _gamePlaySettings;

	[Inject]
	private FontLocalizationService _fontService;

	[SerializeField]
	private EFont _font;

	private TextMeshProUGUI _text;

	private ELanguage _appliedLanguage;

	protected override void Awake()
	{
		base.Awake();
		_text = GetComponent<TextMeshProUGUI>();
	}

	private void Start()
	{
		ApplyFont();
		_gamePlaySettings.OnLanguageApplied.Subscribe(delegate
		{
			ApplyFont();
		}).AddTo(this);
	}

	private void OnEnable()
	{
		if (_gamePlaySettings != null && _appliedLanguage != _gamePlaySettings.Language)
		{
			ApplyFont();
		}
	}

	private void ApplyFont()
	{
		ELanguage eLanguage = ((_gamePlaySettings != null) ? _gamePlaySettings.Language : ELanguage.None);
		_appliedLanguage = eLanguage;
		if (_font == EFont.None || _fontService == null)
		{
			return;
		}
		TMP_FontAsset font = _fontService.GetFont(_font, eLanguage);
		if (!(font == null))
		{
			if (_text != null && _text.font != null && _text.font.name == font.name)
			{
				return;
			}
			if (_text != null)
			{
				_text.font = font;
			}
		}
	}
}
