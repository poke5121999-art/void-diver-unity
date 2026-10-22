using EnhancedUI.EnhancedScroller;
using NemoLib.Mvp;

public class BaseCell : EnhancedScrollerCellView
{
	private Presenter _presenter;

	private Model _currentModel;

	public void SetData(Model data)
	{
		(_currentModel as IClearSelectable)?.ClearSelectable(transform);
		_currentModel = data;
		if (_presenter == null)
		{
			_presenter = GetComponent<Presenter>();
		}
		_presenter.Inject(data);
	}
}
