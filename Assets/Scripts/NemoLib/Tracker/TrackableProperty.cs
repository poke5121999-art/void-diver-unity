namespace Tracker
{
	// TODO(decomp): generic type has no pseudo-C; change tracking is reconstructed from the field layout
	public class TrackableProperty<TValue> where TValue : class, ITrackableData<TValue>
	{
		private EDataOperation _operation;

		private TValue _oldValue;

		private TValue _newValue;

		public TValue Value
		{
			get
			{
				return _newValue;
			}
			set
			{
				if (_operation == EDataOperation.None)
				{
					_oldValue = _newValue;
				}
				_newValue = value;
				if (_oldValue == null)
				{
					_operation = ((_newValue != null) ? EDataOperation.Add : EDataOperation.None);
				}
				else
				{
					_operation = ((_newValue != null) ? EDataOperation.Replace : EDataOperation.Remove);
				}
			}
		}

		public bool HasChange => _operation != EDataOperation.None;

		public TrackableProperty(TValue initialValue = null)
		{
			_newValue = initialValue;
		}

		public TValue Flush()
		{
			if (_operation == EDataOperation.None)
			{
				return null;
			}
			TValue result = ((_operation == EDataOperation.Remove) ? _oldValue : _newValue)?.Clone();
			if (result != null)
			{
				result.Operation = _operation;
			}
			ClearDirty();
			return result;
		}

		public void ClearDirty()
		{
			_operation = EDataOperation.None;
			_oldValue = null;
		}

		public override string ToString()
		{
			return _newValue?.ToString() ?? "null";
		}
	}
}
