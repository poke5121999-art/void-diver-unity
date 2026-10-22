using UnityEngine.Rendering.Universal;

namespace LCArt.URP
{
	public interface IRenderPassEventAware
	{
		void SetRenderPassEvent(RenderPassEvent renderPassEvent);
	}
}
