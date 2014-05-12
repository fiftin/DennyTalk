using System;

namespace Common
{
	public static class CommonUtil
	{
		public static bool IsWindows {
			get {
				return Environment.OSVersion.Platform == PlatformID.Win32NT
					|| Environment.OSVersion.Platform == PlatformID.Win32Windows
					|| Environment.OSVersion.Platform == PlatformID.Win32S;
			}
		}
	}
}

