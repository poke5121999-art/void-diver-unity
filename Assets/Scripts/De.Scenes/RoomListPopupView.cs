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

	public Button Background => null;

	public GameObject Window => null;

	public GameObject Buttons => null;

	public Button CancelButton => null;

	public Button EnterButton => null;

	public Button EnterCodeButton => null;

	public GameObject ListTitle => null;

	public Image ScrollbarVertical => null;

	public GameObject ScrollList => null;

	public GameObject Top => null;

	public Button FilterButton => null;

	public Button RefreshButton => null;

	public GameObject TitleGroup => null;
}
