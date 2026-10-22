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

	public Button Background => background;

	public GameObject Contents => contents;

	public GameObject Info => info;

	public GameObject Left => left;

	public GameObject Scroller => scroller;

	public GameObject Right => right;

	public GameObject Empty => empty;

	public MonsterInfoPanelPresenter MonsterInfoPanel => monsterInfoPanel;

	public GameObject Top => top;

	public TextMeshProUGUI Title => title;

	public TextMeshProUGUI TitleSub => titleSub;
}
