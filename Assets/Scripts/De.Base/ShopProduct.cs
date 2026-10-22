using ProtoBuf;
using Tracker;

[ProtoContract]
public class ShopProduct : ITrackableDictionaryData<int, ShopProduct>, ITrackableData<ShopProduct>
{
	public int Key => 0;

	[ProtoMember(1)]
	public EDataOperation Operation { get; set; }

	[ProtoMember(2)]
	public int Id { get; set; }

	[ProtoMember(3)]
	public int Stock { get; set; }

	[ProtoMember(4)]
	public int StockMax { get; set; }

	public ShopProduct Clone()
	{
		return null;
	}
}
