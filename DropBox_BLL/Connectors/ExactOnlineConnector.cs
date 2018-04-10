using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ExactOnline.Client.Models;
using ExactOnline.Client.Sdk.Controllers;
using ExactOnline.Client.Sdk.Helpers;
using ExactOnline.Client.OAuth;

namespace ExactAssignment.BLL
{
    //*** This Class has all Functions that connect to Exact Online APIs and use it
    public class ExactOnlineConnector
    {
        //************************************************
        //*** APIs Calling URIs
        //************************************************
        private  const string constUserURI = "/api/v1/current/Me";
        private  const string constDocumentsURI = "/api/v1/{0}/documents/Documents";
        private  const string constDocumentFoldersURI = "/api/v1/{0}/documents/DocumentFolders";
        private  const string constAttachmentDocumentType = ""; 
        //************************************************
        //*** Static Public Variables
        public  string MsgError = "";
        public string AccessToken = "";

        //*** Private Variables
        private  readonly string _clientId;
        private  readonly string _clientSecret;
        private  readonly string _endpoint;
        private  readonly Uri _callbackUrl;
        private  readonly UserAuthorization _authorization;
        private readonly string _AuthorizationUri;

        /// <summary>
        /// *** This is Class Constructor
        /// </summary>
        /// <param name="clientId">Exact Online Client Id</param>
        /// <param name="clientSecret">Exact Online Client Secret.</param>
        /// <param name="EndPoint">End Point</param>
        /// <param name="callbackUrl">CallBack URL</param>
        /// <returns></returns>
        public ExactOnlineConnector(string clientId, string clientSecret, string EndPoint, Uri callbackUrl, string AuthorizationUri)
		{            
			_clientId = clientId;
			_clientSecret = clientSecret;
            _endpoint = EndPoint;
			_callbackUrl = callbackUrl;
            _AuthorizationUri = AuthorizationUri;
			_authorization = new UserAuthorization();
		}

        /// <summary>
        /// Get Access Token which is used to into All API functions
        /// In case of error return error string
        /// </summary>
        /// <returns>Access Token String</returns>
		public string GetAccessToken()
		{
            try
            {
                if (AccessToken != "")
                {
                    return AccessToken;
                }
                else   ///*** Issue New one
                { 
                    UserAuthorizations.Authorize(_authorization, _endpoint, _clientId, _clientSecret, _callbackUrl, _AuthorizationUri);

                    AccessToken = _authorization.AccessToken;
                    return _authorization.AccessToken;
                }
            }
            catch (Exception e) //*** Error
            {
                MsgError = e.ToString();

                return "";
                //return null;
            }           
		}
        
        /// <summary>
        /// *** This Function is used to get ExactOnline User Info Class
        /// *** Incase of error it set static variable MsgError with error details
        /// </summary>        
        /// <returns>Exact Online User object Instance</returns>
        public ExactOnlineUser getUserInfo()
        {
            //*** Initialization
            MsgError = "";

            try
            {
                //*** Get Exact Online Client Object
                var client = new ExactOnlineClient(_endpoint, GetAccessToken);
                
                //*** Get the Fields of First Account Needed in the administration
                var fields = new[] { "UserID", "CurrentDivision", "Email", "FirstName", "FullName", "Gender", "LastName", "Title", "UserName" };                
                var account = client.For<Me>().Top(1).Select(fields).Get().FirstOrDefault();

                //*** Set Class with Values
                //ExactOnlineUser objExactOnlineUser = new ExactOnlineUser();
                ExactOnlineUser objExactOnlineUser = new ExactOnlineUser();

                objExactOnlineUser.AccountId = account.UserID.ToString();
                objExactOnlineUser.AccountDivisionId = account.CurrentDivision.ToString();
                objExactOnlineUser.AccountEmail = account.Email;                
                objExactOnlineUser.FirstName = account.FirstName;
                objExactOnlineUser.FullName = account.FullName;
                objExactOnlineUser.Gender = account.Gender;
                objExactOnlineUser.LastName = account.LastName;
                objExactOnlineUser.Title = account.Title;
                objExactOnlineUser.UserName = account.UserName;

                return objExactOnlineUser;
            }
            catch (Exception e) //*** Error
            {
                MsgError = e.ToString();

                return null;
            }
        }

