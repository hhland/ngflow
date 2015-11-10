using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.En;
using System.Text;
using System.Data;
using BP.WF;
using BP.DA;
using BP.Sys;
using BP.Web;

namespace CCFlow.WF.Comm
{
    public partial class HelperOfTBEUI : System.Web.UI.Page
    {
        #region
        public string getUTF8ToString(string param)
        {
            return HttpUtility.UrlDecode(Request[param], System.Text.Encoding.UTF8);
        }
        public int getTaps
        {
            get
            {
                try
                {
                    return int.Parse(this.Request.QueryString["getTaps"]);
                }
                catch
                {
                    return 100;
                }
            }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            string method = string.Empty;
            // The return value 
            string s_responsetext = string.Empty;
            if (string.IsNullOrEmpty(Request["method"]))
                return;

            method = Request["method"].ToString();
            switch (method)
            {
                case "getMyData":
                    s_responsetext = getMyData();
                    break;
                case "insertSameNodeMet":
                    s_responsetext = insertSameNodeMet();
                    break;
                case "insertSonNodeMet":
                    s_responsetext = insertSonNodeMet();
                    break;
                case "editNodeMet":
                    s_responsetext = editNodeMet();
                    break;
                case "delNodeMet":
                    s_responsetext = delNodeMet();
                    break;
                case "saveHistoryWordMet":
                    s_responsetext = saveHistoryWordMet();
                    break;
            }
            if (string.IsNullOrEmpty(s_responsetext))
                s_responsetext = "";
            // Assembly ajax String format , Return to the calling client 
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "text/html";
            Response.Expires = 0;
            Response.Write(s_responsetext);
            Response.End();
        }
        private string saveHistoryWordMet()
        {
            string saveHistoryWord = getUTF8ToString("saveHistoryWord");
            string[] hisWordS = saveHistoryWord.Split(',');
            string hasOrNot = string.Format("select * from Sys_DefVal where EnsDesc='{0}' AND FK_Emp='{0}'", WebUser.No);
            DataTable hasOrNotDt = DBAccess.RunSQLReturnTable(hasOrNot);

            string getHisParNo = string.Format("select No from Sys_DefVal where EnsDesc='{0}' AND FK_Emp='{0}'", WebUser.No);

            for (int i = 0; i < hisWordS.Count(); i++)
            {
                bool hasOrNotBool = true;
                for (int t = 0; t < hasOrNotDt.Rows.Count; t++)
                {
                    if (hisWordS[i] == hasOrNotDt.Rows[t]["Val"].ToString() || string.IsNullOrEmpty(hisWordS[i]))
                    {
                        hasOrNotBool = false;
                    }
                }
                if (hasOrNotBool)
                {
                    // Implementation of data retention 
                    DefVal Dv = new DefVal();
                    Dv.EnsDesc = WebUser.No;
                    Dv.ParentNo = DBAccess.RunSQLReturnTable(getHisParNo).Rows[0]["No"].ToString();
                    Dv.IsParent = "0";
                    Dv.FK_Emp = WebUser.No;// People currently logged 
                    Dv.Val = hisWordS[i];
                    Dv.Insert();
                    hasOrNotBool = false;
                }
            }

            return "";
        }
        private string insertSameNodeMet()
        {
            string setFk_Emp = null;
            if (getTaps == 1)// My vocabulary FK_Emp Who account for the currently logged on 
            {
                setFk_Emp = WebUser.No;
            }
            else
            {
                setFk_Emp = "";
            }
            string selectId = getUTF8ToString("selectId");
            string setText = getUTF8ToString("setText");
            string sql = string.Format("select ParentNo from Sys_DefVal where No='{0}'", selectId);
            string isParentNo = DBAccess.RunSQLReturnTable(sql).Rows[0]["ParentNo"].ToString();
            if (int.Parse(isParentNo) == 0)// Description of the currently selected node is the parent node ( The root )
            {
                DefVal Dv = new DefVal();
                Dv.FK_Emp = setFk_Emp;
                Dv.Val = setText;
                Dv.ParentNo = "0";
                Dv.IsParent = "1";// Increase parent siblings --- Assigned "1"
                Dv.Insert();
            }
            else
            {
                DefVal Dv = new DefVal();
                Dv.FK_Emp = setFk_Emp;
                Dv.Val = setText;
                Dv.ParentNo = isParentNo;
                Dv.IsParent = "0";// Child nodes increase siblings --- Assigned "0"
                Dv.Insert();
            }

            return "";
        }
        /// <summary>
        ///  All nodes can increase --------(旧) Only a parent node "0" The nodes can add child nodes 
        /// </summary>
        /// <returns></returns>
        private string insertSonNodeMet()
        {
            string setFk_Emp = null;
            if (getTaps == 1)// My vocabulary FK_Emp Who account for the currently logged on 
            {
                setFk_Emp = WebUser.No;
            }
            else
            {
                setFk_Emp = "";
            }
            string selectId = getUTF8ToString("selectId");
            string setText = getUTF8ToString("setText");
            string sql = string.Format("select ParentNo from Sys_DefVal where No='{0}'", selectId);
            string isParentNo = DBAccess.RunSQLReturnTable(sql).Rows[0]["ParentNo"].ToString();
            //if (isParentNo == 0)// Description of the currently selected node is the parent node ( The root )
            //{
            DefVal Dv = new DefVal();
            Dv.FK_Emp = setFk_Emp;
            Dv.Val = setText;
            Dv.ParentNo = selectId;
            Dv.IsParent = "0";// Increase child nodes child nodes --- Assigned "0"
            Dv.Insert();

            DefVal DvuUpFatNode = new DefVal();// Increase in child node after the selected node --- Currently selected node IsParent Attribute assignment for "1"
            DvuUpFatNode.Retrieve(DefValAttr.No, selectId);
            DvuUpFatNode.IsParent = "1";
            DvuUpFatNode.Update();
            //}

            return "";
        }
        /// <summary>
        ///  Edit Node --- All nodes can be edited 
        /// </summary>
        /// <returns></returns>
        private string editNodeMet()
        {
            string selectId = getUTF8ToString("selectId");
            string setText = getUTF8ToString("setText");

            DefVal Dv = new DefVal();
            Dv.Retrieve(DefValAttr.No, selectId);
            Dv.Val = setText;
            Dv.Update();

            return "";
        }
        /// <summary>
        ///  Delete Node -- If the parent node also has a child node -- Remove this parent ban 
        /// </summary>
        /// <returns></returns>
        private string delNodeMet()
        {
            string selectId = getUTF8ToString("selectId");
            string sql = string.Format("select ParentNo from Sys_DefVal where No='{0}'", selectId);
            string isParentNo = DBAccess.RunSQLReturnTable(sql).Rows[0]["ParentNo"].ToString();
            if (int.Parse(isParentNo) == 0)// Parent 
            {
                string canNotDelSql = "";
                if (getTaps == 1)// My vocabulary 
                {
                    canNotDelSql = string.Format("select No from Sys_DefVal where ParentNo='0' and EnsDesc ='' and FK_Emp='{0}'", WebUser.No);
                }
                else if (getTaps == 0)
                {
                    canNotDelSql = "select No from Sys_DefVal where ParentNo='0' and EnsDesc ='' and  FK_Emp=''";
                }

                if (DBAccess.RunSQLReturnCOUNT(canNotDelSql) == 1)// Finally, a parent node limit here can not be deleted , Avoid empty data being given bug
                {
                    return "";
                }
                DefVal Dv = new DefVal();
                Dv.Delete(DefValAttr.No, selectId);
            }
            else// Delete not limit a child node 
            {
                string upParent = string.Format("select ParentNo from Sys_DefVal where No='{0}'", selectId);
                string getParentNo = DBAccess.RunSQLReturnTable(upParent).Rows[0]["ParentNo"].ToString();
                string isFirstParent = string.Format("select ParentNo from Sys_DefVal where No='{0}'", getParentNo);

                DefVal Dv = new DefVal();
                Dv.Delete(DefValAttr.No, selectId);

                string hasSonOrNot = string.Format("select No from Sys_DefVal where ParentNo='{0}'", getParentNo);
                if (DBAccess.RunSQLReturnCOUNT(hasSonOrNot) == 0 && int.Parse(DBAccess.RunSQLReturnTable(isFirstParent).Rows[0]["ParentNo"].ToString()) != 0)
                {
                    DefVal DvUp = new DefVal();
                    DvUp.Retrieve(DefValAttr.No, getParentNo);
                    DvUp.IsParent = "0";
                    DvUp.Update();
                }
            }
            return "";
        }

