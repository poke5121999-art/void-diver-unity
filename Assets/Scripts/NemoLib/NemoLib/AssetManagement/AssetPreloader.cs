using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UniRx;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.U2D;

namespace NemoLib.AssetManagement
{
	internal class AssetPreloader
	{
		[CompilerGenerated]
		private sealed class _003C_003Ec__DisplayClass3_0
		{
			public AssetPreloader _003C_003E4__this;

			public string label;

			public IList<IResourceLocation> locations;

			public Stopwatch stopwatch;

			internal UniTask _003CEnsureAsync_003Eb__0(KeyValuePair<Type, IAssetContainer> _)
			{
				return default;
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CEnsureAssetWithLogAsync_003Ed__4 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder _003C_003Et__builder;

			public IAssetContainer container;

			public string label;

			public IList<IResourceLocation> locations;

			private UniTask.Awaiter _003C_003Eu__1;

			private void MoveNext()
			{
			}

			void IAsyncStateMachine.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				this.MoveNext();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
			}

			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
				this.SetStateMachine(stateMachine);
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CEnsureAsync_003Ed__3 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder _003C_003Et__builder;

			public AssetPreloader _003C_003E4__this;

			public string label;

			private _003C_003Ec__DisplayClass3_0 _003C_003E8__1;

			public ReactiveProperty<float> progress;

			private ThreadPriority _003CoriginalLoadingPriority_003E5__2;

			private int _003CoriginalUploadTimeSlice_003E5__3;

			private int _003CoriginalUploadBufferSize_003E5__4;

			private UniTask<IList<IResourceLocation>>.Awaiter _003C_003Eu__1;

			private UniTask[] _003Ctasks_003E5__5;

			private int _003CtotalTaskCount_003E5__6;

			private UniTask.Awaiter _003C_003Eu__2;

			private void MoveNext()
			{
			}

			void IAsyncStateMachine.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				this.MoveNext();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
			}

			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
				this.SetStateMachine(stateMachine);
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CReleaseAsync_003Ed__5 : IAsyncStateMachine
		{
			public int _003C_003E1__state;

			public AsyncUniTaskMethodBuilder _003C_003Et__builder;

			public string label;

			public AssetPreloader _003C_003E4__this;

			private UniTask<IList<IResourceLocation>>.Awaiter _003C_003Eu__1;

			private void MoveNext()
			{
			}

			void IAsyncStateMachine.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				this.MoveNext();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
			}

			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				//ILSpy generated this explicit interface implementation from .override directive in SetStateMachine
				this.SetStateMachine(stateMachine);
			}
		}

		private readonly Dictionary<Type, IAssetContainer> _assetContainers;

		public void AddContainer<T>() where T : UnityEngine.Object
		{
		}

		[AsyncStateMachine(typeof(_003CEnsureAsync_003Ed__3))]
		public UniTask EnsureAsync(string label, ReactiveProperty<float> progress = null)
		{
			return default;
		}

		[AsyncStateMachine(typeof(_003CEnsureAssetWithLogAsync_003Ed__4))]
		private UniTask EnsureAssetWithLogAsync(Type type, IAssetContainer container, string label, IList<IResourceLocation> locations, Stopwatch stopwatch)
		{
			return default;
		}

		[AsyncStateMachine(typeof(_003CReleaseAsync_003Ed__5))]
		public UniTask ReleaseAsync(string label)
		{
			return default;
		}

		private void OnAtlasRequested(string tag, Action<SpriteAtlas> callback)
		{
		}

		public T Get<T>(string key) where T : class
		{
			return null;
		}
	}
}
