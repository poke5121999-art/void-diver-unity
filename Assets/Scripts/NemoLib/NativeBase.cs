using VContainer.Unity;

public abstract class NativeBase
{
	protected NativeBase()
	{
		this.InjectFromRootContainer();
	}
}
