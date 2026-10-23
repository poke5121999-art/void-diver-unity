using System;
using UnityEngine;

public class TentacleStateBehaviour : StateMachineBehaviour
{
	private static readonly TentacleState[] _allStates;

	private static readonly string[] _stateNames;

	public event Action<TentacleState> OnTentacleStateChanged;

	static TentacleStateBehaviour()
	{
		_allStates = (TentacleState[])Enum.GetValues(typeof(TentacleState));
		_stateNames = new string[_allStates.Length];
		for (int i = 0; i < _allStates.Length; i++)
		{
			_stateNames[i] = _allStates[i].ToString();
		}
	}

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (this.OnTentacleStateChanged == null)
		{
			return;
		}
		for (int i = 0; i < _allStates.Length; i++)
		{
			if (stateInfo.IsTag(_stateNames[i]))
			{
				this.OnTentacleStateChanged(_allStates[i]);
				return;
			}
		}
		this.OnTentacleStateChanged(TentacleState.Idle);
	}
}
