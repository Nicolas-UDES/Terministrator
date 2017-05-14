#region Usings

using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using Terministrator.Terministrator.Entites;

#endregion

namespace Terministrator.Terministrator.DAL
{
    /// <summary>
    /// Context with the database. Managed (and obligatory) by Entity-Framework. 
    /// </summary>
    class TerministratorContext : DbContext
    {
        private static readonly Lazy<Mutex> Mutex = new Lazy<Mutex>(() => new Mutex());

        public TerministratorContext() : this(false)
        {
        }

        public TerministratorContext(bool mutex) : base("Terministrator")
        {
            if (mutex)
            {
                //Mutex.Value.WaitOne();
            }
        }

        public DbSet<Entites.Ad> Ad { get; set; }
        public DbSet<Entites.AdSystem> AdSystem { get; set; }
        public DbSet<Entites.BlockedWord> BlockedWord { get; set; }
        public DbSet<Entites.Channel> Channel { get; set; }
        public DbSet<Entites.Application> Application { get; set; }
        public DbSet<Entites.Currencies> Currencies { get; set; }
        public DbSet<Entites.Command> Commands { get; set; }
        public DbSet<Entites.Domain> Domain { get; set; }
        public DbSet<Entites.Extension> Extension { get; set; }
        public DbSet<Entites.ExtensionCategory> ExtensionCategory { get; set; }
        public DbSet<Entites.File> File { get; set; }
        public DbSet<Entites.Link> Link { get; set; }
        public DbSet<Entites.Message> Message { get; set; }
        public DbSet<Entites.MessageType> MessageType { get; set; }
        public DbSet<Entites.MessageTypeToPointSystem> MessageTypeToPointSystem { get; set; }
        public DbSet<Entites.PointsLog> PointsLog { get; set; }
        public DbSet<Entites.PointsLogReason> PointsLogReason { get; set; }
        public DbSet<Entites.PointSystem> PointSystem { get; set; }
        public DbSet<Entites.Privileges> Privileges { get; set; }
        public DbSet<Entites.Rules> Rules { get; set; }
        public DbSet<Entites.SimilarContent> SimilarTexts { get; set; }
        public DbSet<Entites.Text> Text { get; set; }
        public DbSet<Entites.User> User { get; set; }
        public DbSet<Entites.UserName> UserName { get; set; }
        public DbSet<Entites.UserToChannel> UserToChannel { get; set; }

        public new void Dispose()
        {
            //Mutex.Value.ReleaseMutex();
            base.Dispose();
        }

        /// <summary>
        /// Pings the database.
        /// </summary>
        /// <returns>The time necessary to ping. Null if no connection.</returns>
        public static TimeSpan? Ping()
        {
            DateTime now = DateTime.UtcNow;
            using (TerministratorContext context = new TerministratorContext(true))
            {
                try
                {
                    //Pointless call (less populated table with never any results) just to see the reaction time of the DB
                    context.Application.SqlQuery("SELECT * FROM dbo.Applications WHERE ApplicationName is null;").Count();
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return DateTime.UtcNow - now;
        }
    }
}