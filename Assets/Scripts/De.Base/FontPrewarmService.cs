using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using GameCommon.Enum;
using TMPro;
using VContainer;

public class FontPrewarmService : NativeBase, IDisposable
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003CAddCharactersChunked_003Ed__13 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public HashSet<char> chars;

		public TMP_FontAsset font;

		public CancellationToken ct;

		private char[] _003Cbuffer_003E5__2;

		private int _003Ccount_003E5__3;

		private HashSet<char>.Enumerator _003C_003E7__wrap3;

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
	private struct _003CPrewarmAsync_003Ed__6 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncUniTaskMethodBuilder _003C_003Et__builder;

		public FontPrewarmService _003C_003E4__this;

		private ELanguage _003Clanguage_003E5__2;

		private CancellationToken _003Ctoken_003E5__3;

		private List<TMP_FontAsset> _003CdynamicFonts_003E5__4;

		private HashSet<char> _003CtargetChars_003E5__5;

		private Stopwatch _003Cstopwatch_003E5__6;

		private List<TMP_FontAsset>.Enumerator _003C_003E7__wrap6;

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

	private const int CHUNK_SIZE = 256;

	[Inject]
	private FontLocalizationService _fontService;

	[Inject]
	private FontLocalizationSettings _settings;

	[Inject]
	private GamePlaySettingService _gamePlaySettings;

	private ELanguage _prewarmedLanguage;

	private CancellationTokenSource _cts;

	[AsyncStateMachine(typeof(_003CPrewarmAsync_003Ed__6))]
	public UniTask PrewarmAsync()
	{
		return default;
	}

	public void Dispose()
	{
	}

	private HashSet<char> CollectUsedCharacters()
	{
		return null;
	}

	private List<TMP_FontAsset> CollectDynamicFonts()
	{
		return null;
	}

	private HashSet<char> CollectDynamicTargetChars(ELanguage language)
	{
		return null;
	}

	private List<TMP_FontAsset> CollectStaticFonts(ELanguage language)
	{
		return null;
	}

	private void CollectChain(TMP_FontAsset font, List<TMP_FontAsset> chain, HashSet<TMP_FontAsset> visited)
	{
	}

	[AsyncStateMachine(typeof(_003CAddCharactersChunked_003Ed__13))]
	private UniTask AddCharactersChunked(TMP_FontAsset font, HashSet<char> chars, CancellationToken ct)
	{
		return default;
	}
}
