using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VContainer.Unity;

namespace NemoLib.Mvp
{
	public abstract class Presenter : MonoBase
	{
		private bool isAwaken;

		private readonly Dictionary<Type, FieldInfo> injectFields = new Dictionary<Type, FieldInfo>();

		private readonly List<object> injectModels = new List<object>();

		private readonly List<Model> instanceModels = new List<Model>();

		protected override void Awake()
		{
			this.InjectFromRootContainer();
			FieldInfo[] fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
			foreach (FieldInfo field in fields)
			{
				object[] attributes = field.GetCustomAttributes(false);
				foreach (object attribute in attributes)
				{
					if (attribute is ViewAttribute)
					{
						field.SetValue(this, GetComponent(field.FieldType));
					}
					if (attribute is ModelAttribute modelAttribute)
					{
						if (modelAttribute.modelType == ModelType.Instance)
						{
							Model model = Activator.CreateInstance(field.FieldType) as Model;
							field.SetValue(this, model);
							instanceModels.Add(model);
						}
						else if (modelAttribute.modelType == ModelType.Inject)
						{
							injectFields.Add(field.FieldType, field);
						}
					}
				}
			}
			foreach (object model in injectModels)
			{
				InjectInternal(model);
			}
			injectModels.Clear();
			isAwaken = true;
		}

		public void Inject(object model)
		{
			if (!isAwaken)
			{
				injectModels.Add(model);
				return;
			}
			InjectInternal(model);
		}

		private void InjectInternal(object model)
		{
			FieldInfo field = injectFields[model.GetType()];
			field.SetValue(this, model);
			Type baseType = GetType().BaseType;
			if (baseType != null && baseType != typeof(object))
			{
				MethodInfo baseOnInject = baseType.GetMethod("OnInject", BindingFlags.Instance | BindingFlags.NonPublic);
				if (baseOnInject != null)
				{
					baseOnInject.Invoke(this, new object[] { model });
				}
			}
			MethodInfo onInject = GetType().GetMethod("OnInject", BindingFlags.Instance | BindingFlags.NonPublic, Type.DefaultBinder, new Type[] { model.GetType() }, null);
			if (onInject != null)
			{
				onInject.Invoke(this, new object[] { model });
			}
		}
	}
}
