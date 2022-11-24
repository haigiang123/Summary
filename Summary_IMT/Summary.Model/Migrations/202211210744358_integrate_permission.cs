namespace Summary.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class integrate_permission : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.IdentityRoles", newName: "AppRoles");
            RenameTable(name: "dbo.IdentityUserRoles", newName: "AppUserRoles");
            RenameTable(name: "dbo.IdentityUserClaims", newName: "AppUserClaims");
            RenameTable(name: "dbo.IdentityUserLogins", newName: "AppLogins");
            DropPrimaryKey("dbo.AppUserClaims");
            CreateTable(
                "dbo.AppPermissions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ManualId = c.Int(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AppRolePermissions",
                c => new
                    {
                        RoleID = c.String(nullable: false, maxLength: 128),
                        PermissionId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RoleID, t.PermissionId })
                .ForeignKey("dbo.AppPermissions", t => t.PermissionId, cascadeDelete: true)
                .ForeignKey("dbo.AppRoles", t => t.RoleID, cascadeDelete: true)
                .Index(t => t.RoleID)
                .Index(t => t.PermissionId);
            
            AddColumn("dbo.AppRoles", "ManualId", c => c.Int());
            AddColumn("dbo.AppRoles", "Description", c => c.String());
            AddColumn("dbo.AppRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AppUserClaims", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.AppUserClaims", "UserId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.AppUserClaims", new[] { "Id", "UserId" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppRolePermissions", "RoleID", "dbo.AppRoles");
            DropForeignKey("dbo.AppRolePermissions", "PermissionId", "dbo.AppPermissions");
            DropIndex("dbo.AppRolePermissions", new[] { "PermissionId" });
            DropIndex("dbo.AppRolePermissions", new[] { "RoleID" });
            DropPrimaryKey("dbo.AppUserClaims");
            AlterColumn("dbo.AppUserClaims", "UserId", c => c.String());
            AlterColumn("dbo.AppUserClaims", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.AppRoles", "Discriminator");
            DropColumn("dbo.AppRoles", "Description");
            DropColumn("dbo.AppRoles", "ManualId");
            DropTable("dbo.AppRolePermissions");
            DropTable("dbo.AppPermissions");
            AddPrimaryKey("dbo.AppUserClaims", "Id");
            RenameTable(name: "dbo.AppLogins", newName: "IdentityUserLogins");
            RenameTable(name: "dbo.AppUserClaims", newName: "IdentityUserClaims");
            RenameTable(name: "dbo.AppUserRoles", newName: "IdentityUserRoles");
            RenameTable(name: "dbo.AppRoles", newName: "IdentityRoles");
        }
    }
}
