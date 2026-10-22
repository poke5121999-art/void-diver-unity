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
	}

	private void Start()
	{
	}

	private void OnEnable()
	{
	}

	private void ApplyFont()
	{
	}
}
