namespace Tracker
{
	public class TrackableProperty<TValue> where TValue : class, ITrackableData<TValue>
	{
		private EDataOperation _operation;

		private TValue _oldValue;

		private TValue _newValue;

		public TValue Value
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public bool HasChange => false;

		public TrackableProperty(TValue initialValue = null)
		{
		}

		public TValue Flush()
		{
			return null;
		}

		public void ClearDirty()
		{
		}

		public override string ToString()
		{
			return null;
		}
	}
}
