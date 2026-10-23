using UniRx;
using GameCommon.Enum;
using TMPro;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizationText : MonoBase
{
	[Inject]
	private GamePlaySettingService _gamePlaySettings;

	public string Key;

	private ELanguage _appliedLanguage;

	private void Start()
	{
		UpdateLocalText();
		_gamePlaySettings.OnLanguageApplied.Subscribe(delegate
		{
			UpdateLocalText();
		}).AddTo(this);
	}

	private void OnEnable()
	{
		if (_gamePlaySettings != null && _appliedLanguage != _gamePlaySettings.Language)
		{
			UpdateLocalText();
		}
	}

	private void UpdateLocalText()
	{
		if (!LocalizationUtility.TryGetLocalizedText(Key, out var text))
		{
			Debug.LogWarning("Localized Text not found. Key: " + Key, base.gameObject);
		}
		GetComponent<TextMeshProUGUI>().text = text;
		_appliedLanguage = ((_gamePlaySettings != null) ? _gamePlaySettings.Language : ELanguage.None);
	}
}
