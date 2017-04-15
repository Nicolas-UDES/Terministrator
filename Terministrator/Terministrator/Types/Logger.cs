using System;
using System.Runtime.CompilerServices;

namespace Terministrator.Terministrator.Types
{
    public class Logger
    {
        private static readonly Lazy<Logger> Instance = new Lazy<Logger>(() => new Logger());

        public static Logger LoggerInstance => Instance.Value;

        public bool IsNoisy { get; set; }

        private Logger()
        {
            
        }

        public void LogNoisy(string str, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null)
        {
            if (IsNoisy)
            {
                OnLoggingRequested(new LoggingRequestedEventArgs{Rating = Rating.Noisy, CallerFilePath = filePath, CallerLineNumber = lineNumber, CallerMemberName = memberName, Text = str});
            }
        }

        public void LogInformation(string str, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null)
        {
            OnLoggingRequested(new LoggingRequestedEventArgs { Rating = Rating.Information, CallerFilePath = filePath, CallerLineNumber = lineNumber, CallerMemberName = memberName, Text = str });
        }

        public void LogWarning(string str, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null)
        {
            OnLoggingRequested(new LoggingRequestedEventArgs { Rating = Rating.Warning, CallerFilePath = filePath, CallerLineNumber = lineNumber, CallerMemberName = memberName, Text = str });
        }

        public void LogWarning(Exception e, string str = null, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null)
        {
            OnLoggingRequested(new LoggingRequestedEventArgs { Rating = Rating.Warning, CallerFilePath = filePath, CallerLineNumber = lineNumber, CallerMemberName = memberName, Text = str, Exception = e});
        }

        public void LogError(string str, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null)
        {
            OnLoggingRequested(new LoggingRequestedEventArgs { Rating = Rating.Error, CallerFilePath = filePath, CallerLineNumber = lineNumber, CallerMemberName = memberName, Text = str });
        }

        public void LogError(Exception e, string str = null, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null)
        {
            OnLoggingRequested(new LoggingRequestedEventArgs { Rating = Rating.Error, CallerFilePath = filePath, CallerLineNumber = lineNumber, CallerMemberName = memberName, Text = str, Exception = e });
        }

        protected void OnLoggingRequested(LoggingRequestedEventArgs e)
        {
            LoggingRequested?.Invoke(this, e);
        }

        public event EventHandler<LoggingRequestedEventArgs> LoggingRequested;

        public enum Rating
        {
            Noisy,
            Information,
            Warning,
            Error
        }

        public class LoggingRequestedEventArgs : EventArgs
        {
            public Rating Rating { get; set; }
            public string Text { get; set; }
            public Exception Exception { get; set; }
            public int CallerLineNumber { get; set; }
            public string CallerMemberName { get; set; }
            public string CallerFilePath { get; set; }
        }
    }
}
