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
 
namespace BP.Sys
{
	/// <summary>
	/// PageBase  The summary .
	/// </summary>
	public class PubClass 
	{
        /// <summary>
        ///  Send e-mail 
        /// </summary>
        /// <param name="maillAddr"> Address </param>
        /// <param name="title"> Title </param>
        /// <param name="doc"> Content </param>
        public static void SendMail(string maillAddr, string title, string doc)
        {
            System.Net.Mail.MailMessage myEmail = new System.Net.Mail.MailMessage();
            myEmail.From = new System.Net.Mail.MailAddress("ccflow.cn@gmail.com", "ccflow", System.Text.Encoding.UTF8);

            myEmail.To.Add(maillAddr);
            myEmail.Subject = title;
            myEmail.SubjectEncoding = System.Text.Encoding.UTF8;// Mail header encoding 

            myEmail.Body = doc;
            myEmail.BodyEncoding = System.Text.Encoding.UTF8;// Mail content encoding 
            myEmail.IsBodyHtml = true;// Is HTML Mail 

            myEmail.Priority = MailPriority.High;// Priority Mail 

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(SystemConfig.GetValByKey("SendEmailAddress", "ccflow.cn@gmail.com"),
                SystemConfig.GetValByKey("SendEmailPass", "ccflow123"));

            // Said write your email and password 
            client.Port = SystemConfig.GetValByKeyInt("SendEmailPort", 587); // Ports Used 
            client.Host = SystemConfig.GetValByKey("SendEmailHost", "smtp.gmail.com");
            client.EnableSsl = true; // After ssl Encryption .
            object userState = myEmail;
            try
            {
                client.Send(myEmail);

             //   client.SendAsync(myEmail, userState);
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                throw ex;
            }
        }
        public static string ToHtmlColor(string colorName)
        {
            try
            {
                if (colorName.StartsWith("#"))
                    colorName = colorName.Replace("#", string.Empty);
                int v = int.Parse(colorName, System.Globalization.NumberStyles.HexNumber);

                Color col = Color.FromArgb
               (
                     Convert.ToByte((v >> 24) & 255),
                     Convert.ToByte((v >> 16) & 255),
                     Convert.ToByte((v >> 8) & 255),
                     Convert.ToByte((v >> 0) & 255)
                );

                int alpha = col.A;
                var red = Convert.ToString(col.R, 16); ;
                var green = Convert.ToString(col.G, 16);
                var blue = Convert.ToString(col.B, 16);
                return string.Format("#{0}{1}{2}", red, green, blue);
            }
            catch
            {
                return "black";
            }
        }
        public static void InitFrm(string fk_mapdata)
        {
            //  Deleting Data .
            FrmLabs labs = new FrmLabs();
            labs.Delete(FrmLabAttr.FK_MapData, fk_mapdata);

            FrmLines lines = new FrmLines();
            lines.Delete(FrmLabAttr.FK_MapData, fk_mapdata);

            MapData md = new MapData();
            md.No = fk_mapdata;
            if (md.RetrieveFromDBSources() == 0)
            {
                MapDtl mdtl = new MapDtl();
                mdtl.No = fk_mapdata;
                if (mdtl.RetrieveFromDBSources() == 0)
                {
                    throw new Exception("@:" + fk_mapdata+" Mapping information does not exist .");
                }
                else
                {
                    md.Copy(mdtl);
                }
            }

            MapAttrs mattrs = new MapAttrs(fk_mapdata);
            GroupFields gfs = new GroupFields(fk_mapdata);

            int tableW = 700;
            int padingLeft = 3;
            int leftCtrlX = 700 / 100 * 20;
            int rightCtrlX = 700 / 100 * 60;

            string keyID = DateTime.Now.ToString("yyMMddhhmmss");
            // table  Title .
            int currX = 0;
            int currY = 0;
            FrmLab lab = new FrmLab();
            lab.Text = md.Name;
            lab.FontSize = 20;
            lab.X = 200;
            currY += 30;
            lab.Y = currY;
            lab.FK_MapData = fk_mapdata;
            lab.FontWeight = "Bold";
            lab.MyPK = "Lab"+keyID + "1";
            lab.Insert();

            //  The head of the table horizontal .
            currY += 20;
            FrmLine lin = new FrmLine();
            lin.X1 = 0;
            lin.X2 = tableW;
            lin.Y1 = currY;
            lin.Y2 = currY;
            lin.BorderWidth = 2;
            lin.FK_MapData = fk_mapdata;
            lin.MyPK = "Lin"+keyID + "1";
            lin.Insert();
            currY += 5;

            bool isLeft = false;
            int i = 2;
            foreach (GroupField gf in gfs)
            {
                i++;
                lab = new FrmLab();
                lab.X = 0;
                lab.Y = currY;
                lab.Text = gf.Lab;
                lab.FK_MapData = fk_mapdata;
                lab.FontWeight = "Bold";
                lab.MyPK = "Lab" + keyID + i.ToString();
                lab.Insert();

                currY += 15;
                lin = new FrmLine();
                lin.X1 = padingLeft;
                lin.X2 = tableW;
                lin.Y1 = currY;
                lin.Y2 = currY;
                lin.FK_MapData = fk_mapdata;
                lin.BorderWidth = 3;
                lin.MyPK = "Lin" + keyID + i.ToString();
                lin.Insert();

                isLeft = true;
                int idx = 0;
                foreach (MapAttr attr in mattrs)
                {
                    if (gf.OID != attr.GroupID || attr.UIVisible == false)
                        continue;

                    idx++;
                    if (isLeft)
                    {
                        lin = new FrmLine();
                        lin.X1 = 0;
                        lin.X2 = tableW;
                        lin.Y1 = currY;
                        lin.Y2 = currY;
                        lin.FK_MapData = fk_mapdata;
                        lin.MyPK = "Lin" + keyID + i.ToString() + idx;
                        lin.Insert();
                        currY += 14; /*  Draw a horizontal line  .*/

                        lab = new FrmLab();
                        lab.X = lin.X1 + padingLeft;
                        lab.Y = currY;
                        lab.Text = attr.Name;
                        lab.FK_MapData = fk_mapdata;
                        lab.MyPK = "Lab" + keyID + i.ToString() + idx;
                        lab.Insert();

                        lin = new FrmLine();
                        lin.X1 = leftCtrlX;
                        lin.Y1 = currY - 14;

                        lin.X2 = leftCtrlX;
                        lin.Y2 = currY;
                        lin.FK_MapData = fk_mapdata;
                        lin.MyPK = "Lin" + keyID + i.ToString() + idx + "R";
                        lin.Insert(); /* Draw a   Vertical line  */

                        attr.X = leftCtrlX + padingLeft;
                        attr.Y = currY - 3;
                        attr.UIWidth = 150;
                        attr.Update();
                        currY += 14;
                    }
                    else
                    {
                        currY = currY - 14;
                        lab = new FrmLab();
                        lab.X = tableW / 2 + padingLeft;
                        lab.Y = currY;
                        lab.Text = attr.Name;
                        lab.FK_MapData = fk_mapdata;
                        lab.MyPK = "Lab" + keyID + i.ToString() + idx;
                        lab.Insert();

                        lin = new FrmLine();
                        lin.X1 = tableW / 2;
                        lin.Y1 = currY - 14;

                        lin.X2 = tableW / 2;
                        lin.Y2 = currY;
                        lin.FK_MapData = fk_mapdata;
                        lin.MyPK = "Lin" + keyID + i.ToString() + idx;
                        lin.Insert(); /* Draw a   Vertical line  */

                        lin = new FrmLine();
                        lin.X1 = rightCtrlX;
                        lin.Y1 = currY - 14;
                        lin.X2 = rightCtrlX;
                        lin.Y2 = currY;
                        lin.FK_MapData = fk_mapdata;
                        lin.MyPK = "Lin" + keyID + i.ToString() + idx + "R";
                        lin.Insert(); /* Draw a   Vertical line  */

                        attr.X = rightCtrlX + padingLeft;
                        attr.Y = currY - 3;
                        attr.UIWidth = 150;
                        attr.Update();
                        currY += 14;
                    }
                    isLeft = !isLeft;
                }
            }
            // table bottom line.
            lin = new FrmLine();
            lin.X1 = 0;
            lin.Y1 = currY;

            lin.X2 = tableW;
            lin.Y2 = currY;
            lin.FK_MapData = fk_mapdata;
            lin.BorderWidth = 3;
            lin.MyPK = "Lin" + keyID + "eR";
            lin.Insert();

            currY = currY - 28 - 18;
            //  Handle end . table left line
            lin = new FrmLine();
            lin.X1 = 0;
            lin.Y1 = 50;
            lin.X2 = 0;
            lin.Y2 = currY;
            lin.FK_MapData = fk_mapdata;
            lin.BorderWidth = 3;
            lin.MyPK = "Lin" + keyID + "eRr";
            lin.Insert();

            // table right line.
            lin = new FrmLine();
            lin.X1 = tableW;
            lin.Y1 = 50;
            lin.X2 = tableW;
            lin.Y2 = currY;
            lin.FK_MapData = fk_mapdata;
            lin.BorderWidth = 3;
            lin.MyPK = "Lin" + keyID + "eRr4";
            lin.Insert();
        }
        public static String ColorToStr(System.Drawing.Color color)
        {
            try
            {
                string color_s = System.Drawing.ColorTranslator.ToHtml(color);
                color_s = color_s.Substring(1, color_s.Length - 1);
                return "#" + Convert.ToString(Convert.ToInt32(color_s, 16) + 40000, 16);
            }
            catch
            {
                return "black";
            }
        }
        /// <summary>
        ///  Processing field 
        /// </summary>
        /// <param name="fd"></param>
        /// <returns></returns>
        public static string DealToFieldOrTableNames(string fd)
        {
            string keys = "~!@#$%^&*()+{}|:<>?`=[];,./¡«!£À£££¤£¥¡­¡­£¦¡Á£¨£©¡ª¡ª£«£û£ý£ü:[¡¶¡·?£à£­£½£Û£Ý;£§,£®£¯";
            char[] cc = keys.ToCharArray();
            foreach (char c in cc)
                fd = fd.Replace(c.ToString(), "");
            string s = fd.Substring(0, 1);
            try
            {
                int a = int.Parse(s);
                fd = "F" + fd;
            }
            catch
            {
            }
            return fd;
        }
        private static string _KeyFields = null;
        public static string KeyFields
        {
            get
            {
                if (_KeyFields == null)
                    _KeyFields = BP.DA.DataType.ReadTextFile(SystemConfig.PathOfWebApp + "\\WF\\Data\\Sys\\FieldKeys.txt");
                return _KeyFields;
            }
        }
        public static bool IsNum(string str)
        {
            Boolean strResult;
            String cn_Regex = @"^[\u4e00-\u9fa5]+$";
            if (Regex.IsMatch(str, cn_Regex))
            {
                strResult = true;
            }
            else
            {
                strResult = false;
            }
            return strResult;
        }

