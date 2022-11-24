namespace Summary.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit_manualid_type : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AppPermissions", "ManualId", c => c.Byte());
            AlterColumn("dbo.AppRoles", "ManualId", c => c.Byte());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AppRoles", "ManualId", c => c.Int());
            AlterColumn("dbo.AppPermissions", "ManualId", c => c.Int());
        }
    }
}
