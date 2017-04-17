#region Usings

using System;
using System.Runtime.CompilerServices;

#endregion

namespace Terministrator.Terministrator.Types
{
    /// <summary>
    /// Log the requested data in the designated places.
    /// </summary>
    public class Logger
    {
        public enum Rating
        {
            Noisy,
            Information,
            Warning,
            Error
        }

        private static readonly Lazy<Logger> Instance = new Lazy<Logger>(() => new Logger());

        private Logger()
        {
        }

        public static Logger LoggerInstance => Instance.Value;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is noisy. If it's not, it will ignores the calls to LogNoisy. 
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is noisy; otherwise, <c>false</c>.
        /// </value>
        public bool IsNoisy { get; set; }

        /// <summary>
        /// Logs the noisy.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="lineNumber">The line number.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="filePath">The file path.</param>
        public void LogNoisy(string str, [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null)
        {
            if (IsNoisy)
            {
                OnLoggingRequested(new LoggingRequestedEventArgs
                {
                    Rating = Rating.Noisy,
                    CallerFilePath = filePath,
                    CallerLineNumber = lineNumber,
                    CallerMemberName = memberName,
                    Text = str
                });
            }
        }

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="lineNumber">The line number.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="filePath">The file path.</param>
        public void LogInformation(string str, [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null)
        {
            OnLoggingRequested(new LoggingRequestedEventArgs
            {
                Rating = Rating.Information,
                CallerFilePath = filePath,
                CallerLineNumber = lineNumber,
                CallerMemberName = memberName,
                Text = str
            });
        }

        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="lineNumber">The line number.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="filePath">The file path.</param>
        public void LogWarning(string str, [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null)
        {
            OnLoggingRequested(new LoggingRequestedEventArgs
            {
                Rating = Rating.Warning,
                CallerFilePath = filePath,
                CallerLineNumber = lineNumber,
                CallerMemberName = memberName,
                Text = str
            });
        }

        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="e">The exception.</param>
        /// <param name="str">The string.</param>
        /// <param name="lineNumber">The line number.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="filePath">The file path.</param>
        public void LogWarning(Exception e, string str = null, [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null)
        {
            OnLoggingRequested(new LoggingRequestedEventArgs
            {
                Rating = Rating.Warning,
                CallerFilePath = filePath,
                CallerLineNumber = lineNumber,
                CallerMemberName = memberName,
                Text = str,
                Exception = e
            });
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="lineNumber">The line number.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="filePath">The file path.</param>
        public void LogError(string str, [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null)
        {
            OnLoggingRequested(new LoggingRequestedEventArgs
            {
                Rating = Rating.Error,
                CallerFilePath = filePath,
                CallerLineNumber = lineNumber,
                CallerMemberName = memberName,
                Text = str
            });
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="e">The exception.</param>
        /// <param name="str">The string.</param>
        /// <param name="lineNumber">The line number.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="filePath">The file path.</param>
        public void LogError(Exception e, string str = null, [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string memberName = null, [CallerFilePath] string filePath = null)
        {
            OnLoggingRequested(new LoggingRequestedEventArgs
            {
                Rating = Rating.Error,
                CallerFilePath = filePath,
                CallerLineNumber = lineNumber,
                CallerMemberName = memberName,
                Text = str,
                Exception = e
            });
        }

        /// <summary>
        /// Raises the <see cref="E:LoggingRequested" /> event.
        /// </summary>
        /// <param name="e">The <see cref="LoggingRequestedEventArgs"/> instance containing the event data.</param>
        protected void OnLoggingRequested(LoggingRequestedEventArgs e)
        {
            LoggingRequested?.Invoke(this, e);
        }

        /// <summary>
        /// Occurs when logging is requested.
        /// </summary>
        public event EventHandler<LoggingRequestedEventArgs> LoggingRequested;

        /// <summary>
        /// Contains the data this was called with.
        /// </summary>
        /// <seealso cref="System.EventArgs" />
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