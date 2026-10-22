using System;
using System.Threading.Tasks;
using ProtoBuf.Meta;
using SomeNet.Message;
using SomeNet.TypeAlias;

namespace SomeNet.Http
{
	public abstract class HttpChannel
	{
		private string url;

		private PacketSerializer serializer;

		private bool useRequestIncrement;

		private int requestId;

		protected abstract TypeModel TypeModel { get; }

		protected abstract TypeAliasTable TypeTable { get; }

		public HttpChannel(string url, bool useRequestIncrement = false)
		{
			this.url = url;
			this.useRequestIncrement = useRequestIncrement;
			serializer = new PacketSerializer(TypeModel, TypeTable);
		}

		protected abstract void OnException(Packet packet, Exception e);

		public Task<TResponse> PostAsync<TResponse>(MessageBase message, (string key, string value)[] headers = null) where TResponse : MessageBase
		{
			// TODO(decomp): generic method has no native body in GameAssembly (no caller instantiates it), so the
			// async state machine (<PostAsync>d__10<TResponse>.MoveNext) and the lambda <>c__10<TResponse>.<PostAsync>b__10_0
			// could not be read. Fields seen on the state machine: packet, attribute (RequestAttribute), byteArray, requestUri,
			// HttpWebRequest, reply Packet, MemoryStream, reqStream, WebResponse, respStream, exception. @0x0
			throw new NotImplementedException();
		}
	}
}
