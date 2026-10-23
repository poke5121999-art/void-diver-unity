using System.Net;
using System.Text.RegularExpressions;

namespace GameCommon.Utility
{
	public static class NetworkUtility
	{
		private const string HostnamePattern = "^([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,}$";

		public static bool ValidateAddress(string address, string portStr)
		{
			if (!IPAddress.TryParse(address, out _) && !Regex.IsMatch(address, HostnamePattern))
			{
				return false;
			}
			if (!int.TryParse(portStr, out int port) || port < 1)
			{
				return false;
			}
			return port < 65536;
		}

		public static bool IsValidAddress(string address)
		{
			if (IPAddress.TryParse(address, out _))
			{
				return true;
			}
			return Regex.IsMatch(address, HostnamePattern);
		}

		public static bool IsValidPort(string portStr)
		{
			if (!int.TryParse(portStr, out int port) || port < 1)
			{
				return false;
			}
			return port < 65536;
		}
	}
}
