namespace SomeNet
{
	public class ChangeData<T>
	{
		public DataOperation Operation { get; set; }

		public T OldValue { get; set; }

		public T NewValue { get; set; }

		public ChangeData()
		{
		}

		public ChangeData(DataOperation operation, T oldValue, T newValue)
		{
			Operation = operation;
			OldValue = oldValue;
			NewValue = newValue;
		}
	}
}