        public static bool IsCN(string str)
        {
            Boolean strResult;
            String cn_Regex = @"^[\u4e00-\u9fa5]+$";
            if (Regex.IsMatch(str, cn_Regex))
            {
                strResult = true;
            }
            else
            {
                strResult = false;
            }
            return strResult;
        }

        public static bool IsImg(string ext)
        {
            ext = ext.Replace(".", "").ToLower();
            switch (ext)
            {
                case "gif":
                    return true;
                case "jpg":
                    return true;
                case "bmp":
                    return true;
                case "png":
                    return true;
                default:
                    return false;
            }
        }
        /// <summary>
        ///  In accordance with the ratio of the number of small 
        /// </summary>
        /// <param name="ObjH"> Target height </param>
        /// <param name="factH"> Actual height </param>
        /// <param name="factW"> The actual width </param>
        /// <returns> Target width </returns>
        public static int GenerImgW_del(int ObjH, int factH, int factW, int isZeroAsWith)
        {
            if (factH == 0 || factW == 0)
                return isZeroAsWith;

            decimal d= decimal.Parse(ObjH.ToString()) / decimal.Parse(factH.ToString()) * decimal.Parse( factW.ToString()) ;

            try
            {
                return int.Parse(d.ToString("0"));
            }
            catch(Exception ex)
            {
                throw new Exception(d.ToString() +ex.Message );
            }
        }

        /// <summary>
        ///  In accordance with the ratio of the number of small 
        /// </summary>
        /// <param name="ObjH"> Target height </param>
        /// <param name="factH"> Actual height </param>
        /// <param name="factW"> The actual width </param>
        /// <returns> Target width </returns>
        public static int GenerImgH(int ObjW, int factH, int factW, int isZeroAsWith)
        {
            if (factH == 0 || factW == 0)
                return isZeroAsWith;

            decimal d = decimal.Parse(ObjW.ToString()) / decimal.Parse(factW.ToString()) * decimal.Parse(factH.ToString());

            try
            {
                return int.Parse(d.ToString("0"));
            }
            catch (Exception ex)
            {
                throw new Exception(d.ToString() + ex.Message);
            }
        }


        public static string FilesViewStr(string enName, object pk)
        {
            string url = "/WF/Comm/FileManager.aspx?EnsName=" + enName + "&PK=" + pk.ToString();
            string strs = "";
            SysFileManagers ens = new SysFileManagers(enName, pk.ToString());
            string path = BP.Sys.Glo.Request.ApplicationPath;

            foreach (SysFileManager file in ens)
            {
                strs += "<img src='/WF/Img/FileType/" + file.MyFileExt.Replace(".", "") + ".gif' border=0 /><a href='" + path + file.MyFilePath + "' target='_blank' >" + file.MyFileName + file.MyFileExt + "</a>&nbsp;";
                if (file.Rec == WebUser.No)
                {
                    strs += "<a title=' Open it ' href=\"javascript:DoAction('" + path + "Comm/Do.aspx?ActionType=1&OID=" + file.OID + "&EnsName=" + enName + "&PK=" + pk + "',' Delete files ¡¶" + file.MyFileName + file.MyFileExt + "¡·')\" ><img src='" + path + "/WF/Img/Btn/delete.gif' border=0 alt=' Delete this attachment ' /></a>&nbsp;";
                }
            }
            return strs;
        }
		public static string GenerLabelStr(string title)
		{
			string path = BP.Sys.Glo.Request.ApplicationPath;
            if (path == "" || path == "/")
                path = "..";

			string str="";
			str+="<TABLE  height='100%' cellPadding='0' background='"+path+"/Images/DG_bgright.gif'>";
			str+="<TBODY>";
			str+="<TR   >";
			str+="<TD  >";
			str+="<IMG src='"+path+"/Images/DG_Title_Left.gif' border='0'></TD>";
			str+="<TD style='font-size:14px'  vAlign='bottom' noWrap background='"+path+"/Images/DG_Title_BG.gif'>&nbsp;";
			str+=" &nbsp;<b>"+title+"</b>&nbsp;&nbsp;";
			str+="</TD>";
			str+="<TD>";
			str+="<IMG src='"+path+"/Images/DG_Title_Right.gif' border='0'></TD>";
			str+="</TR>";
			str+="</TBODY>";
			str+="</TABLE>";
			return str;		 
			//return str;
		}
        /// <summary>
        ///  Will be converted into phonetic characters 
        /// </summary>
        /// <param name="str"> To convert characters </param>
        /// <returns> Returns Pinyin </returns>
        public string Chs2Pinyin(string str)
        {
            return BP.Tools.chs2py.convert(str); 
        }

