using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
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
			// TODO(decomp): generic body not decompiled (no native address); prefab address = typeof(T).Name is a guess @0x0
			GameObject go = await Addressables.InstantiateAsync(typeof(T).Name, rootTf).ToUniTask();
			return go.GetComponent<T>();
		}

		private async UniTask<T> EnsurePopupAsync<T>() where T : BasePopupPresenter
		{
			// TODO(decomp): generic body not decompiled; reconstructed from cache fields and OnClose @0x0
			Type type = typeof(T);
			if (_cache.TryGetValue(type, out var cached) && cached != null)
			{
				_cache.Remove(type);
				cached.ResetForReuse();
				cached.transform.SetParent(OverlayRootTf, worldPositionStays: false);
				cached.gameObject.SetActive(value: true);
				return (T)cached;
			}
			return await InstantiatePopupAsync<T>(OverlayRootTf);
		}

		public async UniTask PreloadAsync<T>() where T : BasePopupPresenter
		{
			// TODO(decomp): generic body not decompiled; reconstructed from state-machine fields (Type local + UniTask<T>) @0x0
			Type type = typeof(T);
			_cacheTypeSet.Add(type);
			if (!_cache.ContainsKey(type))
			{
				T popup = await InstantiatePopupAsync<T>(_cacheRootTf);
				popup.gameObject.SetActive(value: false);
				_cache[type] = popup;
			}
		}

		public T FindPopup<T>() where T : BasePopupPresenter
		{
			// TODO(decomp): generic body not decompiled @0x0
			return _popups.OfType<T>().LastOrDefault();
		}

		public IEnumerable<T> FindAllPopup<T>() where T : BasePopupPresenter
		{
			// TODO(decomp): generic body not decompiled @0x0
			return _popups.OfType<T>();
		}

		public bool ExistPopup<T>() where T : BasePopupPresenter
		{
			// TODO(decomp): generic body not decompiled @0x0
			return _popups.OfType<T>().Any();
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
			// TODO(decomp): generic body not decompiled @0x0
			OpenAsync<T>(model).Forget();
		}

		public async UniTask<T> OpenAsync<T>(Model model = null) where T : BasePopupPresenter
		{
			// TODO(decomp): generic body not decompiled; order reconstructed from state-machine fields
			// (BlockScope, SemaphoreSlim.WaitAsync, UniTask<T> ensure, UniTask open) and OnClose @0x0
			using BlockScope blockScope = new BlockScope();
			await _openSemaphore.WaitAsync();
			try
			{
				T presenter = await EnsurePopupAsync<T>();
				if (model != null)
				{
					presenter.Inject(model);
				}
				OnBeforeOpenPopup.OnNext(presenter);
				BasePopupPresenter previous = _popups.LastOrDefault();
				if (previous != null)
				{
					previous.DeactivatePopup();
					previous.transform.SetParent(RootTf, worldPositionStays: false);
				}
				_popups.Add(presenter);
				presenter.OnClose.Subscribe(OnClose).AddTo(presenter);
				string sfx = string.IsNullOrEmpty(presenter.OpenSfx) ? _defaultOpenSfx : presenter.OpenSfx;
				_soundManager.PlaySfx(sfx);
				await presenter.OpenAsync();
				presenter.ActivatePopup();
				OnPopupOpened.OnNext(presenter);
				RefreshBlurEffect();
				return presenter;
			}
			finally
			{
				_openSemaphore.Release();
			}
		}

		private void OnClose(BasePopupPresenter target)
		{
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
			// TODO(decomp): generic body not decompiled (closure <>c__42<T>.b__42_0 is Close()) @0x0
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
			// TODO(decomp): generic body not decompiled @0x0
			Type type = typeof(T);
			if (_cache.TryGetValue(type, out var cached))
			{
				_cacheTypeSet.Remove(type);
				UnityEngine.Object.Destroy(cached.gameObject);
				_cache.Remove(type);
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
