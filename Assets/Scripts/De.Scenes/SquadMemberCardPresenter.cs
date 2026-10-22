using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class SquadMemberCardPresenter : BasePanelPresenter
{
	public enum EFocusPanel
	{
		None = 0,
		Base = 1,
		Empty = 2,
		PositiveParadox = 3,
		NegativeParadox = 4
	}

	[InjectModel]
	private SquadMemberCardModel _model;

	[View]
	private SquadMemberCardView _view;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private EnvironmentSettings _environmentSettings;

	[Inject]
	private IRoomMatchmaker _roomMatchmaker;

	private CompositeDisposable _disposable;

	private ulong _steamId;

	private EmptyPanelModel _emptyPanelModel;

	private ParadoxPanelModel _positiveParadoxPanelModel;

	private ParadoxPanelModel _negativeParadoxPanelModel;

	protected override BasePanelModel PanelModel => null;

	private EFocusPanel FocusedPanel => EFocusPanel.None;

	private bool IsSteamConnected => false;

	protected override void OnDestroy()
	{
	}

	private void OnInject(SquadMemberCardModel model)
	{
	}

	protected override void OnInputEnabled()
	{
	}

	protected override void OnInputDisabled()
	{
	}

	private void InitProfile()
	{
	}

	private void InitParadoxPanel()
	{
	}

	private void InitSkillPanel()
	{
	}

	private void InitGamePadNav()
	{
	}

	private void FocusPanel(EFocusPanel panel, ENavDir entryDir)
	{
	}

	private void OnSteamButtonClick()
	{
	}

	private void OnParadoxSlotEnter(ParadoxSimpleSlotModel slotModel)
	{
	}

	private void OnParadoxSlotExit(ParadoxSimpleSlotModel slotModel)
	{
	}

	private void OnSkillSlotEnter(SkillSlotModel slotModel)
	{
	}

	private void OnSkillSlotExit(SkillSlotModel slotModel)
	{
	}
}
