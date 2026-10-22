using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomCodeDisplayPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject address;

	[SerializeField]
	private TextMeshProUGUI codeNum;

	[SerializeField]
	private Button copyButton;

	[SerializeField]
	private Image line;

	[SerializeField]
	private Image lockIcon;

	[SerializeField]
	private Button onOffButton;

	[SerializeField]
	private Image onOffButton_Normal;

	[SerializeField]
	private Image onOffButton_Off;

	[SerializeField]
	private Image onOffButton_On;

	[SerializeField]
	private GameObject bottom;

	[SerializeField]
	private GameObject confirm;

	[SerializeField]
	private Button confirm_Button;

	public Button Background => null;

	public GameObject Contents => null;

	public GameObject Body => null;

	public GameObject Address => null;

	public TextMeshProUGUI CodeNum => null;

	public Button CopyButton => null;

	public Image Line => null;

	public Image LockIcon => null;

	public Button OnOffButton => null;

	public Image OnOffButton_Normal => null;

	public Image OnOffButton_Off => null;

	public Image OnOffButton_On => null;

	public GameObject Bottom => null;

	public GameObject Confirm => null;

	public Button Confirm_Button => null;
}
