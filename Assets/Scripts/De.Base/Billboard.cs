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
		Canvas componentInParent = GetComponentInParent<Canvas>();
		if (componentInParent == null)
		{
			_logger.Error("billboard ui must be child of canvas. " + base.gameObject.name);
			_isUnderWorldSpace = false;
		}
		else if (componentInParent.renderMode == RenderMode.WorldSpace)
		{
			_isUnderWorldSpace = true;
		}
		else
		{
			_isUnderWorldSpace = false;
			_logger.Error("BillboardUI only works with WorldSpace Canvas. " + base.gameObject.name);
		}
	}

	private void LateUpdate()
	{
		AlignForwardWithCamera();
	}

	private void AlignForwardWithCamera()
	{
		if (_isUnderWorldSpace)
		{
			if (_mainCam == null)
			{
				_mainCam = Camera.main;
				if (_mainCam == null)
				{
					return;
				}
			}
			base.transform.forward = _mainCam.transform.forward;
		}
	}
}
