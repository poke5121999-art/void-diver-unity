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
			_model = model;
		}

		public void Serialize(Stream dest, object value)
		{
			_model.Serialize(dest, value);
		}

		public object Deserialize(Stream source, object value, Type type)
		{
			return _model.Deserialize(source, value, type);
		}

		public object Deserialize(Stream source, object value, Type type, int length)
		{
			return _model.Deserialize(source, value, type, length);
		}
	}
}
