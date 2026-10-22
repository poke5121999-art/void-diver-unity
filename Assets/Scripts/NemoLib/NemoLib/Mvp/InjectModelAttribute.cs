namespace NemoLib.Mvp
{
	public class InjectModelAttribute : ModelAttribute
	{
		public InjectModelAttribute()
			: base(ModelType.Inject)
		{
		}
	}
}
