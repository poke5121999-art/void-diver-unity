using System.Net;
using System.Net.Sockets;

namespace SomeNet.Utility
{
	public static class EndPointHelper
	{
		public struct AddressPort
		{
			public string Address;

			public int Port;
		}

		public static IPAddress GetLocalAddress(AddressFamily preferableAddressFamily)
		{
			return null;
		}

		public static IPAddress GetAddress(string hostName, AddressFamily preferableAddressFamily = AddressFamily.Unknown)
		{
			return null;
		}

		public static string GetExternalAddress()
		{
			return null;
		}

		public static AddressPort ParseEndPoint(string endPoint)
		{
			return default;
		}

		public static IPEndPoint ParseIPEndPoint(string endPoint, AddressFamily highPriorityAddressFamily = AddressFamily.InterNetwork)
		{
			return null;
		}
	}
}
