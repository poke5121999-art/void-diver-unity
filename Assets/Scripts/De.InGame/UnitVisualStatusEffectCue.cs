using UnityEngine;

public class UnitVisualStatusEffectCue : MonoBehaviour
{
	[SerializeField]
	private string _keyword = "None";

	public string Keyword => _keyword;
}
