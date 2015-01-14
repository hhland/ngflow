using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Data;
using BP.En;
using BP.DA;
using BP.Port;
using BP.Sys;

namespace BP.Pub
{
    public class RepBill : BP.DTS.DataIOEn
    {
        public RepBill()
        {
            this.Title = "WFV3.0 Documents automatically repair line .";
        }
        public override void Do()
        {
            string msg = "";
            string sql = "  SELECT * FROM WF_BillTemplate";
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                string file = SystemConfig.PathOfCyclostyleFile + dr["URL"].ToString() + ".rtf";
                msg += RepBill.RepairBill(file);
            }
            PubClass.ResponseWriteBlueMsg(msg);
        }
        public static string RepairBill(string file)
        {
            string msg = "";
            string docs;

            //  Read the file .
            try
            {
                StreamReader read = new StreamReader(file, System.Text.Encoding.ASCII); //  File stream .
                docs = read.ReadToEnd();  //  Reading is completed 
                read.Close(); //  Shut down 
            }
            catch (Exception ex)
            {
                return "@ An error occurred while reading the document template .cfile=" + file + " @Ex=" + ex.Message;
            }

            //  Repair .
            docs = RepairLineV2(docs);

            //  Write .
            try
            {
                StreamWriter mywr = new StreamWriter(file, false);
                mywr.Write(docs);
                mywr.Close();
            }
            catch (Exception ex)
            {
                return "@ An error occurred while writing the document template .cfile=" + file + " @Ex=" + ex.Message;
            }
            msg += "@ Invoice :[" + file + "] Successful repair .";
            return msg;
        }

