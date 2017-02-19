using Cicero.Api.Helpers;
using Cicero.Core.DataObjects;
using Cicero.Core.Helpers.Membership;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Cicero.Api.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientID = string.Empty;
            string clientSecret = string.Empty;
            Client client = null;
            MembershipHelper mh = new MembershipHelper();
            PasswordHelper ph = new PasswordHelper();


            if (!context.TryGetBasicCredentials(out clientID, out clientSecret))
            {
                context.TryGetFormCredentials(out clientID, out clientSecret);
            }

            if(context.ClientId == null)
            {
                context.Validated();
                // context.SetError("invalid_clientID", "ClientID should be sent.");
                return Task.FromResult<object>(null);
            }

            client = mh.FindClient(context.ClientId);

            if(client == null)
            {
                context.SetError("invalid_clientID", string.Format("Client '{0}' is not registered in the system", context.ClientId));
                return Task.FromResult<object>(null);
            }

            if(client.ApplicationType == ApplicationTypes.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientID", "Client secret should be sent.");
                    return Task.FromResult<object>(null);
                }
                else
                {
                    if(client.Secret != ph.HashPassword(clientSecret))
                    {
                        context.SetError("invalid_clientID", "Client secret is invalid.");
                        return Task.FromResult<object>(null);
                    }
                }
            }

            if(!client.Active)
            {
                context.SetError("invalid_clientID", "Client is inactive");
                return Task.FromResult<object>(null);
            }

            context.OwinContext.Set<string>("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            MembershipHelper mh = new MembershipHelper();
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

            if(allowedOrigin == null)
            {
                allowedOrigin = "*";
            }

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var user = await mh.FindUser(context.UserName, context.Password);

            if(user == null)
            {
                context.SetError("invalid_grant", "The username or password is incorrect.");
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));

            var props = new AuthenticationProperties(new Dictionary<string, string>
            {
                {
                    "as:client_id", (context.ClientId == null)
                        ? string.Empty
                        : context.ClientId
                },
                {
                    "userName", context.UserName
                }
            });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach(KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if(!string.Equals(originalClient, currentClient, StringComparison.InvariantCultureIgnoreCase))
            {
                context.SetError("invalid_clientID", "Refresh token is issued to a different clientID");
                return Task.FromResult<object>(null);
            }

            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }
    }
}