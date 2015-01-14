using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Web.Hosting;

namespace CCFlow.WF
{
    /// <summary>
    /// SilverlightUploadService  The summary 
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    //  To allow the use of  ASP.NET AJAX  Call this from a script  Web  Service , Please cancel the downlink comment .
    // [System.Web.Script.Services.ScriptService]
    public class SilverlightUploadService : System.Web.Services.WebService
    {

        public SilverlightUploadService()
        {

            // If you are using design components , Uncomment the following line  
            //InitializeComponent(); 
        }


        private string _tempExtension = "_temp";

        [WebMethod]
        public void StoreFileAdvanced(string fileName, byte[] data, int dataLength, string parameters, bool firstChunk, bool lastChunk)
        {
            string uploadFolder = GetUploadFolder();
            string tempFileName = fileName + _tempExtension;

            if (firstChunk)
            {
                //Delete temp file
                if (File.Exists(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + tempFileName))
                    File.Delete(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + tempFileName);

                //Delete target file
                if (File.Exists(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + fileName))
                    File.Delete(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + fileName);

            }


            FileStream fs = File.Open(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + tempFileName, FileMode.Append);
            fs.Write(data, 0, dataLength);
            fs.Close();

            if (lastChunk)
            {
                //Rename file to original file
                File.Move(HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + tempFileName, HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + fileName);

                //Finish stuff....
                FinishedFileUpload(fileName, parameters);
            }

        }

        [WebMethod]
        public void CancelUpload(string fileName)
        {
            string uploadFolder = GetUploadFolder();
            string tempFileName = fileName + _tempExtension;

            if (File.Exists(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + tempFileName))
                File.Delete(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + tempFileName);

        }

        protected void DeleteUploadedFile(string fileName)
        {
            string uploadFolder = GetUploadFolder();


            if (File.Exists(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + fileName))
                File.Delete(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + fileName);
        }

        protected virtual void FinishedFileUpload(string fileName, string parameters)
        {
        }

        protected virtual string GetUploadFolder()
        {
            return "Upload";
        }

    }
}
