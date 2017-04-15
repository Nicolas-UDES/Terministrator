namespace Terministrator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Privileges", "Default", c => c.Boolean(nullable: false));
            DropColumn("dbo.Privileges", "DefaultMod");
            DropColumn("dbo.Privileges", "DefaultUser");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Privileges", "DefaultUser", c => c.Boolean(nullable: false));
            AddColumn("dbo.Privileges", "DefaultMod", c => c.Boolean(nullable: false));
            DropColumn("dbo.Privileges", "Default");
        }
    }
}
