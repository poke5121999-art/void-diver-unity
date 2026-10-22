using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static class DeferredGpuRelease
{
	private enum EReleaseKind
	{
		RenderTextureTemporary = 0,
		RenderTexturePersistent = 1,
		ComputeBuffer = 2
	}

	private struct Entry
	{
		public object Resource;

		public EReleaseKind Kind;

		public int EnqueueFrame;
	}

	private static readonly Queue<Entry> _queue;

	private static readonly HashSet<object> _pendingResources;

	private const int QUEUE_DEPTH_WARNING_THRESHOLD = 64;

	public static int PendingCount => 0;

	public static int DelayFramesOverride { get; set; }

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Initialize()
	{
	}

	public static void ReleaseTemporaryRT(RenderTexture rt)
	{
	}

	public static void ReleasePersistentRT(RenderTexture rt)
	{
	}

	public static void ReleaseBuffer(ComputeBuffer buffer)
	{
	}

	public static void FlushAll()
	{
	}

	private static void Enqueue(object resource, EReleaseKind kind)
	{
	}

	private static void OnBeginContextRendering(ScriptableRenderContext context, List<Camera> cameras)
	{
	}

	private static void ReleaseEntry(Entry entry)
	{
	}
}
