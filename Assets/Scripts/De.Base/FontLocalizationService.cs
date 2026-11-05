using GameCommon.Enum;
using TMPro;
using VContainer;
#if UNITY_WEBGL && !UNITY_EDITOR
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
#endif

public class FontLocalizationService : NativeBase
{
	[Inject]
	private FontLocalizationSettings _settings;

	public TMP_FontAsset GetFont(EFont font, ELanguage language)
	{
		if (_settings == null)
		{
			return null;
		}
#if UNITY_WEBGL && !UNITY_EDITOR
		TMP_FontAsset lazy = GetWebLazyFont(font, language);
		if (lazy != null)
		{
			return lazy;
		}
#endif
		return _settings.GetFont(font, language);
	}

#if UNITY_WEBGL && !UNITY_EDITOR
	// Web player, not in the original (2026-11-05): the Japanese and Chinese fonts (NotoSans JP/TC/SC, about 35 MB) are
	// no longer in the first download. WebGLBuild.CutWebDeadWeight takes them out of FontLocalizationSettings, which
	// every page load reads, and they load from their bundles the first time a text asks for one of these languages;
	// until then the text keeps the default font, then OnWebFontLoaded makes LocalizationTextFont apply it.
	public readonly Subject<Unit> OnWebFontLoaded = new Subject<Unit>();

	private readonly Dictionary<string, TMP_FontAsset> _webFonts = new Dictionary<string, TMP_FontAsset>();

	private readonly HashSet<string> _webLoading = new HashSet<string>();

	private static string WebLazyAddress(EFont font, ELanguage language)
	{
		string weight = font == EFont.PretendardBold ? "Bold" : font == EFont.PretendardRegular ? "Regular" : null;
		string script = language == ELanguage.Jp ? "JP" : language == ELanguage.Tw ? "TC" : language == ELanguage.Cn ? "SC" : null;
		return weight == null || script == null ? null : "Fonts/NotoSans_" + weight + "_" + script + ".asset";
	}

	private TMP_FontAsset GetWebLazyFont(EFont font, ELanguage language)
	{
		string address = WebLazyAddress(font, language);
		if (address == null)
		{
			return null;
		}
		if (_webFonts.TryGetValue(address, out TMP_FontAsset loaded))
		{
			return loaded;
		}
		if (_webLoading.Add(address))
		{
			LoadWebFont(address);
		}
		return null;
	}

	private void LoadWebFont(string address)
	{
		long diag = NemoLib.Log.WebDiag.Begin("Font", "load " + address);
		AsyncOperationHandle<TMP_FontAsset> handle = Addressables.LoadAssetAsync<TMP_FontAsset>(address);
		handle.Completed += h =>
		{
			bool ok = h.Status == AsyncOperationStatus.Succeeded && h.Result != null;
			NemoLib.Log.WebDiag.End(diag, ok, ok ? h.Result.name : (h.OperationException != null ? h.OperationException.Message : h.Status.ToString()));
			if (!ok)
			{
				// The default font stays; the next text that asks tries again.
				_webLoading.Remove(address);
				return;
			}
			_webFonts[address] = h.Result;
			OnWebFontLoaded.OnNext(Unit.Default);
		};
	}
#endif
}
