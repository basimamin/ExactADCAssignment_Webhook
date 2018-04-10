using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dropbox.Api;
using Dropbox.Api.Team;
using Dropbox.Api.Users;
using Dropbox.Api.Files;

namespace ExactAssignment.BLL
{
    //*** This Class has all Functions that connect to DropBox APIs and use it
    public class DropBoxConnector
    {

        public static string dropBoxAccessToken;
        public static string MsgError;
        public static DropBoxUser objDropBoxUser;

        /// <summary>
        /// *** This Function is used to get DropBox Access Token URL
        /// </summary>
        /// <param name="dropBoxAppKey">The Dropbox Application Key</param>
        /// <param name="dropBoxAppSecret">The Dropbox Application Secret.</param>
        /// <param name="returnBackURL">The Dropbox Return Back URL.</param>
        /// <returns>Token URL.</returns>
        public static string getAccessTokenURL(string dropBoxAppKey, string dropBoxAppSecret, string returnBackURL)
        {            
            var redirect = DropboxOAuth2Helper.GetAuthorizeUri(
                OAuthResponseType.Code,
                dropBoxAppKey,
                returnBackURL);

            return redirect.ToString();            
        }

        /// <summary>
        /// *** This Function is used to get DropBox Access Token and set static variable with it
        /// *** Incase of error it set static variable MsgError with error details
        /// </summary>
        /// <param name="ResponseCode">Response Code returned from drop Box</param>
        /// <param name="dropBoxAppKey">The Dropbox Application Key</param>
        /// <param name="dropBoxAppSecret">The Dropbox Application Secret.</param>
        /// <param name="returnBackURL">The Dropbox Return Back URL.</param>
        /// <returns></returns>
        public static async Task getAccessTokenFromResponse(string ResponseCode, string dropBoxAppKey, 
                                                            string dropBoxAppSecret, string returnBackURL)                                
        {
            //*** Initialization
            MsgError = "";

            try
            {
                var response = await DropboxOAuth2Helper.ProcessCodeFlowAsync(
                       ResponseCode,
                       dropBoxAppKey,
                       dropBoxAppSecret,
                       returnBackURL);
                                
                dropBoxAccessToken = response.AccessToken;

            }
            catch (Exception e)
            {
                var message = string.Format(
                    "code: {0}\nAppKey: {1}\nAppSecret: {2}\nRedirectUri: {3}\nException : {4}",
                    ResponseCode,
                    dropBoxAppKey,
                    dropBoxAppSecret,
                    returnBackURL,
                    e);
                MsgError = message.ToString();
            }
        }

        /// <summary>
        /// *** This Function is used to get DropBox User Info Class
        /// *** Incase of error it set static variable MsgError with error details
        /// </summary>
        /// <param name="objDropboxClient">DropBox Client Object</param>
        /// <returns></returns>
        public static async Task getUserInfo(object objDropboxClient)
        {            
            //*** Initialization
            MsgError = "";

            try
            {
                //*** 1. Casting
                DropboxClient dropboxClient = (DropboxClient)objDropboxClient;
 
                //*** Get DropBox Client
                var full = await dropboxClient.Users.GetCurrentAccountAsync();

                //*** Set Class with Values
                //DropBoxUser objDropBoxUser = new DropBoxUser();
                objDropBoxUser = new DropBoxUser();

                objDropBoxUser.AccountId = full.AccountId;
                objDropBoxUser.AccountCountry = full.Country;
                objDropBoxUser.AccountEmail = full.Email;
                objDropBoxUser.AccountIsPaired = full.IsPaired;
                objDropBoxUser.AccountLocale = full.Locale;
                objDropBoxUser.AccountDisplayName = full.Name.DisplayName;
                objDropBoxUser.AccountFamiliarName = full.Name.FamiliarName;
                objDropBoxUser.AccountGivenName = full.Name.GivenName;
                objDropBoxUser.AccountSurname = full.Name.Surname;
                objDropBoxUser.AccountReferralLink = full.ReferralLink;

                //*** If Has Team
                if (full.Team != null)
                {
                    objDropBoxUser.AccountTeamId = full.Team.Id;
                    objDropBoxUser.AccountTeamName = full.Team.Name;
                }

                //return objDropBoxUser;
            }
            catch (Exception e) //*** Error
            {
                MsgError = e.ToString();

                //return null;
            }           
        }

        /// <summary>
        /// *** This Function is used to get DropBox Client Object using Token
        /// *** Incase of error it set static variable MsgError with error details
        /// </summary>
        /// <param name="DropboxToken">Drop Box Token</param>
        /// <returns>Drop Box Client Object</returns>
        public static object getDropboxClient(string DropboxToken)
        { 
            //*** Specify socket level timeout which decides maximum waiting time when on bytes are
            //*** received by the socket.
            var httpClient = new System.Net.Http.HttpClient(new System.Net.Http.WebRequestHandler { ReadWriteTimeout = 10 * 1000 })
            {
                //*** Specify request level timeout which decides maximum time taht can be spent on
                //*** download/upload files.
                Timeout = TimeSpan.FromMinutes(20)
            };

            try
            {
                //*** Struct Config Class
                var config = new DropboxClientConfig("BasimAssignment")
                {
                    HttpClient = httpClient
                };

                //*** Get Client Object
                return new DropboxClient(DropboxToken, config);
            }
            catch (Exception e) //*** Error
            {
                MsgError = e.ToString();

                return null;
            }
         }

