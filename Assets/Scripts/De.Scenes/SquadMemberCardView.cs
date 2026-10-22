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

	public EmptyPanelPresenter EmptyPanel => null;

	public GameObject ParadoxLayout => null;

	public ParadoxPanelPresenter NegativeParadoxPanel => null;

	public ParadoxPanelPresenter PositiveParadoxPanel => null;

	public GameObject PlayerLayout => null;

	public Image Face => null;

	public Image Face_Image => null;

	public TextMeshProUGUI Level => null;

	public MenuSkillPanelPresenter MenuSkillPanel => null;

	public TextMeshProUGUI Name => null;

	public Button SteamButton => null;

	public GameObject SteamButton_Normal => null;

	public TextMeshProUGUI SteamButton_TextUserId => null;

	public GameObject Top => null;

	public Image HostIcon => null;
}
