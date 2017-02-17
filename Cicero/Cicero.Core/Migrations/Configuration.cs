namespace Cicero.Core.Migrations
{
    using DataObjects;
    using Helpers.Membership;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Cicero.Core.DataObjects.CiceroContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Cicero.Core.DataObjects.CiceroContext context)
        {
            var ph = new PasswordHelper();
            var users = new List<User>()
            {
                new User()
                {
                    Username = "Tester",
                    Firstname = "Test",
                    Lastname  = "Machine",
                    Email = "bert.limerkens@gmail.com",
                    Password = ph.HashPassword("tester01"),
                    Created = DateTime.Now,
                }
            };

            users.ForEach(s => context.Users.AddOrUpdate(p => p.Username, s));
            context.SaveChanges();

        }
    }
}
