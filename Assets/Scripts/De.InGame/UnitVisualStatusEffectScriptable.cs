using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Character Visual Effect Data", menuName = "LCArt/Character Visual Effect Data")]
public class UnitVisualStatusEffectScriptable : ScriptableObject
{
	[FormerlySerializedAs("_characterStatusVisualInfos")]
	[SerializeField]
	private List<UnitVisualStatusEffectInfo> _unitStatusVisualInfos;

	public bool GetIsValidKeyword(string keyword)
	{
		return false;
	}

	public UnitVisualStatusEffectInfo GetVisualInfoByKeyword(string keyword)
	{
		return null;
	}
}
