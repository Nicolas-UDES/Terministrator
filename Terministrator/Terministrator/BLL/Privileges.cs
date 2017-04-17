#region Usings

using System.Collections.Generic;
using System.Linq;
using Terministrator.Terministrator.Types;

#endregion

namespace Terministrator.Terministrator.BLL
{
    static class Privileges
    {
        /// <summary>
        /// Creates the default privileges comming with a new channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>A collection of the default privileges.</returns>
        public static List<Entites.Privileges> Create(Entites.Channel channel)
        {
            List<Entites.Privileges> privileges = new List<Entites.Privileges> {GetNewUser(channel), GetNewMod(channel)};
            foreach (Entites.Privileges p in privileges)
            {
                Rules.Create(p.Rules);
                p.RulesId = p.Rules.RulesId;
                DAL.Privileges.Create(p);
            }
            return privileges;
        }

        /// <summary>
        /// Gets the default privileges group from that channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>The requested privileges group.</returns>
        public static Entites.Privileges GetDefaultUser(Entites.Channel channel)
        {
            return DAL.Privileges.GetDefaultUser(channel.NamableId);
        }

        /// <summary>
        /// Gets a new default user privileges group.
        /// </summary>
        /// <param name="channel">The channel where this will be in.</param>
        /// <returns>The requested privileges.</returns>
        public static Entites.Privileges GetNewUser(Entites.Channel channel)
        {
            return new Entites.Privileges("User", true, channel, Rules.GetNewUserRules());
        }

        /// <summary>
        /// Gets a new default mod privileges group.
        /// </summary>
        /// <param name="channel">The channel where this will be in.</param>
        /// <returns>The requested privileges.</returns>
        public static Entites.Privileges GetNewMod(Entites.Channel channel)
        {
            return new Entites.Privileges("Moderator", false, channel, Rules.GetNewModRules());
        }

        /// <summary>
        /// Gets a specific privileges group in a channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="name">The name.</param>
        /// <returns>The requested privileges.</returns>
        public static Entites.Privileges GetPrivileges(Entites.Channel channel, string name)
        {
            return DAL.Privileges.GetPrivileges(channel.NamableId, name);
        }

        /// <summary>
        /// Mod command. Renames the privileges group to an unused name.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="core">The core.</param>
        public static void RenamePrivileges(Command command, Core core = null)
        {
            if (Tools.IsNotModThenSendWarning(command))
            {
                return;
            }

            string[] args = command.Arguement.Split(new[] {' '}, 2);
            if (args.Length != 2)
            {
                command.Message.Application.SendMessage(Message.Answer(command.Message,
                    "The command is [/renameprivileges oldname newname]."));
                return;
            }
            if (args[1].Contains(' '))
            {
                command.Message.Application.SendMessage(Message.Answer(command.Message,
                    "The privileges group name cannot contain spaces."));
                return;
            }

            Entites.Privileges privileges = GetPrivileges(command.Message.UserToChannel.Channel, args[0]);
            if (privileges == null)
            {
                command.Message.Application.SendMessage(Message.Answer(command.Message,
                    $"No privileges group nammed {args[0]} was found."));
                return;
            }
            if (GetPrivileges(command.Message.UserToChannel.Channel, args[1]) != null)
            {
                command.Message.Application.SendMessage(Message.Answer(command.Message,
                    $"A privileges group nammed {args[1]} already exists."));
                return;
            }

            privileges.Name = args[1];
            DAL.Privileges.Update(privileges);
            command.Message.Application.SendMessage(Message.Answer(command.Message,
                $"The privileges group {args[0]} was successfully renammed to {args[1]}."));
        }

        /// <summary>
        /// Mod command. Adds a new privileges group.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="core">The core.</param>
        public static void AddPrivileges(Command command, Core core = null)
        {
            if (Tools.IsNotModThenSendWarning(command))
            {
                return;
            }

            string[] args = command.Arguement.Split(new[] {' '}, 2);
            if (args.Length == 0)
            {
                command.Message.Application.SendMessage(Message.Answer(command.Message,
                    "The command is \"/addprivileges name [copiedprivileges]\"."));
                return;
            }
            if (GetPrivileges(command.Message.UserToChannel.Channel, args[0]) != null)
            {
                command.Message.Application.SendMessage(Message.Answer(command.Message,
                    $"A privileges group named {args[0]} already exists."));
                return;
            }

            Entites.Privileges copying = args.Length == 1
                ? GetDefaultUser(command.Message.UserToChannel.Channel)
                : GetPrivileges(command.Message.UserToChannel.Channel, args[1]);
            if (copying == null)
            {
                command.Message.Application.SendMessage(Message.Answer(command.Message,
                    $"No privileges group nammed {args[1]} was found."));
                return;
            }

            DAL.Privileges.Create(new Entites.Privileges(args[0], false, command.Message.UserToChannel.Channel,
                Rules.Create(Entites.Rules.Copy(DAL.Privileges.LoadRules(copying).Rules))));
            command.Message.Application.SendMessage(Message.Answer(command.Message,
                $"The privileges group named {args[0]} {(args.Length == 2 ? $"inheriting {args[1]} " : "")}was successfully created!"));
        }
    }
}