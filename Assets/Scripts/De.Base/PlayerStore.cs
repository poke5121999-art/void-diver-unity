using System.Collections.Generic;
using ProtoBuf;

[ProtoContract]
public class PlayerStore
{
	[ProtoIgnore]
	private readonly Dictionary<string, string> _datas;

	[ProtoIgnore]
	private readonly Dictionary<string, string> _tempDatas;

	[ProtoMember(1)]
	public int Id { get; set; }

	[ProtoMember(2)]
	public PlayerStoreEntry[] Entries
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public string GetValue(string key)
	{
		return null;
	}

	public void SetValue(string key, string value)
	{
	}

	public void SetTempValue(string key, string value)
	{
	}

	public void Remove(string key)
	{
	}

	public PlayerStore Clone()
	{
		return null;
	}
}
