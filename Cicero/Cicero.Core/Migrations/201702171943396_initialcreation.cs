namespace Cicero.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialcreation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ingredient",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        RecipeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Recipe", t => t.RecipeID, cascadeDelete: true)
                .Index(t => t.RecipeID);
            
            CreateTable(
                "dbo.Recipe",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Photo = c.String(),
                        Created = c.DateTime(nullable: false),
                        Modified = c.DateTime(),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Instruction",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        Recipe_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Recipe", t => t.Recipe_ID)
                .Index(t => t.Recipe_ID);
            
            CreateTable(
                "dbo.Tag",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
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
            
            CreateTable(
                "dbo.TagRecipe",
                c => new
                    {
                        Tag_ID = c.Int(nullable: false),
                        Recipe_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_ID, t.Recipe_ID })
                .ForeignKey("dbo.Tag", t => t.Tag_ID, cascadeDelete: true)
                .ForeignKey("dbo.Recipe", t => t.Recipe_ID, cascadeDelete: true)
                .Index(t => t.Tag_ID)
                .Index(t => t.Recipe_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Recipe", "UserID", "dbo.User");
            DropForeignKey("dbo.TagRecipe", "Recipe_ID", "dbo.Recipe");
            DropForeignKey("dbo.TagRecipe", "Tag_ID", "dbo.Tag");
            DropForeignKey("dbo.Instruction", "Recipe_ID", "dbo.Recipe");
            DropForeignKey("dbo.Ingredient", "RecipeID", "dbo.Recipe");
            DropIndex("dbo.TagRecipe", new[] { "Recipe_ID" });
            DropIndex("dbo.TagRecipe", new[] { "Tag_ID" });
            DropIndex("dbo.Instruction", new[] { "Recipe_ID" });
            DropIndex("dbo.Recipe", new[] { "UserID" });
            DropIndex("dbo.Ingredient", new[] { "RecipeID" });
            DropTable("dbo.TagRecipe");
            DropTable("dbo.User");
            DropTable("dbo.Tag");
            DropTable("dbo.Instruction");
            DropTable("dbo.Recipe");
            DropTable("dbo.Ingredient");
        }
    }
}
