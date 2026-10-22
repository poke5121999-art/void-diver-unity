using System;

namespace NemoLib.Mvp
{
	public class ModelAttribute : Attribute
	{
		public ModelType modelType;

		public ModelAttribute()
		{
			modelType = ModelType.Instance;
		}

		protected ModelAttribute(ModelType modelType)
		{
			this.modelType = modelType;
		}
	}
}
