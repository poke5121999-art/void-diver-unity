using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MoonSharp.Interpreter;
using NemoLib.Log;
using VContainer;

public class LuaRunner : MonoBase, ILuaRunner
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003C_003Ec__DisplayClass9_0
	{
		public DynValue coroutine;

		public LuaRunner _003C_003E4__this;

		public string luaKey;

		public string functionName;
	}

	[CompilerGenerated]
	private sealed class _003CRunLuaCoroutine_003Ed__9 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public DynValue coroutine;

		public LuaRunner _003C_003E4__this;

		public string luaKey;

		public string functionName;

		public DynValue[] args;

		private _003C_003Ec__DisplayClass9_0 _003C_003E8__1;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[DebuggerHidden]
		public _003CRunLuaCoroutine_003Ed__9(int _003C_003E1__state)
		{
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	[Inject]
	private SeriLogger _logger;

	private readonly Dictionary<string, Script> _luaScripts;

	private DynValue _dynLuaApi;

	protected override void Awake()
	{
	}

	private void Start()
	{
	}

	public void LoadLuaScript()
	{
	}

	public void LoadEncryptLuaScript()
	{
	}

	public void RunLua(string luaKey, string functionName, params object[] args)
	{
	}

	public void StopAllScripts()
	{
	}

	[IteratorStateMachine(typeof(_003CRunLuaCoroutine_003Ed__9))]
	private IEnumerator RunLuaCoroutine(DynValue coroutine, DynValue[] args, string luaKey, string functionName)
	{
		return null;
	}
}