        /// <summary>
        ///*** Lists the documents within a folder.
        /// *** Incase of error it set static variable MsgError with error details
        /// </summary>                
        /// <param name="strParentFolderGUID">Parent Folder GUID.</param>        
        /// <returns>List Of ExactOnline Class instance</returns>
        public  List<ExactOnlineDocument> ListDocuments(string strParentFolderGUID)
        {
            try
            {
                //*** Get Exact Online Client Object
                var client = new ExactOnlineClient(_endpoint, GetAccessToken);
           
                //*** Get result into Array List
                var documents = client.For<Document>().Where("TypeDescription eq 'Attachment'" + (strParentFolderGUID != "" ? " and DocumentFolder eq guid'" + strParentFolderGUID + "'" : "")).Select(new string[] { "ID", "Created", "Subject", "DocumentFolder", "DocumentFolderCode", "Modified", "Type", "TypeDescription" }).Get();

                //*** Get Documents Attachments
                var documentsAttachments = client.For<DocumentAttachment>().Select(new string[] { "ID", "Attachment", "Document", "FileName", "FileSize", "Url" }).Get();

                //*** Define Document List
                List<ExactOnlineDocument> lstDocuments = new List<ExactOnlineDocument>();
                
                //***************************
                //*** get Documents
                //***************************                            
                foreach (var document in documents)
                {
                    //**** Fitch for document Attachment
                    var documentAttachment = documentsAttachments.Find(item => item.Document == document.ID);
                    if (documentAttachment != null)
                    {
                        lstDocuments.Add(new ExactOnlineDocument { DocumentId=document.ID.ToString(), FileName = document.Subject, FilePath = document.DocumentFolderCode, ModificationDate = (DateTime)document.Modified, FileSizeInBytes = (ulong)documentAttachment.FileSize, FileURL = documentAttachment.Url });
                    }
                    else
                    {
                        lstDocuments.Add(new ExactOnlineDocument { DocumentId = document.ID.ToString(), FileName = document.Subject, FilePath = document.DocumentFolderCode, ModificationDate = (DateTime)document.Modified, FileSizeInBytes = 0, FileURL = "" });
                    }
                }

                return lstDocuments;
            }
            catch (Exception e) //*** Error
            {
                MsgError = e.ToString();

                return null;
            }
        }

        /// <summary>
        ///*** Lists the Folders
        /// *** Incase of error it set static variable MsgError with error details
        /// </summary>        
        /// <param name="strParentFolderGUID">Parent Folder GUID.</param>        
        /// <returns>List Of ExactOnline Class instance</returns>
        public  List<ExactOnlineFolder> ListFolders(string strParentFolderGUID)
        {
            try
            {
                //*** Get Exact Online Client Object
                var client = new ExactOnlineClient(_endpoint, GetAccessToken);

               //*** Get result into Array List
                var folders = new List<DocumentFolder> { };
                if(strParentFolderGUID == "")
                     folders = client.For<DocumentFolder>().Select(new string[] { "ID", "Code", "Description", "Modified", "ParentFolder" }).Get();
                else
                     folders = client.For<DocumentFolder>().Where("ParentFolder eq guid'" + strParentFolderGUID + "'").Select(new string[] { "ID", "Code", "Description", "Modified", "ParentFolder" }).Get();

                //*** Define Document List
                List<ExactOnlineFolder> lstFolders = new List<ExactOnlineFolder>();

                //***************************
                //*** get Folders
                //***************************                
                foreach (var folder in folders)
                {
                    lstFolders.Add(new ExactOnlineFolder { FolderId = folder.ID.ToString(), FolderName = folder.Code, ModificationDate = (DateTime)folder.Modified, ParentFolder = folder.ParentFolder.ToString() });
                }

                return lstFolders;
            }
            catch (Exception e) //*** Error
            {
                MsgError = e.ToString();

                return null;
            }
        }

        /// <summary>
        ///*** Lists the documents & Folders inside parent folder
        /// *** Incase of error it set static variable MsgError with error details
        /// </summary>        
        /// <param name="strParentFolderGUID">Parent Folder GUID.</param>        
        /// <returns>List Of ExactOnline Class instance</returns>
        public List<ExactOnlineFile> ListDocumentsFolders(string strParentFolderGUID)
        {
            try
            {
                //*** Define Return Class
                List<ExactOnlineFile> lstExactOnlineFile = new List<ExactOnlineFile> { };

                //*** First List Folders
                List<ExactOnlineFolder> lstFolders = ListFolders(strParentFolderGUID);

                foreach (var folder in lstFolders)
                { 
                    lstExactOnlineFile.Add(new ExactOnlineFile{ID = folder.FolderId, FileName=folder.FolderName, FilePath="", FileSizeInBytes=0, isDeleted=false, isFolder=true, ModificationDate = folder.ModificationDate, FileURL=""});
                }
                lstFolders = null;

                //*** Then Get Documents
                List<ExactOnlineDocument> lstDocuments = ListDocuments(strParentFolderGUID);

                foreach (var document in lstDocuments)
                {
                    lstExactOnlineFile.Add(new ExactOnlineFile { ID = document.DocumentId, FileName = document.FileName, FilePath = document.FilePath, FileSizeInBytes = document.FileSizeInBytes, isDeleted = false, isFolder = false, ModificationDate = document.ModificationDate, FileURL = document.FileURL });
                }
                lstDocuments = null;

                return lstExactOnlineFile;
            }
            catch (Exception e) //*** Error
            {
                MsgError = e.ToString();

                return null;
            }
        }