		public static string GenerTablePage(DataTable dt , string title )
		{
		
			string str="<Table id='tb' class=Table >";

            str += "<caption>" + title + "</caption>";
           

			//  Title 
			str+="<TR>";
			foreach(DataColumn dc in dt.Columns)
			{
				str+= "<TD class='DGCellOfHeader"+BP.Web.WebUser.Style+"' nowrap >"+dc.ColumnName+"</TD>";
			}
			str+="</TR>";
			 
			// Content 
			foreach(DataRow dr in dt.Rows)
			{
				str+="<TR>";

		
				foreach(DataColumn dc in dt.Columns)
				{
					//string doc=dr[dc.ColumnName];
					str+= "<TD nowrap=true >&nbsp;"+dr[dc.ColumnName]+"</TD>";
				}
				str+="</TR>";
			}
			str+="</Table>";
			return str;
		}
		/// <summary>
		///  Generate temporary file name 
		/// </summary>
		/// <param name="hz"></param>
		/// <returns></returns>
		public static string GenerTempFileName(string hz)
		{
			return Web.WebUser.No+DateTime.Now.ToString("MMddhhmmss")+"."+hz;
		}
		public static void DeleteTempFiles()
		{
			//string[] strs = System.IO.Directory.GetFiles( MapPath( SystemConfig.TempFilePath )) ;
			string[] strs = System.IO.Directory.GetFiles(   SystemConfig.PathOfTemp  ) ;

			foreach(string s in strs)
			{
				System.IO.File.Delete(s);
			}
		}
		/// <summary>
		///  Re-indexing 
		/// </summary>
		public static void ReCreateIndex()
		{
			ArrayList als = ClassFactory.GetObjects("BP.En.Entity");
			string sql="";
			foreach(object obj in als)
			{
				Entity en = (Entity)obj;
				if (en.EnMap.EnType ==EnType.View)
					continue;
				sql+="IF EXISTS( SELECT name  FROM  sysobjects WHERE  name='"+en.EnMap.PhysicsTable+"') <BR> DROP TABLE "+en.EnMap.PhysicsTable+"<BR>";
				sql+="CREATE TABLE "+en.EnMap.PhysicsTable+" ( <BR>";
				sql+="";
			}


		}
		public static void DBIOToAccess()
		{
			ArrayList al =  BP.En.ClassFactory.GetObjects("BP.En.Entities");
			PubClass.DBIO(DBType.Access,al,false);
		}
        /// <summary>
        ///  Check all physical table 
        /// </summary>
        public static void CheckAllPTable(string nameS)
        {
            ArrayList al = BP.En.ClassFactory.GetObjects("BP.En.Entities");
            foreach (Entities ens in al)
            {
                if (ens.ToString().Contains(nameS) == false)
                    continue;


                try
                {
                    Entity en = ens.GetNewEntity;
                    en.CheckPhysicsTable();
                }
                catch
                {
 
                }

            }

        }
        
