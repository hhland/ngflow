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
    ///  Repair Database   The summary 
    /// </summary>
    public class RepariDB : Method
    {
        /// <summary>
        ///  Method with no arguments 
        /// </summary>
        public RepariDB()
        {
            this.Title = " Repair Database ";
            this.Help = " The latest version of the current data table structure , Do an auto repair ,  Fixes : Missing Column , Missing Column Comments , Column annotation is incomplete or there is a change .";
            this.Help += "<br> Because the Form Designer error , Lost field , It can also automatically fix by .";
            this.Help += "<br><a href='/'> Into the Process Designer </a>";
        }
        /// <summary>
        ///  Set the execution variables 
        /// </summary>
        /// <returns></returns>
        public override void Init()
        {
            //this.Warning = " Are you sure you want to perform ?";
            //HisAttrs.AddTBString("P1", null, " Old Password ", true, false, 0, 10, 10);
            //HisAttrs.AddTBString("P2", null, " New Password ", true, false, 0, 10, 10);
            //HisAttrs.AddTBString("P3", null, " Confirm ", true, false, 0, 10, 10);
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
            string rpt =PubClass.DBRpt(BP.DA.DBCheckLevel.High);

            ////  Manually upgrade . 2011-07-08  Add nodes Field Grouping .
            //string sql = "DELETE FROM Sys_EnCfg WHERE No='BP.WF.Template.Ext.NodeSheet'";
            //BP.DA.DBAccess.RunSQL(sql);

            //sql = "INSERT INTO Sys_EnCfg(No,GroupTitle) VALUES ('BP.WF.Template.Ext.NodeSheet','NodeID= Basic Configuration @WarningDays= Property assessment @SendLab= Feature button labels and status ')";
            //BP.DA.DBAccess.RunSQL(sql);

            //  Repair due bug Missing fields .
            MapDatas mds = new MapDatas();
            mds.RetrieveAll();
            foreach (MapData md in mds)
            {
                string nodeid = md.No.Replace("ND","");
                try
                {
                    BP.WF.Node nd = new Node(int.Parse(nodeid));
                    nd.RepareMap();
                    continue;
                }
                catch(Exception ex)
                {

                }

                MapAttr attr = new MapAttr();
                if (attr.IsExit(MapAttrAttr.KeyOfEn, "OID", MapAttrAttr.FK_MapData, md.No) == false)
                {
                    attr.FK_MapData = md.No;
                    attr.KeyOfEn = "OID";
                    attr.Name = "OID";
                    attr.MyDataType = BP.DA.DataType.AppInt;
                    attr.UIContralType = UIContralType.TB;
                    attr.LGType = FieldTypeS.Normal;
                    attr.UIVisible = false;
                    attr.UIIsEnable = false;
                    attr.DefVal = "0";
                    attr.HisEditType = BP.En.EditType.Readonly;
                    attr.Insert();
                }
            }
            return " Successful implementation ...";
        }
    }
}
