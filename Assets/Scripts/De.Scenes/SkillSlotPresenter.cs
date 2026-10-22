using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UniRx;
using VContainer;

public class SkillSlotPresenter : BaseSlotPresenter<SkillSlotModel>
{
	[View]
	private SkillSlotView _view;

	[InjectModel]
	private SkillSlotModel _model;

	[Inject]
	private AssetManager _assetManager;

	protected override SkillSlotModel SlotModel => _model;

	protected override void Start()
	{
		if (Button != null)
		{
			base.Start();
		}
	}

	protected override void OnSelected(bool isSelected)
	{
		if (Button != null)
		{
			base.OnSelected(isSelected);
		}
	}

	protected override void OnLocked(bool isLocked)
	{
		if (Button != null)
		{
			base.OnLocked(isLocked);
		}
	}

	private void OnInject(SkillSlotModel model)
	{
		InitSlot(model);
		_view.IconLock.gameObject.SetActive(model.ShowLockIcon);
		_view.Icon.gameObject.SetActive(!model.IsLocked);
		model.RxSkillId.Subscribe(OnSkillChanged).AddTo(Disposable);
	}

	private void OnSkillChanged(int skillId)
	{
		_view.Icon.gameObject.SetActive(skillId > 0);
		if (skillId > 0)
		{
			_view.Icon.sprite = _assetManager.GetIconSkill(skillId);
		}
	}
}
