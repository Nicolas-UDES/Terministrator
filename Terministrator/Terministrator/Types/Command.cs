#region Usings

using System;
using Terministrator.Terministrator.Entites;

#endregion

namespace Terministrator.Terministrator.Types
{
    class Command
    {
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