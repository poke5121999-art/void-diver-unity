using System.Linq;
using UniRx;
using GameCommon.Enum;
using NemoLib.Log;
using UnityEngine;
using UnityEngine.Rendering;
using VContainer;

[DefaultExecutionOrder(-100)]
[RequireComponent(typeof(Volume))]
public class WeatherVisualView : MonoBase
{
	[Header("Weather Component Settings")]
	[SerializeField]
	private Volume _volume;

	[Header("Anomaly Settings")]
	[SerializeField]
	private AnomalyVolumeConfig _anomalyVolumeConfig;

	[Inject]
	private SeriLogger _logger;

	[Inject]
	private GameContext _gameContext;

	[Inject]
	private GameManager _gameManager;

	private void Start()
	{
		if (!(base.gameObject.scene.name != "StageScene"))
		{
			_gameManager.RxStageState.Where((EStageState state) => state == EStageState.Playing).Subscribe(delegate
			{
				SetAnomaly(_gameContext.AnomalyType);
			}).AddTo(this);
		}
	}

	private void SetAnomaly(EAnomalyType anomalyType)
	{
		if (_anomalyVolumeConfig == null)
		{
			_logger.Error("AnomalyVolumeConfig is not assigned.");
			return;
		}
		VolumeProfile volumeProfile = _anomalyVolumeConfig.GetProfile(anomalyType);
		if (volumeProfile == null)
		{
			_logger.Warning(string.Format("No profile found for {0} in AnomalyVolumeConfig", anomalyType));
			return;
		}
		_volume.sharedProfile = volumeProfile;
	}
}
