namespace DarkTonic.MasterAudio
{
	public static class MasterAudioExtensions
	{
		public static void StopSound(this PlaySoundResult soundResult)
		{
			// The body is SoundGroupVariation.Stop() inlined (stop end-linked groups, reset pitch,
			// remove active source id, stop following, deactivate / unload clip).
			soundResult.ActingVariation.Stop();
		}
	}
}
