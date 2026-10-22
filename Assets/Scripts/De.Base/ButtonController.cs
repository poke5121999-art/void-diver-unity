using GameCommon.Enum;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Animator))]
public class ButtonController : MonoBehaviour
{
	[SerializeField]
	private string _animatorParameter;

	[SerializeField]
	private bool _enableFocusedSfx;

	private Animator _animator;

	private Button _button;

	private EButtonState _currentState;

	private EButtonState _baseState;

	private bool _isSelected;

	private bool _isLocked;

	private bool _isPointerOver;

	public Subject<Unit> OnClick { get; }

	public Subject<Unit> OnEnter { get; }

	public Subject<Unit> OnExit { get; }

	protected void Awake()
	{
	}

	private void OnEnable()
	{
	}

	private void Start()
	{
	}

	public void SetLocked(bool isLocked)
	{
	}

	public void SetSelected(bool isSelected)
	{
	}

	private void SetBaseState(EButtonState state)
	{
	}

	private void RefreshState()
	{
	}
}
