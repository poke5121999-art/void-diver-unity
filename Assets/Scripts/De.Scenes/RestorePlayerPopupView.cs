using NemoLib.Mvp;
using UnityEngine;

public class RestorePlayerPopupView : View
{
	[SerializeField]
	private GameObject contents;

	[SerializeField]
	private GameObject left;

	[SerializeField]
	private PlayerSaveSlotScrollerPresenter saveSlotScroller;

	[SerializeField]
	private GameObject right;

	[SerializeField]
	private PlayerRestoreSlotScrollerPresenter restoreSlotScroller;

	public GameObject Contents => null;

	public GameObject Left => null;

	public PlayerSaveSlotScrollerPresenter SaveSlotScroller => null;

	public GameObject Right => null;

	public PlayerRestoreSlotScrollerPresenter RestoreSlotScroller => null;
}
