#region Usings

using System.Data.Entity.Migrations;

#endregion

namespace Terministrator.Migrations
{
    public partial class version2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Privileges", "Default", c => c.Boolean(false));
            DropColumn("dbo.Privileges", "DefaultMod");
            DropColumn("dbo.Privileges", "DefaultUser");
        }

        public override void Down()
        {
            AddColumn("dbo.Privileges", "DefaultUser", c => c.Boolean(false));
            AddColumn("dbo.Privileges", "DefaultMod", c => c.Boolean(false));
            DropColumn("dbo.Privileges", "Default");
        }
    }
}