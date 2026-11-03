using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace NemoLib.Popup
{
	public class PopupManager : MonoBase
	{
		[SerializeField]
		private Canvas _canvas;

		[SerializeField]
		private Canvas _overlayCanvas;

		[SerializeField]
		private Transform _cacheRootTf;

		[SerializeField]
		private string _defaultOpenSfx;

		[SerializeField]
		private string _defaultCloseSfx;

		[Inject]
		private SoundManager _soundManager;

		[Inject]
		private AssetManager _assetManager;

		[Inject]
		private CameraManager _cameraManager;

		private readonly List<BasePopupPresenter> _popups = new List<BasePopupPresenter>();

		private readonly HashSet<Type> _cacheTypeSet = new HashSet<Type>();

		private readonly Dictionary<Type, BasePopupPresenter> _cache = new Dictionary<Type, BasePopupPresenter>();

		private readonly SemaphoreSlim _openSemaphore = new SemaphoreSlim(1, 1);

#if UNITY_WEBGL && !UNITY_EDITOR
		// Port fix for the web player, not in the original (2026-11-03): the endless "Calling" spinner. In the .NET
		// class library the player uses, SemaphoreSlim.Release hands a waiting WaitAsync to the thread pool
		// (QueueWaiterTask), and the web player has no threads: when two popups open at once (the reputation level-up
		// popup and "New Feature Unlocked", a quest turn-in and its unlock popup), the second waits for good, holding
		// the input block. On the web the open slot is a main-thread queue: same order, one open at a time, and the
		// next waiter runs on the next frame. A waiter that waits over OpenSlotTimeout (an open that never ends) goes
		// on without the slot rather than block the game for good, and says so in the log.
		private const float OpenSlotTimeout = 15f;

		private bool _openSlotBusy;

		private readonly Queue<UniTaskCompletionSource> _openSlotWaiters = new Queue<UniTaskCompletionSource>();

		private async UniTask AcquireOpenSlotAsync(string popupName)
		{
			if (!_openSlotBusy)
			{
				_openSlotBusy = true;
				return;
			}
			var waiter = new UniTaskCompletionSource();
			_openSlotWaiters.Enqueue(waiter);
			float start = Time.realtimeSinceStartup;
			int index = await UniTask.WhenAny(waiter.Task, UniTask.Delay(TimeSpan.FromSeconds(OpenSlotTimeout), ignoreTimeScale: true));
			if (index != 0)
			{
				// Timed out: leave the queue (a later release must not hand the slot to us) and go on without it.
				waiter.TrySetCanceled();
				NemoLib.Log.WebDiag.Error("Released", string.Format("popup {0} waited {1:0} s for the open slot; opening it anyway so the game does not stay blocked", popupName, Time.realtimeSinceStartup - start));
				_openSlotBusy = true;
			}
		}

		private void ReleaseOpenSlot()
		{
			while (_openSlotWaiters.Count > 0)
			{
				UniTaskCompletionSource next = _openSlotWaiters.Dequeue();
				if (next.Task.Status == UniTaskStatus.Pending)
				{
					// The slot passes straight to the next waiter (it stays busy); it resumes on the next frame.
					HandOverNextFrameAsync(next).Forget();
					return;
				}
			}
			_openSlotBusy = false;
		}

		private static async UniTaskVoid HandOverNextFrameAsync(UniTaskCompletionSource next)
		{
			await UniTask.Yield();
			next.TrySetResult();
		}
#endif

		private bool _skipCloseSfx;

		public readonly Subject<BasePopupPresenter> OnBeforeOpenPopup = new Subject<BasePopupPresenter>();

		public readonly Subject<BasePopupPresenter> OnPopupOpened = new Subject<BasePopupPresenter>();

		public readonly Subject<BasePopupPresenter> OnPopupClosed = new Subject<BasePopupPresenter>();

		public readonly Subject<Unit> OnAllPopupsClosed = new Subject<Unit>();

		public readonly Subject<Unit> OnBlurEffectChanged = new Subject<Unit>();

		private Transform RootTf => _canvas.transform;

		private Transform OverlayRootTf => _overlayCanvas.transform;

		public Canvas OverlayCanvas => _overlayCanvas;

		public void HideCanvas()
		{
			_canvas.gameObject.SetActive(value: false);
			_overlayCanvas.gameObject.SetActive(value: false);
		}

		public void ShowCanvas()
		{
			_canvas.gameObject.SetActive(value: true);
			_overlayCanvas.gameObject.SetActive(value: true);
		}

		public void RefreshBlurEffect()
		{
			OnBlurEffectChanged.OnNext(Unit.Default);
		}

		protected override void Awake()
		{
			this.InjectFromRootContainer();
			UnityEngine.Object.DontDestroyOnLoad(this);
		}

		private void Start()
		{
			_canvas.worldCamera = _cameraManager.UiCamera;
			Canvas.ForceUpdateCanvases();
		}

		private async UniTask<T> InstantiatePopupAsync<T>(Transform rootTf) where T : BasePopupPresenter
		{
			// Shared generic body: PopupManager.<InstantiatePopupAsync>d__29<object>.MoveNext @0x1827C20B0.
			// The Type getter is vtable slot 0x378; Split('.') implies FullName.
			string name = typeof(T).FullName.Split('.').LastOrDefault().Replace("Presenter", string.Empty);
			GameObject prefab = await _assetManager.GetRecentlyCacheAsync<GameObject>("Popup", "Popup/" + name + ".prefab");
			return UnityEngine.Object.Instantiate(prefab, rootTf).GetComponent<T>();
		}

		private async UniTask<T> EnsurePopupAsync<T>() where T : BasePopupPresenter
		{
			// Shared generic body: PopupManager.<EnsurePopupAsync>d__30<object>.MoveNext @0x1826F9960.
			if (_cache.Remove(typeof(T), out var cached))
			{
				return (T)cached;
			}
			T popup = await InstantiatePopupAsync<T>(_cacheRootTf);
			popup.OnClose.Subscribe(OnClose).AddTo(popup);
			return popup;
		}

		public async UniTask PreloadAsync<T>() where T : BasePopupPresenter
		{
			// Shared generic body: PopupManager.<PreloadAsync>d__31<object>.MoveNext @0x180F85610.
			Type type = typeof(T);
			if (_cacheTypeSet.Add(type))
			{
				T popup = await EnsurePopupAsync<T>();
				popup.transform.SetParent(_cacheRootTf, worldPositionStays: false);
				popup.gameObject.SetActive(value: false);
				_cache[type] = popup;
			}
		}

		public T FindPopup<T>() where T : BasePopupPresenter
		{
			return _popups.OfType<T>().FirstOrDefault();
		}

		public IEnumerable<T> FindAllPopup<T>() where T : BasePopupPresenter
		{
			return _popups.OfType<T>();
		}

		public bool ExistPopup<T>() where T : BasePopupPresenter
		{
			return _popups.OfType<T>().FirstOrDefault() != null;
		}

		public bool ExistPopup()
		{
			return _popups.Any();
		}

		public bool ExistInputActionPopup()
		{
			return _popups.Any((BasePopupPresenter p) => p.IsInputActionPopup);
		}

		public bool IsLastPopup(BasePopupPresenter popupPresenter)
		{
			return _popups.LastOrDefault() == popupPresenter;
		}

		public void Open<T>(Model model = null) where T : BasePopupPresenter
		{
			OpenAsync<T>(model).Forget();
		}

		public async UniTask<T> OpenAsync<T>(Model model = null) where T : BasePopupPresenter
		{
			// Shared generic body: PopupManager.<OpenAsync>d__39<object>.MoveNext @0x180F82B70.
			using BlockScope blockScope = new BlockScope(calling: true, background: false);
			// Web diagnostics: an open that never ends holds the semaphore and the input block (spinner) for good,
			// and every later popup waits behind it; the pending list names the stuck one and its step.
			string diagName = typeof(T).Name;
			long diagWait = NemoLib.Log.WebDiag.Begin("Popup", "wait for open slot: " + diagName, quiet: true);
#if UNITY_WEBGL && !UNITY_EDITOR
			await AcquireOpenSlotAsync(diagName);
#else
			await _openSemaphore.WaitAsync();
#endif
			NemoLib.Log.WebDiag.End(diagWait);
			long diag = NemoLib.Log.WebDiag.Begin("Popup", "open " + diagName);
			try
			{
				long diagLoad = NemoLib.Log.WebDiag.Begin("Popup", "load prefab " + diagName, quiet: true);
				T presenter = await EnsurePopupAsync<T>();
				NemoLib.Log.WebDiag.End(diagLoad, presenter != null);
				presenter.ResetForReuse();
				presenter.gameObject.SetActive(value: true);
				if (model != null)
				{
					presenter.Inject(model);
				}
				BasePopupPresenter previous = _popups.LastOrDefault();
				if (previous != null)
				{
					previous.transform.parent = RootTf;
					previous.transform.localScale = Vector3.one;
					previous.transform.localPosition = Vector3.zero;
					previous.GetComponent<RectTransform>().offsetMin = Vector2.zero;
					previous.GetComponent<RectTransform>().offsetMax = Vector2.zero;
					previous.GetComponent<BasePopupPresenter>().DeactivatePopup();
				}
				_popups.Add(presenter);
				presenter.transform.parent = OverlayRootTf;
				presenter.transform.localScale = Vector3.one;
				presenter.transform.localPosition = Vector3.zero;
				presenter.GetComponent<RectTransform>().offsetMin = Vector2.zero;
				presenter.GetComponent<RectTransform>().offsetMax = Vector2.zero;
				OnBeforeOpenPopup.OnNext(presenter);
				long diagOpen = NemoLib.Log.WebDiag.Begin("Popup", "OnOpenAsync " + diagName, quiet: true);
				await presenter.OpenAsync();
				NemoLib.Log.WebDiag.End(diagOpen);
				presenter.ActivatePopup();
				string sfx = (string.IsNullOrEmpty(presenter.OpenSfx) ? _defaultOpenSfx : presenter.OpenSfx);
				_soundManager.PlaySfx(sfx);
				OnPopupOpened.OnNext(presenter);
				NemoLib.Log.WebDiag.End(diag);
				return presenter;
			}
			catch (Exception e)
			{
				NemoLib.Log.WebDiag.End(diag, ok: false, e.ToString());
				throw;
			}
			finally
			{
#if UNITY_WEBGL && !UNITY_EDITOR
				ReleaseOpenSlot();
#else
				_openSemaphore.Release();
#endif
			}
		}

		private void OnClose(BasePopupPresenter target)
		{
			NemoLib.Log.WebDiag.Info("Popup", "closed " + target.GetType().Name);
			bool wasTop = _popups.LastOrDefault() == target;
			_popups.Remove(target);
			if (wasTop)
			{
				target.DeactivatePopup();
				BasePopupPresenter top = _popups.LastOrDefault();
				if (top != null)
				{
					top.transform.parent = _overlayCanvas.transform;
					top.transform.localScale = Vector3.one;
					top.transform.localPosition = Vector3.zero;
					RectTransform rect = top.GetComponent<RectTransform>();
					rect.offsetMin = Vector2.zero;
					rect.offsetMax = Vector2.zero;
					ReactivateWhenStillTopAsync(top).Forget();
				}
			}
			if (!_skipCloseSfx)
			{
				string sfx = (string.IsNullOrEmpty(target.CloseSfx) ? _defaultCloseSfx : target.CloseSfx);
				_soundManager.PlaySfx(sfx);
			}
			OnPopupClosed.OnNext(target);
			if (!_popups.Any())
			{
				OnAllPopupsClosed.OnNext(Unit.Default);
			}
			Type type = target.GetType();
			if (!_cacheTypeSet.Contains(type) || _cache.ContainsKey(type))
			{
				UnityEngine.Object.Destroy(target.gameObject);
				return;
			}
			target.transform.SetParent(_cacheRootTf, worldPositionStays: false);
			target.gameObject.SetActive(value: false);
			_cache[type] = target;
		}

		private async UniTask ReactivateWhenStillTopAsync(BasePopupPresenter popup)
		{
			await UniTask.DelayFrame(1, PlayerLoopTiming.Update, popup.destroyCancellationToken);
			if (!(_popups.LastOrDefault() != popup))
			{
				popup.ActivatePopup();
			}
		}

		public void CloseAll<T>() where T : BasePopupPresenter
		{
			_popups.OfType<T>().ToList().ForEach(delegate(T p)
			{
				p.Close();
			});
		}

		public void CloseAll(params Type[] excludePresenterTypes)
		{
			if (excludePresenterTypes == null || excludePresenterTypes.Length == 0)
			{
				_popups.ToList().ForEach(delegate(BasePopupPresenter p)
				{
					p.Close();
				});
				return;
			}
			HashSet<Type> excludeSet = new HashSet<Type>(excludePresenterTypes);
			_popups.Where((BasePopupPresenter p) => !excludeSet.Contains(p.GetType())).ToList().ForEach(delegate(BasePopupPresenter p)
			{
				p.Close();
			});
		}

		public void CloseAllWithoutSfx(params Type[] excludePresenterTypes)
		{
			_skipCloseSfx = true;
			CloseAll(excludePresenterTypes);
			_skipCloseSfx = false;
		}

		public void CloseTopPopup()
		{
			_popups.LastOrDefault()?.Close();
		}

		public void ClearCache<T>() where T : BasePopupPresenter
		{
			if (_cache.Remove(typeof(T), out var cached))
			{
				_cacheTypeSet.Remove(cached.GetType());
				UnityEngine.Object.Destroy(cached.gameObject);
			}
		}

		public void ClearAllCache()
		{
			foreach (BasePopupPresenter value in _cache.Values)
			{
				_cacheTypeSet.Remove(value.GetType());
				UnityEngine.Object.Destroy(value.gameObject);
			}
			_cache.Clear();
		}
	}
}
