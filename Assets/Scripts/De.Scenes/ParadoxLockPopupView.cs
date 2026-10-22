using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ParadoxLockPopupView : View
{
	[SerializeField]
	private Button background;

	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private TextMeshProUGUI empty;

	[SerializeField]
	private GameObject info;

	[SerializeField]
	private ParadoxSlotPresenter lockedSlot;

	[SerializeField]
	private GameObject[] slots;

	[SerializeField]
	private GameObject normal;

	public Button Background => null;

	public GameObject Contents => null;

	public TextMeshProUGUI Empty => null;

	public GameObject Info => null;

	public ParadoxSlotPresenter LockedSlot => null;

	public GameObject[] Slots => null;

	public GameObject Normal => null;
}
