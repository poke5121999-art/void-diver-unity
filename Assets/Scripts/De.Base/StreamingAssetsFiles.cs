using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

// The game reads its tables and Lua with File and Directory calls under the StreamingAssets folder. In the web player
// that folder is a URL, so PrepareAsync downloads the files listed in ListName (written by the WebGL build) into the
// browser's in-memory file system once, and Root points there. The build packs them into PackName, one download; each
// line of ListName is "<path>\t<length>" in pack order. Everywhere else Root is the real folder and
// PrepareAsync has nothing to do.
public static class StreamingAssetsFiles
{
	public const string ListName = "vd-streaming-files.txt";

	public const string PackName = "vd-streaming-pack.txt";

#if UNITY_WEBGL && !UNITY_EDITOR
	public static string Root => "/tmp/vd-streaming";

	private static UniTask _prepare;

	private static bool _started;

	public static UniTask PrepareAsync()
	{
		if (!_started)
		{
			_started = true;
			_prepare = DownloadAllAsync().Preserve();
		}
		return _prepare;
	}

	private static async UniTask DownloadAllAsync()
	{
		float start = Time.realtimeSinceStartup;
		string[] lines = System.Text.Encoding.UTF8.GetString(await GetAsync(ListName)).Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
		byte[] pack = await GetAsync(PackName);
		int offset = 0;
		foreach (string line in lines)
		{
			int tab = line.LastIndexOf('\t');
			string relativePath = line.Substring(0, tab);
			int length = int.Parse(line.Substring(tab + 1));
			if (offset + length > pack.Length)
			{
				throw new IOException($"StreamingAssets pack ends at {pack.Length} bytes, before {relativePath} ({offset}+{length})");
			}
			string path = Path.Combine(Root, relativePath);
			Directory.CreateDirectory(Path.GetDirectoryName(path));
			using (FileStream stream = File.Create(path))
			{
				stream.Write(pack, offset, length);
			}
			offset += length;
		}
		Debug.Log($"[StreamingAssetsFiles] {lines.Length} files, {offset / 1024} KB in {Time.realtimeSinceStartup - start:F1}s into {Root}");
	}

	// A static host answers a request with a 503 now and then (GitHub Pages, 2026-10-30): retry with a growing wait.
	private static async UniTask<byte[]> GetAsync(string relativePath)
	{
		string url = Application.streamingAssetsPath + "/" + Uri.EscapeUriString(relativePath);
		const int Attempts = 5;
		for (int attempt = 1; ; attempt++)
		{
			using UnityWebRequest request = UnityWebRequest.Get(url);
			try
			{
				await request.SendWebRequest();
				return request.downloadHandler.data;
			}
			catch (UnityWebRequestException e) when (attempt < Attempts && e.ResponseCode != 404)
			{
				Debug.LogWarning($"[StreamingAssetsFiles] {relativePath}: {e.Error} (attempt {attempt} of {Attempts}), retrying");
			}
			catch (UnityWebRequestException e)
			{
				throw new IOException($"StreamingAssets file {relativePath} was not downloaded from {url}: {e.Error}", e);
			}
			await UniTask.Delay(TimeSpan.FromSeconds(1 << attempt));
		}
	}
#else
	public static string Root => Application.streamingAssetsPath;

	public static UniTask PrepareAsync() => UniTask.CompletedTask;
#endif
}
