namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class genrescript : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Gigs", "Venue", c => c.String(nullable: false, maxLength: 255));
            Sql("INSERT INTO Genres (Id, Name) Values (1,'Jazz')");
            Sql("INSERT INTO Genres (Id, Name) Values (2,'Blues')");
            Sql("INSERT INTO Genres (Id, Name) Values (3,'Rock')");
            Sql("INSERT INTO Genres (Id, Name) Values (4,'Country')");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM Genres WHERE Id in(1,2,3,4)");
        
        AlterColumn("dbo.Gigs", "Venue", c => c.String(nullable: false, maxLength: 250));

        }
    }
}
