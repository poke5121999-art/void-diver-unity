using System;
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
			Major = major;
			Minor = minor;
			Patch = patch;
			Revision = revision;
		}

		private int CompareTo(AppVersion other)
		{
			if (other == null)
			{
				return 1;
			}
			int num = Major.CompareTo(other.Major);
			if (num != 0)
			{
				return num;
			}
			num = Minor.CompareTo(other.Minor);
			if (num != 0)
			{
				return num;
			}
			num = Patch.CompareTo(other.Patch);
			if (num != 0)
			{
				return num;
			}
			return Revision.CompareTo(other.Revision);
		}

		public bool Equals(AppVersion other)
		{
			if ((object)other != null && Major == other.Major && Minor == other.Minor && Patch == other.Patch)
			{
				return Revision == other.Revision;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj is AppVersion appVersion && Major == appVersion.Major && Minor == appVersion.Minor && Patch == appVersion.Patch)
			{
				return Revision == appVersion.Revision;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Major, Minor, Patch, Revision);
		}

		public static bool operator ==(AppVersion left, AppVersion right)
		{
			if ((object)left == right)
			{
				return true;
			}
			if ((object)left == null || (object)right == null)
			{
				return false;
			}
			if (left.Major == right.Major && left.Minor == right.Minor && left.Patch == right.Patch)
			{
				return left.Revision == right.Revision;
			}
			return false;
		}

		public static bool operator !=(AppVersion left, AppVersion right)
		{
			return !(left == right);
		}

		public static bool operator <(AppVersion left, AppVersion right)
		{
			if ((object)left == null)
			{
				return (object)right != null;
			}
			return left.CompareTo(right) < 0;
		}

		public static bool operator >(AppVersion left, AppVersion right)
		{
			if ((object)left == null)
			{
				return false;
			}
			return left.CompareTo(right) > 0;
		}

		public static bool operator <=(AppVersion left, AppVersion right)
		{
			if ((object)left == right)
			{
				return true;
			}
			if ((object)left == null)
			{
				return (object)right != null;
			}
			return left.CompareTo(right) <= 0;
		}

		public static bool operator >=(AppVersion left, AppVersion right)
		{
			if ((object)left == right)
			{
				return true;
			}
			if ((object)left == null)
			{
				return false;
			}
			return left.CompareTo(right) >= 0;
		}

		public override string ToString()
		{
			return string.Format("{0}.{1}.{2}.{3}", Major, Minor, Patch, Revision);
		}

		public AppVersion Clone()
		{
			return new AppVersion(Major, Minor, Patch, Revision);
		}
	}
}
