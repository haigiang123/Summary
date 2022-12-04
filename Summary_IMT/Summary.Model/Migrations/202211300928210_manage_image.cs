namespace Summary.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class manage_image : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppUserImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false),
                        Content = c.Binary(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AppUserImages");
        }
    }
}