        public static string RepairLine(string line)//str
        {
            char[] chs = line.ToCharArray();
            string str = "";
            foreach (char ch in chs)
            {
                if (ch == '\\')
                {
                    line = line.Replace("\\" + str, "");
                    str = "";
                }
                else if (ch == ' ')
                {
                    /*  If equal spaces ,  Direct replacement for the original  str */
                    line = line.Replace("\\" + str + ch, "");
                    str = "sssssssssssssssssss";
                }
                else
                    str += ch.ToString();
            }

            line = line.Replace("{", "");
            line = line.Replace("}", "");
            line = line.Replace("\r", "");
            line = line.Replace("\n", "");
            line = line.Replace(" ", "");
            line = line.Replace("..", ".");
            return line;
        }
        /// <summary>
        /// RepairLineV2
        /// </summary>
        /// <param name="docs"></param>
        /// <returns></returns>
        public static string RepairLineV2(string docs)//str
        {
            char[] chars = docs.ToCharArray();
            string strs = "";
            foreach (char c in chars)
            {
                if (c == '<')
                {
                    strs = "<";
                    continue;
                }
                if (c == '>')
                {
                    strs += c.ToString();
                    string line = strs.Clone().ToString();
                    line = RepairLine(line);
                    docs = docs.Replace(strs, line);
                    strs = "";
                    continue;
                }
                strs += c.ToString();
            }
            return docs;
        }
    }
    /// <summary>
    /// WebRtfReport  The summary .
    /// </summary>
    public class RTFEngine
    {
        #region  Data entities 
        private Entities _HisEns = null;
        public Entities HisEns
        {
            get
            {
                if (_HisEns == null)
                    _HisEns = new Emps();

                return _HisEns;
            }
        }
        #endregion  Data entities 

        #region  Data Details entity 
        private System.Text.Encoding _encoder = System.Text.Encoding.GetEncoding("GB2312");

        public string GetCode(string str)
        {
            if (str == "")
                return str;

            string rtn = "";
            byte[] rr = _encoder.GetBytes(str);
            foreach (byte b in rr)
            {
                if (b > 122)
                    rtn += "\\'" + b.ToString("x");
                else
                    rtn += (char)b;
            }
            return rtn.Replace("\n", " \\par ");
        }

        private ArrayList _EnsDataDtls = null;
        public ArrayList EnsDataDtls
        {
            get
            {
                if (_EnsDataDtls == null)
                    _EnsDataDtls = new ArrayList();
                return _EnsDataDtls;
            }
        }

        #endregion  Data Details entity 

        /// <summary>
        ///  Adding a data entity 
        /// </summary>
        /// <param name="en"></param>
        public void AddEn(Entity en)
        {
            this.HisEns.AddEntity(en);
        }
        /// <summary>
        ///  Adding a Ens
        /// </summary>
        /// <param name="ens"></param>
        public void AddDtlEns(Entities dtlEns)
        {
            this.EnsDataDtls.Add(dtlEns);
        }
        public string CyclostyleFilePath = "";
        public string TempFilePath = "";

        #region  Get special process node to process information .
        public string GetValueByKeyOfCheckNode(string[] strs)
        {
            foreach (Entity en in this.HisEns)
            {
                //if (en.ToString()=="BP.WF.NumCheck" || en.ToString()=="BP.WF.GECheckStand" || en.ToString()=="BP.WF.NoteWork"  )
                //{
                //    if (en.GetValStringByKey("NodeID")!=strs[1])
                //        continue;
                //}
                //else
                //{
                //    continue;
                //}

                string val = en.GetValStringByKey(strs[2]);
                switch (strs.Length)
                {
                    case 1:
                    case 2:
                        throw new Exception("step1 Parameter setting error " + strs.ToString());
                    case 3: // S.9001002.Rec
                        return val;
                    case 4: // S.9001002.RDT.Year
                        switch (strs[3])
                        {
                            case "Text":
                                if (val == "0")
                                    return "否";
                                else
                                    return "是";
                            case "YesNo":
                                if (val == "1")
                                    return "[√]";
                                else
                                    return "[×]";
                            case "Year":
                                return val.Substring(0, 4);
                            case "Month":
                                return val.Substring(5, 2);
                            case "Day":
                                return val.Substring(8, 2);
                            case "NYR":
                                return DA.DataType.ParseSysDate2DateTime(val).ToString("yyyy年MM月dd日");
                            case "RMB":
                                return float.Parse(val).ToString("0.00");
                            case "RMBDX":
                                return DA.DataType.ParseFloatToCash(float.Parse(val));
                            default:
                                throw new Exception("step2 Parameter setting error " + strs);
                        }
                    default:
                        throw new Exception("step3 Parameter setting error " + strs);
                }
            }
            throw new Exception("step4 Parameter setting error " + strs);
        }
        public static string GetImgHexString(System.Drawing.Image img, System.Drawing.Imaging.ImageFormat ext)
        {
            StringBuilder imgs = new StringBuilder();
            MemoryStream stream = new MemoryStream();
            img.Save(stream, ext);
            stream.Close();

            byte[] buffer = stream.ToArray();

            for (int i = 0; i < buffer.Length; i++)
            {
                if ((i % 32) == 0)
                {
                    imgs.AppendLine();
                }
                else if ((i % 8) == 0)
                {
                    imgs.Append(" ");
                }
                byte num2 = buffer[i];
                int num3 = (num2 & 240) >> 4;
                int num4 = num2 & 15;
                imgs.Append("0123456789abcdef"[num3]);
                imgs.Append("0123456789abcdef"[num4]);
            }
            return imgs.ToString();
        }
        public Entity HisGEEntity = null;
        /// <summary>
        ///  Get ICON Image data .
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValueImgStrs(string key)
        {
            key = key.Replace(" ", "");
            key = key.Replace("\r\n", "");

            /* Description is a picture file .*/
            string path = key.Replace("OID.Img@AppPath", SystemConfig.PathOfWebApp);

            // Definition rtf Pictures strings 
            StringBuilder pict = new StringBuilder();
            // Get the picture you want to insert 
            System.Drawing.Image img = System.Drawing.Image.FromFile(path);

            // Will convert the picture to be inserted 16 Hexadecimal string 
            string imgHexString;
            key = key.ToLower();

            if (key.Contains(".png"))
                imgHexString = GetImgHexString(img, System.Drawing.Imaging.ImageFormat.Png);
            else if (key.Contains(".jp"))
                imgHexString = GetImgHexString(img, System.Drawing.Imaging.ImageFormat.Jpeg);
            else if (key.Contains(".gif"))
                imgHexString = GetImgHexString(img, System.Drawing.Imaging.ImageFormat.Gif);
            else if (key.Contains(".ico"))
                imgHexString = GetImgHexString(img, System.Drawing.Imaging.ImageFormat.Icon);
            else
                imgHexString = GetImgHexString(img, System.Drawing.Imaging.ImageFormat.Jpeg);

            // Generate rtf Pictures strings 
            pict.AppendLine();
            pict.Append(@"{\pict");
            pict.Append(@"\jpegblip");
            pict.Append(@"\picscalex100");
            pict.Append(@"\picscaley100");
            pict.Append(@"\picwgoal" + img.Size.Width * 15);
            pict.Append(@"\pichgoal" + img.Size.Height * 15);
            pict.Append(imgHexString + "}");
            pict.AppendLine();
            return pict.ToString();
        }
        /// <summary>
        ///  Get M2M Data and outputs 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValueM2MStrs(string key)
        {
            string[] strs = key.Split('.');
            string sql = "SELECT ValsName FROM SYS_M2M WHERE FK_MapData='" + strs[0] + "' AND M2MNo='" + strs[2] + "' AND EnOID='" + this.HisGEEntity.PKVal + "'";
            string vals = DBAccess.RunSQLReturnStringIsNull(sql, null);
            if (vals == null)
                return " No data ";

            vals = vals.Replace("@", "  ");
            vals = vals.Replace("<font color=green>", "");
            vals = vals.Replace("</font>", "");
            return vals;


            string val = "";
            string[] objs = vals.Split('@');
            foreach (string obj in objs)
            {
                string[] noName = obj.Split(',');
                val += noName[1];
            }
            return val;
        }
        /// <summary>
        ///  Get writing version of the data 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValueBPPaintStrs(string key)
        {
            key = key.Replace(" ", "");
            key = key.Replace("\r\n", "");

            string[] strs = key.Split('.');
            string filePath = "";
            try
            {
                filePath = DBAccess.RunSQLReturnString("SELECT Tag2 From Sys_FrmEleDB WHERE RefPKVal=" + this.HisGEEntity.PKVal + " AND EleID='" + strs[2].Trim() + "'");
                if (filePath == null)
                    return "";
            }
            catch
            {
                return "";
            }

            // Definition rtf Pictures strings 
            StringBuilder pict = new StringBuilder();
            // Get the picture you want to insert 
            System.Drawing.Image img = System.Drawing.Image.FromFile(filePath);

            // Will convert the picture to be inserted 16 Hexadecimal string 
            string imgHexString;
            filePath = filePath.ToLower();

            if (filePath.Contains(".png"))
                imgHexString = GetImgHexString(img, System.Drawing.Imaging.ImageFormat.Png);
            else if (filePath.Contains(".jp"))
                imgHexString = GetImgHexString(img, System.Drawing.Imaging.ImageFormat.Jpeg);
            else if (filePath.Contains(".gif"))
                imgHexString = GetImgHexString(img, System.Drawing.Imaging.ImageFormat.Gif);
            else if (filePath.Contains(".ico"))
                imgHexString = GetImgHexString(img, System.Drawing.Imaging.ImageFormat.Icon);
            else
                imgHexString = GetImgHexString(img, System.Drawing.Imaging.ImageFormat.Jpeg);

            // Generate rtf Pictures strings 
            pict.AppendLine();
            pict.Append(@"{\pict");
            pict.Append(@"\jpegblip");
            pict.Append(@"\picscalex100");
            pict.Append(@"\picscaley100");
            pict.Append(@"\picwgoal" + img.Size.Width * 15);
            pict.Append(@"\pichgoal" + img.Size.Height * 15);
            pict.Append(imgHexString + "}");
            pict.AppendLine();
            return pict.ToString();
        }
        /// <summary>
        ///  Get the class name +@+ Data field format .
        ///  Such as :
        /// Demo_Inc@ABC
        /// Emp@Name
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValueByAtKey(string key)
        {
            foreach (Entity en in this.HisEns)
            {
                string enKey = en.ToString();

                // There may be  BP.Port.Emp
                if (enKey.Contains("."))
                    enKey = en.GetType().Name;

                // If you do not include .
                if (key.Contains(enKey + "@") == false)
                    continue;

                //  If you do not include  .  It shows , Do not need to turn back .
                if (key.Contains(".") == false)
                    return en.GetValStringByKey(key.Substring(key.IndexOf('@') + 1));

                // The entity name removed 
                key = key.Replace(enKey + "@", "");
                // Break open data .
                string[] strs = key.Split('.');
                if (strs.Length == 2)
                {
                    if (strs[1].Trim() == "ImgAth")
                    {
                        string path1 = BP.Sys.SystemConfig.PathOfDataUser + "\\ImgAth\\Data\\" + strs[0].Trim() + "_" + en.PKVal + ".png";
                        // Definition rtf Pictures strings .
                        StringBuilder mypict = new StringBuilder();
                        // Get the picture you want to insert 
                        System.Drawing.Image imgAth = System.Drawing.Image.FromFile(path1);

                        // Will convert the picture to be inserted 16 Hexadecimal string 
                        string imgHexStringImgAth = GetImgHexString(imgAth, System.Drawing.Imaging.ImageFormat.Jpeg);
                        // Generate rtf Pictures strings 
                        mypict.AppendLine();
                        mypict.Append(@"{\pict");
                        mypict.Append(@"\jpegblip");
                        mypict.Append(@"\picscalex100");
                        mypict.Append(@"\picscaley100");
                        mypict.Append(@"\picwgoal" + imgAth.Size.Width * 15);
                        mypict.Append(@"\pichgoal" + imgAth.Size.Height * 15);
                        mypict.Append(imgHexStringImgAth + "}");
                        mypict.AppendLine();
                        return mypict.ToString();
                    }

                    string val = en.GetValStringByKey(strs[0].Trim());
                    switch (strs[1].Trim())
                    {
                        case "Text":
                            if (val == "0")
                                return "否";
                            else
                                return "是";
                        case "Year":
                            return val.Substring(0, 4);
                        case "Month":
                            return val.Substring(5, 2);
                        case "Day":
                            return val.Substring(8, 2);
                        case "NYR":
                            return DA.DataType.ParseSysDate2DateTime(val).ToString("yyyy年MM月dd日");
                        case "RMB":
                            return float.Parse(val).ToString("0.00");
                        case "RMBDX":
                            return DA.DataType.ParseFloatToCash(float.Parse(val));
                        case "ImgAth":
                            string path1 = BP.Sys.SystemConfig.PathOfDataUser + "\\ImgAth\\Data\\" + strs[0].Trim() + "_" + this.HisGEEntity.PKVal + ".png";

                            // Definition rtf Pictures strings .
                            StringBuilder mypict = new StringBuilder();
                            // Get the picture you want to insert 
                            System.Drawing.Image imgAth = System.Drawing.Image.FromFile(path1);

                            // Will convert the picture to be inserted 16 Hexadecimal string 
                            string imgHexStringImgAth = GetImgHexString(imgAth, System.Drawing.Imaging.ImageFormat.Jpeg);
                            // Generate rtf Pictures strings 
                            mypict.AppendLine();
                            mypict.Append(@"{\pict");
                            mypict.Append(@"\jpegblip");
                            mypict.Append(@"\picscalex100");
                            mypict.Append(@"\picscaley100");
                            mypict.Append(@"\picwgoal" + imgAth.Size.Width * 15);
                            mypict.Append(@"\pichgoal" + imgAth.Size.Height * 15);
                            mypict.Append(imgHexStringImgAth + "}");
                            mypict.AppendLine();
                            return mypict.ToString();
                        case "Siganture":
                            string path = BP.Sys.SystemConfig.PathOfDataUser + "\\Siganture\\" + val + ".jpg";
                            // Definition rtf Pictures strings .
                            StringBuilder pict = new StringBuilder();
                            // Get the picture you want to insert 
                            System.Drawing.Image img = System.Drawing.Image.FromFile(path);

                            // Will convert the picture to be inserted 16 Hexadecimal string 
                            string imgHexString = GetImgHexString(img, System.Drawing.Imaging.ImageFormat.Jpeg);
                            // Generate rtf Pictures strings 
                            pict.AppendLine();
                            pict.Append(@"{\pict");
                            pict.Append(@"\jpegblip");
                            pict.Append(@"\picscalex100");
                            pict.Append(@"\picscaley100");
                            pict.Append(@"\picwgoal" + img.Size.Width * 15);
                            pict.Append(@"\pichgoal" + img.Size.Height * 15);
                            pict.Append(imgHexString + "}");
                            pict.AppendLine();
                            return pict.ToString();
                        // Replace rtf Template file signature graphic identity for the picture string 
                        // str = str.Replace(imgMark, pict.ToString());
                        default:
                            throw new Exception(" Parameter setting error , Error values in a special way :" + key);
                    }
                }
            } //  Entity cycle .

            throw new Exception(" Parameter setting error  GetValueByKey :" + key);

        }
        /// <summary>
        ///  Audit node representation is   Node ID.Attr.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValueByKey(string key)
        {
            key = key.Replace(" ", "");
            key = key.Replace("\r\n", "");

            if (key.Contains("@"))
                return GetValueByAtKey(key);

            string[] strs = key.Split('.');

            //  If you do not include  .  It shows that he is from Rpt Fetch data .
            if (this.HisGEEntity != null && key.Contains("ND") == false)
            {
                if (strs.Length == 1)
                    return this.HisGEEntity.GetValStringByKey(key);

                if (strs[1].Trim() == "ImgAth")
                {
                    string path1 = BP.Sys.SystemConfig.PathOfDataUser + "\\ImgAth\\Data\\" + strs[0].Trim() + "_" + this.HisGEEntity.PKVal + ".png";

                    // Definition rtf Pictures strings .
                    StringBuilder mypict = new StringBuilder();
                    // Get the picture you want to insert 
                    System.Drawing.Image imgAth = System.Drawing.Image.FromFile(path1);

                    // Will convert the picture to be inserted 16 Hexadecimal string 
                    string imgHexStringImgAth = GetImgHexString(imgAth, System.Drawing.Imaging.ImageFormat.Jpeg);
                    // Generate rtf Pictures strings 
                    mypict.AppendLine();
                    mypict.Append(@"{\pict");
                    mypict.Append(@"\jpegblip");
                    mypict.Append(@"\picscalex100");
                    mypict.Append(@"\picscaley100");
                    mypict.Append(@"\picwgoal" + imgAth.Size.Width * 15);
                    mypict.Append(@"\pichgoal" + imgAth.Size.Height * 15);
                    mypict.Append(imgHexStringImgAth + "}");
                    mypict.AppendLine();
                    return mypict.ToString();
                }

                if (strs[1].Trim() == "BPPaint")
                {
                    string path1 = DBAccess.RunSQLReturnString("SELECT  Tag2 FROM Sys_FrmEleDB WHERE REFPKVAL=" + this.HisGEEntity.PKVal + " AND EleID='" + strs[0].Trim() + "'");
                    //  string path1 = BP.Sys.SystemConfig.PathOfDataUser + "\\BPPaint\\" + this.HisGEEntity.ToString().Trim() + "\\" + this.HisGEEntity.PKVal + ".png";
                    // Definition rtf Pictures strings .
                    StringBuilder mypict = new StringBuilder();
                    // Get the picture you want to insert 
                    System.Drawing.Image myBPPaint = System.Drawing.Image.FromFile(path1);

                    // Will convert the picture to be inserted 16 Hexadecimal string 
                    string imgHexStringImgAth = GetImgHexString(myBPPaint, System.Drawing.Imaging.ImageFormat.Jpeg);
                    // Generate rtf Pictures strings 
                    mypict.AppendLine();
                    mypict.Append(@"{\pict");
                    mypict.Append(@"\jpegblip");
                    mypict.Append(@"\picscalex100");
                    mypict.Append(@"\picscaley100");
                    mypict.Append(@"\picwgoal" + myBPPaint.Size.Width * 15);
                    mypict.Append(@"\pichgoal" + myBPPaint.Size.Height * 15);
                    mypict.Append(imgHexStringImgAth + "}");
                    mypict.AppendLine();
                    return mypict.ToString();
                }


                if (strs.Length == 2)
                {
                    string val = this.HisGEEntity.GetValStringByKey(strs[0].Trim());
                    switch (strs[1].Trim())
                    {

                        case "Text":
                            if (val == "0")
                                return "否";
                            else
                                return "是";
                        case "Year":
                            return val.Substring(0, 4);
                        case "Month":
                            return val.Substring(5, 2);
                        case "Day":
                            return val.Substring(8, 2);
                        case "NYR":
                            return DA.DataType.ParseSysDate2DateTime(val).ToString("yyyy年MM月dd日");
                        case "RMB":
                            return float.Parse(val).ToString("0.00");
                        case "RMBDX":
                            return DA.DataType.ParseFloatToCash(float.Parse(val));
                        case "Siganture":
                            string path = BP.Sys.SystemConfig.PathOfDataUser + "\\Siganture\\" + val + ".jpg";
                            // Definition rtf Pictures strings 
                            StringBuilder pict = new StringBuilder();
                            // Get the picture you want to insert 
                            System.Drawing.Image img = System.Drawing.Image.FromFile(path);

                            // Will convert the picture to be inserted 16 Hexadecimal string 
                            string imgHexString = GetImgHexString(img, System.Drawing.Imaging.ImageFormat.Jpeg);
                            // Generate rtf Pictures strings 
                            pict.AppendLine();
                            pict.Append(@"{\pict");
                            pict.Append(@"\jpegblip");
                            pict.Append(@"\picscalex100");
                            pict.Append(@"\picscaley100");
                            pict.Append(@"\picwgoal" + img.Size.Width * 15);
                            pict.Append(@"\pichgoal" + img.Size.Height * 15);
                            pict.Append(imgHexString + "}");
                            pict.AppendLine();
                            return pict.ToString();
                        // Replace rtf Template file signature graphic identity for the picture string 
                        // str = str.Replace(imgMark, pict.ToString());
                        case "YesNo":
                            if (val == "1")
                                return "[√]";
                            else
                                return "[×]";
                            break;
                        case "Yes":
                            if (val == "0")
                                return "[×]";
                            else
                                return "[√]";
                        case "No":
                            if (val == "0")
                                return "[√]";
                            else
                                return "[×]";
                        default:
                            throw new Exception(" Parameter setting error , Error values in a special way :" + key);
                    }
                }
                else
                {
                    throw new Exception(" Parameter setting error , Error values in a special way :" + key);
                }
            }


            throw new Exception(" Parameter setting error  GetValueByKey :" + key);
        }
        #endregion

        #region  Generate documents 
        /// <summary>
        ///  Generate documents 
        /// </summary>
        /// <param name="cfile"> Template files </param>
        public void MakeDoc(string cfile, string replaceVal)
        {
            string file = PubClass.GenerTempFileName("doc");
            this.MakeDoc(cfile, SystemConfig.PathOfTemp, file, replaceVal, true);
        }
        public string ensStrs = "";
        /// <summary>
        ///  Document Generation  
        /// </summary>
        /// <param name="cfile"> Template files </param>
        /// <param name="path"> Build path </param>
        /// <param name="file"> Makefile </param>
        /// <param name="isOpen"> Whether to use IE Turn on ?</param>
        public void MakeDoc(string cfile, string path, string file, string replaceVals, bool isOpen)
        {
            string str = Cash.GetBillStr(cfile, false).Substring(0);
            if (this.HisEns.Count == 0)
                if (this.HisGEEntity == null)
                    throw new Exception("@ You do not set the data source for the report ...");

            this.ensStrs = "";
            if (this.HisEns.Count != 0)
            {
                foreach (Entity en in this.HisEns)
                    ensStrs += en.ToString();
            }
            else
            {
                ensStrs = this.HisGEEntity.ToString();
            }

            string error = "";
            string[] paras = null;
            if (this.HisGEEntity != null)
                paras = Cash.GetBillParas(cfile, ensStrs, this.HisGEEntity);
            else
                paras = Cash.GetBillParas(cfile, ensStrs, this.HisEns);

            this.TempFilePath = path + file;
            try
            {
                string key = "";
                string ss = "";

                #region  Replace the main table tag 
                foreach (string para in paras)
                {
                    if (para == null || para == "")
                        continue;
                    try
                    {
                        if (para.Contains("ImgAth"))
                            str = str.Replace("<" + para + ">", this.GetValueByKey(para));
                        else if (para.Contains("Siganture"))
                            str = str.Replace("<" + para + ">", this.GetValueByKey(para));
                        else if (para.Contains("Img@AppPath"))
                            str = str.Replace("<" + para + ">", this.GetValueImgStrs(para));
                        else if (para.Contains(".BPPaint"))
                            str = str.Replace("<" + para + ">", this.GetValueBPPaintStrs(para));
                        else if (para.Contains(".M2M"))
                            str = str.Replace("<" + para + ">", this.GetValueM2MStrs(para));
                        else if (para.Contains(".RMB"))
                            str = str.Replace("<" + para + ">", this.GetValueByKey(para));
                        else if (para.Contains(".RMBDX"))
                            str = str.Replace("<" + para + ">", this.GetValueByKey(para));
                        else if (para.Contains(".NYR"))
                            str = str.Replace("<" + para + ">", this.GetCode(this.GetValueByKey(para)));
                        else if (para.Contains(".") == true)
                            continue; /* There may be a list of data .*/
                        else
                            str = str.Replace("<" + para + ">", this.GetCode(this.GetValueByKey(para)));
                    }
                    catch (Exception ex)
                    {
                        error += " Replace the main table tags take parameters [" + para + "] Error : There are circumstances that cause this error ;1 You use Text The value of time , This property is not a foreign key .2, No such attribute class .3, This field is a list field, but lost the schedule mark .<br> More information :<br>" + ex.Message;
                        if (SystemConfig.IsDebug)
                            throw new Exception(error);
                        Log.DebugWriteError(error);
                    }
                }
                #endregion  Replace the main table tag 

                #region  From Table 
                string shortName = "";
                ArrayList al = this.EnsDataDtls;
                foreach (Entities dtls in al)
                {
                    Entity dtl = dtls.GetNewEntity;
                    string dtlEnName = dtl.ToString();
                    shortName = dtlEnName.Substring(dtlEnName.LastIndexOf(".") + 1);

                    if (str.IndexOf(shortName) == -1)
                        continue;

                    int pos_rowKey = str.IndexOf(shortName);
                    int row_start = -1, row_end = -1;
                    if (pos_rowKey != -1)
                    {
                        row_start = str.Substring(0, pos_rowKey).LastIndexOf("\\row");
                        row_end = str.Substring(pos_rowKey).IndexOf("\\row");
                    }

                    if (row_start != -1 && row_end != -1)
                    {
                        string row = str.Substring(row_start, (pos_rowKey - row_start) + row_end);
                        str = str.Replace(row, "");

                        Map map = dtls.GetNewEntity.EnMap;
                        int i = dtls.Count;
                        while (i > 0)
                        {
                            i--;
                            string rowData = row.Clone() as string;
                            dtl = dtls[i];
                            foreach (Attr attr in map.Attrs)
                            {
                                switch (attr.MyDataType)
                                {
                                    case DataType.AppDouble:
                                    case DataType.AppFloat:
                                    case DataType.AppRate:
                                        rowData = rowData.Replace("<" + shortName + "." + attr.Key + ">", dtl.GetValStringByKey(attr.Key));
                                        break;
                                    case DataType.AppMoney:
                                        rowData = rowData.Replace("<" + shortName + "." + attr.Key + ">", dtl.GetValDecimalByKey(attr.Key).ToString("0.00"));
                                        break;
                                    case DataType.AppInt:

                                        if (attr.MyDataType == DataType.AppBoolean)
                                        {
                                            rowData = rowData.Replace("<" + shortName + "." + attr.Key + ">", dtl.GetValStrByKey(attr.Key));
                                            int v = dtl.GetValIntByKey(attr.Key);
                                            if (v == 1)
                                                rowData = rowData.Replace("<" + shortName + "." + attr.Key + "Text>", "是");
                                            else
                                                rowData = rowData.Replace("<" + shortName + "." + attr.Key + "Text>", "否");
                                        }
                                        else
                                        {
                                            if (attr.IsEnum)
                                                rowData = rowData.Replace("<" + shortName + "." + attr.Key + "Text>", GetCode(dtl.GetValRefTextByKey(attr.Key)));
                                            else
                                                rowData = rowData.Replace("<" + shortName + "." + attr.Key + ">", dtl.GetValStrByKey(attr.Key));
                                        }
                                        break;
                                    default:
                                        rowData = rowData.Replace("<" + shortName + "." + attr.Key + ">", GetCode(dtl.GetValStrByKey(attr.Key)));
                                        break;
                                }
                            }

                            str = str.Insert(row_start, rowData);
                        }
                    }
                }
                #endregion  From Table 

                #region  Details   Total Information .
                al = this.EnsDataDtls;
                foreach (Entities dtls in al)
                {
                    Entity dtl = dtls.GetNewEntity;
                    string dtlEnName = dtl.ToString();
                    shortName = dtlEnName.Substring(dtlEnName.LastIndexOf(".") + 1);
                    //shortName = dtls.ToString().Substring(dtls.ToString().LastIndexOf(".") + 1);
                    Map map = dtl.EnMap;
                    foreach (Attr attr in map.Attrs)
                    {
                        switch (attr.MyDataType)
                        {
                            case DataType.AppDouble:
                            case DataType.AppFloat:
                            case DataType.AppMoney:
                            case DataType.AppRate:
                                key = "<" + shortName + "." + attr.Key + ".SUM>";
                                if (str.IndexOf(key) != -1)
                                    str = str.Replace(key, dtls.GetSumFloatByKey(attr.Key).ToString());

                                key = "<" + shortName + "." + attr.Key + ".SUM.RMB>";
                                if (str.IndexOf(key) != -1)
                                    str = str.Replace(key, dtls.GetSumFloatByKey(attr.Key).ToString("0.00"));

                                key = "<" + shortName + "." + attr.Key + ".SUM.RMBDX>";
                                if (str.IndexOf(key) != -1)
                                    str = str.Replace(key,
                                        GetCode(DA.DataType.ParseFloatToCash(dtls.GetSumFloatByKey(attr.Key))));
                                break;
                            case DataType.AppInt:
                                key = "<" + shortName + "." + attr.Key + ".SUM>";
                                if (str.IndexOf(key) != -1)
                                    str = str.Replace(key, dtls.GetSumIntByKey(attr.Key).ToString());
                                break;
                            default:
                                break;
                        }
                    }
                }
                #endregion  Total from the table 

                #region  The field to be replaced 
                if (replaceVals != null && replaceVals.Contains("@"))
                {
                    string[] vals = replaceVals.Split('@');
                    foreach (string val in vals)
                    {
                        if (val == null || val == "")
                            continue;

                        if (val.Contains("=") == false)
                            continue;

                        string myRep = val.Clone() as string;

                        myRep = myRep.Trim();
                        myRep = myRep.Replace("null", "");
                        string[] myvals = myRep.Split('=');
                        str = str.Replace("<" + myvals[0] + ">", "<" + myvals[1] + ">");
                    }
                }
                #endregion

                StreamWriter wr = new StreamWriter(this.TempFilePath, false, Encoding.ASCII);
                str = str.Replace("<", "");
                str = str.Replace(">", "");
                wr.Write(str);
                wr.Close();
            }
            catch (Exception ex)
            {
                string msg = "";
                if (SystemConfig.IsDebug)
                {  //  Abnormalities may be related to the configuration of the documents .
                    try
                    {
                        this.CyclostyleFilePath = SystemConfig.PathOfDataUser + "\\CyclostyleFile\\" + cfile;
                        str = Cash.GetBillStr(cfile, false);
                        string s = RepBill.RepairBill(this.CyclostyleFilePath);
                        msg = "@ Has successfully perform the repair line   RepairLineV2, You re-sent once or , Once again sent back , Whether you can solve this problem .@" + s;
                    }
                    catch (Exception ex1)
                    {
                        msg = " Failure to perform the repair line .  RepairLineV2 " + ex1.Message;
                    }
                }
                throw new Exception(" Failed to generate documentation : Documents Name [" + this.CyclostyleFilePath + "]  Exception Information :" + ex.Message + " @ Auto repair information documents :" + msg);
            }
            if (isOpen)
                PubClass.Print(BP.Sys.Glo.Request.ApplicationPath + "Temp/" + file);
        }
        #endregion


        #region  Generate documents 
        #region  Generate documents 
        /// <summary>
        ///  According to generate documents 
        /// </summary>
        /// <param name="templeteFile"> Template files </param>
        /// <param name="saveToFile"></param>
        /// <param name="mainDT"></param>
        /// <param name="dtls"></param>
        public void MakeDocByDataSet(string templeteFile, string saveToPath,
            string saveToFileName, DataTable mainDT, DataSet dtlsDS)
        {
            string valMain = DBAccess.RunSQLReturnString("SELECT NO FROM SYS_MapData");
            this.HisGEEntity = new GEEntity(valMain);
            this.HisGEEntity.Row.LoadDataTable(mainDT, mainDT.Rows[0]);
            this.AddEn(this.HisGEEntity); // Add a main table .
            if (dtlsDS != null)
            {
                foreach (DataTable dt in dtlsDS.Tables)
                {
                    string dtlID = DBAccess.RunSQLReturnString("SELECT NO FROM SYS_MapDtl ");
                    BP.Sys.GEDtls dtls = new BP.Sys.GEDtls(dtlID);
                    foreach (DataRow dr in dt.Rows)
                    {
                        BP.Sys.GEDtl dtl = dtls.GetNewEntity as BP.Sys.GEDtl;
                        dtl.Row.LoadDataTable(dt, dr);
                        dtls.AddEntity(dtl);
                    }
                    this.AddDtlEns(dtls); // A clear increase .
                }
            }

            this.MakeDoc(templeteFile, saveToPath, saveToFileName, "", false);
        }
        #endregion
        #endregion

        #region  Method 
        /// <summary>
        /// RTFEngine
        /// </summary>
        public RTFEngine()
        {
            this._EnsDataDtls = null;
            this._HisEns = null;
        }
        /// <summary>
        ///  Repair line 
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>


        #endregion
    }


}
