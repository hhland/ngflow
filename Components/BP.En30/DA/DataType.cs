using System;
using System.Data;
using System.Web;
using System.Text.RegularExpressions;
using System.Net;
using System.Text;
using System.IO;
using BP.Sys;

namespace BP.DA
{
    /// <summary>
    /// DataType  The summary .
    /// </summary>
    public class DataType
    {
        #region  Date-related operations .
        /// <summary>
        ///  Specify the date of the Week 1 First Date .
        /// </summary>
        /// <param name="dt"> Specified date </param>
        /// <returns></returns>
        public static DateTime WeekOfMonday(DateTime dt)
        {
            if (dt.DayOfWeek == DayOfWeek.Monday)
                return DataType.ParseSysDate2DateTime(dt.ToString("yyyy-MM-dd") + " 00:01");

            for (int i = 0; i < 7; i++)
            {
                DateTime mydt = dt.AddDays(-i);
                if (mydt.DayOfWeek == DayOfWeek.Monday)
                    return DataType.ParseSysDate2DateTime(mydt.ToString("yyyy-MM-dd") + " 00:01");
            }
            throw new Exception("@ System error .");
        }
        /// <summary>
        ///  Specify the date of the Week 7第7 Day Date .
        /// </summary>
        /// <param name="dt"> Specified date </param>
        /// <returns></returns>
        public static DateTime WeekOfSunday(DateTime dt)
        {
            if (dt.DayOfWeek == DayOfWeek.Sunday)
                return DataType.ParseSysDate2DateTime(dt.ToString("yyyy-MM-dd") + " 00:01");

            for (int i = 0; i < 7; i++)
            {
                DateTime mydt = dt.AddDays(i);
                if (mydt.DayOfWeek == DayOfWeek.Sunday)
                    return DataType.ParseSysDate2DateTime(mydt.ToString("yyyy-MM-dd") + " 00:01");
            }
            throw new Exception("@ System error .");
        }
        /// <summary>
        ///  Saturday removed 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public static DateTime AddDays(DateTime dt, int days)
        {
            dt = dt.AddDays(days);

            if (dt.DayOfWeek == DayOfWeek.Saturday)
                return dt.AddDays(2);


            if (dt.DayOfWeek == DayOfWeek.Sunday)
                return dt.AddDays(1);

            return dt;
        }
        public static DateTime AddDays(string sysdt, int days)
        {
            DateTime dt = DataType.ParseSysDate2DateTime(sysdt);
            return AddDays(dt, days);
        }
        /// <summary> 
        ///  Take the specified date is the first few weeks of the year  
        /// </summary> 
        /// <param name="dtime"> Given date </param> 
        /// <returns> Digital   The first few weeks of the year </returns> 
        public static int WeekOfYear(DateTime dtime)
        {
            int weeknum = 0;
            DateTime tmpdate = DateTime.Parse(dtime.Year.ToString() + "-1" + "-1");
            DayOfWeek firstweek = tmpdate.DayOfWeek;
            //if(firstweek) 
            int i = dtime.DayOfYear - 1 + (int)firstweek;
            weeknum = i / 7;
            if (i > 0)
            {
                weeknum++;
            }
            return weeknum;
        }
        public static string TurnToJiDuByDataTime(string dt)
        {
            if (dt.Length <= 6)
                throw new Exception("@ To date format conversion quarter incorrect :" + dt);
            string yf = dt.Substring(5, 2);
            switch (yf)
            {
                case "01":
                case "02":
                case "03":
                    return dt.Substring(0, 4) + "-03";
                case "04":
                case "05":
                case "06":
                    return dt.Substring(0, 4) + "-06";
                case "07":
                case "08":
                case "09":
                    return dt.Substring(0, 4) + "-09";
                case "10":
                case "11":
                case "12":
                    return dt.Substring(0, 4) + "-12";
                default:
                    break;
            }
            return null;
        }
        #endregion

        #region Datatable Converted to Json
        /// <summary>     
        /// Datatable Converted to Json     
        /// </summary>    
        /// <param name="table">Datatable Object </param>     
        /// <returns>Json String </returns>     
        public static string ToJson(DataTable dt)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("[");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                jsonString.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName;
                    /** Zhou Peng small modifications -2014/11/11----------------------------START**/
                    // BillNoFormat Correspondence value:{YYYY}-{MM}-{dd}-{LSH4} Format When will an exception .
                    if (strKey.Equals("BillNoFormat"))
                    {
                        continue;
                    }
                    /** Zhou Peng small modifications -2014/11/11----------------------------END**/
                    string strValue = drc[i][j].ToString();
                    Type type = dt.Columns[j].DataType;
                    jsonString.Append("\"" + strKey + "\":");

