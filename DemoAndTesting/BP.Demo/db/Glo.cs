using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Web;
using BP.WF.Port;
using BP.Web;
using BP.WF;
using BP.DA;
using System.Web.UI.WebControls;
using System.Collections;
using BP.Port;
using BP.En;
using BP.Sys;

namespace BP.Demo
{
    public class Glo
    {


        public static void DDL_SetSelectVal(DropDownList ddl, string val, string sql)
        {
            ddl.Items.Clear();
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                ddl.Items.Add(new ListItem(dr["Name"].ToString(), dr["No"].ToString()));
            }
            foreach (ListItem li in ddl.Items)
            {
                if (li.Value == val)
                {
                    li.Selected = true;
                    break;
                }
            }
        }

        public static void DDL_SetSelectVal2(DropDownList ddl, string val, string sql)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem(" Please select ", "-1"));
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                ddl.Items.Add(new ListItem(dr["Name"].ToString(), dr["No"].ToString()));
            }
            foreach (ListItem li in ddl.Items)
            {
                if (li.Value == val)
                {
                    li.Selected = true;
                    break;
                }
            }
        }

        public static void DDL_SetSelectVal(DropDownList ddl, string val)
        {
            foreach (ListItem li in ddl.Items)
            {
                li.Selected = false;
            }

            foreach (ListItem li in ddl.Items)
            {
                if (li.Value == val)
                {
                    li.Selected = true;
                    break;
                }
            }
        }

    }
}
