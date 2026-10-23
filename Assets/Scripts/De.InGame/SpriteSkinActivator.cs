using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D.Animation;

[RequireComponent(typeof(SpriteSkin))]
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteSkinActivator : MonoBehaviour
{
	private SpriteSkin _spriteSkin;

	private SpriteRenderer _spriteRenderer;

	private void OnEnable()
	{
		if (_spriteSkin != null && _spriteSkin.enabled)
		{
			return;
		}
		_spriteSkin = GetComponent<SpriteSkin>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
		Activate().Forget();
	}

	private async UniTask Activate()
	{
		_spriteSkin.enabled = true;
		await UniTask.DelayFrame(2, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
		if (!(_spriteRenderer == null))
		{
			_spriteRenderer.enabled = true;
		}
	}
}
