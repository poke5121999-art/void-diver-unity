using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
[ExecuteAlways]
public class UIMaterialController : UIBehaviour, IMaterialModifier
{
	public enum PropertyType
	{
		Float = 0,
		Color = 1,
		FourFloat = 2
	}

	[Header("Property 0")]
	public string propertyName0;

	public PropertyType propertyType0;

	public float floatValue0;

	public Color colorValue0;

	public float fourFloatValue0X;

	public float fourFloatValue0Y;

	public float fourFloatValue0Z;

	public float fourFloatValue0W;

	[Header("Property 1")]
	public string propertyName1;

	public PropertyType propertyType1;

	public float floatValue1;

	public Color colorValue1;

	public float fourFloatValue1X;

	public float fourFloatValue1Y;

	public float fourFloatValue1Z;

	public float fourFloatValue1W;

	[Header("Property 2")]
	public string propertyName2;

	public PropertyType propertyType2;

	public float floatValue2;

	public Color colorValue2;

	public float fourFloatValue2X;

	public float fourFloatValue2Y;

	public float fourFloatValue2Z;

	public float fourFloatValue2W;

	[Header("Property 3")]
	public string propertyName3;

	public PropertyType propertyType3;

	public float floatValue3;

	public Color colorValue3;

	public float fourFloatValue3X;

	public float fourFloatValue3Y;

	public float fourFloatValue3Z;

	public float fourFloatValue3W;

	[Header("Property 4")]
	public string propertyName4;

	public PropertyType propertyType4;

	public float floatValue4;

	public Color colorValue4;

	public float fourFloatValue4X;

	public float fourFloatValue4Y;

	public float fourFloatValue4Z;

	public float fourFloatValue4W;

	[Header("Property 5")]
	public string propertyName5;

	public PropertyType propertyType5;

	public float floatValue5;

	public Color colorValue5;

	public float fourFloatValue5X;

	public float fourFloatValue5Y;

	public float fourFloatValue5Z;

	public float fourFloatValue5W;

	[NonSerialized]
	private Graphic _graphic;

	[NonSerialized]
	protected Material material;

	public Graphic graphic => null;

	protected override void OnEnable()
	{
	}

	protected override void OnDisable()
	{
	}

	protected override void OnDidApplyAnimationProperties()
	{
	}

	private void SetDirty()
	{
	}

	private void CleanUpMaterial()
	{
	}

	public Material GetModifiedMaterial(Material baseMaterial)
	{
		return null;
	}

	public virtual void ApplyProperties()
	{
	}

	private void ApplyProperty(string propertyName, PropertyType type, float floatValue, Color colorValue, float fourFloatX, float fourFloatY, float fourFloatZ, float fourFloatW)
	{
	}
}
