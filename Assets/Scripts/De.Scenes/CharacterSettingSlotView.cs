using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSettingSlotView : View
{
	[SerializeField]
	private GameObject empty;

	[SerializeField]
	private Image empty_ImageFace;

	[SerializeField]
	private GameObject profile;

	[SerializeField]
	private Image profile_ImageFace;

	[SerializeField]
	private GameObject seleted;

	public GameObject Empty => null;

	public Image Empty_ImageFace => null;

	public GameObject Profile => null;

	public Image Profile_ImageFace => null;

	public GameObject Seleted => null;
}