        /// <summary>
        ///*** Create Docuemnt Folder inside parent folder
        /// *** Incase of error it set static variable MsgError with error details
        /// </summary>        
        /// <param name="FolderName">FolderName</param>        
        /// <param name="strParentFolderGUID">Parent Folder GUID.</param>        
        /// <returns>Folder GUID</returns>
        public string CreateDocumentFolder(string FolderName, string strParentFolderGUID)
        {
            try
            {
                //*** Create new entity Instance
                var objDocumentFolder = new DocumentFolder{Code=FolderName, Description=FolderName};
                if(strParentFolderGUID != "")
                {
                    objDocumentFolder.ParentFolder = Guid.Parse(strParentFolderGUID);
                }

                var client = new ExactOnlineClient(_endpoint, GetAccessToken);

                if (client.For<DocumentFolder>().Insert(ref objDocumentFolder))
                    return objDocumentFolder.ID.ToString();
                else
                    return "";
            }
            catch (Exception e) //*** Error
            {
                MsgError = e.ToString();

                return "";
            }
        }

        /// <summary>
        ///*** Create Docuemnt with Attachment File inside parent folder
        /// *** Incase of error it set static variable MsgError with error details
        /// </summary>        
        /// <param name="FileName">FileName</param>        
        /// <param name="strParentFolderGUID">Parent Folder GUID.</param>        
        /// <param name="FileContent">File Content in Byte Array.</param>        
        /// <returns>Document Guid</returns>
        public string CreateDocumentWithAttachment(string FileName, string strParentFolderGUID, byte[] FileContent = null)
        {
            string strDocumentGUID = "";
            try
            {
                //*** 1. Get Attachment Document Type
                int AttachmentDocumentType = getAttachmentDocumentTypeId();

                if (AttachmentDocumentType > 0)
                {
                    //*** First of All Check if the file already exist before, if yes: then update
                    //*** Get Exact Online Client Object
                    var client = new ExactOnlineClient(_endpoint, GetAccessToken);
                                      
                    //*** Check first to see if document already exist or not 
                    //*** Get Documents 
                    string strFilter = "Subject eq '" + FileName + "'";
                    if (strParentFolderGUID != "") strFilter += " and DocumentFolder eq guid'" + strParentFolderGUID + "'";

                    var documents = client.For<Document>().Top(1).Where(strFilter).Select(new string[] { "ID", "Subject", "Type", "Body" }).Get();

                    if (documents.Count <= 0)    //**** Create
                    {
                        //*** Set Common entites
                        //*** Create new entity Instance
                        var objDocument = new Document { Subject = FileName, Type = AttachmentDocumentType, Body = FileName };
                        if (strParentFolderGUID != "")
                        {
                            objDocument.DocumentFolder = Guid.Parse(strParentFolderGUID);
                        }
                        //****************************************************************************

                        //*** No Attachment with same name and folder, The  Create
                        if (client.For<Document>().Insert(ref objDocument))
                        {
                            strDocumentGUID = objDocument.ID.ToString();

                            //*** If there is Attachment file
                            if (FileContent != null)
                            {
                                //**** Add Attachment
                                var attachment = new DocumentAttachment { Document = objDocument.ID, FileName = FileName, Attachment = FileContent };

                                if (client.For<DocumentAttachment>().Insert(ref attachment))
                                {
                                    return strDocumentGUID;
                                }
                                else
                                {
                                    //*** Error while creating attachment
                                    MsgError = "error while creating attachment";

                                    return "";
                                }
                            }
                            else
                            {
                                return strDocumentGUID;
                            }
                        }
                        else   ///*** Problem creating document
                        {
                            MsgError = "error while creating document";

                            return "";
                        }
                    }
                    else   //*** Document Alreay Exist then Update & Add Attachment
                    {
                        //*** No Attachment with same name and folder, The  Create
                        //*** Update document properties
                        documents[0].Type = AttachmentDocumentType;
                        documents[0].Subject = FileName;
                        documents[0].Body = FileName;
                        strDocumentGUID = documents[0].ID.ToString();

                         if (client.For<Document>().Update(documents[0]))
                        {
                             //*** If there is Attachment file
                            if (FileContent != null)
                            {
                                //*** Check if Attachment exist
                                var documentAttachment = client.For<DocumentAttachment>().Top(1).Where("Document eq guid'" + documents[0].ID.ToString() + "' and FileName eq '" + FileName + "'").Select(new string[] { "ID", "FileName", "Attachment" }).Get();

                                 if (documentAttachment.Count <= 0)    //**** Create
                                 {
                                     //**** Add Attachment
                                     var attachment = new DocumentAttachment { Document = documents[0].ID, FileName = FileName, Attachment = FileContent };

                                     if (client.For<DocumentAttachment>().Insert(ref attachment))
                                     {
                                         return strDocumentGUID;
                                     }
                                     else
                                     {
                                         //*** Error while creating attachment
                                         MsgError = "error while adding attachment";

                                         return "";
                                     }
                                 }
                                 else  //**** Update Attachment
                                 {
                                     documentAttachment[0].FileName = FileName;
                                     documentAttachment[0].Attachment = FileContent;

                                     if (client.For<DocumentAttachment>().Update(documentAttachment[0]))
                                     {
                                         return strDocumentGUID;
                                     }
                                     else
                                     {
                                         //*** Error while creating attachment
                                         MsgError = "error while updating attachment";

                                         return "";
                                     }
                                 }
                            }
                            else
                            {
                                return strDocumentGUID;
                            }
                        }
                        else   ///*** Problem creating document
                        {
                            MsgError = "error while updating document";

                            return "";
                        }

                    }
                    
                                        
                }
                else
                {
                    MsgError = "No 'attachment' into Document Type";

                    return "";                 
                }
            }
            catch (Exception e) //*** Error
            {
                MsgError = e.ToString();

                return "";
            }
        }

