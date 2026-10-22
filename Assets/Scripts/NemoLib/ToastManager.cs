using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class ToastManager : MonoBase
{
	[SerializeField]
	private Canvas _canvas;

	[SerializeField]
	private string _defaultOpenSfx;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private SoundManager _soundManager;

	private const float _queueDelay = 0.4f;

	private readonly Queue<Model> _queue = new Queue<Model>();

	private bool _isProcessing;

	private readonly Subject<string> _onCloseDuplicateToast = new Subject<string>();

	private Transform RootTf => _canvas.transform;

	protected override void Awake()
	{
		this.InjectFromRootContainer();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private async UniTask<GameObject> InstantiateToastAsync(string baseName)
	{
		GameObject prefab = await _assetManager.GetRecentlyCacheAsync<GameObject>("Toast", "Toast/" + baseName + ".prefab");
		return UnityEngine.Object.Instantiate(prefab);
	}

	public void Show(Model model)
	{
		if (model != null)
		{
			_queue.Enqueue(model);
			if (!_isProcessing)
			{
				_isProcessing = true;
				ProcessQueueAsync().Forget();
			}
		}
	}

	private async UniTaskVoid ProcessQueueAsync()
	{
		while (_queue.Count > 0)
		{
			Model model = _queue.Dequeue();
			string name = model.GetType().Name;
			if (name.EndsWith("Model"))
			{
				name = name.Substring(0, name.Length - "Model".Length);
			}
			GameObject go = await InstantiateToastAsync(name);
			BaseToastPresenter presenter = go.GetComponents<BaseToastPresenter>().FirstOrDefault();
			if (presenter == null)
			{
				if (go != null)
				{
					UnityEngine.Object.Destroy(go);
				}
				continue;
			}
			presenter.Inject(model);
			presenter.Show();
			presenter.OnClose.Subscribe(OnClose).AddTo(this);
			_onCloseDuplicateToast.Subscribe(presenter.OnCloseDuplicateToast).AddTo(presenter);
			RectTransform rectTransform = go.GetComponent<RectTransform>();
			go.transform.parent = _canvas.transform;
			go.transform.localScale = Vector3.one;
			go.transform.localPosition = Vector3.zero;
			if (rectTransform != null)
			{
				rectTransform.offsetMin = Vector2.zero;
				rectTransform.offsetMax = Vector2.zero;
			}
			string sfx = (!string.IsNullOrEmpty(presenter.OpenSfx)) ? presenter.OpenSfx : _defaultOpenSfx;
			if (!string.IsNullOrEmpty(sfx))
			{
				_soundManager.PlaySfx(sfx);
			}
			await UniTask.Delay(TimeSpan.FromSeconds(0.4f), false, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
		}
		_isProcessing = false;
	}

	private void OnClose(BaseToastPresenter target)
	{
		if (target != null)
		{
			UnityEngine.Object.Destroy(target.gameObject);
		}
	}

	public void CloseDuplicateToasts(string message)
	{
		_onCloseDuplicateToast.OnNext(message);
	}
}
