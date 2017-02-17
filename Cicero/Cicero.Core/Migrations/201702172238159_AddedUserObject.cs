namespace Cicero.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUserObject : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.User",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Firstname = c.String(),
                        Lastname = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        Created = c.DateTime(nullable: false),
                        Modified = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.User");
        }
    }
}
