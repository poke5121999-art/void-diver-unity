using VContainer;

public class GlobalShaderManager : MonoBase
{
	[Inject]
	private GameManager _gameManager;

	private static readonly int _globalIsRuntimePropertyID;

	private static readonly int _globalIsCurrentlyInStagePropertyID;

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	private void OnStageLoadCompleted()
	{
	}

	private void OnDisable()
	{
	}

	private void OnDestroy()
	{
	}
}
