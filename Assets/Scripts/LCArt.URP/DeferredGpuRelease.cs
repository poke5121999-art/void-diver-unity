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

	private static readonly Queue<Entry> _queue = new Queue<Entry>();

	private static readonly HashSet<object> _pendingResources = new HashSet<object>();

	private const int QUEUE_DEPTH_WARNING_THRESHOLD = 64;

	public static int PendingCount => _queue.Count;

	public static int DelayFramesOverride { get; set; }

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Initialize()
	{
		RenderPipelineManager.beginContextRendering -= OnBeginContextRendering;
		RenderPipelineManager.beginContextRendering += OnBeginContextRendering;
		Application.quitting -= FlushAll;
		Application.quitting += FlushAll;
	}

	public static void ReleaseTemporaryRT(RenderTexture rt)
	{
		if (!(rt == null))
		{
			Enqueue(rt, EReleaseKind.RenderTextureTemporary);
		}
	}

	public static void ReleasePersistentRT(RenderTexture rt)
	{
		if (!(rt == null))
		{
			Enqueue(rt, EReleaseKind.RenderTexturePersistent);
		}
	}

	public static void ReleaseBuffer(ComputeBuffer buffer)
	{
		if (buffer != null)
		{
			Enqueue(buffer, EReleaseKind.ComputeBuffer);
		}
	}

	public static void FlushAll()
	{
		while (_queue.Count > 0)
		{
			ReleaseEntry(_queue.Dequeue());
		}
		_pendingResources.Clear();
	}

	private static void Enqueue(object resource, EReleaseKind kind)
	{
		if (!Application.isPlaying)
		{
			ReleaseEntry(new Entry
			{
				Resource = resource,
				Kind = kind
			});
		}
		else if (_pendingResources.Add(resource))
		{
			_queue.Enqueue(new Entry
			{
				Resource = resource,
				Kind = kind,
				EnqueueFrame = Time.frameCount
			});
		}
	}

	private static void OnBeginContextRendering(ScriptableRenderContext context, List<Camera> cameras)
	{
		if (_queue.Count == 0)
		{
			return;
		}
		int num = ((DelayFramesOverride > 0) ? DelayFramesOverride : Mathf.Max(3, QualitySettings.maxQueuedFrames + 1));
		int frameCount = Time.frameCount;
		while (_queue.Count > 0)
		{
			if (frameCount - _queue.Peek().EnqueueFrame <= num)
			{
				break;
			}
			Entry entry = _queue.Dequeue();
			_pendingResources.Remove(entry.Resource);
			ReleaseEntry(entry);
		}
	}

	private static void ReleaseEntry(Entry entry)
	{
		switch (entry.Kind)
		{
		case EReleaseKind.RenderTextureTemporary:
		{
			RenderTexture renderTexture = entry.Resource as RenderTexture;
			if (renderTexture != null)
			{
				RenderTexture.ReleaseTemporary(renderTexture);
			}
			break;
		}
		case EReleaseKind.RenderTexturePersistent:
		{
			RenderTexture renderTexture = entry.Resource as RenderTexture;
			if (!(renderTexture == null))
			{
				renderTexture.Release();
				if (Application.isPlaying)
				{
					Object.Destroy(renderTexture);
				}
				else
				{
					Object.DestroyImmediate(renderTexture);
				}
			}
			break;
		}
		case EReleaseKind.ComputeBuffer:
		{
			ComputeBuffer computeBuffer = entry.Resource as ComputeBuffer;
			if (computeBuffer != null)
			{
				computeBuffer.Release();
			}
			break;
		}
		}
	}
}
