using System;
using System.Collections.Generic;
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

	public IReadOnlyList<TMP_FontAsset> DynamicPrewarmFonts => null;

	public TMP_FontAsset GetFont(EFont font, ELanguage language)
	{
		return null;
	}
}
