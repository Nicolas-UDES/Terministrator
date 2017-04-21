#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace Terministrator.Terministrator.BLL
{
    /// <summary>
    /// Business logic layer of the message types. Makes sure the database is up to date with the set types and gives them on request.
    /// </summary>
    static class MessageType
    {
        private static readonly string[] Types =
        {
            "Unknown", "Text", "Photo", "Audio", "Video", "Voice", "Document",
            "Sticker", "Location", "Contact", "Service", "Venue"
        };

        /// <summary>
        /// Gets the specified message type.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The requested message type.</returns>
        public static Entites.MessageType Get(string name)
        {
            if (DAL.MessageType.Count() != Types.Length)
            {
                DAL.MessageType.DeleteAll();
                return
                    CreateTypes().FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            }
            return DAL.MessageType.Get(name);
        }

        /// <summary>
        /// Gets all existing message types.
        /// </summary>
        /// <returns>The requested collection.</returns>
        public static List<Entites.MessageType> GetAll()
        {
            if (DAL.MessageType.Count() != Types.Length)
            {
                DAL.MessageType.DeleteAll();
                return CreateTypes();
            }
            return DAL.MessageType.GetAll();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents every available message types.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" />.
        /// </returns>
        public new static string ToString()
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < Types.Length; ++i)
            {
                str.Append(Types[i]);
                if (i + 2 == Types.Length)
                {
                    str.Append(" and ");
                }
                else if (i + 1 != Types.Length)
                {
                    str.Append(", ");
                }
            }
            return str.ToString();
        }

        /// <summary>
        /// Counts how many message types exists.
        /// </summary>
        /// <returns></returns>
        public static int Count()
        {
            return Types.Length;
        }

        /// <summary>
        /// Creates the default message types.
        /// </summary>
        /// <returns>The created message types.</returns>
        private static List<Entites.MessageType> CreateTypes()
        {
            List<Entites.MessageType> types = new List<Entites.MessageType>();
            foreach (string type in Types)
            {
                types.Add(DAL.MessageType.Create(new Entites.MessageType(type)));
            }
            return types;
        }
    }
}