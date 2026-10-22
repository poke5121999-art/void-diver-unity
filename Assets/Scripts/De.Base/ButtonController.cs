using GameCommon.Enum;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Animator))]
public class ButtonController : MonoBehaviour
{
	[SerializeField]
	private string _animatorParameter = "Layout";

	[SerializeField]
	private bool _enableFocusedSfx;

	private Animator _animator;

	private Button _button;

	private EButtonState _currentState;

	private EButtonState _baseState;

	private bool _isSelected;

	private bool _isLocked;

	private bool _isPointerOver;

	public Subject<Unit> OnClick { get; } = new Subject<Unit>();

	public Subject<Unit> OnEnter { get; } = new Subject<Unit>();

	public Subject<Unit> OnExit { get; } = new Subject<Unit>();

	protected void Awake()
	{
		_animator = GetComponent<Animator>();
		_button = GetComponent<Button>();
	}

	private void OnEnable()
	{
		_currentState = (EButtonState)(-1);
		_isPointerOver = false;
		RefreshState();
	}

	private void Start()
	{
		_button.OnPointerEnterAsObservable().Subscribe(delegate
		{
			if (!InputManager.IsGamePad)
			{
				_isPointerOver = true;
				SetBaseState(EButtonState.Focused);
				OnEnter.OnNext(Unit.Default);
			}
		}).AddTo(this);
		_button.OnPointerExitAsObservable().Subscribe(delegate
		{
			if (!InputManager.IsGamePad)
			{
				_isPointerOver = false;
				SetBaseState(EButtonState.Normal);
				OnExit.OnNext(Unit.Default);
			}
		}).AddTo(this);
		_button.OnPointerDownAsObservable().Subscribe(delegate
		{
			SetBaseState(EButtonState.Pressed);
		}).AddTo(this);
		_button.OnPointerUpAsObservable().Subscribe(delegate
		{
			if (!(_button == null))
			{
				SetBaseState((!_button.enabled) ? EButtonState.Disabled : (_isPointerOver ? EButtonState.Focused : EButtonState.Normal));
			}
		}).AddTo(this);
		_button.OnDisableAsObservable().Subscribe(delegate
		{
			SetBaseState(EButtonState.Disabled);
		}).AddTo(this);
		_button.OnEnableAsObservable().Subscribe(delegate
		{
			SetBaseState(EButtonState.Normal);
		}).AddTo(this);
		_button.OnSelectAsObservable().Subscribe(delegate
		{
			SetBaseState(EButtonState.Focused);
			OnEnter.OnNext(Unit.Default);
		}).AddTo(this);
		_button.OnDeselectAsObservable().Subscribe(delegate
		{
			SetBaseState(_isPointerOver ? EButtonState.Focused : EButtonState.Normal);
			OnExit.OnNext(Unit.Default);
		}).AddTo(this);
		if (EventSystem.current?.currentSelectedGameObject == base.gameObject)
		{
			SetBaseState(EButtonState.Focused);
			OnEnter.OnNext(Unit.Default);
		}
	}

	public void SetLocked(bool isLocked)
	{
		if (_isLocked != isLocked)
		{
			_isLocked = isLocked;
			RefreshState();
		}
	}

	public void SetSelected(bool isSelected)
	{
		if (_isSelected != isSelected)
		{
			_isSelected = isSelected;
			RefreshState();
		}
	}

	private void SetBaseState(EButtonState state)
	{
		_baseState = state;
		RefreshState();
	}

	private void RefreshState()
	{
		EButtonState eButtonState = ResolveState();
		if (_currentState != eButtonState)
		{
			_currentState = eButtonState;
			if (_animator != null)
			{
				_animator.SetInteger(_animatorParameter, (int)eButtonState);
			}
			if (eButtonState == EButtonState.Focused && _enableFocusedSfx)
			{
				SoundManager.Instance?.PlaySfx("MouseOverChange");
			}
		}
		EButtonState ResolveState()
		{
			if (_isLocked)
			{
				return EButtonState.Locked;
			}
			if (!_isSelected)
			{
				return _baseState;
			}
			return EButtonState.Selected;
		}
	}
}
