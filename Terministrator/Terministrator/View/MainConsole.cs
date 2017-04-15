#region Usings

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Terministrator.Terministrator.Entites;
using Terministrator.Terministrator.Types;

#endregion

namespace Terministrator.Terministrator.View
{
    public partial class MainConsole : Form
    {
        private const string TimeFormat = "d'd 'hh':'mm':'ss";
        private readonly Action<Entites.Message> _sendMessage;
        private readonly Dictionary<int, StringBuilder> _channelDiscussions;
        private readonly Dictionary<string, List<Entites.Channel>> _channels;
        private readonly Label[] _statusPings;

        private int _messagesSent;
        private int _messagesReceived;
        private float _points;
        

        public int MessagesSent {
            get { return _messagesSent; }
            set
            {
                _messagesSent = value;
                SetLabel(label_MessagesSent, value.ToString());
            }
        }

        public int MessagesReceived
        {
            get { return _messagesReceived; }
            set
            {
                _messagesReceived = value;
                SetLabel(label_MessagesReceived, value.ToString());
            }
        }

        public float Points
        {
            get { return _points; }
            set
            {
                _points = value;
                SetLabel(label_Points, value.ToString());
            }
        }

        internal MainConsole(Action<Entites.Message> sendMessage)
        {
            InitializeComponent();
            _sendMessage = sendMessage;
            _channelDiscussions = new Dictionary<int, StringBuilder>();
            _channels = new Dictionary<string, List<Entites.Channel>>();
            _statusPings = new [] {label_Status0, label_Ping0, label_Status1, label_Ping1, label_Status2, label_Ping2};

        }

        private void RadioButton_Channel_CheckedChanged(object sender, EventArgs e)
        {
            label_ChannelUser.Text = (radioButton_Channel.Checked ? radioButton_Channel.Text : radioButton_User.Text) + ':';
            RefreshChannelUserDataSource();
        }

        private void RefreshChannelUserDataSource()
        {
            comboBox_ChannelUser.DataSource = _channels[comboBox_Application.Text].Where(x => x.Private == radioButton_User.Checked).ToList();
        }

        private void ControlAccess(Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(action);
            }
            else
            {
                action();
            }
        }

        private void SetLabel(Label label, string text)
        {
            ControlAccess(label, () => { label.Text = text; });
        }

        public void UpdateUpTime(TimeSpan upTime)
        {
            SetLabel(label_Uptime, upTime.ToString(TimeFormat));
        }

        public void UpdateUpSince(DateTime upSince)
        {
            label_UpSince.Text = upSince.ToString(CultureInfo.InvariantCulture);
        }

        public void RefreshPing(int index, long? ping)
        {
            SetLabel(_statusPings[index * 2], ping != null ? "Active" : "Inactive");
            SetLabel(_statusPings[index * 2 + 1], (ping ?? 0) + " ms");
        }

        private void Button_Send_Click(object sender, EventArgs e)
        {
            Entites.Message message = BuildMessage();
            textBox_Send.Clear();
            _sendMessage(message);

            DAL.User.LoadUserNames(message.UserToChannel.User);
            AddMessage(message);
        }

        private Entites.Message BuildMessage()
        {
            Entites.Message message = new Entites.Message(
                (Entites.Application) comboBox_Application.SelectedItem, 
                null,
                DateTime.UtcNow,
                new UserToChannel(
                    (Entites.Application) comboBox_Application.SelectedItem,
                    BLL.User.GetOrCreate(((Entites.Application) comboBox_Application.SelectedItem).GetTerministrator()),
                    (Channel) comboBox_ChannelUser.SelectedItem, 
                    DateTime.MinValue, 
                    null), 
                BLL.MessageType.Get("Text"));

            message.Texts = new List<Text> {new Text(textBox_Send.Text, DateTime.UtcNow, message)};

            return message;
        }

        internal void AddMessage(Entites.Message message)
        {
            int id = message.UserToChannel.ChannelId.Value;
            if (_channelDiscussions.ContainsKey(id))
            {
                _channelDiscussions[id].AppendLine(message.ToString());
            }
            else
            {
                _channelDiscussions.Add(id, new StringBuilder(message.ToString()));
            }

            Action maybeAddText = delegate {
                if (((Entites.Channel)comboBox_ChannelUser.SelectedItem)?.NamableId == message.UserToChannel.ChannelId)
                {
                    textBox_Interact.AppendText((textBox_Interact.Text.Length > 0 ? "\r\n" : "") + message);
                }
            };

            ControlAccess(textBox_Interact, maybeAddText);
        }

        private void ComboBox_ChannelUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = ((Entites.Channel) comboBox_ChannelUser.SelectedItem).NamableId;
            textBox_Interact.Text = _channelDiscussions.ContainsKey(id) ? _channelDiscussions[((Entites.Channel) comboBox_ChannelUser.SelectedItem).NamableId].ToString() : "";
        }

        private void comboBox_Application_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshChannelUserDataSource();
        }

        internal void AddClients(List<Entites.Application> clients)
        {
            clients.ForEach(AddClient);
        }

        internal void AddClient(Entites.Application client)
        {
            if (_channels.ContainsKey(client.ApplicationName))
            {
                return;
            }

            _channels.Add(client.ApplicationName, new List<Channel>());
            ControlAccess(comboBox_Application, () => { comboBox_Application.Items.Add(client); });
        }

        internal void AddChannels(List<Entites.Channel> channels)
        {
            channels.ForEach(AddChannel);
        }

        internal void AddChannel(Entites.Channel channel)
        {
            if (_channels[channel.ApplicationName].Exists(x => x.NamableId == channel.NamableId))
            {
                return;
            }

            _channels[channel.ApplicationName].Add(channel);
            if (channel.Private == radioButton_User.Checked && comboBox_Application.Text.Equals(channel.ApplicationName, StringComparison.InvariantCultureIgnoreCase))
            {
                RefreshChannelUserDataSource();
            }
        }

        public void Log(object logger, Logger.LoggingRequestedEventArgs args)
        {
            string log = $"{GetPaddedRating(RatingToString(args.Rating))} at " +
                         $"{DateTime.Now} located in {args.CallerFilePath?.Substring(args.CallerFilePath.LastIndexOf('\\') + 1)} " +
                         $"{(args.CallerMemberName != null ? $"{args.CallerMemberName} " : "")}" +
                         $"line {args.CallerLineNumber}{(args.Text != null ? ": " + args.Text : ".")}" +
                         $"{(args.Exception != null ? $"\r\n{args.Exception}" : "")}";
            Log(log);
        }

        private string RatingToString(Logger.Rating rating)
        {
            switch (rating)
            {
                case Logger.Rating.Noisy:
                    return "";
                case Logger.Rating.Information:
                    return "Info";
                case Logger.Rating.Warning:
                    return "/?\\ Warning /?\\";
                case Logger.Rating.Error:
                    return "/!\\ Error /!\\";
            }

            return null;
        }

        private string GetPaddedRating(string rating)
        {
            return new string(' ', 15 - rating.Length) + rating;
        }

        private void Log(string str)
        {
            ControlAccess(textBox_log, () => { textBox_log.AppendText($"{(textBox_log.Text.Length > 0 ? "\r\n" : "")}{str}"); });
        }

        private void button_Clear_Click(object sender, EventArgs e)
        {
            ControlAccess(textBox_log, () => { textBox_log.Clear(); });
        }
    }
}