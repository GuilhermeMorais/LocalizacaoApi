using System.Threading;
using System.Web.Http;

namespace LocalizacaoApi.Controllers
{
    /// <summary>
    /// Base for API controllers.
    /// </summary>
    public class BaseApiController : ApiController
    {
        private string userName;
        private int userId;

        /// <summary>
        /// Get the Name of the current user.
        /// </summary>
        public string UserName
        {
            get
            {
                if (string.IsNullOrEmpty(userName))
                {
                    FillDetails();
                }

                return userName;
            }
        }

        /// <summary>
        /// Get the Id the current user.
        /// </summary>
        public int UserId
        {
            get
            {
                if (userId == default(int) || userId == -1)
                {
                    FillDetails();
                }

                return userId;
            }
        }

        private void FillDetails()
        {
            var name = Thread.CurrentPrincipal.Identity;

            if (!name.Name.Contains("@"))
            {
                userId = -1;
                userName = string.Empty;
                return;
            }

            var idAtHome = name.Name.Split('@');
            userId = int.Parse(idAtHome[0]);
            userName = idAtHome[1];
        }
    }
}