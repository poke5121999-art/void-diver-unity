using GameCommon.Enum;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using VContainer;

public class MonsterWeakPointGaugePresenter : Presenter
{
	[InjectModel]
	private MonsterWeakPointGaugeModel _model;

	[View]
	private MonsterWeakPointGaugeView _view;

	[Inject]
	private AssetManager _assetManager;

	private MonsterModel _monsterModel => null;

	protected void OnInject(MonsterWeakPointGaugeModel model)
	{
	}

	private void OnWeakPointTypeChanged(EElementalType type)
	{
	}

	private void OnStateChanged(EActionState state)
	{
	}

	private void OnWeakPointStackDamageChanged(float stackDamage)
	{
	}
}
