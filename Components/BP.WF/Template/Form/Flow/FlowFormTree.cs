using System;
using System.Data;
using System.Collections;
using BP.DA;
using BP.En;
using BP.Port;

namespace BP.WF.Template
{
    public class FlowFormTreeAttr : EntityTreeAttr
    {
        /// <summary>
        ///  Process 
        /// </summary>
        public const string FK_Flow = "FK_Flow";
        /// <summary>
        ///  Node Type 
        /// </summary>
        public const string NodeType = "NodeType";
        /// <summary>
        /// url
        /// </summary>
        public const string Url = "Url";
    }
    /// <summary>
    ///  Process tree form 
    /// </summary>
    public class FlowFormTree : EntityMultiTree
    {
        #region  Extended Attributes , Do data manipulation 
        /// <summary>
        ///  Node Type 
        /// </summary>
        public string NodeType { get; set; }
        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }
        #endregion
        #region  Property 
        public string FK_Flow
        {
            get
            {
                return this.GetValStringByKey(FrmNodeAttr.FK_Flow);
            }
            set
            {
                this.SetValByKey(FrmNodeAttr.FK_Flow, value);
            }
        }
        #endregion  Property 

        #region  Constructor 
        /// <summary>
        ///  Process tree form 
        /// </summary>
        public FlowFormTree()
        {
        }
        /// <summary>
        ///  Process tree form 
        /// </summary>
        /// <param name="_No"></param>
        public FlowFormTree(string _No) : base(_No) { }
        #endregion

        /// <summary>
        ///  Group field 
        /// </summary>
        public override string RefObjField
        {
            get { return FlowFormTreeAttr.FK_Flow; }
        }
        /// <summary>
        ///  Process tree form Map
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("WF_FlowFormTree");
                map.EnDesc = " Process tree form ";
                map.CodeStruct = "2";

                map.DepositaryOfEntity = Depositary.Application;
                map.DepositaryOfMap = Depositary.Application;

                map.AddTBStringPK(FlowFormTreeAttr.No, null, " Serial number ", true, true, 1, 10, 20);
                map.AddTBString(FlowFormTreeAttr.Name, null, " Name ", true, false, 0, 100, 30);
                map.AddTBString(FlowFormTreeAttr.ParentNo, null, " Parent No", false, false, 0, 100, 30);
                map.AddTBString(FlowFormTreeAttr.TreeNo, null, "TreeNo", false, false, 0, 100, 30);
                map.AddTBInt(FlowFormTreeAttr.Idx, 0, "Idx", false, false);
                map.AddTBInt(FlowFormTreeAttr.IsDir, 0, " Whether it is a directory ?", false, false);

                //  Membership process ID .
                map.AddTBString(FlowFormTreeAttr.FK_Flow, null, " Process ID ", true, true, 1, 20, 20);

