using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MoonSharp.Interpreter;
using GameCommon.Enum;
using NemoLib.Log;
using NemoLib.Utility;
using UnityEngine;
using VContainer;

public class LuaRunner : MonoBase, ILuaRunner
{
	[Inject]
	private SeriLogger _logger;

	private readonly Dictionary<string, Script> _luaScripts = new Dictionary<string, Script>();

	private DynValue _dynLuaApi = new DynValue();

	protected override void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
		UserData.RegisterType<LuaApi>();
		UserData.RegisterType<TaskWrapper>();
		UserData.RegisterType<Vector3>();
		UserData.RegisterType<DialogSpeakerEntry>();
	}

	private void Start()
	{
		_dynLuaApi = UserData.Create(new LuaApi());
		LoadEncryptLuaScript();
	}

	public void LoadLuaScript()
	{
		_luaScripts.Clear();
		string text = Application.streamingAssetsPath + "/Lua";
		string[] array = (from _ in Directory.GetFiles(text, "*.lua", SearchOption.AllDirectories)
			select _.Replace("\\", "/")).ToArray();
		if (array.Length == 0)
		{
			return;
		}
		string text2 = string.Empty;
		string text3 = null;
		try
		{
			text3 = Path.Combine(text, "Common").Replace("\\", "/");
			if (!Directory.Exists(text3))
			{
				_logger.Warning("Common folder not found.");
			}
			else
			{
				string[] files = Directory.GetFiles(text3, "*.lua", SearchOption.AllDirectories);
				StringBuilder stringBuilder = new StringBuilder();
				string[] array2 = files;
				foreach (string text4 in array2)
				{
					try
					{
						stringBuilder.AppendLine(File.ReadAllText(text4));
						stringBuilder.AppendLine();
					}
					catch (Exception arg)
					{
						_logger.Warning(string.Format("Failed to read common file: {0}\n{1}", text4, arg));
					}
				}
				text2 = stringBuilder.ToString();
			}
		}
		catch (Exception arg2)
		{
			_logger.Error(string.Format("Common scripts\n{0}", arg2));
		}
		string[] array3 = array;
		foreach (string text5 in array3)
		{
			if (text5.StartsWith(text3))
			{
				continue;
			}
			string text6;
			try
			{
				text6 = File.ReadAllText(text5);
			}
			catch (Exception arg3)
			{
				_logger.Error(string.Format("Failed to read lua file: {0}\n{1}", text5, arg3));
				continue;
			}
			try
			{
				Script script = new Script();
				script.Globals["LuaApi"] = _dynLuaApi;
				script.Globals["DialogSpeakerEntry"] = (Func<ESpeaker, int, string, DialogSpeakerEntry>)((ESpeaker speaker, int tableId, string imageName) => new DialogSpeakerEntry(speaker, tableId, imageName));
				if (text2.HasValue())
				{
					script.DoString(text2);
				}
				script.DoString(text6);
				string key = Path.GetRelativePath(text, text5).Replace("\\", "/").Replace(".lua", "");
				_luaScripts.Add(key, script);
			}
			catch (Exception arg4)
			{
				_logger.Error(string.Format("{0}.lua\n{1}", Path.GetFileNameWithoutExtension(text5), arg4));
			}
		}
	}

	public void LoadEncryptLuaScript()
	{
		_luaScripts.Clear();
		string text = Application.streamingAssetsPath + "/LuaEncrypted";
		string[] array = (from _ in Directory.GetFiles(text, "*.bytes", SearchOption.AllDirectories)
			select _.Replace("\\", "/")).ToArray();
		if (array.Length == 0)
		{
			return;
		}
		string text2 = string.Empty;
		string text3 = null;
		try
		{
			text3 = Path.Combine(text, "Common").Replace("\\", "/");
			if (!Directory.Exists(text3))
			{
				_logger.Warning("Common folder not found.");
			}
			else
			{
				string[] files = Directory.GetFiles(text3, "*.bytes", SearchOption.AllDirectories);
				StringBuilder stringBuilder = new StringBuilder();
				string[] array2 = files;
				foreach (string text4 in array2)
				{
					try
					{
						stringBuilder.AppendLine(EncryptUtility.DecryptFile(text4, Const.Encrypt.LUA_KEY));
						stringBuilder.AppendLine();
					}
					catch (Exception arg)
					{
						_logger.Warning(string.Format("Failed to read common file: {0}\n{1}", text4, arg));
					}
				}
				text2 = stringBuilder.ToString();
			}
		}
		catch (Exception arg2)
		{
			_logger.Error(string.Format("Common scripts\n{0}", arg2));
		}
		string[] array3 = array;
		foreach (string text5 in array3)
		{
			if (text5.StartsWith(text3))
			{
				continue;
			}
			string text6;
			try
			{
				text6 = EncryptUtility.DecryptFile(text5, Const.Encrypt.LUA_KEY);
			}
			catch (Exception arg3)
			{
				_logger.Error(string.Format("Failed to decrypt lua file: {0}\n{1}", text5, arg3));
				continue;
			}
			try
			{
				Script script = new Script();
				script.Globals["LuaApi"] = _dynLuaApi;
				script.Globals["DialogSpeakerEntry"] = (Func<ESpeaker, int, string, DialogSpeakerEntry>)((ESpeaker speaker, int tableId, string imageName) => new DialogSpeakerEntry(speaker, tableId, imageName));
				if (text2.HasValue())
				{
					script.DoString(text2);
				}
				script.DoString(text6);
				string key = Path.GetRelativePath(text, text5).Replace("\\", "/").Replace(".bytes", "");
				_luaScripts.Add(key, script);
			}
			catch (Exception arg4)
			{
				_logger.Error(string.Format("{0}.bytes\n{1}", Path.GetFileNameWithoutExtension(text5), arg4));
			}
		}
	}

	public void RunLua(string luaKey, string functionName, params object[] args)
	{
		if (string.IsNullOrEmpty(luaKey))
		{
			_logger.Error("luaName is null or empty");
			return;
		}
		if (!_luaScripts.TryGetValue(luaKey, out var luaScript))
		{
			_logger.Error(string.Concat("luaName(", luaKey, ") not found"));
			return;
		}
		DynValue dynValue = luaScript.Globals.Get(functionName);
		if (dynValue.Type != DataType.Function)
		{
			_logger.Error(string.Concat("[", luaKey, ".lua] function(", functionName, ") not found"));
			return;
		}
		DynValue[] args2 = ((args != null && args.Any()) ? args.Select((object arg) => DynValue.FromObject(luaScript, arg)).ToArray() : null);
		DynValue coroutine = luaScript.CreateCoroutine(dynValue.Function);
		StartCoroutine(RunLuaCoroutine(coroutine, args2, luaKey, functionName));
	}

	public void StopAllScripts()
	{
		StopAllCoroutines();
	}

	private IEnumerator RunLuaCoroutine(DynValue coroutine, DynValue[] args, string luaKey, string functionName)
	{
		bool SafeResume(DynValue[] resumeArgs, out DynValue resumeResult)
		{
			resumeResult = null;
			try
			{
				resumeResult = ((resumeArgs == null || resumeArgs.Length < 1) ? coroutine.Coroutine.Resume() : coroutine.Coroutine.Resume(resumeArgs));
				return true;
			}
			catch (InterpreterException ex)
			{
				_logger.Error(string.Format("[{0}.lua:{1}] {2}", luaKey, functionName, ex.DecoratedMessage ?? (object)ex));
				return false;
			}
			catch (Exception ex2)
			{
				_logger.Error(string.Concat("[", luaKey, ".lua:", functionName, "] ", ex2.Message));
				return false;
			}
		}
		if (!SafeResume(args, out var result))
		{
			yield break;
		}
		while (true)
		{
			if (result.Type != DataType.YieldRequest && coroutine.Coroutine.State == CoroutineState.Dead)
			{
				yield break;
			}
			yield return null;
			if (!SafeResume(null, out result))
			{
				yield break;
			}
		}
	}
}
