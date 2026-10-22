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
			return _flags;
		}
		set
		{
			_flags = value;
		}
	}

	public BitFlags()
	{
		_flags = System.Array.Empty<ulong>();
	}

	public BitFlags(int size)
	{
		_flags = new ulong[(size + 63) / 64];
	}

	public void Set(int index)
	{
		int word = index / 64;
		if (word >= _flags.Length)
		{
			Resize(word * 64 + 64);
		}
		_flags[word] |= 1UL << (index & 63);
	}

	public void ClearIndex(int index)
	{
		int word = index / 64;
		_flags[word] &= ~(1UL << (index % 64));
	}

	public bool Toggle(int index)
	{
		int word = index / 64;
		if (word < _flags.Length && (_flags[word] & (1UL << (index & 63))) != 0)
		{
			_flags[word] &= ~(1UL << (index & 63));
			return false;
		}
		if (word >= _flags.Length)
		{
			Resize(word * 64 + 64);
		}
		_flags[word] |= 1UL << (index & 63);
		return true;
	}

	public bool IsSet(int index)
	{
		int word = index / 64;
		if (word >= _flags.Length)
		{
			return false;
		}
		return (_flags[word] & (1UL << (index % 64))) != 0;
	}

	public void Resize(int newSize)
	{
		ulong[] array = new ulong[(newSize + 63) / 64];
		System.Array.Copy(_flags, array, System.Math.Min(_flags.Length, array.Length));
		_flags = array;
	}

	public void Clear()
	{
		for (int i = 0; i < _flags.Length; i++)
		{
			_flags[i] = 0UL;
		}
	}

	public BitFlags Clone()
	{
		BitFlags bitFlags = new BitFlags();
		bitFlags._flags = new ulong[_flags.Length];
		System.Array.Copy(_flags, bitFlags._flags, _flags.Length);
		return bitFlags;
	}

	public int[] GetAddedBitFlags(BitFlags newBitFlags)
	{
		if (newBitFlags == null)
		{
			return System.Array.Empty<int>();
		}
		var list = new System.Collections.Generic.List<int>();
		int oldLen = _flags.Length;
		int newLen = newBitFlags._flags.Length;
		int max = System.Math.Max(oldLen, newLen);
		for (int i = 0; i < max; i++)
		{
			ulong a = i < oldLen ? _flags[i] : 0UL;
			ulong b = i < newLen ? newBitFlags._flags[i] : 0UL;
			ulong added = (b ^ a) & b;
			if (added == 0UL)
			{
				continue;
			}
			for (int j = 0; j < 64; j++)
			{
				if (((added >> j) & 1UL) != 0)
				{
					list.Add(i * 64 + j);
				}
			}
		}
		return list.ToArray();
	}
}
