using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SquadMemberCardView : View
{
	[SerializeField]
	private EmptyPanelPresenter emptyPanel;

	[SerializeField]
	private GameObject paradoxLayout;

	[SerializeField]
	private ParadoxPanelPresenter negativeParadoxPanel;

	[SerializeField]
	private ParadoxPanelPresenter positiveParadoxPanel;

	[SerializeField]
	private GameObject playerLayout;

	[SerializeField]
	private Image face;

	[SerializeField]
	private Image face_Image;

	[SerializeField]
	private TextMeshProUGUI level;

	[SerializeField]
	private MenuSkillPanelPresenter menuSkillPanel;

	[SerializeField]
	private new TextMeshProUGUI name;

	[SerializeField]
	private Button steamButton;

	[SerializeField]
	private GameObject steamButton_Normal;

	[SerializeField]
	private TextMeshProUGUI steamButton_TextUserId;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private Image hostIcon;

	public EmptyPanelPresenter EmptyPanel => emptyPanel;

	public GameObject ParadoxLayout => paradoxLayout;

	public ParadoxPanelPresenter NegativeParadoxPanel => negativeParadoxPanel;

	public ParadoxPanelPresenter PositiveParadoxPanel => positiveParadoxPanel;

	public GameObject PlayerLayout => playerLayout;

	public Image Face => face;

	public Image Face_Image => face_Image;

	public TextMeshProUGUI Level => level;

	public MenuSkillPanelPresenter MenuSkillPanel => menuSkillPanel;

	public TextMeshProUGUI Name => name;

	public Button SteamButton => steamButton;

	public GameObject SteamButton_Normal => steamButton_Normal;

	public TextMeshProUGUI SteamButton_TextUserId => steamButton_TextUserId;

	public GameObject Top => top;

	public Image HostIcon => hostIcon;
}
