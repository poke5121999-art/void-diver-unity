using DG.Tweening;
using NemoLib.Mvp;
using UnityEngine;
using VContainer;

public class PlayerNoticeToastPresenter : BaseToastPresenter
{
	[InjectModel]
	private PlayerNoticeToastModel _model;

	[View]
	private PlayerNoticeToastView _view;

	[Inject]
	private ToastManager _toastManager;

	[SerializeField]
	private float _duration;

	[SerializeField]
	private float _moveDuration;

	[SerializeField]
	private float _fadeDelay;

	[SerializeField]
	private float _endY;

	private Tween _tween;

	public override string OpenSfx => null;

	protected override void OnShow()
	{
	}

	private void OnDestroy()
	{
	}

	public override void OnCloseDuplicateToast(string message)
	{
	}
}
