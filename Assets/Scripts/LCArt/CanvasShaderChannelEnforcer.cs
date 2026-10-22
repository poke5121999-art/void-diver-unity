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
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void EnforceChannels()
	{
	}
}
