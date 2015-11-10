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
    ///  Regenerate title   The summary 
    /// </summary>
    public class ClearRepLineLab : Method
    {
        /// <summary>
        ///  Method with no arguments 
        /// </summary>
        public ClearRepLineLab()
        {
            this.Title = " Clear form in duplicate Line Lab  Data ";
            this.Help = " Due to the previous form template Bug Tag and the line leading to duplication of data .";
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
            FrmLines ens = new FrmLines();
            ens.RetrieveAllFromDBSource();
            string sql = "";
            foreach (FrmLine item in ens)
            {
                sql = "DELETE FROM " + item.EnMap.PhysicsTable + " WHERE FK_MapData='" + item.FK_MapData + "' AND X=" + item.X + " AND Y=" + item.Y + " and x1=" + item.X1 + " and x2=" + item.X2 + " and y1=" + item.Y1 + " and y2=" + item.Y2;
                DBAccess.RunSQL(sql);
                item.MyPK = BP.DA.DBAccess.GenerOIDByGUID().ToString();
                item.Insert();
            }


            FrmLabs labs = new FrmLabs();
            labs.RetrieveAllFromDBSource();
            foreach (FrmLab item in labs)
            {
                sql = "DELETE FROM " + item.EnMap.PhysicsTable + " WHERE FK_MapData='" + item.FK_MapData + "' and x=" + item.X + " and y=" + item.Y + " and Text='" + item.Text + "'";
                DBAccess.RunSQL(sql);
                item.MyPK = BP.DA.DBAccess.GenerOIDByGUID().ToString();
                item.Insert();
            }
            return " Deleted successfully ";
        }
    }
}
