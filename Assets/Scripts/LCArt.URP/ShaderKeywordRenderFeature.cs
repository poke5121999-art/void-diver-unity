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
		public List<string> EnableKeywords = new List<string>();

		[Tooltip("키워드를 적용할 시점")]
		public RenderPassEvent ApplyEvent = (RenderPassEvent)250;

		[Tooltip("키워드를 복원할 시점")]
		public RenderPassEvent RestoreEvent = (RenderPassEvent)1000;

		[Tooltip("렌더링 종료 후 키워드 상태를 원래대로 복원할지 여부")]
		public bool RestoreKeywordsAfterRendering = true;

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
			_enableKeywords = enableKeywords;
			_trackPreviousState = trackPreviousState;
			_enableKeywordsPreviousState = new bool[enableKeywords.Length];
			_profilingSampler = new ProfilingSampler("ShaderKeywordApplyPass");
		}

		public bool[] GetEnableKeywordsPreviousState()
		{
			return _enableKeywordsPreviousState;
		}

		public GlobalKeyword[] GetEnableKeywords()
		{
			return _enableKeywords;
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			using IUnsafeRenderGraphBuilder builder = renderGraph.AddUnsafePass<PassData>(_profilingSampler.name, out var passData, _profilingSampler);
			passData.EnableKeywords = _enableKeywords;
			passData.EnableKeywordsPreviousState = _enableKeywordsPreviousState;
			passData.TrackPreviousState = _trackPreviousState;
			builder.AllowPassCulling(false);
			builder.SetRenderFunc(static (PassData data, UnsafeGraphContext context) =>
			{
				UnsafeCommandBuffer cmd = context.cmd;
				for (int i = 0; i < data.EnableKeywords.Length; i++)
				{
					if (data.TrackPreviousState)
					{
						data.EnableKeywordsPreviousState[i] = Shader.IsKeywordEnabled(in data.EnableKeywords[i]);
					}
					cmd.EnableKeyword(in data.EnableKeywords[i]);
				}
			});
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
			_applyPass = applyPass;
			_profilingSampler = new ProfilingSampler("ShaderKeyword_Restore");
		}

		public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
		{
			using IUnsafeRenderGraphBuilder builder = renderGraph.AddUnsafePass<PassData>(_profilingSampler.name, out var passData, _profilingSampler);
			passData.EnableKeywords = _applyPass.GetEnableKeywords();
			passData.EnableKeywordsPreviousState = _applyPass.GetEnableKeywordsPreviousState();
			builder.AllowPassCulling(false);
			builder.SetRenderFunc(static (PassData data, UnsafeGraphContext context) =>
			{
				UnsafeCommandBuffer cmd = context.cmd;
				for (int i = 0; i < data.EnableKeywords.Length; i++)
				{
					if (data.EnableKeywordsPreviousState[i])
					{
						cmd.EnableKeyword(in data.EnableKeywords[i]);
					}
					else
					{
						cmd.DisableKeyword(in data.EnableKeywords[i]);
					}
				}
			});
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
		CacheKeywords();
		_applyPass = new ShaderKeywordApplyPass(_enableKeywordsCache, PassSettings.RestoreKeywordsAfterRendering);
		_applyPass.renderPassEvent = PassSettings.ApplyEvent;
		if (PassSettings.RestoreKeywordsAfterRendering)
		{
			_restorePass = new ShaderKeywordRestorePass(_applyPass);
			_restorePass.renderPassEvent = PassSettings.RestoreEvent;
		}
	}

	private void CacheKeywords()
	{
		List<GlobalKeyword> list = new List<GlobalKeyword>();
		foreach (string enableKeyword in PassSettings.EnableKeywords)
		{
			if (!string.IsNullOrEmpty(enableKeyword))
			{
				list.Add(GlobalKeyword.Create(enableKeyword));
			}
		}
		_enableKeywordsCache = list.ToArray();
	}

	public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
	{
		if (_enableKeywordsCache == null)
		{
			CacheKeywords();
		}
		if (_enableKeywordsCache.Length != 0 && ShouldExecute())
		{
			renderer.EnqueuePass(_applyPass);
			if (PassSettings.RestoreKeywordsAfterRendering && _restorePass != null)
			{
				renderer.EnqueuePass(_restorePass);
			}
		}
	}

	private bool ShouldExecute()
	{
		return PassSettings.ExecutionMode != ExecutionMode.EditorOnly;
	}

	protected override void Dispose(bool disposing)
	{
	}
}
