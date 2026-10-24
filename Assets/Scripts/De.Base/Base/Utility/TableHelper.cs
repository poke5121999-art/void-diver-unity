using System;
using System.IO;
using Cysharp.Threading.Tasks;
using GameCommon.Data;
using GameCommon.Table;
using GameCommon.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace Base.Utility
{
	public static class TableHelper
	{
		// Const.Encrypt.TABLE_KEY lives in De.Const, outside this assembly's reach; the shipped tables are XOR 0xCC.
		private const byte TableKey = 234; // Const.Encrypt.TABLE_KEY; EncryptUtility.GetKey makes 234 ^ 18 ^ 52 = 0xCC, the key the shipped tables decrypt with (harness TableTests)

		public static readonly JsonConverter[] JsonConverters = new JsonConverter[]
		{
			new GoodsDataJsonConverter(),
			new GoodsDataJsonArrayConverter(),
			new FixedSectorConverter(),
			new FixedSectorArrayConverter(),
			new ContentUnlockConditionJsonConverter(),
			new ContentUnlockConditionJsonArrayConverter(),
			new Vector2JsonConverter(),
			new Vector2JsonArrayConverter(),
			new Vector3JsonConverter(),
			new Vector3JsonArrayConverter(),
			new StringEnumConverter(),
			new StatJsonConverter(),
			new StatJsonArrayConverter(),
			new TalentEffectJsonConverter(),
			new TalentEffectJsonArrayConverter(),
			new ArtifactDealCardEffectJsonConverter(),
			new ArtifactDealCardEffectJsonArrayConverter(),
			new InteriorJsonConverter(),
			new InteriorJsonArrayConverter(),
		};

		// Application.streamingAssetsPath is unavailable outside the Unity player, so tools fall back to the project folder.
		private static string StreamingAssetsPath()
		{
			try
			{
				return Application.streamingAssetsPath;
			}
			catch (Exception)
			{
				return Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets");
			}
		}

		public static void LoadTables()
		{
			SomeNet.Utility.JsonUtility.AddConverter(JsonConverters);
			TableUtility.LoadTables(StreamingAssetsPath() + "/Table");
			TableUtility.LoadTables(StreamingAssetsPath() + "/TableByTool");
		}

		public static void LoadEncryptedTables()
		{
			SomeNet.Utility.JsonUtility.AddConverter(JsonConverters);
			TableUtility.LoadEncryptedTables(StreamingAssetsPath() + "/TableEncrypted", TableKey);
		}

		public static async UniTask LoadEncryptedTablesAsync()
		{
			await UniTask.RunOnThreadPool(LoadEncryptedTables);
		}

		public static void LoadLocalizedTables()
		{
			string directory = StreamingAssetsPath() + "/TableEncrypted/";
			LoadDecryptedTable(typeof(TCharacter), directory + "Character.bytes", TableKey);
			LoadDecryptedTable(typeof(TDialogReplay), directory + "DialogReplay.bytes", TableKey);
			LoadDecryptedTable(typeof(TEmployee), directory + "Employee.bytes", TableKey);
			LoadDecryptedTable(typeof(TArtifactDeal), directory + "ArtifactDeal.bytes", TableKey);
		}

		public static void LoadTable(Type dataType, string filePath)
		{
			SomeNet.Utility.JsonUtility.AddConverter(JsonConverters);
			TableUtility.LoadTable(dataType, filePath);
		}

		public static void LoadDecryptedTable(Type dataType, string filePath, byte key)
		{
			SomeNet.Utility.JsonUtility.AddConverter(JsonConverters);
			TableUtility.LoadDecryptedTable(dataType, filePath, key);
		}
	}
}
