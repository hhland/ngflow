using System;
using System.Net;
using System.Net.Mail;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls; 
using System.IO;
using System.Text; 
using BP.En;
using BP.DA;
using BP.Sys;
using BP.Web;
using System.Text.RegularExpressions;
using BP.Port;
 
namespace BP.En
{
	/// <summary>
	///   With regard to the Entity Expand , The method .
	/// </summary>
    public class GloEntity
    {
        #region  Used ddl  The method .
        public static string GetTextByValue(Entities ens, string no, string isNullAsVal)
        {
            try
            { 
                return GetTextByValue(ens, no);
            }
            catch
            {
                return isNullAsVal;
            }
        }
        public static string GetTextByValue(Entities ens, string no)
        {
            foreach (Entity en in ens)
            {
                if (en.GetValStringByKey("No") == no)
                    return en.GetValStringByKey("Name");
            }
            if (ens.Count == 0)
                throw new Exception("@ There is no solid data collection .");
            else
                throw new Exception("@ Not found No=" + no + " Entity inside ");
        }
        #endregion

        public static string GetEnFilesUrl(Entity en)
        {
            string str = null;
            SysFileManagers ens = en.HisSysFileManagers;

            string path = BP.Sys.Glo.Request.ApplicationPath;
            foreach (SysFileManager file in ens)
            {
                str += "[<a href='" + path + file.MyFilePath + "' target='f" + file.OID + "' >" + file.MyFileName + "</a>]";
            }
            return str;
        }

        #region  With regard to the entity  Treatment 

