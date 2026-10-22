using GameCommon.Enum;
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

	private readonly NetworkVariable<int> _syncArtifactId;

	private readonly ReactiveProperty<int> _rxArtifactId;

	private bool _salesSlotSubscribed;

	public override void OnNetworkSpawn()
	{
	}

	public override void OnNetworkDespawn()
	{
	}

	private void RegisterVariableEvents()
	{
	}

	private void UnregisterVariableEvents()
	{
	}

	private void TrySubscribeSalesSlot()
	{
	}

	private int GetSalesSlotIndex()
	{
		return 0;
	}

	private void SyncArtifactFromSalesSlot(int slotIndex)
	{
	}

	private void RefreshArtifactIcon(int artifactId)
	{
	}

	private void RefreshGradeVfx(EGoodsGradeType grade)
	{
	}



}
