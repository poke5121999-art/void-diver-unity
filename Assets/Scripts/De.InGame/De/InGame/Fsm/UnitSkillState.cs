using System.Collections.Generic;
using System.Threading;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using UniRx;
using UnityEngine;

namespace De.InGame.Fsm
{
	public class UnitSkillState : UnitStateBase
	{
		private bool _isCompleted;

		private int _skillId;

		private SkillActionNode _currentActionNode;

		private SkillAction _currentAction;

		private IUnitEvent _currentTriggeredEvent;

		private SkillAnimationData _currentAnimationData;

		private List<ITarget> _triggerTargets;

		private Vector3 _aimPos;

		private Vector3 _deltaAimPos;

		private Vector3 _normalizedDir;

		private bool _isFirstAimUpdate;

		private CompositeDisposable _actionDisposable;

		private CancellationTokenSource _actionCancellationTokenSource;

		private float _animationTimeScale;

		private float _logicTimeScale;

		private bool _useCachedInput;

		private int _pendingCancelSkillId;

		private Queue<ESkillInputEvent> _cachedInput;

		private Queue<SkillAnimationData> _animationQueue;

		private bool _nextNodeNeedInput;

		private bool _isMoveStart;

		private ESkillMoveType _moveType;

		private Vector3 _moveDir;

		private float _remainMoveTime;

		private float _moveSpeed;

		private AnimationCurve _moveSpeedCurve;

		private bool _ignoreMoveCollision;

		private bool _isGroundSfxActive;

		private float _groundSfxTimer;

		private float _skillActionElapsedTime;

		private readonly List<(UnitModel owner, int id, int stack)> _actionLinkedBuffList;

		private readonly List<long> _actionLinkedVfxList;

		private readonly List<long> _skillLinkedVfxList;

		private readonly List<ActionEvent> _reservedActionEvents;

		private bool _isRemovingActionLinkedBuffs;

		public override EActionState ActionState => EActionState.None;

		public override bool IsAimEnable => false;

		private TSkill Skill => null;

		private ITarget TriggerTarget => null;

		public UnitSkillState(UnitStateMachine stateMachine)
			: base(null)
		{
		}

		public override void OnEnter(IUnitEvent unitEvent = null)
		{
		}

		private void ExecuteSkillAction(SkillActionNode node, IUnitEvent triggeredEvent, List<ITarget> triggerTargets)
		{
		}

		public override void OnUpdate()
		{
		}

		public override void OnExit()
		{
		}

		public override EActionState GetNextStateEvent(IUnitEvent unitEvent)
		{
			return EActionState.None;
		}

		public override Vector3 GetForward()
		{
			return default;
		}

		private bool IsWaitingInputSkillRemoved()
		{
			return false;
		}

		private bool IsNextNodeNeedInput(SkillActionNode node)
		{
			return false;
		}

		protected bool ProceedNextSkillAction(IUnitEvent unitEvent)
		{
			return false;
		}

		private bool CheckBuffCondition(List<BuffCondition> buffConditions)
		{
			return false;
		}

		protected override bool TriggerActiveSkill(IUnitEvent unitEvent)
		{
			return false;
		}

		private CancelableTime GetCancelableTime(int inputSkillId)
		{
			return null;
		}

		private void SendSkillIndicator()
		{
		}

		private void SendSkillIndicatorOff()
		{
		}

		public void PlayAnimation()
		{
		}

		private void InitMoveAction()
		{
		}

		private void UpdateMoveByInputAnimation(Vector3 prev, Vector3 current)
		{
		}

		private void OnUpdateMoveAndAim()
		{
		}

		private float GetCalculatedMoveSpeed()
		{
			return 0f;
		}

		private void CheckMovable()
		{
		}

		private void StopMoveAction(bool isBlocked, ITarget hitTarget = null)
		{
		}

		private void OnStartMove()
		{
		}

		private void InitGroundSfxOnMove()
		{
		}

		private void UpdateGroundSfxOnMove()
		{
		}

		private void UpdateAimPosition()
		{
		}

		private Vector3 CalculateMoveToAimPos()
		{
			return default;
		}

		private Vector3 CalculateMoveToTarget()
		{
			return default;
		}

		private void InitSkillAction()
		{
		}

		private void OnSkillActionTimeEnded()
		{
		}

		private void ExecuteActionEvent(ActionEvent actionEvent)
		{
		}

		private void ExecuteActionEventOnTarget(ActionEvent actionEvent, ITarget actionTarget)
		{
		}

		private void RemoveActionLinkedBuffs()
		{
		}

		private void RemoveActionLinkedVfx()
		{
		}

		private void RemoveSkillLinkedVfx()
		{
		}
	}
}
