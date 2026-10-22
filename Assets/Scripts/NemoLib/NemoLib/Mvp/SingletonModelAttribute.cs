namespace NemoLib.Mvp
{
	public class SingletonModelAttribute : ModelAttribute
	{
		public SingletonModelAttribute()
			: base(ModelType.Singleton)
		{
		}
	}
}
