using Serilog;

namespace XSLTTransformer.Core
{
    /// <summary>
    /// Centralized logging facade using Serilog.
    /// </summary>
    public static class AppLogger
    {
        static AppLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }

        public static void Info(string message) => Log.Information(message);
        public static void Error(string message, System.Exception? ex = null) => Log.Error(ex, message);
    }
}