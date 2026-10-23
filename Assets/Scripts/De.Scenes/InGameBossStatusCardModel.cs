using NemoLib.Mvp;
using UniRx;

public class InGameBossStatusCardModel : Model
{
	public MonsterModel MonsterModel;

	public readonly Subject<InGameBuffSlotModel> OnSlotEnter = new Subject<InGameBuffSlotModel>();

	public readonly Subject<InGameBuffSlotModel> OnSlotExit = new Subject<InGameBuffSlotModel>();
}
