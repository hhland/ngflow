using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BP.En;

namespace BP.Tools
{
    public class Entitis2Json
    {
        private volatile static Entitis2Json _instance = null;
        private Entitis2Json() { }

        public static Entitis2Json Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Entitis2Json();
                return _instance;
            }
        }

        /// <summary>
        ///  The entity class into json List Format data 
        /// </summary>
        /// <param name="ens"> Entity collections </param>
        /// <returns></returns>
        public static string ConvertEntities2ListJson(Entities ens)
        {
            return Instance.TranslateEntitiesToListJson(ens, null);
        }

        /// <summary>
        ///  The entity class into json List Format data 
        /// </summary>
        /// <param name="ens"> Entity collections </param>
        /// <param name="hidenKeys"> Need to hide columns ,如:@No@Name</param>
        /// <returns></returns>
        public static string ConvertEntities2ListJson(Entities ens, string hidenKeys)
        {
            return Instance.TranslateEntitiesToListJson(ens, hidenKeys);
        }

        /// <summary>
        /// 将Entitis Converted into tree json
        /// </summary>
        /// <param name="ens"> Entity collections </param>
        /// <param name="rootNo"> Root node number </param>
        /// <returns></returns>
        public static string ConvertEntitis2GenerTree(Entities ens, string rootNo)
        {
            return Instance.TansEntitiesToGenerTree(ens, rootNo);
        }

        /// <summary>
        ///  The entity class into json Format data 
        /// </summary>
        /// <param name="ens"> Entity collections </param>
        /// <returns></returns>
        public static string ConvertEntitis2GridJsonOnlyData(Entities ens)
        {
            return Instance.TranslateEntitiesToGridJsonOnlyData(ens, 0, null);
        }
        /// <summary>
        ///  The entity class into json Format data for pagination 
        /// </summary>
        /// <param name="ens"> Entity collections </param>
        /// <param name="totalRows"> Total number of rows </param>
        /// <returns></returns>
        public static string ConvertEntitis2GridJsonOnlyData(Entities ens, int totalRows)
        {
            return Instance.TranslateEntitiesToGridJsonOnlyData(ens, totalRows, null);
        }
        /// <summary>
        ///  The entity class into json Format data 
        /// </summary>
        /// <param name="ens"> Entity collections </param>
        /// <param name="hidenKeys"> Need to hide columns ,如:@No@Name</param>
        /// <returns></returns>
        public static string ConvertEntitis2GridJsonOnlyData(Entities ens, string hidenKeys)
        {
            return Instance.TranslateEntitiesToGridJsonOnlyData(ens, 0, hidenKeys);
        }
        /// <summary>
        ///  The entity class into json Format data for pagination 
        /// </summary>
        /// <param name="ens"> Entity collections </param>
        /// <param name="totalRows"> Total number of rows </param>
        /// <param name="hidenKeys"> Need to hide columns ,如:@No@Name</param>
        /// <returns></returns>
        public static string ConvertEntitis2GridJsonOnlyData(Entities ens, int totalRows, string hidenKeys)
        {
            return Instance.TranslateEntitiesToGridJsonOnlyData(ens, totalRows, hidenKeys);
        }
        /// <summary>
        ///  The entity collection class into json Format   Contains the column names and data 
        /// </summary>
        /// <param name="ens"> Entity collections </param>
        /// <returns>Json Format data </returns>
        public static string ConvertEntitis2GridJsonAndData(Entities ens)
        {
            return Instance.TranslateEntitiesToGridJsonColAndData(ens, 0, null);
        }
        /// <summary>
        ///  The entity collection class into json Format   Contains the column names and data 
        /// </summary>
        /// <param name="ens"> Entity collections </param>
        /// <param name="totalRows"> Total number of rows </param>
        /// <returns>Json Format data </returns>
        public static string ConvertEntitis2GridJsonAndData(Entities ens, int totalRows)
        {
            return Instance.TranslateEntitiesToGridJsonColAndData(ens, totalRows, null);
        }
        /// <summary>
        ///  The entity collection class into json Format   Contains the column names and data 
        /// </summary>
        /// <param name="ens"> Entity collections </param>
        /// <param name="hidenKeys"> Need to hide columns ,如:@No@Name</param>
        /// <returns></returns>
        public static string ConvertEntitis2GridJsonAndData(Entities ens, string hidenKeys)
        {
            return Instance.TranslateEntitiesToGridJsonColAndData(ens, 0, hidenKeys);
        }
        /// <summary>
        ///  The entity collection class into json Format   Contains the column names and data 
        /// </summary>
        /// <param name="ens"> Entity collections </param>
        /// <param name="totalRows"> Total number of rows </param>
        /// <param name="hidenKeys"> Need to hide columns ,如:@No@Name</param>
        /// <returns></returns>
        public static string ConvertEntitis2GridJsonAndData(Entities ens, int totalRows, string hidenKeys)
        {
            return Instance.TranslateEntitiesToGridJsonColAndData(ens, totalRows, hidenKeys);
        }

        /// <summary>
        ///  The entity class into json Format List
        /// </summary>
        /// <param name="ens"></param>
        /// <param name="hidenKeys"> Hidden Fields </param>
        /// <returns></returns>
        public string TranslateEntitiesToListJson(BP.En.Entities ens, string hidenKeys)
        {
            Attrs attrs = ens.GetNewEntity.EnMap.Attrs;
            StringBuilder append = new StringBuilder();
            append.Append("[");

            foreach (Entity en in ens)
            {
                append.Append("{");
                foreach (Attr attr in attrs)
                {
                    if (!string.IsNullOrEmpty(hidenKeys) && hidenKeys.Contains("@" + attr.Key))
                        continue;

                    string strValue = en.GetValStrByKey(attr.Key);
                    if (!string.IsNullOrEmpty(strValue) && strValue.LastIndexOf("\\") > -1)
                    {
                        strValue = strValue.Substring(0, strValue.LastIndexOf("\\"));
                    }
                    append.Append(attr.Key + ":'" + strValue + "',");
                }
                append = append.Remove(append.Length - 1, 1);
                append.Append("},");
            }
            if (append.Length > 1)
                append = append.Remove(append.Length - 1, 1);
            append.Append("]");
            return ReplaceIllgalChart(append.ToString());
        }

        /// <summary>
        ///  The entity class into json Format 
        /// </summary>
        /// <param name="ens"></param>
        /// <param name="hidenKeys"></param>
        /// <returns></returns>
        public string TranslateEntitiesToGridJsonOnlyData(BP.En.Entities ens, int totalRows, string hidenKeys)
        {
            Attrs attrs = ens.GetNewEntity.EnMap.Attrs;
            StringBuilder append = new StringBuilder();
            append.Append("{rows:[");

            foreach (Entity en in ens)
            {
                append.Append("{");
                foreach (Attr attr in attrs)
                {
                    if (!string.IsNullOrEmpty(hidenKeys) && hidenKeys.Contains("@" + attr.Key))
                        continue;

                    string strValue = en.GetValStrByKey(attr.Key);
                    if (!string.IsNullOrEmpty(strValue) && strValue.LastIndexOf("\\") > -1)
                    {
                        strValue = strValue.Substring(0, strValue.LastIndexOf("\\"));
                    }
                    append.Append(attr.Key + ":'" + strValue + "',");
                }
                append = append.Remove(append.Length - 1, 1);
                append.Append("},");
            }
            // Longer than {rows:[ Only interception 
            if (append.Length > 7)
                append = append.Remove(append.Length - 1, 1);


            if (totalRows == 0)
            {
                append.Append("],total:");
                append.Append(ens != null ? ens.Count : 0);
            }
            else
            {
                append.Append("],total:" + totalRows);
            }
            append.Append("}");
            return ReplaceIllgalChart(append.ToString());
        }

        /// <summary>
        ///  The entity class into json Format   Contains the column names and data 
        /// </summary>
        /// <param name="ens"></param>
        /// <param name="hidenKeys"></param>
        /// <returns></returns>
        public string TranslateEntitiesToGridJsonColAndData(Entities ens, int totalRows, string hidenKeys)
        {
            Attrs attrs = ens.GetNewEntity.EnMap.Attrs;
            StringBuilder append = new StringBuilder();
            append.Append("{");
            // Finishing the column name 
            append.Append("columns:[");
            foreach (Attr attr in attrs)
            {
                if (attr.UIVisible == false)
                    continue;
                if (!string.IsNullOrEmpty(hidenKeys) && hidenKeys.Contains("@" + attr.Key))
                    continue;
                if (attr.IsRefAttr || attr.IsFK || attr.IsEnum)
                {
                    append.Append("{");
                    append.Append(string.Format("field:'{0}',title:'{1}',width:{2},sortable:true", attr.Key + "Text", attr.Desc, attr.UIWidth * 2));
                    append.Append("},");
                    continue;
                }
                append.Append("{");
                append.Append(string.Format("field:'{0}',title:'{1}',width:{2},sortable:true", attr.Key, attr.Desc, attr.UIWidth * 2));
                append.Append("},");
            }
            if (append.Length > 10)
                append = append.Remove(append.Length - 1, 1);
            append.Append("]");

            // Organize data 
            bool bHaveData = false;
            append.Append(",data:{rows:[");
            foreach (Entity en in ens)
            {
                bHaveData = true;
                append.Append("{");
                foreach (Attr attr in attrs)
                {
                    //if (attr.IsRefAttr || attr.UIVisible == false)
                    //    continue;

                    if (attr.IsRefAttr || attr.IsFK || attr.IsEnum)
                    {
                        append.Append(attr.Key + "Text:'" + en.GetValRefTextByKey(attr.Key) + "',");
                        continue;
                    }
                    append.Append(attr.Key + ":'" + en.GetValStrByKey(attr.Key) + "',");
                }
                append = append.Remove(append.Length - 1, 1);
                append.Append("},");
            }
            if (append.Length > 11 && bHaveData)
                append = append.Remove(append.Length - 1, 1);

            append.Append("],total:" + totalRows + "}");
            append.Append("}");

            return ReplaceIllgalChart(append.ToString());
        }

        /// <summary>
        ///  The entity into a tree 
        /// </summary>
        /// <param name="ens"></param>
        /// <param name="rootNo"></param>
        StringBuilder appendMenus = new StringBuilder();
        StringBuilder appendMenuSb = new StringBuilder();
        public string TansEntitiesToGenerTree(Entities ens, string rootNo)
        {
            EntityTree root = ens.GetEntityByKey(EntityTreeAttr.ParentNo, rootNo) as EntityTree;
            if (root == null)
                throw new Exception("@ Not found rootNo=" + rootNo + "的entity.");
            appendMenus.Append("[{");
            appendMenus.Append("'id':'" + rootNo + "'");
            appendMenus.Append(",'text':'" + root.Name + "'");

            //  Increase its children .
            appendMenus.Append(",'children':");
            AddChildren(root, ens);
            appendMenus.Append(appendMenuSb);
            appendMenus.Append("}]");

            return ReplaceIllgalChart(appendMenus.ToString());
        }

        public void AddChildren(EntityTree parentEn, Entities ens)
        {
            appendMenus.Append(appendMenuSb);
            appendMenuSb.Clear();

            appendMenuSb.Append("[");
            foreach (EntityTree item in ens)
            {
                if (item.ParentNo != parentEn.No)
                    continue;

                appendMenuSb.Append("{'id':'" + item.No + "','text':'" + item.Name + "','state':'closed'");
                EntityTree treeNode = item as EntityTree;
                //  Increase its children .
                appendMenuSb.Append(",'children':");
                AddChildren(item, ens);
                appendMenuSb.Append("},");
            }
            if (appendMenuSb.Length > 1)
                appendMenuSb = appendMenuSb.Remove(appendMenuSb.Length - 1, 1);
            appendMenuSb.Append("]");
            appendMenus.Append(appendMenuSb);
            appendMenuSb.Clear();
        }
        /// <summary>
        ///  Remove the special characters 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string ReplaceIllgalChart(string s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0, j = s.Length; i < j; i++)
            {

                char c = s[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '/':
                        sb.Append("\\/");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }
    }
}

