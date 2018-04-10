using ExactAssignment.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DropBoxWebhookAPI.Controllers
{
    public class DropBoxAuthController : Controller
    {

        // GET: DropBoxAuth
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            //*** Initlaize
            Session["CodeReturned"] = "false";

            //*** Check If it's called from Site Page
            if (Request.QueryString["Code"] == null)
            {
                //*** get access token                        
                Response.Redirect((DropBoxConnector.getAccessTokenURL(System.Configuration.ConfigurationManager.AppSettings["dropBoxAppKey"], System.Configuration.ConfigurationManager.AppSettings["dropBoxAppSecret"], Session["dropBoxReturnBackURL"].ToString())).ToLower(), false);
            }
            else   //**** Get Code from Auth Provider
            {
                //*** Check If Code returned into Connection String
                if (Session["dropBoxAccessToken"] == null && !String.IsNullOrEmpty(Request.QueryString["Code"]))
                {
                    //**** Initialize Session Folder Path
                    List<string> Dump = new List<string> { };
                    Session["FolderPath"] = Dump;

                    await DropBoxConnector.getAccessTokenFromResponse(Request.QueryString["Code"], System.Configuration.ConfigurationManager.AppSettings["dropBoxAppKey"], System.Configuration.ConfigurationManager.AppSettings["dropBoxAppSecret"], Session["dropBoxReturnBackURL"].ToString().ToLower());

                    if (DropBoxConnector.MsgError == "")
                    {
                        //*** Get Token
                        Session["dropBoxAccessToken"] = DropBoxConnector.dropBoxAccessToken;

                        //**** Close & Refresh parent
                        Session["CodeReturned"] = "true";
                    }
                    else   //*** If Error returned
                    {
                        Response.Write(DropBoxConnector.MsgError);
                    }
                }
            }
            return View();
        }
    }
}