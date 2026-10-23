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

	public Button Background => background;

	public GameObject Contents => contents;

	public GameObject Body => body;

	public GameObject Address => address;

	public TextMeshProUGUI CodeNum => codeNum;

	public Button CopyButton => copyButton;

	public Image Line => line;

	public Image LockIcon => lockIcon;

	public Button OnOffButton => onOffButton;

	public Image OnOffButton_Normal => onOffButton_Normal;

	public Image OnOffButton_Off => onOffButton_Off;

	public Image OnOffButton_On => onOffButton_On;

	public GameObject Bottom => bottom;

	public GameObject Confirm => confirm;

	public Button Confirm_Button => confirm_Button;
}
