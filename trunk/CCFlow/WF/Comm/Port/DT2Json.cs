using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CCFlow.WF.Comm.Port
{
    public class DataTableTree2Json
    {
        private const String LEVEL_COLUMN_NAME = "XXXYYYTREELEVELCOL";
        private System.Data.DataTable _DT;
        private string _IdName;
        private string _ParentIdName;
        private string _ChildNodeName;
        private string _RootParent;
        public string QuoteMark = "\'"; // Could be replaced by single quotes  

        /// <summary>
        ///  If it is true, All fields are treated as strings , Both sides will be added QuoteMark.
        ///  If it is false, Will detect the field type and value type , If a number or Boolean , The sides without QuoteMark.
        /// </summary>
        public bool SetAllQuote = false;
        /// <summary>
        ///  Formatting mode 
        /// 0- Do not format 
        /// 1- Object formatting , Each object row 
        /// 2- Attribute Format , Each property line 
        /// </summary>
        public int FormatMode = 1;
        /// <summary>
        ///  Serialization method  0- Replace manual ,1-Microsoft.Jscript Serialization 
        /// </summary>
        public int EscapeMethod = 0;
        /// <summary>
        ///  Without Quote Fields List ,当AllQuote为true When work .
        /// </summary>
        public string[] NoQuoteCols = null;
        /// <summary>
        ///  Serialized Fields 
        /// </summary>
        public string[] EscapeCols = null;
        public DataTableTree2Json(System.Data.DataTable dt, string idName, string parentIdName, string childNodeName, string rootParent)
        {
            this._DT = dt;
            this._IdName = idName;
            this._ParentIdName = parentIdName;
            this._ChildNodeName = childNodeName;
            this._RootParent = rootParent;

            this.CheckInit();
            this.AddDegreeColumn();
        }
        private void CheckInit()
        {
            if (this._DT == null || String.IsNullOrEmpty(this._IdName) || String.IsNullOrEmpty(this._ParentIdName))
            {
                throw new ApplicationException(" Input variables can not be empty !");
            }
            if (String.IsNullOrEmpty(this._ChildNodeName))
                this._ChildNodeName = "Childs";
        }
        private void AddDegreeColumn()
        {
            this._DT.Columns.Add(LEVEL_COLUMN_NAME);
            string parentIdList = this._RootParent;
            for (int i = 0; i < 100; i++)
            {
                string con = null;
                if (String.IsNullOrEmpty(parentIdList))
                    con = String.Format("{0} is null", this._ParentIdName);
                else
                    con = String.Format("{0} in ({1})", this._ParentIdName, String.Format("'{0}'", parentIdList.Replace(",", "','")));
                DataRow[] drs = this._DT.Select(con);
                if (drs.Length == 0) break;
                parentIdList = String.Empty;
                foreach (DataRow dr in drs)
                {
                    dr[LEVEL_COLUMN_NAME] = i;
                    if (!String.IsNullOrEmpty(parentIdList)) parentIdList += ",";
                    parentIdList += Convert.ToString(dr[_IdName]);
                }
            }
        }
        /// <summary>
        ///  The only external interface 
        /// </summary>
        /// <param name="parentId"> As for Null时, Definition RootParent, As for Convert.DBNull时, The execution Null Value inquiry </param>
        /// <returns></returns>
        public string ToJsonStr(string parentId)
        {
            if (String.IsNullOrEmpty(parentId))
                parentId = this._RootParent;

            string jsonStr = this.GetJsonFromDataTable(parentId);
            if (this.FormatMode == 0)
                return String.Format("[{0}]", jsonStr);
            else
                return String.Format("[{0}{1}]", jsonStr, "\r\n");
        }
        private string Name(string name)
        {
            return String.Format("{0}{1}{0}", this.QuoteMark, name);
        }
        private string GetJsonFromDataTable(String parentId)
        {
            String jsonStr = String.Empty;
            string con = null;
            if (String.IsNullOrEmpty(parentId))
                con = String.Format("{0} is null", this._ParentIdName);
            else
                con = String.Format("{0} = '{1}'", this._ParentIdName, parentId);
            DataRow[] drs = this._DT.Select(con);
            foreach (DataRow dr in drs)
            {
                if (!String.IsNullOrEmpty(jsonStr)) jsonStr += ",";
                String drJson = this.GetJsonFromDataRow(dr);
                jsonStr += drJson;
            }
            return jsonStr;
        }
        private string RepeatString(String strToRepeat, int repeatCount)
        {
            string strTemp = String.Empty;
            for (int i = 0; i < repeatCount; i++) strTemp += strToRepeat;
            return strTemp;
        }
        private string GetJsonFromDataRow(DataRow drT)
        {
            string jsonStr = String.Empty;
            if (true)
            {
                for (int i = 0; i < this._DT.Columns.Count; i++)
                {
                    // Add their own logo , Filtered 
                    if (this._DT.Columns[i].ColumnName == LEVEL_COLUMN_NAME) continue;
                    // Normal field 
                    if (!String.IsNullOrEmpty(jsonStr)) jsonStr += ",";
                    if (this.FormatMode == 2)
                        if (this.FormatMode == 2)
                        {
                            jsonStr += "\r\n" + this.RepeatString("\t", Convert.ToInt32(drT[LEVEL_COLUMN_NAME]) + 1) + this.GetJsonFromKeyValue(this._DT.Columns[i].ColumnName, drT[i]);
                        }
                        else
                        {
                            jsonStr += this.GetJsonFromKeyValue(this._DT.Columns[i].ColumnName, drT[i]);
                        }
                }
            }
            if (true)
            {
                DataRow[] drs = this._DT.Select(String.Format("{0}='{1}'", this._ParentIdName, drT[this._IdName]));
                string childJson = String.Empty;
                foreach (DataRow dr in drs)
                {
                    if (!String.IsNullOrEmpty(childJson)) childJson += ",";
                    String drJson = this.GetJsonFromDataRow(dr);
                    childJson += drJson;
                }
                if (!String.IsNullOrEmpty(childJson))
                {
                    if (this.FormatMode == 0)
                        childJson = this.Name(this._ChildNodeName) + ":[" + childJson + "]";
                    else if (this.FormatMode == 1)
                        childJson = this.Name(this._ChildNodeName) + ":[" + childJson + "\r\n" + this.RepeatString("\t", Convert.ToInt32(drT[LEVEL_COLUMN_NAME]) + 1) + "]";
                    else
                        childJson = this.Name(this._ChildNodeName) + ":"
                            + "\r\n" + this.RepeatString("\t", Convert.ToInt32(drT[LEVEL_COLUMN_NAME]) + 1) + "["
                            + childJson
                            + "\r\n" + this.RepeatString("\t", Convert.ToInt32(drT[LEVEL_COLUMN_NAME]) + 1) + "]";
                }
                else
                {
                    childJson = this.Name(this._ChildNodeName) + ": null";
                }
                if (!String.IsNullOrEmpty(jsonStr) && !String.IsNullOrEmpty(childJson)) jsonStr += ",";
                // Formatting 
                if (this.FormatMode == 2)
                {
                    childJson = "\r\n" + this.RepeatString("\t", Convert.ToInt32(drT[LEVEL_COLUMN_NAME]) + 1) + childJson;
                }
                jsonStr += childJson;
            }
            if (this.FormatMode == 0)
                return "{" + jsonStr + "}";
            else if (this.FormatMode == 1)
            {
                return "\r\n" + this.RepeatString("\t", Convert.ToInt32(drT[LEVEL_COLUMN_NAME])) + "{" + jsonStr + "}";
            }
            else //if (this.FormatMode == 2)
                return "\r\n" + this.RepeatString("\t", Convert.ToInt32(drT[LEVEL_COLUMN_NAME])) + "{"
                    + jsonStr
                    + "\r\n" + this.RepeatString("\t", Convert.ToInt32(drT[LEVEL_COLUMN_NAME])) + "}";
        }
        private string GetJsonFromKeyValue(string key, object value)
        {
            return String.Format("{2}{0}{2}:{1}", key, this.GetJsonFromValue(key, value), this.QuoteMark);
        }
        private string GetJsonFromValue(string key, object value)
        {
            if (value == null || value == Convert.DBNull)
            {
                return "null";
            }
            else
            {
                return String.Format(this.GetQuoteFormat(key, Convert.ToString(value)), this.SerializeJsonValue(key, Convert.ToString(value)));
            }
            //else if (!this.SetAllQuote && (this.isNumber(key, value) || this.IsBool(key, value)))
            //{
            //    return String.Format("{0}", value);
            //}
            //else if (this.SetAllQuote)
            //{
            //    if (this.NoQuoteCols != null)
            //    {
            //        List<String> ls = new List<string>(NoQuoteCols);
            //        if (ls.Contains(key))
            //        {
            //            return this.SerializeJsonValue(key, String.Format("{0}", value));
            //        }
            //        else
            //        {
            //            return this.QuoteMark + this.SerializeJsonValue(key, String.Format("{0}", value)) + this.QuoteMark;
            //        }
            //    }
            //    else
            //    {
            //        return this.QuoteMark + this.SerializeJsonValue(key, String.Format("{0}", value)) + this.QuoteMark;
            //    }
            //}
            //else
            //{
            //    return this.QuoteMark + this.SerializeJsonValue(key, String.Format("{0}", value)) + this.QuoteMark;
            //}
        }
        private string GetQuoteFormat(string key, string value)
        {
            if (!this.SetAllQuote && (this.isNumber(key, value) || this.IsBool(key, value)))
            {
                return "{0}";
            }
            else if (this.SetAllQuote)
            {
                if (this.NoQuoteCols != null)
                {
                    List<String> ls = new List<string>(NoQuoteCols);
                    if (ls.Contains(key))
                    {
                        return "{0}";
                    }
                    else
                    {
                        return this.QuoteMark + "{0}" + this.QuoteMark;
                    }
                }
                else
                {
                    return this.QuoteMark + "{0}" + this.QuoteMark;
                }
            }
            else
            {
                return this.QuoteMark + "{0}" + this.QuoteMark;
            }
        }
        private string SerializeJsonValue(string key, string value)
        {
            if (this.EscapeMethod == 0)
                return this.SerializeJsonValue_Replace(value);
            else
                return this.SerializeJsonValue_MJ(value);
        }
        private string SerializeJsonValue_MJ(string value)
        {
            return Uri.EscapeDataString(value);
        }
        private string SerializeJsonValue_Replace(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            string temstr;
            temstr = value;
            temstr = temstr.Replace("{", "｛").Replace("}", "｝")
                .Replace(":", ":")
                .Replace(",", ",")
                .Replace("[", "【")
                .Replace("]", "】")
                .Replace(";", ";")
                .Replace("\n", "<br/>")
                .Replace("\r", "");

            temstr = temstr.Replace("\t", "   ");
            temstr = temstr.Replace("'", "\'");
            temstr = temstr.Replace(@"\", @"\\");
            temstr = temstr.Replace("\"", "\"\"");
            return temstr;
        }
        private bool isNumber(string key, object value)
        {
            // If the first digit value ( Superfluous ), Because the data type , If an error occurs for digital . If you use to do it with , Open 
            //double d = 0;
            //bool bln = double.TryParse(Convert.ToString(value), out d);
            //if (!bln) return false;
            // Again the data type to digital 
            bool b = false;
            switch (this._DT.Columns[key].DataType.FullName)
            {
                case "DbType.AnsiString": break;
                case "DbType.AnsiStringFixedLength": break;
                case "DbType.Binary": break;
                case "DbType.Boolean": break;
                case "DbType.Byte": b = true; break;
                case "DbType.Currency": b = true; break;
                case "DbType.Date": break;
                case "DbType.DateTime": break;
                case "DbType.DateTime2": break;
                case "DbType.DateTimeOffset": break;
                case "DbType.Decimal": b = true; break;
                case "DbType.Double": b = true; break;
                case "DbType.Guid": break;
                case "DbType.Int16": b = true; break;
                case "DbType.Int32": b = true; break;
                case "DbType.Int64": b = true; break;
                case "DbType.Object": break;
                case "DbType.SByte": b = true; break;
                case "DbType.Single": b = true; break;
                case "DbType.String": break;
                case "DbType.StringFixedLength": break;
                case "DbType.Time": break;
                case "DbType.UInt16": b = true; break;
                case "DbType.UInt32": b = true; break;
                case "DbType.UInt64": b = true; break;
                case "DbType.VarNumeric": b = true; break;
                case "DbType.Xml": break;
            }
            return b;
        }
        private bool IsBool(string key, object value)
        {
            bool bR = false;
            bool bln = bool.TryParse(Convert.ToString(value), out bR);
            if (!bln) return false;

            bool b = false;
            switch (this._DT.Columns[key].DataType.FullName)
            {
                case "DbType.AnsiString": break;
                case "DbType.AnsiStringFixedLength": break;
                case "DbType.Binary": break;
                case "DbType.Boolean": b = true; break;
                case "DbType.Byte": break;
                case "DbType.Currency": break;
                case "DbType.Date": break;
                case "DbType.DateTime": break;
                case "DbType.DateTime2": break;
                case "DbType.DateTimeOffset": break;
                case "DbType.Decimal": break;
                case "DbType.Double": break;
                case "DbType.Guid": break;
                case "DbType.Int16": break;
                case "DbType.Int32": break;
                case "DbType.Int64": break;
                case "DbType.Object": break;
                case "DbType.SByte": break;
                case "DbType.Single": break;
                case "DbType.String": break;
                case "DbType.StringFixedLength": break;
                case "DbType.Time": break;
                case "DbType.UInt16": break;
                case "DbType.UInt32": break;
                case "DbType.UInt64": break;
                case "DbType.VarNumeric": break;
                case "DbType.Xml": break;
            }
            return b;
        }
        private bool IsDateTime(string key, object value)
        {
            DateTime d = DateTime.Now;
            bool bln = DateTime.TryParse(Convert.ToString(value), out d);
            if (!bln) return false;

            bool b = false;
            switch (this._DT.Columns[key].DataType.FullName)
            {
                case "DbType.AnsiString": break;
                case "DbType.AnsiStringFixedLength": break;
                case "DbType.Binary": break;
                case "DbType.Boolean": break;
                case "DbType.Byte": break;
                case "DbType.Currency": break;
                case "DbType.Date": b = true; break;
                case "DbType.DateTime": b = true; break;
                case "DbType.DateTime2": b = true; break;
                case "DbType.DateTimeOffset": b = true; break;
                case "DbType.Decimal": break;
                case "DbType.Double": break;
                case "DbType.Guid": break;
                case "DbType.Int16": break;
                case "DbType.Int32": break;
                case "DbType.Int64": break;
                case "DbType.Object": break;
                case "DbType.SByte": break;
                case "DbType.Single": break;
                case "DbType.String": break;
                case "DbType.StringFixedLength": break;
                case "DbType.Time": b = true; break;
                case "DbType.UInt16": break;
                case "DbType.UInt32": break;
                case "DbType.UInt64": break;
                case "DbType.VarNumeric": break;
                case "DbType.Xml": break;
            }
            return b;
        }
    }
    public class DataTableLine2Json
    {
        private System.Data.DataTable _DT;
        public string QuoteMark = "\'"; // Could be replaced by single quotes  

        /// <summary>
        ///  If it is true, All fields are treated as strings , Both sides will be added QuoteMark.
        ///  If it is false, Will detect the field type and value type , If a number or Boolean , The sides without QuoteMark.
        /// </summary>
        public bool SetAllQuote = false;
        /// <summary>
        ///  Formatting mode 
        /// 0- Do not format 
        /// 1- Object formatting , Each object row 
        /// 2- Attribute Format , Each property line 
        /// </summary>
        public int FormatMode = 1;

        /// <summary>
        ///  Without Quote Fields List ,当AllQuote为true When work .
        /// </summary>
        public string[] NoQuoteCols = null;
        /// <summary>
        ///  Serialized Fields 
        /// </summary>
        public string[] EscapeCols = null;
        /// <summary>
        ///  Serialization method  0- Replace manual ,1-Microsoft.Jscript Serialization 
        /// </summary>
        public int EscapeMethod = 0;
        public DataTableLine2Json(System.Data.DataTable dt)
        {
            this._DT = dt;

            this.CheckInit();
        }
        private void CheckInit()
        {
            if (this._DT == null)
            {
                throw new ApplicationException(" Input variables can not be empty !");
            }
        }
        /// <summary>
        ///  The only external interface 
        /// </summary>
        /// <returns></returns>
        public string ToJsonStr(string con)
        {
            string jsonStr = this.GetJsonFromDataTable(con);
            if (this.FormatMode == 0)
                return String.Format("[{0}]", jsonStr);
            else
                return String.Format("[{0}{1}]", jsonStr, "\r\n");
        }
        private string Name(string name)
        {
            return String.Format("{0}{1}{0}", this.QuoteMark, name);
        }
        private string GetJsonFromDataTable(string con)
        {
            String jsonStr = String.Empty;
            DataRow[] drs = this._DT.Select(con);
            foreach (DataRow dr in drs)
            {
                if (!String.IsNullOrEmpty(jsonStr)) jsonStr += ",";
                String drJson = this.GetJsonFromDataRow(dr);
                jsonStr += drJson;
            }
            return jsonStr;
        }
        private string RepeatString(String strToRepeat, int repeatCount)
        {
            string strTemp = String.Empty;
            for (int i = 0; i < repeatCount; i++) strTemp += strToRepeat;
            return strTemp;
        }
        private string GetJsonFromDataRow(DataRow drT)
        {
            string jsonStr = String.Empty;
            if (true)
            {
                for (int i = 0; i < this._DT.Columns.Count; i++)
                {
                    // Normal field 
                    if (!String.IsNullOrEmpty(jsonStr)) jsonStr += ",";
                    if (this.FormatMode == 2)
                        if (this.FormatMode == 2)
                        {
                            jsonStr += "\r\n" + this.RepeatString("\t", 1) + this.GetJsonFromKeyValue(this._DT.Columns[i].ColumnName, drT[i]);
                        }
                        else
                        {
                            jsonStr += this.GetJsonFromKeyValue(this._DT.Columns[i].ColumnName, drT[i]);
                        }
                }
            }
            if (this.FormatMode == 0)
                return "{" + jsonStr + "}";
            else if (this.FormatMode == 1)
            {
                return "\r\n" + this.RepeatString("\t", 1) + "{" + jsonStr + "}";
            }
            else //if (this.FormatMode == 2)
                return "\r\n" + this.RepeatString("\t", 1) + "{"
                    + jsonStr
                    + "\r\n" + this.RepeatString("\t", 1) + "}";
        }
        private string GetJsonFromKeyValue(string key, object value)
        {
            return String.Format("{2}{0}{2}:{1}", key, this.GetJsonFromValue(key, value), this.QuoteMark);
        }
        private string GetJsonFromValue(string key, object value)
        {
            if (value == null || value == Convert.DBNull)
            {
                return "null";
            }
            else
            {
                return String.Format(this.GetQuoteFormat(key, Convert.ToString(value)), this.SerializeJsonValue(key, Convert.ToString(value)));
            }
            //else if (!this.SetAllQuote && (this.isNumber(key, value) || this.IsBool(key, value)))
            //{
            //    return String.Format("{0}", value);
            //}
            //else if (this.SetAllQuote)
            //{
            //    if (this.NoQuoteCols != null)
            //    {
            //        List<String> ls = new List<string>(NoQuoteCols);
            //        if (ls.Contains(key))
            //        {
            //            return this.SerializeJsonValue(key, String.Format("{0}", value));
            //        }
            //        else
            //        {
            //            return this.QuoteMark + this.SerializeJsonValue(key, String.Format("{0}", value)) + this.QuoteMark;
            //        }
            //    }
            //    else
            //    {
            //        return this.QuoteMark + this.SerializeJsonValue(key, String.Format("{0}", value)) + this.QuoteMark;
            //    }
            //}
            //else
            //{
            //    return this.QuoteMark + this.SerializeJsonValue(key, String.Format("{0}", value)) + this.QuoteMark;
            //}
        }
        private string GetQuoteFormat(string key, string value)
        {
            if (!this.SetAllQuote && (this.isNumber(key, value) || this.IsBool(key, value)))
            {
                return "{0}";
            }
            else if (this.SetAllQuote)
            {
                if (this.NoQuoteCols != null)
                {
                    List<String> ls = new List<string>(NoQuoteCols);
                    if (ls.Contains(key))
                    {
                        return "{0}";
                    }
                    else
                    {
                        return this.QuoteMark + "{0}" + this.QuoteMark;
                    }
                }
                else
                {
                    return this.QuoteMark + "{0}" + this.QuoteMark;
                }
            }
            else
            {
                return this.QuoteMark + "{0}" + this.QuoteMark;
            }
        }
        private string SerializeJsonValue(string key, string value)
        {
            if (this.EscapeMethod == 0)
                return this.SerializeJsonValue_Replace(value);
            else
                return this.SerializeJsonValue_MJ(value);
        }
        private string SerializeJsonValue_MJ(string value)
        {
            return Uri.EscapeDataString(value);
        }
        private string SerializeJsonValue_Replace(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            string temstr;
            temstr = value;
            temstr = temstr.Replace("{", "｛").Replace("}", "｝")
                .Replace(":", ":")
                .Replace(",", ",")
                .Replace("[", "【")
                .Replace("]", "】")
                .Replace(";", ";")
                .Replace("\n", "<br/>")
                .Replace("\r", "");

            temstr = temstr.Replace("\t", "   ");
            temstr = temstr.Replace("'", "\'");
            temstr = temstr.Replace(@"\", @"\\");
            temstr = temstr.Replace("\"", "\"\"");
            return temstr;
        }
        private bool isNumber(string key, object value)
        {
            // If the first digit value ( Superfluous ), Because the data type , If an error occurs for digital . If you use to do it with , Open 
            //double d = 0;
            //bool bln = double.TryParse(Convert.ToString(value), out d);
            //if (!bln) return false;
            // Again the data type to digital 
            bool b = false;
            switch (this._DT.Columns[key].DataType.FullName)
            {
                case "DbType.AnsiString": break;
                case "DbType.AnsiStringFixedLength": break;
                case "DbType.Binary": break;
                case "DbType.Boolean": break;
                case "DbType.Byte": b = true; break;
                case "DbType.Currency": b = true; break;
                case "DbType.Date": break;
                case "DbType.DateTime": break;
                case "DbType.DateTime2": break;
                case "DbType.DateTimeOffset": break;
                case "DbType.Decimal": b = true; break;
                case "DbType.Double": b = true; break;
                case "DbType.Guid": break;
                case "DbType.Int16": b = true; break;
                case "DbType.Int32": b = true; break;
                case "DbType.Int64": b = true; break;
                case "DbType.Object": break;
                case "DbType.SByte": b = true; break;
                case "DbType.Single": b = true; break;
                case "DbType.String": break;
                case "DbType.StringFixedLength": break;
                case "DbType.Time": break;
                case "DbType.UInt16": b = true; break;
                case "DbType.UInt32": b = true; break;
                case "DbType.UInt64": b = true; break;
                case "DbType.VarNumeric": b = true; break;
                case "DbType.Xml": break;
            }
            return b;
        }
        private bool IsBool(string key, object value)
        {
            bool bR = false;
            bool bln = bool.TryParse(Convert.ToString(value), out bR);
            if (!bln) return false;

            bool b = false;
            switch (this._DT.Columns[key].DataType.FullName)
            {
                case "DbType.AnsiString": break;
                case "DbType.AnsiStringFixedLength": break;
                case "DbType.Binary": break;
                case "DbType.Boolean": b = true; break;
                case "DbType.Byte": break;
                case "DbType.Currency": break;
                case "DbType.Date": break;
                case "DbType.DateTime": break;
                case "DbType.DateTime2": break;
                case "DbType.DateTimeOffset": break;
                case "DbType.Decimal": break;
                case "DbType.Double": break;
                case "DbType.Guid": break;
                case "DbType.Int16": break;
                case "DbType.Int32": break;
                case "DbType.Int64": break;
                case "DbType.Object": break;
                case "DbType.SByte": break;
                case "DbType.Single": break;
                case "DbType.String": break;
                case "DbType.StringFixedLength": break;
                case "DbType.Time": break;
                case "DbType.UInt16": break;
                case "DbType.UInt32": break;
                case "DbType.UInt64": break;
                case "DbType.VarNumeric": break;
                case "DbType.Xml": break;
            }
            return b;
        }
        private bool IsDateTime(string key, object value)
        {
            DateTime d = DateTime.Now;
            bool bln = DateTime.TryParse(Convert.ToString(value), out d);
            if (!bln) return false;

            bool b = false;
            switch (this._DT.Columns[key].DataType.FullName)
            {
                case "DbType.AnsiString": break;
                case "DbType.AnsiStringFixedLength": break;
                case "DbType.Binary": break;
                case "DbType.Boolean": break;
                case "DbType.Byte": break;
                case "DbType.Currency": break;
                case "DbType.Date": b = true; break;
                case "DbType.DateTime": b = true; break;
                case "DbType.DateTime2": b = true; break;
                case "DbType.DateTimeOffset": b = true; break;
                case "DbType.Decimal": break;
                case "DbType.Double": break;
                case "DbType.Guid": break;
                case "DbType.Int16": break;
                case "DbType.Int32": break;
                case "DbType.Int64": break;
                case "DbType.Object": break;
                case "DbType.SByte": break;
                case "DbType.Single": break;
                case "DbType.String": break;
                case "DbType.StringFixedLength": break;
                case "DbType.Time": b = true; break;
                case "DbType.UInt16": break;
                case "DbType.UInt32": break;
                case "DbType.UInt64": break;
                case "DbType.VarNumeric": break;
                case "DbType.Xml": break;
            }
            return b;
        }
    }
}