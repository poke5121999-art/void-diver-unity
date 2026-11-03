using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using NemoLib.Log;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace NemoLib.Popup
{
	public class BlockManager : MonoBase
	{
		[SerializeField]
		private GameObject _contents;

		[SerializeField]
		private Image _background;

		[SerializeField]
		private GameObject _calling;

		[SerializeField]
		private float _callingDelay = 0.5f;

		[Inject]
		private SeriLogger _logger;

		[Inject]
		private InputManager _inputManager;

		private int _stackCount;

		private int _callingStackCount;

		private int _backgroundStackCount;

		private CancellationTokenSource _cts;

		private long _diagCalling;

#if UNITY_WEBGL && !UNITY_EDITOR
		// Web player safety release, not in the original (2026-11-03): the spinner block is held by an await (popup
		// open, host request, load); if one of them never ends the game would stay blocked with the spinner for good.
		// After CallingReleaseAfter seconds the block is lifted and the log says so ([VD:Released]); the hang itself is
		// in the log above it ([VD:Pending]).
		private const float CallingReleaseAfter = 30f;

		private const float CallingHangReport = 8f;

		private float _callingSince = -1f;

		private bool _callingHangReported;

		private void Update()
		{
			if (_callingStackCount == 0)
			{
				_callingSince = -1f;
				_callingHangReported = false;
				return;
			}
			if (_callingSince < 0f)
			{
				_callingSince = Time.realtimeSinceStartup;
			}
			float held = Time.realtimeSinceStartup - _callingSince;
			if (!_callingHangReported && held > CallingHangReport)
			{
				_callingHangReported = true;
				NemoLib.Log.WebDiag.Warn("Hang", string.Format("input blocked with the spinner for {0:0} s (blocks {1}, calling {2})", held, _stackCount, _callingStackCount));
			}
			if (held > CallingReleaseAfter)
			{
				NemoLib.Log.WebDiag.Error("Released", string.Format("input block with the spinner lifted after {0:0} s (blocks {1}, calling {2}); something it waited on never finished, see [VD:Pending]", held, _stackCount, _callingStackCount));
				NemoLib.Log.WebDiag.End(_diagCalling, ok: false, "lifted by the safety release");
				_stackCount = 0;
				_callingStackCount = 0;
				_backgroundStackCount = 0;
				DisableCalling();
				SetBackgroundAlpha(0f);
				_contents.SetActive(value: false);
				_inputManager?.UnblockInput();
			}
		}
#endif

		protected override void Awake()
		{
			this.InjectFromRootContainer();
			UnityEngine.Object.DontDestroyOnLoad(this);
		}

		public void Block(bool calling = true, bool background = false)
		{
			_stackCount++;
			if (_stackCount == 1)
			{
				_inputManager?.BlockInput();
				_contents.SetActive(value: true);
			}
			if (calling)
			{
				_callingStackCount++;
				if (_callingStackCount == 1)
				{
					EnableCallingAsync().Forget();
					// Web diagnostics: the spinner and the input block last as long as this; the pending list shows it.
					_diagCalling = NemoLib.Log.WebDiag.Begin("Block", "input blocked with the loading spinner", quiet: true);
				}
			}
			if (background)
			{
				_backgroundStackCount++;
				if (_backgroundStackCount == 1)
				{
					SetBackgroundAlpha(1f);
				}
			}
		}

		public void Release(bool calling = true, bool background = false)
		{
			_stackCount--;
			if (_stackCount < 0)
			{
				_logger.Warning("Release called more than Block");
				_stackCount = 0;
			}
			if (calling)
			{
				_callingStackCount--;
				if (_callingStackCount < 0)
				{
					_logger.Warning("Release(calling) called more than Block(calling)");
					_callingStackCount = 0;
				}
				if (_callingStackCount == 0)
				{
					DisableCalling();
					NemoLib.Log.WebDiag.End(_diagCalling);
				}
			}
			if (background)
			{
				_backgroundStackCount--;
				if (_backgroundStackCount < 0)
				{
					// message string literal and Warning level (EmitGuarded level 3) confirmed @0x182d0e4fd
					_logger.Warning("Release(background) called more than Block(background)");
					_backgroundStackCount = 0;
				}
				if (_backgroundStackCount == 0)
				{
					SetBackgroundAlpha(0f);
				}
			}
			if (_stackCount == 0)
			{
				if (_callingStackCount != 0)
				{
					NemoLib.Log.WebDiag.End(_diagCalling);
				}
				DisableCalling();
				SetBackgroundAlpha(0f);
				_backgroundStackCount = 0;
				_contents.SetActive(value: false);
				_inputManager?.UnblockInput();
			}
		}

		private async UniTaskVoid EnableCallingAsync()
		{
			DisableCalling();
			_cts = new CancellationTokenSource();
			await UniTask.Delay(TimeSpan.FromSeconds(_callingDelay), ignoreTimeScale: false, PlayerLoopTiming.Update, _cts.Token);
			_calling.SetActive(value: true);
		}

		private void DisableCalling()
		{
			_cts?.Cancel();
			_cts?.Dispose();
			_cts = null;
			_calling.SetActive(value: false);
		}

		private void SetBackgroundAlpha(float alpha)
		{
			if (!(_background == null))
			{
				Color color = _background.color;
				color.a = alpha;
				_background.color = color;
			}
		}

		private void OnDestroy()
		{
			DisableCalling();
		}
	}
}
