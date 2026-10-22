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

	public float FrontRange => 0f;

	public float BackRange => 0f;

	public float ViewAngle => 0f;

	public bool ToggleOnOff => false;

	public bool IsBlinded => false;

	public bool IsMine => false;

	public bool IsShared => false;

	public Transform SightTransform => null;

	public SightInfo()
	{
	}

	public SightInfo(in float frontRange, in float backRange, in float viewAngle)
	{
	}

	public SightInfo(in Transform characterTransform, in float frontRange, in float backRange, in float viewAngle)
	{
	}

	public void SetSightInfo(in float frontRange, in float backRange, in float viewAngle)
	{
	}

	public void SetTransform(Transform inTransform)
	{
	}

	public void SetOnOffToggle(in bool targetOnOff)
	{
	}

	public void SetIsBlinded(in bool targetIsBlinded)
	{
	}

	public void SetIsMine(in bool targetIsMine)
	{
	}

	public void SetIsShared(in bool targetIsShared)
	{
	}
}