		/// <summary>
		///  Data transmission 
		/// </summary>
		/// <param name="dbtype"> Object </param>
		/// <returns></returns>
        public static void DBIO(DA.DBType dbtype, ArrayList als, bool creatTableOnly)
        {
            foreach (Entities ens in als)
            {
                Entity myen = ens.GetNewEntity;
                if (myen.EnMap.EnType == EnType.View)
                    continue;

                #region create table
                switch (dbtype)
                {

                    case DBType.Oracle:
                        try
                        {
                           
                            DBAccessOfOracle.RunSQL("drop table " + myen.EnMap.PhysicsTable);
                        }
                        catch
                        {
                        }
                        try
                        {
                            DBAccessOfOracle.RunSQL(SqlBuilder.GenerCreateTableSQLOfOra_OK(myen));
                        }
                        catch
                        {

                        }
                        break;
                    case DBType.MSSQL:
                         try
                        {
                            if (myen.EnMap.PhysicsTable.Contains("."))
                                continue;

                            if (DBAccessOfMSMSSQL.IsExitsObject(myen.EnMap.PhysicsTable))
                                continue;

                            DBAccessOfMSMSSQL.RunSQL("drop table " + myen.EnMap.PhysicsTable);
                        }
                        catch
                        {
                        }
                        DBAccessOfMSMSSQL.RunSQL(SqlBuilder.GenerCreateTableSQLOfMS(myen));
                        break;
                    case DBType.Informix:
                        try
                        {
                            if (myen.EnMap.PhysicsTable.Contains("."))
                                continue;

                            if (DBAccessOfMSMSSQL.IsExitsObject(myen.EnMap.PhysicsTable))
                                continue;

                            DBAccessOfMSMSSQL.RunSQL("drop table " + myen.EnMap.PhysicsTable);
                        }
                        catch
                        {
                        }
                        DBAccessOfMSMSSQL.RunSQL(SqlBuilder.GenerCreateTableSQLOfInfoMix(myen));
                        break;
                    case DBType.Access:
                        try
                        {
                            DBAccessOfOLE.RunSQL("drop table " + myen.EnMap.PhysicsTable);
                        }
                        catch
                        {
                        }
                        DBAccessOfOLE.RunSQL(SqlBuilder.GenerCreateTableSQLOf_OLE(myen));
                        break;
                    default:
                        throw new Exception("error :");

                }
                #endregion

                if (creatTableOnly)
                    return;

                try
                {
                    QueryObject qo = new QueryObject(ens);
                    qo.DoQuery();
                    // ens.RetrieveAll(1000);
                }
                catch
                {
                    continue;
                }

                #region insert data
                foreach (Entity en in ens)
                {
                    try
                    {
                        switch (dbtype)
                        {
                            case DBType.Oracle:
                            case DBType.Informix:
                                DBAccessOfOracle.RunSQL(SqlBuilder.Insert(en));
                                break;
                            case DBType.MSSQL:
                                DBAccessOfMSMSSQL.RunSQL(SqlBuilder.Insert(en));
                                break;
                            case DBType.Access:
                                DBAccessOfOLE.RunSQL(SqlBuilder.InsertOFOLE(en));
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.DefaultLogWriteLineError( dbtype.ToString()+ "bak Error :" + ex.Message);
                    }
                }
                #endregion
            }
        }
        /// <summary>
        ///  Get datatable.
        /// </summary>
        /// <param name="uiBindKey"></param>
        /// <returns></returns>
        public static System.Data.DataTable GetDataTableByUIBineKey(string uiBindKey)
        {
            DataTable dt = new DataTable();
            if (uiBindKey.Contains("."))
            {
                Entities ens = BP.En.ClassFactory.GetEns(uiBindKey);
                if (ens == null)
                    ens = BP.En.ClassFactory.GetEns(uiBindKey);
                
                if (ens == null)
                    ens = BP.En.ClassFactory.GetEns(uiBindKey);
                if (ens == null)
                    throw new Exception(" Class name wrong :" + uiBindKey + ", Can not be converted into ens.");

                ens.RetrieveAllFromDBSource();
                dt = ens.ToDataTableField(uiBindKey);
                return dt;
            }
            else
            {

                string sql = "SELECT No,Name FROM " + uiBindKey;
                dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
                dt.TableName = uiBindKey;
                return dt;
            }
        }
        /// <summary>
        ///  Get the data source 
        /// </summary>
        /// <param name="uiBindKey"> Foreign key bindings or enumeration </param>
        /// <returns></returns>
        public static System.Data.DataTable GetDataTableByUIBineKeyForCCFormDesigner(string uiBindKey)
        {
            int topNum = 40;

            DataTable dt = new DataTable();
            if (uiBindKey.Contains("."))
            {
                Entities ens = BP.En.ClassFactory.GetEns(uiBindKey);
                if (ens == null)
                    ens = BP.En.ClassFactory.GetEns(uiBindKey);

                if (ens == null)
                    ens = BP.En.ClassFactory.GetEns(uiBindKey);
                if (ens == null)
                    throw new Exception(" Class name wrong :" + uiBindKey + ", Can not be converted into ens.");

                BP.En.QueryObject qo = new QueryObject(ens);
                return qo.DoQueryToTable(topNum);
            }
            else
            {
                string sql = "";
                switch (BP.Sys.SystemConfig.AppCenterDBType)
                {
                    case DBType.Oracle:
                        sql = "SELECT No,Name FROM " + uiBindKey + " where rowNum <= " + topNum;
                        break;
                    case DBType.MSSQL:
                        sql = "SELECT top " + topNum + " No,Name FROM " + uiBindKey;
                        break;
                    default:
                        sql = "SELECT  No,Name FROM " + uiBindKey;
                        break;
                }
                dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
                dt.TableName = uiBindKey;
                return dt;
            }
        }

		#region  Scheduling 
        public static string GenerDBOfOreacle()
        {
            ArrayList als = ClassFactory.GetObjects("BP.En.Entity");
            string sql = "";
            foreach (object obj in als)
            {
                Entity en = (Entity)obj;
                sql += "IF EXISTS( SELECT name  FROM  sysobjects WHERE  name='" + en.EnMap.PhysicsTable + "') <BR> DROP TABLE " + en.EnMap.PhysicsTable + "<BR>";
                sql += "CREATE TABLE " + en.EnMap.PhysicsTable + " ( <BR>";
                sql += "";
            }
            //DA.Log.DefaultLogWriteLine(LogType.Error,msg.Replace("<br>@","\n") ); // 
            return sql;
        }
        public static string DBRpt(DBCheckLevel level)
        {
            //  Remove all of the entities 
            ArrayList als = ClassFactory.GetObjects("BP.En.Entities");
            string msg = "";
            foreach (object obj in als)
            {
                Entities ens = (Entities)obj;
                try
                {
                    msg += DBRpt1(level, ens);
                }
                catch (Exception ex)
                {
                    msg += "<hr>" + ens.ToString() + " Examination failure :" + ex.Message;
                }
            }

            MapDatas mds = new MapDatas();
            mds.RetrieveAllFromDBSource();
            foreach (MapData md in mds)
            {
                try
                {
                    md.HisGEEn.CheckPhysicsTable();
                    PubClass.AddComment(md.HisGEEn);
                }
                catch (Exception ex)
                {
                    msg += "<hr>" + md.No + " Examination failure :" + ex.Message;
                }
            }

            MapDtls dtls = new MapDtls();
            dtls.RetrieveAllFromDBSource();
            foreach (MapDtl dtl in dtls)
            {
                try
                {
                    dtl.HisGEDtl.CheckPhysicsTable();
                    PubClass.AddComment(dtl.HisGEDtl);
                }
                catch (Exception ex)
                {
                    msg += "<hr>" + dtl.No + " Examination failure :" + ex.Message;
                }
            }

            #region  Check processing necessary basic data  Pub_Day .
            string sql = "";
            string sqls = "";
            sql = "SELECT count(*) Num FROM Pub_Day";
            try
            {
                if (DBAccess.RunSQLReturnValInt(sql) == 0)
                {
                    for (int i = 1; i <= 31; i++)
                    {
                        string d = i.ToString().PadLeft(2, '0');
                        sqls += "@INSERT INTO Pub_Day(No,Name)VALUES('" + d.ToString() + "','" + d.ToString() + "')";
                    }
                }
            }
            catch
            {
            }

            sql = "SELECT count(*) Num FROM Pub_YF";
            try
            {
                if (DBAccess.RunSQLReturnValInt(sql) == 0)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        string d = i.ToString().PadLeft(2, '0');
                        sqls += "@INSERT INTO Pub_YF(No,Name)VALUES('" + d.ToString() + "','" + d.ToString() + "')";
                    }
                }
            }
            catch
            {
            }

            sql = "SELECT count(*) Num FROM Pub_ND";
            try
            {
                if (DBAccess.RunSQLReturnValInt(sql) == 0)
                {
                    for (int i = 2010; i < 2015; i++)
                    {
                        string d = i.ToString();
                        sqls += "@INSERT INTO Pub_ND(No,Name)VALUES('" + d.ToString() + "','" + d.ToString() + "')";
                    }
                }
            }
            catch
            {

            }
            sql = "SELECT count(*) Num FROM Pub_NY";
            try
            {
                if (DBAccess.RunSQLReturnValInt(sql) == 0)
                {
                    for (int i = 2010; i < 2015; i++)
                    {

                        for (int yf = 1; yf <= 12; yf++)
                        {
                            string d = i.ToString() + "-" + yf.ToString().PadLeft(2,'0');
                            sqls += "@INSERT INTO Pub_NY(No,Name)VALUES('" + d + "','" + d + "')";
                        }
                    }
                }
            }
            catch
            {
            }

