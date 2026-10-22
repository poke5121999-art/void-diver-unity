using NemoLib.Mvp;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactDealCardPlayPresenter : Presenter
{
	[View]
	private ArtifactDealCardPlayView _view;

	[InjectModel]
	private ArtifactDealCardPlayModel _model;

	private Animator _animator;

	private const string CardUpAnimation = "Up";

	private const string IdleAnimation = "Idle";

	private ArtifactDealCardPresenter _innerCard;

	private CompositeDisposable _innerDisposable;

	private CompositeDisposable _disposable;

	private Animator Animator => null;

	public Button NavButton => null;

	private void OnInject(ArtifactDealCardPlayModel model)
	{
	}

	private void OnDestroy()
	{
	}

	public void PlayCardUp()
	{
	}

	public void PlayComplete()
	{
	}

	public void SetInteractable(bool interactable)
	{
	}

	private void ApplyPressed(bool isPressed)
	{
	}

	private void ApplyComplete(bool isComplete)
	{
	}
}
