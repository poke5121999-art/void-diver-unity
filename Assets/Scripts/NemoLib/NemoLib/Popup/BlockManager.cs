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
				}
			}
			if (background)
			{
				_backgroundStackCount--;
				if (_backgroundStackCount < 0)
				{
					// TODO(decomp): message truncated in symbol; assumed pattern @0x182d0e500
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
