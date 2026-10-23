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

	public GameObject Content => content;

	public GameObject Body => body;

	public GameObject[] GoodsSlots => goodsSlots;

	public GameObject Confirm => confirm;

	public Button Confirm_Button => confirm_Button;

	public GameObject Top => top;

	public TextMeshProUGUI GoodsCountText => goodsCountText;
}
