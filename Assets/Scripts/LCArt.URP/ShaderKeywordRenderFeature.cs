using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

public class ShaderKeywordRenderFeature : ScriptableRendererFeature
{
	public enum ExecutionMode
	{
		[Tooltip("항상 실행")]
		Always = 0,
		[Tooltip("런타임(Play Mode)에서만 실행")]
		RuntimeOnly = 1,
		[Tooltip("에디터(Edit Mode)에서만 실행")]
		EditorOnly = 2
	}

	[Serializable]
	public class Settings
	{
		[Tooltip("렌더링 시작 시 활성화할 Global Shader Keywords")]
		public List<string> EnableKeywords;

		[Tooltip("키워드를 적용할 시점")]
		public RenderPassEvent ApplyEvent;

		[Tooltip("키워드를 복원할 시점")]
		public RenderPassEvent RestoreEvent;

		[Tooltip("렌더링 종료 후 키워드 상태를 원래대로 복원할지 여부")]
		public bool RestoreKeywordsAfterRendering;

		[Tooltip("실행 모드 설정")]
		public ExecutionMode ExecutionMode;
	}

	private class ShaderKeywordApplyPass : ScriptableRenderPass
	{
		private class PassData
		{
			public GlobalKeyword[] EnableKeywords;

			public bool[] EnableKeywordsPreviousState;

			public bool TrackPreviousState;
		}

		private readonly GlobalKeyword[] _enableKeywords;

		private readonly bool _trackPreviousState;

		private bool[] _enableKeywordsPreviousState;

		private readonly ProfilingSampler _profilingSampler;

		public ShaderKeywordApplyPass(GlobalKeyword[] enableKeywords, bool trackPreviousState)
		{
		}

		public bool[] GetEnableKeywordsPreviousState()
		{
			return null;
		}

		public GlobalKeyword[] GetEnableKeywords()
		{
			return null;
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
		}

		public void Cleanup()
		{
		}
	}

	private class ShaderKeywordRestorePass : ScriptableRenderPass
	{
		private class PassData
		{
			public GlobalKeyword[] EnableKeywords;

			public bool[] EnableKeywordsPreviousState;
		}

		private readonly ShaderKeywordApplyPass _applyPass;

		private readonly ProfilingSampler _profilingSampler;

		public ShaderKeywordRestorePass(ShaderKeywordApplyPass applyPass)
		{
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
		}

		public void Cleanup()
		{
		}
	}

	public Settings PassSettings;

	private ShaderKeywordApplyPass _applyPass;

	private ShaderKeywordRestorePass _restorePass;

	private GlobalKeyword[] _enableKeywordsCache;

	public override void Create()
	{
	}

	private void CacheKeywords()
	{
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
	}

	private bool ShouldExecute()
	{
		return false;
	}

	protected override void Dispose(bool disposing)
	{
	}
}
