using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class GrabpassRenderFeature : ScriptableRendererFeature
{
	private class GrabpassRenderPass : ScriptableRenderPass
	{
		private class PassData
		{
			public RendererListHandle RendererListHandle;
		}

		private readonly ShaderTagId _shaderTagID;

		private readonly SortingCriteria _sortingCriteria;

		private FilteringSettings _filteringSettings;

		private const string _profilerID = "GrabpassRenderPass";

		public GrabpassRenderPass(in RenderPassEvent renderPassEvent, in string shaderLightMode, in SortingCriteria sortingCriteria, in RenderQueueRange renderQueueRange, in int layerMask)
		{
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
		}
	}

	[Serializable]
	public class GrabpassEntry
	{
		[Tooltip("LightMode 이름. 비어있으면 Sampling만 수행 (렌더 패스 생략)")]
		public string lightModeName;

		public RenderPassEvent renderPassEvent;

		[Tooltip("RenderPassEvent에 더해지는 오프셋 (다른 엔트리와 실행 순서 분리용)")]
		public int renderPassEventOffset;

		public SortingCriteria sortingCriteria;

		public RenderQueueRange renderQueueRange;

		[Header("Per-Entry Texture (선택)")]
		[Tooltip("이 엔트리 전용 그랩 텍스처명(셰이더 샘플러명). 비우면 피쳐 공용 텍스처명 사용.\n독립 해상도를 쓰려면 반드시 고유명을 지정해 다른 엔트리와 RT를 분리할 것")]
		public string textureName;

		[Tooltip("체크 시 이 엔트리는 아래 해상도를 독립 사용. 해제 시 피쳐 공용 해상도")]
		public bool overrideResolution;

		public CustomCameraUtils.ECustomCameraResolutionType resolutionType;

		[Header("그래픽 옵션 연동 (선택)")]
		[Tooltip("이 엔트리의 enable+해상도를 제어하는 전역 프로퍼티명 (GraphicOptionController가 quality별 push).\n값: 0=비활성, 1=Full, 2=Half, 4=Quarter, 8=Eighth. 비우면 위 직렬화 설정 사용")]
		public string qualityProperty;
	}

	[SerializeField]
	[FormerlySerializedAs("m_grabpassTextureName")]
	private string _grabpassTextureName;

	[SerializeField]
	[FormerlySerializedAs("m_screenResolutionType")]
	private CustomCameraUtils.ECustomCameraResolutionType _screenResolutionType;

	[Tooltip("모든 엔트리 공용 컬링 레이어 마스크. 이 레이어 오브젝트만 그랩패스 렌더링됨")]
	[SerializeField]
	private LayerMask _layerMask;

	[SerializeField]
	private List<GrabpassEntry> _entries;

	[HideInInspector]
	[SerializeField]
	[FormerlySerializedAs("m_grabpassLightModeName")]
	private string _grabpassLightModeName;

	[HideInInspector]
	[SerializeField]
	[FormerlySerializedAs("m_renderPassEvent")]
	private RenderPassEvent _renderPassEvent;

	[HideInInspector]
	[SerializeField]
	[FormerlySerializedAs("m_sortingCriteria")]
	private SortingCriteria _sortingCriteria;

	[HideInInspector]
	[SerializeField]
	[FormerlySerializedAs("m_renderQueueRange")]
	private RenderQueueRange _renderQueueRange;

	private readonly List<SamplingRenderpass> _samplingPasses;

	private readonly List<GrabpassRenderPass> _renderPasses;

	private readonly HashSet<string> _disabledLightModes;

	public void SetScreenResolutionType(CustomCameraUtils.ECustomCameraResolutionType resolutionType)
	{
	}

	public void SetEntryEnabled(string lightModeName, bool enabled)
	{
	}

	public override void Create()
	{
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
	}

	private bool IsEntryEnabled(GrabpassEntry entry)
	{
		return false;
	}

	private bool IsEntryActiveThisFrame(GrabpassEntry entry)
	{
		return false;
	}

	private bool TryGetQualityScale(GrabpassEntry entry, out float scale)
	{
		scale = default;
		return false;
	}
}
