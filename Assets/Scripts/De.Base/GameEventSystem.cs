using UniRx;

public class GameEventSystem : NativeBase
{
	public readonly Subject<IBaseEvent> OnEventOccured = new Subject<IBaseEvent>();

	public void Trigger(IBaseEvent baseEvent)
	{
		OnEventOccured.OnNext(baseEvent);
	}
}
