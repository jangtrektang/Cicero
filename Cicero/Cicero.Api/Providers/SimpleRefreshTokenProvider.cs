using Cicero.Api.Helpers;
using Cicero.Core.DataObjects;
using Cicero.Core.Helpers.Membership;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Cicero.Api.Providers
{
    public class SimpleRefreshTokenProvider : IAuthenticationTokenProvider
    {
        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            MembershipHelper mh = new MembershipHelper();
            PasswordHelper ph = new PasswordHelper();
            var clientID = context.Ticket.Properties.Dictionary["as:client_id"];

            if (string.IsNullOrEmpty(clientID))
                return;

            var tokenID = Guid.NewGuid().ToString("n");
            var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

            var token = new RefreshToken()
            {
                ID = ph.HashPassword(tokenID),
                ClientID = clientID,
                Subject = context.Ticket.Identity.Name,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
            };

            context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
            context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

            token.ProtectedTicket = context.SerializeTicket();

            var result = await mh.AddRefreshToken(token);

            if (result)
                context.SetToken(tokenID);
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            MembershipHelper mh = new MembershipHelper();
            PasswordHelper ph = new PasswordHelper();
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            string hashedTokenID = ph.HashPassword(context.Token);

            var token = await mh.FindRefreshToken(hashedTokenID);

            if(token != null)
            {
                context.DeserializeTicket(token.ProtectedTicket);
                var result = await mh.RemoveRefreshToken(hashedTokenID);
            }

        }
    }
}