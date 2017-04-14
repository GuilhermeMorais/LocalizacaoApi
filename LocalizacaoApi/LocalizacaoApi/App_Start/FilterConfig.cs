using System.Web;
using System.Web.Mvc;
#pragma warning disable 1591

namespace LocalizacaoApi
{
    /// <summary>
    /// Filters Configurations
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Global filters.
        /// </summary>
        /// <param name="filters">Filters</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ErrorHandler.AiHandleErrorAttribute());
        }
    }
}
