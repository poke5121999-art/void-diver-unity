using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using VContainer;

public class GameNpcDialogManager : MonoBase
{
	[Inject]
	private GameManager _gameManager;

	private readonly Dictionary<int, List<DialogInfo>> _dialogs = new Dictionary<int, List<DialogInfo>>();

	public Subject<int> OnDialogChanged { get; } = new Subject<int>();

	protected override void Awake()
	{
		base.Awake();
		Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		_gameManager.OnGameClosed.Subscribe(delegate
		{
			ClearAll();
		}).AddTo(this);
	}

	private void ClearAll()
	{
		_dialogs.Clear();
	}

	public List<DialogInfo> GetDialogInfos(int npcId)
	{
		// GetDialogInfos @0x1807c1ca0 (machine code checked): an NPC without dialogs gets an empty list, stored for its id,
		// so callers (NpcHudPresenter.UpdateDialogMarks, InGameScene.OnNpcInteractionAsync) never see null.
		List<DialogInfo> value;
		if (!_dialogs.TryGetValue(npcId, out value))
		{
			value = new List<DialogInfo>();
			_dialogs[npcId] = value;
		}
		return value;
	}

	public bool HasDialog(int npcId)
	{
		List<DialogInfo> value;
		return _dialogs.TryGetValue(npcId, out value) && value.Count > 0;
	}

	public void AddDialog(int npcId, DialogInfo info)
	{
		List<DialogInfo> value;
		if (!_dialogs.TryGetValue(npcId, out value))
		{
			value = new List<DialogInfo>();
			_dialogs[npcId] = value;
		}
		if (!value.Any((DialogInfo d) => d.Key == info.Key))
		{
			value.Add(info);
			OnDialogChanged.OnNext(npcId);
		}
	}

	public void RemoveDialog(int npcId, string key)
	{
		List<DialogInfo> value;
		if (_dialogs.TryGetValue(npcId, out value))
		{
			DialogInfo dialogInfo = value.FirstOrDefault((DialogInfo d) => d.Key == key);
			if (dialogInfo != null)
			{
				value.Remove(dialogInfo);
				OnDialogChanged.OnNext(npcId);
			}
		}
	}

	public void ClearDialogs(int npcId)
	{
		List<DialogInfo> value;
		if (_dialogs.TryGetValue(npcId, out value) && value.Count != 0)
		{
			value.Clear();
			OnDialogChanged.OnNext(npcId);
		}
	}
}
