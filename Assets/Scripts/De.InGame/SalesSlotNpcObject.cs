using System.Linq;
using GameCommon.Enum;
using GameCommon.Table;
using NemoLib.AssetManagement;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class SalesSlotNpcObject : NpcObject
{
	[SerializeField]
	private Image _artifactIcon;

	[SerializeField]
	private GameObject _gradeVfxRare;

	[SerializeField]
	private GameObject _gradeVfxElite;

	[SerializeField]
	private GameObject _gradeVfxEpic;

	[SerializeField]
	private GameObject _gradeVfxLegend;

	[SerializeField]
	private GameObject _gradeVfxUnique;

	[Inject]
	private AssetManager _assetManager;

	[Inject]
	private TycoonManager _tycoonManager;

	private readonly NetworkVariable<int> _syncArtifactId = new NetworkVariable<int>();

	private readonly ReactiveProperty<int> _rxArtifactId = new ReactiveProperty<int>();

	private bool _salesSlotSubscribed;

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		RegisterVariableEvents();
		if (base.IsHost)
		{
			TrySubscribeSalesSlot();
			base.RxNpcFunctionIds.ObserveAdd().Subscribe(delegate
			{
				TrySubscribeSalesSlot();
			}).AddTo(base.Disposable);
		}
		_rxArtifactId.Subscribe(RefreshArtifactIcon).AddTo(base.Disposable);
	}

	public override void OnNetworkDespawn()
	{
		UnregisterVariableEvents();
		base.OnNetworkDespawn();
	}

	private void RegisterVariableEvents()
	{
		_rxArtifactId.Value = _syncArtifactId.Value;
		_syncArtifactId.OnValueChanged += delegate(int _, int newValue)
		{
			_rxArtifactId.Value = newValue;
		};
	}

	private void UnregisterVariableEvents()
	{
		_syncArtifactId.OnValueChanged = null;
	}

	private void TrySubscribeSalesSlot()
	{
		if (_salesSlotSubscribed)
		{
			return;
		}
		int slotIndex = GetSalesSlotIndex();
		if (slotIndex >= 0)
		{
			_salesSlotSubscribed = true;
			SyncArtifactFromSalesSlot(slotIndex);
			_tycoonManager.OnSalesSlotsChanged.Subscribe(delegate
			{
				SyncArtifactFromSalesSlot(slotIndex);
			}).AddTo(base.Disposable);
		}
	}

	private int GetSalesSlotIndex()
	{
		int[] npcFunctionIds = base.NpcFunctionIds;
		if (npcFunctionIds != null)
		{
			TNpcFunction tNpcFunction = base.NpcFunctionIds.Select((int _) => TNpcFunction.Table[_]).FirstOrDefault((TNpcFunction _) => _.Type == ENpcFunctionType.TycoonSalesSlot);
			if (tNpcFunction != null)
			{
				return tNpcFunction.Value1;
			}
		}
		return -1;
	}

	private void SyncArtifactFromSalesSlot(int slotIndex)
	{
		if (PlayerManager.SalesSlots.TryGetValue(slotIndex, out var value))
		{
			_syncArtifactId.Value = value?.Goods?.Id ?? 0;
		}
		else
		{
			_syncArtifactId.Value = 0;
		}
	}

	private void RefreshArtifactIcon(int artifactId)
	{
		if (_artifactIcon == null)
		{
			return;
		}
		if (artifactId < 1)
		{
			_artifactIcon.sprite = null;
			_artifactIcon.gameObject.SetActive(value: false);
			_gradeVfxRare?.SetActive(value: false);
			_gradeVfxElite?.SetActive(value: false);
			_gradeVfxEpic?.SetActive(value: false);
			_gradeVfxLegend?.SetActive(value: false);
			_gradeVfxUnique?.SetActive(value: false);
		}
		else
		{
			_artifactIcon.sprite = _assetManager.GetIconEquipment(artifactId);
			_artifactIcon.gameObject.SetActive(value: true);
			RefreshGradeVfx(TEquipment.Table[artifactId].Grade);
		}
	}

	private void RefreshGradeVfx(EGoodsGradeType grade)
	{
		_gradeVfxRare?.SetActive(grade == EGoodsGradeType.Rare);
		_gradeVfxElite?.SetActive(grade == EGoodsGradeType.Elite);
		_gradeVfxEpic?.SetActive(grade == EGoodsGradeType.Epic);
		_gradeVfxLegend?.SetActive(grade == EGoodsGradeType.Legend);
		_gradeVfxUnique?.SetActive(grade == EGoodsGradeType.Unique);
	}



}
