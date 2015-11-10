namespace Rabbit.DataUp.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class V01 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Revisions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Type = c.String(),
                        IntegratedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Revisions");
        }
    }
}
