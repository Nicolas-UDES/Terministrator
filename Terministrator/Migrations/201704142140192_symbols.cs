#region Usings

using System.Data.Entity.Migrations;
// ReSharper disable InconsistentNaming
#pragma warning disable 1591

#endregion

namespace Terministrator.Migrations
{
    public partial class symbols : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Applications", "CommandSymbols", c => c.String());
            AddColumn("dbo.Applications", "UserSymbols", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Applications", "UserSymbols");
            DropColumn("dbo.Applications", "CommandSymbols");
        }
    }
}