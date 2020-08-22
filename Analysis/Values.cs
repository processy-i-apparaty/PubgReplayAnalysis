using System;

namespace Analysis
{
    public static class Values
    {
        public static string ReplaysDirectory =
            Environment.GetEnvironmentVariable("localappdata") + "\\TslGame\\Saved\\Demos";
    }
}