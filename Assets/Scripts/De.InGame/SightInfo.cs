using System;
using UnityEngine;

[Serializable]
public class SightInfo
{
	[Header("Sight Info : Common")]
	[SerializeField]
	private float _frontRange;

	[SerializeField]
	private float _backRange;

	[SerializeField]
	private float _viewAngle;

	[Header("Sight Info : In-Game")]
	[SerializeField]
	private bool _toggleOnOff;

	[SerializeField]
	private bool _isBlinded;

	[SerializeField]
	private bool _isMine;

	[SerializeField]
	private bool _isShared;

	private Transform _sightTransform;

	public float FrontRange => _frontRange;

	public float BackRange => _backRange;

	public float ViewAngle => _viewAngle;

	public bool ToggleOnOff => _toggleOnOff;

	public bool IsBlinded => _isBlinded;

	public bool IsMine => _isMine;

	public bool IsShared => _isShared;

	public Transform SightTransform => _sightTransform;

	public SightInfo()
	{
	}

	public SightInfo(in float frontRange, in float backRange, in float viewAngle)
	{
		_frontRange = frontRange;
		_backRange = backRange;
		_viewAngle = viewAngle;
		_toggleOnOff = false;
		_isBlinded = false;
	}

	public SightInfo(in Transform characterTransform, in float frontRange, in float backRange, in float viewAngle)
	{
		_sightTransform = characterTransform;
		_frontRange = frontRange;
		_backRange = backRange;
		_viewAngle = viewAngle;
		_toggleOnOff = false;
		_isBlinded = false;
	}

	public void SetSightInfo(in float frontRange, in float backRange, in float viewAngle)
	{
		_frontRange = frontRange;
		_backRange = backRange;
		_viewAngle = viewAngle;
	}

	public void SetTransform(Transform inTransform)
	{
		_sightTransform = inTransform;
	}

	public void SetOnOffToggle(in bool targetOnOff)
	{
		_toggleOnOff = targetOnOff;
	}

	public void SetIsBlinded(in bool targetIsBlinded)
	{
		_isBlinded = targetIsBlinded;
	}

	public void SetIsMine(in bool targetIsMine)
	{
		_isMine = targetIsMine;
	}

	public void SetIsShared(in bool targetIsShared)
	{
		_isShared = targetIsShared;
	}
}
