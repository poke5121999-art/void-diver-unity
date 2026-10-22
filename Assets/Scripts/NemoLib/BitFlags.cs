using Newtonsoft.Json;
using ProtoBuf;

[ProtoContract]
public class BitFlags
{
	private ulong[] _flags;

	[JsonProperty]
	[ProtoMember(1)]
	public ulong[] Flags
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public BitFlags()
	{
	}

	public BitFlags(int size)
	{
	}

	public void Set(int index)
	{
	}

	public void ClearIndex(int index)
	{
	}

	public bool Toggle(int index)
	{
		return false;
	}

	public bool IsSet(int index)
	{
		return false;
	}

	public void Resize(int newSize)
	{
	}

	public void Clear()
	{
	}

	public BitFlags Clone()
	{
		return null;
	}

	public int[] GetAddedBitFlags(BitFlags newBitFlags)
	{
		return null;
	}
}
