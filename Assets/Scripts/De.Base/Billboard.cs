using Serilog.Core;
using UnityEngine;
using VContainer;

[DisallowMultipleComponent]
public class Billboard : MonoBehaviour
{
	[Inject]
	private Serilog.Core.Logger _logger;

	private Camera _mainCam;

	private bool _isUnderWorldSpace;

	private void OnEnable()
	{
	}

	private void LateUpdate()
	{
	}

	private void AlignForwardWithCamera()
	{
	}
}
