using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Services.Objects;
using Services.Service;

namespace LocalizacaoApi.Filters
{
    /// <summary>
    /// Base authorization for every API controller.
    /// 
    /// <remarks>Attention: </remarks>
    /// Every HEADER needs to have one Authorization Attribute:
    /// SwaggerCalls: Should user as user: Demo and password Demo
    /// The HEADER received will be like: <remarks>Authorization: Basic Z2xvcGVzOjEyMzQ=</remarks>
    /// 
    /// NormalCalls: Should use User:Password for example: glopes:1234 -> Z2xvcGVzOjEyMzQ=
    /// The HEADER will be like this: <remarks>Authorization: Tce.Checkin Z2xvcGVzOjEyMzQ=</remarks>
    ///
    /// PS: To decrypt/encrypt go to https://www.base64decode.org/, use "ISO-8859-1"
    /// </summary>
    public class BasicAuthorizationAttribute : AuthorizationFilterAttribute
    {
        private static Encoding encoding = Encoding.GetEncoding("iso-8859-1");

        /// <summary>
        /// Personal authorization.
        /// </summary>
        /// <param name="actionContext">Action demanded from the user.</param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
                {
                    return;
                }

                var authHeader = actionContext.Request.Headers.Authorization;

                if (!string.IsNullOrWhiteSpace(authHeader?.Parameter))
                {
                    var data = ConvertParameters(authHeader.Parameter);

                    switch (authHeader.Scheme)
                    {
                        case "Basic":
                            if (data.Item1.Equals("Demo", StringComparison.InvariantCultureIgnoreCase) && data.Item2.Equals("Demo", StringComparison.InvariantCultureIgnoreCase))
                            {
                                HandleAutorization("1@Demo", new[] { "Demo" });
                                return;
                            }

                            break;
                        case "Tce.Checkin":
                            var service = new ServiceOfUser();
                            var user = service.Verify(new User {UserName = data.Item1, Password = data.Item2});

                            if (user == null)
                            {
                                HandleUnauthorization(actionContext);
                                return;
                            }
                            
                            var keyName = user.Id + "@" + user.UserName;
                            HandleAutorization(keyName, new[] {"User"});
                            break;
                        default:
                            throw new Exception("Unexpected Key for authorization, stopping communications.");
                    }
                }

                HandleUnauthorization(actionContext);
            }
            catch (Exception e)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, "Unexpected Error: " + e.GetBaseException().Message);
            }
            base.OnAuthorization(actionContext);
        }

        private Tuple<string, string> ConvertParameters(string rawValues)
        {
            var credentials = encoding.GetString(Convert.FromBase64String(rawValues));
            string user;
            string pwd;
            if (credentials.Count(x => x == ':') > 1)
            {
                var pos1 = credentials.IndexOf(":", StringComparison.InvariantCultureIgnoreCase);
                user = credentials.Substring(0, pos1);
                pwd = credentials.Substring(pos1, credentials.Length - pos1);
            }
            else
            {
                var split = credentials.Split(':');
                user = split[0];
                pwd = split[1];
            }

            return new Tuple<string, string>(user, pwd);
        }

        private void HandleAutorization(string name, string[] roles)
        {
            var principal = new GenericPrincipal(new GenericIdentity(name), roles);
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }
        
        private void HandleUnauthorization(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
        }
    }
}