using De.InGame.Fsm;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using UnityEngine;

public class MonsterCapturedState : MonsterStateBase
{
	private float _despawnTime;

	private bool _isDespawned;

	public override EActionState ActionState => EActionState.Captured;

	public override bool IsAimEnable => false;

	public MonsterCapturedState(UnitStateMachine stateMachine)
		: base(stateMachine)
	{
	}

	public override void OnEnter(IUnitEvent unitEvent)
	{
		base.OnEnter(unitEvent);
		_isDespawned = false;
		_despawnTime = TimeNow + TConst.DespawnDelayOnCaptured.FloatValue;
		UnitView.MoveTo(((CaptureEvent)unitEvent).HitPosition);
	}

	public override void OnUpdate()
	{
		if (!_isDespawned && !(TimeNow <= _despawnTime))
		{
			_isDespawned = true;
			DropCapturedGoods();
			UnitModel.NetworkObject.Despawn();
		}
	}

	private void DropCapturedGoods()
	{
		GoodsData capturedDropGoodsData = MonsterData.CapturedDropGoodsData;
		if (!capturedDropGoodsData.IsEmpty)
		{
			int count = Mathf.Max(1, GameManager.CharacterModels.Count);
			ObjectManager.SpawnDropGoods(capturedDropGoodsData.ToGoodsAsBase(), UnitModel.transform.position, count);
		}
	}

	public override EActionState GetNextStateEvent(IUnitEvent unitEvent)
	{
		return EActionState.None;
	}

	public override Vector3 GetForward()
	{
		return Vector3.zero;
	}
}
