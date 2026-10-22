using NemoLib.Popup;
using UniRx;
using UnityEngine;
using VContainer;

public class CampaignListBackgroundController : CampaignBackgroundController
{
	[Inject]
	private PopupManager _popupManager;

	private CompositeDisposable _disposable = new CompositeDisposable();

	[Header("Time Ranges: Entry List")]
	public float EntryStart;

	public float EntryEnd = 1.67f;

	[Header("Campaign Popup Prefab: Checking is First Entry")]
	[SerializeField]
	private BasePopupPresenter _cachedOwnerPopup;

	[Header("Time Ranges: Exit List")]
	public float ExitStart = 3.7f;

	public float ExitEnd = 3.8f;

	[Header("Time Ranges: Entry to Detail")]
	public float EntryDetailStart = 2.4f;

	public float EntryDetailEnd = 2.9f;

	[Header("Time Ranges: Exit from Detail")]
	public float ExitDetailStart = 3f;

	public float ExitDetailEnd = 3.6f;

	private bool _shouldPlayEntry;

	private bool _isInitialzied;

	protected override void Awake()
	{
		base.Awake();
		if (_cachedOwnerPopup != null)
		{
			_popupManager.OnBeforeOpenPopup.Where((BasePopupPresenter p) => p == _cachedOwnerPopup).Subscribe(delegate
			{
				_shouldPlayEntry = true;
			}).AddTo(_disposable);
		}
	}

	private void OnEnable()
	{
		if (!_isInitialzied)
		{
			PlayRange(EntryStart, EntryEnd);
			_isInitialzied = true;
		}
		else if (!_shouldPlayEntry)
		{
			PlayRange(ExitDetailStart, ExitDetailEnd);
		}
		else
		{
			PlayRange(EntryStart, EntryEnd);
			_shouldPlayEntry = false;
		}
	}

	[ContextMenu("PlayEntryMenu")]
	public void PlayEntryList()
	{
		PlayRange(EntryStart, EntryEnd);
	}

	[ContextMenu("PlayExitMenu")]
	public void PlayExitList()
	{
		PlayRange(ExitStart, ExitEnd);
	}

	[ContextMenu("PlayEntryToDetail")]
	public void PlayEntryToDetail()
	{
		PlayRange(EntryDetailStart, EntryDetailEnd);
	}

	[ContextMenu("PlayExitFromDetail")]
	public void PlayExitFromDetail()
	{
		PlayRange(ExitDetailStart, ExitDetailEnd);
	}
}
