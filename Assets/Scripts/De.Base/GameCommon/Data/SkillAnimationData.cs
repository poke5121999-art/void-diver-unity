namespace GameCommon.Data
{
	public class SkillAnimationData
	{
		public string animationName;

		public string nonCombatAnimationName;

		public string moveAnimationName;

		public bool UseMultiTrackMoveAnimation;

		public string nonCombatMoveAnimationName;

		public bool isLoop;

		public bool IsLoopAnimationData;

		public float animStartOffsetTime;

		public float duration = -1f;

		public AnimationSpeed[] animationSpeeds;
	}
}
