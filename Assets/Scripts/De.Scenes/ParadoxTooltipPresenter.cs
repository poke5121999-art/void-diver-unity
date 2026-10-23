using GameCommon.Table;
using NemoLib.Mvp;
using UnityEngine;

public class ParadoxTooltipPresenter : BasePanelPresenter
{
	[InjectModel]
	private ParadoxTooltipModel _model;

	[View]
	private ParadoxTooltipView _view;

	protected override BasePanelModel PanelModel => _model;

	private void OnInject(ParadoxTooltipModel model)
	{
		TParadox tParadox = TParadox.Table[_model.ParadoxId];
		bool isPositive = tParadox.IsPositive;
		_view.Paradox_Name.text = tParadox.Name;
		_view.Paradox_Level.text = tParadox.GetParadoxLevelText();
		_view.Paradox_LockIcon.gameObject.SetActive(model.IsLocked);
		_view.Desc.text = tParadox.Desc;
		Color color = UiUtility.HexToColor(isPositive ? "#58744C" : "#954E4E");
		_view.Paradox_Level.color = color;
		_view.Paradox_Icon.color = color;
		_view.Paradox_Icon.transform.localScale = (isPositive ? new Vector3(1f, -1f, 1f) : Vector3.one);
	}
}
