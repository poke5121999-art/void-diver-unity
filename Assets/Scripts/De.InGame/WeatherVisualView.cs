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
	}

	private void SetAnomaly(EAnomalyType anomalyType)
	{
	}
}
