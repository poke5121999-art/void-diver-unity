using DG.Tweening;
using De.InGame.Unit;
using Febucci.UI;
using NemoLib.AssetManagement;
using NemoLib.Log;
using NemoLib.Mvp;
using TMPro;
using UniRx;
using UnityEngine;
using VContainer;

public class PetHudPresenter : BaseUnitHudPresenter
{
	[SerializeField]
	private PetModel _petModel;

	[View]
	private PetHudView _view;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameChannel _gameChannel;

	private CompositeDisposable _disposable = new CompositeDisposable();

	private Tween _aggroTween;

	private float _elapsedActiveTime;

	private GameObject _currentEmoji;

	protected override GameObject Bubble => _view.Bubble;

	protected override TypewriterByCharacter BubbleTextWriter => _view.Bubble_Text.GetComponent<TypewriterByCharacter>();

	protected override TextMeshProUGUI Monologue => _view.Monologue;

	private MonsterView MonsterView => _petModel.GetComponent<MonsterView>();

	private void Start()
	{
		PetController component = _petModel.GetComponent<PetController>();
		component.OnShowBubbleText.Subscribe(ShowBubbleText).AddTo(_disposable);
		component.OnShowMonologueText.Subscribe(ShowMonologueText).AddTo(_disposable);
		component.OnShowEmoji.Subscribe(OnShowEmoji).AddTo(_disposable);
	}

	private void OnShowEmoji(int emojiId)
	{
		if (_currentEmoji != null)
		{
			_currentEmoji.DespawnOrDestroy();
		}
		_currentEmoji = _assetManager.SpawnEmoji(emojiId, _view.EmojiRoot.transform);
		_currentEmoji.transform.localPosition = Vector3.zero;
		_currentEmoji.DespawnOrDestroy(1.5f);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_disposable.Dispose();
	}
}
