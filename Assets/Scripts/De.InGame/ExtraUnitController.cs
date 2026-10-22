using De.InGame.Fsm;
using GameCommon.Table;
using NemoLib.Log;
using Unity.Netcode;
using VContainer;

public class ExtraUnitController : UnitController
{
	public class Args
	{
		public int ExtraUnitId;
	}

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private NetworkManager _networkManager;

	private Args _args;

	private UnitStateMachine _stateMachine;

	protected override UnitStateMachine StateMachine => null;

	public override ulong ClientId => 0uL;

	private ExtraUnitView ExtraUnitView => null;

	private ExtraUnitModel ExtraUnitModel => null;

	private int ExtraUnitId => 0;

	private TExtraUnit ExtraUnitData => null;

	public void Init(Args args)
	{
	}

	public override void OnNetworkSpawn()
	{
	}



}
