using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "StressCameraEffectSettings", menuName = "Settings/Stress Camera Effect Settings")]
public class StressCameraEffectSettings : ScriptableObject
{
	[Header("Escape Setting")]
	[Tooltip("Stress 효과에서 벗어날 때의 딜레이를 설정합니다.")]
	public float EscapeDelay = 1f;

	[Tooltip("Stress 효과에서 벗어날 때의 페이드 지속 시간을 설정합니다.")]
	public float EscapeDuration = 2f;

	[Tooltip("Stress 효과에서 벗어날 때의 이징 타입을 설정합니다.")]
	public Ease EscapeEase = Ease.InOutSine;

	[Header("Stress Setting")]
	[Tooltip("Stress 구역마다 효과를 설정합니다.")]
	[SerializeField]
	public List<StressCameraEffectSequence> EffectSequences = new List<StressCameraEffectSequence>();
}
