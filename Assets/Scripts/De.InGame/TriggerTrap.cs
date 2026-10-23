using System.Linq;
using GameCommon.Enum;
using UniRx;
using UniRx.Triggers;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTrap : TrapObject
{
	[SerializeField]
	protected Collider[] TriggerColliders;

	[SerializeField]
	private Animator _animator;

	private readonly HashSet<UnitModel> _targetDatas = new HashSet<UnitModel>();

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		if (!base.IsHost || TriggerColliders == null)
		{
			return;
		}
		Collider[] triggerColliders = TriggerColliders;
		foreach (Collider collider in triggerColliders)
		{
			collider.OnTriggerEnterAsObservable().Subscribe(OnTriggerEnter).AddTo(this);
			collider.OnTriggerExitAsObservable().Subscribe(OnTriggerExit).AddTo(this);
		}
	}

	protected override bool CheckExecutable()
	{
		if (!base.CheckExecutable())
		{
			return false;
		}
		_targetDatas.Where((UnitModel unitModel) => unitModel.State == EActionState.Dead).ToList().ForEach(delegate(UnitModel unitModel)
		{
			_targetDatas.Remove(unitModel);
		});
		return _targetDatas.Count > 0;
	}

	private void OnTriggerEnter(Collider other)
	{
		UnitModel component = other.GetComponent<UnitModel>();
		if (!(component == null) && component.State != EActionState.Dead)
		{
			_targetDatas.Add(component);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		UnitModel component = other.GetComponent<UnitModel>();
		if (!(component == null))
		{
			_targetDatas.Remove(component);
		}
	}

	protected override void OnIsActivatedValueChanged(bool oldValue, bool newValue)
	{
		if (_animator != null)
		{
			_animator.SetBool("ExecuteTrigger", newValue);
		}
	}



}
