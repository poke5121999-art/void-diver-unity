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
			_filteringSettings = new FilteringSettings(renderQueueRange, layerMask, uint.MaxValue, 0);
			base.renderPassEvent = renderPassEvent + 1;
			_shaderTagID = new ShaderTagId(shaderLightMode);
			_sortingCriteria = sortingCriteria;
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			UniversalCameraData universalCameraData = frameData.Get<UniversalCameraData>();
			UniversalRenderingData universalRenderingData = frameData.Get<UniversalRenderingData>();
			UniversalResourceData universalResourceData = frameData.Get<UniversalResourceData>();
			PassData passData;
			using (IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<PassData>("GrabpassRenderPass", out passData))
			{
				SortingSettings sortingSettings = new SortingSettings(universalCameraData.camera);
				sortingSettings.criteria = _sortingCriteria;
				DrawingSettings drawingSettings = new DrawingSettings(_shaderTagID, sortingSettings);
				drawingSettings.perObjectData = (PerObjectData)1039;
				RendererListParams rendererListParams = new RendererListParams(universalRenderingData.cullResults, drawingSettings, _filteringSettings);
				passData.RendererListHandle = renderGraph.CreateRendererList(in rendererListParams);
				builder.UseRendererList(passData.RendererListHandle);
				builder.SetRenderAttachment(universalResourceData.activeColorTexture, 0, AccessFlags.Write);
				if (universalResourceData.activeDepthTexture.IsValid())
				{
					builder.SetRenderAttachmentDepth(universalResourceData.activeDepthTexture, AccessFlags.ReadWrite);
				}
				builder.AllowPassCulling(false);
				builder.SetRenderFunc<PassData>(static (PassData data, RasterGraphContext context) =>
				{
					context.cmd.DrawRendererList(data.RendererListHandle);
				});
			}
		}
	}

	[Serializable]
	public class GrabpassEntry
	{
		[Tooltip("LightMode 이름. 비어있으면 Sampling만 수행 (렌더 패스 생략)")]
		public string lightModeName = "";

		public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;

		[Tooltip("RenderPassEvent에 더해지는 오프셋 (다른 엔트리와 실행 순서 분리용)")]
		public int renderPassEventOffset;

		public SortingCriteria sortingCriteria = SortingCriteria.BackToFront;

		public RenderQueueRange renderQueueRange = RenderQueueRange.all;

		[Header("Per-Entry Texture (선택)")]
		[Tooltip("이 엔트리 전용 그랩 텍스처명(셰이더 샘플러명). 비우면 피쳐 공용 텍스처명 사용.\n독립 해상도를 쓰려면 반드시 고유명을 지정해 다른 엔트리와 RT를 분리할 것")]
		public string textureName = "";

		[Tooltip("체크 시 이 엔트리는 아래 해상도를 독립 사용. 해제 시 피쳐 공용 해상도")]
		public bool overrideResolution;

		public CustomCameraUtils.ECustomCameraResolutionType resolutionType = CustomCameraUtils.ECustomCameraResolutionType.Half;

		[Header("그래픽 옵션 연동 (선택)")]
		[Tooltip("이 엔트리의 enable+해상도를 제어하는 전역 프로퍼티명 (GraphicOptionController가 quality별 push).\n값: 0=비활성, 1=Full, 2=Half, 4=Quarter, 8=Eighth. 비우면 위 직렬화 설정 사용")]
		public string qualityProperty = "";
	}

	[SerializeField]
	[FormerlySerializedAs("m_grabpassTextureName")]
	private string _grabpassTextureName = "";

	[SerializeField]
	[FormerlySerializedAs("m_screenResolutionType")]
	private CustomCameraUtils.ECustomCameraResolutionType _screenResolutionType = CustomCameraUtils.ECustomCameraResolutionType.Half;

	[Tooltip("모든 엔트리 공용 컬링 레이어 마스크. 이 레이어 오브젝트만 그랩패스 렌더링됨")]
	[SerializeField]
	private LayerMask _layerMask = -1;

	[SerializeField]
	private List<GrabpassEntry> _entries = new List<GrabpassEntry>();

	[HideInInspector]
	[SerializeField]
	[FormerlySerializedAs("m_grabpassLightModeName")]
	private string _grabpassLightModeName = "";

	[HideInInspector]
	[SerializeField]
	[FormerlySerializedAs("m_renderPassEvent")]
	private RenderPassEvent _renderPassEvent = RenderPassEvent.AfterRenderingTransparents;

	[HideInInspector]
	[SerializeField]
	[FormerlySerializedAs("m_sortingCriteria")]
	private SortingCriteria _sortingCriteria = SortingCriteria.BackToFront;

	[HideInInspector]
	[SerializeField]
	[FormerlySerializedAs("m_renderQueueRange")]
	private RenderQueueRange _renderQueueRange = RenderQueueRange.all;

	private readonly List<SamplingRenderpass> _samplingPasses = new List<SamplingRenderpass>();

	private readonly List<GrabpassRenderPass> _renderPasses = new List<GrabpassRenderPass>();

	private readonly HashSet<string> _disabledLightModes = new HashSet<string>();

	public void SetScreenResolutionType(CustomCameraUtils.ECustomCameraResolutionType resolutionType)
	{
		if (_screenResolutionType != resolutionType)
		{
			_screenResolutionType = resolutionType;
			Create();
		}
	}

	public void SetEntryEnabled(string lightModeName, bool enabled)
	{
		if (!string.IsNullOrEmpty(lightModeName))
		{
			if (enabled)
			{
				_disabledLightModes.Remove(lightModeName);
			}
			else
			{
				_disabledLightModes.Add(lightModeName);
			}
		}
	}

	public override void Create()
	{
		if (_entries.Count == 0 && !string.IsNullOrEmpty(_grabpassLightModeName))
		{
			GrabpassEntry grabpassEntry = new GrabpassEntry();
			grabpassEntry.lightModeName = _grabpassLightModeName;
			grabpassEntry.renderPassEvent = _renderPassEvent;
			grabpassEntry.sortingCriteria = _sortingCriteria;
			grabpassEntry.renderQueueRange = _renderQueueRange;
			_entries.Add(grabpassEntry);
		}
		_samplingPasses.Clear();
		_renderPasses.Clear();
		float downsamplingFloat = CustomCameraUtils.GetDownsamplingFloat(_screenResolutionType);
		for (int i = 0; i < _entries.Count; i++)
		{
			GrabpassEntry grabpassEntry2 = _entries[i];
			RenderPassEvent renderPassEvent = grabpassEntry2.renderPassEvent + grabpassEntry2.renderPassEventOffset;
			float downsamplingScale = (grabpassEntry2.overrideResolution ? CustomCameraUtils.GetDownsamplingFloat(grabpassEntry2.resolutionType) : downsamplingFloat);
			string renderPassName = (string.IsNullOrEmpty(grabpassEntry2.textureName) ? _grabpassTextureName : grabpassEntry2.textureName);
			_samplingPasses.Add(new SamplingRenderpass(renderPassEvent, renderPassName, downsamplingScale, FilterMode.Point));
			if (!string.IsNullOrEmpty(grabpassEntry2.lightModeName))
			{
				_renderPasses.Add(new GrabpassRenderPass(renderPassEvent, grabpassEntry2.lightModeName, grabpassEntry2.sortingCriteria, grabpassEntry2.renderQueueRange, _layerMask.value));
			}
		}
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		int num = -1;
		for (int i = 0; i < _entries.Count; i++)
		{
			if (IsEntryEnabled(_entries[i]))
			{
				num = i;
			}
		}
		int num2 = 0;
		for (int j = 0; j < _entries.Count; j++)
		{
			GrabpassEntry grabpassEntry = _entries[j];
			bool flag = string.IsNullOrEmpty(grabpassEntry.lightModeName);
			if (IsEntryActiveThisFrame(grabpassEntry))
			{
				if (TryGetQualityScale(grabpassEntry, out var scale))
				{
					_samplingPasses[j].SetDownsamplingScale(scale);
				}
				_samplingPasses[j].SkipCleanup = j < num;
				renderer.EnqueuePass(_samplingPasses[j]);
				if (!flag)
				{
					renderer.EnqueuePass(_renderPasses[num2]);
					num2++;
				}
			}
			else if (!flag)
			{
				num2++;
			}
		}
	}

	private bool IsEntryEnabled(GrabpassEntry entry)
	{
		if (string.IsNullOrEmpty(entry.lightModeName))
		{
			return true;
		}
		return !_disabledLightModes.Contains(entry.lightModeName);
	}

	private bool IsEntryActiveThisFrame(GrabpassEntry entry)
	{
		if (!string.IsNullOrEmpty(entry.lightModeName) && _disabledLightModes.Contains(entry.lightModeName))
		{
			return false;
		}
		if (!string.IsNullOrEmpty(entry.qualityProperty))
		{
			return (int)Shader.GetGlobalFloat(entry.qualityProperty) > 0;
		}
		return true;
	}

	private bool TryGetQualityScale(GrabpassEntry entry, out float scale)
	{
		scale = 1f;
		if (!string.IsNullOrEmpty(entry.qualityProperty))
		{
			CustomCameraUtils.ECustomCameraResolutionType eCustomCameraResolutionType = (CustomCameraUtils.ECustomCameraResolutionType)(int)Shader.GetGlobalFloat(entry.qualityProperty);
			if ((int)eCustomCameraResolutionType > 0)
			{
				scale = CustomCameraUtils.GetDownsamplingFloat(eCustomCameraResolutionType);
				return true;
			}
		}
		return false;
	}
}
