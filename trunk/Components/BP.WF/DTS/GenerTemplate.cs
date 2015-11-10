using System;
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
    public class GenerTemplate : Method
    {
        /// <summary>
        ///  Method with no arguments 
        /// </summary>
        public GenerTemplate()
        {
            this.Title = " Generating process templates and form templates ";
            this.Help = " The system processes and converted into a form template in the specified directory .";
        //    this.Warning = " Are you sure you want to perform ?";

            this.HisAttrs.AddTBString("Path", "C:\\ccflow.Template", " Path generation ", true, false, 1, 1900, 200);
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
        /// <summary>
        ///  Carried out 
        /// </summary>
        /// <returns> Return to the results </returns>
        public override object Do()
        {
            string path = this.GetValStrByKey("Path") + "_" + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            if (System.IO.Directory.Exists(path))
                return " System being implemented , Please wait .";

            System.IO.Directory.CreateDirectory(path);
            System.IO.Directory.CreateDirectory(path + "\\Flow. Process Template ");
            System.IO.Directory.CreateDirectory(path + "\\Frm. Form Template ");

            Flows fls = new Flows();
            fls.RetrieveAll();
            FlowSorts sorts = new FlowSorts();
            sorts.RetrieveAll();

            //  Generation process template .
            foreach (FlowSort sort in sorts)
            {
                string pathDir = path + "\\Flow. Process Template \\" + sort.No + "." + sort.Name;
                System.IO.Directory.CreateDirectory(pathDir);
                foreach (Flow fl in fls)
                {
                    fl.DoExpFlowXmlTemplete(pathDir);
                }
            }

            //  Generate a form template .
            foreach (FlowSort sort in sorts)
            {
                string pathDir = path + "\\Frm. Form Template \\" + sort.No + "." + sort.Name;
                System.IO.Directory.CreateDirectory(pathDir);
                foreach (Flow fl in fls)
                {
                    string pathFlowDir = pathDir + "\\" + fl.No + "." + fl.Name;
                    System.IO.Directory.CreateDirectory(pathFlowDir);
                    Nodes nds = new Nodes(fl.No);
                    foreach (Node nd in nds)
                    {
                        MapData md = new MapData("ND" + nd.NodeID);
                        System.Data.DataSet ds = md.GenerHisDataSet();
                        ds.WriteXml(pathFlowDir + "\\" + nd.NodeID + "." + nd.Name + ".Frm.xml");
                    }
                }
            }

            //  Process form template .
            SysFormTrees frmSorts = new SysFormTrees();
            frmSorts.RetrieveAll();
            foreach (SysFormTree sort in frmSorts)
            {
                string pathDir = path + "\\Frm. Form Template \\" + sort.No + "." + sort.Name;
                System.IO.Directory.CreateDirectory(pathDir);

                MapDatas mds = new MapDatas();
                mds.Retrieve(MapDataAttr.FK_FrmSort, sort.No);
                foreach (MapData md in mds)
                {
                    System.Data.DataSet ds = md.GenerHisDataSet();
                    ds.WriteXml(pathDir + "\\" + md.No + "." + md.Name + ".Frm.xml");
                }
            }
            return " Generate success , Please open " + path + ".<br> If you want to share them, please send to compressed template£Àccflow.org";
        }
    }
}
