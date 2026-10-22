using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyInteractionPanelView : View
{
	[SerializeField]
	private GameObject content;

	[SerializeField]
	private GameObject body;

	[SerializeField]
	private GameObject[] goodsSlots;

	[SerializeField]
	private GameObject confirm;

	[SerializeField]
	private Button confirm_Button;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private TextMeshProUGUI goodsCountText;

	public GameObject Content => null;

	public GameObject Body => null;

	public GameObject[] GoodsSlots => null;

	public GameObject Confirm => null;

	public Button Confirm_Button => null;

	public GameObject Top => null;

	public TextMeshProUGUI GoodsCountText => null;
}
