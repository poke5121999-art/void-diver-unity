using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon.Enum;
using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/FontLocalizationSettings")]
public class FontLocalizationSettings : ScriptableObject
{
	[Serializable]
	public class LanguageFont
	{
		public ELanguage Language;

		public TMP_FontAsset Font;
	}

	[Serializable]
	public class FontInfo
	{
		public EFont Font;

		public TMP_FontAsset DefaultFont;

		public List<LanguageFont> LanguageFonts;
	}

	[SerializeField]
	private FontInfo[] _fontInfos;

	[SerializeField]
	private List<TMP_FontAsset> _dynamicPrewarmFonts;

	public IReadOnlyList<TMP_FontAsset> DynamicPrewarmFonts => _dynamicPrewarmFonts;

	public TMP_FontAsset GetFont(EFont font, ELanguage language)
	{
		if (font != EFont.None && _fontInfos != null && _fontInfos.Length != 0)
		{
			FontInfo fontInfo = _fontInfos.FirstOrDefault((FontInfo x) => x.Font == font);
			if (fontInfo != null)
			{
				if (fontInfo.LanguageFonts != null)
				{
					LanguageFont languageFont = fontInfo.LanguageFonts.FirstOrDefault((LanguageFont x) => x.Language == language && x.Font != null);
					if (languageFont != null)
					{
						return languageFont.Font;
					}
				}
				return fontInfo.DefaultFont;
			}
		}
		return null;
	}
}
