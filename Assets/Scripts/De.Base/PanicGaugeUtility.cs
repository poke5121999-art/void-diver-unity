using GameCommon.Table;
using UnityEngine;
using UnityEngine.UI;

public static class PanicGaugeUtility
{
	private static readonly Color NormalBack = new Color(0.345f, 0.804f, 0.792f, 1f);

	private static readonly Color NormalFront = new Color(0.275f, 0.643f, 0.631f, 1f);

	private static readonly Color PanicBack = new Color(1f, 0.553f, 0.475f, 1f);

	private static readonly Color PanicFront = new Color(0.643f, 0.337f, 0.275f, 1f);

	public static void Apply(Slider slider, Image gaugeBack, Image gaugeFront, int stress, bool isPanic)
	{
		int intValue = TConst.StressMax.IntValue;
		float num = 1f;
		if (intValue > 0)
		{
			num = 1f - (float)stress / (float)intValue;
		}
		slider.value = num;
		gaugeBack.color = (isPanic ? PanicBack : NormalBack);
		gaugeFront.color = (isPanic ? PanicFront : NormalFront);
	}
}
