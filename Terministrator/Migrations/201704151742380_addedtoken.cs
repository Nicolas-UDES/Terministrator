namespace Terministrator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedtoken : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Applications", "Token", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Applications", "Token");
        }
    }
}
