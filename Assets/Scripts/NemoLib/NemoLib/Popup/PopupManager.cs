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
			await _openSemaphore.WaitAsync();
			try
			{
				T presenter = await EnsurePopupAsync<T>();
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
				await presenter.OpenAsync();
				presenter.ActivatePopup();
				string sfx = (string.IsNullOrEmpty(presenter.OpenSfx) ? _defaultOpenSfx : presenter.OpenSfx);
				_soundManager.PlaySfx(sfx);
				OnPopupOpened.OnNext(presenter);
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
