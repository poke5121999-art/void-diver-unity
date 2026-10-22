using GameCommon.Data;

public class GoodsTooltipModel : BasePanelModel
{
	public bool IsArtifactShop;

	public bool IsArtifactDeal;

	public bool ShowKeyPrompt;

	public bool IsCraftingAvailable;

	public bool HideEffectGroup;

	public Goods Goods { get; set; }
}