                    strValue = String.Format(strValue, type);
                    if (j < dt.Columns.Count - 1)
                    {
                        jsonString.Append("\"" + strValue + "\",");
                    }
                    else
                    {
                        jsonString.Append("\"" + strValue + "\"");
                    }
                }
                jsonString.Append("},");
            }
            jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("]");
            return jsonString.ToString();
        }
        /// <summary>    
        /// DataTable Converted to Json     
        /// </summary>    
        public static string ToJson(DataTable dt, string jsonName)
        {
            StringBuilder Json = new StringBuilder();
            if (string.IsNullOrEmpty(jsonName))
                jsonName = dt.TableName;
            Json.Append("{\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Type type = dt.Rows[i][j].GetType();
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":" + String.Format(dt.Rows[i][j].ToString(), type));
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
        #endregion

        /// <summary>
        ///  Generate tree structure according to the general administration of the tree 
        /// </summary>
        /// <param name="dtTree"> The general format of the data table No,Name,ParentNo列</param>
        /// <param name="dtTree"> Root of number values </param>
        /// <returns></returns>
        public static DataTable PraseParentTree2TreeNo(DataTable dtTree, string parentNo)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("No", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Grade", typeof(string));
            dt.Columns.Add("IsDtl", typeof(string));

            dt.Columns.Add("RefNo", typeof(string));
            dt.Columns.Add("RefParentNo", typeof(string));

            return dt;
        }
        private static DataTable _PraseParentTree2TreeNo(DataTable dtTree)
        {
            return null;
        }

        /// <summary>
        ///  Convert MB
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static float PraseToMB(long val)
        {
            try
            {
                return float.Parse(String.Format("{0:##.##}", val / 1048576));
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strs"></param>
        /// <param name="isNumber"></param>
        /// <returns></returns>
        public static string PraseAtToInSql(string strs, bool isNumber)
        {
            strs = strs.Replace("@", "','");
            strs = strs + "'";
            strs = strs.Substring(2);
            if (isNumber)
                strs = strs.Replace("'", "");
            return strs;
        }
        /// <summary>
        ///  The contents of the things inside processed into ultra-connected .
        /// </summary>
        /// <param name="strContent"></param>
        /// <returns></returns>
        public static string DealSuperLink(string doc)
        {
            if (doc == null)
                return null;

            return doc;

            Regex urlregex = new Regex(@"(http:\/\/([\w.]+\/?)\S*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            doc = urlregex.Replace(doc, "<a href='' target='_blank'></a>");
            Regex emailregex = new Regex(@"([a-zA-Z_0-9.-]+@[a-zA-Z_0-9.-]+\.\w+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            doc = emailregex.Replace(doc, "<a href=mailto:></a>");
            return doc;
        }

        /// <summary>
        ///  Write file 
        /// </summary>
        /// <param name="file"> Path </param>
        /// <param name="Doc"> Content </param>
        public static void WriteFile(string file, string Doc)
        {
            System.IO.StreamWriter sr;
            if (System.IO.File.Exists(file))
                System.IO.File.Delete(file);


            try
            {
                //sr = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("GB2312"));
                sr = new System.IO.StreamWriter(file, false, System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new Exception("@ File :" + file + ", Error :" + ex.Message);
            }

            sr.Write(Doc);
            sr.Close();
        }
        /// <summary>
        ///  Read URL Content 
        /// </summary>
        /// <param name="url"> To read url</param>
        /// <param name="timeOut"> Timeout </param>
        /// <param name="encode">text code.</param>
        /// <returns> Return to read the contents </returns>
        public static string ReadURLContext(string url, int timeOut, Encoding encode)
        {
            HttpWebRequest webRequest = null;
            try
            {
                webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = "get";
                webRequest.Timeout = timeOut;
                string str = webRequest.Address.AbsoluteUri;
                str = str.Substring(0, str.LastIndexOf("/"));
            }
            catch (Exception ex)
            {
                try
                {
                    BP.DA.Log.DefaultLogWriteLineWarning("@ Read URL Error :URL=" + url + "@ Error Messages :" + ex.Message);
                    return null;
                }
                catch
                {
                    return ex.Message;
                }
            }
            //	 Examples of the type of return because it is WebRequest Instead of HttpWebRequest, So remember to be cast 
            //   Next, establish a HttpWebResponse To receive information sent by the server , It is a call HttpWebRequest.GetResponse To obtain the :
            HttpWebResponse webResponse;
            try
            {
                webResponse = (HttpWebResponse)webRequest.GetResponse();
            }
            catch (Exception ex)
            {
                try
                {
                    //  If a dead connection .
                    BP.DA.Log.DefaultLogWriteLineWarning("@ Get url=" + url + " Failure . Exception Information :" + ex.Message, true);
                    return null;
                }
                catch
                {
                    return ex.Message;
                }
            }

            // In case webResponse.StatusCode Value HttpStatusCode.OK, For success , Then you can then read the contents of the received :
            //  Get received stream 
            Stream stream = webResponse.GetResponseStream();
            System.IO.StreamReader streamReader = new StreamReader(stream, encode);
            string content = streamReader.ReadToEnd();
            webResponse.Close();
            return content;
        }
        /// <summary>
        ///  Read the file 
        /// </summary>
        /// <param name="file"> Path </param>
        /// <returns> Content </returns>
        public static string ReadTextFile(string file)
        {
            System.IO.StreamReader read = new System.IO.StreamReader(file, System.Text.Encoding.UTF8); //  File stream .
            string doc = read.ReadToEnd();  // Reading is completed .
            read.Close(); //  Shut down .
            return doc;
        }
        public static bool SaveAsFile(string filePath, string doc)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(filePath, false);
            sw.Write(doc);
            sw.Close();
            return true;
        }
        public static string ReadTextFile2Html(string file)
        {
            return DataType.ParseText2Html(ReadTextFile(file));
        }
        /// <summary>
        ///  Determine whether all characters 
        /// </summary>
        /// <param name="htmlstr"></param>
        /// <returns></returns>
        public static bool CheckIsChinese(string htmlstr)
        {
            char[] chs = htmlstr.ToCharArray();
            foreach (char c in chs)
            {
                int i = c.ToString().Length;
                if (i == 1)
                    return false;
            }
            return true;
        }

        #region  Yuan arc minutes 
        public static string TurnToFiels(float money)
        {
            string je = money.ToString("0.00");

            string strs = "";

            switch (je.Length)
            {
                case 7: //         千                                百                                  十                              元                                角                              分;
                    strs = "D" + je.Substring(0, 1) + ".TW,THOU.TW,D" + je.Substring(1, 1) + ".TW,HUN.TW,D" + je.Substring(2, 1) + ".TW,TEN.TW,D" + je.Substring(3, 1) + ".TW,YUAN.TW,D" + je.Substring(5, 1) + ".TW,JIAO.TW,D" + je.Substring(6, 1) + ".TW,FEN.TW";
                    break;
                case 6: // 百;
                    strs = "D" + je.Substring(0, 1) + ".TW,HUN.TW,D" + je.Substring(1, 1) + ".TW,TEN.TW,D" + je.Substring(2, 1) + ".TW,YUAN.TW,D" + je.Substring(4, 1) + ".TW,JIAO.TW,D" + je.Substring(5, 1) + ".TW,FEN.TW";
                    break;
                case 5: // 十;
                    strs = "D" + je.Substring(0, 1) + ".TW,TEN.TW,D" + je.Substring(1, 1) + ".TW,YUAN.TW,D" + je.Substring(3, 1) + ".TW,JIAO.TW,D" + je.Substring(4, 1) + ".TW,FEN.TW";
                    break;
                case 4: // 元;
                    if (money > 1)
                        strs = "D" + je.Substring(0, 1) + ".TW,YUAN.TW,D" + je.Substring(2, 1) + ".TW,JIAO.TW,D" + je.Substring(3, 1) + ".TW,FEN.TW";
                    else
                        strs = "D" + je.Substring(2, 1) + ".TW,JIAO.TW,D" + je.Substring(3, 1) + ".TW,FEN.TW";
                    break;
                default:
                    throw new Exception(" Did not involve such a large amount of broadcast ");
            }

            //			strs=strs.Replace(",D0.TW,JIAO.TW,D0.TW,FEN.TW",""); //  Replace  .0角0分;
            //			strs=strs.Replace("D0.TW,HUN.TW,D0.TW,TEN.TW","D0.TW"); //  Replace  .0百0十 为 0 ;
            //			strs=strs.Replace("D0.TW,THOU.TW","D0.TW");  //  Replace the zero one thousand .
            //			strs=strs.Replace("D0.TW,HUN.TW","D0.TW");
            //			strs=strs.Replace("D0.TW,TEN.TW","D0.TW");
            //			strs=strs.Replace("D0.TW,JIAO.TW","D0.TW");
            //			strs=strs.Replace("D0.TW,FEN.TW","D0.TW");
            return strs;
        }
        #endregion

        public static string Html2Text(string htmlstr)
        {
            htmlstr = htmlstr.Replace("<BR>", "\n");
            return htmlstr.Replace("&nbsp;", " ");
            //	return htmlstr;
        }
        public static string ByteToString(byte[] bye)
        {
            string s = "";
            foreach (byte b in bye)
            {
                s += b.ToString();
            }
            return s;
        }
        public static byte[] StringToByte(string s)
        {
            byte[] bs = new byte[s.Length];
            char[] cs = s.ToCharArray();
            int i = 0;
            foreach (char c in cs)
            {
                bs[i] = Convert.ToByte(c);
                i++;
            }
            return bs;
        }

        /// <summary>
        ///  By way of a percentage 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string GetPercent(decimal a, decimal b)
        {
            decimal p = a / b;
            return p.ToString("0.00%");
        }
        public static string GetWeek(int weekidx)
        {
            switch (weekidx)
            {
                case 0:
                    return " On Sunday ";
                case 1:
                    return " Monday ";
                case 2:
                    return " Tuesday ";
                case 3:
                    return " Wednesday ";
                case 4:
                    return " Thursday ";
                case 5:
                    return " Friday ";
                case 6:
                    return " On Saturday ";
                default:
                    throw new Exception("error weekidx=" + weekidx);
            }
        }

        public static string GetABC(string abc)
        {
            switch (abc)
            {
                case "A":
                    return "B";
                case "B":
                    return "C";
                case "C":
                    return "D";
                case "D":
                    return "E";
                case "E":
                    return "F";
                case "F":
                    return "G";
                case "G":
                    return "H";
                case "H":
                    return "I";
                case "I":
                    return "J";
                case "J":
                    return "K";
                case "K":
                    return "L";
                case "L":
                    return "M";
                case "M":
                    return "N";
                case "N":
                    return "O";
                case "Z":
                    return "A";
                default:
                    throw new Exception("abc error" + abc);
            }
        }
        public static string GetBig5(string text)
        {
            System.Text.Encoding e2312 = System.Text.Encoding.GetEncoding("GB2312");
            byte[] bs = e2312.GetBytes(text);
            System.Text.Encoding e5 = System.Text.Encoding.GetEncoding("Big5");
            byte[] bs5 = System.Text.Encoding.Convert(e2312, e5, bs);
            return e5.GetString(bs5);
        }
        /// <summary>
        ///  Return  data1 - data2  Number of days .
        /// </summary>
        /// <param name="data1">fromday</param>
        /// <param name="data2">today</param>
        /// <returns> The number of days apart </returns>
        public static int SpanDays(string fromday, string today)
        {
            try
            {
                TimeSpan span = DateTime.Parse(today.Substring(0, 10)) - DateTime.Parse(fromday.Substring(0, 10));
                return span.Days;
            }
            catch
            {
                //throw new Exception(ex.Message +"" +fromday +"  " +today ) ; 
                return 0;
            }
        }
        /// <summary>
        ///  Return  QuarterFrom - QuarterTo  Quarter .
        /// </summary>
        /// <param name="QuarterFrom">QuarterFrom</param>
        /// <param name="QuarterTo">QuarterTo</param>
        /// <returns> Separated by a quarter </returns>
        public static int SpanQuarter(string _APFrom, string _APTo)
        {
            DateTime fromdate = Convert.ToDateTime(_APFrom + "-01");
            DateTime todate = Convert.ToDateTime(_APTo + "-01");
            int i = 0;
            if (fromdate > todate)
                throw new Exception(" Select an error ! Starting times " + _APFrom + " Can not be greater than the termination of the period " + _APTo + "!");

            while (fromdate <= todate)
            {
                i++;
                fromdate = fromdate.AddMonths(1);
            }

            int j = (i + 2) / 3;
            return j;
        }
        /// <summary>
        ///  Now the number of days .
        /// </summary>
        /// <param name="data1"></param>
        /// <returns></returns>
        public static int SpanDays(string data1)
        {
            TimeSpan span = DateTime.Now - DateTime.Parse(data1.Substring(0, 10));
            return span.Days;
        }
        /// <summary>
        ///  Check whether a field or table name 
        /// </summary>
        /// <param name="str"> To check the field or table names </param>
        /// <returns> Legality </returns>
        public static bool CheckIsFieldOrTableName(string str)
        {
            string s = str.Substring(0, 1);
            if (DataType.IsNumStr(s))
                return false;

            string chars = "~!@#$%^&*()_+`{}|:'<>?[];',./";
            if (chars.Contains(s) == true)
                return false;
            return true;
        }
        public static string ParseText2Html(string val)
        {
            //val = val.Replace("&", "&amp;");
            //val = val.Replace("<","&lt;");
            //val = val.Replace(">","&gt;");

            //val = val.Replace(char(34), "&quot;");
            //val = val.Replace(char(9), "&nbsp;&nbsp;&nbsp;");
            //val = val.Replace(" ", "&nbsp;");

            return val.Replace("\n", "<BR>").Replace("~", "'");

            //return val.Replace("\n", "<BR>&nbsp;&nbsp;").Replace("~", "'");

        }
        public static string ParseHtmlToText(string val)
        {
            if (val == null)
                return val;

            val = val.Replace("&nbsp;", " ");
            val = val.Replace("  ", " ");

            val = val.Replace("</td>", "");
            val = val.Replace("</TD>", "");

            val = val.Replace("</tr>", "");
            val = val.Replace("</TR>", "");

            val = val.Replace("<tr>", "");
            val = val.Replace("<TR>", "");

            val = val.Replace("</font>", "");
            val = val.Replace("</FONT>", "");

            val = val.Replace("</table>", "");
            val = val.Replace("</TABLE>", "");


            val = val.Replace("<BR>", "\n\t");
            val = val.Replace("<BR>", "\n\t");
            val = val.Replace("&nbsp;", " ");

            val = val.Replace("<BR><BR><BR><BR>", "<BR><BR>");
            val = val.Replace("<BR><BR><BR><BR>", "<BR><BR>");
            val = val.Replace("<BR><BR>", "<BR>");

            char[] chs = val.ToCharArray();

            bool isStartRec = false;
            string recStr = "";
            foreach (char c in chs)
            {
                if (c == '<')
                {
                    recStr = "";
                    isStartRec = true; /*  Start recording  */
                }

                if (isStartRec)
                {
                    recStr += c.ToString();
                }

                if (c == '>')
                {
                    isStartRec = false;

                    if (recStr == "")
                    {
                        isStartRec = false;
                        continue;
                    }

                    /*  This marked the beginning of the analysis stuff inside .*/
                    string market = recStr.ToLower();
                    if (market.Contains("<img"))
                    {
                        /*  This is an image tag  */
                        isStartRec = false;
                        recStr = "";
                        continue;
                    }
                    else
                    {
                        val = val.Replace(recStr, "");
                        isStartRec = false;
                        recStr = "";
                    }
                }
            }


            val = val.Replace(" Fonts : Medium and small ", "");
            val = val.Replace(" Fonts : Medium and small ", "");

            val = val.Replace("  ", " ");
            val = val.Replace("\t", "");
            val = val.Replace("\n", "");
            val = val.Replace("\r", "");
            return val;
        }
        /// <summary>
        ///  Will be transformed into Chinese Pinyin 
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static string ParseStringToPinyin(string exp)
        {
            exp = exp.Trim();
            string pinYin = "", str = null;
            char[] chars = exp.ToCharArray();
            foreach (char c in chars)
            {
                try
                {
                    str = BP.Tools.chs2py.convert(c.ToString());
                    pinYin += str.Substring(0, 1).ToUpper() + str.Substring(1);
                }
                catch
                {
                    pinYin += c;
                }
            }
            return pinYin;
        }
        /// <summary>
        ///  Converted into phonetic alphabet characters first 
        /// </summary>
        /// <param name="str"> Chinese string to be converted </param>
        /// <returns> Pinyin </returns>
        public static string ParseStringToPinyinWordFirst(string str)
        {
            try
            {
                String _Temp = null;
                for (int i = 0; i < str.Length; i++)
                {
                    _Temp = _Temp + BP.DA.DataType.ParseStringToPinyin(str.Substring(i, 1));
                }
                return _Temp;
            }
            catch (Exception ex)
            {
                throw new Exception("@ Error :" + str + ", Can not be converted into phonetic .");
            }
        }
        /// <summary>
        ///  Converted into phonetic alphabet characters first 
        /// </summary>
        /// <param name="str"> Chinese string to be converted </param>
        /// <returns> Pinyin </returns>
        public static string ParseStringToPinyinJianXie(string str)
        {
            try
            {
                String _Temp = null;
                var re = string.Empty;
                for (int i = 0; i < str.Length; i++)
                {
                    re = BP.DA.DataType.ParseStringToPinyin(str.Substring(i, 1));
                    _Temp += re.Length == 0 ? "" : re.Substring(0, 1);
                }
                return _Temp;
            }
            catch (Exception ex)
            {
                throw new Exception("@ Error :" + str + ", Can not be converted into phonetic .");
            }
        }
        /// <summary>
        ///  Converted into  decimal
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static decimal ParseExpToDecimal(string exp)
        {
            if (exp.Trim() == "")
                throw new Exception("DataType.ParseExpToDecimal To convert the expression is empty .");


            exp = exp.Replace("+-", "-");
            exp = exp.Replace("￥", "");
            //exp=exp.Replace(" ","");  You can not replace , Because there sql Time expression formula , An error .
            exp = exp.Replace("\n", "");
            exp = exp.Replace("\t", "");

            exp = exp.Replace("＋", "+");
            exp = exp.Replace("－", "-");
            exp = exp.Replace("＊", "*");
            exp = exp.Replace("／", "/");
            exp = exp.Replace("）", ")");
            exp = exp.Replace("（", "(");

            exp = exp.Replace(".00.00", "00");

            exp = exp.Replace("--", "- -");


            if (exp.IndexOf("@") != -1)
                return 0;

            string val = exp.Substring(0, 1);
            if (val == "-")
                exp = exp.Substring(1);

            //  exp = exp.Replace("*100%", "*100");

            exp = exp.Replace("*100%", "*1");

            try
            {
                return decimal.Parse(exp);
            }
            catch
            {
            }

            try
            {
                string sql = "SELECT  " + exp + " as Num  ";
                switch (SystemConfig.AppCenterDBType)
                {
                    case DBType.MSSQL:
                    case DBType.Access:
                        sql = "SELECT  " + exp + " as Num  ";
                        return DBAccess.RunSQLReturnValDecimal(sql, 0, 2);
                    case DBType.Oracle:
                        sql = "SELECT  " + exp + " NUM from DUAL ";
                        return DBAccess.RunSQLReturnValDecimal(sql, 0, 2);
                    case DBType.Informix:
                        sql = "SELECT  " + exp + " NUM from  taa_onerow ";
                        return DBAccess.RunSQLReturnValDecimal(sql, 0, 2);
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.DefaultLogWriteLineInfo(ex.Message);
                /*  If an exception is thrown on the press  0   Calculate . */
                return 0;
            }

            exp = exp.Replace("-0", "");


            try
            {
                BP.Tools.StringExpressionCalculate sc = new BP.Tools.StringExpressionCalculate();
                return sc.TurnToDecimal(exp);
            }
            catch (Exception ex)
            {
                if (exp.IndexOf("/") != -1)
                    return 0;
                throw new Exception(" Expression (\"" + exp + "\") Calculation error :" + ex.Message);
            }
        }
        public static string ParseFloatToCash(float money)
        {
            if (money == 0)
                return " Lingyuanlingjiao zero ";
            BP.Tools.DealString d = new BP.Tools.DealString();
            d.InputString = money.ToString();
            d.ConvertToChineseNum();
            return d.OutString;
        }
        public static string ParseFloatToRMB(float money)
        {
            if (money == 0)
                return " Lingyuanlingjiao zero ";
            BP.Tools.DealString d = new BP.Tools.DealString();
            d.InputString = money.ToString();
            d.ConvertToChineseNum();
            return d.OutString;
        }
        /// <summary>
        ///  Get a date , The system 
        /// </summary>
        /// <param name="dataStr"></param>
        /// <returns></returns>
        public DateTime Parse(string dataStr)
        {
            return DateTime.Parse(dataStr);
        }
        /// <summary>
        ///  System-defined time format  yyyy-MM-dd .
        /// </summary>
        public static string SysDataFormat
        {
            get
            {
                return "yyyy-MM-dd";
            }
        }
        /// <summary>
        ///  Current date 
        /// </summary>
        public static string CurrentData
        {
            get
            {
                return DateTime.Now.ToString(DataType.SysDataFormat);
            }
        }
        public static string CurrentTime
        {
            get
            {
                return DateTime.Now.ToString("hh:mm");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string CurrentTimeQuarter
        {
            get
            {
                return DateTime.Now.ToString("hh:mm");
            }
        }
        /// <summary>
        ///  For a time , Returns a moment kind of time .
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ParseTime2TimeQuarter(string time)
        {
            string hh = time.Substring(0, 3);
            int mm = int.Parse(time.Substring(3, 2));
            if (mm == 0)
            {
                return hh + "00";
            }

            if (mm < 15)
            {
                return hh + "00";
            }
            if (mm >= 15 && mm < 30)
            {
                return hh + "15";
            }

            if (mm >= 30 && mm < 45)
            {
                return hh + "30";
            }

            if (mm >= 45 && mm < 60)
            {
                return hh + "45";
            }
            return time;
        }
        public static string CurrentDay
        {
            get
            {
                return DateTime.Now.ToString("dd");
            }
        }

        /// <summary>
        ///  The current accounting period 
        /// </summary>
        public static string CurrentAP
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM");
            }
        }
        /// <summary>
        ///  The current accounting period 
        /// </summary>
        public static string CurrentYear
        {
            get
            {
                return DateTime.Now.ToString("yyyy");
            }
        }
        public static string CurrentMonth
        {
            get
            {
                return DateTime.Now.ToString("MM");
            }
        }
        /// <summary>
        ///  The current accounting period  yyyy-MM
        /// </summary>
        public static string CurrentYearMonth
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM");
            }
        }
        public static string GetJDByMM(string mm)
        {
            string jd = "01";
            switch (mm)
            {
                case "01":
                case "02":
                case "03":
                    jd = "01";
                    break;
                case "04":
                case "05":
                case "06":
                    jd = "04";
                    break;
                case "07":
                case "08":
                case "09":
                    jd = "07";
                    break;
                case "10":
                case "11":
                case "12":
                    jd = "10";
                    break;
                default:
                    throw new Exception("@ Not a valid month format " + mm);
            }
            return jd;
        }
        /// <summary>
        ///  During the current quarter yyyy-MM
        /// </summary>
        public static string CurrentAPOfJD
        {
            get
            {
                return DateTime.Now.ToString("yyyy") + "-" + DataType.GetJDByMM(DateTime.Now.ToString("MM"));
            }
        }
        /// <summary>
        ///  The previous quarter of the current quarter .
        /// </summary>
        public static string CurrentAPOfJDOfFrontFamily
        {
            get
            {
                DateTime now = DateTime.Now.AddMonths(-3);
                return now.ToString("yyyy") + "-" + DataType.GetJDByMM(now.ToString("MM"));
            }
        }
        /// <summary>
        /// yyyy-JD
        /// </summary>
        public static string CurrentAPOfPrevious
        {
            get
            {
                int m = int.Parse(DateTime.Now.ToString("MM"));
                return DateTime.Now.ToString("yyyy-MM");
            }
        }
        /// <summary>
        ///  Remove the previous month of the current month 
        /// </summary>
        public static string CurrentNYOfPrevious
        {
            get
            {
                DateTime dt = DateTime.Now;
                dt = dt.AddMonths(-1);
                return dt.ToString("yyyy-MM");
            }
        }
        /// <summary>
        ///  During the current quarter 
        /// </summary>
        public static string CurrentAPOfYear
        {
            get
            {
                return DateTime.Now.ToString("yyyy");
            }
        }
        /// <summary>
        ///  Current date and time 
        /// </summary>
        public static string CurrentDataTime
        {
            get
            {
                return DateTime.Now.ToString(DataType.SysDataTimeFormat);
            }
        }
        public static string CurrentDataTimeOfDef
        {
            get
            {
                switch (BP.Web.WebUser.SysLang)
                {
                    case "CH":
                    case "B5":
                        return CurrentDataTimeCNOfShort;
                    case "EN":
                        return DateTime.Now.ToString("MM/DD/YYYY");
                    default:
                        break;
                }
                return CurrentDataTimeCNOfShort;
            }
        }
        public static string CurrentDataTimeCNOfShort
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            }
        }
        public static string CurrentDataCNOfShort
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd");
            }
        }
        public static string CurrentDataCNOfLong
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd");
            }
        }
        /// <summary>
        ///  Current date and time 
        /// </summary>
        public static string CurrentDataTimeCN
        {
            get
            {
                return DateTime.Now.ToString(DataType.SysDataFormatCN) + "," + GetWeekName(DateTime.Now.DayOfWeek);
            }
        }
        private static string GetWeekName(System.DayOfWeek dw)
        {
            switch (dw)
            {
                case DayOfWeek.Monday:
                    return " Monday ";
                case DayOfWeek.Thursday:
                    return " Thursday ";
                case DayOfWeek.Friday:
                    return " Friday ";
                case DayOfWeek.Saturday:
                    return " On Saturday ";
                case DayOfWeek.Sunday:
                    return " On Sunday ";
                case DayOfWeek.Tuesday:
                    return " Tuesday ";
                case DayOfWeek.Wednesday:
                    return " Wednesday ";
                default:
                    return "";
            }
        }

        /// <summary>
        ///  Current date and time 
        /// </summary>
        public static string CurrentDataTimess
        {
            get
            {
                return DateTime.Now.ToString(DataType.SysDataTimeFormat + ":ss");
            }
        }
        public static string ParseSysDateTime2SysDate(string sysDateformat)
        {
            try
            {
                return sysDateformat.Substring(0, 10);
            }
            catch (Exception ex)
            {
                throw new Exception(" Date format error :" + sysDateformat + " errorMsg=" + ex.Message);
            }
        }
        /// <summary>
        /// 把chichengsoft The system date format to the system date format .
        /// </summary>
        /// <param name="sysDateformat">yyyy-MM-dd</param>
        /// <returns>DateTime</returns>
        public static DateTime ParseSysDate2DateTime(string sysDateformat)
        {
            if (sysDateformat == null || sysDateformat.Trim().Length == 0)
                return DateTime.Now;


            try
            {
                if (sysDateformat.Length > 10)
                    return ParseSysDateTime2DateTime(sysDateformat);

                sysDateformat = sysDateformat.Trim();
                //DateTime.Parse(sysDateformat,
                string[] strs = null;
                if (sysDateformat.IndexOf("-") != -1)
                {
                    strs = sysDateformat.Split('-');
                }

                if (sysDateformat.IndexOf("/") != -1)
                {
                    strs = sysDateformat.Split('/');
                }

                int year = int.Parse(strs[0]);
                int month = int.Parse(strs[1]);
                int day = int.Parse(strs[2]);

                //DateTime dt= DateTime.Now;
                return new DateTime(year, month, day, 0, 0, 0);
            }
            catch (Exception ex)
            {
                throw new Exception(" Date [" + sysDateformat + "] Conversion Error :" + ex.Message + " Invalid date format .");
            }
            //return dt;			 
        }
        /// <summary>
        /// 2005-11-04 09:12
        /// </summary>
        /// <param name="sysDateformat"></param>
        /// <returns></returns>
        public static DateTime ParseSysDateTime2DateTime(string sysDateformat)
        {
            try
            {
                //if (sysDateformat.Length==10)
                //    sysDateformat=sysDateformat+" 00:01";

                //int year = int.Parse(sysDateformat.Substring(0,4)) ;
                //int month = int.Parse(sysDateformat.Substring(5,2)) ;
                //int day = int.Parse(sysDateformat.Substring(8,2)) ;

                //int hh=int.Parse(sysDateformat.Substring(11,2)) ;
                //int mm=int.Parse(sysDateformat.Substring(14,2)) ;
                //DateTime dt = new DateTime(year,month,day,hh,mm,1) ;
                //return dt;
                return Convert.ToDateTime(sysDateformat);
            }
            catch (Exception ex)
            {
                throw new Exception("@ Time format is incorrect :" + sysDateformat + "@ Technical Information :" + ex.Message);
            }
        }

        /// <summary>
        ///  Gets a string representation of the time between the two ,如:1天2时34分
        /// <para>added by liuxc,2014-12-4</para>
        /// </summary>
        /// <param name="t1"> Start Time </param>
        /// <param name="t2"> End Time </param>
        /// <returns> Return :x天x时x分</returns>
        public static string GetSpanTime(DateTime t1, DateTime t2)
        {
            var span = t2 - t1;
            var days = span.Days;
            var hours = span.Hours;
            var minutes = span.Minutes;

            if (days == 0 && hours == 0 && minutes == 0)
                minutes = span.Seconds > 0 ? 1 : 0;

            var spanStr = string.Empty;

            if (days > 0)
                spanStr += days + "days";

            if (hours > 0)
                spanStr += hours + "h";

            if (minutes > 0)
                spanStr += minutes + "min";

            if (spanStr.Length == 0)
                spanStr = "0min";

            return spanStr;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtoffrom"></param>
        /// <param name="dtofto"></param>
        /// <returns></returns>
        public static int GetSpanDays(string dtoffrom, string dtofto)
        {
            DateTime dtfrom = DataType.ParseSysDate2DateTime(dtoffrom);
            DateTime dtto = DataType.ParseSysDate2DateTime(dtofto);

            TimeSpan ts = dtfrom - dtto;
            return ts.Days;
        }
        public static int GetSpanMinute(string fromdatetim, string toDateTime)
        {
            DateTime dtfrom = DataType.ParseSysDateTime2DateTime(fromdatetim);
            DateTime dtto = DataType.ParseSysDateTime2DateTime(toDateTime);

            TimeSpan ts = dtfrom - dtto;
            return ts.Minutes;
        }
        /// <summary>
        ///  The time is now 
        /// </summary>
        /// <param name="fromdatetim"></param>
        /// <returns> The number of points </returns>
        public static int GetSpanMinute(string fromdatetim)
        {
            DateTime dtfrom = DataType.ParseSysDateTime2DateTime(fromdatetim);
            DateTime dtto = DateTime.Now;
            TimeSpan ts = dtfrom - dtto;
            return ts.Minutes + ts.Hours * 60;
        }
        /// <summary>
        ///  System-defined date and time format  yyyy-MM-dd hh:mm
        /// </summary>
        public static string SysDataTimeFormat
        {
            get
            {
                return "yyyy-MM-dd HH:mm";
            }
        }
        public static string SysDataFormatCN
        {
            get
            {
                return "yyyy-MM-dd";
            }
        }
        public static string SysDatatimeFormatCN
        {
            get
            {
                return "yyyy-MM-dd HH:mm";
            }
        }
        public static DBUrlType GetDBUrlByString(string strDBUrl)
        {
            switch (strDBUrl)
            {
                case "AppCenterDSN":
                    return DBUrlType.AppCenterDSN;
                case "DBAccessOfOracle":
                    return DBUrlType.DBAccessOfOracle;
                case "DBAccessOfMSMSSQL":
                    return DBUrlType.DBAccessOfMSMSSQL;
                case "DBAccessOfOLE":
                    return DBUrlType.DBAccessOfOLE;
                case "DBAccessOfODBC":
                    return DBUrlType.DBAccessOfODBC;
                default:
                    throw new Exception("@ Without this type [" + strDBUrl + "]");
            }
        }
        public static int GetDataTypeByString(string datatype)
        {
            switch (datatype)
            {
                case "AppBoolean":
                    return DataType.AppBoolean;
                case "AppDate":
                    return DataType.AppDate;
                case "AppDateTime":
                    return DataType.AppDateTime;
                case "AppDouble":
                    return DataType.AppDouble;
                case "AppFloat":
                    return DataType.AppFloat;
                case "AppInt":
                    return DataType.AppInt;
                case "AppMoney":
                    return DataType.AppMoney;
                case "AppString":
                    return DataType.AppString;
                default:
                    throw new Exception("@ Without this type " + datatype);
            }
        }
        public static string GetDataTypeDese(int datatype)
        {
            if (Web.WebUser.SysLang == "CH")
            {
                switch (datatype)
                {
                    case DataType.AppBoolean:
                        return " Boolean (Int)";
                    case DataType.AppDate:
                        return " Date nvarchar";
                    case DataType.AppDateTime:
                        return " Date Time nvarchar";
                    case DataType.AppDouble:
                        return " Double (double)";
                    case DataType.AppFloat:
                        return " Floating point (float)";
                    case DataType.AppInt:
                        return " Integer (int)";
                    case DataType.AppMoney:
                        return " Currency (float)";
                    case DataType.AppString:
                        return " Character (nvarchar)";
                    default:
                        throw new Exception("@ Without this type ");
                }
            }

            switch (datatype)
            {
                case DataType.AppBoolean:
                    return "Boolen";
                case DataType.AppDate:
                    return "Date";
                case DataType.AppDateTime:
                    return "Datetime";
                case DataType.AppDouble:
                    return "Double";
                case DataType.AppFloat:
                    return "Float";
                case DataType.AppInt:
                    return "Int";
                case DataType.AppMoney:
                    return "Money";
                case DataType.AppString:
                    return "Nvarchar";
                default:
                    throw new Exception("@ Without this type ");
            }
        }
        /// <summary>
        ///  Produced adapt image size 
        ///  Use : In the position of the fixed size of the container , Display a fixed image .
        /// </summary>
        /// <param name="height"> Container height </param>
        /// <param name="width"> Container width </param>
        /// <param name="AdaptHeight"> Original Image Height </param>
        /// <param name="AdaptWidth"> Original Image Width </param>
        /// <param name="isFull"> Whether filling :是, Small pictures will enlarge filled container . 否, Small picture is not enlarged to retain the original size </param>
        public static void GenerPictSize(float panelHeight, float panelWidth, ref float AdaptHeight, ref float AdaptWidth, bool isFullPanel)
        {
            if (isFullPanel == false)
            {
                if (panelHeight <= AdaptHeight && panelWidth <= AdaptWidth)
                    return;
            }

            float zoom = 1;
            zoom = System.Math.Min(panelHeight / AdaptHeight, panelWidth / AdaptWidth);
            AdaptHeight = AdaptHeight * zoom;
            AdaptWidth = AdaptWidth * zoom;
        }



        #region  Data Types .
        /// <summary>
        /// string
        /// </summary>
        public const int AppString = 1;
        /// <summary>
        /// int
        /// </summary>
        public const int AppInt = 2;
        /// <summary>
        /// float
        /// </summary>
        public const int AppFloat = 3;
        /// <summary>
        /// AppBoolean
        /// </summary>
        public const int AppBoolean = 4;
        /// <summary>
        /// AppDouble
        /// </summary>
        public const int AppDouble = 5;
        /// <summary>
        /// AppDate
        /// </summary>
        public const int AppDate = 6;
        /// <summary>
        /// AppDateTime
        /// </summary>
        public const int AppDateTime = 7;
        /// <summary>
        /// AppMoney
        /// </summary>
        public const int AppMoney = 8;
        /// <summary>
        ///  Rate Percent .
        /// </summary>
        public const int AppRate = 9;
        #endregion

        public static string StringToDateStr(string str)
        {
            try
            {
                DateTime dt = DateTime.Parse(str);
                string year = dt.Year.ToString();
                string month = dt.Month.ToString();
                string day = dt.Day.ToString();
                return year + "-" + month.PadLeft(2, '0') + "-" + day.PadLeft(2, '0');
                //return str;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public static string GenerSpace(int spaceNum)
        {
            if (spaceNum <= 0)
                return "";

            string strs = "";
            while (spaceNum != 0)
            {
                strs += "&nbsp;&nbsp;";
                spaceNum--;
            }
            return strs;
        }
        public static string GenerBR(int spaceNum)
        {
            string strs = "";
            while (spaceNum != 0)
            {
                strs += "<BR>";
                spaceNum--;
            }
            return strs;
        }
        public static bool IsImgExt(string ext)
        {
            ext = ext.Replace(".", "").ToLower();
            switch (ext)
            {
                case "jpg":
                case "gif":
                case "jepg":
                case "bmp":
                case "png":
                case "tif":
                case "gsp":
                case "mov":
                case "psd":
                case "tiff":
                case "wmf":
                    return true;
                default:
                    return false;
            }
        }
        public static bool IsVoideExt(string ext)
        {
            ext = ext.Replace(".", "").ToLower();
            switch (ext)
            {
                case "mp3":
                case "mp4":
                case "asf":
                case "wma":
                case "rm":
                case "rmvb":
                case "mpg":
                case "wmv":
                case "quicktime":
                case "avi":
                case "flv":
                case "mpeg":
                    return true;
                default:
                    return false;
            }
        }
        /// <summary>
        ///  Determine whether it is Num  Character string .
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumStr(string str)
        {
            try
            {
                decimal d = decimal.Parse(str);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        ///  Is an odd time to time 
        /// </summary>
        /// <param name="num">will judg value</param>
        /// <returns></returns>
        public static bool IsQS(int num)
        {
            int ii = 0;
            for (int i = 0; i < 500; i++)
            {
                if (num == ii)
                    return false;
                ii = ii + 2;
            }
            return true;
        }

        public static bool StringToBoolean(string str)
        {
            if (str == null || str == "" || str == ",nbsp;")
                return false;

            if (str == "0" || str == "1")
            {
                if (str == "0")
                    return false;
                else
                    return true;
            }
            else if (str == "true" || str == "false")
            {
                if (str == "false")
                    return false;
                else
                    return true;

            }
            else if (str == "Yes" || str == "No")
            {
                if (str == "No")
                    return false;
                else
                    return true;
            }
            else
                throw new Exception("@ To convert [" + str + "] No bool  Type ");
        }

        #region  Ported on str Operations .
        /// <summary>
        ///  Determine whether it is an integer 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInt(string value)
        {
            string pattern = @"^[+-]?\d+$";
            return Regex.IsMatch(value, pattern);
        }
        /// <summary>
        ///  Determine whether it is a decimal 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDecimal(string value)
        {
            string pattern = @"^(\d*\.)?\d+$";
            return Regex.IsMatch(value, pattern);
        }
        public static bool IsTelOrHandSetNum(string no)
        {
            if (no.Length < 7)
                return false;

            return true;
        }

        /// <summary>
        ///  Whether it is a real number 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsRealNum(string value)
        {
            string pattern = @"^[-+]?[0-9]+\.?[0-9]*$";
            return Regex.IsMatch(value, pattern);
        }

        /// <summary>
        ///  Determine whether it is legitimate Email
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEmail(string value)
        {
            string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            return Regex.IsMatch(value, pattern);
        }

        /// <summary>
        ///   It is judged whether the letter , Numbers and special characters （-_.'&） The combination 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsWord(string value)
        {
            string pattern = @"^([a-zA-z0-9_\-\.\'\&])*$";
            return Regex.IsMatch(value, pattern);
        }

        /// <summary>
        ///  Determine whether it is a legitimate password format 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsPassword(string value)
        {
            string pattern = @"^(\w|\s|[`~!@#\$%\^&\*\(\)_\+\-=\{\}\[\]\:\'\<\>,\.\?\|/\\;""])*$";
            return Regex.IsMatch(value, pattern);
        }
        /// <summary>
        ///  Determines whether a valid username , English alphabet , Digital , And special characters （'_-.&）, After the special character must be a letter or number , And only in the middle of the string 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsUserName(string value)
        {
            string pattern = @"^(([a-zA-z0-9][\'_\-\.\&])*)?[a-zA-z0-9]+$";
            return Regex.IsMatch(value, pattern);
        }
        /// <summary>
        ///  Determine whether it is Chinese , Letter , Numbers and special characters （-_.'&） The combination 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsName(string value)
        {
            string pattern = @"^(\w|\s|[\'_\-\.\&])*$";
            return Regex.IsMatch(value, pattern);
        }
        /// <summary>
        ///  Get bulk character 
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static string GetStrUnit(string unit)
        {
            return Regex.Replace(unit, @"[0-9]", "", RegexOptions.IgnoreCase).ToString();
        }
        /// <summary>
        ///  Batch unit numbers obtained 
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static decimal GetUnitNum(string unit)
        {
            unit = unit.Trim().Replace(" ", "");
            // Define and initialize return value 
            decimal unitNum = 0;

            // Define and initialize a non-numeric part 
            string left = Regex.Replace(unit, @"[A-Za-z]", "", RegexOptions.IgnoreCase);
            try
            {
                unitNum = decimal.Parse(left);
            }
            catch // If the batch does not include a digital 
            {
                if (unit.Trim().ToLower().Replace(" ", "") == "twounit")
                {
                    unitNum = 2;
                }
                else
                {
                    unitNum = 1;
                }
            }
            return unitNum;
        }

        /// <summary>
        ///  Depending on the number （ Batch ） And bulk units to get the number of individuals 
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static int GetQuantity(decimal quantity, string unit)
        {
            decimal dunit = GetUnitNum(unit);
            return (int)(quantity * dunit + 0.5m);
        }

        /// <summary>
        ///  Get the number of bulk quantities based on the number of individuals and units 
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static decimal GetUnitQty(decimal quantity, string unit)
        {
            decimal dunit = GetUnitNum(unit);
            return quantity / dunit;
        }

        /// <summary>
        ///   The minimum unit price to get , If the weight of the unit is , To divide the batch 
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public static decimal GetLeastPrice(string unit, decimal price)
        {
            // To remove the bulk of the digital part 
            string sUnit = Regex.Replace(unit, @"[0-9]", "", RegexOptions.IgnoreCase);

            // Quantities are by weight (g或pound)
            if (sUnit.Trim().ToLower() == "g" || sUnit.ToLower().IndexOf("pound") > 0)
            {
                // Obtained in the digital part of the batch unit 
                decimal dUnit = GetUnitNum(unit);
                return decimal.Divide(price, dUnit);
            }
            else
            {
                // Batch already is the smallest unit 
                return price;
            }
        }

        /// <summary>
        ///  Category display commodity prices , Note that the minimum unit price is stored inside the database 
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public static decimal GetUnitPrice(string unit, decimal price)
        {
            //  To remove the bulk of the digital part 
            string sUnit = Regex.Replace(unit, @"[0-9]", "", RegexOptions.IgnoreCase);

            //  Quantities are by weight (g或pound), To display volume pricing 
            if (sUnit.Trim().ToLower() == "g" || sUnit.ToLower().IndexOf("pound") > 0)
            {
                //  Obtained in the digital part of the batch unit 
                decimal dUnit = GetUnitNum(unit);
                return decimal.Multiply(price, dUnit);
            }
            else
            {
                //  Individual quantities of commodity prices show the smallest unit 
                return price;
            }
        }
        ///   <summary>
        ///    To determine whether a string consisting of all of the English alphabet , Case returns true, Otherwise, it returns false   
        ///   </summary>   
        ///   <param   name="sQYMC"> String </param>   
        ///   <returns></returns>   
        public static bool IsZM(string sQYMC)
        {
            // The string into lowercase , In the array into a string    
            char[] cQYMC = sQYMC.ToLower().ToCharArray();

            // BP.DA.DBAccess.GenerOIDByGUID();

            // Iterate , And letters ascii Code comparison value , If there is a class is not in its scope , The non-Pinyin    
            //a--97,z--122   
            for (int i = 0; i < sQYMC.Length; i++)
            {
                if (!(cQYMC[i] >= 97 && cQYMC[i] <= 122))
                    return false;
            }
            return true;
        }

        #endregion
    }

}
