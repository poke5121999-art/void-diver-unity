using UniRx;

public class GameEventSystem : NativeBase
{
	public readonly Subject<IBaseEvent> OnEventOccured;

	public void Trigger(IBaseEvent baseEvent)
	{
	}
}
