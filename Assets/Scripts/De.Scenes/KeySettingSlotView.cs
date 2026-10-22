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

	public GameObject Normal => normal;

	public GameObject Key => key;

	public GameObject Binding => binding;

	public GameObject Binding_Normal => binding_Normal;

	public GameObject Binding_KeyPrompt => binding_KeyPrompt;

	public GameObject Modifier => modifier;

	public GameObject Modifier_Normal => modifier_Normal;

	public GameObject Modifier_KeyPrompt => modifier_KeyPrompt;

	public GameObject Plus => plus;

	public Image Locked => locked;

	public TextMeshProUGUI Text => text;
}
