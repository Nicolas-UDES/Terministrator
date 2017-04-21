#region Usings

using System.Data.Entity.Migrations;
// ReSharper disable InconsistentNaming
#pragma warning disable 1591

#endregion

namespace Terministrator.Migrations
{
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "dbo.Ads",
                    c => new
                    {
                        AdId = c.Int(false, true),
                        MaxShow = c.Int(false),
                        Name = c.String(),
                        LastSent = c.DateTime(false),
                        MessageId = c.Int(false),
                        AdSystemId = c.Int(false)
                    })
                .PrimaryKey(t => t.AdId)
                .ForeignKey("dbo.AdSystems", t => t.AdSystemId, true)
                .ForeignKey("dbo.Messages", t => t.MessageId, true)
                .Index(t => t.MessageId)
                .Index(t => t.AdSystemId);

            CreateTable(
                    "dbo.AdSystems",
                    c => new
                    {
                        ChannelId = c.Int(false),
                        MinNbOfMessage = c.Int(false),
                        MinTime = c.Time(false, 7),
                        BothConditions = c.Boolean(false),
                        Message_MessageId = c.Int()
                    })
                .PrimaryKey(t => t.ChannelId)
                .ForeignKey("dbo.Messages", t => t.Message_MessageId)
                .ForeignKey("dbo.Namables", t => t.ChannelId)
                .Index(t => t.ChannelId)
                .Index(t => t.Message_MessageId);

            CreateTable(
                    "dbo.Namables",
                    c => new
                    {
                        NamableId = c.Int(false, true),
                        ApplicationName = c.String(false, 128),
                        IdForApplication = c.String(false, 128),
                        Private = c.Boolean(),
                        Discriminator = c.String(false, 128),
                        Application_ApplicationName = c.String(maxLength: 128),
                        Application_ApplicationName1 = c.String(maxLength: 128)
                    })
                .PrimaryKey(t => t.NamableId)
                .ForeignKey("dbo.Applications", t => t.Application_ApplicationName)
                .ForeignKey("dbo.Applications", t => t.ApplicationName, true)
                .ForeignKey("dbo.Applications", t => t.Application_ApplicationName1)
                .Index(t => new {t.ApplicationName, t.IdForApplication, t.Discriminator}, unique: true,
                    name: "IX_OneInApplication")
                .Index(t => t.Application_ApplicationName)
                .Index(t => t.Application_ApplicationName1);

            CreateTable(
                    "dbo.Applications",
                    c => new
                    {
                        ApplicationName = c.String(false, 128)
                    })
                .PrimaryKey(t => t.ApplicationName);

            CreateTable(
                    "dbo.UserToChannels",
                    c => new
                    {
                        UserToChannelId = c.Int(false, true),
                        ApplicationName = c.String(maxLength: 128),
                        UserId = c.Int(),
                        ChannelId = c.Int(),
                        JoinedOn = c.DateTime(false),
                        PrivilegesId = c.Int(),
                        SilencedTo = c.DateTime(),
                        NbSilences = c.Int(false),
                        Points = c.Single(false)
                    })
                .PrimaryKey(t => t.UserToChannelId)
                .ForeignKey("dbo.Applications", t => t.ApplicationName)
                .ForeignKey("dbo.Namables", t => t.ChannelId)
                .ForeignKey("dbo.Privileges", t => t.PrivilegesId)
                .ForeignKey("dbo.Namables", t => t.UserId)
                .Index(t => t.ApplicationName)
                .Index(t => new {t.UserId, t.ChannelId}, unique: true, name: "IX_OneUserToChannel")
                .Index(t => t.PrivilegesId);

            CreateTable(
                    "dbo.Messages",
                    c => new
                    {
                        MessageId = c.Int(false, true),
                        UserToChannelId = c.Int(false),
                        RepliesToId = c.Int(),
                        MessageTypeId = c.Int(false),
                        SentOn = c.DateTime(false),
                        Deleted = c.Boolean(false),
                        JoinedUserId = c.Int(),
                        ApplicationName = c.String(false, 128),
                        IdForApplication = c.String(false, 128),
                        UserToChannel_UserToChannelId = c.Int()
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.Applications", t => t.ApplicationName, true)
                .ForeignKey("dbo.UserToChannels", t => t.JoinedUserId)
                .ForeignKey("dbo.MessageTypes", t => t.MessageTypeId, true)
                .ForeignKey("dbo.Messages", t => t.RepliesToId)
                .ForeignKey("dbo.UserToChannels", t => t.UserToChannelId, true)
                .ForeignKey("dbo.UserToChannels", t => t.UserToChannel_UserToChannelId)
                .Index(t => t.UserToChannelId)
                .Index(t => t.RepliesToId)
                .Index(t => t.MessageTypeId)
                .Index(t => t.JoinedUserId)
                .Index(t => new {t.ApplicationName, t.IdForApplication}, unique: true, name: "IX_OneInApplication")
                .Index(t => t.UserToChannel_UserToChannelId);

            CreateTable(
                    "dbo.Files",
                    c => new
                    {
                        FileId = c.Int(false, true),
                        FileName = c.String(),
                        Message_MessageId = c.Int(),
                        Extension_ExtensionId = c.Int()
                    })
                .PrimaryKey(t => t.FileId)
                .ForeignKey("dbo.Messages", t => t.Message_MessageId)
                .ForeignKey("dbo.Extensions", t => t.Extension_ExtensionId)
                .Index(t => t.Message_MessageId)
                .Index(t => t.Extension_ExtensionId);

            CreateTable(
                    "dbo.MessageTypes",
                    c => new
                    {
                        MessageTypeId = c.Int(false, true),
                        Name = c.String(maxLength: 128)
                    })
                .PrimaryKey(t => t.MessageTypeId)
                .Index(t => t.Name, unique: true);

            CreateTable(
                    "dbo.MessageTypeToPointSystems",
                    c => new
                    {
                        MessageTypeId = c.Int(false),
                        PointSystemId = c.Int(false),
                        Reward = c.Single(false)
                    })
                .PrimaryKey(t => new {t.MessageTypeId, t.PointSystemId})
                .ForeignKey("dbo.MessageTypes", t => t.MessageTypeId, true)
                .ForeignKey("dbo.PointSystems", t => t.PointSystemId, true)
                .Index(t => t.MessageTypeId)
                .Index(t => t.PointSystemId);

            CreateTable(
                    "dbo.PointSystems",
                    c => new
                    {
                        ChannelId = c.Int(false),
                        Total = c.Single(false),
                        ExchangeEnabled = c.Boolean(false),
                        RewardsEnabled = c.Boolean(false)
                    })
                .PrimaryKey(t => t.ChannelId)
                .ForeignKey("dbo.Namables", t => t.ChannelId)
                .Index(t => t.ChannelId);

            CreateTable(
                    "dbo.Currencies",
                    c => new
                    {
                        CurrenciesId = c.Int(false, true),
                        Name = c.String(maxLength: 100),
                        Value = c.Single(false),
                        PointSystem_ChannelId = c.Int()
                    })
                .PrimaryKey(t => t.CurrenciesId)
                .ForeignKey("dbo.PointSystems", t => t.PointSystem_ChannelId)
                .Index(t => t.Name, unique: true, name: "IX_NamePointSystem")
                .Index(t => t.Value, unique: true, name: "IX_ValuePointSystem")
                .Index(t => t.PointSystem_ChannelId);

            CreateTable(
                    "dbo.Rules",
                    c => new
                    {
                        RulesId = c.Int(false, true),
                        SpamDelay = c.Time(precision: 7),
                        ExtensionBlocked = c.Boolean(false),
                        DomainBlocked = c.Boolean(false),
                        MessageTypeBlocked = c.Boolean(false),
                        BlockedWordsEnabled = c.Boolean(false),
                        R9KEnabled = c.Boolean(false)
                    })
                .PrimaryKey(t => t.RulesId);

            CreateTable(
                    "dbo.BlockedWords",
                    c => new
                    {
                        BlockedWordId = c.Int(false, true),
                        Word = c.String()
                    })
                .PrimaryKey(t => t.BlockedWordId);

            CreateTable(
                    "dbo.Domains",
                    c => new
                    {
                        DomainId = c.Int(false, true),
                        Name = c.String()
                    })
                .PrimaryKey(t => t.DomainId);

            CreateTable(
                    "dbo.Links",
                    c => new
                    {
                        LinkId = c.Int(false, true),
                        URL = c.String(),
                        Message_MessageId = c.Int()
                    })
                .PrimaryKey(t => t.LinkId)
                .ForeignKey("dbo.Messages", t => t.Message_MessageId)
                .Index(t => t.Message_MessageId);

            CreateTable(
                    "dbo.Extensions",
                    c => new
                    {
                        ExtensionId = c.Int(false, true),
                        Name = c.String(),
                        ExtensionCategory_ExtensionCategoryId = c.Int()
                    })
                .PrimaryKey(t => t.ExtensionId)
                .ForeignKey("dbo.ExtensionCategories", t => t.ExtensionCategory_ExtensionCategoryId)
                .Index(t => t.ExtensionCategory_ExtensionCategoryId);

            CreateTable(
                    "dbo.ExtensionCategories",
                    c => new
                    {
                        ExtensionCategoryId = c.Int(false, true),
                        Name = c.String()
                    })
                .PrimaryKey(t => t.ExtensionCategoryId);

            CreateTable(
                    "dbo.Privileges",
                    c => new
                    {
                        PrivilegesId = c.Int(false, true),
                        Name = c.String(false),
                        ChannelId = c.Int(false),
                        RulesId = c.Int(false),
                        DefaultMod = c.Boolean(false),
                        DefaultUser = c.Boolean(false)
                    })
                .PrimaryKey(t => t.PrivilegesId)
                .ForeignKey("dbo.Namables", t => t.ChannelId, true)
                .ForeignKey("dbo.Rules", t => t.RulesId, true)
                .Index(t => t.ChannelId)
                .Index(t => t.RulesId);

            CreateTable(
                    "dbo.Texts",
                    c => new
                    {
                        TextId = c.Int(false, true),
                        MessageId = c.Int(false),
                        SimilarTextsId = c.Int(),
                        ZeText = c.String(),
                        SetOn = c.DateTime(false),
                        R9KText = c.String()
                    })
                .PrimaryKey(t => t.TextId)
                .ForeignKey("dbo.Messages", t => t.MessageId, true)
                .ForeignKey("dbo.SimilarTexts", t => t.SimilarTextsId)
                .Index(t => t.MessageId)
                .Index(t => t.SimilarTextsId);

            CreateTable(
                    "dbo.SimilarTexts",
                    c => new
                    {
                        SimilarMessagesId = c.Int(false, true),
                        NBSimilar = c.Int(false),
                        LastIncrement = c.DateTime(false)
                    })
                .PrimaryKey(t => t.SimilarMessagesId);

            CreateTable(
                    "dbo.UserNames",
                    c => new
                    {
                        UserNameId = c.Int(false, true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Username = c.String(),
                        Current = c.Boolean(false),
                        ChangedOn = c.DateTime(false),
                        OwnedById = c.Int(false)
                    })
                .PrimaryKey(t => t.UserNameId)
                .ForeignKey("dbo.Namables", t => t.OwnedById, true)
                .Index(t => t.OwnedById);

            CreateTable(
                    "dbo.PointsLogs",
                    c => new
                    {
                        PointsLogId = c.Int(false, true),
                        Amount = c.Single(false),
                        Comment = c.String(),
                        From_UserToChannelId = c.Int(),
                        PointsLogReason_PointsLogReasonId = c.Int(),
                        To_UserToChannelId = c.Int(false)
                    })
                .PrimaryKey(t => t.PointsLogId)
                .ForeignKey("dbo.UserToChannels", t => t.From_UserToChannelId)
                .ForeignKey("dbo.PointsLogReasons", t => t.PointsLogReason_PointsLogReasonId)
                .ForeignKey("dbo.UserToChannels", t => t.To_UserToChannelId, true)
                .Index(t => t.From_UserToChannelId)
                .Index(t => t.PointsLogReason_PointsLogReasonId)
                .Index(t => t.To_UserToChannelId);

            CreateTable(
                    "dbo.PointsLogReasons",
                    c => new
                    {
                        PointsLogReasonId = c.Int(false, true),
                        Text = c.String()
                    })
                .PrimaryKey(t => t.PointsLogReasonId);

            CreateTable(
                    "dbo.BlockedWordRules",
                    c => new
                    {
                        BlockedWord_BlockedWordId = c.Int(false),
                        Rules_RulesId = c.Int(false)
                    })
                .PrimaryKey(t => new {t.BlockedWord_BlockedWordId, t.Rules_RulesId})
                .ForeignKey("dbo.BlockedWords", t => t.BlockedWord_BlockedWordId, true)
                .ForeignKey("dbo.Rules", t => t.Rules_RulesId, true)
                .Index(t => t.BlockedWord_BlockedWordId)
                .Index(t => t.Rules_RulesId);

            CreateTable(
                    "dbo.LinkDomains",
                    c => new
                    {
                        Link_LinkId = c.Int(false),
                        Domain_DomainId = c.Int(false)
                    })
                .PrimaryKey(t => new {t.Link_LinkId, t.Domain_DomainId})
                .ForeignKey("dbo.Links", t => t.Link_LinkId, true)
                .ForeignKey("dbo.Domains", t => t.Domain_DomainId, true)
                .Index(t => t.Link_LinkId)
                .Index(t => t.Domain_DomainId);

            CreateTable(
                    "dbo.DomainRules",
                    c => new
                    {
                        Domain_DomainId = c.Int(false),
                        Rules_RulesId = c.Int(false)
                    })
                .PrimaryKey(t => new {t.Domain_DomainId, t.Rules_RulesId})
                .ForeignKey("dbo.Domains", t => t.Domain_DomainId, true)
                .ForeignKey("dbo.Rules", t => t.Rules_RulesId, true)
                .Index(t => t.Domain_DomainId)
                .Index(t => t.Rules_RulesId);

            CreateTable(
                    "dbo.ExtensionRules",
                    c => new
                    {
                        Extension_ExtensionId = c.Int(false),
                        Rules_RulesId = c.Int(false)
                    })
                .PrimaryKey(t => new {t.Extension_ExtensionId, t.Rules_RulesId})
                .ForeignKey("dbo.Extensions", t => t.Extension_ExtensionId, true)
                .ForeignKey("dbo.Rules", t => t.Rules_RulesId, true)
                .Index(t => t.Extension_ExtensionId)
                .Index(t => t.Rules_RulesId);

            CreateTable(
                    "dbo.RulesMessageTypes",
                    c => new
                    {
                        Rules_RulesId = c.Int(false),
                        MessageType_MessageTypeId = c.Int(false)
                    })
                .PrimaryKey(t => new {t.Rules_RulesId, t.MessageType_MessageTypeId})
                .ForeignKey("dbo.Rules", t => t.Rules_RulesId, true)
                .ForeignKey("dbo.MessageTypes", t => t.MessageType_MessageTypeId, true)
                .Index(t => t.Rules_RulesId)
                .Index(t => t.MessageType_MessageTypeId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.PointsLogs", "To_UserToChannelId", "dbo.UserToChannels");
            DropForeignKey("dbo.PointsLogs", "PointsLogReason_PointsLogReasonId", "dbo.PointsLogReasons");
            DropForeignKey("dbo.PointsLogs", "From_UserToChannelId", "dbo.UserToChannels");
            DropForeignKey("dbo.Ads", "MessageId", "dbo.Messages");
            DropForeignKey("dbo.Ads", "AdSystemId", "dbo.AdSystems");
            DropForeignKey("dbo.AdSystems", "ChannelId", "dbo.Namables");
            DropForeignKey("dbo.Namables", "Application_ApplicationName1", "dbo.Applications");
            DropForeignKey("dbo.UserNames", "OwnedById", "dbo.Namables");
            DropForeignKey("dbo.Namables", "ApplicationName", "dbo.Applications");
            DropForeignKey("dbo.UserToChannels", "UserId", "dbo.Namables");
            DropForeignKey("dbo.UserToChannels", "PrivilegesId", "dbo.Privileges");
            DropForeignKey("dbo.Messages", "UserToChannel_UserToChannelId", "dbo.UserToChannels");
            DropForeignKey("dbo.Messages", "UserToChannelId", "dbo.UserToChannels");
            DropForeignKey("dbo.Texts", "SimilarTextsId", "dbo.SimilarTexts");
            DropForeignKey("dbo.Texts", "MessageId", "dbo.Messages");
            DropForeignKey("dbo.Messages", "RepliesToId", "dbo.Messages");
            DropForeignKey("dbo.Messages", "MessageTypeId", "dbo.MessageTypes");
            DropForeignKey("dbo.Privileges", "RulesId", "dbo.Rules");
            DropForeignKey("dbo.Privileges", "ChannelId", "dbo.Namables");
            DropForeignKey("dbo.RulesMessageTypes", "MessageType_MessageTypeId", "dbo.MessageTypes");
            DropForeignKey("dbo.RulesMessageTypes", "Rules_RulesId", "dbo.Rules");
            DropForeignKey("dbo.ExtensionRules", "Rules_RulesId", "dbo.Rules");
            DropForeignKey("dbo.ExtensionRules", "Extension_ExtensionId", "dbo.Extensions");
            DropForeignKey("dbo.Files", "Extension_ExtensionId", "dbo.Extensions");
            DropForeignKey("dbo.Extensions", "ExtensionCategory_ExtensionCategoryId", "dbo.ExtensionCategories");
            DropForeignKey("dbo.DomainRules", "Rules_RulesId", "dbo.Rules");
            DropForeignKey("dbo.DomainRules", "Domain_DomainId", "dbo.Domains");
            DropForeignKey("dbo.Links", "Message_MessageId", "dbo.Messages");
            DropForeignKey("dbo.LinkDomains", "Domain_DomainId", "dbo.Domains");
            DropForeignKey("dbo.LinkDomains", "Link_LinkId", "dbo.Links");
            DropForeignKey("dbo.BlockedWordRules", "Rules_RulesId", "dbo.Rules");
            DropForeignKey("dbo.BlockedWordRules", "BlockedWord_BlockedWordId", "dbo.BlockedWords");
            DropForeignKey("dbo.MessageTypeToPointSystems", "PointSystemId", "dbo.PointSystems");
            DropForeignKey("dbo.Currencies", "PointSystem_ChannelId", "dbo.PointSystems");
            DropForeignKey("dbo.PointSystems", "ChannelId", "dbo.Namables");
            DropForeignKey("dbo.MessageTypeToPointSystems", "MessageTypeId", "dbo.MessageTypes");
            DropForeignKey("dbo.Messages", "JoinedUserId", "dbo.UserToChannels");
            DropForeignKey("dbo.Files", "Message_MessageId", "dbo.Messages");
            DropForeignKey("dbo.Messages", "ApplicationName", "dbo.Applications");
            DropForeignKey("dbo.AdSystems", "Message_MessageId", "dbo.Messages");
            DropForeignKey("dbo.UserToChannels", "ChannelId", "dbo.Namables");
            DropForeignKey("dbo.UserToChannels", "ApplicationName", "dbo.Applications");
            DropForeignKey("dbo.Namables", "Application_ApplicationName", "dbo.Applications");
            DropIndex("dbo.RulesMessageTypes", new[] {"MessageType_MessageTypeId"});
            DropIndex("dbo.RulesMessageTypes", new[] {"Rules_RulesId"});
            DropIndex("dbo.ExtensionRules", new[] {"Rules_RulesId"});
            DropIndex("dbo.ExtensionRules", new[] {"Extension_ExtensionId"});
            DropIndex("dbo.DomainRules", new[] {"Rules_RulesId"});
            DropIndex("dbo.DomainRules", new[] {"Domain_DomainId"});
            DropIndex("dbo.LinkDomains", new[] {"Domain_DomainId"});
            DropIndex("dbo.LinkDomains", new[] {"Link_LinkId"});
            DropIndex("dbo.BlockedWordRules", new[] {"Rules_RulesId"});
            DropIndex("dbo.BlockedWordRules", new[] {"BlockedWord_BlockedWordId"});
            DropIndex("dbo.PointsLogs", new[] {"To_UserToChannelId"});
            DropIndex("dbo.PointsLogs", new[] {"PointsLogReason_PointsLogReasonId"});
            DropIndex("dbo.PointsLogs", new[] {"From_UserToChannelId"});
            DropIndex("dbo.UserNames", new[] {"OwnedById"});
            DropIndex("dbo.Texts", new[] {"SimilarTextsId"});
            DropIndex("dbo.Texts", new[] {"MessageId"});
            DropIndex("dbo.Privileges", new[] {"RulesId"});
            DropIndex("dbo.Privileges", new[] {"ChannelId"});
            DropIndex("dbo.Extensions", new[] {"ExtensionCategory_ExtensionCategoryId"});
            DropIndex("dbo.Links", new[] {"Message_MessageId"});
            DropIndex("dbo.Currencies", new[] {"PointSystem_ChannelId"});
            DropIndex("dbo.Currencies", "IX_ValuePointSystem");
            DropIndex("dbo.Currencies", "IX_NamePointSystem");
            DropIndex("dbo.PointSystems", new[] {"ChannelId"});
            DropIndex("dbo.MessageTypeToPointSystems", new[] {"PointSystemId"});
            DropIndex("dbo.MessageTypeToPointSystems", new[] {"MessageTypeId"});
            DropIndex("dbo.MessageTypes", new[] {"Name"});
            DropIndex("dbo.Files", new[] {"Extension_ExtensionId"});
            DropIndex("dbo.Files", new[] {"Message_MessageId"});
            DropIndex("dbo.Messages", new[] {"UserToChannel_UserToChannelId"});
            DropIndex("dbo.Messages", "IX_OneInApplication");
            DropIndex("dbo.Messages", new[] {"JoinedUserId"});
            DropIndex("dbo.Messages", new[] {"MessageTypeId"});
            DropIndex("dbo.Messages", new[] {"RepliesToId"});
            DropIndex("dbo.Messages", new[] {"UserToChannelId"});
            DropIndex("dbo.UserToChannels", new[] {"PrivilegesId"});
            DropIndex("dbo.UserToChannels", "IX_OneUserToChannel");
            DropIndex("dbo.UserToChannels", new[] {"ApplicationName"});
            DropIndex("dbo.Namables", new[] {"Application_ApplicationName1"});
            DropIndex("dbo.Namables", new[] {"Application_ApplicationName"});
            DropIndex("dbo.Namables", "IX_OneInApplication");
            DropIndex("dbo.AdSystems", new[] {"Message_MessageId"});
            DropIndex("dbo.AdSystems", new[] {"ChannelId"});
            DropIndex("dbo.Ads", new[] {"AdSystemId"});
            DropIndex("dbo.Ads", new[] {"MessageId"});
            DropTable("dbo.RulesMessageTypes");
            DropTable("dbo.ExtensionRules");
            DropTable("dbo.DomainRules");
            DropTable("dbo.LinkDomains");
            DropTable("dbo.BlockedWordRules");
            DropTable("dbo.PointsLogReasons");
            DropTable("dbo.PointsLogs");
            DropTable("dbo.UserNames");
            DropTable("dbo.SimilarTexts");
            DropTable("dbo.Texts");
            DropTable("dbo.Privileges");
            DropTable("dbo.ExtensionCategories");
            DropTable("dbo.Extensions");
            DropTable("dbo.Links");
            DropTable("dbo.Domains");
            DropTable("dbo.BlockedWords");
            DropTable("dbo.Rules");
            DropTable("dbo.Currencies");
            DropTable("dbo.PointSystems");
            DropTable("dbo.MessageTypeToPointSystems");
            DropTable("dbo.MessageTypes");
            DropTable("dbo.Files");
            DropTable("dbo.Messages");
            DropTable("dbo.UserToChannels");
            DropTable("dbo.Applications");
            DropTable("dbo.Namables");
            DropTable("dbo.AdSystems");
            DropTable("dbo.Ads");
        }
    }
}