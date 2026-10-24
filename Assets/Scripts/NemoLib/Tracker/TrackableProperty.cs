namespace Tracker
{
	// TrackableProperty<object> @0x181c09430..0x181c09600 (machine code checked)
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
				// no-op when the new value equals the current one (`call 0x182b515a0` is object.Equals(a, b), then `jne` to the return)
				if (object.Equals(_newValue, value))
				{
					return;
				}
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
			TValue result = ((_operation == EDataOperation.Remove) ? _oldValue : _newValue).Clone();
			result.Operation = _operation;
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
			return _newValue?.ToString();
		}
	}
}
