using RoleBasedWebApiDemo.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace RoleBasedWebApiDemo.Infrastructure.Core
{


    public class TokenValidationHandler : DelegatingHandler
    {
        const string AUTHORIZATION_TOKEN_HEADER = "Authorization-Token";

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!request.Headers.Contains("Authorization-Token"))
                {
                    return Task<HttpResponseMessage>.Factory.StartNew(() =>
                    {
                        return new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new StringContent("You need to include Authorization-Token")
                        };
                    });
                }
                var token = request.Headers.GetValues("Authorization-Token").FirstOrDefault();
                if (string.IsNullOrEmpty(token))
                {
                    return Task<HttpResponseMessage>.Factory.StartNew(() =>
                    {
                        return new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new StringContent("Missing Authorization-Token")
                        };
                    });
                }
                var decryptedToken = RSAClass.Decrypt(token); // TODO: do your query to find the user
                var user = decryptedToken;
                var identity = new GenericIdentity(decryptedToken);
                string[] roles = new[] { "Users", "Testers" };
                var principal = new GenericPrincipal(identity, roles);
                Thread.CurrentPrincipal = principal;
            }
            catch
            {
                return Task<HttpResponseMessage>.Factory.StartNew(() =>
                {
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("Error encountered in authorization token")
                    };
                });

            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}