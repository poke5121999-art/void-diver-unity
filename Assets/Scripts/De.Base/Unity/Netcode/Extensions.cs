using System;
using System.Collections.Generic;

namespace Unity.Netcode
{
	public static class Extensions
	{
		public static T[] ToArray<T>(this NetworkList<T> networkList) where T : struct, IEquatable<T>
		{
			return null;
		}

		public static void AddRange<T>(this NetworkList<T> networkList, IEnumerable<T> items) where T : struct, IEquatable<T>
		{
		}

		public static void AddRange<T>(this List<T> list, NetworkList<T> items) where T : struct, IEquatable<T>
		{
		}

		public static void AssertIfNotHost(this NetworkManager networkManager)
		{
		}
	}
}
