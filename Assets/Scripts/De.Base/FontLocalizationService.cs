using GameCommon.Enum;
using TMPro;
using VContainer;

public class FontLocalizationService : NativeBase
{
	[Inject]
	private FontLocalizationSettings _settings;

	public TMP_FontAsset GetFont(EFont font, ELanguage language)
	{
		return null;
	}
}
