using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SomeNet.Table
{
	public class CsvTableLoader<TKey, TValue> : ITableLoader<TKey, TValue> where TKey : IComparable where TValue : class, ITableData<TKey>, new()
	{
		[CompilerGenerated]
		private sealed class _003CLoad_003Ed__2 : IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IEnumerator<KeyValuePair<TKey, TValue>>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private KeyValuePair<TKey, TValue> _003C_003E2__current;

			private int _003C_003El__initialThreadId;

			public CsvTableLoader<TKey, TValue> _003C_003E4__this;

			private string[][] _003Cdatas_003E5__1;

			private int _003Ci_003E5__2;

			private TValue _003Cvalue_003E5__3;

			KeyValuePair<TKey, TValue> IEnumerator<KeyValuePair<TKey, TValue>>.Current
			{
				[DebuggerHidden]
				get
				{
					return default;
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
			public _003CLoad_003Ed__2(int _003C_003E1__state)
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

			[DebuggerHidden]
			IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
			{
				return null;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return null;
			}
		}

		private string _csv;

		public CsvTableLoader(string csv)
		{
		}

		[IteratorStateMachine(typeof(CsvTableLoader<, >._003CLoad_003Ed__2))]
		public IEnumerable<KeyValuePair<TKey, TValue>> Load()
		{
			return null;
		}
	}
}
