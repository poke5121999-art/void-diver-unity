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
		public float maxWidth;

		[Tooltip("최대 높이. 이 값에 도달하면 크기가 고정됩니다.")]
		public float maxHeight;

		private TextMeshProUGUI tmpText;

		private LayoutElement layoutElement;

		private string previousText;

		private bool isLayoutDirty;

		private void Awake()
		{
		}

		private void LateUpdate()
		{
		}

		private void CheckAndUpdateLayout()
		{
		}
	}
}
