using GameCommon.Enum;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "Stress Visual Effect Object Scriptable Data", menuName = "NEMO Art/Stress Visual Effect Object Scriptable Data")]
public class StressVisualEffectObjectScriptable : ScriptableObject
{
	[Header("Property Setting")]
	[Tooltip("조절할 머테리얼 프로퍼티 이름을 설정합니다.")]
	public string PropertyName = "";

	[Tooltip("전역(Global) 머테리얼 프로퍼티인지 여부입니다.")]
	public bool IsGlobalProperty;

	[Tooltip("해당 프로퍼티에 대한 설명입니다.")]
	public string Description = "";

	[Header("Escape Setting")]
	[Tooltip("Stress 상태에서 벗어날 때의 딜레이입니다.")]
	public float EscapeDelay = 1f;

	[Tooltip("Stress 상태에서 벗어날 때의 페이드 시간입니다.")]
	public float EscapeDuration = 2f;

	[Tooltip("Stress 상태에서 벗어날 때 사용할 이징 타입입니다.")]
	public Ease EscapeEase = Ease.InOutSine;

	[Header("Stress Setting")]
	[Tooltip("Stress 상태별 발생 수치 및 커브 설정입니다.")]
	[SerializeField]
	private List<StressVisualEffectSequence> _sequences = new List<StressVisualEffectSequence>
	{
		new StressVisualEffectSequence(EStressState.Alert, 0f, 0f),
		new StressVisualEffectSequence(EStressState.Fear, 0f, 1f),
		new StressVisualEffectSequence(EStressState.Despair, 0f, 1f)
	};

	public List<StressVisualEffectSequence> Sequences => _sequences;
}
