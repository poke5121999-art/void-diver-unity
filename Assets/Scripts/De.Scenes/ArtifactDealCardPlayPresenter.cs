using GameCommon.Table;
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

	private CompositeDisposable _innerDisposable = new CompositeDisposable();

	private CompositeDisposable _disposable = new CompositeDisposable();

	private Animator Animator
	{
		get
		{
			if (_animator == null)
			{
				_animator = GetComponent<Animator>();
			}
			return _animator;
		}
	}

	public Button NavButton => _view.ArtifactDealCard;

	private void OnInject(ArtifactDealCardPlayModel model)
	{
		_innerCard = _view.ArtifactDealCard.GetComponent<ArtifactDealCardPresenter>();
		_innerDisposable.Dispose();
		_innerDisposable = new CompositeDisposable();
		_disposable.Dispose();
		_disposable = new CompositeDisposable();
		ArtifactDealCardModel innerModel = new ArtifactDealCardModel();
		innerModel.Id = _model.Id;
		innerModel.OnClick.Subscribe(delegate
		{
			_model.OnClick.OnNext(_model);
		}).AddTo(_innerDisposable);
		innerModel.RxIsPressed.Subscribe(delegate(bool p)
		{
			_model.RxIsPressed.Value = p;
		}).AddTo(_innerDisposable);
		_innerCard.Inject(innerModel);
		_view.PressedFX.SetActive(value: false);
		_view.Complete.gameObject.SetActive(value: false);
		Animator.Play("Idle");
		_model.RxIsPressed.Subscribe(ApplyPressed).AddTo(_disposable);
		int rarity = (int)TArtifactDealCard.Table[_model.Id].RarityType;
		_view.PressedFX_Tier1.SetActive(rarity == 1);
		_view.PressedFX_Tier2.SetActive(rarity == 2);
		_view.PressedFX_Tier3.SetActive(rarity == 3);
		_view.PressedFX_Tier4.SetActive(rarity == 4);
		_view.PressedFX_Tier5.SetActive(rarity == 5);
	}

	private void OnDestroy()
	{
		_innerDisposable?.Dispose();
		_disposable?.Dispose();
	}

	public void PlayCardUp()
	{
		Animator.Play(Animator.StringToHash("Up"), -1, float.NegativeInfinity);
	}

	public void PlayComplete()
	{
		Animator.Play("Idle");
		_view.Complete.gameObject.SetActive(value: true);
	}

	public void SetInteractable(bool interactable)
	{
		_innerCard.SetInteractable(interactable);
	}

	private void ApplyPressed(bool isPressed)
	{
		_view.PressedFX.SetActive(isPressed);
	}

	private void ApplyComplete(bool isComplete)
	{
		_view.Complete.gameObject.SetActive(isComplete);
	}
}
