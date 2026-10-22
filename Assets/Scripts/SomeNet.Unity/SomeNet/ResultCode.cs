namespace SomeNet
{
	public class ResultCode
	{
		public const int OK = 0;

		public const int SYSTEM_ERROR = -10001;

		public const int SYSTEM_ERROR_GAME_TOKEN = -10002;

		public const int SYSTEM_ERROR_NOT_FOUND_URI = -10003;

		public const int SYSTEM_EXCEPTION_OCCURED = -10004;

		public const int SYSTEM_HACK_DETECTED = -10004;

		public const int SYSTEM_SERIALIZE_EXCEPTION = -10005;

		public const int SYSTEM_ERROR_SERVICE_TOKEN = -10006;

		public const int SYSTEM_ACTIVITY_LOG_FAILED = -10007;

		public const int REQUEST_VALIDATION_FAILED = -11001;

		public const int REQUEST_TOKEN_ERROR = -11002;

		public const int REQUEST_SEQUENCE_ERROR = -11003;

		public const int REQUEST_NOT_PERMITTED = -11004;

		public const int REQUEST_SERVER_UNDER_MAINTENANCE = -11005;

		public const int BIND_SESSION_FAIL = -12001;

		public const int CHANNEL_OFFLINE = -12002;

		public const int DOUBLE_LOGIN = -12003;

		public const int SERVER_STOPPED = -12004;

		public const int SESSION_TIMEOUT = -12006;

		public const int KICK = -13001;

		public const int BAN = -13002;

		public const int DB_ITEM_NOT_FOUND = -14001;

		public const int FAILED_DB_SAVE = -14002;

		public const int UNDEFINED_RESULT_TYPE = -20000;
	}
}
