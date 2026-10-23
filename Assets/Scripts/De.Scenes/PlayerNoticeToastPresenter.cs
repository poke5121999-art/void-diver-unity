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
	private float _duration = 1f;

	[SerializeField]
	private float _moveDuration = 1f;

	[SerializeField]
	private float _fadeDelay = 0.2f;

	[SerializeField]
	private float _endY = 120f;

	private Tween _tween;

	public override string OpenSfx => _model.OpenSfx;

	protected override void OnShow()
	{
		_tween.Kill();
		_toastManager.CloseDuplicateToasts(_model.Message);
		_view.MessageText.text = _model.Message;
		_tween = DOTween.Sequence().Append(base.transform.DOLocalMoveY(_endY, _moveDuration)).AppendInterval(_fadeDelay).Join(_view.MessageText.DOFade(0f, _duration - _fadeDelay))
			.OnComplete(delegate
			{
				Close();
			})
			.OnKill(delegate
			{
				Close();
			});
	}

	private void OnDestroy()
	{
		_tween?.Kill();
		_tween = null;
	}

	public override void OnCloseDuplicateToast(string message)
	{
		if (message.Equals(_model.Message))
		{
			Close();
		}
	}
}
