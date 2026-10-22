using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.Log;
using UnityEngine;
using VContainer;

namespace De.InGame.Unit
{
	public class MonsterView : UnitView
	{
		[Inject]
		private GameObjectManager _gameObjectManager;

		[Inject]
		private SeriLogger _logger;

		[SerializeField]
		private ShadowMonsterVisualEffect _shadowMonsterVisualEffect;

		private OutlineObject _outlineObject;

		private StatusVfx _groggyVfx;

		protected MonsterModel MonsterModel => null;

		private int MonsterId => 0;

		private TMonster MonsterData => null;

		private OutlineObject OutlineObject => null;

		public Vector3 SpawnPosition { get; private set; }

		protected override string ResolveFootstepSound(bool isLeft)
		{
			return null;
		}

		protected override string GetFootstepSfx(EGroundType groundType, bool isLeft)
		{
			return null;
		}

		private bool ShouldPlayFootstep()
		{
			return false;
		}

		public override void Init()
		{
		}

		protected override void Start()
		{
		}

		private void OnHpDamage(NtfHpDamage message)
		{
		}

		protected override void OnStateChanged(EActionState state)
		{
		}

		private void SpawnGroggyVfx()
		{
		}

		private void DespawnGroggyVfx()
		{
		}

		protected override void PlayAnimationByActionState(EActionState state)
		{
		}

		public virtual string GetAggroAnimation()
		{
			return null;
		}

		public virtual string GetReceiveAggroSpreadAnimation()
		{
			return null;
		}

		public virtual string GetLostTargetAnimation()
		{
			return null;
		}

		protected override void OnUsingSkillIdChanged(int skillId)
		{
		}

		public override string GetIdleAnimation()
		{
			return null;
		}

		public override string GetMoveAnimation()
		{
			return null;
		}

		public override string GetDeathAnimation()
		{
			return null;
		}

		public override string GetReviveAnimation()
		{
			return null;
		}

		public override float GetMoveAnimationSpeed()
		{
			return 0f;
		}

		public void SetShadowMonsterEffect(bool isLocalShadowMonster)
		{
		}
	}
}
