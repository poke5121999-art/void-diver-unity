using ProtoBuf;

namespace GameCommon.Data
{
	[ProtoContract]
	public class AppVersion
	{
		[ProtoMember(1)]
		public int Major { get; set; }

		[ProtoMember(2)]
		public int Minor { get; set; }

		[ProtoMember(3)]
		public int Patch { get; set; }

		[ProtoMember(4)]
		public int Revision { get; set; }

		public AppVersion()
		{
		}

		public AppVersion(int major, int minor, int patch, int revision)
		{
		}

		private int CompareTo(AppVersion other)
		{
			return 0;
		}

		public bool Equals(AppVersion other)
		{
			return false;
		}

		public override bool Equals(object obj)
		{
			return false;
		}

		public override int GetHashCode()
		{
			return 0;
		}

		public static bool operator ==(AppVersion left, AppVersion right)
		{
			return false;
		}

		public static bool operator !=(AppVersion left, AppVersion right)
		{
			return false;
		}

		public static bool operator <(AppVersion left, AppVersion right)
		{
			return false;
		}

		public static bool operator >(AppVersion left, AppVersion right)
		{
			return false;
		}

		public static bool operator <=(AppVersion left, AppVersion right)
		{
			return false;
		}

		public static bool operator >=(AppVersion left, AppVersion right)
		{
			return false;
		}

		public override string ToString()
		{
			return null;
		}

		public AppVersion Clone()
		{
			return null;
		}
	}
}
