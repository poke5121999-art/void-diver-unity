using System;
using System.Linq;
using GameCommon.Enum;
using GameCommon.Table;
using SomeNet.Utility;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace De.InGame.Fsm
{
	public class MonsterDeadState : MonsterStateBase
	{
		private Inventory[] _lootingInventories;

		private MonsterBody _monsterBody;

		private int _nextPhaseMonsterId;

		private bool _isCompleted;

		public override EActionState ActionState => EActionState.Dead;

		public override bool IsAimEnable => false;

		public MonsterDeadState(UnitStateMachine stateMachine)
			: base(stateMachine)
		{
		}

		public override void OnEnter(IUnitEvent unitEvent)
		{
			base.OnEnter(unitEvent);
			_monsterBody = MonsterModel.BodyObject.GetComponent<MonsterBody>();
			_lootingInventories = _monsterBody.LootingInventories;
			if (TMonster.Table.TryGetValue(MonsterModel.MonsterId, out var monsterData))
			{
				if (monsterData.RespawnTime > 0)
				{
					for (int i = 0; i < 10; i++)
					{
						float min = Math.Max(20f - (float)i * 3f, 0f);
						Vector3 randomDirection = InGameUtility.GetRandomDirection();
						float randomFloat = RandomUtility.GetRandomFloat(min, 30f - (float)i * 3f);
						Vector3 sourcePosition = StateMachine.UnitModel.transform.position + randomDirection * randomFloat;
						if (NavMesh.SamplePosition(sourcePosition, out var hit, 5f, 1))
						{
							MonsterSpawnData monsterSpawnData = new MonsterSpawnData();
							if (MonsterModel.MonsterSpawnId < 1)
							{
								monsterSpawnData.MonsterId = MonsterModel.MonsterId;
							}
							else
							{
								monsterSpawnData.UseMonsterSpawnTable = true;
								monsterSpawnData.MonsterSpawnGroupId = TMonsterSpawn.Table[MonsterModel.MonsterSpawnId].GroupId;
							}
							monsterSpawnData.SpawnPosition = hit.position;
							monsterSpawnData.IsRespawn = true;
							monsterSpawnData.SpawnableTime = (float)monsterData.RespawnTime + GameManager.TimeNow;
							ObjectManager.AddMonsterSpawnData(monsterSpawnData);
							break;
						}
					}
				}
				_nextPhaseMonsterId = MonsterModel.NextPhaseMonsterId;
				Observable.Timer(TimeSpan.FromSeconds(TMonster.Table[MonsterModel.MonsterId].DeadDuration)).Subscribe(delegate
				{
					OnDeadDurationElapsed();
				}).AddTo(MonsterModel);
			}
		}

		private void OnDeadDurationElapsed()
		{
			_isCompleted = true;
			if (_nextPhaseMonsterId > 0)
			{
				MonsterController.HandleUnitEvent(new MonsterPhaseUpEvent
				{
					NextPhaseMonsterId = _nextPhaseMonsterId
				});
			}
		}

		public override void OnExit()
		{
			if (IsActivated)
			{
				base.OnExit();
				_isCompleted = false;
				_nextPhaseMonsterId = 0;
			}
		}

		public override void OnUpdate()
		{
		}

		public override EActionState GetNextStateEvent(IUnitEvent unitEvent)
		{
			if (unitEvent is MonsterPhaseUpEvent)
			{
				return EActionState.PhaseUp;
			}
			if (_isCompleted && _nextPhaseMonsterId < 1)
			{
				// A boss stays where it fell: its body holds the loot (1103: Donovan's drop group 127000 carries the
				// painting 300011). The branch returns None (@0x1806B53A0, IsBoss -> LAB_1806b55a9).
				if (MonsterData.MonsterType.IsBoss())
				{
					return EActionState.None;
				}
				if (_lootingInventories != null && !MonsterModel.IsLifeTimeFinished)
				{
					if (!_lootingInventories.All((Inventory inventory) => inventory.Slots.Count < 1 || inventory.Slots.All((System.Collections.Generic.KeyValuePair<int, GoodsSlot> slot) => slot.Value.IsEmpty || slot.Value.Count <= 0))
						|| _monsterBody.IsInteracting)
					{
						return EActionState.None;
					}
				}
				return EActionState.ReservedDespawn;
			}
			return EActionState.None;
		}

		public override Vector3 GetForward()
		{
			return Vector3.zero;
		}
	}
}
