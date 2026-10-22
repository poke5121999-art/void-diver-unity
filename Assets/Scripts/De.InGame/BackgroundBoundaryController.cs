using GameCommon.Enum;
using UniRx;
using UnityEngine;
using VContainer;

public class BackgroundBoundaryController : MonoBase
{
	[Header("Scriptable Data")]
	[SerializeField]
	private BackgroundBoundaryScriptableData _backgroundBoundaryScriptableData;

	[Header("Boundary Setting")]
	[SerializeField]
	private bool _boundaryEnable = true;

	[Header("Boundary Box Effects (외곽 4방향)")]
	[SerializeField]
	private BackgroundBoxAreaEffect _boundaryBoxUp;

	[SerializeField]
	private BackgroundBoxAreaEffect _boundaryBoxDown;

	[SerializeField]
	private BackgroundBoxAreaEffect _boundaryBoxLeft;

	[SerializeField]
	private BackgroundBoxAreaEffect _boundaryBoxRight;

	[Header("Boundary Side Particle Effects")]
	[SerializeField]
	private ParticleSystem _boundarySideEffectUpObject;

	[SerializeField]
	private ParticleSystem _boundarySideEffectLeftObject;

	[SerializeField]
	private ParticleSystem _boundarySideEffectDownObject;

	[SerializeField]
	private ParticleSystem _boundarySideEffectRightObject;

	[Inject]
	private GameManager _gameManager;

	[Inject]
	private GameMapManager _gameMapManager;

	private float _mapWidth;

	private float _mapHeight;

	private const float _boundaryParticleRatioByScale = 1f;

	protected override void Awake()
	{
		base.Awake();
	}

	private void Start()
	{
		_gameManager.RxStageState.Where((EStageState s) => s == EStageState.Intro || s == EStageState.Playing).Take(1).Subscribe(delegate
		{
			OnStageLoadCompleted();
		}).AddTo(this);
	}

	private void OnStageLoadCompleted()
	{
		var mapSize = _gameMapManager.MapSize;
		_mapWidth = (float)mapSize.Width * 30f;
		mapSize = _gameMapManager.MapSize;
		_mapHeight = (float)mapSize.Height * 30f;
		bool flag = _boundaryEnable && _mapWidth >= 29f && _mapHeight >= 29f;
		if (flag)
		{
			UpdateBoundaryBoxes();
			InitializeBoundarySideParticles();
		}
		SetBoundaryEffectActive(flag);
	}

	private void UpdateBoundaryBoxes()
	{
		BackgroundBoundaryScriptableData data = _backgroundBoundaryScriptableData;
		if (data == null)
		{
			return;
		}
		float halfWidth = _mapWidth * 0.5f;
		float halfHeight = _mapHeight * 0.5f;
		float thickness = data.BoxThickness;
		float innerThickness = data.BoxInnerThickness;
		float boxHeight = data.BoxHeight;
		Vector3 offset = data.BoundaryPivotOffset;
		SetBoxTransform(_boundaryBoxUp, new Vector3(offset.x + halfWidth, offset.y + 0f, offset.z + (thickness * 0.5f + _mapHeight - (innerThickness + 2f))), new Vector3(thickness + thickness + _mapWidth, boxHeight, thickness));
		SetBoxTransform(_boundaryBoxDown, new Vector3(offset.x + halfWidth, offset.y + 0f, offset.z + (0f - thickness * 0.5f) + innerThickness + 2f), new Vector3(thickness + thickness + _mapWidth, boxHeight, thickness));
		SetBoxTransform(_boundaryBoxLeft, new Vector3(0f - thickness * 0.5f + innerThickness + 2f + offset.x, offset.y + 0f, offset.z + halfHeight), new Vector3(thickness, boxHeight, thickness + thickness + _mapHeight));
		SetBoxTransform(_boundaryBoxRight, new Vector3(thickness * 0.5f + _mapWidth - (innerThickness + 2f) + offset.x, offset.y + 0f, offset.z + halfHeight), new Vector3(thickness, boxHeight, thickness + thickness + _mapHeight));
		ApplyOverlayToBox(_boundaryBoxUp, data);
		ApplyOverlayToBox(_boundaryBoxDown, data);
		ApplyOverlayToBox(_boundaryBoxLeft, data);
		ApplyOverlayToBox(_boundaryBoxRight, data);
	}

	private void SetBoxTransform(BackgroundBoxAreaEffect box, Vector3 position, Vector3 scale)
	{
		if (!(box == null))
		{
			box.transform.position = position;
			box.transform.localScale = scale;
		}
	}

	private void ApplyOverlayToBox(BackgroundBoxAreaEffect box, BackgroundBoundaryScriptableData data)
	{
		if (!(box == null))
		{
			box.ApplyOverlay(data.EffectType, data.OverlayColor, data.EdgeSmooth, data.ShimmerSpeed, data.ShimmerStrength, data.FogDensity, data.TargetIntensity);
		}
	}

	private void InitializeBoundarySideParticles()
	{
		float width = _mapWidth;
		float height = _mapHeight;
		SetupSideParticle(_boundarySideEffectUpObject, new Vector3(width * 0.5f, 0f, height), Quaternion.Euler(0f, 90f, 0f), new Vector3(1f, 1f, width / 30f), width * 1f);
		SetupSideParticle(_boundarySideEffectLeftObject, new Vector3(width, 0f, height * 0.5f), Quaternion.Euler(0f, 0f, 0f), new Vector3(1f, 1f, height / 30f), height * 1f);
		SetupSideParticle(_boundarySideEffectDownObject, new Vector3(width * 0.5f, 0f, 0f), Quaternion.Euler(0f, 90f, 0f), new Vector3(1f, 1f, width / 30f), width * 1f);
		SetupSideParticle(_boundarySideEffectRightObject, new Vector3(0f, 0f, height * 0.5f), Quaternion.Euler(0f, 0f, 0f), new Vector3(1f, 1f, height / 30f), height * 1f);
	}

	private void SetupSideParticle(ParticleSystem particle, Vector3 position, Quaternion rotation, Vector3 scale, float emissionRate)
	{
		if (!(particle == null))
		{
			particle.transform.SetPositionAndRotation(position, rotation);
			particle.transform.localScale = scale;
			ParticleSystem.EmissionModule emission = particle.emission;
			emission.rateOverTime = emissionRate;
		}
	}

	private void SetBoundaryEffectActive(bool active)
	{
		SetBoxActive(_boundaryBoxUp, active);
		SetBoxActive(_boundaryBoxDown, active);
		SetBoxActive(_boundaryBoxLeft, active);
		SetBoxActive(_boundaryBoxRight, active);
		SetParticleActive(_boundarySideEffectUpObject, active);
		SetParticleActive(_boundarySideEffectLeftObject, active);
		SetParticleActive(_boundarySideEffectDownObject, active);
		SetParticleActive(_boundarySideEffectRightObject, active);
	}

	private void SetBoxActive(BackgroundBoxAreaEffect box, bool active)
	{
		if (!(box == null))
		{
			box.gameObject.SetActive(active);
		}
	}

	private void SetParticleActive(ParticleSystem particle, bool active)
	{
		if (!(particle == null))
		{
			particle.gameObject.SetActive(active);
			if (active)
			{
				particle.Play();
			}
			else
			{
				particle.Stop();
			}
		}
	}
}