            DBAccess.RunSQLs(sqls);
            #endregion  Check processing necessary basic data .
            return msg;
        }
        private static void RepleaceFieldDesc(Entity en)
        {
            string tableId = DBAccess.RunSQLReturnVal("select ID from sysobjects WHERE name='" + en.EnMap.PhysicsTable + "' AND xtype='U'").ToString();

            if (tableId == null || tableId == "")
                return;

            foreach (Attr attr in en.EnMap.Attrs)
            {
                if (attr.MyFieldType == FieldType.RefText)
                    continue;

            }
        }
        /// <summary>
        ///  Table add comments 
        /// </summary>
        /// <returns></returns>
        public static string AddComment()
        {
            //  Remove all of the entities 
            ArrayList als = ClassFactory.GetObjects("BP.En.Entities");
            string msg = "";
            Entity en = null;
            Entities ens = null;
            foreach (object obj in als)
            {
                try
                {
                    ens = (Entities)obj;
                    en = ens.GetNewEntity;
                    if (en.EnMap.EnType == EnType.View || en.EnMap.EnType == EnType.ThirdPartApp)
                        continue;
                }
                catch
                {
                    continue;
                }
               msg+= AddComment(en);
            }
            return msg;
        }
        public static string AddComment(Entity en)
        {
            try
            {
                switch (en.EnMap.EnDBUrl.DBType)
                {
                    case DBType.Oracle:
                        AddCommentForTable_Ora(en);
                        break;
                    default:
                        AddCommentForTable_MS(en);
                        break;
                }
                return "";
            }
            catch (Exception ex)
            {
                return "<hr>" + en.ToString() + " Examination failure :" + ex.Message;
            }
        }
        public static void AddCommentForTable_Ora(Entity en)
        {
            en.RunSQL("comment on table " + en.EnMap.PhysicsTable + " IS '" + en.EnDesc + "'");
            SysEnums ses = new SysEnums();
            foreach (Attr attr in en.EnMap.Attrs)
            {
                if (attr.MyFieldType == FieldType.RefText)
                    continue;
                switch (attr.MyFieldType)
                {
                    case FieldType.PK:
                        en.RunSQL("comment on column  " + en.EnMap.PhysicsTable + "." + attr.Field + " IS '" + attr.Desc + " -  Primary key '");
                        break;
                    case FieldType.Normal:
                        en.RunSQL("comment on column  " + en.EnMap.PhysicsTable + "." + attr.Field + " IS '" + attr.Desc + "'");
                        break;
                    case FieldType.Enum:
                        ses = new SysEnums(attr.Key, attr.UITag);
                        en.RunSQL("comment on column  " + en.EnMap.PhysicsTable + "." + attr.Field + " IS '" + attr.Desc + ", Enumerated type :" + ses.ToDesc() + "'");
                        break;
                    case FieldType.PKEnum:
                        ses = new SysEnums(attr.Key, attr.UITag);
                        en.RunSQL("comment on column  " + en.EnMap.PhysicsTable + "." + attr.Field + " IS '" + attr.Desc + ",  Primary key : Enumerated type :" + ses.ToDesc() + "'");
                        break;
                    case FieldType.FK:
                        Entity myen = attr.HisFKEn; // ClassFactory.GetEns(attr.UIBindKey).GetNewEntity;
                        en.RunSQL("comment on column  " + en.EnMap.PhysicsTable + "." + attr.Field + " IS " + attr.Desc + ",  Foreign key : Corresponding physical table :" + myen.EnMap.PhysicsTable + ", The table below describes :" + myen.EnDesc);
                        break;
                    case FieldType.PKFK:
                        Entity myen1 = attr.HisFKEn; // ClassFactory.GetEns(attr.UIBindKey).GetNewEntity;
                        en.RunSQL("comment on column  " + en.EnMap.PhysicsTable + "." + attr.Field + " IS '" + attr.Desc + ",  Primary foreign key : Corresponding physical table :" + myen1.EnMap.PhysicsTable + ", The table below describes :" + myen1.EnDesc + "'");
                        break;
                    default:
                        break;
                }
            }
        }
        private static void AddColNote(Entity en, string table, string col, string note)
        {
            try
            {
                string sql = "execute  sp_dropextendedproperty 'MS_Description','user',dbo,'table','" + table + "','column'," + col;
                en.RunSQL(sql);
            }
            catch(Exception ex)
            {
            }

            try
            {
                string sql = "execute  sp_addextendedproperty 'MS_Description', '" + note + "', 'user', dbo, 'table', '" + table + "', 'column', '" + col + "'";
                en.RunSQL(sql);
            }
            catch (Exception ex)
            {
            }
        }
		/// <summary>
		///  Table explanation for the increase 
		/// </summary>
		/// <param name="en"></param>
        public static void AddCommentForTable_MS(Entity en)
        {
            if (en.EnMap.EnType == EnType.View || en.EnMap.EnType == EnType.ThirdPartApp )
            {
                return;
            }

            try
            {
                string sql = "execute  sp_dropextendedproperty 'MS_Description','user',dbo,'table','" + en.EnMap.PhysicsTable + "'";
                en.RunSQL(sql);
            }
            catch (Exception ex)
            {
            }

            try
            {
                string sql = "execute  sp_addextendedproperty 'MS_Description', '" + en.EnDesc + "', 'user', dbo, 'table', '" + en.EnMap.PhysicsTable + "'";
                en.RunSQL(sql);
            }
            catch (Exception ex)
            {

            }


            SysEnums ses = new SysEnums();
            foreach (Attr attr in en.EnMap.Attrs)
            {
                if (attr.MyFieldType == FieldType.RefText)
                    continue;
                if (attr.Key == attr.Desc)
                    continue;

                switch (attr.MyFieldType)
                {
                    case FieldType.Normal:
                        AddColNote(en, en.EnMap.PhysicsTable, attr.Field, attr.Desc);
                        //en.RunSQL("comment on table "+ en.EnMap.PhysicsTable+"."+attr.Field +" IS '"+en.EnDesc+"'");
                        break;
                    case FieldType.Enum:
                        ses = new SysEnums(attr.Key, attr.UITag);
                        //	en.RunSQL("comment on table "+ en.EnMap.PhysicsTable+"."+attr.Field +" IS '"++"'" );
                        AddColNote(en, en.EnMap.PhysicsTable, attr.Field, attr.Desc + ", Enumerated type :" + ses.ToDesc());
                        break;
                    case FieldType.PKEnum:
                        ses = new SysEnums(attr.Key, attr.UITag);
                        AddColNote(en, en.EnMap.PhysicsTable, attr.Field, attr.Desc + ", Primary key : Enumerated type :" + ses.ToDesc());
                        //en.RunSQL("comment on table "+ en.EnMap.PhysicsTable+"."+attr.Field +" IS '"+en.EnDesc+",  Primary key : Enumerated type :"+ses.ToDesc()+"'" );
                        break;
                    case FieldType.FK:
                        Entity myen = attr.HisFKEn; // ClassFactory.GetEns(attr.UIBindKey).GetNewEntity;
                        AddColNote(en, en.EnMap.PhysicsTable, attr.Field, attr.Desc + ",  Foreign key : Corresponding physical table :" + myen.EnMap.PhysicsTable + ", The table below describes :" + myen.EnDesc);
                        //en.RunSQL("comment on table "+ en.EnMap.PhysicsTable+"."+attr.Field +" IS "+  );
                        break;
                    case FieldType.PKFK:
                        Entity myen1 = attr.HisFKEn; // ClassFactory.GetEns(attr.UIBindKey).GetNewEntity;
                        AddColNote(en, en.EnMap.PhysicsTable, attr.Field, attr.Desc + ",  Primary foreign key : Corresponding physical table :" + myen1.EnMap.PhysicsTable + ", The table below describes :" + myen1.EnDesc);
                        //en.RunSQL("comment on table "+ en.EnMap.PhysicsTable+"."+attr.Field +" IS '"+  );
                        break;
                    default:
                        break;
                }
            }
        }
		/// <summary>
		///  Labor system report , If a problem occurs , To write to the log inside .
		/// </summary>
		/// <returns></returns>
        public static string DBRpt1(DBCheckLevel level, Entities ens)
        {
            Entity en = ens.GetNewEntity;
            if (en.EnMap.EnDBUrl.DBUrlType != DBUrlType.AppCenterDSN)
                return null;

            if (en.EnMap.EnType == EnType.ThirdPartApp)
                return null;

            if (en.EnMap.EnType == EnType.View)
                return null;

            if (en.EnMap.EnType == EnType.Ext)
                return null;

            //  Fields detect physical table .
            en.CheckPhysicsTable();
            
            PubClass.AddComment(en);

            string msg = "";
            //if (level == DBLevel.High)
            //{
            //    try
            //    {
            //        DBAccess.RunSQL("update pub_emp set AuthorizedAgent='1' WHERE AuthorizedAgent='0' ");
            //    }
            //    catch
            //    {
            //    }
            //}
            string table = en.EnMap.PhysicsTable;
            Attrs fkAttrs = en.EnMap.HisFKAttrs;
            if (fkAttrs.Count == 0)
                return msg;
            int num = 0;
            string sql;
            //string msg="";
            foreach (Attr attr in fkAttrs)
            {
                if (attr.MyFieldType == FieldType.RefText)
                    continue;

                string enMsg = "";
                try
                {
                    #region  Update their , Remove about blank , Because foreign key can not contain spaces around .
                    if (level == DBCheckLevel.Middle || level == DBCheckLevel.High)
                    {
                        /* If the high school level , To remove about blank */
                        if (attr.MyDataType == DataType.AppString)
                        {
                            DBAccess.RunSQL("UPDATE " + en.EnMap.PhysicsTable + " SET " + attr.Field + " = rtrim( ltrim(" + attr.Field + ") )");
                        }
                    }
                    #endregion

                    #region  Case processing associated table .
                    Entities refEns = attr.HisFKEns; // ClassFactory.GetEns(attr.UIBindKey);
                    Entity refEn = refEns.GetNewEntity;

                    // Remove the associated table .
                    string reftable = refEn.EnMap.PhysicsTable;
                    //sql="SELECT COUNT(*) FROM "+en.EnMap.PhysicsTable+" WHERE "+attr.Key+" is null or len("+attr.Key+") < 1 ";
                    //  Determine whether there is a foreign key table .

                    sql = "SELECT COUNT(*) FROM  sysobjects  WHERE  name = '" + reftable + "'";
                    //num=DA.DBAccess.RunSQLReturnValInt(sql,0);
                    if (DBAccess.IsExitsObject(reftable) == false)
                    {
                        // Report an error message 
                        enMsg += "<br>@ Check Entity :" + en.EnDesc + ", Field  " + attr.Key + " ,  Field Description :" + attr.Desc + " ,  Foreign key physical table :" + reftable + " Does not exist :" + sql;
                    }
                    else
                    {
                        Attr attrRefKey = refEn.EnMap.GetAttrByKey(attr.UIRefKeyValue); //  Remove the left and right main key   Blank £®
                        if (attrRefKey.MyDataType == DataType.AppString)
                        {
                            if (level == DBCheckLevel.Middle || level == DBCheckLevel.High)
                            {
                                /* If the high school level , To remove about blank */
                                DBAccess.RunSQL("UPDATE " + reftable + " SET " + attrRefKey.Field + " = rtrim( ltrim(" + attrRefKey.Field + ") )");
                            }
                        }

                        Attr attrRefText = refEn.EnMap.GetAttrByKey(attr.UIRefKeyText);  //  Remove the primary key  Text  About   Blank £®

                        if (level == DBCheckLevel.Middle || level == DBCheckLevel.High)
                        {
                            /* If the high school level , To remove about blank */
                            DBAccess.RunSQL("UPDATE " + reftable + " SET " + attrRefText.Field + " = rtrim( ltrim(" + attrRefText.Field + ") )");
                        }

                    }
                    #endregion

                    #region  Whether the entity is a foreign key blank 
                    switch (en.EnMap.EnDBUrl.DBType)
                    {
                        case DBType.Oracle:
                            sql = "SELECT COUNT(*) FROM " + en.EnMap.PhysicsTable + " WHERE " + attr.Field + " is null or length(" + attr.Field + ") < 1 ";
                            break;
                        default:
                            sql = "SELECT COUNT(*) FROM " + en.EnMap.PhysicsTable + " WHERE " + attr.Field + " is null or len(" + attr.Field + ") < 1 ";
                            break;
                    }

                    num = DA.DBAccess.RunSQLReturnValInt(sql, 0);
                    if (num == 0)
                    {
                    }
                    else
                    {
                        enMsg += "<br>@ Check Entity :" + en.EnDesc + ", Physical table :" + en.EnMap.PhysicsTable + " Appear " + attr.Key + "," + attr.Desc + " Incorrect , Total [" + num + "] No data rows ." + sql;
                    }
                    #endregion

                    #region  Can correspond to a foreign key 
                    // Can correspond to a foreign key .
                    sql = "SELECT COUNT(*) FROM " + en.EnMap.PhysicsTable + " WHERE " + attr.Field + " NOT IN ( SELECT " + refEn.EnMap.GetAttrByKey(attr.UIRefKeyValue).Field + " FROM " + reftable + "	 ) ";
                    num = DA.DBAccess.RunSQLReturnValInt(sql, 0);
                    if (num == 0)
                    {
                    }
                    else
                    {
                        /* If the high school level .*/
                        string delsql = "DELETE FROM " + en.EnMap.PhysicsTable + " WHERE " + attr.Field + " NOT IN ( SELECT " + refEn.EnMap.GetAttrByKey(attr.UIRefKeyValue).Field + " FROM " + reftable + "	 ) ";
                        //int i =DBAccess.RunSQL(delsql);
                        enMsg += "<br>@" + en.EnDesc + ", Physical table :" + en.EnMap.PhysicsTable + " Appear " + attr.Key + "," + attr.Desc + " Incorrect , Total [" + num + "] Rows is not associated with data , Check the physical table and foreign key table ." + sql + " If you do not want to remove these correspond to the data on the run as follows SQL: " + delsql + "  Please carefully executed .";
                    }
                    #endregion

                    #region  Judge   Primary key 
                    //DBAccess.IsExits("");
                    #endregion
                }
                catch (Exception ex)
                {
                    enMsg += "<br>@" + ex.Message;
                }

                if (enMsg != "")
                {
                    msg += "<BR><b>--  Inspection [" + en.EnDesc + "," + en.EnMap.PhysicsTable + "] Problem arises , The class name :" + en.ToString() + "</b>";
                    msg += enMsg;
                }
            }
            return msg;
        }
		#endregion

