using NemoLib.Mvp;
using UnityEngine;

public class SquadPageView : View
{
	[SerializeField]
	private SquadPageJoinedPanelPresenter joinedPanel;

	[SerializeField]
	private SquadPageNotJoinedPanelPresenter notJoinedPanel;

	public SquadPageJoinedPanelPresenter JoinedPanel => null;

	public SquadPageNotJoinedPanelPresenter NotJoinedPanel => null;
}
