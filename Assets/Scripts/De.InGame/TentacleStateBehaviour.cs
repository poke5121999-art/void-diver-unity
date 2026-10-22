using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TentacleStateBehaviour : StateMachineBehaviour
{
	private static readonly TentacleState[] _allStates;

	private static readonly string[] _stateNames;

	[CompilerGenerated]
	private Action<TentacleState> m_OnTentacleStateChanged;

	public event Action<TentacleState> OnTentacleStateChanged
	{
		[CompilerGenerated]
		add
		{
		}
		[CompilerGenerated]
		remove
		{
		}
	}

	static TentacleStateBehaviour()
	{
	}

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	}
}
