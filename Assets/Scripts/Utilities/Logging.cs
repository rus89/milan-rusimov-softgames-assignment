using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Softgames.Utilities
{
	public static class Logging
	{
		[Conditional("DEBUG")]
		public static void Log(string message)
		{
			Debug.Log($"[Softgames LOG] {message}");
		}

		[Conditional("DEBUG")]
		public static void LogWarning(string message)
		{
			Debug.LogWarning($"[Softgames WARNING] {message}");
		}

		[Conditional("DEBUG")]
		public static void LogError(string message)
		{
			Debug.LogError($"[Softgames ERROR] {message}");
		}

		[Conditional("DEBUG")]
		public static void LogException(Exception exception)
		{
			Debug.LogException(new Exception($"[Softgames EXCEPTION] {exception}"));
		}
	}
}