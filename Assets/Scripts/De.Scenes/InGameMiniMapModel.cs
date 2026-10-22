using NemoLib.Mvp;
using UniRx;

public class InGameMiniMapModel : Model
{
	public float MaskRadius;

	public float BaseZoomScale;

	public float FullScreenRatio;

	public float FullScreenZoomScale;

	public readonly ReactiveProperty<bool> RxIsFullScreen;

	public bool IsFullScreen => false;
}