        private string getMyData()
        {
            DefVal dv = new DefVal();
            dv.CheckPhysicsTable();
            // Initialize the database --- If it is empty , Adding default data 
            string MyhasNoDataSql = string.Format("SELECT * FROM Sys_DefVal where FK_Emp='{0}' AND ParentNo=0 AND EnsDesc != '{0}' ORDER BY No", WebUser.No);
            string PubhasNoDataSql = "SELECT * FROM Sys_DefVal where FK_Emp='' AND ParentNo=0";
            string historyHasNoDataSql = string.Format("SELECT * FROM Sys_DefVal where FK_Emp='{0}' AND EnsDesc= '{0}' ", WebUser.No);
            if (DBAccess.RunSQLReturnCOUNT(MyhasNoDataSql) == 0)// My vocabulary 
            {
                DefVal MyDv = new DefVal();
                MyDv.Val = " My vocabulary ";
                MyDv.FK_Emp = WebUser.No;
                MyDv.ParentNo = "0";
                MyDv.IsParent = "1";
                MyDv.Insert();
            }

            if (DBAccess.RunSQLReturnCOUNT(PubhasNoDataSql) == 0)// Global vocabulary 
            {
                DefVal PubDv = new DefVal();
                PubDv.Val = " Global vocabulary ";
                PubDv.FK_Emp = "";
                PubDv.ParentNo = "0";
                PubDv.IsParent = "1";
                PubDv.Insert();
            }

            if (DBAccess.RunSQLReturnCOUNT(historyHasNoDataSql) == 0)
            {
                string setTabNo = "hisW" + WebUser.No;
                DefVal HisDv = new DefVal();
                HisDv.EnsDesc = WebUser.No;
                HisDv.FK_Emp = WebUser.No;
                HisDv.Val = " History Glossary ";
                HisDv.ParentNo = "0";
                HisDv.IsParent = "1";
                HisDv.Insert();
            }
            string getMyDataSql = null;
            if (getTaps == 1)// My vocabulary FK_Emp Who account for the currently logged on 
            {
                getMyDataSql = string.Format("select No,Val,ParentNo,IsParent from Sys_DefVal where FK_Emp='{0}' and EnsDesc != '{0}' ORDER BY No", WebUser.No);
            }
            else if (getTaps == 0)
            {
                getMyDataSql = string.Format("select No,Val,ParentNo,IsParent from Sys_DefVal where FK_Emp ='{0}' and EnsDesc = '{0}' ORDER BY No", "");

            }
            else
            {
                getMyDataSql = string.Format("select  No,Val,ParentNo,IsParent from Sys_DefVal where FK_Emp='{0}' and EnsDesc = '{0}' " +
                                             " and ParentNo='0' union select top 15 No,Val,ParentNo " +
                                             ",IsParent from Sys_DefVal where FK_Emp='{0}' and EnsDesc = '{0}' ORDER BY No desc", WebUser.No);
            }
            DataTable dt_dept = DBAccess.RunSQLReturnTable(getMyDataSql);
            string s_responsetext = string.Empty;
            string s_checkded = string.Empty;

            s_responsetext = GetTreeJsonByTable(dt_dept, "No", "Val", "ParentNo", "0", "IsParent", s_checkded);

            return s_responsetext;
        }
        /// <summary>
        ///  According to DataTable Generate Json Tree 
        /// </summary>
        StringBuilder treeResult = new StringBuilder();
        StringBuilder treesb = new StringBuilder();
        public string GetTreeJsonByTable(DataTable tabel, string idCol, string txtCol, string rela, object pId, string IsParent, string CheckedString)
        {
            string treeJson = string.Empty;
            treeResult.Append(treesb.ToString());

            treesb.Clear();
            if (tabel.Rows.Count > 0)
            {
                treesb.Append("[");
                string filer = string.Empty;
                if (pId.ToString() == "")
                {
                    filer = string.Format("{0} is null", rela);
                }
                else
                {
                    filer = string.Format("{0}='{1}'", rela, pId);
                }
                DataRow[] rows = tabel.Select(filer);
                if (rows.Length > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        string deptNo = row[idCol].ToString();

                        if (treeResult.Length == 0)
                        {
                            treesb.Append("{\"id\":\"" + row[idCol]
                                + "\",\"text\":\"" + row[txtCol]
                                //+ "\",\"IsParent\":\"" + row[IsParent]
                                  + "\",\"attributes\":{\"IsParent\":\"" + row[IsParent] + "\"}"
                                   + ",\"checked\":" + CheckedString.Contains("," + row[idCol] + ",").ToString().ToLower() + ",\"state\":\"open\"");
                            //+ "\",\"checked\":" + CheckedString.Contains("," + row[idCol] + ",").ToString().ToLower() + ",\"state\":\"open\"");
                        }
                        else if (tabel.Select(string.Format("{0}='{1}'", rela, row[idCol])).Length > 0)
                        {
                            treesb.Append("{\"id\":\"" + row[idCol]
                                + "\",\"text\":\"" + row[txtCol]
                                //+ "\",\"IsParent\":\"" + row[IsParent]
                                   + "\",\"attributes\":{\"IsParent\":\"" + row[IsParent] + "\"}"
                                   + ",\"checked\":" + CheckedString.Contains("," + row[idCol] + ",").ToString().ToLower() + ",\"state\":\"open\"");
                            //+ "\",\"checked\":" + CheckedString.Contains("," + row[idCol] + ",").ToString().ToLower() + ",\"state\":\"open\"");
                        }
                        else
                        {
                            treesb.Append("{\"id\":\"" + row[idCol]
                                + "\",\"text\":\"" + row[txtCol]
                                //+ "\",\"IsParent\":\"" +row[IsParent]
                                 + "\",\"attributes\":{\"IsParent\":\"" + row[IsParent] + "\"}"
                                     + ",\"checked\":" + CheckedString.Contains("," + row[idCol] + ",").ToString().ToLower());
                            //+ "\",\"checked\":" + CheckedString.Contains("," + row[idCol] + ",").ToString().ToLower());
                        }


                        if (tabel.Select(string.Format("{0}='{1}'", rela, row[idCol])).Length > 0)
                        {
                            treesb.Append(",\"children\":");
                            GetTreeJsonByTable(tabel, idCol, txtCol, rela, row[idCol], IsParent, CheckedString);
                            treeResult.Append(treesb.ToString());
                            treesb.Clear();
                        }
                        treeResult.Append(treesb.ToString());
                        treesb.Clear();
                        treesb.Append("},");
                    }
                    treesb = treesb.Remove(treesb.Length - 1, 1);
                }
                treesb.Append("]");
                treeResult.Append(treesb.ToString());
                treeJson = treeResult.ToString();
                treesb.Clear();
            }
            return treeJson;
        }
    }
}