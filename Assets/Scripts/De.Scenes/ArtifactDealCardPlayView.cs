using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactDealCardPlayView : View
{
	[SerializeField]
	private GameObject root;

	[SerializeField]
	private GameObject back;

	[SerializeField]
	private Image cover;

	[SerializeField]
	private GameObject front;

	[SerializeField]
	private Button artifactDealCard;

	[SerializeField]
	private Image complete;

	[SerializeField]
	private GameObject pressedFX;

	[SerializeField]
	private GameObject pressedFX_Tier1;

	[SerializeField]
	private GameObject pressedFX_Tier2;

	[SerializeField]
	private GameObject pressedFX_Tier3;

	[SerializeField]
	private GameObject pressedFX_Tier4;

	[SerializeField]
	private GameObject pressedFX_Tier5;

	public GameObject Root => null;

	public GameObject Back => null;

	public Image Cover => null;

	public GameObject Front => null;

	public Button ArtifactDealCard => null;

	public Image Complete => null;

	public GameObject PressedFX => null;

	public GameObject PressedFX_Tier1 => null;

	public GameObject PressedFX_Tier2 => null;

	public GameObject PressedFX_Tier3 => null;

	public GameObject PressedFX_Tier4 => null;

	public GameObject PressedFX_Tier5 => null;
}
