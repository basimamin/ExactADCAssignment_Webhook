using DropBox_BLL.DBEntity;
using ExactAssignment.BLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DropBoxWebhookAPI.Controllers
{
    public class HomeController : Controller
    {
        protected string DownloadFilePath;
        protected string strExactOnlineAuthParam;
        protected string strSiteBaseURL;

        public async System.Threading.Tasks.Task<ActionResult> Index()

        {
            //*** Check Lock Variable First
            if(!(Boolean)HttpContext.Application["SyncInProgress"])
            {
                HttpContext.Application["SyncInProgress"] = true; //*** Set Lock Flag

                ViewBag.Title = "Sync Dropbox Files";

                //*** Initialization
                Session["lblDropBoxMsg"] = "";
                Session["btnDropBoxbtnV"] = "hidden";
                Session["btnExactOnlinebtnV"] = "hidden";
                Session["SyncAllNumbers"] = "";
                Session["ResultNumbers"] = "";

                strSiteBaseURL = Request.Url.Scheme + "://" + Request.Url.Host;
                if (Request.Url.Port > 0) { strSiteBaseURL += ":" + Request.Url.Port.ToString(); };

                //*** Adjust Drop Box Call Back URL for 
                if (Session["dropBoxReturnBackURL"] == null)
                {
                    Session["dropBoxReturnBackURL"] = strSiteBaseURL;
                    Session["dropBoxReturnBackURL"] += "/" + System.Configuration.ConfigurationManager.AppSettings["dropBoxAuthReturnPage"];
                }
                //******************************************************

                //*********************************************
                //*** Query on Files last Modified 
                //*********************************************
                DateTime dtLastModifiedDate = new DateTime(2000, 1, 1);
                CloudStorageEntities objCloudStorageEntities = new CloudStorageEntities();

                //*** Check First if File Already exisit into DB
                DropboxWebhook objRecord = objCloudStorageEntities.DropboxWebhooks.Where(i => i.DW_Processed == 0).FirstOrDefault();
                if (objRecord != null)
                {
                    dtLastModifiedDate = Convert.ToDateTime(objRecord.DW_TimeStamp);
                }
            
                if(dtLastModifiedDate > Convert.ToDateTime("1/1/2000"))
                {
                    //******************************************************************************************************
                    //*** Dropbox Part
                    //******************************************************************************************************  
                    //*** Check dropBox Aurhentication Token
                    if (Session["dropBoxAccessToken"] == null)
                    {
                        //***************************
                        //*** access token is empty
                        //***************************
                        //*** 1. Check first for dropBox App Key & App secret
                        if (String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["dropBoxAppKey"]) ||
                            String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["dropBoxAppSecret"]))
                        {
                            //*** Show Error Message
                            Session["lblDropBoxMsg"] = "Please set app key and secret in this project's Web.config file and restart. " +
                                                   "App key/secret can be found in the Dropbox App Console, here: " +
                                                   "https://www.dropbox.com/developers/apps";
                            goto lnIndexEnd;
                        }
                        else   //*** AppKey & secret exist
                        {
                            //*** Show Connect to DropBox button
                            Session["btnDropBoxbtnV"] = "visible";
                        }
                    }
                    else
                    {
                        //*** Get DropBox Client Object
                        Session["dropBoxClientObj"] = DropBoxConnector.getDropboxClient(Session["dropBoxAccessToken"].ToString());

                        if (DropBoxConnector.MsgError != "")    //*** If error
                        {
                            Session["lblDropBoxMsg"] = "Dropbox Error: " + DropBoxConnector.MsgError;
                        }
                        else
                        {
                            //******************************************************************************************************
                            //*** Exact Online Part
                            //******************************************************************************************************   
                            //*** Adjust Drop Box Call Back URL for 
                            if (Session["exactOnlineReturnBackURL"] == null)
                            {
                                Session["exactOnlineReturnBackURL"] = Request.Url.Scheme + "://" + Request.Url.Host;
                                if (Request.Url.Port > 0) {Session["exactOnlineReturnBackURL"] += ":" + Request.Url.Port.ToString(); };
                                Session["exactOnlineReturnBackURL"] += "/" + System.Configuration.ConfigurationManager.AppSettings["exactOnlineReturnPage"];
                            }

                            //*** Check If Code returned into Connection String
                            if (Session["ExactOnlineAccessToken"] == null && Session["ExactOnlineReturnCode"] != null)
                            {
                                //**** Initialize Session Folder Path
                                List<string> Dump = new List<string> { };
                                Session["ExactOnlineFolderPath"] = Dump;

                                //**** Construct Exact Online Class
                                ExactOnlineConnector objExactOnlineConnector = new ExactOnlineConnector(System.Configuration.ConfigurationManager.AppSettings["exactOnlineClientId"], System.Configuration.ConfigurationManager.AppSettings["exactOnlineClientSecret"], System.Configuration.ConfigurationManager.AppSettings["exactOnlineEndPoint"], new Uri(Session["exactOnlineReturnBackURL"].ToString()), Session["ExactOnlineReturnCode"].ToString());

                                Session["ExactOnlineAccessToken"] = objExactOnlineConnector.GetAccessToken();

                                if (objExactOnlineConnector.MsgError != "")
                                {   //*** If Error returned
                                    Session["lblDropBoxMsg"] = "Exact Online Error: " + objExactOnlineConnector.MsgError;

                                    goto lnIndexEnd;
                                }                                                         
                            }

                            //*** Check ExactOnline Aurhentication Token
                            if (Session["ExactOnlineAccessToken"] == null && Session["ExactOnlineReturnCode"] == null)
                            {

                                //***************************
                                //*** access token is empty
                                //***************************
                                //*** 1. Check first for ExactOnline App Key & App secret
                                if (String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["exactOnlineClientId"]) ||
                                    String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["exactOnlineClientSecret"]) ||
                                    String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["exactOnlineEndPoint"]))
                                {
                                    //*** Show Error Message
                                    Session["lblDropBoxMsg"] = "Please set client id, client secret and end point URL into this project's Web.config file and restart. " +
                                                           "client id/secret can be found in the ExactOnline App Console, here: " +
                                                           "https://start.exactonline.co.uk";

                                    goto lnIndexEnd;

                                }
                                else   //*** AppKey & secret exist
                                {
                                    //*** Show Connect to ExactOnline button
                                    Session["btnExactOnlinebtnV"] = "visible";
                                }
                            }

                            if (Session["ExactOnlineReturnCode"] != null)
                            {
                                //*** 1. List all Dropbox Files 
                               await getDropBoxFilesList((List<string>)Session["FolderPath"]);

                                //*** 2. Call Sync Function which Sync all Modified files with Exact Online Store
                                await SyncAllFilesFolders((List<DropBoxFile>)Session["lstDropBoxFile"]);

                                if (objRecord != null)
                                {
                                    //*** Update Flag of WebHook Table
                                    objRecord.DW_Processed = 1;
                                    objRecord.DW_ProcessTimeStamp = DateTime.Now;
                                    objCloudStorageEntities.SaveChanges();
                                }
                            }
                        }
                    }
                }

