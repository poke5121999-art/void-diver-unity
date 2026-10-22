using DarkTonic.MasterAudio;
using GameCommon.Enum;
using NemoLib.AssetManagement;
using NemoLib.Log;
using NemoLib.Mvp;
using NemoLib.Popup;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

public class InventoryGoodsSlotPresenter : BaseSlotPresenter<InventoryGoodsSlotModel>
{
	public enum ELayout
	{
		Normal = 0,
		Locked = 1,
		Unrevealed = 2,
		Revealing = 3
	}

	[SerializeField]
	private Color _outlineBaseColor;

	[SerializeField]
	private Color _outlineSelectedColor;

	[SerializeField]
	private float _dragThreshold;

	[InjectModel]
	private InventoryGoodsSlotModel _model;

	[View]
	private InventoryGoodsSlotView _view;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private PlayerManager _playerManager;

	[Inject]
	private GameCampaignManager _campaignManager;

	[Inject]
	private GameMissionManager _missionManager;

	[Inject]
	private SoundManager _soundManager;

	[Inject]
	private PopupManager _popupManager;

	[Inject]
	private ToastManager _toastManager;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameInputManager _inputManager;

	private bool _isFocused;

	private Vector2 _pointerDownPos;

	private bool _isDragging;

	private PlaySoundResult _revealLoopSfxResult;

	private bool _isDisabled;

	private ELayout _prevLayout;

	protected override InventoryGoodsSlotModel SlotModel => null;

	protected override Button Button => null;

	private GamePlayer Player => null;

	private InputActions.UiActions UiActions => default;

	private Animator RevealingAnimator => null;

	protected override void Start()
	{
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	protected void OnInject(InventoryGoodsSlotModel slotModel)
	{
	}

	private void UpdateLayout(ELayout layout)
	{
	}

	private void SetNormalLayout()
	{
	}

	private void OnGoodsSlotChanged(GoodsSlot goodsSlot)
	{
	}

	private void OnGoodsMarkChanged((EGoodsType goodsType, int id, bool isMarked) data)
	{
	}

	private void OnMarkGoodsClick()
	{
	}

	private void OnSlotDown(PointerEventData eventData)
	{
	}

	private void OnSlotDrag(PointerEventData eventData)
	{
	}

	private void OnSlotUp(PointerEventData eventData)
	{
	}

	protected override void OnSlotEnter()
	{
	}

	protected override void OnSlotExit()
	{
	}

	protected override void OnLeftClick()
	{
	}

	protected override void OnRightClick()
	{
	}

	private bool TryShowGoodsTooltipPopup()
	{
		return false;
	}

	private bool IsPointerEntered()
	{
		return false;
	}
}
