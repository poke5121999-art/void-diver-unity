using UnityEngine;

namespace Serilog
{
	public static class ILoggerExtensions
	{
		public static ILogger ForContext(this ILogger logger, Object context)
		{
			return logger.ForContext(new UnityObjectEnricher(context));
		}

		public static ILogger WithUnityTag(this ILogger logger, string tag)
		{
			return logger.ForContext(new UnityTagEnricher(tag));
		}
	}
}
