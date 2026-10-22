using System;
using De.InGame.Fsm;
using GameCommon.Enum;
using UnityEngine;
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

	protected override UnitStateMachine StateMachine => _stateMachine;

	public override ulong ClientId => OwnerClientId;

	private ExtraUnitView ExtraUnitView => UnitView as ExtraUnitView;

	private ExtraUnitModel ExtraUnitModel => UnitModel as ExtraUnitModel;

	private int ExtraUnitId => ExtraUnitModel.SyncExtraUnitId.Value;

	private TExtraUnit ExtraUnitData => TExtraUnit.Table[ExtraUnitModel.ExtraUnitId];

	public void Init(Args args)
	{
		AssertIfNotHost();
		_args = args;
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		if (base.IsHost)
		{
			ExtraUnitModel.SyncExtraUnitId.Value = _args.ExtraUnitId;
			switch (ExtraUnitData.ExtraUnitType)
			{
			case EExtraUnitType.StaticUnit:
				_stateMachine = new StaticUnitStateMachine(this);
				break;
			case EExtraUnitType.StationaryUnit:
				_stateMachine = new StationaryUnitStateMachine(this);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
		base.name = string.Format("{0}", ExtraUnitModel.ExtraUnitId);
		ExtraUnitModel.Init();
		ExtraUnitView.Init();
		if (base.IsHost)
		{
			if (GameContext.IsInStage)
			{
				TAnomaly.Table[GameContext.AnomalyType].PlayerBuffIds?.ForEach(delegate(int id)
				{
					ExtraUnitModel.AddBuff(id, ExtraUnitModel, 1, 0uL, Vector3.zero);
				});
			}
			UnitModel.PassiveSkillIds?.ForEach(delegate(int _)
			{
				HandleUnitEvent(new AddPassiveEvent
				{
					SkillId = _
				});
			});
		}
	}



}
