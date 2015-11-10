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
    ///  Modify personnel numbers   The summary 
    /// </summary>
    public class ChangeUserNo : Method
    {
        /// <summary>
        ///  Method with no arguments 
        /// </summary>
        public ChangeUserNo()
        {
            this.Title = " Modify personnel numbers £¨ The original called number in one operation A, Now modified B£©";
            this.Help = " Please carefully executed , Please back up your database before performing , The system will generate the SQL On the log , Open the log file (" + BP.Sys.SystemConfig.PathOfDataUser + "\\Log), Then find these sql.";
        }
        /// <summary>
        ///  Set the execution variables 
        /// </summary>
        /// <returns></returns>
        public override void Init()
        {
            this.Warning = " Are you sure you want to perform ?";
            HisAttrs.AddTBString("P1", null, " Original username ", true, false, 0, 10, 10);
            HisAttrs.AddTBString("P2", null, " New user name ", true, false, 0, 10, 10);
        }
        /// <summary>
        ///  Whether the current operator can perform this method 
        /// </summary>
        public override bool IsCanDo
        {
            get
            {
                if (BP.Web.WebUser.No == "admin")
                    return true;
                else
                    return false;
            }
        }
        /// <summary>
        ///  Carried out 
        /// </summary>
        /// <returns> Return to the results </returns>
        public override object Do()
        {
            string oldNo = this.GetValStrByKey("P1");
            string newNo = this.GetValStrByKey("P2");

            string sqls = "";

            sqls += "UPDATE Port_Emp Set No='" + newNo + "' WHERE No='" + oldNo + "'";
            sqls += "\t\n UPDATE Port_EmpDept Set FK_Emp='" + newNo + "' WHERE FK_Emp='" + oldNo + "'";
            sqls += "\t\n UPDATE Port_EmpStation Set FK_Emp='" + newNo + "' WHERE FK_Emp='" + oldNo + "'";

            MapDatas mds = new MapDatas();
            mds.RetrieveAll();

            foreach (MapData md in mds)
            {
                MapAttrs attrs = new MapAttrs(md.No);
                foreach (MapAttr attr in attrs)
                {
                    if (attr.UIIsEnable == false && attr.DefValReal == "@WebUser.No")
                    {
                        sqls += "\t\n UPDATE " + md.PTable + " SET ";
                    }
                    continue;

                }
                sqls += "UPDATE";

            }

            return " Successful implementation ..." + sqls;
        }
    }
}
