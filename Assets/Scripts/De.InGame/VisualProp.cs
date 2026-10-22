using GameCommon.Enum;
using UnityEngine;

public class VisualProp : MonoBehaviour
{
	[SerializeField]
	private EMeshType _meshType;

	[SerializeField]
	private GameObject _frontObject;

	[SerializeField]
	private GameObject _backObject;

	public void LookAtForward(Vector3 forward)
	{
	}
}
