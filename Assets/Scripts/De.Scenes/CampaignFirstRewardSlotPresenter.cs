using NemoLib.Mvp;
using NemoLib.Popup;
using UniRx;
using UnityEngine.UI;
using VContainer;

public class CampaignFirstRewardSlotPresenter : Presenter
{
	[View]
	private CampaignFirstRewardSlotView _view;

	[InjectModel]
	private CampaignFirstRewardSlotModel _model;

	[Inject]
	private PopupManager _popupManager;

	private CompositeDisposable _disposable;

	public Selectable Selectable => null;

	private void OnInject(CampaignFirstRewardSlotModel model)
	{
	}

	private void OnDestroy()
	{
	}
}
