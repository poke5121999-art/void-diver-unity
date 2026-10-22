using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArchiveMonsterPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject info;

	[SerializeField]
	private GameObject left;

	[SerializeField]
	private GameObject scroller;

	[SerializeField]
	private GameObject right;

	[SerializeField]
	private GameObject empty;

	[SerializeField]
	private MonsterInfoPanelPresenter monsterInfoPanel;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private TextMeshProUGUI title;

	[SerializeField]
	private TextMeshProUGUI titleSub;

	public Button Background => null;

	public GameObject Contents => null;

	public GameObject Info => null;

	public GameObject Left => null;

	public GameObject Scroller => null;

	public GameObject Right => null;

	public GameObject Empty => null;

	public MonsterInfoPanelPresenter MonsterInfoPanel => null;

	public GameObject Top => null;

	public TextMeshProUGUI Title => null;

	public TextMeshProUGUI TitleSub => null;
}
