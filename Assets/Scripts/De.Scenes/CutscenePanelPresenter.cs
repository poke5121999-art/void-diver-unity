using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using VContainer;

public class CutscenePanelPresenter : Presenter
{
	[View]
	private CutscenePanelView _view;

	[Inject]
	private GameCutsceneManager _cutsceneManager;

	private void Start()
	{
		_cutsceneManager.RxIsPlaying.Subscribe(delegate(bool isPlaying)
		{
			_view.Content.SetActive(isPlaying);
		}).AddTo(this);
		_cutsceneManager.RxSkipProgress.Subscribe(delegate(float p)
		{
			_view.GaugeFill.fillAmount = Mathf.Clamp01(p);
		}).AddTo(this);
	}
}
