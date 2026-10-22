using Unity.Netcode;

public class EmployeeNpcObject : NpcObject
{
	public readonly NetworkVariable<int> SyncEmployeeState = new NetworkVariable<int>();

	public readonly NetworkVariable<int> SyncEmployeeId = new NetworkVariable<int>();

	private int _salesSlotIndex = -1;

	public void SetSalesSlotIndex(int salesSlotIndex)
	{
		_salesSlotIndex = salesSlotIndex;
	}

	public override void OnHostSpawn()
	{
		base.OnHostSpawn();
		if (_salesSlotIndex >= 0 && PlayerManager.SalesSlots.TryGetValue(_salesSlotIndex, out var slot))
		{
			SyncEmployeeId.Value = slot.EmployeeId;
			if (PlayerManager.Employees.TryGetValue(slot.EmployeeId, out var employee) && slot.IsFinished && employee.IsPanic)
			{
				SyncEmployeeState.Value = 3;
			}
			else
			{
				SyncEmployeeState.Value = (slot.IsFinished ? 2 : 1);
			}
		}
		else
		{
			SyncEmployeeState.Value = 0;
		}
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		SyncEmployeeState.OnValueChanged += OnEmployeeStateChanged;
		PlayAnimationByState((ETycoonEmployeeState)SyncEmployeeState.Value);
		SyncEmployeeId.OnValueChanged += delegate(int _, int newId)
		{
			ApplyEmployeeVisual(newId);
		};
		ApplyEmployeeVisual(SyncEmployeeId.Value);
	}

	public override void OnNetworkDespawn()
	{
		SyncEmployeeState.OnValueChanged -= OnEmployeeStateChanged;
		base.OnNetworkDespawn();
	}

	private void ApplyEmployeeVisual(int employeeId)
	{
		if (employeeId > 0)
		{
			RxEmployeeId.Value = employeeId;
			SetSkin(employeeId.ToString());
			SetSkeletonScaleX(-1f);
		}
	}

	private void OnEmployeeStateChanged(int prev, int curr)
	{
		PlayAnimationByState((ETycoonEmployeeState)curr);
	}

	private void PlayAnimationByState(ETycoonEmployeeState state)
	{
		string text;
		switch (state)
		{
		case ETycoonEmployeeState.InProgress:
			text = "negotiation";
			break;
		case ETycoonEmployeeState.Complete:
			text = "idle";
			break;
		case ETycoonEmployeeState.Panic:
			text = GetRandomPanicAnimation();
			break;
		default:
			return;
		}
		if (text != null)
		{
			PlayAnimation(text, true);
		}
	}

	private static string GetRandomPanicAnimation()
	{
		string[] panicAnimations = Const.EmployeeAnimation.PANIC_ANIMATIONS;
		return panicAnimations[UnityEngine.Random.Range(0, panicAnimations.Length)];
	}
}
