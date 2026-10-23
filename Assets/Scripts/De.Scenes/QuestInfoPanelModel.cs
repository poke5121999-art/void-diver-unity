using GameCommon.Enum;
using UniRx;

public class QuestInfoPanelModel : BasePanelModel
{
	public EQuestType QuestType;

	public int TableId;

	public bool DisplayFullDialog;

	public readonly Subject<Unit> OnBack = new Subject<Unit>();
}