lnIndexEnd:
                //*** GC
                objCloudStorageEntities.Dispose();
            }

            HttpContext.Application["SyncInProgress"] = false; //*** Release Lock Flag

            //*** Display Results
            if (Session["SyncAllNumbers"].ToString() != "")
            {
                string[] strAllResultNumbers = Session["SyncAllNumbers"].ToString().Split(',');

                Session["ResultNumbers"] = "Dropbox Files= " + strAllResultNumbers[0] + " ,Files Processed= " + strAllResultNumbers[1] + " ,Files Replaced Successfully= " + strAllResultNumbers[2] + " ,Failure= " + strAllResultNumbers[3];
            }
          
            try
            {                
                return View();
            }
            finally
            {                
            }
        }

        //*****************************************************************
        //*** Drop Box Files List Function
        //*****************************************************************
        /// <summary>
        /// *** Get DropBox Files List
        /// </summary>
        /// <param name="FolderPath">Folder Path in Array Format</param>
        /// <returns></returns>
        private async Task getDropBoxFilesList(List<string> FolderPath = null)
        {
            try
            { 
                //*** Constract Path String
                string strFolderpath = "";
                string strFolderName = "";
                if (FolderPath != null)
                {
                    foreach (var item in FolderPath)
                    {
                        strFolderpath += "/" + item;
                        strFolderName = item;
                    }
                }

                //*** Bind the Grid & Bread Crumb
                //*** Get Files & Folders List
                //var returnTask = DropBoxConnector.ListFolder(Session["dropBoxClientObj"], strFolderpath);
                //returnTask.Wait();
                //List<DropBoxFile> lstDropBoxFile = returnTask.Result;
                List<DropBoxFile> lstDropBoxFile = await DropBoxConnector.ListFolder(Session["dropBoxClientObj"], strFolderpath);
                
                if (DropBoxConnector.MsgError != "")    //*** If error
                {
                    Session["lblDropBoxMsg"] = DropBoxConnector.MsgError;
                }
                else
                {
                    //*** Set Session with All Dropbox List
                    Session["lstDropBoxFile"] = lstDropBoxFile;
                }
            }
            catch (Exception e)
            {
                Session["lblDropBoxMsg"] = "getDropBoxFilesList Error: " + e.ToString();
            }
        }


        //*****************************************************************
        //*** Major Sync Function with Exact online store
        //*****************************************************************
        /// <summary>
        /// *** Sync Function which Sync all Modified files with Exact Online Store
        /// </summary>
        /// <param name="lstDropBoxFile">Dropbox files in List Format</param>
        /// <returns></returns>
        private async Task SyncAllFilesFolders(List<DropBoxFile> lstDropBoxFile)
        {
            //System.Threading.Thread.Sleep(30000);

            try
            {
                //*** Set Variables
                int intCount = 0, intSuccess = 0, intFailed = 0;
                Session["SyncAllNumbers"] = "";
                string strExactFileGUID = "";
                string strFolderpath = "";
                string strParentFolderpath = "";
                CloudStorageEntities objCloudStorageEntities = new CloudStorageEntities();

                //**** Construct Exact Online Class
                ExactOnlineConnector objExactOnlineConnector = new ExactOnlineConnector(System.Configuration.ConfigurationManager.AppSettings["exactOnlineClientId"], System.Configuration.ConfigurationManager.AppSettings["exactOnlineClientSecret"], System.Configuration.ConfigurationManager.AppSettings["exactOnlineEndPoint"], new Uri(Session["exactOnlineReturnBackURL"].ToString()), Session["ExactOnlineReturnCode"].ToString());

                //*** First Reset All "FileStillAlive" Flag into DB
                (from p in objCloudStorageEntities.DropBoxExactOnlines
                 where p.Id >= 0
                 select p).ToList().ForEach(x => x.FileStillAlive = 0);
                objCloudStorageEntities.SaveChanges();

                //*** Loop on All Objects on DropBox Grid View
                foreach (var DropBoxFile in lstDropBoxFile)
                {
                    strFolderpath = "";
 
                    //*** Refresh Counts
                    intCount += 1;
                                        
                    //**** Check If File item exist into DB with same Modified Date or not
                    //*** 1. Exist with Same Modified Date, Do Nothing
                    //*** 2. Not Exist, So Add File to Exact Online and DB
                    //*** 3. Exist with Different Modified Date, So Update File into Exact Online and Modified Date into DB
               
                    DropBoxExactOnline objRecord = objCloudStorageEntities.DropBoxExactOnlines.Where(i => i.DropBoxPath == DropBoxFile.FileName).FirstOrDefault();
                    if (objRecord == null || (objRecord != null && DropBoxFile.ModificationDate != objRecord.DropBoxFileModifiedDate))   //*** Not Exist Or File Exist with Different Modification Date
                    {
                        //********************************************************************
                        //*** Add File to Exact Online
                        //********************************************************************

                        //**** Get File Stream then upload it to ExactOnline & Flush
                        //*** Construct Parent Folder Path String            
                        strParentFolderpath = "";
                        if ((List<string>)Session["FolderPath"] != null)
                        {
                            foreach (var item in (List<string>)Session["FolderPath"])
                            {
                                strParentFolderpath += "/" + item;
                            }
                        }

                        string strPath = strParentFolderpath + "/" + DropBoxFile.FileName;
                        strFolderpath = strPath;

                        //*** Create Folder Function
                        Stream fnStreamResult = await DropBoxConnector.Download(Session["dropBoxClientObj"], strPath);

                        if (DropBoxConnector.MsgError != "")    //*** If error
                        {
                            intFailed += 1;
                        }
                        else
                        {
                            //*************************************************************
                            //*** Convert File to Byte Array and upload it to Exact Online
                            //*************************************************************                           
                            if (Session["ExactOnlineAccessToken"] != null)
                            {
                                objExactOnlineConnector.AccessToken = Session["ExactOnlineAccessToken"].ToString();
                            }

                            //**** Get Document Folder GUID
                            Session["CurrentExactFolderGUID"] = string.Empty;       //*** Root Folder

                            //*** If File already exisit then Delete it first   
                            if(objRecord != null && DropBoxFile.ModificationDate != objRecord.DropBoxFileModifiedDate)
                            {
                                objExactOnlineConnector.DeleteDocument(objRecord.ExactOnlineGUID);  //**** Call Delete Document
                            }

                            strExactFileGUID = objExactOnlineConnector.CreateDocumentWithAttachment(DropBoxFile.FileName, Session["CurrentExactFolderGUID"].ToString(), Common.ConvertStreamtoByteArr(fnStreamResult));
                            if (strExactFileGUID == "")
                            {
                                intFailed += 1;
                            }
                            else
                            {
                                intSuccess += 1;
                            }                           
                        }
                        //******************************************************************                    

                        if (objRecord == null)
                        {
                            //*** add to DB
                            DropBoxExactOnline objRecordNew = new DropBoxExactOnline();
                            objRecordNew.DropBoxPath = DropBoxFile.FileName;
                            objRecordNew.DropBoxFileModifiedDate = DropBoxFile.ModificationDate;
                            objRecordNew.ExactOnlineGUID = strExactFileGUID;
                            objRecordNew.isFile = 1;
                            objRecordNew.FileStillAlive = 1;

                            objCloudStorageEntities.DropBoxExactOnlines.Add(objRecordNew);
                            objCloudStorageEntities.SaveChanges();
                        }
                        else
                        {
                            //**** Update DB      
                            objRecord.DropBoxFileModifiedDate = DropBoxFile.ModificationDate;
                            objRecord.ExactOnlineGUID = strExactFileGUID;
                            objRecord.FileStillAlive = 1;

                            objCloudStorageEntities.SaveChanges();
                        }
                    }

                    //*** If File still exit and not changed
                    if (objRecord != null && DropBoxFile.ModificationDate == objRecord.DropBoxFileModifiedDate)
                    {
                        objRecord.FileStillAlive = 1;

                        objCloudStorageEntities.SaveChanges();
                    }

                    //*** set Session Variable (Shared Variable)
                    Session["SyncAllNumbers"] = lstDropBoxFile.Count.ToString() + "," + intCount.ToString() + "," + intSuccess.ToString() + "," + intFailed.ToString();
                }   //*** For Loop

                //***************************************************************************************
                //*** Then Check For Not Alive Files to Delete from DB and Exact Online
                //***************************************************************************************
                List<DropBoxExactOnline> lstFiles = objCloudStorageEntities.DropBoxExactOnlines.Where(item => item.FileStillAlive == 0).ToList();

                foreach (var file in lstFiles)
                {
                    //****************************************************************
                    //**** Delete File from Exact Online
                    //****************************************************************
                    if (file.ExactOnlineGUID != "")
                    {
                        //*** Delete File on Exact Online also
                        if (Session["ExactOnlineAccessToken"] != null)
                        {
                            objExactOnlineConnector.AccessToken = Session["ExactOnlineAccessToken"].ToString();
                        }

                        //**** Call Delete Document
                        objExactOnlineConnector.DeleteDocument(file.ExactOnlineGUID);
                    }
                    //***************************************************************************   

                    //*** Delete From DB
                    objCloudStorageEntities.DropBoxExactOnlines.Remove(file);
                }   //*** For Loop

                //*** Submit Delete from DB 
                objCloudStorageEntities.SaveChanges();
                //***************************************************************************************
            }
            catch (Exception e)
            {
                Session["lblDropBoxMsg"] = "SyncAllFilesFolders Error: " + e.ToString();
            }
        }

        //*****************************************************************

    }
}