		#region  Format conversion   chen
		/// <summary>
		///  The data is converted into a control Excel File 
		/// </summary>
		/// <param name="ctl"></param>
		public static void ToExcel(System.Web.UI.Control ctl ,string filename)  
		{
			HttpContext.Current.Response.Charset ="GB2312";	
			HttpContext.Current.Response.AppendHeader("Content-Disposition","attachment;filename="+ filename +".xls");
			HttpContext.Current.Response.ContentEncoding =System.Text.Encoding.GetEncoding("GB2312"); 
			HttpContext.Current.Response.ContentType ="application/ms-excel";//"application/ms-excel";
			//image/JPEG;text/HTML;image/GIF;application/ms-msword
			ctl.Page.EnableViewState =false;
			System.IO.StringWriter  tw = new System.IO.StringWriter() ;
			System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter (tw);
			ctl.RenderControl(hw);
			HttpContext.Current.Response.Write(tw.ToString());
			HttpContext.Current.Response.End();
		}
		/// <summary>
		///  The data is converted into a control Word File 
		/// </summary>
		/// <param name="ctl"></param>
		public static void ToWord(System.Web.UI.Control ctl ,string filename)  
		{
            filename = HttpUtility.UrlEncode(filename);
			HttpContext.Current.Response.Charset ="GB2312";	
			HttpContext.Current.Response.AppendHeader("Content-Disposition","attachment;filename="+ filename +".doc");
			HttpContext.Current.Response.ContentEncoding =System.Text.Encoding.GetEncoding("GB2312"); 
			HttpContext.Current.Response.ContentType ="application/ms-msword";//image/JPEG;text/HTML;image/GIF;application/ms-excel
			ctl.Page.EnableViewState =false;			
			System.IO.StringWriter  tw = new System.IO.StringWriter() ;
			System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter (tw);
			ctl.RenderControl(hw);
			HttpContext.Current.Response.Write(tw.ToString());
		}

