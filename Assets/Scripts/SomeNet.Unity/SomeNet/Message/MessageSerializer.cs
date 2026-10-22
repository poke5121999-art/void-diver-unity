using System;
using System.IO;
using ProtoBuf.Meta;

namespace SomeNet.Message
{
	public class MessageSerializer
	{
		private readonly TypeModel _model;

		public MessageSerializer(TypeModel model)
		{
		}

		public void Serialize(Stream dest, object value)
		{
		}

		public object Deserialize(Stream source, object value, Type type)
		{
			return null;
		}

		public object Deserialize(Stream source, object value, Type type, int length)
		{
			return null;
		}
	}
}
