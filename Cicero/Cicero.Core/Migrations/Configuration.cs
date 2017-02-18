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

            var clients = new List<Client>()
            {
                new Client()
                {
                    ID = "angularApp",
                    Secret = ph.HashPassword("angularversie2"),
                    Name = "Angular front-end Application",
                    ApplicationType = ApplicationTypes.JavaScript,
                    Active = true,
                    RefreshTokenLifeTime = 7200,
                    AllowedOrigin = "*"
                },
                new Client()
                {
                    ID = "consoleApp",
                    Secret = ph.HashPassword("consoleapp"),
                    Name = "Console Application",
                    ApplicationType = ApplicationTypes.NativeConfidential,
                    Active = true,
                    RefreshTokenLifeTime = 7200,
                    AllowedOrigin = "*"
                }
            };

            clients.ForEach(s => context.Clients.AddOrUpdate(p => p.ID, s));
            context.SaveChanges();

        }
    }
}