        public static void OpenExcel(string filepath, string tempName)
        {
            tempName = HttpUtility.UrlEncode(tempName);
            HttpContext.Current.Response.Charset = "GB2312";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + tempName);
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            HttpContext.Current.Response.ContentType = "application/ms-excel"; 
            HttpContext.Current.Response.WriteFile(filepath);
            HttpContext.Current.Response.End();
            HttpContext.Current.Response.Close();
        }
        public static void DownloadFile(string filepath, string tempName)
        {
            tempName = HttpUtility.UrlEncode(tempName);
            HttpContext.Current.Response.Charset = "GB2312";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + tempName);
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");

            //HttpContext.Current.Response.ContentType = "application/ms-msword";  //image/JPEG;text/HTML;image/GIF;application/ms-excel
            //HttpContext.Current.EnableViewState =false;

            HttpContext.Current.Response.WriteFile(filepath);
            HttpContext.Current.Response.End();
            HttpContext.Current.Response.Close();
        }
        public static void DownloadFileV2(string filepath, string tempName)
        {
             
            FileInfo fileInfo = new FileInfo(filepath);
            if (fileInfo.Exists)
            {
                byte[] buffer = new byte[102400];
                HttpContext.Current.Response.Clear();
                using (FileStream iStream = File.OpenRead(fileInfo.FullName))
                {
                    long dataLengthToRead = iStream.Length; // Get the total size of the downloaded file 

                    HttpContext.Current.Response.ContentType = "application/octet-stream";
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;  filename=" +
                                       HttpUtility.UrlEncode(tempName, System.Text.Encoding.UTF8));
                    while (dataLengthToRead > 0 && HttpContext.Current.Response.IsClientConnected)
                    {
                        int lengthRead = iStream.Read(buffer, 0, Convert.ToInt32(102400));//' The size of the read 

                        HttpContext.Current.Response.OutputStream.Write(buffer, 0, lengthRead);
                        HttpContext.Current.Response.Flush();
                        dataLengthToRead = dataLengthToRead - lengthRead;
                    }
                    HttpContext.Current.Response.Close();
                    HttpContext.Current.Response.End();
                }
            }
        }
        public static void OpenWordDoc(string filepath, string tempName)
        {
            tempName = HttpUtility.UrlEncode(tempName);

            HttpContext.Current.Response.Charset = "GB2312";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + tempName);
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            HttpContext.Current.Response.ContentType = "application/ms-msword";  //image/JPEG;text/HTML;image/GIF;application/ms-excel
            //HttpContext.Current.EnableViewState =false;
            HttpContext.Current.Response.WriteFile(filepath);
            HttpContext.Current.Response.End();
            HttpContext.Current.Response.Close();
        }
        public static void OpenWordDocV2(string filepath, string tempName)
        {
            //tempName = HttpUtility.UrlEncode(tempName);

            FileInfo fileInfo = new FileInfo(filepath);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = false;
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.Charset = "UTF-8";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(tempName, System.Text.Encoding.UTF8));
            HttpContext.Current.Response.AppendHeader("Content-Length", fileInfo.Length.ToString());
            HttpContext.Current.Response.WriteFile(fileInfo.FullName);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
		#endregion
		
		#region

		#region
		public static void To(string url)
		{
			 System.Web.HttpContext.Current.Response.Redirect(url,true);
		}
		public static void Print(string url)
		{
			System.Web.HttpContext.Current.Response.Write( "<script language='JavaScript'> var newWindow =window.open('"+url+"','p','width=0,top=10,left=10,height=1,scrollbars=yes,resizable=yes,toolbar=yes,location=yes,menubar=yes') ; newWindow.focus(); </script> ");
		}
        public static BP.En.Entity CopyFromRequest(BP.En.Entity en)
        {
            return CopyFromRequest(en, BP.Sys.Glo.Request);
        }
        public static BP.En.Entity CopyFromRequest(BP.En.Entity en,HttpRequest reqest)
        {
            string allKeys = ";";
            foreach (string myK in reqest.Params.Keys)
                allKeys += myK + ";";

            //  For each attribute value .            
            Attrs attrs = en.EnMap.Attrs;
            foreach (Attr item in attrs)
            {
                string relKey = null;
                switch (item.UIContralType)
                {
                    case UIContralType.TB:
                        relKey = "TB_" + item.Key;
                        break;
                    case UIContralType.CheckBok:
                        relKey = "CB_" + item.Key;
                        break;
                    case UIContralType.DDL:
                        relKey = "DDL_" + item.Key;
                        break;
                    default:
                        break;
                }

                if (allKeys.Contains(relKey + ";"))
                {
                    /* Explained that it had found the information in this field .*/
                    foreach (string myK in BP.Sys.Glo.Request.Params.Keys)
                    {
                        if (myK == null || myK == "")
                            continue;

                        if (myK.EndsWith(relKey))
                        {
                            if (item.UIContralType == UIContralType.CheckBok)
                            {
                                string val = BP.Sys.Glo.Request.Params[myK];
                                if (val == "on" || val == "1")
                                    en.SetValByKey(item.Key, 1);
                                else
                                    en.SetValByKey(item.Key, 0);
                            }
                            else
                            {
                                en.SetValByKey(item.Key, BP.Sys.Glo.Request.Params[myK]);
                            }
                        }
                        // if (myK.Contains(relKey+";" ))
                    }
                    continue;
                }
            }
            return en;
        }
        public static void WinClose(string returnVal)
        {
            string clientscript = "<script language='javascript'> window.returnValue = '" + returnVal + "'; window.close(); </script>";
            System.Web.HttpContext.Current.Response.Write(clientscript);
        }
        public static void WinCloseAndReParent(string returnVal)
        {
            string clientscript = "<script language='javascript'> window.opener.location.reload(); window.close(); </script>";
            System.Web.HttpContext.Current.Response.Write(clientscript);
        }
        public static void WinClose()
        {
            System.Web.HttpContext.Current.Response.Write("<script language='JavaScript'>  window.close(); </script> ");
        }
        public static void Open(string url)
        {
          //  System.Web.HttpContext.Current.Response.Write("<script language='JavaScript'> newWindow =window.open('" + url + "','" + winName + "','width=" + width + ",top=" + top + ",scrollbars=yes,resizable=yes,toolbar=false,location=false') ; newWindow.focus(); </script> ");
            System.Web.HttpContext.Current.Response.Write("<script language='JavaScript'> var newWindow =window.open('" + url + "','p' ) ; newWindow.focus(); </script> ");
        }
        public static void WinReload()
        {
            System.Web.HttpContext.Current.Response.Write("<script language='JavaScript'>window.parent.main.document.location.reload(); </script> ");
        }
		public static void WinOpen(string url)
		{
            PubClass.WinOpen(url, "", "msg" + DateTime.Now.ToString("MMddHHmmss"), 300, 300);
		}
        public static void WinOpen(string url,int w, int h)
        {
            PubClass.WinOpen(url, "","msg"+DateTime.Now.ToString("MMddHHmmss"), w, h);
        }
		public static void WinOpen(string url,string title,string winName, int width, int height)
		{
            PubClass.WinOpen(url, title, winName, width, height, 100, 200);
		}
        public static void WinOpen(string url, string title, int width, int height)
        {
            PubClass.WinOpen(url, title, "ActivePage", width, height, 100, 200);
        }
        public static void WinOpen(string url, string title, string winName, int width, int height, int top, int left)
        {
            url = url.Replace("<", "[");
            url = url.Replace(">", "]");
            url = url.Trim();
            title = title.Replace("<", "[");
            title = title.Replace(">", "]");
            title = title.Replace("\"", "[");
            HttpContext hcontext = System.Web.HttpContext.Current;
            string flag = hcontext.Request.Params["flag"];
            if (flag.Contains("frame"))
            {
                hcontext.Response.Redirect(url);
                //hcontext.Response.Write("<script language='JavaScript'> window.location.hre' + url +'; </script> ");
            }
            else if (top == 0 && left == 0)
                hcontext.Response.Write("<script language='JavaScript'> var newWindow =window.open('" + url + "','" + winName + "','width=" + width + ",top=" + top + ",scrollbars=yes,resizable=yes,toolbar=false,location=false') ; </script> ");
            else
                hcontext.Response.Write("<script language='JavaScript'> var newWindow =window.open('" + url + "','" + winName + "','width=" + width + ",top=" + top + ",left=" + left + ",height=" + height + ",scrollbars=yes,resizable=yes,toolbar=false,location=false');</script>");
        }
		/// <summary>
		///  Output to the red warning page .
		/// </summary>
		/// <param name="msg"> News </param>
        protected void ResponseWriteRedMsg(string msg)
        {
            //this.Response.Write("<BR><font color='red' size='"+MsgFontSize.ToString()+"' > <b>"+msg+"</b></font>");
            //if (msg.Length < 200)
            //	return ;
            msg = msg.Replace("@", "<BR>@");
            System.Web.HttpContext.Current.Session["info"] = msg;
            string url = "/WF/Comm/Port/ErrorPage.aspx";
            WinOpen(url, " Caveat ", msg + DateTime.Now.ToString("mmss"), 500, 400, 150, 270);
        }
		/// <summary>
		///  Output to the information on the page blue .
		/// </summary>
		/// <param name="msg"> News </param>
		public static void ResponseWriteBlueMsg(string msg)
		{ 
			 
			if (SystemConfig.IsBSsystem)
			{
				msg=msg.Replace("@","<BR>@");
				System.Web.HttpContext.Current.Session["info"]=msg;
				string url= "/WF/Comm/Port/InfoPage.aspx";
				WinOpen(url," Information ","sysmsg",500,400,150,270);
			}
			else
			{
				Log.DebugWriteInfo(msg);
			}
		}
		/// <summary>
		///  Saved successfully 
		/// </summary>
		public static void ResponseWriteBlueMsg_SaveOK()
		{
			//this.Alert(" Saved successfully !");
			 
			ResponseWriteBlueMsg(" Saved successfully !");
		}
		/// <summary>
		/// ResponseWriteBlueMsg_DeleteOK
		/// </summary>
		public static void ResponseWriteBlueMsg_DeleteOK()
		{			
			//this.Alert(" Deleted successfully !");
			ResponseWriteBlueMsg(" Deleted successfully !");
		}
		/// <summary>
		/// ResponseWriteBlueMsg_UpdataOK
		/// </summary>
		public static void ResponseWriteBlueMsg_UpdataOK()
		{			
			// this.Alert(" Update successful !");
			ResponseWriteBlueMsg(" Update successful !");
		}
		/// <summary>
		///  Output to the information on the page black .
		/// </summary>
		/// <param name="msg"> News </param>
		public static void ResponseWriteBlackMsg(string msg)
		{			
			System.Web.HttpContext.Current.Response.Write("<font color='Black' size=5 ><b>"+msg+"</b></font>");
		}
		public static void ResponseSript(string Sript)
		{
			System.Web.HttpContext.Current.Response.Write( Sript );
		}
		public static void ToSignInPage()
		{		 
			System.Web.HttpContext.Current.Response.Redirect(BP.Sys.Glo.Request.ApplicationPath+"/SignIn.aspx?url=/Wel.aspx");
		}
		public static void ToWelPage()
		{
			System.Web.HttpContext.Current.Response.Redirect(BP.Sys.Glo.Request.ApplicationPath+"/Wel.aspx");
		}
		/// <summary>
		///  Information can also switch to the surface .
		/// </summary>
		/// <param name="mess"></param>
        public static void ToErrorPage(string mess)
        {
            System.Web.HttpContext.Current.Session["info"] = mess;
            string path = BP.Sys.Glo.Request.ApplicationPath;
            if (path == "/" || path == "")
                path = "";

            System.Web.HttpContext.Current.Response.Redirect(path + "Comm/Port/InfoPage.aspx");
        }
		/// <summary>
		///  Information can also switch to the surface .
		/// </summary>
		/// <param name="mess"></param>
		public static void ToMsgPage(string mess)
		{		
			mess=mess.Replace("@","<BR>@");
			System.Web.HttpContext.Current.Session["info"]=mess;
			System.Web.HttpContext.Current.Response.Redirect("/WF/Comm/Port/InfoPage.aspx?d="+DateTime.Now.ToString(),false);

			//System.Web.HttpContext.Current.Session["info"]=mess;
			//System.Web.HttpContext.Current.Response.Redirect(BP.Sys.Glo.Request.ApplicationPath+"/Port/InfoPage.aspx",true);
		}
		#endregion 
 
		/// <summary>
		/// Go to a page on . '_top'
		/// </summary>
		/// <param name="mess"></param>
		/// <param name="target">'_top'</param>
		public static void ToErrorPage(string mess, string target)
		{
			System.Web.HttpContext.Current.Session["info"]=mess;

            string path = BP.Sys.Glo.Request.ApplicationPath;
            if (path == "/" || path == "")
                path = "";

			System.Web.HttpContext.Current.Response.Redirect(path+"Comm/Port/InfoPage.aspx target='_top'");
		}
        //public static void AlertSaveOK()
        //{
        //    " Saved successfully ";
        //}

        
		/// <summary>
		///  Need not page  Parameters ,show message
		/// </summary>
		/// <param name="mess"></param>
        public static void Alert(string mess)
        {
            //string msg1 = "<script language=javascript>alert('" + msg + "');</script>";
            //if (! System.Web.HttpContext.Current.ClientScript.IsClientScriptBlockRegistered("a "))
            //    ClientScript.RegisterClientScriptBlock(this.GetType(), "a ", msg1);


            string script = "<script language=JavaScript>alert('" + mess + "');</script>";
            System.Web.HttpContext.Current.Response.Write(script);
       
		    

		    //	System.Web.HttpContext.Current.Response.aps ( script );
		    //  System.Web.HttpContext.Current.Response.Write(script);
        }

		public static void ResponseWriteScript(string script )
		{
			script= "<script language=JavaScript> "+script+"</script>";
			System.Web.HttpContext.Current.Response.Write( script );
		}
		#endregion

	}
}
