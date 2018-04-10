using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DropBoxWebhookAPI.Controllers
{
    public class ExactOnlineAuthController : Controller
    {
        // GET: ExactOnlineAuth
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {

            //*** Initlaize
            Session["CodeReturned"] = "false";

            //*** Check If it's called from Site Page
            if (Request.QueryString["Code"] == null)
            {
                Uri AuthorizationEndpoint = new Uri(string.Format("{0}/api/oauth2/auth?client_id={1}&redirect_uri={2}&response_type=code&force_login=1", System.Configuration.ConfigurationManager.AppSettings["exactOnlineEndPoint"], System.Configuration.ConfigurationManager.AppSettings["exactOnlineClientId"], HttpUtility.UrlEncode(Session["exactOnlineReturnBackURL"].ToString())));

                //*** Open Authentication window
                Response.Redirect(AuthorizationEndpoint.AbsoluteUri);
            }
            else   //**** Get Code from Auth Provider
            {
                Session["ExactOnlineReturnCode"] = Request.Url.AbsoluteUri;

                //**** Close & Refresh parent
                Session["CodeReturned"] = "true";
            }

            return View();
        }
    }
}