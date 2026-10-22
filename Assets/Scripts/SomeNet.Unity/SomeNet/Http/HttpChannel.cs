using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ProtoBuf.Meta;
using SomeNet.Message;
using SomeNet.TypeAlias;

namespace SomeNet.Http
{
	public abstract class HttpChannel
	{
		[CompilerGenerated]
		private sealed class _003CPostAsync_003Ed__10<TResponse> : IAsyncStateMachine where TResponse : MessageBase
		{
			public int _003C_003E1__state;

			public AsyncTaskMethodBuilder<TResponse> _003C_003Et__builder;

			public MessageBase message;

			public (string key, string value)[] headers;

			public HttpChannel _003C_003E4__this;

			private Packet _003Cpacket_003E5__1;

			private RequestAttribute _003Cattribute_003E5__2;

			private byte[] _003CbyteArray_003E5__3;

			private string _003CrequestUri_003E5__4;

			private HttpWebRequest _003Crequest_003E5__5;

			private Packet _003Creply_003E5__6;

			private MemoryStream _003CmemoryStream_003E5__7;

			private int _003Ci_003E5__8;

			private Stream _003CreqStream_003E5__9;

			private Stream _003C_003Es__10;

			private WebResponse _003Cresponse_003E5__11;

			private WebResponse _003C_003Es__12;

			private Stream _003CrespStream_003E5__13;

			private MemoryStream _003CmemoryStream_003E5__14;

			private Exception _003Ce_003E5__15;

			private TaskAwaiter<Stream> _003C_003Eu__1;

			private TaskAwaiter<WebResponse> _003C_003Eu__2;

			private TaskAwaiter _003C_003Eu__3;

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

		private string url;

		private PacketSerializer serializer;

		private bool useRequestIncrement;

		private int requestId;

		protected abstract TypeModel TypeModel { get; }

		protected abstract TypeAliasTable TypeTable { get; }

		public HttpChannel(string url, bool useRequestIncrement = false)
		{
		}

		protected abstract void OnException(Packet packet, Exception e);

		[AsyncStateMachine(typeof(_003CPostAsync_003Ed__10<>))]
		[DebuggerStepThrough]
		public Task<TResponse> PostAsync<TResponse>(MessageBase message, (string key, string value)[] headers = null) where TResponse : MessageBase
		{
			return null;
		}
	}
}
