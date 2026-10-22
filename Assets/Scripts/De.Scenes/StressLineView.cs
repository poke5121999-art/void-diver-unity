using NemoLib.Mvp;
using TMPro;
using UnityEngine;

public class StressLineView : View
{
	[SerializeField]
	private GameObject stress;

	[SerializeField]
	private TextMeshProUGUI stress_Max;

	[SerializeField]
	private TextMeshProUGUI stress_Value;

	public GameObject Stress => null;

	public TextMeshProUGUI Stress_Max => null;

	public TextMeshProUGUI Stress_Value => null;
}
