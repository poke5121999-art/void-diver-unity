using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Character Visual Effect Data", menuName = "LCArt/Character Visual Effect Data")]
public class UnitVisualStatusEffectScriptable : ScriptableObject
{
	[FormerlySerializedAs("_characterStatusVisualInfos")]
	[SerializeField]
	private List<UnitVisualStatusEffectInfo> _unitStatusVisualInfos = new List<UnitVisualStatusEffectInfo>();

	public bool GetIsValidKeyword(string keyword)
	{
		foreach (UnitVisualStatusEffectInfo unitStatusVisualInfo in _unitStatusVisualInfos)
		{
			if (unitStatusVisualInfo.Keyword.Equals(keyword))
			{
				return true;
			}
		}
		return false;
	}

	public UnitVisualStatusEffectInfo GetVisualInfoByKeyword(string keyword)
	{
		foreach (UnitVisualStatusEffectInfo unitStatusVisualInfo in _unitStatusVisualInfos)
		{
			if (unitStatusVisualInfo.Keyword.Equals(keyword))
			{
				return unitStatusVisualInfo;
			}
		}
		return null;
	}
}
