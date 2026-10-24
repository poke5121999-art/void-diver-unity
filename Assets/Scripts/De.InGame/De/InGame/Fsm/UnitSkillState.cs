using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GameCommon.Data;
using GameCommon.Enum;
using GameCommon.Table;
using SomeNet.Utility;
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

		private List<ITarget> _triggerTargets = new List<ITarget>();

		private Vector3 _aimPos;

		private Vector3 _deltaAimPos;

		private Vector3 _normalizedDir;

		private bool _isFirstAimUpdate;

		private CompositeDisposable _actionDisposable;

		private CancellationTokenSource _actionCancellationTokenSource;

		private float _animationTimeScale = 1f;

		private float _logicTimeScale = 1f;

		private bool _useCachedInput;

		private int _pendingCancelSkillId;

		private Queue<ESkillInputEvent> _cachedInput = new Queue<ESkillInputEvent>();

		private Queue<SkillAnimationData> _animationQueue = new Queue<SkillAnimationData>();

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

		private readonly List<(UnitModel owner, int id, int stack)> _actionLinkedBuffList = new List<(UnitModel owner, int id, int stack)>();

		private readonly List<long> _actionLinkedVfxList = new List<long>();

		private readonly List<long> _skillLinkedVfxList = new List<long>();

		private readonly List<ActionEvent> _reservedActionEvents = new List<ActionEvent>();

		private bool _isRemovingActionLinkedBuffs;

		public override EActionState ActionState => EActionState.Skill;

		public override bool IsAimEnable => _moveType == ESkillMoveType.MoveByInput;

		private TSkill Skill => TSkill.Table[_skillId];

		private ITarget TriggerTarget => _triggerTargets?.FirstOrDefault();

		public UnitSkillState(UnitStateMachine stateMachine)
			: base(stateMachine)
		{
		}

		public override void OnEnter(IUnitEvent unitEvent = null)
		{
			base.OnEnter(unitEvent);
			UseSkillEvent useSkillEvent = unitEvent as UseSkillEvent;
			UnitModel.UpdateAim();
			if (useSkillEvent == null)
			{
				return;
			}
			_skillId = useSkillEvent.SkillId;
			if (Skill == null)
			{
				return;
			}
			float num = (Skill.IsBasicAttack ? UnitModel.AtkSpeed : 1f);
			_animationTimeScale = num;
			_logicTimeScale = num;
			TSkill tSkill = TSkill.Table[_skillId];
			if (tSkill != null && tSkill.SkillTags != null && tSkill.SkillTags.Contains(ESkillTagType.Item))
			{
				IList<BuffEffectInfo<ItemUseSpeedAmplifierEffect>> buffEffects = UnitModel.GetBuffEffects<ItemUseSpeedAmplifierEffect>();
				if (buffEffects.Any())
				{
					float value = buffEffects.Sum((BuffEffectInfo<ItemUseSpeedAmplifierEffect> b) => (float)b.Stack * b.BuffEffect.Percent) / 100f + 1f;
					value = Mathf.Clamp(value, TConst.BuffEffectAmplifierMin.FloatValue, TConst.BuffEffectAmplifierMax.FloatValue);
					_logicTimeScale = value * _logicTimeScale;
				}
			}
			if (!UnitModel.CheckSkillActionCost(Skill.RootActionNode, Skill))
			{
				_isCompleted = true;
				return;
			}
			if (!CheckBuffCondition(Skill.RootActionNode.skillAction.BuffConditionList) || !UnitController.CheckTalentCondition(Skill.RootActionNode.skillAction.TalentConditionList))
			{
				_isCompleted = true;
				return;
			}
			UnitModel.UsingSkillId = _skillId;
			_aimPos = UnitModel.CalculatedDeltaAimPos + UnitModel.transform.position;
			_deltaAimPos = UnitModel.CalculatedDeltaAimPos;
			_normalizedDir = new Vector3(_deltaAimPos.x, 0f, _deltaAimPos.z).normalized;
			SkillActionNode rootActionNode = Skill.RootActionNode;
			List<ITarget> triggerTargets = new List<ITarget> { useSkillEvent.TriggerTarget ?? UnitModel };
			ExecuteSkillAction(rootActionNode, unitEvent, triggerTargets);
			if (useSkillEvent.CachedSkillInput == null)
			{
				return;
			}
			ESkillInputEvent[] array = useSkillEvent.CachedSkillInput.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != ESkillInputEvent.None)
				{
					UnitController.HandleUnitEvent(new SkillInputEvent { SkillInputType = array[i] });
				}
			}
		}

		private void ExecuteSkillAction(SkillActionNode node, IUnitEvent triggeredEvent, List<ITarget> triggerTargets)
		{
			int hpCost = 0;
			int stressCost = 0;
			int staminaCost = 0;
			RemoveActionLinkedBuffs();
			RemoveActionLinkedVfx();
			if (_currentAction != null && _currentAction.indicatorType == ESkillIndicatorType.SelectCastInRange)
			{
				Vector3 position = UnitModel.transform.position;
				Vector3 vector = ((!(_deltaAimPos.magnitude > _currentAction.castRangeRadius)) ? (_deltaAimPos + UnitModel.transform.position) : (UnitModel.transform.position + _currentAction.castRangeRadius * _normalizedDir));
				Vector3 vector2 = vector - position;
				float magnitude = vector2.magnitude;
				if (magnitude > 0f && Physics.Raycast(position, vector2.normalized, out var hitInfo, magnitude, 1 << Const.Layer.HIGH_OBSTACLE))
				{
					vector = hitInfo.point;
				}
				_aimPos = vector;
				_deltaAimPos = vector - UnitModel.transform.position;
			}
			_currentActionNode = node;
			_currentAction = node.skillAction;
			_nextNodeNeedInput = IsNextNodeNeedInput(node);
			_currentTriggeredEvent = triggeredEvent;
			_useCachedInput = false;
			_pendingCancelSkillId = 0;
			_animationQueue.Clear();
			if (_currentAction.SkillAnimationDatas != null)
			{
				SkillAnimationData[] skillAnimationDatas = _currentAction.SkillAnimationDatas;
				foreach (SkillAnimationData item in skillAnimationDatas)
				{
					_animationQueue.Enqueue(item);
				}
			}
			_triggerTargets = triggerTargets;
			if (_currentAction.IsGivingCoolDown)
			{
				UnitModel.StartSkillCoolDown(_skillId);
			}
			if (_currentAction.IsUsingStack && GameManager.IsInStage)
			{
				UnitModel.ConsumeSkillStack(_skillId);
			}
			if (GameManager.IsInStage && _currentAction.IsUsingCost)
			{
				hpCost = Skill.HpCost;
				stressCost = Skill.StressCost;
				staminaCost = Skill.StaminaCost;
				UnitController.AdjustSkillCosts(Skill, ref hpCost, ref stressCost, ref staminaCost);
				UnitController.ApplySkillCost(_skillId, hpCost, stressCost, staminaCost);
			}
			_actionDisposable?.Dispose();
			_actionDisposable = new CompositeDisposable();
			_skillActionElapsedTime = 0f;
			InitMoveAction();
			_isFirstAimUpdate = true;
			if (_currentAction.UpdateAimOnStart || IsAimEnable || _currentAction.UpdateAimOnFrame)
			{
				UpdateAimPosition();
			}
			InitSkillAction();
			PlayAnimation();
			SendSkillIndicator();
		}

		public override void OnUpdate()
		{
			if (_currentAction == null || _isCompleted)
			{
				return;
			}
			if (IsAimEnable || _currentAction.UpdateAimOnFrame)
			{
				UpdateAimPosition();
			}
			OnUpdateMoveAndAim();
			_skillActionElapsedTime += _logicTimeScale * Time.fixedDeltaTime;
			foreach (ActionEvent reservedActionEvent in _reservedActionEvents)
			{
				if (reservedActionEvent.startTime <= _skillActionElapsedTime)
				{
					ExecuteActionEvent(reservedActionEvent);
				}
			}
			_reservedActionEvents.RemoveAll((ActionEvent ev) => ev.startTime <= _skillActionElapsedTime);
			if (_currentAction.actionDuration >= 0f && _currentAction.actionDuration <= _skillActionElapsedTime)
			{
				OnSkillActionTimeEnded();
			}
		}

		public override void OnExit()
		{
			if (!IsActivated)
			{
				return;
			}
			base.OnExit();
			_actionDisposable?.Dispose();
			_actionCancellationTokenSource?.Cancel();
			_actionCancellationTokenSource?.Dispose();
			_actionCancellationTokenSource = null;
			_isCompleted = false;
			_skillId = 0;
			_aimPos = Vector3.zero;
			_normalizedDir = Vector3.zero;
			_remainMoveTime = 0f;
			_moveSpeed = 0f;
			RemoveActionLinkedBuffs();
			RemoveActionLinkedVfx();
			RemoveSkillLinkedVfx();
			UnitView.SetNoObstacleAvoidanceType(false);
			UnitModel.UsingSkillId = 0;
			UnitModel.UpdateAim();
			UnitModel.StartGlobalSkillCooldown();
			if (UnitModel is CharacterModel characterModel)
			{
				GameHost.SendToAll(new NtfSkillIndicator
				{
					ClientId = characterModel.ClientId,
					IndicatorType = (ESkillIndicatorType)0
				});
			}
			UnitModel.GetComponentsInChildren<IndicatorVfx>().ForEach(delegate(IndicatorVfx v)
			{
				v.DestroyIndicateVfx();
			});
		}

		public override EActionState GetNextStateEvent(IUnitEvent unitEvent)
		{
			if (unitEvent is CutsceneStartEvent)
			{
				return EActionState.Cutscene;
			}
			if (unitEvent is CrowdControlEvent crowdControlEvent)
			{
				if (crowdControlEvent.CcInfo.ccType != ECrowdControlType.None)
				{
					return EActionState.CrowdControlled;
				}
			}
			else
			{
				if (unitEvent is UseSkillEvent)
				{
					return EActionState.Skill;
				}
				if (unitEvent is DieEvent)
				{
					return EActionState.Dying;
				}
				if (unitEvent is CaptureEvent)
				{
					return EActionState.Captured;
				}
				if (unitEvent is EscapeEvent)
				{
					return EActionState.Escaped;
				}
			}
			if (!_isCompleted && !IsWaitingInputSkillRemoved())
			{
				if (!ProceedNextSkillAction(unitEvent))
				{
					TriggerActiveSkill(unitEvent);
				}
				return EActionState.None;
			}
			return (UnitModel.MoveDir != Vector3.zero) ? EActionState.Move : EActionState.Idle;
		}

		public override Vector3 GetForward()
		{
			return UnitModel.CalculatedDeltaAimPos;
		}

		private bool IsWaitingInputSkillRemoved()
		{
			if (UnitModel.UnitType == EUnitType.Character && _skillId >= 1 && _nextNodeNeedInput)
			{
				return !UnitModel.GetActiveSkillIds().Contains(_skillId);
			}
			return false;
		}

		private bool IsNextNodeNeedInput(SkillActionNode node)
		{
			if (UnitModel.UnitType == EUnitType.Character && node.childNodes != null)
			{
				return node.childNodes.Any((SkillActionNode _) => _.skillAction.SkillTrigger is SkillInputTrigger skillInputTrigger && (skillInputTrigger.SkillInputType == ESkillInputEvent.CurrentInputUp || skillInputTrigger.SkillInputType == ESkillInputEvent.CurrentInputDown));
			}
			return false;
		}

		protected bool ProceedNextSkillAction(IUnitEvent unitEvent)
		{
			// The binary returns false for a null event first (@0x1806cc230); UnitStateMachine.OnUpdate sends TriggerEvent(null)
			// every tick, and the trigger lambda below dereferences the event.
			if (unitEvent == null)
			{
				return false;
			}
			if (_currentActionNode == null)
			{
				return false;
			}
			List<ITarget> triggerTargets = null;
			SkillActionNode skillActionNode = _currentActionNode.childNodes?.Where(delegate(SkillActionNode node)
			{
				if (unitEvent.CheckSkillTriggered(UnitModel, node.skillAction.SkillTrigger, out var triggerTargets2))
				{
					triggerTargets = triggerTargets2;
					return true;
				}
				return unitEvent is SkillInputEvent skillInputEvent && CheckCurrentSkillInputTriggered(_skillId, node.skillAction.SkillTrigger, skillInputEvent);
			}).Where((SkillActionNode node) => CheckBuffCondition(node.skillAction.BuffConditionList) && UnitController.CheckTalentCondition(node.skillAction.TalentConditionList)).FirstOrDefault((SkillActionNode node) => !node.skillAction.checkTime || (_skillActionElapsedTime >= node.skillAction.timeFrom && _skillActionElapsedTime <= node.skillAction.timeTo));
			if (skillActionNode == null)
			{
				return false;
			}
			if (!UnitModel.CheckSkillActionCost(skillActionNode, Skill))
			{
				return false;
			}
			if (skillActionNode.skillAction.ExecuteType == ESkillActionExecuteType.SpecifiedTime)
			{
				float num = skillActionNode.skillAction.executeTime - _skillActionElapsedTime;
				if (num >= 0f)
				{
					Logger.Trace("waitng next node " + _currentActionNode.Text + " -> " + skillActionNode.Text);
					ObservableFixed.FixedTimer(TimeSpan.FromSeconds(num)).Subscribe(delegate
					{
						Logger.Trace("goto next node " + _currentActionNode.Text + " -> " + skillActionNode.Text);
						ExecuteSkillAction(skillActionNode, unitEvent, triggerTargets ?? _triggerTargets);
					}).AddTo(_actionDisposable);
					return true;
				}
			}
			Logger.Trace("goto next node " + _currentActionNode.Text + " -> " + skillActionNode.Text);
			ExecuteSkillAction(skillActionNode, unitEvent, triggerTargets ?? _triggerTargets);
			return true;
		}

		private bool CheckBuffCondition(List<BuffCondition> buffConditions)
		{
			if (buffConditions == null || buffConditions.Count == 0)
			{
				return true;
			}
			UnitController triggerTargetController = (((object)(TriggerTarget as UnitModel) != null && (UnityEngine.Object)(TriggerTarget as UnitModel) != null) ? (TriggerTarget as UnitModel).GetComponent<UnitController>() : null);
			return buffConditions.All(delegate(BuffCondition c)
			{
				switch (c.ConditionOwnerType)
				{
				case EConditionTargetType.SkillOwner:
					return UnitController.CheckBuffCondition(c, UnitController.NetworkObjectId, (triggerTargetController != null) ? triggerTargetController.NetworkObjectId : 0);
				case EConditionTargetType.TriggerTarget:
					return triggerTargetController != null && triggerTargetController.CheckBuffCondition(c, UnitController.NetworkObjectId, triggerTargetController.NetworkObjectId);
				default:
					return true;
				}
			});
		}

		protected override bool TriggerActiveSkill(IUnitEvent unitEvent)
		{
			if (unitEvent is SkillInputEvent skillInputEvent)
			{
				List<int> activeSkillIds = UnitModel.GetActiveSkillIds();
				if (activeSkillIds.Count == 0)
				{
					return false;
				}
				foreach (int checkSkillId in activeSkillIds)
				{
					CancelableTime cancelableTime = GetCancelableTime(checkSkillId);
					if (_skillId <= 0 || cancelableTime == null || !TSkill.Table.TryGetValue(checkSkillId, out var skill) || !UnitModel.CheckSkill(checkSkillId) || skill.SkillType == ESkillType.Passive || skill.SkillType == ESkillType.Preview)
					{
						continue;
					}
					if (!skillInputEvent.CheckSkillTriggered(UnitModel, skill.RootActionNode.SkillTrigger, out var triggerTargets) && !CheckCurrentSkillInputTriggered(checkSkillId, skill.RootActionNode.SkillTrigger, skillInputEvent))
					{
						continue;
					}
					ITarget triggerTarget = triggerTargets?.FirstOrDefault();
					if (cancelableTime.ExecuteType != ESkillActionExecuteType.SpecifiedTime || cancelableTime.ExecuteTime <= _skillActionElapsedTime)
					{
						UnitController.HandleUnitEvent(new UseSkillEvent
						{
							SkillId = checkSkillId,
							TriggerTarget = triggerTarget
						});
						return true;
					}
					if (_useCachedInput && _pendingCancelSkillId != 0 && _pendingCancelSkillId != _skillId && _pendingCancelSkillId != checkSkillId)
					{
						continue;
					}
					_cachedInput.Clear();
					_useCachedInput = true;
					_pendingCancelSkillId = checkSkillId;
					ObservableFixed.FixedTimer(TimeSpan.FromSeconds(cancelableTime.ExecuteTime - _skillActionElapsedTime)).Subscribe(delegate
					{
						UnitController.HandleUnitEvent(new UseSkillEvent
						{
							SkillId = checkSkillId,
							TriggerTarget = triggerTarget,
							CachedSkillInput = new Queue<ESkillInputEvent>(_cachedInput)
						});
					}).AddTo(_actionDisposable);
					return true;
				}
			}
			return false;
		}

		private CancelableTime GetCancelableTime(int inputSkillId)
		{
			CancelableTime[] cancelableTimes = _currentAction.cancelableTimes;
			if (cancelableTimes == null || cancelableTimes.Length == 0)
			{
				return null;
			}
			return cancelableTimes.Where((CancelableTime t) => _skillActionElapsedTime >= t.startTime && _skillActionElapsedTime < t.endTime).FirstOrDefault((CancelableTime t) => t.IsCancelableByAnySkill || (t.cancelSkillIds != null && t.cancelSkillIds.Contains(inputSkillId)));
		}

		private void SendSkillIndicator()
		{
			if (UnitModel is CharacterModel characterModel)
			{
				GameHost.SendToAll(new NtfSkillIndicator
				{
					ClientId = characterModel.ClientId,
					IndicatorType = _currentActionNode.skillAction.indicatorType,
					CastRange = _currentActionNode.skillAction.castRangeRadius,
					EffectRange = _currentActionNode.skillAction.effectAreaRadius,
					Length = _currentActionNode.skillAction.LineLength,
					Angle = _currentActionNode.skillAction.effectAreaAngle
				});
			}
		}

		private void SendSkillIndicatorOff()
		{
			if (UnitModel is CharacterModel characterModel)
			{
				GameHost.SendToAll(new NtfSkillIndicator
				{
					ClientId = characterModel.ClientId,
					IndicatorType = (ESkillIndicatorType)0
				});
			}
		}

		public void PlayAnimation()
		{
			if (_animationQueue.Count == 0)
			{
				return;
			}
			_currentAnimationData = _animationQueue.Dequeue();
			AnimationSpeed[] animationSpeeds;
			if (Mathf.Approximately(1f, _animationTimeScale))
			{
				animationSpeeds = _currentAnimationData.animationSpeeds;
			}
			else if (_currentAnimationData.animationSpeeds == null)
			{
				animationSpeeds = new AnimationSpeed[1]
				{
					new AnimationSpeed
					{
						endTime = -1f,
						speed = _animationTimeScale
					}
				};
			}
			else
			{
				animationSpeeds = _currentAnimationData.animationSpeeds.Select(delegate(AnimationSpeed a)
				{
					AnimationSpeed animationSpeed = new AnimationSpeed();
					animationSpeed.endTime = a.endTime / _animationTimeScale;
					animationSpeed.speed = _animationTimeScale * a.speed;
					return animationSpeed;
				}).ToArray();
			}
			string animationName;
			string multiTrackAnimationName = null;
			if (!_currentAnimationData.UseMultiTrackMoveAnimation)
			{
				animationName = ((!string.IsNullOrEmpty(_currentAnimationData.moveAnimationName) && UnitModel.MoveDir != Vector3.zero) ? _currentAnimationData.moveAnimationName : _currentAnimationData.animationName);
			}
			else if (string.IsNullOrEmpty(_currentAnimationData.moveAnimationName) || UnitModel.MoveDir == Vector3.zero)
			{
				animationName = _currentAnimationData.animationName;
			}
			else
			{
				animationName = UnitView.GetMoveAnimation();
				multiTrackAnimationName = _currentAnimationData.moveAnimationName;
			}
			if (string.IsNullOrEmpty(animationName))
			{
				return;
			}
			GameHost.SendToAll(new NtfPlayAnimation
			{
				NetworkObjectId = UnitModel.NetworkObjectId,
				AnimationName = animationName,
				IsLoop = _currentAnimationData.isLoop,
				AnimStartOffsetTime = _currentAnimationData.animStartOffsetTime,
				AnimationSpeeds = animationSpeeds,
				MultiTrackAnimationName = multiTrackAnimationName
			});
			if (_currentAnimationData.IsLoopAnimationData)
			{
				_animationQueue.Enqueue(_currentAnimationData);
			}
			if (!(_currentAnimationData.duration < 0f))
			{
				ObservableFixed.FixedTimer(TimeSpan.FromSeconds(_currentAnimationData.duration / _animationTimeScale)).Subscribe(delegate
				{
					PlayAnimation();
				}).AddTo(_actionDisposable);
			}
		}

		private void InitMoveAction()
		{
			_isMoveStart = false;
			_moveType = ESkillMoveType.None;
			_moveDir = Vector3.zero;
			_remainMoveTime = 0f;
			_moveSpeed = 0f;
			_moveSpeedCurve = null;
			_isGroundSfxActive = false;
			_groundSfxTimer = 0f;
			if (_currentAction.moveType != ESkillMoveType.None)
			{
				if (_currentAction.moveType == ESkillMoveType.MoveByInput || _currentAction.moveType == ESkillMoveType.MoveByInputNoAim)
				{
					UnitModel.RxMoveDir.Pairwise().Subscribe(delegate(Pair<Vector3> pair)
					{
						UpdateMoveByInputAnimation(pair.Previous, pair.Current);
					}).AddTo(_actionDisposable);
				}
				if (_currentAction.moveStartTime <= 0f)
				{
					OnStartMove();
				}
			}
		}

		private void UpdateMoveByInputAnimation(Vector3 prev, Vector3 current)
		{
			if (prev != Vector3.zero && current != Vector3.zero)
			{
				return;
			}
			if (_currentAnimationData == null)
			{
				Logger.Warning(string.Format("{0}'s skill {1} has no animation data for inputMove animation.", UnitModel.TableId, _skillId));
			}
			string text = _currentAnimationData?.moveAnimationName;
			string text2 = (string.IsNullOrEmpty(text) ? "default/run" : text);
			string text3 = (string.IsNullOrEmpty(_currentAnimationData.animationName) ? "default/walk" : _currentAnimationData.animationName);
			if (_currentAnimationData.UseMultiTrackMoveAnimation)
			{
				if (prev != Vector3.zero)
				{
					GameHost.SendToAll(new NtfPlayAnimation
					{
						NetworkObjectId = UnitModel.NetworkObjectId,
						AnimationName = _currentAnimationData.animationName,
						IsLoop = _currentAnimationData.isLoop
					});
				}
				else
				{
					GameHost.SendToAll(new NtfPlayAnimation
					{
						NetworkObjectId = UnitModel.NetworkObjectId,
						AnimationName = UnitView.GetMoveAnimation(),
						IsLoop = true,
						MultiTrackAnimationName = text2
					});
				}
			}
			else if (!string.IsNullOrEmpty(_currentAnimationData.moveAnimationName))
			{
				if (prev == Vector3.zero)
				{
					GameHost.SendToAll(new NtfUpdateAnimation
					{
						NetworkObjectId = UnitModel.NetworkObjectId,
						AnimationName = text2,
						PrevAnimationName = text3
					});
				}
				else
				{
					GameHost.SendToAll(new NtfUpdateAnimation
					{
						NetworkObjectId = UnitModel.NetworkObjectId,
						AnimationName = text3,
						PrevAnimationName = text2
					});
				}
			}
		}

		private void OnUpdateMoveAndAim()
		{
			if (!_isMoveStart && _currentAction.moveStartTime <= _skillActionElapsedTime)
			{
				OnStartMove();
			}
			if (UnitModel.MoveDir != Vector3.zero && _currentAction.cancelableTimes != null && _currentAction.cancelableTimes.Any((CancelableTime t) => _skillActionElapsedTime >= t.startTime && _skillActionElapsedTime < t.endTime && t.IsCancelableByMove))
			{
				_isCompleted = true;
			}
			if (_moveType == ESkillMoveType.None)
			{
				return;
			}
			if (_moveType != ESkillMoveType.MoveByInput)
			{
				UnitModel.SetCalculatedDeltaAimPos(_deltaAimPos);
				if (_moveType != ESkillMoveType.MoveByInputNoAim)
				{
					float calculatedMoveSpeed = GetCalculatedMoveSpeed();
					float dt = Mathf.Min(Time.fixedDeltaTime * _logicTimeScale, _remainMoveTime);
					UnitView.Move(_moveDir, calculatedMoveSpeed, dt);
					CheckMovable();
					UpdateGroundSfxOnMove();
					return;
				}
			}
			UnitView.Move(UnitModel.MoveDir, UnitModel.MoveSpeed, Time.fixedDeltaTime);
			CheckMovable();
			UpdateGroundSfxOnMove();
		}

		private float GetCalculatedMoveSpeed()
		{
			if (_moveSpeedCurve == null)
			{
				return _moveSpeed;
			}
			float num = _currentAction.moveEndTime - _currentAction.moveStartTime;
			return _moveSpeedCurve.Evaluate((num - _remainMoveTime) / num) * _moveSpeed;
		}

		private void CheckMovable()
		{
			if (_moveType == ESkillMoveType.MoveByInput || _moveType == ESkillMoveType.MoveByInputNoAim)
			{
				return;
			}
			_remainMoveTime -= Time.fixedDeltaTime * _logicTimeScale;
			if (_remainMoveTime <= 0f)
			{
				StopMoveAction(false, TriggerTarget);
				return;
			}
			if (!_ignoreMoveCollision)
			{
				if (_moveDir == Vector3.zero)
				{
					StopMoveAction(false, TriggerTarget);
					return;
				}
				if (!Physics.Raycast(UnitView.transform.position, _moveDir, out var _, Time.fixedDeltaTime * _moveSpeed * _logicTimeScale + UnitView.Radius, Const.LayerMask.HIGH_OBSTACLE | Const.LayerMask.LOW_OBSTACLE))
				{
					return;
				}
				UnitModel hitTarget = null;
				if (Physics.Raycast(UnitView.transform.position, _moveDir, out var hitInfo2, Time.fixedDeltaTime * _moveSpeed * _logicTimeScale) && hitInfo2.collider != null)
				{
					hitTarget = hitInfo2.collider.GetComponent<UnitModel>();
				}
				StopMoveAction(true, hitTarget);
				return;
			}
			if (Physics.Raycast(UnitView.transform.position, _moveDir, out var _, Time.fixedDeltaTime * _moveSpeed * _logicTimeScale + UnitView.Radius, Const.LayerMask.HIGH_OBSTACLE | Const.LayerMask.LOW_OBSTACLE | Const.LayerMask.BREAKABLE_PROP))
			{
				StopMoveAction(true, null);
			}
		}

		private void StopMoveAction(bool isBlocked, ITarget hitTarget = null)
		{
			_isGroundSfxActive = false;
			_moveType = ESkillMoveType.None;
			UnitController.HandleUnitEvent(new MoveFinishEvent
			{
				IsBlocked = isBlocked,
				HitTarget = hitTarget
			});
		}

		private void OnStartMove()
		{
			_isMoveStart = true;
			_moveType = _currentAction.moveType;
			_moveSpeed = _currentAction.moveSpeed;
			if (TSkill.Table[_skillId].SkillTags != null && TSkill.Table[_skillId].SkillTags.Contains(ESkillTagType.Dash))
			{
				float stat = UnitModel.GetStat(EStatType.DashDistancePercent);
				if (stat != 0f)
				{
					_moveSpeed = (stat + 100f) / 100f * _moveSpeed;
				}
			}
			_moveSpeedCurve = _currentAction.MoveSpeedCurve;
			_ignoreMoveCollision = _currentAction.ignoreMoveCollision;
			_remainMoveTime = ((_currentAction.actionDuration >= 0f) ? (_currentAction.moveEndTime - _currentAction.moveStartTime) : 99999f);
			UnitView.SetNoObstacleAvoidanceType(_ignoreMoveCollision);
			InitGroundSfxOnMove();
			if (_moveType == ESkillMoveType.MoveByInput || _moveType == ESkillMoveType.MoveByInputNoAim)
			{
				return;
			}
			Vector3 moveDir;
			switch (_moveType)
			{
			case ESkillMoveType.MoveToOppositeAimDir:
				moveDir = -_normalizedDir;
				break;
			case ESkillMoveType.MoveToMoveDir:
				moveDir = ((UnitModel.MoveDir != Vector3.zero) ? UnitModel.MoveDir : _normalizedDir);
				break;
			case ESkillMoveType.MoveToAimDir:
				moveDir = _normalizedDir;
				break;
			case ESkillMoveType.MoveToAimPos:
				moveDir = CalculateMoveToAimPos();
				break;
			case ESkillMoveType.MoveToTarget:
				moveDir = CalculateMoveToTarget();
				break;
			case ESkillMoveType.MoveIfAligned:
				moveDir = ((UnitModel.MoveDir != Vector3.zero && Vector3.Angle(_normalizedDir, UnitModel.MoveDir) <= 45f) ? _normalizedDir : Vector3.zero);
				break;
			case ESkillMoveType.MoveIfOpposite:
				moveDir = ((UnitModel.MoveDir != Vector3.zero && Vector3.Angle(_normalizedDir, UnitModel.MoveDir) >= 135f) ? (-_normalizedDir) : Vector3.zero);
				break;
			default:
				moveDir = _moveDir;
				break;
			}
			_moveDir = moveDir;
		}

		private void InitGroundSfxOnMove()
		{
			if (_currentAction.IsGroundSfxOnMove)
			{
				_isGroundSfxActive = true;
				_groundSfxTimer = 0f;
			}
		}

		private void UpdateGroundSfxOnMove()
		{
			if (!_isGroundSfxActive)
			{
				return;
			}
			_groundSfxTimer -= Time.fixedDeltaTime * _logicTimeScale;
			if (_groundSfxTimer <= 0f)
			{
				_groundSfxTimer = _currentAction.groundSfxInterval;
				EGroundType groundType = BackgroundTileTypeCore.GetGroundType(UnitModel.Position);
				string[] sfxNames = TSkillGroundMoveSfx.Table[groundType].SfxNames;
				if (sfxNames != null && sfxNames.Length != 0)
				{
					string sfx = sfxNames[RandomUtility.GetRandomInt(sfxNames.Length)];
					GameHost.SendToAll(new NtfPlaySfx
					{
						Sfx = sfx,
						Position = UnitModel.Position.ToProtoVector3(),
						TargetNetworkObjectId = UnitModel.NetworkObjectId
					});
				}
			}
		}

		private void UpdateAimPosition()
		{
			Vector3 normalizedDir = _normalizedDir;
			if (!_currentAction.AutoAimToTarget)
			{
				UpdateAimByInput();
			}
			else
			{
				UpdateAimToTarget();
			}
			if (!_isFirstAimUpdate)
			{
				if (_currentAction.maxAimAnglePerSecond > 0f && normalizedDir != Vector3.zero && _normalizedDir != Vector3.zero)
				{
					float num = _currentAction.maxAimAnglePerSecond * Time.fixedDeltaTime * _logicTimeScale;
					if (Vector3.Angle(normalizedDir, _normalizedDir) > num)
					{
						_normalizedDir = Vector3.RotateTowards(normalizedDir, _normalizedDir, num * ((float)Math.PI / 180f), 0f).normalized;
						float magnitude = new Vector3(_deltaAimPos.x, 0f, _deltaAimPos.z).magnitude;
						_deltaAimPos = new Vector3(magnitude * _normalizedDir.x, _deltaAimPos.y, magnitude * _normalizedDir.z);
						_aimPos = UnitView.transform.position + _deltaAimPos;
						if (_currentAction.AutoAimToTarget)
						{
							UnitModel.SetCalculatedDeltaAimPos(_deltaAimPos);
						}
					}
				}
			}
			else
			{
				_isFirstAimUpdate = false;
			}
			UnitModel.CalculatedDeltaAimPos = _deltaAimPos;
			void UpdateAimByInput()
			{
				Vector3 vector;
				if (_currentAction.indicatorType == ESkillIndicatorType.SelectCastInRange)
				{
					vector = ((!(UnitModel.CalculatedDeltaAimPos.magnitude > _currentAction.castRangeRadius)) ? (UnitModel.DeltaAimPos + UnitModel.transform.position) : (UnitModel.transform.position + UnitModel.DeltaAimPos.normalized * _currentAction.castRangeRadius));
				}
				else
				{
					vector = UnitModel.DeltaAimPos + UnitModel.transform.position;
				}
				_aimPos = vector;
				_deltaAimPos = _aimPos - UnitView.transform.position;
				_normalizedDir = new Vector3(_deltaAimPos.x, 0f, _deltaAimPos.z).normalized;
			}
			void UpdateAimToTarget()
			{
				ITarget target = _triggerTargets?.FirstOrDefault();
				if (target != null)
				{
					Vector3 vector = target.Position - UnitModel.Position;
					UnitModel.DeltaAimPos = vector;
					UnitModel.SetCalculatedDeltaAimPos(vector);
					_deltaAimPos = vector;
					_aimPos = UnitModel.transform.position + vector;
					_normalizedDir = new Vector3(vector.x, 0f, vector.z).normalized;
				}
			}
		}

		private Vector3 CalculateMoveToAimPos()
		{
			_moveDir = _normalizedDir;
			float castRangeRadius = _currentAction.castRangeRadius;
			float moveOffset = _currentAction.moveOffset;
			float num = _remainMoveTime;
			float num2 = moveOffset + castRangeRadius;
			float magnitude = (_aimPos - UnitView.transform.position).magnitude;
			float num3 = ((magnitude <= castRangeRadius) ? magnitude : castRangeRadius);
			float num4 = Mathf.Abs(num3 + moveOffset);
			if (_currentAction.moveSpeedType == EMoveSpeedType.Distance)
			{
				num = num4 / num2 * num;
			}
			_remainMoveTime = num;
			_moveSpeed = num4 / num;
			return _moveDir;
		}

		private Vector3 CalculateMoveToTarget()
		{
			ITarget target = _triggerTargets.FirstOrDefault();
			Vector3 vector = target.Position - UnitModel.Position;
			float moveOffset = _currentAction.moveOffset;
			float num = vector.magnitude + moveOffset;
			Vector3 normalized = vector.normalized;
			if (0f - moveOffset > normalized.magnitude)
			{
				return Vector3.zero;
			}
			if (_currentAction.moveSpeedType == EMoveSpeedType.Distance)
			{
				_moveSpeed = num / _remainMoveTime;
			}
			else
			{
				_remainMoveTime = num / _moveSpeed;
			}
			return new Vector3(normalized.x, 0f, normalized.z).normalized;
		}

		private void InitSkillAction()
		{
			_actionCancellationTokenSource?.Cancel();
			_actionCancellationTokenSource?.Dispose();
			_actionCancellationTokenSource = new CancellationTokenSource();
			_reservedActionEvents.Clear();
			if (UnitModel is CharacterModel)
			{
				GameHost.Receive<ReqInputSkill>().Where((ReqInputSkill req) => req.SenderId == UnitController.ClientId).Where((ReqInputSkill req) => _useCachedInput)
					.Subscribe(delegate(ReqInputSkill req)
					{
						_cachedInput.Enqueue(req.InputEvent);
					})
					.AddTo(_actionDisposable);
			}
			_currentAction.actionEvents.ForEach(delegate(ActionEvent actionEvent)
			{
				_reservedActionEvents.Add(actionEvent);
			});
		}

		private void OnSkillActionTimeEnded()
		{
			SkillActionNode skillActionNode = _currentActionNode.childNodes?.Where((SkillActionNode _) => _.SkillTrigger == null).Where((SkillActionNode _) => CheckBuffCondition(_.skillAction.BuffConditionList) && UnitController.CheckTalentCondition(_.skillAction.TalentConditionList)).FirstOrDefault((SkillActionNode _) => UnitModel.CheckSkillActionCost(_, Skill));
			if (skillActionNode != null)
			{
				ExecuteSkillAction(skillActionNode, null, _triggerTargets);
			}
			else
			{
				_isCompleted = true;
			}
		}

		private void ExecuteActionEvent(ActionEvent actionEvent)
		{
			GameManager.AssertIfNotHost();
			List<ITarget> list = ((actionEvent.ActionTarget != ESkillActionTarget.TriggerTarget) ? new List<ITarget> { UnitModel } : _triggerTargets);
			foreach (ITarget item in list)
			{
				if (UnitController.IsActionEventExcutable(actionEvent, UnitController.NetworkObjectId, item?.NetworkObjectId ?? 0))
				{
					ExecuteActionEventOnTarget(actionEvent, item);
				}
			}
		}

		private void ExecuteActionEventOnTarget(ActionEvent actionEvent, ITarget actionTarget)
		{
			if (actionEvent is BuffActionEvent buffActionEvent)
			{
				if (actionTarget is UnitModel unitModel)
				{
					UnitController.OnBuffActionEvent(buffActionEvent, unitModel);
					if (buffActionEvent.RemoveOnActionEnd)
					{
						_actionLinkedBuffList.Add((unitModel, buffActionEvent.Id, buffActionEvent.stackAmount));
					}
				}
			}
			else if (actionEvent is RemoveStatusEffectActionEvent actionEvent2)
			{
				UnitController.OnRemoveBuffActionEvent(actionEvent2, actionTarget);
			}
			else if (actionEvent is VfxEvent vfxEvent)
			{
				long item = UnitController.OnVfxEvent(vfxEvent, actionTarget);
				if (vfxEvent.DestroyOnActionEnd)
				{
					_actionLinkedVfxList.Add(item);
				}
				else if (vfxEvent.DestroyOnSkillEnd)
				{
					_skillLinkedVfxList.Add(item);
				}
			}
			else if (actionEvent is CrowdControlActionEvent actionEvent3)
			{
				UnitController.OnCrowdControlActionEvent(actionEvent3, actionTarget);
			}
			else if (actionEvent is SfxEvent sfxEvent)
			{
				UnitController.OnSfxEvent(sfxEvent, actionTarget);
			}
			else if (actionEvent is IndicatorVfxEvent indicatorVfxEvent)
			{
				if (actionTarget is UnitModel casterUnitModel)
				{
					UnitController.OnIndicatorVfxEvent(indicatorVfxEvent, casterUnitModel, _deltaAimPos, actionTarget);
				}
			}
			else if (actionEvent is HitBoxIteratorEvent hitBoxIteratorEvent)
			{
				UnitController.OnHitBoxIteratorEvent(hitBoxIteratorEvent, actionTarget, _skillId, _deltaAimPos, _currentAction, _logicTimeScale, hitBoxIteratorEvent.CancelOnActionEnd ? _actionCancellationTokenSource : null);
			}
			else if (actionEvent is SummonActionEvent summonActionEvent)
			{
				UnitController.OnSummonActionEvent(summonActionEvent, actionTarget);
			}
			else if (actionEvent is SpecialFieldActionEvent specialFieldActionEvent)
			{
				UnitController.OnSpecialFieldActionEvent(specialFieldActionEvent, actionTarget);
			}
			else if (actionEvent is DamageActionEvent actionEvent4)
			{
				UnitController.OnDamageActionEvent(actionEvent4, actionTarget, _skillId);
			}
			else if (actionEvent is RecoveryActionEvent actionEvent5)
			{
				UnitController.OnRecoveryActionEvent(actionEvent5, actionTarget, _skillId);
			}
			else if (actionEvent is StressDamageActionEvent actionEvent6)
			{
				UnitController.OnStressDamageEvent(actionEvent6, actionTarget, _skillId);
			}
			else if (actionEvent is StressRecoveryActionEvent actionEvent7)
			{
				UnitController.OnStressRecoveryEvent(actionEvent7, actionTarget, _skillId);
			}
			else if (actionEvent is SoulRecoveryActionEvent actionEvent8)
			{
				UnitController.OnSoulRecoveryEvent(actionEvent8, actionTarget);
			}
			else if (actionEvent is BrightnessDamageActionEvent actionEvent9)
			{
				UnitController.OnBrightnessDamageEvent(actionEvent9, actionTarget, _skillId);
			}
			else if (actionEvent is BrightnessRecoveryActionEvent actionEvent10)
			{
				UnitController.OnBrightnessRecoveryEvent(actionEvent10, actionTarget, _skillId);
			}
			else if (actionEvent is ConsumeItemActionEvent consumeItemActionEvent)
			{
				UnitController.OnConsumeItemActionEvent(consumeItemActionEvent, actionTarget);
			}
			else if (actionEvent is CameraShakeActionEvent cameraShakeActionEvent)
			{
				UnitController.OnCameraShakeActionEvent(cameraShakeActionEvent);
			}
			else if (actionEvent is KillActionEvent)
			{
				UnitController.OnKillActionEvent(actionTarget);
			}
			else if (actionEvent is TeleportActionEvent teleportActionEvent)
			{
				UnitController.OnTeleportActionEvent(teleportActionEvent, actionTarget, TriggerTarget);
			}
			else if (actionEvent is ForceUpdateTargetActionEvent forceUpdateTargetActionEvent)
			{
				UnitController.OnForceUpdateTargetActionEvent(forceUpdateTargetActionEvent, actionTarget);
			}
			else if (actionEvent is HitBoxEvent hitBoxEvent)
			{
				UnitController.OnTargetableHitBoxEvent(hitBoxEvent, actionTarget, _skillId, _deltaAimPos, _currentAction);
			}
			else if (actionEvent is ApplyPolymorphSkillsActionEvent applyPolymorphSkillsActionEvent)
			{
				UnitController.OnApplyPolymorphSkillsEvent(applyPolymorphSkillsActionEvent, actionTarget);
			}
			else if (actionEvent is RevertPolymorphSkillsActionEvent revertPolymorphSkillsActionEvent)
			{
				UnitController.OnRevertPolymorphSkillsEvent(revertPolymorphSkillsActionEvent, actionTarget);
			}
			else if (actionEvent is ChangeSkinActionEvent changeSkinActionEvent)
			{
				UnitController.OnChangeSkinActionEvent(changeSkinActionEvent, actionTarget);
			}
			else if (actionEvent is SpreadAggroActionEvent spreadAggroActionEvent)
			{
				UnitController.OnSpreadAggroActionEvent(spreadAggroActionEvent, actionTarget);
			}
			else if (actionEvent is ShowConsumeGaugeActionEvent actionEvent11)
			{
				UnitController.OnShowConsumeGaugeActionEvent(actionEvent11);
			}
			else if (actionEvent is HitBoxCollectActionEvent hitBoxCollectActionEvent)
			{
				UnitController.OnHitBoxCollectActionEvent(hitBoxCollectActionEvent, actionTarget);
			}
			else if (actionEvent is MonologueActionEvent monologueActionEvent)
			{
				UnitController.OnMonologueActionEvent(monologueActionEvent, actionTarget);
			}
			else if (actionEvent is ShieldActionEvent actionEvent12)
			{
				UnitController.OnShieldActionEvent(actionEvent12, actionTarget);
			}
			else if (actionEvent is AcquireParadoxActionEvent actionEvent13)
			{
				UnitController.OnAcquireParadoxActionEvent(actionEvent13, actionTarget);
			}
			else if (actionEvent is RemoveParadoxActionEvent actionEvent14)
			{
				UnitController.OnRemoveParadoxActionEvent(actionEvent14, actionTarget);
			}
			else if (actionEvent is AddSuppressionActionEvent actionEvent15)
			{
				UnitController.OnAddSuppressionActionEvent(actionEvent15, actionTarget);
			}
		}

		private void RemoveActionLinkedBuffs()
		{
			if (!_isRemovingActionLinkedBuffs)
			{
				_isRemovingActionLinkedBuffs = true;
				_actionLinkedBuffList.ForEach(delegate((UnitModel owner, int id, int stack) x)
				{
					x.owner?.RemoveBuff(x.id, x.stack, false);
				});
				_actionLinkedBuffList.Clear();
				_isRemovingActionLinkedBuffs = false;
			}
		}

		private void RemoveActionLinkedVfx()
		{
			_actionLinkedVfxList.ForEach(delegate(long id)
			{
				GameHost.SendToAll(new NtfRemoveFx { UniqueId = id });
			});
			_actionLinkedVfxList.Clear();
		}

		private void RemoveSkillLinkedVfx()
		{
			_skillLinkedVfxList.ForEach(delegate(long id)
			{
				GameHost.SendToAll(new NtfRemoveFx { UniqueId = id });
			});
			_skillLinkedVfxList.Clear();
		}
	}
}
