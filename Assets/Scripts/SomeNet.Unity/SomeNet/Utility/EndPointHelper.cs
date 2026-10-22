using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

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
			IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
			IPAddress address = PickAddress(host, preferableAddressFamily);
			if (address.Equals(IPAddress.None))
			{
				address = (preferableAddressFamily == AddressFamily.InterNetwork) ? IPAddress.Loopback : IPAddress.IPv6Loopback;
			}
			return address;
		}

		public static IPAddress GetAddress(string hostName, AddressFamily preferableAddressFamily = AddressFamily.Unknown)
		{
			return PickAddress(Dns.GetHostEntry(hostName), preferableAddressFamily);
		}

		// First address of the preferred family, otherwise the first address, otherwise IPAddress.None.
		private static IPAddress PickAddress(IPHostEntry host, AddressFamily preferableAddressFamily)
		{
			IPAddress[] list = host.AddressList;
			if (preferableAddressFamily != AddressFamily.Unknown)
			{
				for (int i = 0; i < list.Length; i++)
				{
					if (list[i].AddressFamily == preferableAddressFamily)
					{
						return list[i];
					}
				}
			}
			return (list.Length != 0) ? list[0] : IPAddress.None;
		}

		public static string GetExternalAddress()
		{
			WebClient client = new WebClient();
			string page = client.DownloadString("http://checkip.dyndns.org/");
			Regex regex = new Regex("\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}");
			return regex.Matches(page)[0].ToString();
		}

		public static AddressPort ParseEndPoint(string endPoint)
		{
			AddressPort result = default(AddressPort);
			int index = endPoint.LastIndexOf(':');
			if (index != -1)
			{
				string address = endPoint.Substring(0, index);
				string portText = endPoint.Substring(index + 1);
				if (!int.TryParse(portText, System.Globalization.NumberStyles.None, System.Globalization.NumberFormatInfo.InvariantInfo, out int port))
				{
					throw new FormatException("Invalid port");
				}
				result.Address = address;
				result.Port = port;
			}
			else
			{
				result.Address = endPoint;
				result.Port = 0;
			}
			return result;
		}

		public static IPEndPoint ParseIPEndPoint(string endPoint, AddressFamily highPriorityAddressFamily = AddressFamily.InterNetwork)
		{
			AddressPort parsed = ParseEndPoint(endPoint);
			IPAddress address;
			if (!IPAddress.TryParse(parsed.Address, out address))
			{
				IPAddress[] candidates = Dns.GetHostAddresses(parsed.Address);
				address = candidates.Where(a => a.AddressFamily == highPriorityAddressFamily).FirstOrDefault();
				if (address == null)
				{
					address = candidates.Where(a => a.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();
					if (address == null)
					{
						throw new FormatException("Invalid address");
					}
				}
			}
			return new IPEndPoint(address, parsed.Port);
		}
	}
}