        /// <summary>
        ///*** Delete Docuemnt Folder inside parent folder
        /// *** Incase of error it set static variable MsgError with error details
        /// </summary>        
        /// <param name="strFolderGUID">Folder GUID.</param>        
        /// <returns>Deleteion Result</returns>
        public bool DeleteDocumentFolder(string strFolderGUID)
        {
            try
            {
                //*** Get Exact Online Client Object
                var client = new ExactOnlineClient(_endpoint, GetAccessToken);

                //*** Get Folder Entity
                var documentFolder = client.For<DocumentFolder>().Top(1).Where("ID eq guid'" + strFolderGUID + "'").Select(new string[] { "ID"}).Get();

                if (documentFolder.Count > 0)
                {

                    return client.For<DocumentFolder>().Delete(documentFolder[0]);
                }
                else
                {
                    MsgError = "Folder not found";
                    return false;
                }
            }
            catch (Exception e) //*** Error
            {
                MsgError = e.ToString();

                return false;
            }
        }

        /// <summary>
        ///*** Delete Docuemnt Folder inside parent folder
        /// *** Incase of error it set static variable MsgError with error details
        /// </summary>        
        /// <param name="strFolderGUID">Folder GUID.</param>        
        /// <returns>Deleteion Result</returns>
        public bool DeleteDocument(string strDocumentGUID)
        {
            try
            {
                //*** Get Exact Online Client Object
                var client = new ExactOnlineClient(_endpoint, GetAccessToken);

                //*** Get Folder Entity
                var document = client.For<Document>().Top(1).Where("ID eq guid'" + strDocumentGUID + "'").Select(new string[] { "ID" }).Get();

                if (document.Count > 0)
                {

                    return client.For<Document>().Delete(document[0]);
                }
                else
                {
                    MsgError = "Document not found";
                    return false;
                }              
            }
            catch (Exception e) //*** Error
            {
                MsgError = e.ToString();

                return false;
            }
        }

        /// <summary>
        ///*** Get the documents Type "Attachment".
        /// *** Incase of error it set static variable MsgError with error details
        /// </summary>                        
        /// <returns>DocumentType Attachment ID</returns>
        private int getAttachmentDocumentTypeId()
        {
            try
            {
                //*** Get Exact Online Client Object
                var client = new ExactOnlineClient(_endpoint, GetAccessToken);

                //*** Get result into Array List
                var documentsType = client.For<DocumentType>().Top(1).Where("Description eq 'Attachment'").Select(new string[] { "ID" }).Get();

                if (documentsType.Count > 0)
                    return documentsType[0].ID;
                else
                    return 0;                
            }
            catch (Exception e) //*** Error
            {
                MsgError = e.ToString();

                return 0;
            }
        }

    }
}
