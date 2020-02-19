using System;
using System.Diagnostics.CodeAnalysis;

using THNETII.Common;

namespace TelenorConnexion.ManagedIoTCloud.Sample.Cmd
{
    public static class Program
    {
        [SuppressMessage("Globalization", "CA1303: Do not pass literals as localized parameters")]
        public static int Main()
        {
            Console.WriteLine("Hello World");

            return ProcessExitCode.ExitSuccess;
        }
    }
}