namespace BP.DA
{
    public class DataTableConvertJson
    {

        #region dataTable Convert Json Format 
        /// <summary>  
        /// dataTable Convert Json Format   
        /// </summary>  
        /// <param name="dt"></param>  
        /// <returns></returns>  
        public static string DataTable2Json(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            if (jsonBuilder.Length > 1)
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");

            return jsonBuilder.ToString();
        }
        /// <summary>
        ///   dataTable Convert Json Format   秦2014年06月23日 17:11
        /// </summary>
        /// <param name="dt">表</param>
        /// <param name="totalRows"> Total number of rows </param>
        /// <returns></returns>
        public static string DataTable2Json(DataTable dt, int totalRows)
        {
            StringBuilder jsonBuilder = new StringBuilder();

            jsonBuilder.Append("{rows:[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append(":");
                    jsonBuilder.Append("'" + dt.Rows[i][j].ToString() + "'");
                    jsonBuilder.Append(",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            // When the data does not exist 
            if (jsonBuilder.Length > 7)
            {
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            }
            //jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            if (totalRows == 0)
            {
                jsonBuilder.Append("],total:0");
            }
            else
            {
                jsonBuilder.Append("],total:" + totalRows);
            }
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }

        #endregion dataTable Convert Json Format 

        #region DataSet Convert Json Format 
        /// <summary>  
        /// DataSet Convert Json Format   
        /// </summary>  
        /// <param name="ds">DataSet</param> 
        /// <returns></returns>  
        public static string Dataset2Json(DataSet ds)
        {
            StringBuilder json = new StringBuilder();

            foreach (DataTable dt in ds.Tables)
            {
                json.Append("{\"");
                json.Append(dt.TableName);
                json.Append("\":");
                json.Append(DataTable2Json(dt));
                json.Append("}");
            } return json.ToString();
        }
        #endregion

        /// <summary>
        /// Msdn
        /// </summary>
        /// <param name="jsonName"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToJson(string jsonName, DataTable dt)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("{\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":\"" + dt.Rows[i][j].ToString() + "\"");
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }
        /// <summary>
        ///  According to DataTable Generate Json Tree 
        /// </summary>
        /// <param name="tabel"> Data Sources </param>
        /// <param name="idCol">ID列</param>
        /// <param name="txtCol">Text列</param>
        /// <param name="rela"> Relationship field </param>
        /// <param name="pId">父ID</param>
        ///<returns>easyui tree json Format </returns>
        public static string TransDataTable2TreeJson(DataTable tabel, string idCol, string txtCol, string rela, object pId)
        {
            treeResult = new StringBuilder();
            treesb = new StringBuilder();
            return GetTreeJsonByTable(tabel, idCol, txtCol, rela, pId);
        }

        /// <summary>
        ///  According to DataTable Generate Json Tree 
        /// </summary>
        /// <param name="tabel"> Data Sources </param>
        /// <param name="idCol">ID列</param>
        /// <param name="txtCol">Text列</param>
        /// <param name="rela"> Relationship field </param>
        /// <param name="pId">父ID</param>
        ///<returns>easyui tree json Format </returns>
        static StringBuilder treeResult = new StringBuilder();
        static StringBuilder treesb = new StringBuilder();
        private static string GetTreeJsonByTable(DataTable tabel, string idCol, string txtCol, string rela, object pId)
        {
            string treeJson = string.Empty;
            string treeState = "close";
            treeResult.Append(treesb.ToString());

            treesb.Clear();
            if (treeResult.Length == 0)
            {
                treeState = "open";
            }
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
                        treesb.Append("{\"id\":\"" + row[idCol]
                                + "\",\"text\":\"" + row[txtCol]
                                + "\",\"state\":\"" + treeState + "\"");


                        if (tabel.Select(string.Format("{0}='{1}'", rela, row[idCol])).Length > 0)
                        {
                            treesb.Append(",\"children\":");
                            GetTreeJsonByTable(tabel, idCol, txtCol, rela, row[idCol]);
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