using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class CanvasShaderChannelEnforcer : MonoBehaviour
{
	[SerializeField]
	[Tooltip("이 캔버스가 유지해야 할 채널 마스크.\n일반 UI: Nothing\nSpine UI: TexCoord1 | TexCoord2\n라이팅 폰트(Normal 입력): Normal | Tangent | TexCoord1")]
	private AdditionalCanvasShaderChannels _requiredChannels;

	private Canvas _canvas;

	private void Awake()
	{
		_canvas = GetComponent<Canvas>();
	}

	private void OnEnable()
	{
		Canvas.willRenderCanvases -= EnforceChannels;
		Canvas.willRenderCanvases += EnforceChannels;
	}

	private void OnDisable()
	{
		Canvas.willRenderCanvases -= EnforceChannels;
	}

	private void EnforceChannels()
	{
		if (!(_canvas == null) && _canvas.additionalShaderChannels != _requiredChannels)
		{
			_canvas.additionalShaderChannels = _requiredChannels;
		}
	}
}
