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

	public GameObject Root => root;

	public GameObject Back => back;

	public Image Cover => cover;

	public GameObject Front => front;

	public Button ArtifactDealCard => artifactDealCard;

	public Image Complete => complete;

	public GameObject PressedFX => pressedFX;

	public GameObject PressedFX_Tier1 => pressedFX_Tier1;

	public GameObject PressedFX_Tier2 => pressedFX_Tier2;

	public GameObject PressedFX_Tier3 => pressedFX_Tier3;

	public GameObject PressedFX_Tier4 => pressedFX_Tier4;

	public GameObject PressedFX_Tier5 => pressedFX_Tier5;
}
