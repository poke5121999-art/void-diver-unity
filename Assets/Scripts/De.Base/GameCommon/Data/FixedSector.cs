using System;
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
			IndexX = 0;
			IndexZ = 0;
			SectorId = 0;
			Layer = 0;
		}

		public static FixedSector Empty()
		{
			return default;
		}

		public override string ToString()
		{
			return null;
		}

		public static bool TryParse(string str, out FixedSector fixedSector)
		{
			fixedSector = default;
			return false;
		}
	}
}
