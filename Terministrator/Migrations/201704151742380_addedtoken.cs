#region Usings

using System.Data.Entity.Migrations;

#endregion

namespace Terministrator.Migrations
{
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