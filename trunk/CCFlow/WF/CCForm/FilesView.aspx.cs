using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using BP;
using BP.Sys;
using BP.Web;

namespace CCFlow.WF.CCForm
{
    public partial class FilesView : PageBase
    {
        #region   Property 
        public string DelPKVal
        {
            get
            {
                return this.Request.QueryString["DelPKVal"];
            }
        }
        public string DoType
        {
            get { return this.Request.QueryString["DoType"]; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            FrmAttachmentDB downDB = new FrmAttachmentDB();

            if (DoType.Equals("view"))
            {
                if (!string.IsNullOrEmpty(DelPKVal))
                {
                    downDB.MyPK = this.DelPKVal;
                    downDB.Retrieve();
                    string filePath = "";

                    try
                    {
                        filePath = Server.MapPath("~/" + downDB.FileFullName);
                    }
                    catch (Exception ex)
                    {
                        filePath = downDB.FileFullName;

                    }

                    if (downDB.FileExts.ToUpper().Equals("CEB"))
                    {
                        // Judgment is an absolute path or a relative path 
                        string fileSave = Server.MapPath("~/DataUser/UploadFile/"+ downDB.MyPK + "." + downDB.FileName);

                        if (!System.IO.File.Exists(fileSave))
                        {

                            byte[] fileBytes = File.ReadAllBytes(filePath);

                            File.WriteAllBytes(fileSave, fileBytes);

                        }
                        this.Response.Redirect("/DataUser/UploadFile/" + downDB.MyPK + "." +
                                                      downDB.FileName,true);
                    }
                   
                    if (File.Exists(filePath))
                    {
                        byte[] result;
                        try
                        {
                            result = File.ReadAllBytes(filePath);
                        }
                        catch
                        {
                            result = File.ReadAllBytes(downDB.FileFullName);
                        }

                        Response.Clear();
                        if (downDB.FileExts == "pdf")
                            Response.ContentType = "Application/pdf";

                        Response.BinaryWrite(result);
                        Response.End();
                    }
                    else
                    {
                        this.Alert(" File not found .");
                        this.WinClose();
                    }
                }
            }
        }
    }
}