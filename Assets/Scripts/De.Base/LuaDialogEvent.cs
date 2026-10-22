using GameCommon.Data;

public class LuaDialogEvent : IBaseEvent
{
	public string LuaKey { get; private set; }

	public string FunctionName { get; private set; }

	public LuaDialogEvent(string luaKey, string functionName)
	{
	}

	public long CalculateProgress(EventCondition eventCondition, long currentProgress, long goal)
	{
		return 0L;
	}
}