        #region  Change dataset
        /// <summary>
        ///  The specified ens  Converted to  dataset
        /// </summary>
        /// <param name="spen"> Specified ens</param>
        /// <returns> Returns relationship dataset</returns>
        public static DataSet ToDataSet(Entities spens)
        {

            DataSet ds = new DataSet(spens.ToString());

            /*  The main table join DataSet */
            Entity en = spens.GetNewEntity;
            DataTable dt = new DataTable();
            if (spens.Count == 0)
            {
                QueryObject qo = new QueryObject(spens);
                dt = qo.DoQueryToTable();
            }
            else
            {
                dt = spens.ToDataTableField();
            }
            dt.TableName = en.EnDesc; // Sets the name of the main table .
            dt.RowChanged += new DataRowChangeEventHandler(dt_RowChanged);

            //dt.RowChanged+=new DataRowChangeEventHandler(dt_RowChanged);

            ds.Tables.Add(DealBoolTypeInDataTable(en, dt));


            foreach (EnDtl ed in en.EnMap.DtlsAll)
            {
                /*  Details cycle master table , Edited relationships and put them in  DataSet  Inside .*/
                Entities edens = ed.Ens;
                Entity eden = edens.GetNewEntity;
                DataTable edtable = edens.RetrieveAllToTable();
                edtable.TableName = eden.EnDesc;
                ds.Tables.Add(DealBoolTypeInDataTable(eden, edtable));

                DataRelation r1 = new DataRelation(ed.Desc,
                    ds.Tables[dt.TableName].Columns[en.PK],
                    ds.Tables[edtable.TableName].Columns[ed.RefKey]);
                ds.Relations.Add(r1);


                //	int i = 0 ;

                foreach (EnDtl ed1 in eden.EnMap.DtlsAll)
                {
                    /*  Detailed breakdown of the main table .*/
                    Entities edlens1 = ed1.Ens;
                    Entity edlen1 = edlens1.GetNewEntity;

                    DataTable edlensTable1 = edlens1.RetrieveAllToTable();
                    edlensTable1.TableName = edlen1.EnDesc;
                    //edlensTable1.TableName =ed1.Desc ;


                    ds.Tables.Add(DealBoolTypeInDataTable(edlen1, edlensTable1));

                    DataRelation r2 = new DataRelation(ed1.Desc,
                        ds.Tables[edtable.TableName].Columns[eden.PK],
                        ds.Tables[edlensTable1.TableName].Columns[ed1.RefKey]);
                    ds.Relations.Add(r2);
                }

            }


            return ds;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="en"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static DataTable DealBoolTypeInDataTable(Entity en, DataTable dt)
        {

            foreach (Attr attr in en.EnMap.Attrs)
            {
                if (attr.MyDataType == DataType.AppBoolean)
                {
                    DataColumn col = new DataColumn();
                    col.ColumnName = "tmp" + attr.Key;
                    col.DataType = typeof(bool);
                    dt.Columns.Add(col);
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr[attr.Key].ToString() == "1")
                            dr["tmp" + attr.Key] = true;
                        else
                            dr["tmp" + attr.Key] = false;
                    }
                    dt.Columns.Remove(attr.Key);
                    dt.Columns["tmp" + attr.Key].ColumnName = attr.Key;
                    continue;
                }
                if (attr.MyDataType == DataType.AppDateTime || attr.MyDataType == DataType.AppDate)
                {
                    DataColumn col = new DataColumn();
                    col.ColumnName = "tmp" + attr.Key;
                    col.DataType = typeof(DateTime);
                    dt.Columns.Add(col);
                    foreach (DataRow dr in dt.Rows)
                    {
                        try
                        {
                            dr["tmp" + attr.Key] = DateTime.Parse(dr[attr.Key].ToString());
                        }
                        catch
                        {
                            if (attr.DefaultVal.ToString() == "")
                                dr["tmp" + attr.Key] = DateTime.Now;
                            else
                                dr["tmp" + attr.Key] = DateTime.Parse(attr.DefaultVal.ToString());

                        }

                    }
                    dt.Columns.Remove(attr.Key);
                    dt.Columns["tmp" + attr.Key].ColumnName = attr.Key;
                    continue;
                }
            }
            return dt;
        }
        /// <summary>
        /// DataRowChangeEventArgs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void dt_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            throw new Exception(sender.ToString() + "  rows change ." + e.Row.ToString());
        }

        #endregion

        /// <summary>
        ///  The property information ,”Îvlaue  Converted to Table
        /// </summary>
        /// <param name="en"> To convert entity</param>
        /// <param name="editStyle"> Editing Style </param>
        /// <returns>datatable</returns>
        public static DataTable ToTable(Entity en, int editStyle)
        {
            if (editStyle == 0)
                return GloEntity.ToTable0(en);
            else
                return GloEntity.ToTable1(en);
        }
        /// <summary>
        ///  Users style 0
        /// </summary>
        /// <returns></returns>
        private static DataTable ToTable0(Entity en)
        {
            string nameOfEnterInfo = en.EnDesc;
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn(" Enter the project ", typeof(string)));
            dt.Columns.Add(new DataColumn(nameOfEnterInfo, typeof(string)));
            dt.Columns.Add(new DataColumn(" Enter the required information ", typeof(string)));

            foreach (Attr attr in en.EnMap.Attrs)
            {
                DataRow dr = dt.NewRow();
                dr[" Enter the project "] = attr.Desc;
                dr[nameOfEnterInfo] = en.GetValByKey(attr.Key);
                dr[" Enter the required information "] = attr.EnterDesc;
                dt.Rows.Add(dr);
            }
            //  If the entity is required attachments .
            if (en.EnMap.AdjunctType != AdjunctType.None)
            {
                //  Join accessories .
                DataRow dr1 = dt.NewRow();
                dr1[" Enter the project "] = " Accessory ";
                dr1[nameOfEnterInfo] = "";
                dr1[" Enter the required information "] = " Edit attachments ";
                dt.Rows.Add(dr1);
            }
            //  Details 
            foreach (EnDtl dtl in en.EnMap.Dtls)
            {
                DataRow dr = dt.NewRow();
                dr[" Enter the project "] = dtl.Desc;
                dr[nameOfEnterInfo] = "EnsName_" + dtl.Ens.ToString() + "_RefKey_" + dtl.RefKey;
                dr[" Enter the required information "] = " Please enter the Edit Details ";
                dt.Rows.Add(dr);
            }
            foreach (AttrOfOneVSM attr in en.EnMap.AttrsOfOneVSM)
            {
                DataRow dr = dt.NewRow();
                dr[" Enter the project "] = attr.Desc;
                dr[nameOfEnterInfo] = "OneVSM" + attr.EnsOfMM.ToString();
                dr[" Enter the required information "] = " Please enter edit multiple choice ";
                dt.Rows.Add(dr);
            }
            return dt;

        }
        /// <summary>
        ///  Users style 1
        /// </summary>
        /// <returns></returns>
        private static DataTable ToTable1(Entity en)
        {

            string col1 = " Field name 1";
            string col2 = " Content 1";
            string col3 = " Field name 2";
            string col4 = " Content 2";

            //string enterNote=null;
            //			if (this.EnMap.Dtls.Count==0 || this.EnMap.AttrsOfOneVSM.Count==0)
            //				enterNote=" Content 1";
            //			else
            //				enterNote=" Saved to edit the relevant information ";


            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn(col1, typeof(string)));
            dt.Columns.Add(new DataColumn(col2, typeof(string)));
            dt.Columns.Add(new DataColumn(col3, typeof(string)));
            dt.Columns.Add(new DataColumn(col4, typeof(string)));


            for (int i = 0; i < en.EnMap.HisPhysicsAttrs.Count; i++)
            {
                DataRow dr = dt.NewRow();
                Attr attr = en.EnMap.HisPhysicsAttrs[i];
                dr[col1] = attr.Desc;
                dr[col2] = en.GetValByKey(attr.Key);

                i++;
                if (i == en.EnMap.HisPhysicsAttrs.Count)
                {
                    dt.Rows.Add(dr);
                    break;
                }
                attr = en.EnMap.HisPhysicsAttrs[i];
                dr[col3] = attr.Desc;
                dr[col4] = en.GetValByKey(attr.Key);
                dt.Rows.Add(dr);
            }


            //  If the entity is required attachments .
            if (en.EnMap.AdjunctType != AdjunctType.None)
            {
                //  Join accessories .
                DataRow dr1 = dt.NewRow();
                dr1[col1] = " Accessory ";
                dr1[col2] = " Edit attachments ";
                //dr[" Enter the project 2"]=" Additional information ";

                dt.Rows.Add(dr1);
            }
            //  Details 
            foreach (EnDtl dtl in en.EnMap.Dtls)
            {
                DataRow dr = dt.NewRow();
                dr[col1] = dtl.Desc;
                dr[col2] = "EnsName_" + dtl.Ens.ToString() + "_RefKey_" + dtl.RefKey;
                //dr[" Enter the project 2"]=" Detailed information ";
                dt.Rows.Add(dr);
            }
            //  Many relationship 
            foreach (AttrOfOneVSM attr in en.EnMap.AttrsOfOneVSM)
            {
                DataRow dr = dt.NewRow();
                dr[col1] = attr.Desc;
                dr[col2] = "OneVSM" + attr.EnsOfMM.ToString();
                //dr[" Enter the project 2"]=" Multiple choice ";
                dt.Rows.Add(dr);
            }
            return dt;
        }
        #endregion

        #region ’≈
        /// <summary>
        ///  Through a set of , A key, A division symbol , Get this property substring .
        /// </summary>
        /// <param name="key"></param>
        /// <param name="listspt"></param>
        /// <returns></returns>
        public static string GetEnsString(Entities ens, string key, string listspt)
        {
            string str = "";
            foreach (Entity en in ens)
            {
                str += en.GetValByKey(key) + listspt;
            }
            return str;
        }
        /// <summary>
        ///  Through a set of , A division symbol , Get this property substring .
        /// </summary>		
        /// <param name="listspt"></param>
        /// <returns></returns>
        public static string GetEnsString(Entities ens, string listspt)
        {
            return GetEnsString(ens, ens.GetNewEntity.PK, listspt);
        }
        /// <summary>
        ///  Through a collection to get this property substring .
        /// </summary>		
        /// <param name="listspt"></param>
        /// <returns></returns>
        public static string GetEnsString(Entities ens)
        {
            return GetEnsString(ens, ens.GetNewEntity.PK, ";");
        }
        #endregion
    }
}
