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

	public GameObject Empty => empty;

	public Image Empty_ImageFace => empty_ImageFace;

	public GameObject Profile => profile;

	public Image Profile_ImageFace => profile_ImageFace;

	public GameObject Seleted => seleted;
}
