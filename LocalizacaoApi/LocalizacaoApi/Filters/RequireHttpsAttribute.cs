using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace LocalizacaoApi.Filters
{
    /// <summary>
    /// Will force Https if "UsaHttps" is true on web.config.
    /// </summary>
    public class RequireHttpsAttribute : AuthorizationFilterAttribute
    {
        /// <summary>
        /// Will call be every time.
        /// </summary>
        /// <param name="actionContext">Context from user call.</param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var req = actionContext.Request;

            if (req.RequestUri.Scheme == Uri.UriSchemeHttps)
            {
                return;
            }

            const string html = "<p> Https é obrigatório </p>";

            if (req.Method.Method == "GET")
            {
                actionContext.Response = req.CreateResponse(HttpStatusCode.Found);
                actionContext.Response.Content = new StringContent(html, Encoding.UTF8, "text/html");
                var newUri = new UriBuilder(req.RequestUri)
                {
                    Scheme = Uri.UriSchemeHttps,
                    Port = 443
                };

                actionContext.Response.Headers.Location = newUri.Uri;
            }
            else
            {
                actionContext.Response = req.CreateResponse(HttpStatusCode.NotFound);
                actionContext.Response.Content = new StringContent(html, Encoding.UTF8, "text/html");
            }
        }
    }
}