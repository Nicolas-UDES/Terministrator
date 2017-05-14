namespace Terministrator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ads",
                c => new
                    {
                        AdId = c.Int(nullable: false, identity: true),
                        MaxShow = c.Int(nullable: false),
                        Name = c.String(),
                        LastSent = c.DateTime(nullable: false),
                        MessageId = c.Int(nullable: false),
                        AdSystemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AdId)
                .ForeignKey("dbo.AdSystems", t => t.AdSystemId, cascadeDelete: true)
                .ForeignKey("dbo.Messages", t => t.MessageId, cascadeDelete: true)
                .Index(t => t.MessageId)
                .Index(t => t.AdSystemId);
            
            CreateTable(
                "dbo.AdSystems",
                c => new
                    {
                        ChannelId = c.Int(nullable: false),
                        MinNbOfMessage = c.Int(nullable: false),
                        MinTime = c.Time(nullable: false, precision: 7),
                        BothConditions = c.Boolean(nullable: false),
                        Message_MessageId = c.Int(),
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
                        NamableId = c.Int(nullable: false, identity: true),
                        ApplicationName = c.String(nullable: false, maxLength: 128),
                        IdForApplication = c.String(nullable: false, maxLength: 128),
                        Private = c.Boolean(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Application_ApplicationName = c.String(maxLength: 128),
                        Application_ApplicationName1 = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.NamableId)
                .ForeignKey("dbo.Applications", t => t.Application_ApplicationName)
                .ForeignKey("dbo.Applications", t => t.ApplicationName, cascadeDelete: true)
                .ForeignKey("dbo.Applications", t => t.Application_ApplicationName1)
                .Index(t => new { t.ApplicationName, t.IdForApplication, t.Discriminator }, unique: true, name: "IX_OneInApplication")
                .Index(t => t.Application_ApplicationName)
                .Index(t => t.Application_ApplicationName1);
            
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        ApplicationName = c.String(nullable: false, maxLength: 128),
                        CommandSymbols = c.String(),
                        UserSymbols = c.String(),
                        Token = c.String(),
                    })
                .PrimaryKey(t => t.ApplicationName);
            
            CreateTable(
                "dbo.UserToChannels",
                c => new
                    {
                        UserToChannelId = c.Int(nullable: false, identity: true),
                        ApplicationName = c.String(maxLength: 128),
                        UserId = c.Int(),
                        ChannelId = c.Int(),
                        JoinedOn = c.DateTime(nullable: false),
                        PrivilegesId = c.Int(),
                        SilencedTo = c.DateTime(),
                        NbSilences = c.Int(nullable: false),
                        Points = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.UserToChannelId)
                .ForeignKey("dbo.Applications", t => t.ApplicationName)
                .ForeignKey("dbo.Namables", t => t.ChannelId)
                .ForeignKey("dbo.Privileges", t => t.PrivilegesId)
                .ForeignKey("dbo.Namables", t => t.UserId)
                .Index(t => t.ApplicationName)
                .Index(t => new { t.UserId, t.ChannelId }, unique: true, name: "IX_OneUserToChannel")
                .Index(t => t.PrivilegesId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        UserToChannelId = c.Int(nullable: false),
                        RepliesToId = c.Int(),
                        MessageTypeId = c.Int(nullable: false),
                        SentOn = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        JoinedUserId = c.Int(),
                        ApplicationName = c.String(nullable: false, maxLength: 128),
                        IdForApplication = c.String(nullable: false, maxLength: 128),
                        UserToChannel_UserToChannelId = c.Int(),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.Applications", t => t.ApplicationName, cascadeDelete: true)
                .ForeignKey("dbo.UserToChannels", t => t.JoinedUserId)
                .ForeignKey("dbo.MessageTypes", t => t.MessageTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Messages", t => t.RepliesToId)
                .ForeignKey("dbo.UserToChannels", t => t.UserToChannelId, cascadeDelete: true)
                .ForeignKey("dbo.UserToChannels", t => t.UserToChannel_UserToChannelId)
                .Index(t => t.UserToChannelId)
                .Index(t => t.RepliesToId)
                .Index(t => t.MessageTypeId)
                .Index(t => t.JoinedUserId)
                .Index(t => new { t.ApplicationName, t.IdForApplication }, unique: true, name: "IX_OneInApplication")
                .Index(t => t.UserToChannel_UserToChannelId);
            
            CreateTable(
                "dbo.MessageContents",
                c => new
                    {
                        MessageContentId = c.Int(nullable: false, identity: true),
                        MessageId = c.Int(nullable: false),
                        SimilarContentId = c.Int(),
                        SetOn = c.DateTime(nullable: false),
                        FileName = c.String(),
                        ZeText = c.String(),
                        R9KText = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Message_MessageId = c.Int(),
                        Extension_ExtensionId = c.Int(),
                        Message_MessageId1 = c.Int(),
                    })
                .PrimaryKey(t => t.MessageContentId)
                .ForeignKey("dbo.Messages", t => t.MessageId, cascadeDelete: true)
                .ForeignKey("dbo.SimilarContents", t => t.SimilarContentId)
                .ForeignKey("dbo.Messages", t => t.Message_MessageId)
                .ForeignKey("dbo.Extensions", t => t.Extension_ExtensionId)
                .ForeignKey("dbo.Messages", t => t.Message_MessageId1)
                .Index(t => t.MessageId)
                .Index(t => t.SimilarContentId)
                .Index(t => t.Message_MessageId)
                .Index(t => t.Extension_ExtensionId)
                .Index(t => t.Message_MessageId1);
            
            CreateTable(
                "dbo.SimilarContents",
                c => new
                    {
                        SimilarMessagesId = c.Int(nullable: false, identity: true),
                        NBSimilar = c.Int(nullable: false),
                        LastIncrement = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SimilarMessagesId);
            
            CreateTable(
                "dbo.MessageTypes",
                c => new
                    {
                        MessageTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MessageTypeId)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.MessageTypeToPointSystems",
                c => new
                    {
                        MessageTypeId = c.Int(nullable: false),
                        PointSystemId = c.Int(nullable: false),
                        Reward = c.Single(nullable: false),
                    })
                .PrimaryKey(t => new { t.MessageTypeId, t.PointSystemId })
                .ForeignKey("dbo.MessageTypes", t => t.MessageTypeId, cascadeDelete: true)
                .ForeignKey("dbo.PointSystems", t => t.PointSystemId, cascadeDelete: true)
                .Index(t => t.MessageTypeId)
                .Index(t => t.PointSystemId);
            
            CreateTable(
                "dbo.PointSystems",
                c => new
                    {
                        ChannelId = c.Int(nullable: false),
                        Total = c.Single(nullable: false),
                        ExchangeEnabled = c.Boolean(nullable: false),
                        RewardsEnabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ChannelId)
                .ForeignKey("dbo.Namables", t => t.ChannelId)
                .Index(t => t.ChannelId);
            
            CreateTable(
                "dbo.Currencies",
                c => new
                    {
                        CurrenciesId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        Value = c.Single(nullable: false),
                        PointSystem_ChannelId = c.Int(),
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
                        RulesId = c.Int(nullable: false, identity: true),
                        SpamDelay = c.Time(precision: 7),
                        ExtensionBlocked = c.Boolean(nullable: false),
                        DomainBlocked = c.Boolean(nullable: false),
                        MessageTypeBlocked = c.Boolean(nullable: false),
                        BlockedWordsEnabled = c.Boolean(nullable: false),
                        R9KEnabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RulesId);
            
            CreateTable(
                "dbo.BlockedWords",
                c => new
                    {
                        BlockedWordId = c.Int(nullable: false, identity: true),
                        Word = c.String(),
                    })
                .PrimaryKey(t => t.BlockedWordId);
            
            CreateTable(
                "dbo.Domains",
                c => new
                    {
                        DomainId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.DomainId);
            
            CreateTable(
                "dbo.Links",
                c => new
                    {
                        LinkId = c.Int(nullable: false, identity: true),
                        URL = c.String(),
                        Text_MessageContentId = c.Int(),
                    })
                .PrimaryKey(t => t.LinkId)
                .ForeignKey("dbo.MessageContents", t => t.Text_MessageContentId)
                .Index(t => t.Text_MessageContentId);
            
            CreateTable(
                "dbo.Extensions",
                c => new
                    {
                        ExtensionId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ExtensionCategory_ExtensionCategoryId = c.Int(),
                    })
                .PrimaryKey(t => t.ExtensionId)
                .ForeignKey("dbo.ExtensionCategories", t => t.ExtensionCategory_ExtensionCategoryId)
                .Index(t => t.ExtensionCategory_ExtensionCategoryId);
            
            CreateTable(
                "dbo.ExtensionCategories",
                c => new
                    {
                        ExtensionCategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ExtensionCategoryId);
            
            CreateTable(
                "dbo.Privileges",
                c => new
                    {
                        PrivilegesId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ChannelId = c.Int(nullable: false),
                        RulesId = c.Int(nullable: false),
                        Default = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PrivilegesId)
                .ForeignKey("dbo.Namables", t => t.ChannelId, cascadeDelete: true)
                .ForeignKey("dbo.Rules", t => t.RulesId, cascadeDelete: true)
                .Index(t => t.ChannelId)
                .Index(t => t.RulesId);
            
            CreateTable(
                "dbo.UserNames",
                c => new
                    {
                        UserNameId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Username = c.String(),
                        Current = c.Boolean(nullable: false),
                        ChangedOn = c.DateTime(nullable: false),
                        OwnedById = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserNameId)
                .ForeignKey("dbo.Namables", t => t.OwnedById, cascadeDelete: true)
                .Index(t => t.OwnedById);
            
            CreateTable(
                "dbo.Commands",
                c => new
                    {
                        MessageId = c.Int(nullable: false),
                        Name = c.String(),
                        Arguement = c.String(),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.Messages", t => t.MessageId)
                .Index(t => t.MessageId);
            
            CreateTable(
                "dbo.PointsLogs",
                c => new
                    {
                        PointsLogId = c.Int(nullable: false, identity: true),
                        Amount = c.Single(nullable: false),
                        Comment = c.String(),
                        From_UserToChannelId = c.Int(),
                        PointsLogReason_PointsLogReasonId = c.Int(),
                        To_UserToChannelId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PointsLogId)
                .ForeignKey("dbo.UserToChannels", t => t.From_UserToChannelId)
                .ForeignKey("dbo.PointsLogReasons", t => t.PointsLogReason_PointsLogReasonId)
                .ForeignKey("dbo.UserToChannels", t => t.To_UserToChannelId, cascadeDelete: true)
                .Index(t => t.From_UserToChannelId)
                .Index(t => t.PointsLogReason_PointsLogReasonId)
                .Index(t => t.To_UserToChannelId);
            
            CreateTable(
                "dbo.PointsLogReasons",
                c => new
                    {
                        PointsLogReasonId = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.PointsLogReasonId);
            
            CreateTable(
                "dbo.BlockedWordRules",
                c => new
                    {
                        BlockedWord_BlockedWordId = c.Int(nullable: false),
                        Rules_RulesId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BlockedWord_BlockedWordId, t.Rules_RulesId })
                .ForeignKey("dbo.BlockedWords", t => t.BlockedWord_BlockedWordId, cascadeDelete: true)
                .ForeignKey("dbo.Rules", t => t.Rules_RulesId, cascadeDelete: true)
                .Index(t => t.BlockedWord_BlockedWordId)
                .Index(t => t.Rules_RulesId);
            
            CreateTable(
                "dbo.LinkDomains",
                c => new
                    {
                        Link_LinkId = c.Int(nullable: false),
                        Domain_DomainId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Link_LinkId, t.Domain_DomainId })
                .ForeignKey("dbo.Links", t => t.Link_LinkId, cascadeDelete: true)
                .ForeignKey("dbo.Domains", t => t.Domain_DomainId, cascadeDelete: true)
                .Index(t => t.Link_LinkId)
                .Index(t => t.Domain_DomainId);
            
            CreateTable(
                "dbo.DomainRules",
                c => new
                    {
                        Domain_DomainId = c.Int(nullable: false),
                        Rules_RulesId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Domain_DomainId, t.Rules_RulesId })
                .ForeignKey("dbo.Domains", t => t.Domain_DomainId, cascadeDelete: true)
                .ForeignKey("dbo.Rules", t => t.Rules_RulesId, cascadeDelete: true)
                .Index(t => t.Domain_DomainId)
                .Index(t => t.Rules_RulesId);
            
            CreateTable(
                "dbo.ExtensionRules",
                c => new
                    {
                        Extension_ExtensionId = c.Int(nullable: false),
                        Rules_RulesId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Extension_ExtensionId, t.Rules_RulesId })
                .ForeignKey("dbo.Extensions", t => t.Extension_ExtensionId, cascadeDelete: true)
                .ForeignKey("dbo.Rules", t => t.Rules_RulesId, cascadeDelete: true)
                .Index(t => t.Extension_ExtensionId)
                .Index(t => t.Rules_RulesId);
            
            CreateTable(
                "dbo.RulesMessageTypes",
                c => new
                    {
                        Rules_RulesId = c.Int(nullable: false),
                        MessageType_MessageTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Rules_RulesId, t.MessageType_MessageTypeId })
                .ForeignKey("dbo.Rules", t => t.Rules_RulesId, cascadeDelete: true)
                .ForeignKey("dbo.MessageTypes", t => t.MessageType_MessageTypeId, cascadeDelete: true)
                .Index(t => t.Rules_RulesId)
                .Index(t => t.MessageType_MessageTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PointsLogs", "To_UserToChannelId", "dbo.UserToChannels");
            DropForeignKey("dbo.PointsLogs", "PointsLogReason_PointsLogReasonId", "dbo.PointsLogReasons");
            DropForeignKey("dbo.PointsLogs", "From_UserToChannelId", "dbo.UserToChannels");
            DropForeignKey("dbo.Commands", "MessageId", "dbo.Messages");
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
            DropForeignKey("dbo.MessageContents", "Message_MessageId1", "dbo.Messages");
            DropForeignKey("dbo.Messages", "RepliesToId", "dbo.Messages");
            DropForeignKey("dbo.Messages", "MessageTypeId", "dbo.MessageTypes");
            DropForeignKey("dbo.Privileges", "RulesId", "dbo.Rules");
            DropForeignKey("dbo.Privileges", "ChannelId", "dbo.Namables");
            DropForeignKey("dbo.RulesMessageTypes", "MessageType_MessageTypeId", "dbo.MessageTypes");
            DropForeignKey("dbo.RulesMessageTypes", "Rules_RulesId", "dbo.Rules");
            DropForeignKey("dbo.ExtensionRules", "Rules_RulesId", "dbo.Rules");
            DropForeignKey("dbo.ExtensionRules", "Extension_ExtensionId", "dbo.Extensions");
            DropForeignKey("dbo.MessageContents", "Extension_ExtensionId", "dbo.Extensions");
            DropForeignKey("dbo.Extensions", "ExtensionCategory_ExtensionCategoryId", "dbo.ExtensionCategories");
            DropForeignKey("dbo.DomainRules", "Rules_RulesId", "dbo.Rules");
            DropForeignKey("dbo.DomainRules", "Domain_DomainId", "dbo.Domains");
            DropForeignKey("dbo.Links", "Text_MessageContentId", "dbo.MessageContents");
            DropForeignKey("dbo.LinkDomains", "Domain_DomainId", "dbo.Domains");
            DropForeignKey("dbo.LinkDomains", "Link_LinkId", "dbo.Links");
            DropForeignKey("dbo.BlockedWordRules", "Rules_RulesId", "dbo.Rules");
            DropForeignKey("dbo.BlockedWordRules", "BlockedWord_BlockedWordId", "dbo.BlockedWords");
            DropForeignKey("dbo.MessageTypeToPointSystems", "PointSystemId", "dbo.PointSystems");
            DropForeignKey("dbo.Currencies", "PointSystem_ChannelId", "dbo.PointSystems");
            DropForeignKey("dbo.PointSystems", "ChannelId", "dbo.Namables");
            DropForeignKey("dbo.MessageTypeToPointSystems", "MessageTypeId", "dbo.MessageTypes");
            DropForeignKey("dbo.Messages", "JoinedUserId", "dbo.UserToChannels");
            DropForeignKey("dbo.MessageContents", "Message_MessageId", "dbo.Messages");
            DropForeignKey("dbo.MessageContents", "SimilarContentId", "dbo.SimilarContents");
            DropForeignKey("dbo.MessageContents", "MessageId", "dbo.Messages");
            DropForeignKey("dbo.Messages", "ApplicationName", "dbo.Applications");
            DropForeignKey("dbo.AdSystems", "Message_MessageId", "dbo.Messages");
            DropForeignKey("dbo.UserToChannels", "ChannelId", "dbo.Namables");
            DropForeignKey("dbo.UserToChannels", "ApplicationName", "dbo.Applications");
            DropForeignKey("dbo.Namables", "Application_ApplicationName", "dbo.Applications");
            DropIndex("dbo.RulesMessageTypes", new[] { "MessageType_MessageTypeId" });
            DropIndex("dbo.RulesMessageTypes", new[] { "Rules_RulesId" });
            DropIndex("dbo.ExtensionRules", new[] { "Rules_RulesId" });
            DropIndex("dbo.ExtensionRules", new[] { "Extension_ExtensionId" });
            DropIndex("dbo.DomainRules", new[] { "Rules_RulesId" });
            DropIndex("dbo.DomainRules", new[] { "Domain_DomainId" });
            DropIndex("dbo.LinkDomains", new[] { "Domain_DomainId" });
            DropIndex("dbo.LinkDomains", new[] { "Link_LinkId" });
            DropIndex("dbo.BlockedWordRules", new[] { "Rules_RulesId" });
            DropIndex("dbo.BlockedWordRules", new[] { "BlockedWord_BlockedWordId" });
            DropIndex("dbo.PointsLogs", new[] { "To_UserToChannelId" });
            DropIndex("dbo.PointsLogs", new[] { "PointsLogReason_PointsLogReasonId" });
            DropIndex("dbo.PointsLogs", new[] { "From_UserToChannelId" });
            DropIndex("dbo.Commands", new[] { "MessageId" });
            DropIndex("dbo.UserNames", new[] { "OwnedById" });
            DropIndex("dbo.Privileges", new[] { "RulesId" });
            DropIndex("dbo.Privileges", new[] { "ChannelId" });
            DropIndex("dbo.Extensions", new[] { "ExtensionCategory_ExtensionCategoryId" });
            DropIndex("dbo.Links", new[] { "Text_MessageContentId" });
            DropIndex("dbo.Currencies", new[] { "PointSystem_ChannelId" });
            DropIndex("dbo.Currencies", "IX_ValuePointSystem");
            DropIndex("dbo.Currencies", "IX_NamePointSystem");
            DropIndex("dbo.PointSystems", new[] { "ChannelId" });
            DropIndex("dbo.MessageTypeToPointSystems", new[] { "PointSystemId" });
            DropIndex("dbo.MessageTypeToPointSystems", new[] { "MessageTypeId" });
            DropIndex("dbo.MessageTypes", new[] { "Name" });
            DropIndex("dbo.MessageContents", new[] { "Message_MessageId1" });
            DropIndex("dbo.MessageContents", new[] { "Extension_ExtensionId" });
            DropIndex("dbo.MessageContents", new[] { "Message_MessageId" });
            DropIndex("dbo.MessageContents", new[] { "SimilarContentId" });
            DropIndex("dbo.MessageContents", new[] { "MessageId" });
            DropIndex("dbo.Messages", new[] { "UserToChannel_UserToChannelId" });
            DropIndex("dbo.Messages", "IX_OneInApplication");
            DropIndex("dbo.Messages", new[] { "JoinedUserId" });
            DropIndex("dbo.Messages", new[] { "MessageTypeId" });
            DropIndex("dbo.Messages", new[] { "RepliesToId" });
            DropIndex("dbo.Messages", new[] { "UserToChannelId" });
            DropIndex("dbo.UserToChannels", new[] { "PrivilegesId" });
            DropIndex("dbo.UserToChannels", "IX_OneUserToChannel");
            DropIndex("dbo.UserToChannels", new[] { "ApplicationName" });
            DropIndex("dbo.Namables", new[] { "Application_ApplicationName1" });
            DropIndex("dbo.Namables", new[] { "Application_ApplicationName" });
            DropIndex("dbo.Namables", "IX_OneInApplication");
            DropIndex("dbo.AdSystems", new[] { "Message_MessageId" });
            DropIndex("dbo.AdSystems", new[] { "ChannelId" });
            DropIndex("dbo.Ads", new[] { "AdSystemId" });
            DropIndex("dbo.Ads", new[] { "MessageId" });
            DropTable("dbo.RulesMessageTypes");
            DropTable("dbo.ExtensionRules");
            DropTable("dbo.DomainRules");
            DropTable("dbo.LinkDomains");
            DropTable("dbo.BlockedWordRules");
            DropTable("dbo.PointsLogReasons");
            DropTable("dbo.PointsLogs");
            DropTable("dbo.Commands");
            DropTable("dbo.UserNames");
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
            DropTable("dbo.SimilarContents");
            DropTable("dbo.MessageContents");
            DropTable("dbo.Messages");
            DropTable("dbo.UserToChannels");
            DropTable("dbo.Applications");
            DropTable("dbo.Namables");
            DropTable("dbo.AdSystems");
            DropTable("dbo.Ads");
        }
    }
}
