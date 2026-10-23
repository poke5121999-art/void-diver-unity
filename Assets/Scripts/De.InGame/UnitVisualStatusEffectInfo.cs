using System;
using UnityEngine;

[Serializable]
public class UnitVisualStatusEffectInfo
{
	[SerializeField]
	private string _keyword;

	[SerializeField]
	private Color _innerColor = new Color(0f, 0f, 0f, 1f);

	[SerializeField]
	private Color _outerColor = new Color(1f, 1f, 1f, 1f);

	[SerializeField]
	[Range(0f, 10f)]
	private float _intensity = 1f;

	public string Keyword => _keyword;

	public Color InnerColor => _innerColor;

	public Color OuterColor => _outerColor;

	public float Intensity => _intensity;
}
