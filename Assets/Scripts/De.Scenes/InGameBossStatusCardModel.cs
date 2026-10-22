using NemoLib.Mvp;
using UniRx;

public class InGameBossStatusCardModel : Model
{
	public MonsterModel MonsterModel;

	public readonly Subject<InGameBuffSlotModel> OnSlotEnter;

	public readonly Subject<InGameBuffSlotModel> OnSlotExit;
}
