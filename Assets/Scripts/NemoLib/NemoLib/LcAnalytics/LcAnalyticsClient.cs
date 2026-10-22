using System;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using NemoLib.Log;
using Newtonsoft.Json;
using VContainer.Unity;
using UnityEngine.Networking;
using VContainer;

namespace NemoLib.LcAnalytics
{
	public class LcAnalyticsClient : NativeBase, IDisposable
	{
		private const string _trackPath = "/v1/track";

		private const string _contentTypeJson = "application/json";

		private const string _logSourceContext = "[LcAnalyticsClient]";

		[Inject]
		private SeriLogger _logger;

		private readonly CancellationTokenSource _cts;

		private readonly SynchronizationContext _mainContext;

		private readonly string _trackUrl;

		private readonly string _authHeader;

		private readonly int _timeoutSec;

		private bool IsConfigured => !string.IsNullOrEmpty(_trackUrl) && !string.IsNullOrEmpty(_authHeader);

		public LcAnalyticsClient(string endpoint, string writeKey, int timeoutSec = 5)
		{
			_cts = new CancellationTokenSource();
			_mainContext = SynchronizationContext.Current;
			this.InjectFromRootContainer();
			_trackUrl = (string.IsNullOrEmpty(endpoint) ? null : (endpoint.TrimEnd('/') + _trackPath));
			_authHeader = BuildBasicAuth(writeKey);
			_timeoutSec = ((timeoutSec > 0) ? timeoutSec : 1);
		}

		public void Send(LcAnalyticsEvent evt)
		{
			if (TryGetActiveToken(out var token))
			{
				RunOnMain(delegate
				{
					SendInternalAsync(evt, token).Forget();
				});
			}
		}

		private void RunOnMain(Action action)
		{
			if (_mainContext != null && SynchronizationContext.Current != _mainContext)
			{
				_mainContext.Post(delegate
				{
					action();
				}, null);
			}
			else
			{
				action();
			}
		}

		private bool TryGetActiveToken(out CancellationToken token)
		{
			if (_cts == null)
			{
				token = default(CancellationToken);
				return false;
			}
			token = _cts.Token;
			return !token.IsCancellationRequested;
		}

		public void Dispose()
		{
			_cts?.Cancel();
		}

		private async UniTaskVoid SendInternalAsync(LcAnalyticsEvent evt, CancellationToken ct)
		{
			if (evt == null || string.IsNullOrEmpty(evt.Event))
			{
				return;
			}
			if (string.IsNullOrEmpty(_trackUrl) || string.IsNullOrEmpty(_authHeader))
			{
				_logger.EnqueueWarning("Not initialized. Provide endpoint and writeKey.", _logSourceContext);
				return;
			}
			string json = JsonConvert.SerializeObject(evt);
			_logger.EnqueueDebug("\u2192 " + evt.Event + ": " + json, _logSourceContext);
			using (UnityWebRequest request = new UnityWebRequest(_trackUrl, "POST"))
			{
				request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json))
				{
					contentType = _contentTypeJson
				};
				request.downloadHandler = new DownloadHandlerBuffer();
				request.SetRequestHeader("Accept", _contentTypeJson);
				request.SetRequestHeader("Authorization", _authHeader);
				request.timeout = _timeoutSec;
				UnityWebRequestAsyncOperation operation = request.SendWebRequest();
				bool aborted = false;
				while (!operation.isDone)
				{
					if (!aborted && ct.IsCancellationRequested)
					{
						aborted = true;
						request.Abort();
					}
					await UniTask.Yield();
				}
				if (aborted || ct.IsCancellationRequested || request.result == UnityWebRequest.Result.Success)
				{
					return;
				}
				string error = ((request.responseCode > 0) ? string.Format("{0}: {1}", request.responseCode, request.downloadHandler.text) : request.error);
				_logger.EnqueueWarning("\u2717 " + evt.Event + ": " + error, _logSourceContext);
			}
		}

		private static string BuildBasicAuth(string writeKey)
		{
			if (string.IsNullOrEmpty(writeKey))
			{
				return string.Empty;
			}
			return "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(writeKey + ":"));
		}
	}
}
