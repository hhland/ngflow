using System;
using System.IO;
using System.Data;
using System.Collections;
using BP.DA;
using BP.Web.Controls;
using System.Reflection;
using BP.Port;
using BP.En;
using BP.Sys;
namespace BP.WF.DTS
{
    /// <summary>
    /// Method  The summary 
    /// </summary>
    public class LoadTemplete : Method
    {
        /// <summary>
        ///  Method with no arguments 
        /// </summary>
        public LoadTemplete()
        {
            this.Title = " Load flow presentation template ";
            this.Help = " To help you learn and master lover ccflow,  Special offer some form templates and process templates to facilitate learning .";
            this.Help += "@ These templates are located " + SystemConfig.PathOfData + "\\FlowDemo\\";
        }
        /// <summary>
        ///  Set the execution variables 
        /// </summary>
        /// <returns></returns>
        public override void Init()
        {
        }
        /// <summary>
        ///  Whether the current operator can perform this method 
        /// </summary>
        public override bool IsCanDo
        {
            get
            {
                return true;
            }
        }
        public override object Do()
        {
            string msg = "";

            #region  Dealing with Forms .
            //  Scheduling form file .
            SysFormTrees fss = new SysFormTrees();
            fss.ClearTable();

            string frmPath = SystemConfig.PathOfData + "\\FlowDemo\\Form\\";
            DirectoryInfo dirInfo = new DirectoryInfo(frmPath);
            DirectoryInfo[] dirs = dirInfo.GetDirectories();
            foreach (DirectoryInfo item in dirs)
            {
                if (item.FullName.Contains(".svn"))
                    continue;

                string[] fls = System.IO.Directory.GetFiles(item.FullName);
                if (fls.Length == 0)
                    continue;
                SysFormTree fs = new SysFormTree();
                fs.No = item.Name.Substring(0, 2);
                fs.Name = item.Name.Substring(3);
                fs.Insert();

                foreach (string f in fls)
                {
                    try
                    {
                        msg += "@ Begin scheduling form template file :" + f;
                        System.IO.FileInfo info = new System.IO.FileInfo(f);
                        if (info.Extension != ".xml")
                            continue;

                        DataSet ds = new DataSet();
                        ds.ReadXml(f);

                        MapData md = MapData.ImpMapData(ds, false);
                        md.FK_FrmSort = fs.No;
                        md.Update();
                    }
                    catch (Exception ex)
                    {
                        msg += "@ Scheduling failure " + ex.Message;
                    }
                }
            }
            #endregion  Dealing with Forms .

            #region  Processing .
            FlowSorts sorts = new FlowSorts();
            sorts.ClearTable();
              dirInfo = new DirectoryInfo(SystemConfig.PathOfData + "\\FlowDemo\\Flow\\");
           dirs = dirInfo.GetDirectories();

            FlowSort fsRoot = new FlowSort();
            fsRoot.No = "99";
            fsRoot.Name = " Process tree ";
            fsRoot.ParentNo = "0";
            fsRoot.DirectInsert();

            foreach (DirectoryInfo dir in dirs)
            {
                if (dir.FullName.Contains(".svn"))
                    continue;

                string[] fls = System.IO.Directory.GetFiles(dir.FullName);
                if (fls.Length == 0)
                    continue;

                FlowSort fs = new FlowSort();
                fs.No = dir.Name.Substring(0, 2);
                fs.Name = dir.Name.Substring(3);
                fs.ParentNo = fsRoot.No;
                fs.Insert();

                foreach (string filePath in fls)
                {
                    msg += "@ Begin scheduling process template file :" + filePath;
                    Flow myflow = BP.WF.Flow.DoLoadFlowTemplate(fs.No, filePath, ImpFlowTempleteModel.AsTempleteFlowNo);
                    msg += "@ Process :" + myflow.Name + " Load success .";

                    System.IO.FileInfo info = new System.IO.FileInfo(filePath);
                    myflow.Name = info.Name.Replace(".xml", "");
                    if (myflow.Name.Substring(2, 1) == ".")
                        myflow.Name = myflow.Name.Substring(3);
                    myflow.DirectUpdate();
                }


                // It under a directory scheduling .
                DirectoryInfo dirSubInfo = new DirectoryInfo(SystemConfig.PathOfData + "\\FlowDemo\\Flow\\"+dir.Name);
                DirectoryInfo[] myDirs = dirSubInfo.GetDirectories();
                foreach (DirectoryInfo mydir in myDirs)
                {
                    if (mydir.FullName.Contains(".svn"))
                        continue;

                    string[] myfls = System.IO.Directory.GetFiles(mydir.FullName);
                    if (myfls.Length == 0)
                        continue;

                    // 
                    FlowSort subFlowSort = fs.DoCreateSubNode() as FlowSort;
                    subFlowSort.Name = mydir.Name.Substring(3);
                    subFlowSort.Update();

                    foreach (string filePath in myfls)
                    {
                        msg += "@ Begin scheduling process template file :" + filePath;
                        Flow myflow = BP.WF.Flow.DoLoadFlowTemplate(subFlowSort.No, filePath, ImpFlowTempleteModel.AsTempleteFlowNo);
                        msg += "@ Process :" + myflow.Name + " Load success .";

                        System.IO.FileInfo info = new System.IO.FileInfo(filePath);
                        myflow.Name = info.Name.Replace(".xml", "");
                        if (myflow.Name.Substring(2, 1) == ".")
                            myflow.Name = myflow.Name.Substring(3);
                        myflow.DirectUpdate();
                    }
                }

            }
            #endregion  Processing .


            BP.DA.Log.DefaultLogWriteLineInfo(msg);

            // Remove the extra space .
            BP.WF.DTS.DeleteBlankGroupField dts = new DeleteBlankGroupField();
            dts.Do();

            return msg;
        }
    }
}
