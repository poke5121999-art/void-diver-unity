using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace NemoLib.Collections
{
	public class MultiKeyDictionary<K1, K2, V> : Dictionary<KeyTuple<K1, K2>, V>
	{
		[CompilerGenerated]
		private sealed class _003Cget_FirstKeys_003Ed__6 : IEnumerable<K1>, IEnumerable, IEnumerator<K1>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private K1 _003C_003E2__current;

			private int _003C_003El__initialThreadId;

			public MultiKeyDictionary<K1, K2, V> _003C_003E4__this;

			private HashSet<K1> _003CkeySet_003E5__2;

			private Enumerator _003C_003E7__wrap2;

			K1 IEnumerator<K1>.Current
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
			public _003Cget_FirstKeys_003Ed__6(int _003C_003E1__state)
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

			private void _003C_003Em__Finally1()
			{
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
			}

			[DebuggerHidden]
			IEnumerator<K1> IEnumerable<K1>.GetEnumerator()
			{
				return null;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return null;
			}
		}

		[CompilerGenerated]
		private sealed class _003Cget_KeyValuePairs_003Ed__4 : IEnumerable<MultiKeyValuePair<K1, K2, V>>, IEnumerable, IEnumerator<MultiKeyValuePair<K1, K2, V>>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private MultiKeyValuePair<K1, K2, V> _003C_003E2__current;

			private int _003C_003El__initialThreadId;

			public MultiKeyDictionary<K1, K2, V> _003C_003E4__this;

			private Enumerator _003C_003E7__wrap1;

			MultiKeyValuePair<K1, K2, V> IEnumerator<MultiKeyValuePair<K1, K2, V>>.Current
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
			public _003Cget_KeyValuePairs_003Ed__4(int _003C_003E1__state)
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

			private void _003C_003Em__Finally1()
			{
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
			}

			[DebuggerHidden]
			IEnumerator<MultiKeyValuePair<K1, K2, V>> IEnumerable<MultiKeyValuePair<K1, K2, V>>.GetEnumerator()
			{
				return null;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return null;
			}
		}

		public V this[K1 key1, K2 key2]
		{
			get
			{
				return default;
			}
			set
			{
			}
		}

		public IEnumerable<MultiKeyValuePair<K1, K2, V>> KeyValuePairs
		{
			[IteratorStateMachine(typeof(MultiKeyDictionary<, , >._003Cget_KeyValuePairs_003Ed__4))]
			get
			{
				return null;
			}
		}

		public IEnumerable<K1> FirstKeys
		{
			[IteratorStateMachine(typeof(MultiKeyDictionary<, , >._003Cget_FirstKeys_003Ed__6))]
			get
			{
				return null;
			}
		}

		public void Add(K1 key1, K2 key2, V value)
		{
		}

		public bool ContainsKey(K1 key1, K2 key2)
		{
			return false;
		}

		public bool TryGetValue(K1 key1, K2 key2, out V value)
		{
			value = default;
			return false;
		}
	}
}
