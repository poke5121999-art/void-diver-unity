using Cysharp.Threading.Tasks;
using GameCommon.Table;

namespace NemoLib.Popup
{
	public static class Extensions
	{
		public static void ShowTextPopup(this PopupManager popupManager, string message, string additionalMessage = null)
		{
			popupManager.Open<TextPopupPresenter>(new TextPopupModel
			{
				CommonPopupType = CommonPopupType.ONE_BUTTON,
				Message = message,
				AdditionalMessage = additionalMessage
			});
		}

		public static async UniTask ShowTextPopupAsync(this PopupManager popupManager, string message, string additionalMessage = null)
		{
			TextPopupPresenter textPopupPresenter = await popupManager.OpenAsync<TextPopupPresenter>(new TextPopupModel
			{
				CommonPopupType = CommonPopupType.ONE_BUTTON,
				Message = message,
				AdditionalMessage = additionalMessage
			});
			await textPopupPresenter.Task;
		}

		public static void ShowConnectionFailPopup(this PopupManager popupManager, int resultCode)
		{
			string message;
			switch (resultCode)
			{
			case 3001:
				message = TLocalizedText.GameConnectionFailed.LocalText;
				break;
			case 3002:
				message = TLocalizedText.GameAlreadyStarted.LocalText;
				break;
			case 3004:
				message = TLocalizedText.PlayerLimitExceeded.LocalText;
				break;
			case 3005:
				message = TLocalizedText.NetworkOffline.LocalText;
				break;
			case 3006:
				message = TLocalizedText.CoopVersionMismatch.LocalText;
				break;
			default:
				message = TLocalizedText.NetworkErrorFormat.Format(resultCode);
				break;
			}
			popupManager.Open<TextPopupPresenter>(new TextPopupModel
			{
				CommonPopupType = CommonPopupType.ONE_BUTTON,
				Message = message
			});
		}

		public static async UniTask<bool> ShowConfirmationTextPopupAsync(this PopupManager popupManager, string message, string additionalMessage = null)
		{
			TextPopupPresenter textPopupPresenter = await popupManager.OpenAsync<TextPopupPresenter>(new TextPopupModel
			{
				CommonPopupType = CommonPopupType.TWO_BUTTON,
				Message = message,
				AdditionalMessage = additionalMessage
			});
			object obj = await textPopupPresenter.Task;
			return obj != null && (bool)obj;
		}

		public static void Notice(this ToastManager toastManager, string text, string sfx = null)
		{
			if (!(toastManager == null) && !string.IsNullOrEmpty(text))
			{
				toastManager.Show(new NoticeToastModel
				{
					Message = text,
					OpenSfx = sfx
				});
			}
		}

		public static void NoticeFail(this ToastManager toastManager, string text)
		{
			if (!(toastManager == null) && !string.IsNullOrEmpty(text))
			{
				toastManager.Show(new NoticeToastModel
				{
					Message = text,
					OpenSfx = "Fail"
				});
			}
		}

		public static void PlayerNotice(this ToastManager toastManager, string text, string sfx = null)
		{
			if (!(toastManager == null) && !string.IsNullOrEmpty(text))
			{
				toastManager.Show(new PlayerNoticeToastModel
				{
					Message = text,
					OpenSfx = sfx
				});
			}
		}
	}
}
