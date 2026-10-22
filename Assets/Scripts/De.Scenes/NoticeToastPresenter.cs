using DG.Tweening;
using NemoLib.Mvp;
using UnityEngine;
using VContainer;

public class NoticeToastPresenter : BaseToastPresenter
{
	[InjectModel]
	private NoticeToastModel _model;

	[View]
	private NoticeToastView _view;

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
