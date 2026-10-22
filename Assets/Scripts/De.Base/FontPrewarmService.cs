using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameCommon.Enum;
using GameCommon.Table;
using UnityEngine;
using TMPro;
using VContainer;

public class FontPrewarmService : NativeBase, IDisposable
{
	private const int CHUNK_SIZE = 256;

	[Inject]
	private FontLocalizationService _fontService;

	[Inject]
	private FontLocalizationSettings _settings;

	[Inject]
	private GamePlaySettingService _gamePlaySettings;

	private ELanguage _prewarmedLanguage;

	private CancellationTokenSource _cts;

	public async UniTask PrewarmAsync()
	{
		ELanguage language = _gamePlaySettings.Language;
		if (_prewarmedLanguage == language)
		{
			return;
		}
		_cts?.Cancel();
		_cts?.Dispose();
		_cts = new CancellationTokenSource();
		CancellationToken token = _cts.Token;
		List<TMP_FontAsset> dynamicFonts = CollectDynamicFonts();
		if (dynamicFonts.Count == 0)
		{
			return;
		}
		HashSet<char> targetChars = CollectDynamicTargetChars(language);
		if (targetChars.Count == 0)
		{
			return;
		}
		Stopwatch stopwatch = Stopwatch.StartNew();
		foreach (TMP_FontAsset item in dynamicFonts)
		{
			await AddCharactersChunked(item, targetChars, token);
		}
		stopwatch.Stop();
		UnityEngine.Debug.Log(string.Format("[FontPrewarm] {0}: {1} chars \u00d7 {2} dynamic fonts ", language, targetChars.Count, dynamicFonts.Count) + string.Format("baked in {0}ms", stopwatch.ElapsedMilliseconds));
		_prewarmedLanguage = language;
	}

	public void Dispose()
	{
		_cts?.Cancel();
		_cts?.Dispose();
		_cts = null;
	}

	private HashSet<char> CollectUsedCharacters()
	{
		HashSet<char> hashSet = new HashSet<char>();
		foreach (TLocalizedText value in TLocalizedText.Table.Values)
		{
			if (LocalizationUtility.TryGetLocalizedText(value.Id, out var text))
			{
				for (int i = 0; i < text.Length; i++)
				{
					hashSet.Add(text[i]);
				}
			}
		}
		return hashSet;
	}

	private List<TMP_FontAsset> CollectDynamicFonts()
	{
		IReadOnlyList<TMP_FontAsset> dynamicPrewarmFonts;
		if (_settings == null || (dynamicPrewarmFonts = _settings.DynamicPrewarmFonts) == null)
		{
			return new List<TMP_FontAsset>();
		}
		return dynamicPrewarmFonts.Where((TMP_FontAsset f) => f != null && f.atlasPopulationMode != AtlasPopulationMode.Static).Distinct().ToList();
	}

	private HashSet<char> CollectDynamicTargetChars(ELanguage language)
	{
		HashSet<char> hashSet = CollectUsedCharacters();
		if (hashSet.Count == 0)
		{
			return hashSet;
		}
		List<TMP_FontAsset> staticFonts = CollectStaticFonts(language);
		HashSet<char> hashSet2 = new HashSet<char>();
		foreach (char c in hashSet)
		{
			char ch = c;
			if (!staticFonts.Any((TMP_FontAsset f) => f != null && f.HasCharacter(ch, searchFallbacks: false, tryAddCharacter: false)))
			{
				hashSet2.Add(c);
			}
		}
		return hashSet2;
	}

	private List<TMP_FontAsset> CollectStaticFonts(ELanguage language)
	{
		HashSet<TMP_FontAsset> hashSet = new HashSet<TMP_FontAsset>();
		EFont[] array = new EFont[2] { EFont.PretendardBold, EFont.PretendardRegular };
		for (int i = 0; i < array.Length; i++)
		{
			TMP_FontAsset font = _fontService.GetFont(array[i], language);
			if (!(font == null))
			{
				List<TMP_FontAsset> list = new List<TMP_FontAsset>();
				HashSet<TMP_FontAsset> visited = new HashSet<TMP_FontAsset>();
				CollectChain(font, list, visited);
				foreach (TMP_FontAsset item in list)
				{
					if (item.atlasPopulationMode == AtlasPopulationMode.Static)
					{
						hashSet.Add(item);
					}
				}
			}
		}
		return hashSet.ToList();
	}

	private void CollectChain(TMP_FontAsset font, List<TMP_FontAsset> chain, HashSet<TMP_FontAsset> visited)
	{
		if (font == null || !visited.Add(font))
		{
			return;
		}
		chain.Add(font);
		List<TMP_FontAsset> fallbackFontAssetTable = font.fallbackFontAssetTable;
		if (fallbackFontAssetTable == null)
		{
			return;
		}
		foreach (TMP_FontAsset item in fallbackFontAssetTable)
		{
			CollectChain(item, chain, visited);
		}
	}

	private async UniTask AddCharactersChunked(TMP_FontAsset font, HashSet<char> chars, CancellationToken ct)
	{
		char[] buffer = new char[256];
		int count = 0;
		foreach (char c in chars)
		{
			buffer[count++] = c;
			if (count >= 256)
			{
				font.TryAddCharacters(new string(buffer, 0, count), out var _, includeFontFeatures: false);
				count = 0;
				await UniTask.Yield(PlayerLoopTiming.Update, ct);
			}
		}
		if (count > 0)
		{
			font.TryAddCharacters(new string(buffer, 0, count), out var _, includeFontFeatures: false);
		}
	}
}
