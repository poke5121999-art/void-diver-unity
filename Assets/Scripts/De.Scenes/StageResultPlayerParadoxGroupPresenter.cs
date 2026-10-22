using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using NemoLib.AssetManagement;
using NemoLib.Mvp;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class StageResultPlayerParadoxGroupPresenter : Presenter
{
	[View]
	private StageResultPlayerParadoxGroupView _view;

	[InjectModel]
	private StageResultPlayerParadoxGroupModel _model;

	[Inject]
	private AssetManager _assetManager;

	private const float BODY_WIDTH = 680f;

	private const float SLOT_SPACE = 10f;

	private readonly List<StageResultParadoxSlotPresenter> _cachedSlots = new List<StageResultParadoxSlotPresenter>();

	protected void OnInject(StageResultPlayerParadoxGroupModel model)
	{
		if (_view.UserName != null)
		{
			_view.UserName.text = model.UserName;
		}
		DeactivateAllSlots();
		List<(int, bool)> list = new List<(int, bool)>();
		if (model.GainedParadoxIds != null)
		{
			foreach (int id in model.GainedParadoxIds)
			{
				list.Add((id, false));
			}
		}
		if (model.RemovedParadoxIds != null)
		{
			foreach (int id2 in model.RemovedParadoxIds)
			{
				list.Add((id2, true));
			}
		}
		if (_view.Lines != null && _view.Lines.Length != 0)
		{
			if (list.Count != 0)
			{
				for (int i = 0; i < list.Count; i++)
				{
					StageResultParadoxSlotPresenter slot = GetOrCreateSlot(i);
					if (!(slot == null))
					{
						(int, bool) item = list[i];
						slot.gameObject.SetActive(value: true);
						StageResultParadoxSlotModel slotModel = new StageResultParadoxSlotModel();
						slotModel.ParadoxId = item.Item1;
						slotModel.IsRemoved = item.Item2;
						slot.Inject(slotModel);
					}
				}
			}
		}
		ArrangeSlotsAsync().Forget();
	}

	private StageResultParadoxSlotPresenter GetOrCreateSlot(int index)
	{
		while (true)
		{
			if (index < _cachedSlots.Count)
			{
				return _cachedSlots[index];
			}
			GameObject gameObject = _assetManager.InstantiateStageResultParadoxSlot();
			if (gameObject == null)
			{
				return null;
			}
			StageResultParadoxSlotPresenter component = gameObject.GetComponent<StageResultParadoxSlotPresenter>();
			if (component == null)
			{
				Object.Destroy(gameObject);
				return null;
			}
			gameObject.SetActive(value: false);
			_cachedSlots.Add(component);
		}
	}

	private async UniTask ArrangeSlotsAsync()
	{
		GameObject[] lines = _view.Lines;
		if (lines == null || lines.Length == 0)
		{
			return;
		}
		for (int i = 0; i < lines.Length; i++)
		{
			if (lines[i] != null)
			{
				lines[i].SetActive(value: true);
			}
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(_view.transform as RectTransform);
		await UniTask.Yield();
		float x = 0f;
		int lineIndex = 0;
		Transform parent = _view.Lines[0].transform;
		foreach (StageResultParadoxSlotPresenter cached in _cachedSlots)
		{
			if (cached == null || !cached.gameObject.activeSelf)
			{
				continue;
			}
			float width = (cached.transform as RectTransform).rect.width;
			if (x > 0f && x + width > 680f)
			{
				lineIndex++;
				if (lineIndex >= _view.Lines.Length)
				{
					lineIndex = _view.Lines.Length - 1;
				}
				parent = _view.Lines[lineIndex].transform;
				x = 0f;
			}
			cached.transform.SetParent(parent, worldPositionStays: false);
			x += width + 10f;
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(_view.transform as RectTransform);
		await UniTask.Yield();
		for (int j = 0; j < _view.Lines.Length; j++)
		{
			GameObject line = _view.Lines[j];
			if (!(line == null))
			{
				line.SetActive(line.transform.childCount > 0 && HasActiveChild(line.transform));
			}
		}
	}

	private bool HasActiveChild(Transform parent)
	{
		for (int i = 0; i < parent.childCount; i++)
		{
			if (parent.GetChild(i).gameObject.activeSelf)
			{
				return true;
			}
		}
		return false;
	}

	private void DeactivateAllSlots()
	{
		foreach (StageResultParadoxSlotPresenter cached in _cachedSlots)
		{
			if (!(cached == null))
			{
				cached.gameObject.SetActive(value: false);
			}
		}
		GameObject[] lines = _view.Lines;
		if (lines == null)
		{
			return;
		}
		foreach (GameObject gameObject in lines)
		{
			if (!(gameObject == null))
			{
				gameObject.SetActive(value: false);
			}
		}
	}
}
