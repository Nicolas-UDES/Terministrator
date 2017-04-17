#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using Terministrator.Terministrator.Types;

#endregion

namespace Terministrator.Terministrator.BLL
{
    static class MessageTypeToPointSystem
    {
        private const string UniqueOthers = "!\"/$%?&*(others)*&?%$/\"!";

        /// <summary>
        /// Creates the specified point system.
        /// </summary>
        /// <param name="pointSystem">The point system.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="reward">The reward.</param>
        /// <returns></returns>
        public static Entites.MessageTypeToPointSystem Create(Entites.PointSystem pointSystem,
            Entites.MessageType messageType, float reward)
        {
            return
                DAL.MessageTypeToPointSystem.Create(new Entites.MessageTypeToPointSystem(pointSystem, messageType,
                    reward));
        }

        /// <summary>
        /// Creates the specified message type to point system.
        /// </summary>
        /// <param name="messageTypeToPointSystem">The message type to point system.</param>
        /// <returns></returns>
        public static Entites.MessageTypeToPointSystem Create(Entites.MessageTypeToPointSystem messageTypeToPointSystem)
        {
            return DAL.MessageTypeToPointSystem.Create(messageTypeToPointSystem);
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="pointSystemId">The point system identifier.</param>
        /// <returns></returns>
        public static List<Entites.MessageTypeToPointSystem> GetAll(int pointSystemId)
        {
            return DAL.MessageTypeToPointSystem.GetAll(pointSystemId);
        }

        /// <summary>
        /// Deletes all the amounts between a point system and a message type.
        /// </summary>
        /// <param name="pointSystemId">The point system identifier.</param>
        public static void DeleteAllFrom(int pointSystemId)
        {
            DAL.MessageTypeToPointSystem.DeleteAllFrom(pointSystemId);
        }

        /// <summary>
        /// Sets the amounts between the different message types and the point system.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="core">The core.</param>
        internal static void SetAmounts(Command command, Core core = null)
        {
            if (Tools.IsNotModThenSendWarning(command))
            {
                return;
            }

            List<Entites.MessageTypeToPointSystem> olds =
                GetAll(command.Message.UserToChannel.Channel.PointSystem.ChannelId);
            DeleteAllFrom(command.Message.UserToChannel.Channel.PointSystem.ChannelId);

            List<Tuple<string, float>> values;
            string errorMessage = ReadArray(command.SplitArguements(';'), out values);

            if (errorMessage == null && values.Count != MessageType.Count() &&
                !values.Exists(x => x.Item1.Equals(UniqueOthers)))
            {
                errorMessage =
                    $"You need to either have {MessageType.Count()} elements or have a default value (only a number, without a name).";
            }

            if (errorMessage == null)
            {
                foreach (Entites.MessageType messageType in MessageType.GetAll())
                {
                    Create(command.Message.UserToChannel.Channel.PointSystem, messageType,
                        values.FirstOrDefault(x => x.Item1.Equals(messageType.Name))?.Item2 ??
                        values.First(x => x.Item1.Equals(UniqueOthers)).Item2);
                }
            }
            else
            {
                DeleteAllFrom(command.Message.UserToChannel.Channel.NamableId);
                foreach (Entites.MessageTypeToPointSystem messageTypeToPointSystem in olds)
                {
                    Create(messageTypeToPointSystem);
                }
                Entites.Message.SendMessage(Message.Answer(command.Message, errorMessage));
            }
        }

        /// <summary>
        /// Reads the array for the SetAmounts function.
        /// </summary>
        /// <param name="amounts">The amounts.</param>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        private static string ReadArray(string[] amounts, out List<Tuple<string, float>> array)
        {
            array = new List<Tuple<string, float>>();

            foreach (string amount in amounts)
            {
                string[] splited = amount.Split(new[] {'-'}, 2);

                float reward;
                if (!float.TryParse(splited[0], out reward))
                {
                    return $"Is \"{splited[0]}\" supposed to be a number?";
                }

                if (splited.Length == 1)
                {
                    if (array.Exists(x => x.Item1.Equals(UniqueOthers)))
                    {
                        return "You can only have one default value.";
                    }
                    array.Add(new Tuple<string, float>(UniqueOthers, reward));
                    continue;
                }

                if (array.Exists(x => x.Item1.Equals(splited[1])))
                {
                    return $"You have the element \"{splited[1]}\" more than once.";
                }

                Entites.MessageType messageType = MessageType.Get(splited[1]);
                if (messageType == null)
                {
                    return
                        $"The message type \"{splited[1]}\" is not recognized. It can only be one of the followings: {MessageType.ToString()}";
                }

                array.Add(new Tuple<string, float>(UniqueOthers, reward));
            }

            return null;
        }
    }
}