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

	private CompositeDisposable _disposable;

	private Tween _aggroTween;

	private float _elapsedActiveTime;

	private GameObject _currentEmoji;

	protected override GameObject Bubble => null;

	protected override TypewriterByCharacter BubbleTextWriter => null;

	protected override TextMeshProUGUI Monologue => null;

	private MonsterView MonsterView => null;

	private void Start()
	{
	}

	private void OnShowEmoji(int emojiId)
	{
	}

	protected override void OnDestroy()
	{
	}
}
