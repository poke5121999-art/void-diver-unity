using VContainer;

public class AnimationEventSfxPlayer : MonoBase
{
	[Inject]
	private SoundManager _soundManager;

	public void PlaySfx(string sfxKey)
	{
		_soundManager.PlaySfx(sfxKey);
	}
}
