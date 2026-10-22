using Cysharp.Threading.Tasks;

namespace UnityEngine.UI
{
	public static class Extensions
	{
		public static void SetSprite(this Image image, Sprite sprite)
		{
			if (!(image == null))
			{
				image.sprite = sprite;
			}
		}

		public static void SetSprite(this Image image, UniTask<Sprite> spriteTask)
		{
			image.SetSpriteAsync(spriteTask).Forget();
		}

		private static async UniTask SetSpriteAsync(this Image image, UniTask<Sprite> iconTask)
		{
			bool previousEnabled = image.enabled;
			image.enabled = false;
			Sprite sprite = await iconTask;
			if (!(image == null))
			{
				image.sprite = sprite;
				if (previousEnabled)
				{
					image.enabled = true;
				}
			}
		}
	}
}
