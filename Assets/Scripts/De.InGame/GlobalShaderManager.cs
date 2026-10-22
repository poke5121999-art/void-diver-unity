using GameCommon.Enum;
using UniRx;
using UnityEngine;
using VContainer;

public class GlobalShaderManager : MonoBase
{
	[Inject]
	private GameManager _gameManager;

	private static readonly int _globalIsRuntimePropertyID = Shader.PropertyToID("_GlobalIsRuntime");

	private static readonly int _globalIsCurrentlyInStagePropertyID = Shader.PropertyToID("_GlobalIsCurrentlyInStage");

	protected override void Awake()
	{
		base.Awake();
		Object.DontDestroyOnLoad(this);
	}

	private void Start()
	{
		Shader.SetGlobalFloat(_globalIsRuntimePropertyID, 1f);
		_gameManager.RxStageState.Where((EStageState s) => s == EStageState.Intro || s == EStageState.Playing).Take(1).Subscribe(delegate
		{
			OnStageLoadCompleted();
		}).AddTo(this);
	}

	private void OnStageLoadCompleted()
	{
		Shader.SetGlobalFloat(_globalIsCurrentlyInStagePropertyID, _gameManager.IsInStage ? 1f : 0f);
	}

	private void OnDisable()
	{
		Shader.SetGlobalFloat(_globalIsRuntimePropertyID, 0f);
		Shader.SetGlobalFloat(_globalIsCurrentlyInStagePropertyID, 0f);
	}

	private void OnDestroy()
	{
		Shader.SetGlobalFloat(_globalIsRuntimePropertyID, 0f);
		Shader.SetGlobalFloat(_globalIsCurrentlyInStagePropertyID, 0f);
	}
}
