using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BP.En;
using BP.DA;

namespace CCFlow.WF.Comm.RefFunc
{
    /// <summary>
    /// Serv  The summary 
    /// </summary>
    public class Serv : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string ensName = context.Request.QueryString["EnsName"];
            string PKVal = context.Request.QueryString["PKVal"];
            BP.En.Entities ens = BP.En.ClassFactory.GetEns(ensName);
            BP.En.Entity en =ens.GetNewEntity; 
            if (PKVal != null)
            {
                en.PKVal = PKVal;
                en.RetrieveFromDBSources();
            }
            en = BP.Sys.PubClass.CopyFromRequest(en, context.Request);
            en.Save();

            #region  Save   Entity Accessories 
            try
            {
                if (en.EnMap.Attrs.Contains("MyFileName"))
                {
                    //HtmlInputFile file = this.UCEn1.FindControl("file") as HtmlInputFile;
                    //if (file != null && file.Value.IndexOf(".") != -1)
                    //{
                    //    BP.Sys.EnCfg cfg = new EnCfg(en.ToString());
                    //    if (System.IO.Directory.Exists(cfg.FJSavePath) == false)
                    //        System.IO.Directory.CreateDirectory(cfg.FJSavePath);

                    //    /*  If you include these two fields .*/
                    //    string fileName = file.PostedFile.FileName;
                    //    fileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);

                    //    string filePath = cfg.FJSavePath;
                    //    en.SetValByKey("MyFilePath", filePath);

                    //    string ext = "";
                    //    if (fileName.IndexOf(".") != -1)
                    //        ext = fileName.Substring(fileName.LastIndexOf(".") + 1);

                    //    en.SetValByKey("MyFileExt", ext);
                    //    en.SetValByKey("MyFileName", fileName);
                    //    en.SetValByKey("WebPath", cfg.FJWebPath + en.PKVal + "." + ext);

                    //    string fullFile = filePath + "/" + en.PKVal + "." + ext;

                    //    file.PostedFile.SaveAs(fullFile);
                    //    file.PostedFile.InputStream.Close();
                    //    file.PostedFile.InputStream.Dispose();
                    //    file.Dispose();

                    //    System.IO.FileInfo info = new System.IO.FileInfo(fullFile);
                    //    en.SetValByKey("MyFileSize", BP.DA.DataType.PraseToMB(info.Length));
                    //    if (DataType.IsImgExt(ext))
                    //    {
                    //        System.Drawing.Image img = System.Drawing.Image.FromFile(fullFile);
                    //        en.SetValByKey("MyFileH", img.Height);
                    //        en.SetValByKey("MyFileW", img.Width);
                    //        img.Dispose();
                    //    }
                    //    en.Update();
                    //}
                }
            }
            catch (Exception ex)
            {
              //  this.Alert(" Save Attachments Error :" + ex.Message);
            }
            #endregion

            #region  Save   Property   Accessory 
            try
            {
                //AttrFiles fils = en.EnMap.HisAttrFiles;
                //SysFileManagers sfs = new SysFileManagers(en.ToString(), en.PKVal.ToString());
                //foreach (AttrFile fl in fils)
                //{
                //    HtmlInputFile file = (HtmlInputFile)this.UCEn1.FindControl("F" + fl.FileNo);
                //    if (file.Value.Contains(".") == false)
                //        continue;

                //    SysFileManager enFile = sfs.GetEntityByKey(SysFileManagerAttr.AttrFileNo, fl.FileNo) as SysFileManager;
                //    SysFileManager enN = null;
                //    if (enFile == null)
                //    {
                //        enN = this.FileSave(null, file, en);
                //    }
                //    else
                //    {
                //        enFile.Delete();
                //        enN = this.FileSave(null, file, en);
                //    }

                //    enN.AttrFileNo = fl.FileNo;
                //    enN.AttrFileName = fl.FileName;
                //    enN.EnName = en.ToString();
                //    enN.Update();
                //}
            }
            catch 
            {
           //     this.Alert(" Save Attachments Error :" + ex.Message);
            }
            #endregion
            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}