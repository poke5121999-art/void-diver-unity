using GameCommon.Enum;
using UnityEngine;

public class EnvironmentProp : MonoBase
{
	[SerializeField]
	private EMeshType _meshType;

	[SerializeField]
	private GameObject _frontObject;

	[SerializeField]
	private GameObject _backObject;

	public void Init(Vector3 forward)
	{
		InGameUtility.LookAtForward(_meshType, forward, _frontObject, _backObject);
	}
}
