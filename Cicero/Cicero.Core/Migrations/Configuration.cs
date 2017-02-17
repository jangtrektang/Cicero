namespace Cicero.Core.Migrations
{
    using DataObjects;
    using Helpers;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Cicero.Core.DataObjects.DataContext.CiceroContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Cicero.Core.DataObjects.DataContext.CiceroContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var uh = new UserHelper();
            var users = new List<User>()
            {
                new User
                {
                    Username = "Tester",
                    Firstname = "Test",
                    Lastname = "Machine",
                    Password = uh.HashPassword("tester01"),
                    Email = "bert.limerkens@gmail.com",
                    Created = DateTime.Now
                }
            };

            users.ForEach(s => context.Users.AddOrUpdate(p => p.Username, s));
            context.SaveChanges();
        }
    }
}
