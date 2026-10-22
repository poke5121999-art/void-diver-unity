using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Netcode
{
	public static class Extensions
	{
		public static T[] ToArray<T>(this NetworkList<T> networkList) where T : unmanaged, IEquatable<T>
		{
			var result = new T[networkList.Count];
			for (int i = 0; i < result.Length; i++)
			{
				result[i] = networkList[i];
			}
			return result;
		}

		public static void AddRange<T>(this NetworkList<T> networkList, IEnumerable<T> items) where T : unmanaged, IEquatable<T>
		{
			foreach (T item in items)
			{
				networkList.Add(item);
			}
		}

		public static void AddRange<T>(this List<T> list, NetworkList<T> items) where T : unmanaged, IEquatable<T>
		{
			for (int i = 0; i < items.Count; i++)
			{
				list.Add(items[i]);
			}
		}

		public static void AssertIfNotHost(this NetworkManager networkManager)
		{
			Debug.Assert(networkManager != null && networkManager.IsHost, "network manager is not host");
		}
	}
}
