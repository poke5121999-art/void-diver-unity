using Unity.Netcode;

public class EmployeeNpcObject : NpcObject
{
	public readonly NetworkVariable<int> SyncEmployeeState;

	public readonly NetworkVariable<int> SyncEmployeeId;

	private int _salesSlotIndex;

	public void SetSalesSlotIndex(int salesSlotIndex)
	{
	}

	public override void OnHostSpawn()
	{
	}

	public override void OnNetworkSpawn()
	{
	}

	public override void OnNetworkDespawn()
	{
	}

	private void ApplyEmployeeVisual(int employeeId)
	{
	}

	private void OnEmployeeStateChanged(int prev, int curr)
	{
	}

	private void PlayAnimationByState(ETycoonEmployeeState state)
	{
	}

	private static string GetRandomPanicAnimation()
	{
		return null;
	}

	protected override void __initializeVariables()
	{
	}

	protected override void __initializeRpcs()
	{
	}

	protected internal override string __getTypeName()
	{
		return null;
	}
}
