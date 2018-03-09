using System;

namespace MarkdownToRW
{
    public static class MonoHelper
    {
        public static bool IsRunningOnMono => Type.GetType("Mono.Runtime") != null;
    }
}