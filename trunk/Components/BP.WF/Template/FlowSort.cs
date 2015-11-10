using System;
using System.Data;
using System.Collections;
using BP.DA;
using BP.En;
using BP.Port;
using BP.Sys;

namespace BP.WF
{
    /// <summary>
    ///  Process category attribute 
    /// </summary>
    public class FlowSortAttr : EntityTreeAttr
    {
    }
    /// <summary>
    ///   Process Category 
    /// </summary>
    public class FlowSort : EntityTree
    {
        #region  Constructor 
        /// <summary>
        ///  Process Category 
        /// </summary>
        public FlowSort()
        {
        }
        /// <summary>
        ///  Process Category 
        /// </summary>
        /// <param name="_No"></param>
        public FlowSort(string _No) : base(_No) { }
        #endregion

        /// <summary>
        ///  Process Category Map
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("WF_FlowSort");
                map.EnDesc = " Process Category ";
                map.CodeStruct = "2";
                map.IsAllowRepeatName = false;

                map.DepositaryOfEntity = Depositary.Application;
                map.DepositaryOfMap = Depositary.Application;

                map.AddTBStringPK(FlowSortAttr.No, null, " Serial number ", true, true, 1, 10, 20);
                map.AddTBString(FlowSortAttr.Name, null, " Name ", true, false, 0, 100, 30);
                map.AddTBString(FlowSortAttr.ParentNo, null, " Parent No", false, false, 0, 100, 30);
                map.AddTBString(FlowSortAttr.TreeNo, null, "TreeNo", false, false, 0, 100, 30);

                map.AddTBInt(FlowSortAttr.Idx, 0, "Idx", false, false);
                map.AddTBInt(FlowSortAttr.IsDir, 0, "IsDir", false, false);

                this._enMap = map;
                return this._enMap;
            }
        }

        #region  Overriding methods 
        public void WritToGPM()
        {
            return;

            if (BP.WF.Glo.OSModel == OSModel.WorkFlow)
                return;

            string pMenuNo = "";

            #region  Check the system , And return to the system menu number 
            string sql = "SELECT * FROM GPM_App where No='" + SystemConfig.SysNo + "'";

            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            if (dt != null && dt.Rows.Count == 0)
            {
                // System Category 
                sql = "SELECT No FROM GPM_Menu WHERE ParentNo=0";
                string sysRootNo = DBAccess.RunSQLReturnStringIsNull(sql,"0");
                //  The key number to obtain primary function menu .
                pMenuNo = DBAccess.GenerOID("BP.GPM.Menu").ToString();
                string url = Glo.HostURL;
                /* If there is no system , It is inserted into the system menu .*/
                sql = "INSERT INTO GPM_Menu(No,Name,ParentNo,IsDir,MenuType,FK_App,IsEnable,Flag)";
                sql += " VALUES('{0}','{1}','{2}',1,2,'{3}',1,'{4}')";
                sql = string.Format(sql, pMenuNo, SystemConfig.SysName, sysRootNo, SystemConfig.SysNo, "FlowSort" + SystemConfig.SysNo);
                DBAccess.RunSQL(sql);
                /* If there is no system , It is inserted into the system .*/
                sql = "INSERT INTO GPM_App(No,Name,AppModel,FK_AppSort,Url,RefMenuNo,MyFileName)";
                sql += " VALUES('{0}','{1}',0,'01','{2}','{3}','admin.gif')";

                sql = string.Format(sql, SystemConfig.SysNo, SystemConfig.SysName, url, pMenuNo);
                sql = sql.Replace("//", "/");
                DBAccess.RunSQL(sql);
            }
            else
            {
                pMenuNo = dt.Rows[0]["RefMenuNo"].ToString();
            }
            #endregion

            try
            {
                sql = "SELECT * FROM GPM_Menu WHERE Flag='FlowSort" + this.No + "'";
                dt = DBAccess.RunSQLReturnTable(sql);
                if (dt.Rows.Count >= 1)
                    DBAccess.RunSQL("DELETE FROM GPM_Menu WHERE Flag='FlowSort" + this.No + "' AND FK_App='" + SystemConfig.SysNo + "' ");
            }
            catch
            {
            }

            //  Organize data .
            //  Get his ParentNo
            string parentNo = "";//this.ParentNo
            if (!string.IsNullOrEmpty(this.ParentNo))
            {
                sql = "SELECT * FROM GPM_Menu WHERE Flag='FlowSort" + this.ParentNo + "'";
                dt = DBAccess.RunSQLReturnTable(sql);
                if (dt.Rows.Count >= 1)
                {
                    pMenuNo = dt.Rows[0]["No"].ToString();
                }
            }

            string menuNo = DBAccess.GenerOID("BP.GPM.Menu").ToString();
            //  An insert .
            sql = "INSERT INTO GPM_Menu(No,Name,ParentNo,IsDir,MenuType,FK_App,IsEnable,Flag)";
            sql += " VALUES('{0}','{1}','{2}',{3},{4},'{5}',{6},'{7}')";
            sql = string.Format(sql, menuNo, this.Name, pMenuNo, 1, 3, SystemConfig.SysNo, 1, "FlowSort" + this.No);
            DBAccess.RunSQL(sql);
        }

        protected override bool beforeInsert()
        {
            this.WritToGPM();
            return base.beforeInsert();
        }

        protected override bool beforeDelete()
        {
            try
            {
                // Delete rights management 
                if (BP.WF.Glo.OSModel == OSModel.BPM)
                    DBAccess.RunSQL("DELETE FROM GPM_Menu WHERE Flag='FlowSort" + this.No + "' AND FK_App='" + SystemConfig.SysNo + "'");
            }
            catch
            {
            }
            return base.beforeDelete();
        }

        protected override bool beforeUpdate()
        {
            // Modify permissions management 
            if (BP.WF.Glo.OSModel == OSModel.BPM)
            {
                DBAccess.RunSQL("UPDATE  GPM_Menu SET Name='" + this.Name + "' WHERE Flag='FlowSort" + this.No + "' AND FK_App='" + SystemConfig.SysNo + "'");
            }
            return base.beforeUpdate();
        }
        #endregion  Overriding methods 
    }
    /// <summary>
    ///  Process Category 
    /// </summary>
    public class FlowSorts : EntitiesTree
    {
        /// <summary>
        ///  Process Category s
        /// </summary>
        public FlowSorts() { }
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new FlowSort();
            }

        }
        /// <summary>
        ///  Process Category s
        /// </summary>
        /// <param name="no">ss</param>
        /// <param name="name">anme</param>
        public void AddByNoName(string no, string name)
        {
            FlowSort en = new FlowSort();
            en.No = no;
            en.Name = name;
            this.AddEntity(en);
        }
        public override int RetrieveAll()
        {
            int i = base.RetrieveAll();
            if (i == 0)
            {
                FlowSort fs = new FlowSort();
                fs.Name = " Document type ";
                fs.No = "01";
                fs.Insert();

                fs = new FlowSort();
                fs.Name = " Office class ";
                fs.No = "02";
                fs.Insert();
                i = base.RetrieveAll();
            }

            return i;
        }
    }
}
