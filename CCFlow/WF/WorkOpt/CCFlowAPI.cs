using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using BP.DA;
using BP.Sys;
using BP.Web;
using BP.En;
using BP.WF;
using BP.WF.Data;
using BP.WF.Template;
using BP.Port;
using Silverlight.DataSetConnector;
using System.Drawing.Imaging;
using System.Drawing;
using System.Configuration;
using BP.Tools;

/// <summary>
/// ccflowAPI  The summary 
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow the use of  ASP.NET AJAX  Call this from a script  Web  Service , Please cancel the downlink comment . 
// [System.Web.Script.Services.ScriptService]
public class CCFlowAPI : CCForm
{
    #region  Process api
    /// <summary>
    ///  Reminders 
    /// </summary>
    /// <param name="workid"> Job No. </param>
    /// <param name="msg"> News </param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string Flow_DoPress(Int64 workid, string msg, string userNo)
    {
        if (BP.Web.WebUser.No != userNo)
            BP.WF.Dev2Interface.Port_Login(userNo);
        System.Data.DataSet ds = new DataSet();
        ds.Tables.Add(BP.WF.Dev2Interface.Flow_DoPress(workid, msg, true));
        return BP.DA.DataType.ToJson(ds.Tables[0]);
    }
    #endregion

    #region Port API

    /// <summary>
    ///  Logout 
    /// </summary>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public void Port_SigOut(string userNo)
    {
        BP.WF.Dev2Interface.Port_SigOut();
    }
    /// <summary>
    ///  Gets the menu 
    /// </summary>
    /// <param name="userNo"> User ID </param>
    [WebMethod(EnableSession = true)]
    public string Port_Menu(string userNo)
    {
        BP.WF.XML.Tools xmls = new BP.WF.XML.Tools();
        xmls.RetrieveAll();

        DataSet ds = new DataSet();
        ds.Tables.Add(xmls.ToDataTable());
        //  ds.WriteXml("c:\\Port_Menu Gets the menu .xml");
        //return Connector.ToXml(ds);
        return BP.DA.DataType.ToJson(ds.Tables[0]);
    }
    /// <summary>
    ///  Change Password 
    /// </summary>
    /// <param name="userNo"> Username </param>
    /// <param name="oldPass"> Old Password </param>
    /// <param name="newPass"> New Password </param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string Port_ChangePassword(string userNo, string oldPass, string newPass)
    {
        Emp emp = new Emp(userNo);
        if (emp.Pass == oldPass)
        {
            emp.Pass = newPass;
            emp.Update();
            return " Modified successfully , Keep in mind that your new password .";
        }
        else
        {
            return " Password change fails , Old password error .";
        }
    }
    /// <summary>
    ///  Get Station Letters 
    ///  Return MsgType, Num  Two columns .
    /// MsgType 在 BP.Sys.SMSMsgType Defined . 
    /// </summary>
    /// <param name="userNo"> Personnel Number </param>
    /// <param name="lastTime"> Last accessed time </param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string Port_SMS(string userNo, string lastTime)
    {
        if (BP.Web.WebUser.No != userNo)
            BP.WF.Dev2Interface.Port_Login(userNo);

        Paras ps = new Paras();
        ps.SQL = "SELECT MsgType , Count(*) as Num FROM Sys_SMS WHERE SendTo='" + userNo + "' AND  RDT >'" + lastTime + "' AND MsgType IS NOT NULL Group By MsgType";
        ps.Add(BP.WF.SMSAttr.SendTo, userNo);

        DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(ps);
        return BP.DA.DataType.ToJson(dt);
        //string strs = "";
        //foreach (DataRow dr in dt.Rows)
        //    strs += "@" + dr[0].ToString() + "=" + dr[1].ToString();
        ////return strs;
    }
    /// <summary>
    ///  Get the current system operator message 
    /// </summary>
    /// <param name="userNo"></param>
    /// <param name="lastTime"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string Port_SMS_DB(string userNo, string lastTime)
    {
        if (BP.Web.WebUser.No != userNo)
            BP.WF.Dev2Interface.Port_Login(userNo);

        Paras ps = new Paras();
        ps.SQL = "SELECT * FROM Sys_SMS WHERE SendTo='" + userNo + "' AND  RDT >'" + lastTime + "' ORDER BY RDT ";
        ps.Add(BP.WF.SMSAttr.SendTo, userNo);

        DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(ps);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        return BP.DA.DataType.ToJson(ds.Tables[0]);
        //return Connector.ToXml(ds);
    }
    #endregion Port API

