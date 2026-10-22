using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SomeNet.TypeAlias
{
	public static class TypeUtility
	{
		public struct TypeAndAttribute<TAttribute>
		{
			public Type Type;

			public TAttribute Attribute;
		}

		[CompilerGenerated]
		private sealed class _003CGetTypes_003Ed__7 : IEnumerable<Type>, IEnumerable, IEnumerator<Type>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private Type _003C_003E2__current;

			private int _003C_003El__initialThreadId;

			private Assembly assembly;

			public Assembly _003C_003E3__assembly;

			private Type[] _003CtypesInAsm_003E5__1;

			private Type[] _003CtypeArray_003E5__2;

			private ReflectionTypeLoadException _003Cex_003E5__3;

			private int _003Cindex_003E5__4;

			private Type _003Ctype_003E5__5;

			Type IEnumerator<Type>.Current
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
			public _003CGetTypes_003Ed__7(int _003C_003E1__state)
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
			IEnumerator<Type> IEnumerable<Type>.GetEnumerator()
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
		private sealed class _003CGetTypesContainingAttribute_003Ed__8<TAttribute> : IEnumerable<TypeAndAttribute<TAttribute>>, IEnumerable, IEnumerator<TypeAndAttribute<TAttribute>>, IEnumerator, IDisposable where TAttribute : Attribute
		{
			private int _003C_003E1__state;

			private TypeAndAttribute<TAttribute> _003C_003E2__current;

			private int _003C_003El__initialThreadId;

			private Assembly assembly;

			public Assembly _003C_003E3__assembly;

			private IEnumerator<Type> _003C_003Es__1;

			private Type _003Ctype_003E5__2;

			private TAttribute _003Cattr_003E5__3;

			TypeAndAttribute<TAttribute> IEnumerator<TypeAndAttribute<TAttribute>>.Current
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
			public _003CGetTypesContainingAttribute_003Ed__8(int _003C_003E1__state)
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
			IEnumerator<TypeAndAttribute<TAttribute>> IEnumerable<TypeAndAttribute<TAttribute>>.GetEnumerator()
			{
				return null;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return null;
			}
		}

		private static ConcurrentDictionary<string, Type> _typeCacheMap;

		public static IAssemblyProvider AssemblyProvider { get; set; }

		public static Type GetType(string typeName)
		{
			return null;
		}

		public static Assembly GetContainingAssembly(Type type)
		{
			return null;
		}

		[IteratorStateMachine(typeof(_003CGetTypes_003Ed__7))]
		public static IEnumerable<Type> GetTypes(Assembly assembly)
		{
			return null;
		}

		[IteratorStateMachine(typeof(_003CGetTypesContainingAttribute_003Ed__8<>))]
		public static IEnumerable<TypeAndAttribute<TAttribute>> GetTypesContainingAttribute<TAttribute>(Assembly assembly) where TAttribute : Attribute
		{
			return null;
		}

		public static IEnumerable<MethodInfo> GetExtensionMethods(Assembly assembly, Type extendedType)
		{
			return null;
		}

		public static MethodInfo GetExtensionMethod(Assembly assembly, Type extendedType, string methodName)
		{
			return null;
		}
	}
}
