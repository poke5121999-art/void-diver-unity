using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;

public class RoomListPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject window;

	[SerializeField]
	private GameObject buttons;

	[SerializeField]
	private Button cancelButton;

	[SerializeField]
	private Button enterButton;

	[SerializeField]
	private Button enterCodeButton;

	[SerializeField]
	private GameObject listTitle;

	[SerializeField]
	private Image scrollbarVertical;

	[SerializeField]
	private GameObject scrollList;

	[SerializeField]
	private GameObject top;

	[SerializeField]
	private Button filterButton;

	[SerializeField]
	private Button refreshButton;

	[SerializeField]
	private GameObject titleGroup;

	public Button Background => background;

	public GameObject Window => window;

	public GameObject Buttons => buttons;

	public Button CancelButton => cancelButton;

	public Button EnterButton => enterButton;

	public Button EnterCodeButton => enterCodeButton;

	public GameObject ListTitle => listTitle;

	public Image ScrollbarVertical => scrollbarVertical;

	public GameObject ScrollList => scrollList;

	public GameObject Top => top;

	public Button FilterButton => filterButton;

	public Button RefreshButton => refreshButton;

	public GameObject TitleGroup => titleGroup;
}
