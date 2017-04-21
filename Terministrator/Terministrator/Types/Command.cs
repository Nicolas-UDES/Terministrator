#region Usings

using System;
using Terministrator.Terministrator.Entites;

#endregion

namespace Terministrator.Terministrator.Types
{
    /// <summary>
    /// Used to hold the information of a command when a message is found to be one.
    /// </summary>
    class Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Command"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="name">The name.</param>
        /// <param name="arguement">The arguement.</param>
        public Command(Message message, string name, string arguement)
        {
            Message = message;
            Name = name;
            Arguement = arguement;
        }

        public Message Message { get; }

        public string Name { get; private set; }

        public string Arguement { get; }

        /// <summary>
        /// Splits the arguements.
        /// </summary>
        /// <param name="separator">The separator.</param>
        /// <param name="count">How many strings expected.</param>
        /// <returns></returns>
        public string[] SplitArguements(char separator = ' ', int count = -1)
        {
            return count < 0 ? Arguement.Split(separator) : Arguement.Split(new[] {separator}, count);
        }
    }
}