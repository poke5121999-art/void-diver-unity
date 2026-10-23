using System;
using System.Linq;
using System.Collections.Generic;
using ProtoBuf;

[ProtoContract]
public class PlayerStore
{
	[ProtoIgnore]
	private readonly Dictionary<string, string> _datas = new Dictionary<string, string>();

	[ProtoIgnore]
	private readonly Dictionary<string, string> _tempDatas = new Dictionary<string, string>();

	[ProtoMember(1)]
	public int Id { get; set; }

	[ProtoMember(2)]
	public PlayerStoreEntry[] Entries
	{
		get
		{
			return _datas.Select((KeyValuePair<string, string> kv) => new PlayerStoreEntry
			{
				Key = kv.Key,
				Value = kv.Value
			}).ToArray();
		}
		set
		{
			_datas.Clear();
			value?.ForEach(delegate(PlayerStoreEntry e)
			{
				_datas.Add(e.Key, e.Value);
			});
		}
	}

	public string GetValue(string key)
	{
		if (_datas.TryGetValue(key, out var value))
		{
			return value;
		}
		if (_tempDatas.TryGetValue(key, out value))
		{
			return value;
		}
		return null;
	}

	public void SetValue(string key, string value)
	{
		_datas[key] = value;
	}

	public void SetTempValue(string key, string value)
	{
		_tempDatas[key] = value;
	}

	public void Remove(string key)
	{
		if (_datas.ContainsKey(key))
		{
			_datas.Remove(key);
		}
		if (_tempDatas.ContainsKey(key))
		{
			_tempDatas.Remove(key);
		}
	}

	public PlayerStore Clone()
	{
		return new PlayerStore
		{
			Id = Id,
			Entries = Entries
		};
	}
}
