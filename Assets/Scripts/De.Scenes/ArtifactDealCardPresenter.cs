using NemoLib.Mvp;
using TMPro;
using UnityEngine;
using VContainer;

public class ArtifactDealCardPresenter : BaseSlotPresenter<ArtifactDealCardModel>
{
	[InjectModel]
	private ArtifactDealCardModel _model;

	[View]
	private ArtifactDealCardView _view;

	[Inject]
	private ArtifactDealManager _artifactDealManager;

	[SerializeField]
	private TextMeshProUGUI _textPercent;

	[SerializeField]
	private TextMeshProUGUI _textTitle;

	private bool _isInteractable;

	protected override ArtifactDealCardModel SlotModel => null;

	protected override void Start()
	{
	}

	private void OnInject(ArtifactDealCardModel model)
	{
	}

	protected override void OnSlotEnter()
	{
	}

	protected override void OnSlotExit()
	{
	}

	public void SetInteractable(bool interactable)
	{
	}

	private void UpdateLocked()
	{
	}

	private void UpdateSuccessPercent()
	{
	}
}
