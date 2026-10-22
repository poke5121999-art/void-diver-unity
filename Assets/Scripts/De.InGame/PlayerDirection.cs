using GameCommon.Enum;
using UnityEngine;

public class PlayerDirection : MonoBehaviour
{
	[SerializeField]
	private MeshRenderer meshRenderer;

	public void Init(UnitModel unitModel, Color color)
	{
	}

	private void OnDeltaAimPosChanged(Vector3 pos)
	{
	}

	private void OnStateChanged(EActionState state)
	{
	}
}
