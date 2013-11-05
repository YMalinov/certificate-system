using System.Web;
using System.Web.Mvc;

namespace WebApplicationCertificateUserSystem
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
