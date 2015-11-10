using System;
using System.Web;
using System.Text.RegularExpressions;
using System.Net;
using System.Text;
using System.IO;

namespace CCPortal.DA
{
    public class FormatProvider : IFormatProvider
    {
        #region IFormatProvider 成员
        public object GetFormat(Type formatType)
        {
            // TODO:  添加 Interfac.GetFormat 实现
            return null;
        }
        #endregion
    }
    /// <summary>
    /// DataType 的摘要说明。
    /// </summary>
    public class DataType
    {
        public static string TurnToJiDuByDataTime(string dt)
        {
            if (dt.Length <= 6)
                throw new Exception("@要转化季度的日期格式不正确:" + dt);
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
        /// <summary>
        /// 转换成MB
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
        /// 把内容里面的东西处理成超连接。
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
        /// 写文件
        /// </summary>
        /// <param name="file">路径</param>
        /// <param name="Doc">内容</param>
        public static void WriteFile(string file, string Doc)
        {
            System.IO.StreamWriter sr;
            if (System.IO.File.Exists(file))
                System.IO.File.Delete(file);

            //sr = new System.IO.StreamWriter(file, false, System.Text.Encoding.GetEncoding("GB2312"));
            sr = new System.IO.StreamWriter(file, false, System.Text.Encoding.UTF8);
            sr.Write(Doc);
            sr.Close();
        }
        /// <summary>
        /// 读取URL内容
        /// </summary>
        /// <param name="url">要读取的url</param>
        /// <param name="timeOut">超时时间</param>
        /// <param name="encode">text code.</param>
        /// <returns>返回读取内容</returns>
        public static string ReadURLContext(string url, int timeOut, Encoding encode)
        {
            HttpWebRequest webRequest = null;
            try
            {
                webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Timeout = timeOut;
                string str = webRequest.Address.AbsoluteUri;
                str = str.Substring(0, str.LastIndexOf("/"));
            }
            catch (Exception ex)
            {
                try
                {
                  //  CCPortal.DA.Log.DefaultLogWriteLineWarning("@读取URL出现错误:URL=" + url + "@错误信息：" + ex.Message);
                    return null;
                }
                catch
                {
                    return ex.Message;
                }
            }
            //	因为它返回的实例类型是WebRequest而不是HttpWebRequest,因此记得要进行强制类型转换
            //  接下来建立一个HttpWebResponse以便接收服务器发送的信息，它是调用HttpWebRequest.GetResponse来获取的：
            HttpWebResponse webResponse;
            try
            {
                webResponse = (HttpWebResponse)webRequest.GetResponse();
            }
            catch (Exception ex)
            {
                try
                {
                    // 如果出现死连接。
                   // CCPortal.DA.Log.DefaultLogWriteLineWarning("@获取url=" + url + "失败。异常信息:" + ex.Message, true);
                    return null;
                }
                catch
                {
                    return ex.Message;
                }
            }

            //如果webResponse.StatusCode的值为HttpStatusCode.OK，表示成功，那你就可以接着读取接收到的内容了：
            // 获取接收到的流
            Stream stream = webResponse.GetResponseStream();
            System.IO.StreamReader streamReader = new StreamReader(stream, encode);
            string content = streamReader.ReadToEnd();
            webResponse.Close();
            return content;
        }
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="file">路径</param>
        /// <returns>内容</returns>
        public static string ReadTextFile(string file)
        {
            System.IO.StreamReader read = new System.IO.StreamReader(file, System.Text.Encoding.UTF8); // 文件流.
            string doc = read.ReadToEnd();  //读取完毕。
            read.Close(); // 关闭。
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
        /// 判断是否全部是汉字
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

        #region 元角分
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
                    throw new Exception("没有涉及到这么大的金额播出");
            }

            //			strs=strs.Replace(",D0.TW,JIAO.TW,D0.TW,FEN.TW",""); // 替换掉 .0角0分;
            //			strs=strs.Replace("D0.TW,HUN.TW,D0.TW,TEN.TW","D0.TW"); // 替换掉 .0百0十 为 0 ;
            //			strs=strs.Replace("D0.TW,THOU.TW","D0.TW");  // 替换掉零千。
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
        /// 去掉周六日
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
        public static DateTime AddDays_Bak(DateTime dt, int days)
        {
            while (days > 0)
            {
                dt = dt.AddDays(1);
                if (dt.DayOfWeek == DayOfWeek.Sunday || dt.DayOfWeek == DayOfWeek.Saturday)
                    continue;
                days--;
            }
            return dt;
        }
        public static DateTime AddDays(string sysdt, int days)
        {
            DateTime dt = DataType.ParseSysDate2DateTime(sysdt);
            return AddDays(dt, days);
        }
        /// <summary>
        /// 取道百分比
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
                    return "星期日";
                case 1:
                    return "星期一";
                case 2:
                    return "星期二";
                case 3:
                    return "星期三";
                case 4:
                    return "星期四";
                case 5:
                    return "星期五";
                case 6:
                    return "星期六";
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
        /// 返回 data1 - data2 的天数.
        /// </summary>
        /// <param name="data1">fromday</param>
        /// <param name="data2">today</param>
        /// <returns>相隔的天数</returns>
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
        /// 返回 QuarterFrom - QuarterTo 的季度.
        /// </summary>
        /// <param name="QuarterFrom">QuarterFrom</param>
        /// <param name="QuarterTo">QuarterTo</param>
        /// <returns>相隔的季度</returns>
        public static int SpanQuarter(string _APFrom, string _APTo)
        {
            DateTime fromdate = Convert.ToDateTime(_APFrom + "-01");
            DateTime todate = Convert.ToDateTime(_APTo + "-01");
            int i = 0;
            if (fromdate > todate)
                throw new Exception("选择出错！起始时期" + _APFrom + "不能大于终止时期" + _APTo + "!");

            while (fromdate <= todate)
            {
                i++;
                fromdate = fromdate.AddMonths(1);
            }

            int j = (i + 2) / 3;
            return j;
        }
        /// <summary>
        /// 到现在的天数。
        /// </summary>
        /// <param name="data1"></param>
        /// <returns></returns>
        public static int SpanDays(string data1)
        {
            TimeSpan span = DateTime.Now - DateTime.Parse(data1.Substring(0, 10));
            return span.Days;
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
                    isStartRec = true; /* 开始记录 */
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

                    /* 开始分析这个标记内的东西。*/
                    string market = recStr.ToLower();
                    if (market.Contains("<img"))
                    {
                        /* 这是一个图片标记 */
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


            val = val.Replace("字体：大中小", "");
            val = val.Replace("字体:大中小", "");

            val = val.Replace("  ", " ");
            val = val.Replace("\t", "");
            val = val.Replace("\n", "");
            val = val.Replace("\r", "");
            return val;
        }
        
         
        /// <summary>
        /// 得到一个日期,根据系统
        /// </summary>
        /// <param name="dataStr"></param>
        /// <returns></returns>
        public DateTime Parse(string dataStr)
        {
            return DateTime.Parse(dataStr);
        }
        /// <summary>
        /// 系统定义的时间格式 yyyy-MM-dd .
        /// </summary>
        public static string SysDataFormat
        {
            get
            {
                return "yyyy-MM-dd";
            }
        }
        /// <summary>
        /// 当前的日期
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
        /// 给一个时间，返回一个刻种时间。
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
        /// 当前的会计期间
        /// </summary>
        public static string CurrentAP
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM");
            }
        }
        /// <summary>
        /// 当前的会计期间
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
        /// 当前的会计期间 yyyy-MM
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
                    throw new Exception("@不是有效的月份格式" + mm);
            }
            return jd;
        }
        /// <summary>
        /// 当前的季度期间yyyy-MM
        /// </summary>
        public static string CurrentAPOfJD
        {
            get
            {
                return DateTime.Now.ToString("yyyy") + "-" + DataType.GetJDByMM(DateTime.Now.ToString("MM"));
            }
        }
        /// <summary>
        /// 当前的季度的前一个季度.
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
        /// 取出当前月份的上一个月份
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
        /// 当前的季度期间
        /// </summary>
        public static string CurrentAPOfYear
        {
            get
            {
                return DateTime.Now.ToString("yyyy");
            }
        }
        /// <summary>
        /// 当前的日期时间
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
                switch ("CH")
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
                return DateTime.Now.ToString("yy年MM月dd日 HH时mm分");
            }
        }
        public static string CurrentDataCNOfShort
        {
            get
            {
                return DateTime.Now.ToString("yy年MM月dd日");
            }
        }
        public static string CurrentDataCNOfLong
        {
            get
            {
                return DateTime.Now.ToString("yyyy年MM月dd日");
            }
        }
        /// <summary>
        /// 当前的日期时间
        /// </summary>
        public static string CurrentDataTimeCN
        {
            get
            {
                return DateTime.Now.ToString(DataType.SysDataFormatCN) + "，" + GetWeekName(DateTime.Now.DayOfWeek);
            }
        }
        private static string GetWeekName(System.DayOfWeek dw)
        {
            switch (dw)
            {
                case DayOfWeek.Monday:
                    return "星期一";
                case DayOfWeek.Thursday:
                    return "星期四";
                case DayOfWeek.Friday:
                    return "星期五";
                case DayOfWeek.Saturday:
                    return "星期六";
                case DayOfWeek.Sunday:
                    return "星期日";
                case DayOfWeek.Tuesday:
                    return "星期二";
                case DayOfWeek.Wednesday:
                    return "星期三";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 当前的日期时间
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
                throw new Exception("日期格式错误:" + sysDateformat + " errorMsg=" + ex.Message);
            }
        }
        /// <summary>
        /// 把chichengsoft本系统日期格式转换为系统日期格式。
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
                throw new Exception("日期[" + sysDateformat + "]转换出现错误:" + ex.Message + "无效的日期是格式。");
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
                throw new Exception("@时间格式不正确:" + sysDateformat + "@技术信息:" + ex.Message);
            }
        }
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
        /// 到现在的时间
        /// </summary>
        /// <param name="fromdatetim"></param>
        /// <returns>分中数</returns>
        public static int GetSpanMinute(string fromdatetim)
        {
            DateTime dtfrom = DataType.ParseSysDateTime2DateTime(fromdatetim);
            DateTime dtto = DateTime.Now;
            TimeSpan ts = dtfrom - dtto;
            return ts.Minutes + ts.Hours * 60;

        }

        /// <summary>
        /// 系统定义日期时间格式 yyyy-MM-dd hh:mm
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
                return "yyyy年MM月dd日";
            }
        }
        public static string SysDatatimeFormatCN
        {
            get
            {
                return "yyyy年MM月dd日 HH时mm分";
            }
        }
        public static DBUrlType GetDBUrlByString(string strDBUrl)
        {
            switch (strDBUrl)
            {
                case "CCPortal.DSN":
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
                    throw new Exception("@没有此类型[" + strDBUrl + "]");
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
                    throw new Exception("@没有此类型" + datatype);
            }
        }
        public static string GetDataTypeDese(int datatype)
        {
            if ( 1==1)
            {
                switch (datatype)
                {
                    case DataType.AppBoolean:
                        return "布尔(Int)";
                    case DataType.AppDate:
                        return "日期NVARCHAR";
                    case DataType.AppDateTime:
                        return "日期时间NVARCHAR";
                    case DataType.AppDouble:
                        return "双精度(double)";
                    case DataType.AppFloat:
                        return "浮点(float)";
                    case DataType.AppInt:
                        return "整型(int)";
                    case DataType.AppMoney:
                        return "货币(float)";
                    case DataType.AppString:
                        return "字符(NVARCHAR)";
                    default:
                        throw new Exception("@没有此类型");
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
                    return "Varchar";
                default:
                    throw new Exception("@没有此类型");
            }

        }


        /// <summary>
        /// 产生适应的图片大小
        /// 用途:在固定容器大小的位置，显示固定的图片。
        /// </summary>
        /// <param name="height">容器高度</param>
        /// <param name="width">容器宽度</param>
        /// <param name="AdaptHeight">原始图片高度</param>
        /// <param name="AdaptWidth">原始图片宽度</param>
        /// <param name="isFull">是否填充:是,小图片将会放大填充容器. 否,小图片不放大保留原来的大小</param>
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


        #region datatype
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
        /// 率百分比。
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
        /// 判断是否是Num 字串。
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
        /// 是不时奇数
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
            else if (str == "是" || str == "否")
            {
                if (str == "否")
                    return false;
                else
                    return true;
            }
            else
                throw new Exception("@要转换的[" + str + "]不是bool 类型");
        }
    }

}
