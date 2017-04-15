#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace Terministrator.Terministrator.BLL
{
    static class MessageType
    {
        private static readonly string[] Types =
        {
            "Unknown", "Text", "Photo", "Audio", "Video", "Voice", "Document",
            "Sticker", "Location", "Contact", "Service", "Venue"
        };

        public static Entites.MessageType Get(string name)
        {
            if (DAL.MessageType.Count() != Types.Length)
            {
                DAL.MessageType.DeleteAll();
                return CreateTypes().FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            }
            return DAL.MessageType.Get(name);
        }

        public static List<Entites.MessageType> GetAll()
        {
            if (DAL.MessageType.Count() != Types.Length)
            {
                DAL.MessageType.DeleteAll();
                return CreateTypes();
            }
            return DAL.MessageType.GetAll();
        }

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

        public static int Count()
        {
            return Types.Length;
        }

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