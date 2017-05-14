#region Usings

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Terministrator.Terministrator.Entites;
using Terministrator.Terministrator.Types;
using Message = Terministrator.Terministrator.Entites.Message;
using MessageType = Terministrator.Terministrator.BLL.MessageType;
using User = Terministrator.Terministrator.DAL.User;

#endregion

namespace Terministrator.Terministrator.View
{
    /// <summary>
    /// The main console to administrate Terministrator.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class MainConsole : Form
    {
        private const string TimeFormat = "d'd 'hh':'mm':'ss";
        private readonly Dictionary<int, StringBuilder> _channelDiscussions;
        private readonly Dictionary<string, List<Channel>> _channels;
        private readonly Action<Message> _sendMessage;
        private readonly Label[] _statusPings;
        private int _messagesReceived;

        private int _monitoredChannels;
        private int _messagesSent;
        private float _points;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainConsole"/> class.
        /// </summary>
        /// <param name="sendMessage">The send message.</param>
        internal MainConsole(Action<Message> sendMessage)
        {
            InitializeComponent();
            _sendMessage = sendMessage;
            _channelDiscussions = new Dictionary<int, StringBuilder>();
            _channels = new Dictionary<string, List<Channel>>();
            _statusPings = new[] {label_Status0, label_Ping0, label_Status1, label_Ping1, label_Status2, label_Ping2};
        }

        /// <summary>
        /// Gets or sets the messages sent.
        /// </summary>
        /// <value>
        /// The messages sent.
        /// </value>
        public int MessagesSent
        {
            get { return _messagesSent; }
            set
            {
                _messagesSent = value;
                SetLabel(label_MessagesSent, value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets the messages received.
        /// </summary>
        /// <value>
        /// The messages received.
        /// </value>
        public int MessagesReceived
        {
            get { return _messagesReceived; }
            set
            {
                _messagesReceived = value;
                SetLabel(label_MessagesReceived, value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        /// <value>
        /// The points.
        /// </value>
        public float Points
        {
            get { return _points; }
            set
            {
                _points = value;
                SetLabel(label_Points, value.ToString());
            }
        }

        /// <summary>
        /// Gets the monitored channels.
        /// </summary>
        /// <value>
        /// The monitored channels.
        /// </value>
        public int MonitoredChannels
        {
            get { return _monitoredChannels; }
            private set
            {
                _monitoredChannels = value;
                SetLabel(label_monitoredChannels, value.ToString());
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the RadioButton_Channel control. Refresh the available channels when it happens.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void RadioButton_Channel_CheckedChanged(object sender, EventArgs e)
        {
            label_ChannelUser.Text = (radioButton_Channel.Checked ? radioButton_Channel.Text : radioButton_User.Text) +
                                     ':';
            RefreshChannelUserDataSource();
        }

        /// <summary>
        /// Refreshes list of channel.
        /// </summary>
        private void RefreshChannelUserDataSource()
        {
            comboBox_ChannelUser.DataSource =
                _channels[comboBox_Application.Text].Where(x => x.Private == radioButton_User.Checked).ToList();
        }

        /// <summary>
        /// Controls the access on a control over multiple threads.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="action">The action.</param>
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

        /// <summary>
        /// Wrapper to set a label.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="text">The text.</param>
        private void SetLabel(Label label, string text)
        {
            ControlAccess(label, () => { label.Text = text; });
        }

        /// <summary>
        /// Updates since how long the application has been running.
        /// </summary>
        /// <param name="upTime">Up time.</param>
        public void UpdateUpTime(TimeSpan upTime)
        {
            SetLabel(label_Uptime, upTime.ToString(TimeFormat));
        }

        /// <summary>
        /// Updates since when the appplication has been running.
        /// </summary>
        /// <param name="upSince">Up since.</param>
        public void UpdateUpSince(DateTime upSince)
        {
            SetLabel(label_UpSince, upSince.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Refreshes the ping of the selected index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="ping">The ping.</param>
        public void RefreshPing(int index, long? ping)
        {
            SetLabel(_statusPings[index * 2], ping != null ? "Active" : "Inactive");
            SetLabel(_statusPings[index * 2 + 1], (ping ?? 0) + " ms");
        }

        /// <summary>
        /// Handles the Click event of the Button_Send control. Send what was written in the third tab's box.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Button_Send_Click(object sender, EventArgs e)
        {
            Message message = BuildMessage();
            textBox_Send.Clear();
            _sendMessage(message);

            User.LoadUserNames(message.UserToChannel.User);
            AddMessage(message);
        }

        /// <summary>
        /// Builds a message sent with the third tab.
        /// </summary>
        /// <returns></returns>
        private Message BuildMessage()
        {
            Message message = new Message(
                (Entites.Application) comboBox_Application.SelectedItem,
                null,
                DateTime.UtcNow,
                new UserToChannel(
                    (Entites.Application) comboBox_Application.SelectedItem,
                    BLL.User.GetOrCreate(((Entites.Application) comboBox_Application.SelectedItem).GetTerministrator()),
                    (Channel) comboBox_ChannelUser.SelectedItem,
                    DateTime.MinValue,
                    null),
                MessageType.Get("Text"));

            message.Texts = new List<Text> {new Text(textBox_Send.Text, DateTime.UtcNow, message)};

            return message;
        }

        /// <summary>
        /// Adds the message in the third tab.
        /// </summary>
        /// <param name="message">The message.</param>
        internal void AddMessage(Message message)
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

            Action maybeAddText = delegate
            {
                if (((Channel) comboBox_ChannelUser.SelectedItem)?.NamableId == message.UserToChannel.ChannelId)
                {
                    textBox_Interact.AppendText((textBox_Interact.Text.Length > 0 ? "\r\n" : "") + message);
                }
            };

            ControlAccess(textBox_Interact, maybeAddText);
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ComboBox_ChannelUser control. Shows the new channel in the text box when it happens.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ComboBox_ChannelUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = ((Channel) comboBox_ChannelUser.SelectedItem).NamableId;
            textBox_Interact.Text = _channelDiscussions.ContainsKey(id)
                ? _channelDiscussions[((Channel) comboBox_ChannelUser.SelectedItem).NamableId].ToString()
                : "";
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the comboBox_Application control. Refreshes the available channels when it happens.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ComboBox_Application_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshChannelUserDataSource();
        }

        /// <summary>
        /// Adds the client to the third tab.
        /// </summary>
        /// <param name="client">The client.</param>
        internal void AddClient(Entites.Application client)
        {
            if (_channels.ContainsKey(client.ApplicationName))
            {
                return;
            }

            _channels.Add(client.ApplicationName, new List<Channel>());
            ControlAccess(comboBox_Application, () =>
            {
                comboBox_Application.Items.Add(client);
                if (comboBox_Application.Items.Count == 1)
                {
                    comboBox_Application.SelectedIndex = 0;
                }
            });
        }

        /// <summary>
        /// Adds the channels.
        /// </summary>
        /// <param name="channels">The channels.</param>
        internal void AddChannels(List<Channel> channels)
        {
            channels.ForEach(AddChannel);
        }

        /// <summary>
        /// Adds the channel in the third tab and count it in the first one.
        /// </summary>
        /// <param name="channel">The channel.</param>
        internal void AddChannel(Channel channel)
        {
            if (_channels[channel.ApplicationName].Exists(x => x.NamableId == channel.NamableId))
            {
                return;
            }

            _channels[channel.ApplicationName].Add(channel);
            MonitoredChannels++;
            if (channel.Private == radioButton_User.Checked &&
                comboBox_Application.Text.Equals(channel.ApplicationName, StringComparison.InvariantCultureIgnoreCase))
            {
                RefreshChannelUserDataSource();
            }
        }

        /// <summary>
        /// Logs the specified data.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="args">The <see cref="Logger.LoggingRequestedEventArgs"/> instance containing the event data.</param>
        public void Log(object logger, Logger.LoggingRequestedEventArgs args)
        {
            Log($"{GetPaddedRating(RatingToString(args.Rating))} at " +
                $"{DateTime.Now} located in {args.CallerFilePath?.Substring(args.CallerFilePath.LastIndexOf('\\') + 1)} " +
                $"{(args.CallerMemberName != null ? $"{args.CallerMemberName} " : "")}" +
                $"line {args.CallerLineNumber}{(args.Text != null ? ": " + args.Text : ".")}" +
                $"{(args.Exception != null ? $"\r\n{args.Exception}" : "")}");
        }

        /// <summary>
        /// Change the enum to a showable string.
        /// </summary>
        /// <param name="rating">The rating.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the padded rating.
        /// </summary>
        /// <param name="rating">The rating.</param>
        /// <returns></returns>
        private string GetPaddedRating(string rating)
        {
            return new string(' ', RatingToString(Logger.Rating.Warning).Length - rating.Length) + rating;
        }

        /// <summary>
        /// Logs the specified string in the second tab.
        /// </summary>
        /// <param name="str">The string.</param>
        private void Log(string str)
        {
            ControlAccess(textBox_log,
                () => { textBox_log.AppendText($"{(textBox_log.Text.Length > 0 ? "\r\n" : "")}{str}"); });
        }

        /// <summary>
        /// Handles the Click event of the button_Clear control. Clears the logs on the second tab.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Button_Clear_Click(object sender, EventArgs e)
        {
            ControlAccess(textBox_log, () => { textBox_log.Clear(); });
        }
    }
}