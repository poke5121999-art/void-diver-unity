using UnityEngine;
using UnityEngine.Playables;

public class PlaneSightWeightBehaviour : PlayableBehaviour
{
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		if (!(PlaneSightCore.Instance == null))
		{
			float num = 1f - info.weight;
			PlaneSightCore.Instance.EnablePlaneSight(num);
			Shader.SetGlobalFloat(PlanePointConsts.SHADER_PROPERTY_PLANE_POINT_BUFFER_ENABLE, num);
		}
	}

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		if (!(PlaneSightCore.Instance == null))
		{
			PlaneSightCore.Instance.SetIsRevealingMap(PlaneSightCore.Instance.IsRevealingMap);
		}
	}
}
