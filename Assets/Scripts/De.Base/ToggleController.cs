using GameCommon.Enum;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
[RequireComponent(typeof(Animator))]
public class ToggleController : MonoBehaviour
{
	private Animator _animator;

	private Toggle _toggle;

	private bool _pendingIsOn;

	private bool _hasPendingIsOn;

	private string FieldName => null;

	private void OnEnable()
	{
	}

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void SetLayout(EToggleLayout layout)
	{
	}

	public void SetIsOnWithoutNotify(bool value)
	{
	}
}
