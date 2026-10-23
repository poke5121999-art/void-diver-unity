using GameCommon.Enum;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	public static class Extensions
	{
		public static void SetLayout(this Button button, EButtonState state)
		{
			Animator animator = button.GetComponent<Animator>();
			if (animator != null)
			{
				animator.SetInteger("Layout", (int)state);
			}
		}

		public static void Submit(this Button button)
		{
			if (!(button == null) && button.IsInteractable())
			{
				ExecuteEvents.Execute(button.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
			}
		}
	}
}
