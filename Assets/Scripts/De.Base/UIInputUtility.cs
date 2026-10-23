using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using GameCommon.Table;
using Steamworks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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

		private static bool _dirty = true;

		public static void MarkDirty()
		{
			_dirty = true;
		}

		public static bool ContainsMatch(string normalized)
		{
			EnsureBuilt();
			Node node = _root;
			for (int i = 0; i < normalized.Length; i++)
			{
				node = Advance(node, normalized[i]);
				if (node.WordLength > 0 || node.Output != null)
				{
					return true;
				}
			}
			return false;
		}

		public static void CollectMatches(string normalized, Action<int, int> onMatch)
		{
			EnsureBuilt();
			Node node = _root;
			for (int i = 0; i < normalized.Length; i++)
			{
				node = Advance(node, normalized[i]);
				for (Node match = node; match != null; match = match.Output)
				{
					if (match.WordLength > 0)
					{
						onMatch(i - match.WordLength + 1, i);
					}
				}
			}
		}

		private static void EnsureBuilt()
		{
			if (_dirty || _root == null)
			{
				Build();
				_dirty = false;
			}
		}

		private static void Build()
		{
			var root = new Node();
			if (TBannedWord.Table != null)
			{
				foreach (TBannedWord banned in TBannedWord.Table.Values)
				{
					string word = NormalizeWord(banned.Word);
					if (word.Length > 1)
					{
						Node node = root;
						foreach (char c in word)
						{
							if (node.Children == null)
							{
								node.Children = new Dictionary<char, Node>();
							}
							if (!node.Children.TryGetValue(c, out Node next))
							{
								next = new Node();
								node.Children.Add(c, next);
							}
							node = next;
						}
						node.WordLength = word.Length;
					}
				}
			}
			BuildFailLinks(root);
			_root = root;
		}

		private static void BuildFailLinks(Node root)
		{
			root.Fail = root;
			var queue = new Queue<Node>();
			if (root.Children != null)
			{
				foreach (Node child in root.Children.Values)
				{
					child.Fail = root;
					queue.Enqueue(child);
				}
			}
			while (queue.Count > 0)
			{
				Node node = queue.Dequeue();
				if (node.Children == null)
				{
					continue;
				}
				foreach (KeyValuePair<char, Node> pair in node.Children)
				{
					char c = pair.Key;
					Node child = pair.Value;
					Node fail = node.Fail;
					while (fail != root && (fail.Children == null || !fail.Children.ContainsKey(c)))
					{
						fail = fail.Fail;
					}
					if (fail.Children != null && fail.Children.TryGetValue(c, out Node target) && target != child)
					{
						child.Fail = target;
					}
					else
					{
						child.Fail = root;
					}
					child.Output = child.Fail.WordLength > 0 ? child.Fail : child.Fail.Output;
					queue.Enqueue(child);
				}
			}
		}

		private static Node Advance(Node node, char c)
		{
			while (node != _root && (node.Children == null || !node.Children.ContainsKey(c)))
			{
				node = node.Fail;
			}
			if (node.Children != null && node.Children.TryGetValue(c, out Node next))
			{
				return next;
			}
			return _root;
		}
	}

	private static Callback<GamepadTextInputDismissed_t> _gamepadTextInputCallback;

	private static Action<EPadInputResult, string> _gamepadTextInputDismissed;

	private const char MaskChar = '*';

	public static bool IsMouseOverUI()
	{
		if (EventSystem.current == null)
		{
			return false;
		}
		if (Mouse.current != null)
		{
			Vector2 position = Mouse.current.position.ReadValue();
			var pointerData = new PointerEventData(EventSystem.current)
			{
				position = position
			};
			var results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointerData, results);
			return results.Count != 0;
		}
		return false;
	}

	public static UniTask<(EPadInputResult, string)> ShowGamepadTextInputAsync(string description, int maxChars, string existingText)
	{
		if (!SteamManager.Initialized)
		{
			return UniTask.FromResult((EPadInputResult.NotAvailable, (string)null));
		}
		var tcs = new UniTaskCompletionSource<(EPadInputResult, string)>();
		ShowGamepadTextInput(description, maxChars, existingText, (result, text) => tcs.TrySetResult((result, text)));
		return tcs.Task;
	}

	private static void ShowGamepadTextInput(string description, int maxChars, string existingText, Action<EPadInputResult, string> onDismissed)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		_gamepadTextInputCallback?.Dispose();
		_gamepadTextInputCallback = null;
		_gamepadTextInputDismissed = null;
		_gamepadTextInputDismissed = onDismissed;
		_gamepadTextInputCallback = Callback<GamepadTextInputDismissed_t>.Create(result =>
		{
			_gamepadTextInputCallback?.Dispose();
			_gamepadTextInputCallback = null;
			_gamepadTextInputDismissed = null;
			if (!result.m_bSubmitted)
			{
				onDismissed(EPadInputResult.Cancel, null);
			}
			else
			{
				SteamUtils.GetEnteredGamepadTextInput(out string text, 256u);
				onDismissed(EPadInputResult.Success, text);
			}
		});
		if (!SteamUtils.ShowGamepadTextInput(EGamepadTextInputMode.k_EGamepadTextInputModeNormal, EGamepadTextInputLineMode.k_EGamepadTextInputLineModeSingleLine, description, (uint)maxChars, existingText ?? string.Empty))
		{
			_gamepadTextInputCallback?.Dispose();
			_gamepadTextInputCallback = null;
			_gamepadTextInputDismissed = null;
			onDismissed(EPadInputResult.NotAvailable, null);
		}
	}

	static UIInputUtility()
	{
		TBannedWord.OnTableChanged += BannedWordMatcher.MarkDirty;
	}

	public static bool ValidateName(string name)
	{
		return !ContainsBannedWord(name);
	}

	public static bool ValidateText(string text)
	{
		return !ContainsBannedWord(text);
	}

	public static string Mask(string input)
	{
		if (string.IsNullOrEmpty(input))
		{
			return input;
		}
		if (TryNormalize(input, out string normalizedText, out List<int> normToOrigin))
		{
			char[] masked = null;
			BannedWordMatcher.CollectMatches(normalizedText, (start, end) =>
			{
				if (masked == null)
				{
					masked = input.ToCharArray();
				}
				for (int i = normToOrigin[start]; i <= normToOrigin[end]; i++)
				{
					masked[i] = MaskChar;
				}
			});
			if (masked != null)
			{
				return new string(masked);
			}
		}
		return input;
	}

	private static bool ContainsBannedWord(string input)
	{
		if (string.IsNullOrEmpty(input))
		{
			return false;
		}
		if (!TryNormalize(input, out string normalizedText, out List<int> _))
		{
			return false;
		}
		return BannedWordMatcher.ContainsMatch(normalizedText);
	}

	private static bool TryNormalize(string input, out string normalizedText, out List<int> normToOrigin)
	{
		var builder = new StringBuilder(input.Length);
		normToOrigin = new List<int>(input.Length);
		for (int i = 0; i < input.Length; i++)
		{
			char c = input[i];
			if (IsMeaningful(c))
			{
				builder.Append(char.ToLowerInvariant(c));
				normToOrigin.Add(i);
			}
		}
		normalizedText = builder.ToString();
		return normalizedText.Length > 0;
	}

	private static string NormalizeWord(string word)
	{
		if (string.IsNullOrEmpty(word))
		{
			return string.Empty;
		}
		var builder = new StringBuilder(word.Length);
		for (int i = 0; i < word.Length; i++)
		{
			char c = word[i];
			if (IsMeaningful(c))
			{
				builder.Append(char.ToLowerInvariant(c));
			}
		}
		return builder.ToString();
	}

	private static bool IsMeaningful(char c)
	{
		return char.IsLetterOrDigit(c);
	}
}
