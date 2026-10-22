using NemoLib.Mvp;
using UnityEngine;

public class VoteStateView : View
{
	[SerializeField]
	private GameObject no;

	[SerializeField]
	private GameObject waiting;

	[SerializeField]
	private GameObject yes;

	public GameObject No => null;

	public GameObject Waiting => null;

	public GameObject Yes => null;
}
