using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.WebHooks;
using DropBox_BLL.DBEntity;

namespace DropBoxWebhookAPI.Controllers
{
    public class DropboxWebhookController : Controller
    {
        //    // GET: Dropbox
        //    public ActionResult Index()
        //    {
        //        return View();
        //    }

        //**** This is default Handler which takes a challenge parameter and returns that parameter back:
        [HttpGet]
        public ActionResult Index(string challenge)
        {
            try
            {
                return Content(challenge);
            }
            catch (Exception exBody)
            {
                //*** Handle Error

                //*** return back with Status code 400 (Bad request)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        //*** This is Handler for weebhook File changes /delta files
        [HttpPost]
        public async Task<ActionResult> Index()
        {
            try
            {
                //*** Get the request signature & Verify
                var signatureHeader = Request.Headers.GetValues("X-Dropbox-Signature");
                if (signatureHeader == null || !signatureHeader.Any())              //**** If there is no Signature header
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);     //*** Reply back with Status code 400 (Bad request)

                //*** Get the signature value
                string signature = signatureHeader.FirstOrDefault();

                //*** Extract the raw body of the request
                string body = null;
                using (StreamReader reader = new StreamReader(Request.InputStream))
                {
                    body = await reader.ReadToEndAsync();       //*** Extract Request Body
                }
            
                //*** Check that the signature is good
                string appSecret = ConfigurationManager.AppSettings["dropBoxAppSecret"];
                using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(appSecret)))         
                {
                    if (!Crypt.VerifySha256Hash(hmac, body, signature))                       //**** Verify Hash of Header vs. secret key
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);           //*** Reply back with Status code 400 (Bad request)
                }

                //*** Store Body to queue to Process later, Add Entity to DB
                CloudStorageEntities objCloudStorageEntities = new CloudStorageEntities();

                //*** add to DB
                DropboxWebhook objRecordNew = new DropboxWebhook();
                objRecordNew.DW_Body = body;
                objRecordNew.DW_TimeStamp = DateTime.Now;
                objRecordNew.DW_Processed = 0;

                objCloudStorageEntities.DropboxWebhooks.Add(objRecordNew);
                objCloudStorageEntities.SaveChanges();

                //**** Dispose Opbjects
                objCloudStorageEntities.Dispose();
                //*******************************************************

                // Return A-OK :)
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception exBody)
            {
                //*** Handle Error

                //*** return back with Status code 400 (Bad request)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);          
            }
        }
    }
}