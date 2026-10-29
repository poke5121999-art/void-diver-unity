using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

// The game reads its tables and Lua with File and Directory calls under the StreamingAssets folder. In the web player
// that folder is a URL, so PrepareAsync downloads the files listed in ListName (written by the WebGL build) into the
// browser's in-memory file system once, and Root points there. Everywhere else Root is the real folder and
// PrepareAsync has nothing to do.
public static class StreamingAssetsFiles
{
	public const string ListName = "vd-streaming-files.txt";

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
		string[] files = System.Text.Encoding.UTF8.GetString(await GetAsync(ListName)).Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
		long total = 0;
		const int Parallel = 8;
		for (int i = 0; i < files.Length; i += Parallel)
		{
			var batch = new List<UniTask<byte[]>>();
			for (int j = i; j < Math.Min(i + Parallel, files.Length); j++)
			{
				batch.Add(GetAsync(files[j]));
			}
			byte[][] contents = await UniTask.WhenAll(batch);
			for (int k = 0; k < contents.Length; k++)
			{
				string path = Path.Combine(Root, files[i + k]);
				Directory.CreateDirectory(Path.GetDirectoryName(path));
				File.WriteAllBytes(path, contents[k]);
				total += contents[k].Length;
			}
		}
		Debug.Log($"[StreamingAssetsFiles] {files.Length} files, {total / 1024} KB in {Time.realtimeSinceStartup - start:F1}s into {Root}");
	}

	private static async UniTask<byte[]> GetAsync(string relativePath)
	{
		string url = Application.streamingAssetsPath + "/" + Uri.EscapeUriString(relativePath);
		using UnityWebRequest request = UnityWebRequest.Get(url);
		try
		{
			await request.SendWebRequest();
		}
		catch (UnityWebRequestException e)
		{
			throw new IOException($"StreamingAssets file {relativePath} was not downloaded from {url}: {e.Error}", e);
		}
		return request.downloadHandler.data;
	}
#else
	public static string Root => Application.streamingAssetsPath;

	public static UniTask PrepareAsync() => UniTask.CompletedTask;
#endif
}
