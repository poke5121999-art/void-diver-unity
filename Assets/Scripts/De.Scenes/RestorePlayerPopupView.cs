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

	public GameObject Contents => contents;

	public GameObject Left => left;

	public PlayerSaveSlotScrollerPresenter SaveSlotScroller => saveSlotScroller;

	public GameObject Right => right;

	public PlayerRestoreSlotScrollerPresenter RestoreSlotScroller => restoreSlotScroller;
}
