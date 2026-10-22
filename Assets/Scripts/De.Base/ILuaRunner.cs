public interface ILuaRunner
{
	void RunLua(string luaKey, string functionName, params object[] args);

	void LoadLuaScript();

	void StopAllScripts();
}
