using GameCommon.Enum;
using NemoLib.Mvp;

public class KeyInteractionPopupModel : Model
{
	public EKeyItemType[] KeyItemTypes;

	public int[] KeyItemIds;

	public IInteractiveObject InteractiveObject;
}
