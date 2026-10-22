using NemoLib.Popup;
using UniRx;
using UnityEngine;
using VContainer;

public class CampaignListBackgroundController : CampaignBackgroundController
{
	[Inject]
	private PopupManager _popupManager;

	private CompositeDisposable _disposable;

	[Header("Time Ranges: Entry List")]
	public float EntryStart;

	public float EntryEnd;

	[Header("Campaign Popup Prefab: Checking is First Entry")]
	[SerializeField]
	private BasePopupPresenter _cachedOwnerPopup;

	[Header("Time Ranges: Exit List")]
	public float ExitStart;

	public float ExitEnd;

	[Header("Time Ranges: Entry to Detail")]
	public float EntryDetailStart;

	public float EntryDetailEnd;

	[Header("Time Ranges: Exit from Detail")]
	public float ExitDetailStart;

	public float ExitDetailEnd;

	private bool _shouldPlayEntry;

	private bool _isInitialzied;

	protected override void Awake()
	{
	}

	private void OnEnable()
	{
	}

	[ContextMenu("PlayEntryMenu")]
	public void PlayEntryList()
	{
	}

	[ContextMenu("PlayExitMenu")]
	public void PlayExitList()
	{
	}

	[ContextMenu("PlayEntryToDetail")]
	public void PlayEntryToDetail()
	{
	}

	[ContextMenu("PlayExitFromDetail")]
	public void PlayExitFromDetail()
	{
	}
}