    #region  Interface associated with the data source .
    /// <summary>
    ///  Access to mobile menu 
    /// </summary>
    /// <param name="userNo"> Personnel Number </param>
    /// <returns> Menu json</returns>
    [WebMethod(EnableSession = true)]
    public string DB_MobileMenu(string userNo)
    {
        DataSet ds = new DataSet();
        ds.ReadXml(BP.Sys.SystemConfig.PathOfDataUser + "\\Xml\\Mobile.xml");
        return BP.DA.DataType.ToJson(ds.Tables[0]);
    }
    /// <summary>
    ///  Get Contacts 
    /// </summary>
    /// <param name="DeptNo"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string Address_List()
    {
        /**--- Zhou Peng small modifications  2014-11-05---START**/
        //string sql = " select A.No, A.Name as UserName,B.Name as DeptName,A.Tel, A.Email from WF_Emp as A,Port_Dept as B where A.FK_Dept=B.No order by B.No ";
        string sql = " select A.No, A.Name as UserName,B.Name as DeptName,A.Tel, A.Email from Port_Emp as A,Port_Dept as B where A.FK_Dept=B.No order by B.No ";
        /**--- Zhou Peng small modifications  2014-11-05---END**/
        DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
        dt.Columns.Add("Img", typeof(string));
        foreach (DataRow dr in dt.Rows)
        {

            /**--- Zhou Peng small modifications  2014-09-02---START**/
            // dr["Img"] = "/DataUser/UserIcon/" + dr["No"] + ".png";
            dr["Img"] = "/DataUser/UserIcon/" + dr["No"] + "Smaller.png";
            /**--- Zhou Peng small modifications  2014-09-02---END**/
        }
        return BP.DA.DataType.ToJson(dt);
    }
    /// <summary>
    ///  Access to personal information 
    /// </summary>
    /// <param name="DeptNo"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string UserInfoByNo(string userNo)
    {
        string sql = " select A.No, A.Name as UserName,B.Name as DeptName,A.Tel,A.Email from Port_Emp as A,Port_Dept as B where A.FK_Dept=B.No and A.NO='" + userNo + "' order by B.No  ";
        DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
        dt.Columns.Add("Img", typeof(string));
        foreach (DataRow dr in dt.Rows)
        {
            dr["Img"] = "/DataUser/UserIcon/" + dr["No"] + "Smaller.png";
        }
        return BP.DA.DataType.ToJson(dt);
    }
    /// <summary>
    ///  Change of Personal Information 
    /// </summary>
    /// <param name="DeptNo"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string UserInfoChange(string userNo, string userName, string tel, string email)
    {

        string sSql = "Update Port_Emp set Name='" + userName + "',Tel='" + tel + "',Email='" + email + "' where No='" + userNo + "'";
        int i = BP.DA.DBAccess.RunSQL(sSql);
        if (i > 0)
        {
            return "  Modified successfully ! ";
        }
        else
        {
            return "  Modify failure ! ";
        }
    }
    /// <summary>
    ///  Feedback 
    /// </summary>
    /// <param name="DeptNo"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string WriteUserMsg(string userNo, string msg)
    {
        string path = BP.Sys.SystemConfig.PathOfDataUser + "LogOfUser";
        if (Directory.Exists(path) == false)
            Directory.CreateDirectory(path);
        string dd = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
        string file = path + "\\" + userNo + "_" + dd + ".txt";
        DataType.WriteFile(file, msg);
        return " Feedback success .";
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userNo"></param>
    /// <param name="msg"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string WriteUserLog(string userNo, string msg)
    {
        string path = BP.Sys.SystemConfig.PathOfDataUser + "\\LogOfUser";
        if (Directory.Exists(path) == false)
            Directory.CreateDirectory(path);

        string dd = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
        string file = path + "\\Log_" + userNo + "_" + dd + ".txt";

        DataType.WriteFile(file, msg);

        return " Feedback success .";
    }
    /// <summary>
    ///  Upload Photos 
    /// </summary>
    /// <param name="workid"> The work ID</param>
    /// <param name="bytestr"> Picture </param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string FileUploadImage(string userNo, string bytestr, string smaller, string byImg)
    {
        if (bytestr.Trim() == "")
            return "err:@ File upload failed !";

        try
        {
            string filePath = BP.Sys.SystemConfig.PathOfDataUser + "\\UserIcon\\";
            DirectoryInfo di = new DirectoryInfo(filePath);
            if (!di.Exists)
                di.Create();
            string imgBName = filePath + "" + userNo + "Biger.png";
            string imgSName = filePath + "" + userNo + "Smaller.png";
            string imgName = filePath + "" + userNo + ".png";
            bool imgB = StringToFile(bytestr, imgBName);
            bool imgS = StringToFile(smaller, imgSName);
            bool img = StringToFile(byImg, imgName);
        }
        catch (Exception ex)
        {
            return "err:@" + ex.Message;
        }

        return " Successful operation ";

    }
    /// <summary> 
    ///  Put through base64 Save the file encoded string  
    /// </summary> 
    /// <param name="base64String">经base64 After a string of overweight  </param> 
    /// <param name="fileName"> Path and file name to save the file  </param> 
    /// <returns> Whether to save the file successfully  </returns> 
    public static bool StringToFile(string base64String, string fileName)
    {
        //string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase) + @"/beapp/" + fileName; 
        System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Create);
        System.IO.BinaryWriter bw = new System.IO.BinaryWriter(fs);
        if (!string.IsNullOrEmpty(base64String) && File.Exists(fileName))
        {
            bw.Write(Convert.FromBase64String(base64String));
        }
        bw.Close();
        fs.Close();
        return true;
    }
    /// <summary>
    ///  Get a set of nodes can be returned 
    /// </summary>
    /// <param name="nodeID"> Node ID</param>
    /// <param name="workid"> The work ID</param>
    /// <param name="fid"> Process ID</param>
    /// <returns> Retraction of information </returns>
    [WebMethod(EnableSession = true)]
    public string DB_GenerWillReturnNodes(int nodeID, Int64 workid, Int64 fid, string userNo)
    {
        try
        {
            Emp emp = new Emp(userNo);
            BP.Web.WebUser.SignInOfGener(emp);

            System.Data.DataSet ds = new System.Data.DataSet();
            DataTable table = BP.WF.Dev2Interface.DB_GenerWillReturnNodes(nodeID, workid, fid);
            ds.Tables.Add(table);
            return BP.DA.DataType.ToJson(ds.Tables[0]);
            //return Connector.ToXml(ds);
        }
        catch (Exception ex)
        {
            return "err@" + ex.Message;
        }
    }
    /// <summary>
    ///  Get the process tree 
    /// </summary>
    /// <param name="userNo"></param>
    /// <param name="sid"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string DB_FlowTree(string userNo, string sid)
    {
        try
        {

            Emp emp = new Emp(userNo);
            BP.Web.WebUser.SignInOfGener(emp);

            string sql = "SELECT No, Name, ParentNo FROM WF_FlowSort ";
            DataTable sort = DBAccess.RunSQLReturnTable(sql);
            sort.TableName = "WF_FlowSort";

            string sql1 = "SELECT No, Name, FK_FlowSort as ParentNo FROM WF_Flow ";
            DataTable flow = DBAccess.RunSQLReturnTable(sql1);
            flow.TableName = "WF_Flow";

            DataSet ds = new DataSet();
            ds.Tables.Add(sort);
            ds.Tables.Add(flow);

            return BP.Tools.FormatToJson.ToJson(ds);
        }
        catch (Exception ex)
        {
            return "err@" + ex.Message;
        }
    }
    /// <summary>
    ///  For a list of the process has been completed .
    /// </summary>
    /// <param name="userNo"> User ID </param>
    /// <param name="sid">SID</param>
    /// <returns> Return No,Name,Num Three columns </returns>
    [WebMethod(EnableSession = true)]
    public string DB_FlowCompleteGroup(string userNo, string sid)
    {
        try
        {
            Emp emp = new Emp(userNo);
            BP.Web.WebUser.SignInOfGener(emp);

            System.Data.DataSet ds = new System.Data.DataSet();
            DataTable table = BP.WF.Dev2Interface.DB_FlowCompleteGroup(userNo);
            ds.Tables.Add(table);
            return BP.DA.DataType.ToJson(ds.Tables[0]);
        }
        catch (Exception ex)
        {
            return "err@" + ex.Message;
        }
    }
    /// <summary>
    ///  Obtain flow data has been completed 
    /// </summary>
    /// <param name="userNo"></param>
    /// <param name="sid"></param>
    /// <param name="fk_flow"> Process ID </param>
    /// <param name="pageSize"> Number per page </param>
    /// <param name="pageIdx">第n页</param>
    /// <returns> Return WF_GenerWorklist Data , WorkID,Title,Starter,StarterName,WFState,FK_Node</returns>
    [WebMethod(EnableSession = true)]
    public string DB_FlowComplete(string userNo, string sid, string fk_flow, int pageSize, int pageIdx)
    {
        try
        {

            Emp emp = new Emp(userNo);
            BP.Web.WebUser.SignInOfGener(emp);

            DataTable table = BP.WF.Dev2Interface.DB_FlowComplete(userNo, fk_flow, pageSize, pageIdx);
            return BP.DA.DataType.ToJson(table);
        }
        catch (Exception ex)
        {
            return "err@" + ex.Message;
        }
    }
    /// <summary>
    ///  You can return to the node 
    /// </summary>
    /// <param name="nodeID"></param>
    /// <param name="workid"></param>
    /// <param name="fid"></param>
    /// <param name="userNo"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string DataTable_DB_GenerWillReturnNodes(int nodeID, Int64 workid,
        Int64 fid, string userNo)
    {
        try
        {
            Emp emp = new Emp(userNo);
            BP.Web.WebUser.SignInOfGener(emp);
            System.Data.DataSet ds = new System.Data.DataSet();
            DataTable table = BP.WF.Dev2Interface.DB_GenerWillReturnNodes(nodeID, workid, fid);
            ds.Tables.Add(table);
            return BP.DA.DataType.ToJson(ds.Tables[0]);
            //return Connector.ToXml(ds);
        }
        catch (Exception ex)
        {
            return "err@" + ex.Message;
        }
    }
    /// <summary>
    ///  A list of work to get the task pool 
    /// </summary>
    /// <param name="userNo"> Personnel Number </param>
    /// <returns> A list of work to get the task pool xml</returns>
    [WebMethod(EnableSession = true)]
    public string DB_TaskPool(string userNo)
    {
        if (BP.Web.WebUser.No != userNo)
            BP.WF.Dev2Interface.Port_Login(userNo);

        System.Data.DataSet ds = new System.Data.DataSet();
        ds.Tables.Add(BP.WF.Dev2Interface.DB_TaskPool());
        ds.WriteXml("c:\\DB_TaskPool A list of work to get the task pool .xml");
        return BP.DA.DataType.ToJson(ds.Tables[0]);
        //return Connector.ToXml(ds);
    }
    /// <summary>
    ///  For a list of job I applied down from the task pool 
    /// </summary>
    /// <param name="userNo"> Personnel Number </param>
    /// <returns> For a list of job I applied down from the task pool xml</returns>
    [WebMethod(EnableSession = true)]
    public string DB_TaskPoolOfMyApply(string userNo)
    {
        if (BP.Web.WebUser.No != userNo)
            BP.WF.Dev2Interface.Port_Login(userNo);

        System.Data.DataSet ds = new System.Data.DataSet();
        ds.Tables.Add(BP.WF.Dev2Interface.DB_TaskPoolOfMyApply());
        //ds.WriteXml("c:\\DB_TaskPoolOfMyApply For a list of job I applied down from the task pool .xml");
        return BP.DA.DataType.ToJson(ds.Tables[0]);
        //return Connector.ToXml(ds);
    }
    /// <summary>
    ///  Gets the collection process can be initiated by the current operator 
    /// </summary>
    /// <param name="userNo"> Personnel Number </param>
    /// <returns> Can be initiated xml</returns>
    [WebMethod(EnableSession = true)]
    public string DB_GenerCanStartFlowsOfDataTable(string userNo)
    {
        if (BP.Web.WebUser.No != userNo)
            BP.WF.Dev2Interface.Port_Login(userNo);

        System.Data.DataSet ds = new System.Data.DataSet();
        ds.Tables.Add(BP.WF.Dev2Interface.DB_GenerCanStartFlowsOfDataTable(userNo));

        //DataType.WriteFile("c:\\DB_GenerCanStartFlowsOfDataTable Launch .xml", Connector.ToXml(ds));
        //ds.WriteXml("c:\\aa.xml");
        //string strs = BP.DA.DataType.ReadTextFile("c:\\aa.xml");
        //return strs;
        return BP.DA.DataType.ToJson(ds.Tables[0]);
        //return Connector.ToXml(ds);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userNo"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string DataTable_DB_GenerCanStartFlowsOfDataTable(string userNo)
    {
        if (BP.Web.WebUser.No != userNo)
            BP.WF.Dev2Interface.Port_Login(userNo);

        System.Data.DataSet ds = new System.Data.DataSet();
        ds.Tables.Add(BP.WF.Dev2Interface.DB_GenerCanStartFlowsOfDataTable(userNo));
        return BP.DA.DataType.ToJson(ds.Tables[0]);


        //ds.WriteXml("c:\\aa.xml");
        //string strs = BP.DA.DataType.ReadTextFile("c:\\aa.xml");
        //return strs;
        // return Connector.ToXml(ds);
    }
    /// <summary>
    ///  To-do list 
    /// </summary>
    /// <param name="userNo"> Personnel Number </param>
    /// <returns> To-do list xml</returns>
    [WebMethod(EnableSession = true)]
    public string DataTable_DB_GenerEmpWorksOfDataTable(string userNo)
    {
        if (BP.Web.WebUser.No != userNo)
            BP.WF.Dev2Interface.Port_Login(userNo);
        DataTable dt = BP.WF.Dev2Interface.DB_GenerEmpWorksOfDataTable();

        return BP.DA.DataType.ToJson(dt);
    }
    /// <summary>
    ///  To-do list 
    /// </summary>
    /// <param name="userNo"> Personnel Number </param>
    /// <returns> To-do list xml</returns>
    [WebMethod(EnableSession = true)]
    public string DB_GenerEmpWorksOfDataTable(string userNo)
    {
        if (BP.Web.WebUser.No != userNo)
            BP.WF.Dev2Interface.Port_Login(userNo);

        System.Data.DataSet ds = new System.Data.DataSet();
        ds.Tables.Add(BP.WF.Dev2Interface.DB_GenerEmpWorksOfDataTable());
        return BP.DA.DataType.ToJson(ds.Tables[0]);
        //// ds.WriteXml("c:\\DB_GenerEmpWorksOfDataTable Upcoming .xml");
        //string str = Connector.ToXml(ds);
        ////  BP.DA.DataType.WriteFile("c:\\aaa.xml", str);
        //return str;
    }
    /// <summary>
    ///  CC list 
    /// </summary>
    /// <param name="userNo"> Personnel Number </param>
    /// <returns> Send a list of operations xml</returns>
    [WebMethod(EnableSession = true)]
    public string DB_CCList(string userNo)
    {
        if (BP.Web.WebUser.No != userNo)
            BP.WF.Dev2Interface.Port_Login(userNo);

        System.Data.DataSet ds = new System.Data.DataSet();
        ds.Tables.Add(BP.WF.Dev2Interface.DB_CCList(userNo));
        // ds.WriteXml("c:\\DB_CCList Cc .xml");
        return BP.DA.DataType.ToJson(ds.Tables[0]);
        //return Connector.ToXml(ds);
    }
    /// <summary>
    ///  CC have read execution 
    /// </summary>
    /// <param name="fk_flow"> Process ID </param>
    /// <param name="fk_node"> Process Node </param>
    /// <param name="workID"> The work id</param>
    /// <param name="fid"> Process id</param>
    /// <param name="msge"> Fill views </param>
    [WebMethod(EnableSession = true)]
    public string Node_DoCCCheckNote(string userNo, string sid, string fk_flow, int fk_node, Int64 workID, Int64 fid, string msge)
    {
        if (BP.Web.WebUser.No != userNo)
            BP.WF.Dev2Interface.Port_Login(userNo);

        //BP.WF.Dev2Interface.Node_DoCCCheckNote(fk_flow, fk_node, workID, fid, msge);
        return " Have read complete ";
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="flowNo"></param>
    /// <param name="WorkID"></param>
    /// <param name="FID"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string DB_Truck(string flowNo, Int64 WorkID, Int64 FID)
    {
        string sqlOfWhere2 = "";
        string sqlOfWhere1 = "";

        string dbStr = BP.Sys.SystemConfig.AppCenterDBVarStr;
        Paras prs = new Paras();
        if (FID == 0)
        {
            sqlOfWhere1 = " WHERE (FID=" + dbStr + "WorkID11 OR WorkID=" + dbStr + "WorkID12 )  ";
            prs.Add("WorkID11", WorkID);
            prs.Add("WorkID12", WorkID);
        }
        else
        {
            sqlOfWhere1 = " WHERE (FID=" + dbStr + "FID11 OR WorkID=" + dbStr + "FID12 ) ";
            prs.Add("FID11", FID);
            prs.Add("FID12", FID);
        }

        string sql = "";
        sql = "SELECT MyPK,ActionType,ActionTypeText,FID,WorkID,NDFrom,NDFromT,NDTo,NDToT,EmpFrom,EmpFromT,EmpTo,EmpToT,RDT,WorkTimeSpan,Msg,NodeData,Exer FROM ND" + int.Parse(flowNo) + "Track " + sqlOfWhere1 + " ORDER BY RDT";
        prs.SQL = sql;

        DataTable dt = DBAccess.RunSQLReturnTable(prs);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        return BP.DA.DataType.ToJson(ds.Tables[0]);
        //return Connector.ToXml(ds);
    }
    /// <summary>
    ///  Pending List 
    /// </summary>
    /// <param name="userNo"> Personnel Number </param>
    /// <returns> Pending List xml</returns>
    [WebMethod(EnableSession = true)]
    public string DB_GenerHungUpList(string userNo)
    {
        System.Data.DataSet ds = new System.Data.DataSet();
        ds.Tables.Add(BP.WF.Dev2Interface.DB_GenerHungUpList(userNo));
        // ds.WriteXml("c:\\DB_GenerCanStartFlowsOfDataTable Pending .xml");
        return BP.DA.DataType.ToJson(ds.Tables[0]);
        //return Connector.ToXml(ds);
    }
    /// <summary>
    ///  In-transit list 
    /// </summary>
    /// <param name="userNo"> Personnel Number </param>
    /// <returns> In-transit list xml</returns>
    [WebMethod(EnableSession = true)]
    public string DB_GenerRuning(string userNo)
    {

        if (BP.Web.WebUser.No != userNo)
            BP.WF.Dev2Interface.Port_Login(userNo);

        System.Data.DataSet ds = new System.Data.DataSet();
        ds.Tables.Add(BP.WF.Dev2Interface.DB_GenerRuning());
        //ds.WriteXml("c:\\DB_GenerRuning In-transit list .xml");
        return BP.DA.DataType.ToJson(ds.Tables[0]);
        //return Connector.ToXml(ds);
    }
    #endregion  Interface associated with the data source .

    public CCFlowAPI()
    {
        // If you are using design components , Uncomment the following line  
        //InitializeComponent(); 
    }
    /// <summary>
    ///  Create a blank WorkID
    /// </summary>
    /// <param name="flowNo"> Process ID </param>
    /// <param name="starter"> Sponsor </param>
    /// <param name="title"> Title </param>
    /// <returns>workid</returns>
    [WebMethod(EnableSession = true)]
    public string Node_CreateBlankWork(string flowNo, string starter, string title)
    {
        if (WebUser.No != starter)
        {
            BP.WF.Dev2Interface.Port_Login(starter);
            //throw new Exception("@ Currently logged on user non- (" + WebUser.No + ")");
        }
        System.Data.DataSet ds = new System.Data.DataSet();
        ds.Tables.Add(BP.WF.Dev2Interface.Node_CreateBlankWork(flowNo, null, null, starter, title).ToString());
        return BP.DA.DataType.ToJson(ds.Tables[0]);
    }
    /// <summary>
    ///  Delete  
    /// </summary>
    /// <param name="mypk"></param>
    /// <returns></returns>
    ///   Zhou Peng small modifications  2014-09-02  Modify the return value added 
    [WebMethod(EnableSession = true)]
    public string Node_CC_DoDel(string mypk)
    {
        BP.WF.Dev2Interface.Node_CC_DoDel(mypk);
        /**--- Zhou Peng small modifications  2014-09-02---START**/
        return " Deleted successfully !";
        /**--- Zhou Peng small modifications  2014-09-02---END**/
    }
    /// <summary>
    ///  Set read 
    /// </summary>
    /// <param name="mypk"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string Node_CC_SetRead(string mypks)
    {
        string[] strs = mypks.Split(',');
        foreach (string str in strs)
        {
            BP.WF.Dev2Interface.Node_CC_SetRead(str);
        }
        return null;
    }


    /// <summary>
    ///  Execution Cc 
    /// </summary>
    /// <param name="fk_flow"> Process ID </param>
    /// <param name="fk_node"> Node number </param>
    /// <param name="workID"> The work ID</param>
    /// <param name="toEmpNo"> Copied to staff numbers , For example, multiple separated by commas  zhangsan,lisi</param>
    /// <param name="msgTitle"> Message title </param>
    /// <param name="msgDoc"> Message content </param>
    /// <param name="pFlowNo"> Parent process ID ( For null)</param>
    /// <param name="pWorkID"> Parent process WorkID( For 0)</param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string Node_CC(string userNo, string sid, string fk_flow, int fk_node, Int64 workID, string toEmpNos, string msgTitle, string msgDoc, string pFlowNo, Int64 pWorkID)
    {
        if (WebUser.No != userNo)
            BP.WF.Dev2Interface.Port_Login(userNo);


        toEmpNos = toEmpNos.Replace(";", ",");
        toEmpNos = toEmpNos.Replace(";", ",");
        toEmpNos = toEmpNos.Replace(",", ",");

        string[] toEmps = toEmpNos.Split(',');
        string strs = "";
        foreach (string item in toEmps)
        {
            if (string.IsNullOrEmpty(item) == true)
                continue;
            Emp emp = new Emp(item);
            strs += emp.Name + " ";

            BP.WF.Dev2Interface.Node_CC(fk_flow, fk_node, workID, emp.No, emp.Name, msgTitle, msgDoc, pFlowNo, pWorkID);
        }

        return " The successful implementation of the CC , Copied to :" + strs;
    }
    /// <summary>
    ///  Set the current work status as a draft , If you enable the draft , Please increase it in the form on the Save button start node .
    ///  Must be invoked at the start node .
    /// </summary>
    /// <param name="fk_flow"> Process ID </param>
    /// <param name="workID"> The work ID</param>
    [WebMethod(EnableSession = true)]
    public void Node_SetDraft(string fk_flow, Int64 workID)
    {
        BP.WF.Dev2Interface.Node_SetDraft(fk_flow, workID);
    }
    /// <summary>
    ///  Set work has been read 
    /// </summary>
    /// <param name="nodeID"></param>
    /// <param name="workids"></param>
    /// <returns></returns>
    public string Node_SetWorkRead(int nodeID, string workids)
    {
        string[] strs = workids.Split(',');
        foreach (string str in strs)
        {
            if (string.IsNullOrEmpty(str))
                continue;

            BP.WF.Dev2Interface.Node_SetWorkRead(nodeID, Int64.Parse(str));

        }
        return null;
    }

    /// <summary>
    ///  Node work unsuspend 
    /// </summary>
    /// <param name="fk_flow"> Process ID </param>
    /// <param name="workid"> The work ID</param>
    /// <param name="msg"> Unsuspend reason </param>
    /// <returns> Execution information </returns>
    [WebMethod(EnableSession = true)]
    public void Node_UnHungUpWork(string fk_flow, Int64 workid, string msg)
    {
        BP.WF.Dev2Interface.Node_UnHungUpWork(fk_flow, workid, msg);
    }
    /// <summary>
    ///  Send revocation 
    /// </summary>
    /// <param name="fk_flow"></param>
    /// <param name="workid"></param>
    [WebMethod(EnableSession = true)]
    public string Flow_DoUnSend(string fk_flow, Int64 workid, string userNo)
    {
        if (BP.Web.WebUser.No != userNo)
            BP.WF.Dev2Interface.Port_Login(userNo);
        try
        {
            BP.WF.Dev2Interface.Flow_DoUnSend(fk_flow, workid);
            return " Revocation of success .";
        }
        catch (Exception ex)
        {
            return " Revocation failure :" + ex.Message;
        }
    }
    /// <summary>
    ///  Node work hangs 
    /// </summary>
    /// <param name="fk_flow"> Process ID </param>
    /// <param name="workid"> The work ID</param>
    /// <param name="way"> Suspend Mode </param>
    /// <param name="reldata"> Lifting suspend date ( Can be empty )</param>
    /// <param name="hungNote"> Pending reason </param>
    /// <returns> Returns execution information </returns>
    [WebMethod(EnableSession = true)]
    public string Node_HungUpWork(string fk_flow, Int64 workid, int wayInt, string reldata, string hungNote)
    {
        return BP.WF.Dev2Interface.Node_HungUpWork(fk_flow, workid, wayInt, reldata, hungNote);
    }
    /// <summary>
    ///  Request a shared mission 
    /// </summary>
    /// <param name="workid"> The work ID</param>
    /// <param name="toEmp"> Transferred to the staff ( Only to be handed over to a person )</param>
    /// <param name="msg"> Transfer news </param>
    [WebMethod(EnableSession = true)]
    public string Node_TaskPoolTakebackOne(Int64 workID, string userNo)
    {
        if (WebUser.No != userNo)
            BP.WF.Dev2Interface.Port_Login(userNo);

        BP.WF.Dev2Interface.Node_TaskPoolTakebackOne(workID);
        return " Application is successful !";
    }
    /// <summary>
    ///  Request a shared mission 
    /// </summary>
    /// <param name="workid"> The work ID</param>
    /// <param name="toEmp"> Transferred to the staff ( Only to be handed over to a person )</param>
    /// <param name="msg"> Transfer news </param>
    [WebMethod(EnableSession = true)]
    public string Node_TaskPoolPutOne(Int64 workID, string userNo)
    {
        if (WebUser.No != userNo)
            BP.WF.Dev2Interface.Port_Login(userNo);

        BP.WF.Dev2Interface.Node_TaskPoolPutOne(workID);
        return " Application is successful !";
    }
    /// <summary>
    ///  Job transfer 
    /// </summary>
    /// <param name="workid"> The work ID</param>
    /// <param name="toEmp"> Transferred to the staff ( Only to be handed over to a person )</param>
    /// <param name="msg"> Transfer news </param>
    [WebMethod(EnableSession = true)]
    public string Node_Shift(string flowNo, int nodeID, Int64 workID, Int64 fid, string toEmp, string msg, string userNo, string sid)
    {
        if (WebUser.No != userNo)
            BP.WF.Dev2Interface.Port_Login(userNo);
        return BP.WF.Dev2Interface.Node_Shift(flowNo, nodeID, workID, fid, toEmp, msg);
    }

    /**--- Adding small Zhou Peng  2014-09-22---START**/
    /// <summary>
    ///  Revocation of the transfer 
    /// </summary>
    /// <param name="fk_flow"> Process </param>
    /// <param name="workid"> The work ID</param>
    /// <param name="userNo"> User </param>
    /// <param name="sid"> Security code </param>
    [WebMethod(EnableSession = true)]
    public string Un_Node_Shift(string userNo, string sid, string fk_flow, Int64 workID)
    {
        this.LetUserLogin(userNo, sid);

        string resultMsg = null;
        try
        {
            WorkFlow mwf = new WorkFlow(fk_flow, workID);
            string str = mwf.DoUnShift();

            resultMsg = str;
        }
        catch (Exception ex)
        {
            resultMsg = "err: @ Undo failure , Failure information :" + ex.Message;
        }
        return resultMsg;
    }
    /**--- Adding small Zhou Peng  2014-09-22---END**/
    /// <summary>
    ///  Implementation of return ( Returned to the specified point )
    /// </summary>
    /// <param name="fk_flow"> Process ID </param>
    /// <param name="workID"> The work ID</param>
    /// <param name="fid"> Process ID</param>
    /// <param name="currentNodeID"> The current node ID</param>
    /// <param name="returnToNodeID"> Return to work ID</param>
    /// <param name="msg"> Reason for the return </param>
    /// <param name="isBackToThisNode"> Do you want to backtrack after return ?</param>
    /// <returns> Execution results , The results must be presented to the user .</returns>
    [WebMethod(EnableSession = true)]
    public string Node_ReturnWork(string fk_flow, Int64 workID, Int64 fid, int currentNodeID,
        int returnToNodeID, string returnToEmp, string msg, bool isBackToThisNode, string userNo, string sid)
    {
        try
        {
            // Allowing users to log on .
            LetUserLogin(userNo, sid);
            return BP.WF.Dev2Interface.Node_ReturnWork(fk_flow, workID, fid, currentNodeID, returnToNodeID, returnToEmp, msg, isBackToThisNode);
        }
        catch (Exception ex)
        {
            return "err@" + ex.Message;
        }
    }
    public string DataSetToXml(DataSet ds)
    {
        string strs = "";
        strs += "<DataSet>";
        foreach (DataTable dt in ds.Tables)
        {
            strs += "\t\n<" + dt.TableName + ">";
            foreach (DataRow dr in dt.Rows)
            {
                strs += "\t\n< ";
                foreach (DataColumn dc in dt.Columns)
                {
                    strs += dc.ColumnName + "='" + dr[dc.ColumnName] + "' ";
                }
                strs += "/>";
            }
            strs += "\t\n</" + dt.TableName + ">";
        }
        strs += "\t\n</DataSet>";
        return strs;
    }
    /// <summary>
    ///  Upcoming tips 
    /// </summary>
    /// <param name="userNo"></param>
    /// <returns></returns>
    [WebMethod]
    public string AlertString(string userNo)
    {
        return "@EmpWorks=12@CC=34";
    }
    /// <summary>
    ///  User Login 
    /// 0, Username Password error 
    ///  Returns a long string that identifies the login is successful , Identify the local logon security code .
    /// 2, Server Error .
    /// </summary>
    /// <param name="userNo"></param>
    /// <param name="pass"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string Port_Login(string userNo, string pass)
    {
        try
        {
            Emp emp = new Emp();
            emp.No = userNo;

            if (emp.RetrieveFromDBSources() == 0)
                return "0"; // No inquiry into .

            if (emp.CheckPass(pass) == false)
                return "1"; //  Wrong password .

            return BP.WF.Dev2Interface.Port_GetSidName(userNo);
        }
        catch (Exception ex)
        {
            //  Database connections are not .
            Log.DefaultLogWriteLineError(ex.Message);
            return "2";
        }
    }
    /// <summary>
    ///  Set up SID
    /// </summary>
    /// <param name="userNo"> User ID </param>
    /// <param name="sid">SID号</param>
    public void Port_SetSID(string userNo, string sid)
    {
        //   BP.WF.Dev2Interface.Port_SetSID(userNo, sid);
    }
    /// <summary>
    ///  Information execution 
    /// </summary>
    /// <param name="flag"> Marked execution </param>
    /// <param name="val0"></param>
    /// <param name="val1"></param>
    /// <param name="val2"></param>
    /// <param name="val3"></param>
    /// <param name="val4"></param>
    /// <param name="val5"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string DoIt(string flag, string userNo, string sid, string fk_flow, string workID, string msg, string delModel, string val4, string val5)
    {
        LetUserLogin(userNo, sid);
        try
        {

            switch (flag)
            {
                case "UnSend": // Send revocation ..
                    return BP.WF.Dev2Interface.Flow_DoUnSend(fk_flow, Int64.Parse(workID));
                case "EndWorkFlow": // End Process .
                    return BP.WF.Dev2Interface.Flow_DoFlowOver(fk_flow, Int64.Parse(workID), msg);
                case "DelWorkFlow": // Delete Process .
                    string model = delModel;
                    if (model == "1")
                    {
                        /* Tombstone */
                        return BP.WF.Dev2Interface.Flow_DoDeleteFlowByFlag(fk_flow, Int64.Parse(workID), msg, false);
                    }
                    if (model == "2")
                    {
                        /* Written to the log, delete the */
                        return BP.WF.Dev2Interface.Flow_DoDeleteFlowByWriteLog(fk_flow, Int64.Parse(workID), msg, false);
                    }
                    if (model == "3")
                    {
                        /* Completely remove */
                        return BP.WF.Dev2Interface.Flow_DoDeleteFlowByReal(fk_flow, Int64.Parse(workID), false);
                    }
                    throw new Exception("@ No judgment delete mode ." + delModel);
                default:
                    throw new Exception("@ No agreement marks :" + flag);
            }
        }
        catch (Exception ex)
        {
            return "err:" + ex.Message;
        }
    }
    /// <summary>
    ///  Get the trajectory generation process flow data table track.
    /// </summary>
    /// <param name="fk_flow"></param>
    /// <param name="workID"></param>
    /// <param name="fid"></param>
    /// <param name="userNo"></param>
    /// <param name="sid"></param>
    /// <returns> Return process needs to use something </returns>
    [WebMethod(EnableSession = true)]
    public string GenerFlowTrack_Josn(string fk_flow, Int64 workID, Int64 fid, string userNo, string sid)
    {
        DataSet ds = BP.WF.Dev2Interface.DB_GenerTrack(fk_flow, workID, fid);
        return BP.Tools.FormatToJson.ToJson(ds);
    }
    /// <summary>
    ///  Get a work to be done 
    /// </summary>
    /// <param name="fk_flow"> Job No. </param>
    /// <param name="fk_node"> Node number </param>
    /// <param name="workID"> The work ID</param>
    /// <param name="userNo"> Operator number </param>
    /// <returns>string的json</returns>
    [WebMethod(EnableSession = true)]
    public string GenerWorkNode_JSON(string fk_flow, int fk_node, Int64 workID, Int64 fid, string userNo, string sid)
    {
        this.LetUserLogin(userNo, sid);
        DataSet ds = this.GenerWorkNode(fk_flow, fk_node, workID, fid, userNo);
        return BP.Tools.FormatToJson.ToJson(ds);
    }
    private DataSet GenerWorkNode(string fk_flow, int fk_node, Int64 workID, Int64 fid, string userNo)
    {
        if (fk_node == 0)
            fk_node = int.Parse(fk_flow + "01");

        if (workID == 0)
            workID = BP.WF.Dev2Interface.Node_CreateBlankWork(fk_flow, null, null, userNo, null);

        try
        {
            Emp emp = new Emp(userNo);
            BP.Web.WebUser.SignInOfGener(emp);

            MapData md = new MapData();
            md.No = "ND" + fk_node;
            if (md.RetrieveFromDBSources() == 0)
                throw new Exception(" Load error , The form ID=" + md.No + " Lose , Please reload the repair process once again .");

            DataSet myds = md.GenerHisDataSet();

            #region  Process setup information .
            Node nd = new Node(fk_node);

            if (nd.IsStartNode == false)
                BP.WF.Dev2Interface.Node_SetWorkRead(fk_node, workID);

            //  Node data .
            string sql = "SELECT * FROM WF_Node WHERE NodeID=" + fk_node;
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "WF_NodeBar";
            myds.Tables.Add(dt);

            //  Process data .
            Flow fl = new Flow(fk_flow);
            myds.Tables.Add(fl.ToDataTableField("WF_Flow"));
            #endregion  Process setup information .

            #region  The primary data from the table into the inside .
            //. Data put inside to work ,  To perform a pre-loaded into the event before filling .
            BP.WF.Work wk = nd.HisWork;
            wk.OID = workID;
            wk.RetrieveFromDBSources();

            //  Processing the passed parameter .
            foreach (string k in System.Web.HttpContext.Current.Request.QueryString.AllKeys)
            {
                wk.SetValByKey(k, System.Web.HttpContext.Current.Request.QueryString[k]);
            }

            //  Perform a load before filling .
            string msg = md.FrmEvents.DoEventNode(FrmEventList.FrmLoadBefore, wk);
            if (string.IsNullOrEmpty(msg) == false)
                throw new Exception(" Error :" + msg);

            wk.ResetDefaultVal();
            myds.Tables.Add(wk.ToDataTableField(md.No));

            // The data is placed in the annex .
            if (md.FrmAttachments.Count > 0)
            {
                sql = "SELECT * FROM Sys_FrmAttachmentDB where RefPKVal=" + workID + " AND FK_MapData='ND" + fk_node + "'";
                dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
                dt.TableName = "Sys_FrmAttachmentDB";
                myds.Tables.Add(dt);
            }
            //  Image Attachment data into 
            if (md.FrmImgAths.Count > 0)
            {
                sql = "SELECT * FROM Sys_FrmImgAthDB where RefPKVal=" + workID + " AND FK_MapData='ND" + fk_node + "'";
                dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
                dt.TableName = "Sys_FrmImgAthDB";
                myds.Tables.Add(dt);
            }

            // Place the data table from .
            if (md.MapDtls.Count > 0)
            {
                foreach (MapDtl dtl in md.MapDtls)
                {
                    GEDtls dtls = new GEDtls(dtl.No);
                    QueryObject qo = null;
                    try
                    {
                        qo = new QueryObject(dtls);
                        switch (dtl.DtlOpenType)
                        {
                            case DtlOpenType.ForEmp:  //  By staff to control .
                                qo.AddWhere(GEDtlAttr.RefPK, workID);
                                qo.addAnd();
                                qo.AddWhere(GEDtlAttr.Rec, WebUser.No);
                                break;
                            case DtlOpenType.ForWorkID: //  By Job ID To control 
                                qo.AddWhere(GEDtlAttr.RefPK, workID);
                                break;
                            case DtlOpenType.ForFID: //  By the process ID To control .
                                qo.AddWhere(GEDtlAttr.FID, workID);
                                break;
                        }
                    }
                    catch
                    {
                        dtls.GetNewEntity.CheckPhysicsTable();
                    }
                    DataTable dtDtl = qo.DoQueryToTable();

                    //  Set the default value for the list .
                    MapAttrs dtlAttrs = new MapAttrs(dtl.No);
                    foreach (MapAttr attr in dtlAttrs)
                    {
                        // Handle its default value .
                        if (attr.DefValReal.Contains("@") == false)
                            continue;

                        foreach (DataRow dr in dtDtl.Rows)
                            dr[attr.KeyOfEn] = attr.DefVal;
                    }

                    dtDtl.TableName = dtl.No; // Change the name list of .
                    myds.Tables.Add(dtDtl); // Join this list ,  If no data ,xml Reflected empty .
                }
            }
            #endregion

            #region  The foreign key table join DataSet
            DataTable dtMapAttr = myds.Tables["Sys_MapAttr"];
            foreach (DataRow dr in dtMapAttr.Rows)
            {
                string lgType = dr["LGType"].ToString();
                if (lgType != "2")
                    continue;

                string UIIsEnable = dr["UIIsEnable"].ToString();
                if (UIIsEnable == "0")
                    continue;

                string uiBindKey = dr["UIBindKey"].ToString();
                if (string.IsNullOrEmpty(uiBindKey) == true)
                {
                    string myPK = dr["MyPK"].ToString();
                    /* If it is empty */
                    throw new Exception("@ Attribute field data is incomplete , Process :" + fl.No + fl.Name + ", Node :" + nd.NodeID + nd.Name + ", Property :" + myPK + ", UIBindKey IsNull ");
                }

                //  Determine whether there .
                if (myds.Tables.Contains(uiBindKey) == true)
                    continue;

                myds.Tables.Add(BP.Sys.PubClass.GetDataTableByUIBineKey(uiBindKey));
            }
            #endregion End The foreign key table join DataSet

            #region  The flow of information into the inside .
            // The flow of information form and send it in the past .
            GenerWorkFlow gwf = new GenerWorkFlow();
            gwf.WorkID = workID;
            gwf.RetrieveFromDBSources();

            myds.Tables.Add(gwf.ToDataTableField("WF_GenerWorkFlow"));

            if (gwf.WFState == WFState.Shift)
            {
                // If you are forwarding .
                BP.WF.ShiftWorks fws = new ShiftWorks();
                fws.Retrieve(ShiftWorkAttr.WorkID, workID, ShiftWorkAttr.FK_Node, fk_node);
                myds.Tables.Add(fws.ToDataTableField("WF_ShiftWork"));
            }

            if (gwf.WFState == WFState.ReturnSta)
            {
                // If it is returned .
                ReturnWorks rts = new ReturnWorks();
                rts.Retrieve(ReturnWorkAttr.WorkID, workID,
                    ReturnWorkAttr.ReturnToNode, fk_node,
                    ReturnWorkAttr.RDT);
                myds.Tables.Add(rts.ToDataTableField("WF_ReturnWork"));
            }

            if (gwf.WFState == WFState.HungUp)
            {
                // If it hangs .
                HungUps hups = new HungUps();
                hups.Retrieve(HungUpAttr.WorkID, workID, HungUpAttr.FK_Node, fk_node);
                myds.Tables.Add(hups.ToDataTableField("WF_HungUp"));
            }

            //if (gwf.WFState == WFState.Askfor)
            //{
            //    // If endorsement .
            //    BP.WF.ShiftWorks fws = new ShiftWorks();
            //    fws.Retrieve(ShiftWorkAttr.WorkID, workID, ShiftWorkAttr.FK_Node, fk_node);
            //    myds.Tables.Add(fws.ToDataTableField("WF_ShiftWork"));
            //}

            Int64 wfid = workID;
            if (fid != 0)
                wfid = fid;


            // Add track Information .
            Paras ps = new Paras();
            ps.SQL = "SELECT * FROM ND" + int.Parse(fk_flow) + "Track WHERE WorkID=" + BP.Sys.SystemConfig.AppCenterDBVarStr + "WorkID";
            ps.Add("WorkID", wfid);
            DataTable dtNode = DBAccess.RunSQLReturnTable(ps);
            dtNode.TableName = "Track";
            myds.Tables.Add(dtNode);

            // Staff List , Components for auditing .
            ps = new Paras();
            ps.SQL = "SELECT * FROM  WF_GenerWorkerlist WHERE WorkID=" + BP.Sys.SystemConfig.AppCenterDBVarStr + "WorkID";
            ps.Add("WorkID", wfid);
            DataTable dtGenerWorkerlist = DBAccess.RunSQLReturnTable(ps);
            dtGenerWorkerlist.TableName = "WF_GenerWorkerlist";
            myds.Tables.Add(dtGenerWorkerlist);

            // Add CCList Information .  Components for auditing .
            ps = new Paras();
            ps.SQL = "SELECT * FROM WF_CCList WHERE WorkID=" + BP.Sys.SystemConfig.AppCenterDBVarStr + "WorkID";
            ps.Add("WorkID", wfid);
            DataTable dtCCList = DBAccess.RunSQLReturnTable(ps);
            dtCCList.TableName = "WF_CCList";
            myds.Tables.Add(dtCCList);

            // Add WF_SelectAccper Information .  Components for auditing .
            ps = new Paras();
            ps.SQL = "SELECT * FROM WF_SelectAccper WHERE WorkID=" + BP.Sys.SystemConfig.AppCenterDBVarStr + "WorkID";
            ps.Add("WorkID", wfid);
            DataTable dtSelectAccper = DBAccess.RunSQLReturnTable(ps);
            dtSelectAccper.TableName = "WF_SelectAccper";
            myds.Tables.Add(dtSelectAccper);

            // Put all the node information .  Components for auditing .
            ps = new Paras();
            ps.SQL = "SELECT * FROM WF_Node WHERE FK_Flow=" + BP.Sys.SystemConfig.AppCenterDBVarStr + "FK_Flow ORDER BY " + NodeAttr.Step;
            ps.Add("FK_Flow", fk_flow);
            DataTable dtNodes = DBAccess.RunSQLReturnTable(ps);
            dtNodes.TableName = "Nodes";
            myds.Tables.Add(dtNodes);

            #endregion  The flow of information into the inside .

            return myds;
        }
        catch (Exception ex)
        {
            Log.DebugWriteError(ex.StackTrace);
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    ///  Get a work to be done 
    /// </summary>
    /// <param name="fk_flow"> Job No. </param>
    /// <param name="fk_node"> Node number </param>
    /// <param name="workID"> The work ID</param>
    /// <param name="userNo"> Operator number </param>
    /// <returns>string的json</returns>
    [WebMethod(EnableSession = true)]
    public string GenerWorkNode_JSONV2(string fk_flow, int fk_node, Int64 workID, Int64 fid, bool isCc, float srcWidth, float srcHeight, string userNo, string sid)
    {
        this.LetUserLogin(userNo, sid);
        DataSet ds = this.GenerWorkNodeV2(fk_flow, fk_node, workID, fid, isCc, srcWidth, srcHeight);
        return BP.Tools.FormatToJson.ToJson(ds);
    }

    private DataSet GenerWorkNodeV2(string fk_flow, int fk_node, Int64 workID, Int64 fid, bool iscc, float srcWidth, float srcHeight)
    {
        if (fk_node == 0)
            fk_node = int.Parse(fk_flow + "01");

        if (workID == 0)
            workID = BP.WF.Dev2Interface.Node_CreateBlankWork(fk_flow, null, null, WebUser.No, null);

        try
        {
            MapData md = new MapData();
            md.No = "ND" + fk_node;
            if (md.RetrieveFromDBSources() == 0)
                throw new Exception(" Load error , The form ID=" + md.No + " Lose , Please reload the repair process once again .");

            DataSet myds = new DataSet(); // md.GenerHisDataSet();

            #region  Process setup information .
            Node nd = new Node(fk_node);


            // Process data .  Calculated form of displacement .
            string sql = "SELECT  Ver as FlowVer, '" + md.Ver + "' as FormVer, " + MapData.GenerSpanWeiYi(md, srcWidth) + " as WeiYi, " + MapData.GenerSpanHeight(md, srcHeight) + " as SrcH, " + MapData.GenerSpanWidth(md, srcWidth) + " as SrcW  FROM WF_Flow WHERE No='" + fk_flow + "'";
            DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
            dt.TableName = "BaseInfo";

            //  Increase parameter .
            dt.Columns.Add("WeiYi2", typeof(float));
            dt.Columns.Add("SrcH2", typeof(float));
            dt.Columns.Add("SrcW2", typeof(float));

            dt.Rows[0]["WeiYi2"] = MapData.GenerSpanWeiYi(md, srcHeight);
            dt.Rows[0]["SrcH2"] = MapData.GenerSpanHeight(md, srcWidth);
            dt.Rows[0]["SrcW2"] = MapData.GenerSpanWidth(md, srcHeight);

            myds.Tables.Add(dt);
            #endregion  Process setup information .

            #region  The primary data from the table into the inside .
            //. Data put inside to work ,  To perform a pre-loaded into the event before filling .
            BP.WF.Work wk = nd.HisWork;
            wk.OID = workID;
            wk.RetrieveFromDBSources();

            //  Processing the passed parameter .
            foreach (string k in System.Web.HttpContext.Current.Request.QueryString.AllKeys)
            {
                wk.SetValByKey(k, System.Web.HttpContext.Current.Request.QueryString[k]);
            }

            //  Perform a load before filling .
            string msg = md.FrmEvents.DoEventNode(FrmEventList.FrmLoadBefore, wk);
            if (string.IsNullOrEmpty(msg) == false)
                throw new Exception(" Error :" + msg);

            wk.ResetDefaultVal();
            myds.Tables.Add(wk.ToDataTableField(md.No));

            // The data is placed in the annex .
            if (md.FrmAttachments.Count > 0)
            {
                sql = "SELECT * FROM Sys_FrmAttachmentDB where RefPKVal=" + workID + " AND FK_MapData='ND" + fk_node + "'";
                dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
                dt.TableName = "Sys_FrmAttachmentDB";
                myds.Tables.Add(dt);
            }
            //  Image Attachment data into 
            if (md.FrmImgAths.Count > 0)
            {
                sql = "SELECT * FROM Sys_FrmImgAthDB where RefPKVal=" + workID + " AND FK_MapData='ND" + fk_node + "'";
                dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
                dt.TableName = "Sys_FrmImgAthDB";
                myds.Tables.Add(dt);
            }

            // Place the data table from .
            if (md.MapDtls.Count > 0)
            {
                foreach (MapDtl dtl in md.MapDtls)
                {
                    GEDtls dtls = new GEDtls(dtl.No);
                    QueryObject qo = null;
                    try
                    {
                        qo = new QueryObject(dtls);
                        switch (dtl.DtlOpenType)
                        {
                            case DtlOpenType.ForEmp:  //  By staff to control .
                                qo.AddWhere(GEDtlAttr.RefPK, workID);
                                qo.addAnd();
                                qo.AddWhere(GEDtlAttr.Rec, WebUser.No);
                                break;
                            case DtlOpenType.ForWorkID: //  By Job ID To control 
                                qo.AddWhere(GEDtlAttr.RefPK, workID);
                                break;
                            case DtlOpenType.ForFID: //  By the process ID To control .
                                qo.AddWhere(GEDtlAttr.FID, workID);
                                break;
                        }
                    }
                    catch
                    {
                        dtls.GetNewEntity.CheckPhysicsTable();
                    }
                    DataTable dtDtl = qo.DoQueryToTable();

                    //  Set the default value for the list .
                    MapAttrs dtlAttrs = new MapAttrs(dtl.No);
                    foreach (MapAttr attr in dtlAttrs)
                    {
                        // Handle its default value .
                        if (attr.DefValReal.Contains("@") == false)
                            continue;

                        foreach (DataRow dr in dtDtl.Rows)
                            dr[attr.KeyOfEn] = attr.DefVal;
                    }

                    dtDtl.TableName = dtl.No; // Change the name list of .
                    myds.Tables.Add(dtDtl); // Join this list ,  If no data ,xml Reflected empty .
                }
            }
            #endregion

            #region  The foreign key table join  DataSet
            DataTable dtMapAttr = md.GenerHisDataSet().Tables["Sys_MapAttr"];
            foreach (DataRow dr in dtMapAttr.Rows)
            {
                string lgType = dr["LGType"].ToString();
                if (lgType != "2")
                    continue;

                string UIIsEnable = dr["UIIsEnable"].ToString();
                if (UIIsEnable == "0")
                    continue;

                string uiBindKey = dr["UIBindKey"].ToString();
                if (string.IsNullOrEmpty(uiBindKey) == true)
                {
                    string myPK = dr["MyPK"].ToString();
                    /* If it is empty */
                    throw new Exception("@ Attribute field data is incomplete , Process :" + fk_flow + ", Node :" + nd.NodeID + nd.Name + ", Property :" + myPK + ", UIBindKey IsNull ");
                }

                //  Determine whether there .
                if (myds.Tables.Contains(uiBindKey) == true)
                    continue;

                myds.Tables.Add(BP.Sys.PubClass.GetDataTableByUIBineKey(uiBindKey));
            }
            #endregion End The foreign key table join DataSet

            #region  The flow of information into the inside .
            // The flow of information form and send it in the past .
            GenerWorkFlow gwf = new GenerWorkFlow();
            gwf.WorkID = workID;
            gwf.RetrieveFromDBSources();

            myds.Tables.Add(gwf.ToDataTableField("WF_GenerWorkFlow"));

            if (gwf.WFState == WFState.Shift)
            {
                // If you are forwarding .
                BP.WF.ShiftWorks fws = new ShiftWorks();
                fws.Retrieve(ShiftWorkAttr.WorkID, workID, ShiftWorkAttr.FK_Node, fk_node);
                myds.Tables.Add(fws.ToDataTableField("WF_ShiftWork"));
            }

            if (gwf.WFState == WFState.ReturnSta)
            {
                // If it is returned .
                ReturnWorks rts = new ReturnWorks();
                rts.Retrieve(ReturnWorkAttr.WorkID, workID,
                    ReturnWorkAttr.ReturnToNode, fk_node,
                    ReturnWorkAttr.RDT);
                myds.Tables.Add(rts.ToDataTableField("WF_ReturnWork"));
            }

            if (gwf.WFState == WFState.HungUp)
            {
                // If it hangs .
                HungUps hups = new HungUps();
                hups.Retrieve(HungUpAttr.WorkID, workID, HungUpAttr.FK_Node, fk_node);
                myds.Tables.Add(hups.ToDataTableField("WF_HungUp"));
            }
            Int64 wfid = workID;
            if (fid != 0)
                wfid = fid;

            // Add track Information .
            Paras ps = new Paras();
            ps.SQL = "SELECT * FROM ND" + int.Parse(fk_flow) + "Track WHERE WorkID=" + BP.Sys.SystemConfig.AppCenterDBVarStr + "WorkID";
            ps.Add("WorkID", wfid);
            DataTable dtNode = DBAccess.RunSQLReturnTable(ps);
            dtNode.TableName = "Track";
            myds.Tables.Add(dtNode);

            // Staff List , Components for auditing .
            ps = new Paras();
            ps.SQL = "SELECT * FROM  WF_GenerWorkerlist WHERE WorkID=" + BP.Sys.SystemConfig.AppCenterDBVarStr + "WorkID";
            ps.Add("WorkID", wfid);
            DataTable dtGenerWorkerlist = DBAccess.RunSQLReturnTable(ps);
            dtGenerWorkerlist.TableName = "WF_GenerWorkerlist";
            myds.Tables.Add(dtGenerWorkerlist);

            if (dtGenerWorkerlist.Rows.Count != 0 && nd.IsStartNode == false && iscc == false)
            {
                foreach (DataRow dr in dtGenerWorkerlist.Rows)
                {
                    if (dr[GenerWorkerListAttr.IsRead].ToString() == "1"
                        && dr[GenerWorkerListAttr.FK_Emp].ToString() == WebUser.No)
                    {
                        BP.WF.Dev2Interface.Node_SetWorkRead(fk_node, workID);
                        break;
                    }
                }
            }

            // Add CCList Information .  Components for auditing .
            ps = new Paras();
            ps.SQL = "SELECT * FROM WF_CCList WHERE WorkID=" + BP.Sys.SystemConfig.AppCenterDBVarStr + "WorkID";
            ps.Add("WorkID", wfid);
            DataTable dtCCList = DBAccess.RunSQLReturnTable(ps);
            dtCCList.TableName = "WF_CCList";
            myds.Tables.Add(dtCCList);

            // Add WF_SelectAccper Information .  Components for auditing .
            ps = new Paras();
            ps.SQL = "SELECT * FROM WF_SelectAccper WHERE WorkID=" + BP.Sys.SystemConfig.AppCenterDBVarStr + "WorkID";
            ps.Add("WorkID", wfid);
            DataTable dtSelectAccper = DBAccess.RunSQLReturnTable(ps);
            dtSelectAccper.TableName = "WF_SelectAccper";
            myds.Tables.Add(dtSelectAccper);
            #endregion  The flow of information into the inside .

            myds.WriteXml("c:\\22xxx.xml", XmlWriteMode.IgnoreSchema);
            //BP.DA.DataType.WriteFile( "c:\\ss.xml", 

            return myds;
        }
        catch (Exception ex)
        {
            Log.DebugWriteError(ex.StackTrace);
            throw new Exception(ex.Message);
        }
    }
    /* Zhou Peng small modifications -------------------------------START*/
    /// <summary>
    ///  Get the template file 
    /// </summary>
    /// <param name="fk_flow"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string GetFlowTemplete(string fk_flow, string fk_node, string ver)
    //  public string GetFlowTemplete(string fk_flow, string ver)
    {

        string resultJson = "";

        BP.WF.Flow fl = new Flow(fk_flow);
        string path = BP.Sys.SystemConfig.PathOfDataUser + "\\FlowDesc\\" + fl.No + "." + fl.Name;

        // string fileName =  path+ "\\Flow.xml";
        BP.WF.Node node = new Node(fk_node);
        string fileName = path + "\\" + fk_node + "." + node.Name + ".xml";
        /* Zhou Peng small modifications -------------------------------END*/

        if (System.IO.File.Exists(path) == false)
        {
            /* If there is no ,  He would generate . */
            DataSet dstemp = fl.GetFlow(path);
            dstemp.WriteXml(fileName);
            resultJson = FormatToJson.ToJson(dstemp);
        }

        DataSet ds = new DataSet();
        ds.ReadXml(fileName);
        DataTable dtFlow = ds.Tables["WF_Flow"];
        if (dtFlow.Rows[0]["Ver"].ToString() != ver)
        {
            /* If there is no ,  He would generate . */
            DataSet dstemp = fl.GetFlow(path);
            dstemp.WriteXml(fileName);
            resultJson = FormatToJson.ToJson(dstemp);
        }
        else
        {
            resultJson = FormatToJson.ToJson(ds);
        }

        return resultJson;
    }




    /**--- Adding small Zhou Peng  2014-09-13---START**/
    /// <summary>
    ///  Get to select the next node data source 
    /// </summary>
    /// <param name="userNo"> User </param>
    /// <param name="sid"> Security code </param>
    /// <param name="fk_flow"> Workflow </param>
    /// <param name="fk_node"> Node number </param>
    /// <param name="workID"> The work ID</param>
    /// <returns>string的json</returns>
    [WebMethod(EnableSession = true)]
    public string WorkOpt_GetToNodes(string userNo, string sid, string fk_flow, int fk_node, Int64 workID, Int64 fid)
    {
        this.LetUserLogin(userNo, sid);

        Nodes nodes = BP.WF.Dev2Interface.WorkOpt_GetToNodes(fk_flow, fk_node, workID, fid);

        return BP.Tools.Entitis2Json.ConvertEntities2ListJson(nodes);
    }

    /// <summary>
    ///  Select the next node sends 
    /// </summary>
    /// <param name="userNo"> User </param>
    /// <param name="sid"> Security code </param>
    /// <param name="fk_flow"> Job No. </param>
    /// <param name="fk_node"> Node number </param>
    /// <param name="workID"> The work ID</param>
    /// <param name="fid"> Process ID</param>
    /// <param name="to_node"> Arrival node </param>
    /// <returns> Send Results </returns>
    [WebMethod(EnableSession = true)]
    public string WorkOpt_SendToNodes(string userNo, string sid, string fk_flow, int fk_node, Int64 workID, Int64 fid,
        string to_node)
    {
        this.LetUserLogin(userNo, sid);

        //  Performing transmission .
        string msg = "";
        try
        {
            msg = BP.WF.Dev2Interface.WorkOpt_SendToNodes(fk_flow, fk_node, workID, fid, to_node).ToMsgOfText();
        }
        catch (Exception ex)
        {
            msg = " Send an error :" + ex.Message;
        }

        return msg;
    }

    /**--- Adding small Zhou Peng  2014-09-13---END**/

    /**--- Adding small Zhou Peng  2014-09-15---START**/
    /// <summary>
    ///  Obtaining recipient data source 
    /// </summary>
    /// <param name="userNo"> User </param>
    /// <param name="sid"> Security code </param>
    /// <param name="fk_flow"> Job No. </param>
    /// <param name="fk_node"> Node number </param>
    /// <param name="workID"> The work ID</param>
    /// <param name="fid"> Process ID</param> 
    /// <returns> Recipient Json Data </returns>
    [WebMethod(EnableSession = true)]
    public string WorkOpt_AccepterDB(string userNo, string sid, string fk_flow, int fk_node, Int64 workID, Int64 fid)
    {
        try
        {
            this.LetUserLogin(userNo, sid);

            //  Get recipient DataSet
            DataSet ds = BP.WF.Dev2Interface.WorkOpt_AccepterDB(fk_flow, fk_node, workID, fid);

            return BP.Tools.FormatToJson.ToJson(ds);
        }
        catch (Exception ex)
        {
            return "err:" + ex.Message;
        }
    }

    /// <summary>
    ///  Sets the specified node accepts people 
    /// </summary>
    /// <param name="userNo"> User </param>
    /// <param name="sid"> Security code </param>
    /// <param name="fk_node"> Node ID</param>
    /// <param name="workID"> The work ID</param>
    /// <param name="fid"> Process ID</param>
    /// <param name="emps"> Persons designated collection zhangsan,lisi,wangwu</param>
    /// <param name="isNextTime"> Whether the next automatic settings </param>
    [WebMethod(EnableSession = true)]
    public string WorkOpt_SetAccepter(string userNo, string sid, int fk_node, Int64 workID, Int64 fid, string emps, bool isNextTime)
    {
        this.LetUserLogin(userNo, sid);

        BP.WF.Dev2Interface.WorkOpt_SetAccepter(fk_node, workID, fid, emps, isNextTime);

        return " Accept staff set successfully !";
    }
    /**--- Adding small Zhou Peng  2014-09-15---END**/

    /**--- Adding small Zhou Peng  2014-09-17---START**/
    /// <summary>
    ///  Delete Process 
    /// </summary>
    /// <param name="userNo"> User </param>
    /// <param name="sid"> Security code </param>
    /// <param name="fk_flow"> Process ID </param>
    /// <param name="workID"> The work ID</param>
    /// <param name="isDelSubFlow"> Do you want to delete its child processes </param>
    [WebMethod(EnableSession = true)]
    public string Flow_DoDeleteFlowByReal(string userNo, string sid, string fk_flow, Int64 workID, bool isDelSubFlow)
    {
        this.LetUserLogin(userNo, sid);

        BP.WF.Dev2Interface.Flow_DoDeleteFlowByReal(fk_flow, workID, isDelSubFlow);

        return " Deleted successfully !";
    }
    /**--- Adding small Zhou Peng  2014-09-17---END**/

    /**--- Adding small Zhou Peng  2014-09-19---START**/
    /// <summary>
    ///  Reply endorsement content 
    /// </summary>
    /// <param name="userNo"> User </param>
    /// <param name="sid"> Security code </param>
    /// <param name="fk_flow"> Process ID </param>
    /// <param name="fk_node"> Node number </param>
    /// <param name="workID"> The work ID</param>
    /// <param name="fid">FID</param>
    /// <param name="replyNote"> Reply message </param>
    /// <returns> Reply results </returns>
    [WebMethod(EnableSession = true)]
    public string Node_AskforReply(string userNo, string sid, string fk_flow, int fk_node, Int64 workID, Int64 fid, string replyNote)
    {
        this.LetUserLogin(userNo, sid);

        string info = BP.WF.Dev2Interface.Node_AskforReply(fk_flow, fk_node, workID, fid, replyNote);

        return info;
    }
    /**--- Adding small Zhou Peng  2014-09-19---END**/

    /**--- Adding small Zhou Peng  2014-10-04---START**/
    /// <summary>
    ///  Reply endorsement content 
    /// </summary>
    /// <param name="userNo"> User </param>
    /// <param name="sid"> Security code </param>
    /// <param name="fk_flow"> Process ID </param>
    /// <param name="fk_node"> Node number </param>
    /// <param name="workID"> The work ID</param>
    /// <param name="fid">FID</param>
    /// <param name="replyNote"> Reply message </param>
    /// <returns> Reply results </returns>
    [WebMethod(EnableSession = true)]
    public string AttachmentUploadFile(string userNo, string sid, byte[] fileByte, string fileName)
    {

        this.LetUserLogin(userNo, sid);

        //string info = BP.WF.Dev2Interface.UploadFile(fileByte, fileName);

        return "";
    }
    /**--- Adding small Zhou Peng  2014-10-04---END**/

    private DataSet GenerWorkNode_FlowDataOnly(string fk_flow, int fk_node, Int64 workID, Int64 fid, string userNo)
    {
        if (fk_node == 0)
            fk_node = int.Parse(fk_flow + "01");

        try
        {
            Emp emp = new Emp(userNo);
            BP.Web.WebUser.SignInOfGener(emp);

            DataSet myds = new DataSet();

            // Node 
            Node nd = new Node(fk_node);

            #region  The primary data from the table into the inside .
            //. Data put inside to work ,  To perform a pre-loaded into the event before filling .
            BP.WF.Work wk = nd.HisWork;
            wk.OID = workID;
            wk.RetrieveFromDBSources();
            wk.ResetDefaultVal();

            #region  Set Default 
            MapAttrs mattrs = nd.MapData.MapAttrs;
            foreach (MapAttr attr in mattrs)
            {
                if (attr.UIIsEnable)
                    continue;

                if (attr.DefValReal.Contains("@") == false)
                    continue;

                wk.SetValByKey(attr.KeyOfEn, attr.DefVal);
            }
            #endregion  Set Default .

            // Description .
            MapData md = new MapData("ND" + fk_node);

            //  Perform a load before filling .
            string msg = md.FrmEvents.DoEventNode(FrmEventList.FrmLoadBefore, wk);
            if (string.IsNullOrEmpty(msg) == false)
                throw new Exception(" Error :" + msg);

            myds.Tables.Add(wk.ToDataTableField(md.No));
            if (md.MapDtls.Count > 0)
            {
                foreach (MapDtl dtl in md.MapDtls)
                {
                    GEDtls dtls = new GEDtls(dtl.No);
                    QueryObject qo = null;
                    try
                    {
                        qo = new QueryObject(dtls);
                        switch (dtl.DtlOpenType)
                        {
                            case DtlOpenType.ForEmp:  //  By staff to control .
                                qo.AddWhere(GEDtlAttr.RefPK, workID);
                                qo.addAnd();
                                qo.AddWhere(GEDtlAttr.Rec, WebUser.No);
                                break;
                            case DtlOpenType.ForWorkID: //  By Job ID To control 
                                qo.AddWhere(GEDtlAttr.RefPK, workID);
                                break;
                            case DtlOpenType.ForFID: //  By the process ID To control .
                                qo.AddWhere(GEDtlAttr.FID, workID);
                                break;
                        }
                    }
                    catch
                    {
                        dtls.GetNewEntity.CheckPhysicsTable();
                    }
                    DataTable dtDtl = qo.DoQueryToTable();

                    //  Set the default value for the list .
                    MapAttrs dtlAttrs = new MapAttrs(dtl.No);
                    foreach (MapAttr attr in dtlAttrs)
                    {
                        // Handle its default value .
                        if (attr.DefValReal.Contains("@") == false)
                            continue;

                        foreach (DataRow dr in dtDtl.Rows)
                            dr[attr.KeyOfEn] = attr.DefVal;
                    }

                    dtDtl.TableName = dtl.No; // Change the name list of .
                    myds.Tables.Add(dtDtl); // Join this list ,  If no data ,xml Reflected empty .
                }
            }
            #endregion

            #region  The foreign key table join DataSet
            DataTable dtMapAttr = myds.Tables["Sys_MapAttr"];
            foreach (DataRow dr in dtMapAttr.Rows)
            {
                string lgType = dr["LGType"].ToString();
                if (lgType != "2")
                    continue;

                string UIIsEnable = dr["UIIsEnable"].ToString();
                if (UIIsEnable == "0")
                    continue;

                string uiBindKey = dr["UIBindKey"].ToString();

                if (string.IsNullOrEmpty(uiBindKey))
                {
                    string myPK = dr["MyPK"].ToString();
                    /* If it is empty */
                    throw new Exception("@ Attribute field data is incomplete , Process :" + nd.FK_Flow + nd.FlowName + ", Node :" + nd.NodeID + nd.Name + ", Property :" + myPK + ", UIBindKey IsNull ");
                }

                //  Determine whether there .
                if (myds.Tables.Contains(uiBindKey) == true)
                    continue;

                myds.Tables.Add(BP.Sys.PubClass.GetDataTableByUIBineKey(uiBindKey));
            }
            #endregion End The foreign key table join DataSet

            #region  The flow of information into the inside .
            // The flow of information form and send it in the past .
            GenerWorkFlow gwf = new GenerWorkFlow();
            gwf.WorkID = workID;
            myds.Tables.Add(gwf.ToDataTableField("WF_GenerWorkFlow"));

            if (gwf.WFState == WFState.Shift)
            {
                // If you are forwarding .
                BP.WF.ShiftWorks fws = new ShiftWorks();
                fws.Retrieve(ShiftWorkAttr.WorkID, workID, ShiftWorkAttr.FK_Node, fk_node);
                myds.Tables.Add(fws.ToDataTableField("WF_ShiftWork"));
            }

            if (gwf.WFState == WFState.ReturnSta)
            {
                // If it is returned .
                ReturnWorks rts = new ReturnWorks();
                rts.Retrieve(ReturnWorkAttr.WorkID, workID, ReturnWorkAttr.ReturnToNode, fk_node);
                myds.Tables.Add(rts.ToDataTableField("WF_ShiftWork"));
            }

            if (gwf.WFState == WFState.HungUp)
            {
                // If it hangs .
                HungUps hups = new HungUps();
                hups.Retrieve(HungUpAttr.WorkID, workID, HungUpAttr.FK_Node, fk_node);
                myds.Tables.Add(hups.ToDataTableField("WF_HungUp"));
            }

            // Add track Information .
            Paras ps = new Paras();
            ps.SQL = "SELECT * FROM ND" + int.Parse(fk_flow) + "Track WHERE WorkID=" + BP.Sys.SystemConfig.AppCenterDBVarStr + "WorkID";
            ps.Add("WorkID", workID);
            DataTable dtNode = DBAccess.RunSQLReturnTable(ps);
            dtNode.TableName = "Track";
            myds.Tables.Add(dtNode);
            #endregion  The flow of information into the inside .

            myds.WriteXml("c:\\GenerWorkNode_FlowDataOnly.xml");
            return myds;
        }
        catch (Exception ex)
        {
            Log.DebugWriteError(ex.StackTrace);
            throw new Exception(ex.Message);
            //return "@ Generate work FK_Flow=" + fk_flow + ",FK_Node=" + fk_node + ",WorkID=" + workID + ",FID=" + fid + " Error , Error Messages :" + ex.Message;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fk_flow"></param>
    /// <param name="fk_node"></param>
    /// <param name="workID"></param>
    /// <param name="fid"></param>
    /// <param name="userNo"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string GenerFlowTemplete_Json(string fk_flow)
    {
        Flow fl = new Flow(fk_flow);

        string path = BP.Sys.SystemConfig.PathOfDataUser + @"\FlowDesc\";

        DataSet myds = fl.DoExpFlowXmlTemplete(path);
        myds.WriteXml("c:\\GenerFlowTemplete_Json.xml");

        string strs = BP.Tools.FormatToJson.ToJson(myds);
        DataType.WriteFile("c:\\GenerFlowTemplete_Json.txt", strs);
        return strs;
    }
    /// <summary>
    ///  Get a work to be done 
    /// </summary>
    /// <param name="fk_flow"> Job No. </param>
    /// <param name="fk_node"> Node number </param>
    /// <param name="workID"> The work ID</param>
    /// <param name="userNo"> Operator number </param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string Node_SaveWork(string fk_flow, int fk_node, Int64 workID, string userNo, string dsXml)
    {
        try
        {
            Emp emp = new Emp(userNo);
            BP.Web.WebUser.SignInOfGener(emp);

            DataSet ds = Silverlight.DataSetConnector.Connector.FromXml(dsXml);
            Hashtable htMain = new Hashtable();
            DataTable dtMain = ds.Tables["ND" + fk_node]; // Obtain the agreement of the master table data .
            foreach (DataRow dr in dtMain.Rows)
                htMain.Add(dr[0].ToString(), dr[1].ToString());
            return BP.WF.Dev2Interface.Node_SaveWork(fk_flow, fk_node, workID, htMain, null);
        }
        catch (Exception ex)
        {
            return "@ Save the error :" + ex.Message;
        }
    }
    /// <summary>
    ///  Save Data 
    /// </summary>
    /// <param name="fk_flow"> Process ID </param>
    /// <param name="fk_node"> Node number </param>
    /// <param name="workID"> The work ID</param>
    /// <param name="FID">FID</param>
    /// <param name="userNo"> User </param>
    /// <param name="sid"> Verification code </param>
    /// <param name="jsonStr">json</param>
    /// <returns> Return to the results </returns>
    [WebMethod(EnableSession = true)]
    public string Node_SaveWork_Json(string fk_flow, int fk_node, Int64 workID, Int64 fid, string userNo, string sid, string jsonStr)
    {
        try
        {
            if (WebUser.No != userNo)
            {
                Emp emp = new Emp(userNo);
                BP.Web.WebUser.SignInOfGener(emp);
            }

            #region  This part of the code and  send  The same .
            //  Accept data .
            DataSet ds = BP.Tools.FormatToJson.JsonToDataSet(jsonStr);

            //  Data obtained in the primary table .
            string frm = "ND" + fk_node;
            DataTable dtMain = ds.Tables[frm];
            Hashtable htMain = new Hashtable();
            foreach (DataColumn dc in dtMain.Columns)
                htMain.Add(dc.ColumnName, dtMain.Rows[0][dc.ColumnName].ToString());

            //  Determine whether the audit data table .
            if (ds.Tables.Contains("FrmCheck") == true)
            {
                DataTable dtfrm = ds.Tables["FrmCheck"];
                string note = dtfrm.Rows[0][0] as string;
                string opName = dtfrm.Rows[0][1] as string;
                if (note != null)
                    BP.WF.Dev2Interface.WriteTrackWorkCheck(fk_flow, fk_node, workID, fid, note, opName);
            }
            #endregion  This part of the code and  send  The same .

            // The save .
            BP.WF.Dev2Interface.Node_SaveWork(fk_flow, fk_node, workID, htMain, ds);

            // Save the main table after table of data returned from the past , May lead to business computing needs to display the new data .
            #region  Return to the main data from the table only after saving data .
            // Node 
            Node nd = new Node(fk_node);

            // Define data containers .
            DataSet myds = new DataSet();

            //  The primary data from the table into the inside .
            BP.WF.Work wk = nd.HisWork;
            wk.OID = workID;
            QueryObject qoEn = new QueryObject(wk);
            qoEn.AddWhere("OID", workID);
            dtMain = qoEn.DoQueryToTable(1); // wk.ToDataTableField("ND" + fk_node);
            dtMain.TableName = "ND" + fk_node;
            myds.Tables.Add(dtMain);

            // Description .n
            MapData md = new MapData("ND" + fk_node);
            if (md.MapDtls.Count > 0)
            {
                foreach (MapDtl dtl in md.MapDtls)
                {
                    GEDtls dtls = new GEDtls(dtl.No);
                    QueryObject qo = null;
                    try
                    {
                        qo = new QueryObject(dtls);
                        switch (dtl.DtlOpenType)
                        {
                            case DtlOpenType.ForEmp:  //  By staff to control .
                                qo.AddWhere(GEDtlAttr.RefPK, workID);
                                qo.addAnd();
                                qo.AddWhere(GEDtlAttr.Rec, WebUser.No);
                                break;
                            case DtlOpenType.ForWorkID: //  By Job ID To control 
                                qo.AddWhere(GEDtlAttr.RefPK, workID);
                                break;
                            case DtlOpenType.ForFID: //  By the process ID To control .
                                qo.AddWhere(GEDtlAttr.FID, workID);
                                break;
                        }
                    }
                    catch
                    {
                        dtls.GetNewEntity.CheckPhysicsTable();
                    }
                    DataTable dtDtl = qo.DoQueryToTable();

                    dtDtl.TableName = dtl.No; // Change the name list of .
                    myds.Tables.Add(dtDtl); // Join this list ,  If no data ,xml Reflected empty .
                }
            }
            #endregion

            //  Data returned after storage ,  Because saving around , You need to perform an event , Data changes would occur after execution .
            //   return BP.DA.DataTableConvertJson.Dataset2Json(myds);
            return FormatToJson.ToJson(myds);
        }
        catch (Exception ex)
        {
            return "@ Save the error :" + ex.Message;
        }
    }
    /// <summary>
    ///  Performing transmission 
    /// </summary>
    /// <param name="fk_flow"> Process ID </param>
    /// <param name="fk_node"> Node number </param>
    /// <param name="workID"> The work ID</param>
    /// <param name="fid"> Process ID</param>
    /// <param name="userNo"> The operator </param>
    /// <param name="sid">sid</param>
    /// <param name="jsonStr">json</param>
    /// <returns> Send execution information </returns>
    [WebMethod(EnableSession = true)]
    public string Node_SendWork_Json(string fk_flow, int fk_node, Int64 workID, Int64 fid, string userNo, string sid, string jsonStr)
    {
        this.LetUserLogin(userNo, sid);
        try
        {
            SendReturnObjs objs = null;
            if (jsonStr != null)
            {
                #region  This part of the code and  send  The same .
                //  Accept data .
                DataSet ds = BP.Tools.FormatToJson.JsonToDataSet(jsonStr);

                //  Data obtained in the primary table .
                string frm = "ND" + fk_node;
                DataTable dtMain = ds.Tables[frm];
                Hashtable htMain = new Hashtable();
                foreach (DataColumn dc in dtMain.Columns)
                    htMain.Add(dc.ColumnName, dtMain.Rows[0][dc.ColumnName].ToString());

                //  Determine whether the audit data table .
                if (ds.Tables.Contains("FrmCheck") == true)
                {
                    DataTable dtfrm = ds.Tables["FrmCheck"];
                    string note = dtfrm.Rows[0][0] as string;
                    string opName = dtfrm.Rows[0][1] as string;
                    if (note != null)
                        BP.WF.Dev2Interface.WriteTrackWorkCheck(fk_flow, fk_node, workID, fid, note, opName);
                }
                #endregion  This part of the code and  send  The same .

                // Performing transmission .
                objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workID, htMain, ds);
            }
            else
            {
                objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workID, null, null);
            }
            return objs.ToMsgOfText();
        }
        catch (Exception ex)
        {
            return "@ Error sending work :" + ex.Message;
        }
    }
    /// <summary>
    ///  Performing transmission 
    /// </summary>
    /// <param name="fk_flow"></param>
    /// <param name="fk_node"></param>
    /// <param name="workID"></param>
    /// <param name="dsXml"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string Node_SendWork(string fk_flow, int fk_node, Int64 workID, string dsXml, string currUserNo)
    {
        try
        {
            if (BP.Web.WebUser.No != currUserNo)
                BP.WF.Dev2Interface.Port_Login(currUserNo);

            SendReturnObjs objs = null;
            if (dsXml != null)
            {
                StringReader sr = new StringReader(dsXml);
                DataSet ds = new DataSet();
                ds.ReadXml(sr);
                ds.WriteXml("c:\\GenerSendXml.xml");

                Hashtable htMain = new Hashtable();
                DataTable dtMain = ds.Tables["ND" + fk_node];
                foreach (DataRow dr in dtMain.Rows)
                {
                    htMain.Add(dr[0].ToString(), dr[1].ToString());
                }
                objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workID, htMain, ds);
            }
            else
            {
                objs = BP.WF.Dev2Interface.Node_SendWork(fk_flow, workID, null, null);
            }
            return objs.ToMsgOfText();
        }
        catch (Exception ex)
        {
            return "err@ Error sending work :" + ex.Message;
        }
    }
    /// <summary>
    ///  Execution endorsement 
    /// </summary>
    /// <param name="userNo"> Who is currently logged on </param>
    /// <param name="sid"> Checksum </param>
    /// <param name="workID"> The work ID</param>
    /// <param name="_askforHelpSta">@5= Sent directly after endorsement @6= Sent directly from me after endorsement </param>
    /// <param name="toEmpNo"> Plus sign people </param>
    /// <param name="note"> Information </param>
    /// <returns> Execution results </returns>
    [WebMethod(EnableSession = true)]
    public string Node_Askfor(string userNo, string sid, Int64 workID, int _askforHelpSta, string toEmpNo, string note)
    {
        if (BP.Web.WebUser.No != userNo)
            BP.WF.Dev2Interface.Port_Login(userNo);

        AskforHelpSta sta = (AskforHelpSta)_askforHelpSta;
        return BP.WF.Dev2Interface.Node_Askfor(workID, sta, toEmpNo, note);
    }

    [WebMethod]
    public string GetNoName(string SQL)
    {
        DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(SQL);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        return BP.DA.DataType.ToJson(ds.Tables[0]);
    }
    /// <summary>
    ///  Large file uploads 
    /// </summary>
    /// <param name="fileName"> Upload a file name </param>
    /// <param name="offSet"> Offset </param>
    /// <param name="intoBuffer"> Upload every byte array   Unit KB</param>
    /// <returns> Upload successful </returns>
    [WebMethod]
    public bool Upload(string fileName, long offSet, byte[] intoBuffer)
    {
        // Specify upload folder + File name ( Relative path )
        string strPath = @"D:\value-added\CCFlow\DataUser\UploadFile\" + fileName;
        // Will be converted into a server relative path absolute path 
        //strPath = Server.MapPath(strPath);

        if (offSet < 0)
        {
            offSet = 0;
        }

        byte[] buffer = intoBuffer;

        if (buffer != null)
        {
            // Read file stream , Supports simultaneous read and write but also support asynchronous read and write 
            FileStream filesStream = new FileStream(strPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            filesStream.Seek(offSet, SeekOrigin.Begin);
            filesStream.Write(buffer, 0, buffer.Length);
            filesStream.Flush();
            filesStream.Close();
            filesStream.Dispose();
            return true;
        }
        return false;
    }

    [WebMethod]
    public string ParseExp(string strExp)
    {
        DataTable dt = DBAccess.RunSQLReturnTable("select " + strExp);
        if (dt != null && dt.Rows.Count > 0)
        {
            return dt.Rows[0][0].ToString();
        }
        return string.Empty;
    }

    private void InitializeComponent()
    {
    }

    #region  Obtain audit information . edity by xiaozhoupeng.  2014-07-26
    /// <summary>
    ///  Obtain audit information 
    /// </summary>
    /// <param name="userNo"> The current operator number </param>
    /// <param name="sid">SID</param>
    /// <param name="fk_flow"> Process ID </param>
    /// <param name="fk_node"> Node number </param>
    /// <param name="workID"> The work ID</param>
    /// <returns> Audit information </returns>
    [WebMethod(EnableSession = true)]
    public string GetCheckInfo(string userNo, string sid, string fk_flow, int fk_node, Int64 workID)
    {
        if (BP.Web.WebUser.No != userNo)
            BP.WF.Dev2Interface.Port_Login(userNo);

        return BP.WF.Dev2Interface.GetCheckInfo(fk_flow, workID, fk_node);
    }
    /// <summary>
    ///  Write operation audit log :
    /// </summary>
    /// <param name="flowNo"> Process ID </param>
    /// <param name="nodeID"> Node from </param>
    /// <param name="workid"> The work ID</param>
    /// <param name="FID">FID</param>
    /// <param name="msg"> Audit information </param>
    /// <param name="optionName"> Action Name ( Such as : Chief Audit , Department manager for approval ), That is, if it is empty " Check ".</param>
    [WebMethod(EnableSession = true)]
    public string WriteTrackWorkCheck(string userNo, string sid, string flowNo, int nodeFrom, Int64 workid, Int64 fid, string msg, string optionName)
    {
        if (BP.Web.WebUser.No != userNo)
            BP.WF.Dev2Interface.Port_Login(userNo);

        BP.WF.Dev2Interface.WriteTrackWorkCheck(flowNo, nodeFrom, workid, fid, msg, optionName);

        // Setting audit completed .
        BP.WF.Dev2Interface.Node_CC_SetSta(nodeFrom, workid, BP.Web.WebUser.No, CCSta.CheckOver);


        return " Audit Success !";
    }
    #endregion  Obtain audit information .


    #region  Form-related api.
    /// <summary>
    ///  Remove attachment 
    /// </summary>
    /// <param name="userNo"></param>
    /// <param name="sid"></param>
    /// <param name="mypk"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string CCFrom_DelFrmAttachment(string userNo, string sid, string mypk)
    {
        BP.Sys.FrmAttachmentDB db = new FrmAttachmentDB();
        db.MyPK = mypk;
        db.Delete();
        return " Deleted successfully ";
    }
    /// <summary>
    ///  Post attachments 
    /// </summary>
    /// <param name="userNo"></param>
    /// <param name="sid"></param>
    /// <param name="intoBuffer"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string CCForm_UploadFrmAttachment(string userNo, string sid, string fk_frmath, byte[] intoBuffer)
    {

        return " Uploaded successfully ";
        //BP.Sys.FrmAttachment athDesc = new BP.Sys.FrmAttachment(fk_frmath);
        //string exts = System.IO.Path.GetExtension(fu.FileName).ToLower().Replace(".", "");

        //// If you have to upload the type of restrictions , Judging format 
        //if (athDesc.Exts == "*.*" || athDesc.Exts == "")
        //{
        //    /* Any format can be uploaded */
        //}
        //else
        //{
        //    if (athDesc.Exts.ToLower().Contains(exts) == false)
        //    {
        //        this.Alert(" The file you uploaded , Does not meet the format requirements of the system , Required file format :" + athDesc.Exts + ", You can now upload file format :" + exts);
        //        return;
        //    }
        //}

        //string savePath = athDesc.SaveTo;

        //if (savePath.Contains("@") == true || savePath.Contains("*") == true)
        //{
        //    /* If you have a variable */
        //    savePath = savePath.Replace("*", "@");
        //    GEEntity en = new GEEntity(athDesc.FK_MapData);
        //    en.PKVal = this.PKVal;
        //    en.Retrieve();
        //    savePath = BP.WF.Glo.DealExp(savePath, en, null);

        //    if (savePath.Contains("@") && this.FK_Node != null)
        //    {
        //        /* If you include  @ */
        //        BP.WF.Flow flow = new BP.WF.Flow(this.FK_Flow);
        //        BP.WF.Data.GERpt myen = flow.HisGERpt;
        //        myen.OID = this.WorkID;
        //        myen.RetrieveFromDBSources();
        //        savePath = BP.WF.Glo.DealExp(savePath, myen, null);
        //    }
        //    if (savePath.Contains("@") == true)
        //        throw new Exception("@ Path configuration error , Variable is not correct replacement down ." + savePath);
        //}
        //else
        //{
        //    //savePath = athDesc.SaveTo + "\\" + this.PKVal;
        //}

        //// Replace the key string .
        //savePath = savePath.Replace("\\\\", "\\");
        //try
        //{

        //    savePath = Server.MapPath("~/" + savePath);

        //}
        //catch (Exception)
        //{
        //    savePath = savePath;

        //}
        //try
        //{

        //    if (System.IO.Directory.Exists(savePath) == false)
        //    {
        //        System.IO.Directory.CreateDirectory(savePath);
        //        //System.IO.Directory.CreateDirectory(athDesc.SaveTo);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    throw new Exception("@ Create a path error , Might not have permission or the path configuration problem :" + Server.MapPath("~/" + savePath) + "===" + savePath + "@ Technical issues :" + ex.Message);
        //}

        ////int oid = BP.DA.DBAccess.GenerOID();
        //string guid = BP.DA.DBAccess.GenerGUID();

        //string fileName = fu.FileName.Substring(0, fu.FileName.LastIndexOf('.'));
        ////string ext = fu.FileName.Substring(fu.FileName.LastIndexOf('.') + 1);
        //string ext = System.IO.Path.GetExtension(fu.FileName);

        ////string realSaveTo = Server.MapPath("~/" + savePath) + "/" + guid + "." + fileName + "." + ext;

        ////string realSaveTo = Server.MapPath("~/" + savePath) + "\\" + guid + "." + fu.FileName.Substring(fu.FileName.LastIndexOf('.') + 1);
        ////string saveTo = savePath + "/" + guid + "." + fileName + "." + ext;



        //string realSaveTo = savePath + "/" + guid + "." + fileName + "." + ext;

        //string saveTo = realSaveTo;

        //try
        //{
        //    fu.SaveAs(realSaveTo);
        //}
        //catch (Exception ex)
        //{
        //    this.Response.Write("@ File storage failure , There may be a problem path expression , Cause is illegal path name :" + ex.Message);
        //    return;
        //}

        //FileInfo info = new FileInfo(realSaveTo);
        //FrmAttachmentDB dbUpload = new FrmAttachmentDB();

        //dbUpload.MyPK = guid; // athDesc.FK_MapData + oid.ToString();
        //dbUpload.NodeID = FK_Node.ToString();
        //dbUpload.FK_FrmAttachment = this.FK_FrmAttachment;

        //if (athDesc.AthUploadWay == AthUploadWay.Inherit)
        //{
        //    /* If it is inherited , Let him keep the local PK. */
        //    dbUpload.RefPKVal = this.PKVal.ToString();
        //}

        //if (athDesc.AthUploadWay == AthUploadWay.Interwork)
        //{
        //    /* If it is synergistic , Let him be PWorkID. */
        //    string pWorkID = BP.DA.DBAccess.RunSQLReturnValInt("SELECT PWorkID FROM WF_GenerWorkFlow WHERE WorkID=" + this.PKVal, 0).ToString();
        //    if (pWorkID == null || pWorkID == "0")
        //        pWorkID = this.PKVal;

        //    dbUpload.RefPKVal = pWorkID;
        //}

        //dbUpload.FK_MapData = athDesc.FK_MapData;
        //dbUpload.FK_FrmAttachment = this.FK_FrmAttachment;

        //dbUpload.FileExts = info.Extension;
        //dbUpload.FileFullName = saveTo;
        //dbUpload.FileName = fu.FileName;
        //dbUpload.FileSize = (float)info.Length;

        //dbUpload.RDT = DataType.CurrentDataTimess;
        //dbUpload.Rec = BP.Web.WebUser.No;
        //dbUpload.RecName = BP.Web.WebUser.Name;
        //if (athDesc.IsNote)
        //    dbUpload.MyNote = this.Pub1.GetTextBoxByID("TB_Note").Text;

        //if (athDesc.Sort.Contains(","))
        //    dbUpload.Sort = this.Pub1.GetDDLByID("ddl").SelectedItemStringVal;

        //dbUpload.UploadGUID = guid;
        //dbUpload.Insert();

        ////   this.Response.Redirect("AttachmentUpload.aspx?FK_FrmAttachment=" + this.FK_FrmAttachment + "&PKVal=" + this.PKVal, true);
        //this.Response.Redirect(this.Request.RawUrl, true);
    }
    /// <summary>
    ///  All information form 
    /// </summary>
    /// <param name="fk_mapdata"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string CCForm_FrmTemplete(string fk_mapdata)
    {
        MapData md = new MapData(fk_mapdata);
        DataSet ds = md.GenerHisDataSet();
        return BP.Tools.FormatToJson.ToJson(ds);
    }
    /// <summary>
    ///  Process Information 
    /// </summary>
    /// <param name="fk_mapdata"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public string CCFlow_FlowTemplete(string fk_flow)
    {
        DataSet ds = new DataSet();

        string sql = "";
        sql = "SELECT * FROM WF_Flow WHERE No='" + fk_flow + "'";
        DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
        dt.TableName = "WF_Flow";
        ds.Tables.Add(dt);

        sql = "SELECT * FROM WF_Node WHERE FK_Flow='" + fk_flow + "'";
        dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
        dt.TableName = "WF_Node";
        ds.Tables.Add(dt);

        return BP.Tools.FormatToJson.ToJson(ds);
    }
    #endregion
}
