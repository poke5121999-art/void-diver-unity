using System;
using System.Globalization;
using ProtoBuf;

namespace GameCommon.Data
{
	[Serializable]
	[ProtoContract]
	public struct FixedSector
	{
		[ProtoMember(1)]
		public byte IndexX;

		[ProtoMember(2)]
		public byte IndexZ;

		[ProtoMember(3)]
		public int SectorId;

		[ProtoMember(4)]
		public int Layer;

		public FixedSector(byte indexX, byte indexZ, int sectorId, int layer = 0)
		{
			IndexX = indexX;
			IndexZ = indexZ;
			SectorId = sectorId;
			Layer = layer;
		}

		public static FixedSector Empty()
		{
			return default;
		}

		public override string ToString()
		{
			return $"{IndexX}:{IndexZ}:{SectorId}:{Layer}";
		}

		public static bool TryParse(string str, out FixedSector fixedSector)
		{
			fixedSector = default;
			if (string.IsNullOrEmpty(str))
			{
				return false;
			}
			string[] parts = str.Split(':');
			if (parts.Length < 3 || !byte.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out byte x) || !byte.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out byte z) || !int.TryParse(parts[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out int sectorId))
			{
				return false;
			}
			int layer = 0;
			if (parts.Length >= 4 && !int.TryParse(parts[3], NumberStyles.Integer, CultureInfo.InvariantCulture, out layer))
			{
				return false;
			}
			fixedSector = new FixedSector(x, z, sectorId, layer);
			return true;
		}
	}
}