                this._enMap = map;
                return this._enMap;
            }
        }
        //public void WritToGPM()
        //{
        //    if (BP.WF.Glo.OSModel == OSModel.WorkFlow)
        //        return;

        //    string pMenuNo = "";
        //    #region  Check the system , And return to the system menu number 
        //    string sql = "SELECT * FROM GPM_App where No='" + SystemConfig.SysNo + "'";

        //    DataTable dt = DBAccess.RunSQLReturnTable(sql);
        //    if (dt != null && dt.Rows.Count == 0)
        //    {
        //        // System Category 
        //        sql = "SELECT No FROM GPM_Menu WHERE ParentNo=0";
        //        string sysRootNo = DBAccess.RunSQLReturnStringIsNull(sql, "0");
        //        //  The key number to obtain primary function menu .
        //        pMenuNo = DBAccess.GenerOID("BP.GPM.Menu").ToString();
        //        string url = Glo.HostURL;
        //        /* If there is no system , It is inserted into the system menu .*/
        //        sql = "INSERT INTO GPM_Menu(No,Name,ParentNo,IsDir,MenuType,FK_App,IsEnable,Flag)";
        //        sql += " VALUES('{0}','{1}','{2}',1,2,'{3}',1,'{4}')";
        //        sql = string.Format(sql, pMenuNo, SystemConfig.SysName, sysRootNo, SystemConfig.SysNo, "FlowFormTree" + SystemConfig.SysNo);
        //        DBAccess.RunSQL(sql);
        //        /* If there is no system , It is inserted into the system .*/
        //        sql = "INSERT INTO GPM_App(No,Name,AppModel,FK_AppSort,Url,RefMenuNo,MyFileName)";
        //        sql += " VALUES('{0}','{1}',0,'01','{2}','{3}','admin.gif')";

        //        sql = string.Format(sql, SystemConfig.SysNo, SystemConfig.SysName, url, pMenuNo);
        //        DBAccess.RunSQL(sql);
        //    }
        //    else
        //    {
        //        pMenuNo = dt.Rows[0]["RefMenuNo"].ToString();
        //    }
        //    #endregion

        //    try
        //    {
        //        sql = "SELECT * FROM GPM_Menu WHERE Flag='FlowFormTree" + this.No + "'";
        //        dt = DBAccess.RunSQLReturnTable(sql);
        //        if (dt.Rows.Count >= 1)
        //            DBAccess.RunSQL("DELETE FROM GPM_Menu WHERE Flag='FlowFormTree" + this.No + "' AND FK_App='" + SystemConfig.SysNo + "' ");
        //    }
        //    catch
        //    {
        //    }

        //    //  Organize data .
        //    //  Get his ParentNo
        //    string parentNo = "";//this.ParentNo
        //    if (!string.IsNullOrEmpty(this.ParentNo))
        //    {
        //        sql = "SELECT * FROM GPM_Menu WHERE Flag='FlowFormTree" + this.ParentNo + "'";
        //        dt = DBAccess.RunSQLReturnTable(sql);
        //        if (dt.Rows.Count >= 1)
        //        {
        //            pMenuNo = dt.Rows[0]["No"].ToString();
        //        }
        //    }

        //    string menuNo = DBAccess.GenerOID("BP.GPM.Menu").ToString();
        //    //  An insert .
        //    sql = "INSERT INTO GPM_Menu(No,Name,ParentNo,IsDir,MenuType,FK_App,IsEnable,Flag)";
        //    sql += " VALUES('{0}','{1}','{2}',{3},{4},'{5}',{6},'{7}')";
        //    sql = string.Format(sql, menuNo, this.Name, pMenuNo, 1, 3, SystemConfig.SysNo, 1, "FlowFormTree" + this.No);
        //    DBAccess.RunSQL(sql);
        //}
        //protected override bool beforeInsert()
        //{
        //    this.WritToGPM();
        //    return base.beforeInsert();
        //}


        //protected override bool beforeDelete()
        //{
        //    try
        //    {
        //        // Delete rights management 
        //        if (BP.WF.Glo.OSModel == OSModel.BPM)
        //            DBAccess.RunSQL("DELETE FROM GPM_Menu WHERE Flag='FlowFormTree" + this.No + "' AND FK_App='" + SystemConfig.SysNo + "'");
        //    }
        //    catch
        //    {
        //    }
        //    return base.beforeDelete();
        //}
        //protected override bool beforeUpdate()
        //{
        //    // Modify permissions management 
        //    if (BP.WF.Glo.OSModel == OSModel.BPM)
        //    {
        //        DBAccess.RunSQL("UPDATE  GPM_Menu SET Name='" + this.Name + "' WHERE Flag='FlowFormTree" + this.No + "' AND FK_App='" + SystemConfig.SysNo + "'");
        //    }
        //    return base.beforeUpdate();
        //}
    }
    /// <summary>
    ///  Process tree form 
    /// </summary>
    public class FlowFormTrees : EntitiesMultiTree
    {
        /// <summary>
        ///  Process tree form s
        /// </summary>
        public FlowFormTrees()
        {
        }
        /// <summary>
        ///  Process tree form 
        /// </summary>
        public FlowFormTrees(string flowNo)
        {
           int i= this.Retrieve(FlowFormTreeAttr.FK_Flow, flowNo);
           if (i == 0)
           {
               FlowFormTree tree = new FlowFormTree();
               tree.No = "100";
               tree.FK_Flow = flowNo;
               tree.Name = " Root directory ";
               tree.IsDir = false;
               tree.ParentNo = "0";
               tree.Insert();

               // Create a node .
               tree.DoCreateSubNode();
           }
        }
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new FlowFormTree();
            }

        }
    }
}
