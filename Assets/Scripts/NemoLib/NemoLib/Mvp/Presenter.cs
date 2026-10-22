using System;
using System.Collections.Generic;
using System.Reflection;

namespace NemoLib.Mvp
{
	public abstract class Presenter : MonoBase
	{
		private bool isAwaken;

		private readonly Dictionary<Type, FieldInfo> injectFields;

		private readonly List<object> injectModels;

		private readonly List<Model> instanceModels;

		protected override void Awake()
		{
		}

		public void Inject(object model)
		{
		}

		private void InjectInternal(object model)
		{
		}
	}
}
