namespace Cicero.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedErrorLogging : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ErrorLog",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        Type = c.String(),
                        URL = c.String(),
                        Source = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ErrorLog");
        }
    }
}
