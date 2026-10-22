using System.Collections.Generic;
using UniRx;
using VContainer;

public class GameNpcDialogManager : MonoBase
{
	[Inject]
	private GameManager _gameManager;

	private readonly Dictionary<int, List<DialogInfo>> _dialogs;

	public Subject<int> OnDialogChanged { get; }

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	private void ClearAll()
	{
	}

	public List<DialogInfo> GetDialogInfos(int npcId)
	{
		return null;
	}

	public bool HasDialog(int npcId)
	{
		return false;
	}

	public void AddDialog(int npcId, DialogInfo info)
	{
	}

	public void RemoveDialog(int npcId, string key)
	{
	}

	public void ClearDialogs(int npcId)
	{
	}
}
