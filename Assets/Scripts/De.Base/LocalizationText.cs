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
	}

	private void OnEnable()
	{
	}

	private void UpdateLocalText()
	{
	}
}
