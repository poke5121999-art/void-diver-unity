using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Steamworks;

public static class UIInputUtility
{
	private static class BannedWordMatcher
	{
		private sealed class Node
		{
			public Dictionary<char, Node> Children;

			public Node Fail;

			public Node Output;

			public int WordLength;
		}

		private static Node _root;

		private static bool _dirty;

		public static void MarkDirty()
		{
		}

		public static bool ContainsMatch(string normalized)
		{
			return false;
		}

		public static void CollectMatches(string normalized, Action<int, int> onMatch)
		{
		}

		private static void EnsureBuilt()
		{
		}

		private static void Build()
		{
		}

		private static void BuildFailLinks(Node root)
		{
		}

		private static Node Advance(Node node, char c)
		{
			return null;
		}
	}

	private static Callback<GamepadTextInputDismissed_t> _gamepadTextInputCallback;

	private static Action<EPadInputResult, string> _gamepadTextInputDismissed;

	private const char MaskChar = '*';

	public static bool IsMouseOverUI()
	{
		return false;
	}

	public static UniTask<(EPadInputResult, string)> ShowGamepadTextInputAsync(string description, int maxChars, string existingText)
	{
		return default;
	}

	private static void ShowGamepadTextInput(string description, int maxChars, string existingText, Action<EPadInputResult, string> onDismissed)
	{
	}

	static UIInputUtility()
	{
	}

	public static bool ValidateName(string name)
	{
		return false;
	}

	public static bool ValidateText(string text)
	{
		return false;
	}

	public static string Mask(string input)
	{
		return null;
	}

	private static bool ContainsBannedWord(string input)
	{
		return false;
	}

	private static bool TryNormalize(string input, out string normalizedText, out List<int> normToOrigin)
	{
		normalizedText = null;
		normToOrigin = null;
		return false;
	}

	private static string NormalizeWord(string word)
	{
		return null;
	}

	private static bool IsMeaningful(char c)
	{
		return false;
	}
}
