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
	public string propertyName0 = "_Property";

	public PropertyType propertyType0;

	public float floatValue0;

	public Color colorValue0 = Color.white;

	public float fourFloatValue0X;

	public float fourFloatValue0Y;

	public float fourFloatValue0Z;

	public float fourFloatValue0W;

	[Header("Property 1")]
	public string propertyName1 = string.Empty;

	public PropertyType propertyType1;

	public float floatValue1;

	public Color colorValue1 = Color.white;

	public float fourFloatValue1X;

	public float fourFloatValue1Y;

	public float fourFloatValue1Z;

	public float fourFloatValue1W;

	[Header("Property 2")]
	public string propertyName2 = string.Empty;

	public PropertyType propertyType2;

	public float floatValue2;

	public Color colorValue2 = Color.white;

	public float fourFloatValue2X;

	public float fourFloatValue2Y;

	public float fourFloatValue2Z;

	public float fourFloatValue2W;

	[Header("Property 3")]
	public string propertyName3 = string.Empty;

	public PropertyType propertyType3;

	public float floatValue3;

	public Color colorValue3 = Color.white;

	public float fourFloatValue3X;

	public float fourFloatValue3Y;

	public float fourFloatValue3Z;

	public float fourFloatValue3W;

	[Header("Property 4")]
	public string propertyName4 = string.Empty;

	public PropertyType propertyType4;

	public float floatValue4;

	public Color colorValue4 = Color.white;

	public float fourFloatValue4X;

	public float fourFloatValue4Y;

	public float fourFloatValue4Z;

	public float fourFloatValue4W;

	[Header("Property 5")]
	public string propertyName5 = string.Empty;

	public PropertyType propertyType5;

	public float floatValue5;

	public Color colorValue5 = Color.white;

	public float fourFloatValue5X;

	public float fourFloatValue5Y;

	public float fourFloatValue5Z;

	public float fourFloatValue5W;

	[NonSerialized]
	private Graphic _graphic;

	[NonSerialized]
	protected Material material;

	public Graphic graphic
	{
		get
		{
			if (!_graphic)
			{
				_graphic = GetComponent<Graphic>();
			}
			return _graphic;
		}
	}

	protected override void OnEnable()
	{
		SetDirty();
	}

	protected override void OnDisable()
	{
		CleanUpMaterial();
	}

	protected override void OnDidApplyAnimationProperties()
	{
		SetDirty();
	}

	private void SetDirty()
	{
		if (IsActive() && graphic != null)
		{
			graphic.SetMaterialDirty();
		}
	}

	private void CleanUpMaterial()
	{
		if (material != null)
		{
			if (Application.isPlaying)
			{
				UnityEngine.Object.Destroy(material);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(material);
			}
		}
		material = null;
		if (IsActive() && graphic != null)
		{
			graphic.SetMaterialDirty();
		}
	}

	public Material GetModifiedMaterial(Material baseMaterial)
	{
		if (!IsActive())
		{
			return baseMaterial;
		}
		if (graphic == null)
		{
			return baseMaterial;
		}
		if (baseMaterial == null)
		{
			return baseMaterial;
		}
		if (!(material == null))
		{
			if (!(material.shader != baseMaterial.shader))
			{
				ApplyProperties();
				return material;
			}
		}
		material = new Material(baseMaterial);
		material.hideFlags = HideFlags.HideAndDontSave;
		material.CopyPropertiesFromMaterial(baseMaterial);
		ApplyProperties();
		return material;
	}

	public virtual void ApplyProperties()
	{
		if (!(material == null))
		{
			ApplyProperty(propertyName0, propertyType0, floatValue0, colorValue0, fourFloatValue0X, fourFloatValue0Y, fourFloatValue0Z, fourFloatValue0W);
			ApplyProperty(propertyName1, propertyType1, floatValue1, colorValue1, fourFloatValue1X, fourFloatValue1Y, fourFloatValue1Z, fourFloatValue1W);
			ApplyProperty(propertyName2, propertyType2, floatValue2, colorValue2, fourFloatValue2X, fourFloatValue2Y, fourFloatValue2Z, fourFloatValue2W);
			ApplyProperty(propertyName3, propertyType3, floatValue3, colorValue3, fourFloatValue3X, fourFloatValue3Y, fourFloatValue3Z, fourFloatValue3W);
			ApplyProperty(propertyName4, propertyType4, floatValue4, colorValue4, fourFloatValue4X, fourFloatValue4Y, fourFloatValue4Z, fourFloatValue4W);
			ApplyProperty(propertyName5, propertyType5, floatValue5, colorValue5, fourFloatValue5X, fourFloatValue5Y, fourFloatValue5Z, fourFloatValue5W);
		}
	}

	private void ApplyProperty(string propertyName, PropertyType type, float floatValue, Color colorValue, float fourFloatX, float fourFloatY, float fourFloatZ, float fourFloatW)
	{
		if (string.IsNullOrEmpty(propertyName) || !material.HasProperty(propertyName))
		{
			return;
		}
		switch (type)
		{
		case PropertyType.Float:
			material.SetFloat(propertyName, floatValue);
			break;
		case PropertyType.Color:
			material.SetColor(propertyName, colorValue);
			break;
		case PropertyType.FourFloat:
			material.SetVector(propertyName, new Vector4(fourFloatX, fourFloatY, fourFloatZ, fourFloatW));
			break;
		}
	}
}
