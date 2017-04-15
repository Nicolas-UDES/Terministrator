using System;
using Terministrator.Terministrator.Entites;

namespace Terministrator.Terministrator.Types
{
    class Command
    {
        public Command(Message message)
        {
            Message = message;
            string text = message.GetText();
            if (text == null)
            {
                throw new NullReferenceException("The message must have a text to be a command.");
            }
            int firstSpace = text.IndexOf(' ');

            Name = firstSpace < 0 ? text.Substring(1) : text.Substring(1, firstSpace - 1);
            Arguement = firstSpace < 0 ? null : text.Substring(firstSpace + 1);
        }

        public Command(Message message, string name, string arguement)
        {
            Message = message;
            Name = name;
            Arguement = arguement;
        }

        public Message Message { get; }

        public string Name { get; private set; }

        public string Arguement { get; private set; }

        public string[] SplitArguements(char separator = ' ', int count = -1)
        {
            return count < 0 ? Arguement.Split(separator) : Arguement.Split(new [] {separator}, count);
        }
    }
}
