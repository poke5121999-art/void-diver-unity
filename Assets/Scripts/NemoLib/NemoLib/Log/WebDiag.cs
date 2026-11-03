using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace NemoLib.Log
{
	// Web player diagnostics, not from the original game (added 2026-11-02 for bugs seen only in players' browsers).
	// Every line goes to the browser console as "[VD:<area>] ...", where the page's log collector (WebGLBuild.
	// AddLogCollector) keeps it in a ring buffer saved to localStorage, so a hang still leaves its log behind.
	//   Begin/End: a timed operation (scene load, popup open, host request, asset load, Lua function). Operations
	//     still running after 10 s are listed every 15 s as "[VD:Pending]", which names what an endless load waits on.
	//   Info/Warn/Error: one event (stage state, quest state, save written).
	// In the editor and the Standalone player all of it compiles to nothing.
	public static class WebDiag
	{
		private sealed class Op
		{
			public long Id;
			public string Area;
			public string What;
			public float Start;
			public bool Quiet;
		}

		private const float PendingAfter = 10f;

		private const float PendingEvery = 15f;

		private static readonly Dictionary<long, Op> s_ops = new Dictionary<long, Op>();

		private static readonly List<KeyValuePair<UnityWebRequest, Op>> s_requests = new List<KeyValuePair<UnityWebRequest, Op>>();

		private static readonly Dictionary<string, int> s_requestCounts = new Dictionary<string, int>();

		private static long s_nextId;

		private static float s_lastPendingReport;

		// ?debug=1 in the page URL: also log the operations that end quickly (every asset load, every bundle request).
		public static bool Verbose { get; private set; }

		public static bool Enabled
		{
			get
			{
#if UNITY_WEBGL && !UNITY_EDITOR
				return true;
#else
				return false;
#endif
			}
		}

#if UNITY_WEBGL && !UNITY_EDITOR
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void Init()
		{
			string url = Application.absoluteURL ?? string.Empty;
			Verbose = url.Contains("debug=1");
			Application.logMessageReceived += OnLog;
			SceneManager.sceneLoaded += (scene, mode) => Info("Scene", string.Format("loaded {0} ({1})", scene.name, mode));
			SceneManager.sceneUnloaded += scene => Info("Scene", "unloaded " + scene.name);
			var go = new GameObject("VD WebDiag");
			go.hideFlags = HideFlags.HideAndDontSave;
			UnityEngine.Object.DontDestroyOnLoad(go);
			go.AddComponent<Ticker>();
			Info("Diag", string.Format("web diagnostics on, verbose={0}, version {1}, {2}", Verbose, Application.version, url));
		}

		private sealed class Ticker : MonoBehaviour
		{
			private void Update()
			{
				Tick();
			}
		}

		// Unity prints an exception's message to the console, but its stack only goes to logMessageReceived.
		private static void OnLog(string condition, string stackTrace, LogType type)
		{
			if ((type == LogType.Exception || type == LogType.Assert) && !string.IsNullOrEmpty(stackTrace))
			{
				Debug.LogWarning("[VD:Stack] " + Trim(condition, 200) + "\n" + Trim(stackTrace, 3000));
			}
		}
#endif

		public static void Info(string area, string message)
		{
#if UNITY_WEBGL && !UNITY_EDITOR
			Write("[VD:" + area + "] " + message);
#endif
		}

		// SeriLogger.Init sets Debug.unityLogger.filterLogType to warnings, which drops Debug.Log; the log handler
		// below the filter still prints to the browser console, where the page's collector keeps it.
		private static void Write(string line)
		{
			Debug.unityLogger.logHandler.LogFormat(LogType.Log, null, "{0}", line);
		}

		public static void Warn(string area, string message)
		{
#if UNITY_WEBGL && !UNITY_EDITOR
			Debug.LogWarning("[VD:" + area + "] " + message);
#endif
		}

		public static void Error(string area, string message)
		{
#if UNITY_WEBGL && !UNITY_EDITOR
			Debug.LogError("[VD:" + area + "] " + message);
#endif
		}

		// quiet: log the begin and a quick end only with ?debug=1 (slow, failed and pending ones are always logged).
		public static long Begin(string area, string what, bool quiet = false)
		{
#if UNITY_WEBGL && !UNITY_EDITOR
			long id = ++s_nextId;
			s_ops[id] = new Op { Id = id, Area = area, What = what, Start = Time.realtimeSinceStartup, Quiet = quiet };
			if (!quiet || Verbose)
			{
				Write("[VD:" + area + "] begin " + what);
			}
			return id;
#else
			return 0;
#endif
		}

		public static void End(long id, bool ok = true, string result = null)
		{
#if UNITY_WEBGL && !UNITY_EDITOR
			if (!s_ops.TryGetValue(id, out var op))
			{
				return;
			}
			s_ops.Remove(id);
			float seconds = Time.realtimeSinceStartup - op.Start;
			string line = string.Format("[VD:{0}] {1} {2} in {3:0.00} s{4}", op.Area, ok ? "end" : "FAILED", op.What, seconds, string.IsNullOrEmpty(result) ? string.Empty : " - " + result);
			if (!ok)
			{
				Debug.LogError(line);
			}
			else if (!op.Quiet || Verbose || seconds >= 3f)
			{
				Write(line);
			}
#endif
		}

		// Bundle downloads (Addressables.WebRequestOverride): start, result, size and time of each request; a second
		// request for the same URL is a retry.
		public static void TrackRequest(UnityWebRequest request)
		{
#if UNITY_WEBGL && !UNITY_EDITOR
			if (request == null)
			{
				return;
			}
			string url = request.url ?? "?";
			s_requestCounts.TryGetValue(url, out int count);
			s_requestCounts[url] = ++count;
			string name = ShortUrl(url);
			long id = Begin("Bundle", count > 1 ? string.Format("{0} (retry {1})", name, count - 1) : name, quiet: count == 1);
			s_requests.Add(new KeyValuePair<UnityWebRequest, Op>(request, s_ops[id]));
#endif
		}

		private static void Tick()
		{
			float now = Time.realtimeSinceStartup;
			for (int i = s_requests.Count - 1; i >= 0; i--)
			{
				UnityWebRequest request = s_requests[i].Key;
				Op op = s_requests[i].Value;
				bool done;
				string result;
				bool ok;
				try
				{
					done = request.isDone;
					ok = request.result == UnityWebRequest.Result.Success;
					result = done ? string.Format("{0} HTTP {1}, {2} bytes{3}", request.result, request.responseCode, request.downloadedBytes, string.IsNullOrEmpty(request.error) ? string.Empty : ", " + request.error) : null;
				}
				catch (Exception)
				{
					// Addressables disposed the request: it finished, its result went to the bundle load.
					done = true;
					ok = true;
					result = "request disposed";
				}
				if (done)
				{
					s_requests.RemoveAt(i);
					End(op.Id, ok, result);
				}
			}
			if (now - s_lastPendingReport < PendingEvery)
			{
				return;
			}
			s_lastPendingReport = now;
			StringBuilder sb = null;
			int n = 0;
			foreach (Op op in s_ops.Values)
			{
				float age = now - op.Start;
				if (age < PendingAfter)
				{
					continue;
				}
				sb = sb ?? new StringBuilder();
				if (n < 25)
				{
					sb.AppendFormat("\n  {0:0} s  {1}: {2}", age, op.Area, op.What);
				}
				n++;
			}
			if (sb != null)
			{
				Debug.LogWarning(string.Format("[VD:Pending] {0} operations running for over {1:0} s, scene {2}:{3}", n, PendingAfter, SceneManager.GetActiveScene().name, sb));
			}
		}

		private static string ShortUrl(string url)
		{
			int slash = url.LastIndexOf('/');
			return slash >= 0 && slash < url.Length - 1 ? url.Substring(slash + 1) : url;
		}

		private static string Trim(string text, int max)
		{
			return text.Length <= max ? text : text.Substring(0, max) + "...";
		}
	}
}
