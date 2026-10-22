using System;
using UnityEngine;

[Serializable]
public class UnitVisualStatusEffectInfo
{
	[SerializeField]
	private string _keyword;

	[SerializeField]
	private Color _innerColor;

	[SerializeField]
	private Color _outerColor;

	[SerializeField]
	[Range(0f, 10f)]
	private float _intensity;

	public string Keyword => null;

	public Color InnerColor => default;

	public Color OuterColor => default;

	public float Intensity => 0f;
}