        /// <summary>
        ///*** Lists the items within a folder.
        /// *** Incase of error it set static variable MsgError with error details
        /// </summary>
        /// <remarks>This demonstrates calling an rpc style api in the Files namespace.</remarks>
        /// <param name="strFolderPath">The path to list.</param>
        /// <param name="objDropboxClient">DropBox Client Object</param>
        /// <returns>List Of DropBox Class instance</returns>
        public static async Task<List<DropBoxFile>> ListFolder(object objDropboxClient, string strFolderPath)
        {
             try
            {
                //*** 1. Casting
                DropboxClient dropboxClient = (DropboxClient)objDropboxClient;
 
                //*** Define File/Folder List
                List<DropBoxFile> lstFoldersFiles = new List<DropBoxFile>(); 

                //*** Call API Method
                var list = await dropboxClient.Files.ListFolderAsync(strFolderPath);
             
                //***************************
                //*** get folders then files
                //***************************
                More:
                //*** Folder
                foreach (var item in list.Entries.Where(i => i.IsFolder))
                {
                    lstFoldersFiles.Add(new DropBoxFile { FileName = item.Name, FilePath = strFolderPath, isFolder = true, isDeleted=item.IsDeleted });
                }

                //*** Files
                foreach (var item in list.Entries.Where(i => i.IsFile))
                {
                    var file = item.AsFile;

                    lstFoldersFiles.Add(new DropBoxFile { FileName = item.Name, FilePath = strFolderPath, isFolder = false, isDeleted = item.IsDeleted, FileSizeInBytes = file.Size, ModificationDate=file.ServerModified });

                }

                //**** If There is More Files/Folders in List
                while (list.HasMore)
                {
                    list = await dropboxClient.Files.ListFolderContinueAsync(list.Cursor);

                    goto More;
                }

                return lstFoldersFiles;
            }
             catch (Exception e) //*** Error
             {
                 MsgError = e.ToString();

                 return null;
             }
        }

        /// <summary>
        ///*** Creates the specified folder.
        /// *** Incase of error it set static variable MsgError with error details
        /// </summary>
        /// <remarks>This demonstrates calling an rpc style api in the Files namespace.</remarks>
        /// <param name="strFolderPath">The path to list.</param>
        /// <param name="objDropboxClient">DropBox Client Object</param>
        /// <returns>Creation Result Boolean</returns>
        public static async Task<bool> CreateFolder(object objDropboxClient, string strFolderPath)
        {
             try
            {
                //*** 1. Casting
                DropboxClient dropboxClient = (DropboxClient)objDropboxClient;
                             
                var folderArg = new CreateFolderArg(strFolderPath);
                var folder = await dropboxClient.Files.CreateFolderAsync(folderArg);
                 
                return true;
            }
            catch (Exception e) //*** Error
            {
                MsgError = e.ToString();

                return false;
            }
        }

        /// <summary>
        ///*** Delete the specified folder Or File.
        /// *** Incase of error it set static variable MsgError with error details
        /// </summary>
        /// <remarks>This demonstrates calling an rpc style api in the Files namespace.</remarks>
        /// <param name="strPath">The path to list.</param>
        /// <param name="objDropboxClient">DropBox Client Object</param>
        /// <returns>Deletion Result Boolean</returns>
        public static async Task<bool> DeleteFileOrFolder(object objDropboxClient, string strPath)
        {
            try
            {
                //*** 1. Casting
                DropboxClient dropboxClient = (DropboxClient)objDropboxClient;

                var folder = await dropboxClient.Files.DeleteAsync(strPath);

                return true;
            }
            catch (Exception e) //*** Error
            {
                MsgError = e.ToString();

                return false;
            }
        }

        /// <summary>
        ///*** Downloads a file.
        /// </summary>
        /// <remarks>This demonstrates calling a download style api in the Files namespace.</remarks>
        /// <param name="objDropboxClient">The Dropbox client.</param>
        /// <param name="strFilePath">The File path in which the file should be found.</param>        
        /// <returns>File Stream</returns>
        public static async Task<System.IO.Stream> Download(object objDropboxClient, string strFilePath)
        {
             try
            {
                //*** 1. Casting
                DropboxClient dropboxClient = (DropboxClient)objDropboxClient;

                //*** Get File Stream
                var response = await dropboxClient.Files.DownloadAsync(strFilePath);
                
                //*** Return File Stream
                return await response.GetContentAsStreamAsync();
            }
             catch (Exception e) //*** Error
             {
                 MsgError = e.ToString();

                 return null;
             }
        }

        /// <summary>
        ///*** Upload a file.
        /// </summary>
        /// <remarks>This demonstrates calling a download style api in the Files namespace.</remarks>
        /// <param name="objDropboxClient">The Dropbox client.</param>
        /// <param name="strFilePath">The File path in which the file should be found.</param>        
        /// <param name="FileStream">File Content Stream IO</param>        
        /// <returns>upload result boolean</returns>
        public static async Task<bool> Upload(object objDropboxClient, string strFilePath, System.IO.Stream FileStream)
        {
            try
            {
                //*** 1. Casting
                DropboxClient dropboxClient = (DropboxClient)objDropboxClient;

                //*** Upload
                await dropboxClient.Files.UploadAsync(strFilePath, WriteMode.Overwrite.Instance, body: FileStream);

                //*** Return File Stream
                return true;
            }
            catch (Exception e) //*** Error
            {
                MsgError = e.ToString();

                return false;
            }
        }
    }
}
