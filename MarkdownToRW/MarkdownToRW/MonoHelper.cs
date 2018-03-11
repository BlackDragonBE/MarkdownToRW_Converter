using System;

namespace MarkdownToRW
{
    public static class MonoHelper
    {
        public static bool IsRunningOnMono => Type.GetType("Mono.Runtime") != null;
        public static bool IsRunningOnMac => Environment.OSVersion.Platform == PlatformID.MacOSX;
        public static bool IsRunningOnLinux => Environment.OSVersion.Platform == PlatformID.Unix;
    }
}