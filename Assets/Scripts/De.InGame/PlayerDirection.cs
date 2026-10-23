using GameCommon.Enum;
using UniRx;
using UnityEngine;

public class PlayerDirection : MonoBehaviour
{
	[SerializeField]
	private MeshRenderer meshRenderer;

	public void Init(UnitModel unitModel, Color color)
	{
		meshRenderer.material.color = color;
		unitModel.RxDeltaAimPos.Subscribe(OnDeltaAimPosChanged).AddTo(this);
		unitModel.RxState.Subscribe(OnStateChanged).AddTo(this);
	}

	private void OnDeltaAimPosChanged(Vector3 pos)
	{
		pos += base.transform.position;
		base.transform.LookAt(new Vector3(pos.x, base.transform.position.y, pos.z));
	}

	private void OnStateChanged(EActionState state)
	{
		if (state == EActionState.Dead || state == EActionState.Escaped)
		{
			base.gameObject.SetActive(false);
		}
	}
}
