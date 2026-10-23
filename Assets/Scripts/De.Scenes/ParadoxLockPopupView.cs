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

	public Button Background => background;

	public GameObject Contents => contents;

	public TextMeshProUGUI Empty => empty;

	public GameObject Info => info;

	public ParadoxSlotPresenter LockedSlot => lockedSlot;

	public GameObject[] Slots => slots;

	public GameObject Normal => normal;
}
