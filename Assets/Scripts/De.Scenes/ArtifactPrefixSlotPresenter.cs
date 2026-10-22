using GameCommon.Table;
using NemoLib.Mvp;

public class ArtifactPrefixSlotPresenter : Presenter
{
	[View]
	private ArtifactPrefixSlotView _view;

	[InjectModel]
	private ArtifactPrefixSlotModel _model;

	private void OnInject(Model model)
	{
		base.gameObject.SetActive(true);
		TArtifactPrefix tArtifactPrefix = TArtifactPrefix.Table[_model.PrefixId];
		bool flag = tArtifactPrefix.PriceMultiplier >= 0f;
		_view.Positive.SetActive(flag);
		_view.Negative.SetActive(!flag);
		if (flag)
		{
			_view.Positive_Text.text = tArtifactPrefix.Name;
			_view.Positive_DefaultLine.gameObject.SetActive(!_model.IsMatching);
			_view.Positive_ArtifactDealBg.gameObject.SetActive(_model.IsMatching);
			_view.Positive_ArtifactDealLine.gameObject.SetActive(_model.IsMatching);
		}
		else
		{
			_view.Negative_Text.text = tArtifactPrefix.Name;
			_view.Negative_DefaultLine.gameObject.SetActive(!_model.IsMatching);
			_view.Negative_ArtifactDealBg.gameObject.SetActive(_model.IsMatching);
			_view.Negative_ArtifactDealLine.gameObject.SetActive(_model.IsMatching);
		}
	}
}
