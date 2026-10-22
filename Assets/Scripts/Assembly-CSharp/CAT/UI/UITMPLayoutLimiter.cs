using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CAT.UI
{
	[ExecuteAlways]
	[RequireComponent(typeof(TextMeshProUGUI), typeof(LayoutElement))]
	public class UITMPLayoutLimiter : MonoBehaviour
	{
		[Tooltip("최대 너비. 이 값에 도달하면 크기가 고정됩니다.")]
		public float maxWidth = 300f;

		[Tooltip("최대 높이. 이 값에 도달하면 크기가 고정됩니다.")]
		public float maxHeight = 40f;

		private TextMeshProUGUI tmpText;

		private LayoutElement layoutElement;

		private string previousText;

		private bool isLayoutDirty = true;

		private void Awake()
		{
			tmpText = GetComponent<TextMeshProUGUI>();
			layoutElement = GetComponent<LayoutElement>();
		}

		private void LateUpdate()
		{
			if (tmpText.text != previousText)
			{
				previousText = tmpText.text;
				isLayoutDirty = true;
			}
			if (isLayoutDirty)
			{
				CheckAndUpdateLayout();
				isLayoutDirty = false;
			}
		}

		private void CheckAndUpdateLayout()
		{
			float fontSize = tmpText.fontSize;
			tmpText.fontSize = tmpText.fontSizeMax;
			Vector2 preferredValues = tmpText.GetPreferredValues(tmpText.text);
			tmpText.fontSize = fontSize;
			if (preferredValues.x >= maxWidth)
			{
				if (layoutElement.preferredWidth != maxWidth)
				{
					layoutElement.preferredWidth = maxWidth;
				}
			}
			else if (layoutElement.preferredWidth != -1f)
			{
				layoutElement.preferredWidth = -1f;
			}
			if (preferredValues.y >= maxHeight)
			{
				if (layoutElement.preferredHeight != maxHeight)
				{
					layoutElement.preferredHeight = maxHeight;
				}
			}
			else if (layoutElement.preferredHeight != -1f)
			{
				layoutElement.preferredHeight = -1f;
			}
		}
	}
}
