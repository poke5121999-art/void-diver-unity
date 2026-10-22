using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeySettingSlotView : View
{
	[SerializeField]
	private GameObject normal;

	[SerializeField]
	private GameObject key;

	[SerializeField]
	private GameObject binding;

	[SerializeField]
	private GameObject binding_Normal;

	[SerializeField]
	private GameObject binding_KeyPrompt;

	[SerializeField]
	private GameObject modifier;

	[SerializeField]
	private GameObject modifier_Normal;

	[SerializeField]
	private GameObject modifier_KeyPrompt;

	[SerializeField]
	private GameObject plus;

	[SerializeField]
	private Image locked;

	[SerializeField]
	private TextMeshProUGUI text;

	public GameObject Normal => null;

	public GameObject Key => null;

	public GameObject Binding => null;

	public GameObject Binding_Normal => null;

	public GameObject Binding_KeyPrompt => null;

	public GameObject Modifier => null;

	public GameObject Modifier_Normal => null;

	public GameObject Modifier_KeyPrompt => null;

	public GameObject Plus => null;

	public Image Locked => null;

	public TextMeshProUGUI Text => null;
}
