using System;

namespace NemoLib.Mvp
{
	public class ModelAttribute : Attribute
	{
		public ModelType modelType;

		public ModelAttribute()
		{
		}

		protected ModelAttribute(ModelType modelType)
		{
		}
	}
}
