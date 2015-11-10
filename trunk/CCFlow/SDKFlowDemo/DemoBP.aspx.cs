using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.WF;
using BP.En;
using BP.DA;
using BP.Port;

public partial class SDKFlowDemo_DemoEntity : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       //BP.Web.WebUser.HisStations.Contains("ss", );
    }
    /// <summary>
    ///  Written to the log 
    /// </summary>
    public void WriteLogApp()
    {
        //  Write a message .
        BP.Sys.Glo.WriteLineInfo(" This is a message . ");

        //  Write a warning .
        BP.Sys.Glo.WriteLineWarning(" This is a warning . ");

        //  Write an exception or error .
        BP.Sys.Glo.WriteLineError(" This is an error . ");  //  These logs are written  \DataUser\Log\*.*

        // Write to user log 
        BP.Sys.Glo.WriteUserLog("Login", "stone", " System Log ");
        BP.Sys.Glo.WriteUserLog("Login", "stone", " System Log ", "192.168.1.100");  //  User log is written above  Sys_UserLog  Exterior and interior .)
    }
    /// <summary>
    ///  Basic application global , Get information about the current operator .
    /// </summary>
    public void GloBaseApp()
    {
        //  Execution landing .
        Emp emp = new Emp("guobaogeng");
        BP.Web.WebUser.SignInOfGener(emp);

        //  The current staff numbers landing .
        string currLoginUserNo = BP.Web.WebUser.No;
        //  Login name of the person 
        string currLoginUserName = BP.Web.WebUser.Name;
        //  Log personnel department number .
        string currLoginUserDeptNo = BP.Web.WebUser.FK_Dept;
        //  Log personnel department name 
        string currLoginUserDeptName = BP.Web.WebUser.FK_DeptName;

        BP.Web.WebUser.Exit(); // An exit .
    }
    /// <summary>
    ///  Access database operations 
    /// </summary>
    public void DataBaseAccess()
    {
        #region  Do not have arguments .
        //  Carried out Insert ,delete, update  Statement .
        int result = BP.DA.DBAccess.RunSQL("DELETE FROM Port_Emp WHERE 1=2");

        //  Perform multiple sql
        string sqls = "DELETE FROM Port_Emp WHERE 1=2";
        sqls += "@DELETE FROM Port_Emp WHERE 1=2";
        sqls += "@DELETE FROM Port_Emp WHERE 1=2";
        BP.DA.DBAccess.RunSQLs(sqls);

        // Execute the query returns datatable.
        string sql = "SELECT * FROM Port_Emp";
        DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);

        // Execute the query returns  string 值.
        sql = "SELECT FK_Dept FROM Port_Emp WHERE No='" + BP.Web.WebUser.No + "'";
        string fk_dept = BP.DA.DBAccess.RunSQLReturnString(sql);

        // Execute the query returns  int 值.  You can also return float, string
        sql = "SELECT count(*) as Num FROM Port_Emp ";
        int empNum = BP.DA.DBAccess.RunSQLReturnValInt(sql);

        // Run the stored procedure .
        string spName = "MySp";
        BP.DA.DBAccess.RunSP(spName);
        #endregion  Do not have arguments .

        #region  Execution with parameters .
        //  Carried out Insert ,delete, update  Statement .
        //  Has made it clear the type of database .
        Paras ps = new Paras();
        ps.SQL = "DELETE FROM Port_Emp WHERE No=@UserNo";
        ps.Add("UserNo", "abc");
        BP.DA.DBAccess.RunSQL(ps);

        //  I do not know the type of database .
        ps = new Paras();
        ps.SQL = "DELETE FROM Port_Emp WHERE No=" + BP.Sys.SystemConfig.AppCenterDBVarStr + "UserNo";
        ps.Add("UserNo", "abc");
        BP.DA.DBAccess.RunSQL(ps);


        // Execute the query returns datatable.
        ps = new Paras();
        ps.SQL = "SELECT * FROM Port_Emp WHERE FK_Dept=@DeptNoVar";
        ps.Add("DeptNoVar", "0102");
        DataTable dtDept = BP.DA.DBAccess.RunSQLReturnTable(ps);

        // Run the stored procedure .
        ps = new Paras();
        ps.Add("DeptNoVar", "0102");
        spName = "MySp";
        BP.DA.DBAccess.RunSP(spName, ps);
        #endregion  Execution with parameters .
    }
    /// <summary>
    /// Entity  The basic application .
    /// </summary>
    public void EntityBaseApp()
    {
        #region   Plugs directly into a data .
        BP.Port.Emp emp = new BP.Port.Emp();
        emp.CheckPhysicsTable(); 
        /*   Check whether the physical table Map Consistency  
         *  1, If there is no physical table is created .
         *  2, In the absence of field is created .
         *  3, If the field type is not always delete creation , For example, the original int Now type map Modified string Type .
         *  4,map Field reduction is not processed .
         *  5, To increase the physical table fields is not processed manually .
         *  6, The data source is not the creation fails match view fields .
         * */
        emp.No = "zhangsan";
        emp.Name = " Joe Smith ";
        emp.FK_Dept = "01";
        emp.Pass = "pub";
        emp.Insert();  //  If the primary key is repeated to throw an exception .
        #endregion   Plugs directly into a data .

        #region   Saved insert a data .
        emp = new BP.Port.Emp();
        emp.No = "zhangsan";
        emp.Name = " Joe Smith ";
        emp.FK_Dept = "01";
        emp.Pass = "pub";
        emp.Save();  //  If the primary key to repeat direct update , No exception is thrown .
        #endregion   Saved insert a data .

        #region   Data replication .
        /*
         *  If both an entity and a property substantially the same as another entity , Can be performed copy.
         *   Such as : When you create a personnel , Joe Smith and John Doe number two but with a different name , Just change various attributes related to business operations can be performed .
         */
        Emp emp1 = new BP.Port.Emp("zhangsan");
        emp = new BP.Port.Emp();
        emp.Copy(emp1); //  The same entity copy,  Different entities may also be implemented copy.
        emp.No = "lisi";
        emp.Name = " John Doe ";
        emp.Insert();
        // copy  On the business logic often applied , Such as :  In a process A Node forms with B Roughly the same node form fields ,ccflow Is the use of copy Manner .
        #endregion   Data replication .

        #region  Inquiry .
        string msg = "";     //  Query this data .
        BP.Port.Emp myEmp = new BP.Port.Emp();
        myEmp.No = "zhangsan";
        if (myEmp.RetrieveFromDBSources() == 0)  // RetrieveFromDBSources()  Return to the number of queries .
        {
            this.Response.Write(" No inquiry into the number equal to zhangsan Personnel records .");
            return;
        }
        else
        {
            msg = "";
            msg += "<BR> Serial number :" + myEmp.No;
            msg += "<BR> Name :" + myEmp.Name;
            msg += "<BR> Password :" + myEmp.Pass;
            msg += "<BR> Department number :" + myEmp.FK_Dept;
            msg += "<BR> Department name :" + myEmp.FK_DeptText;
            this.Response.Write(msg);
        }

        myEmp = new BP.Port.Emp();
        myEmp.No = "zhangsan";
        myEmp.Retrieve(); //  Execute the query , If you can not find they have thrown .

        msg = "";
        msg += "<BR> Serial number :" + myEmp.No;
        msg += "<BR> Name :" + myEmp.Name;
        msg += "<BR> Password :" + myEmp.Pass;
        msg += "<BR> Department number :" + myEmp.FK_Dept;
        msg += "<BR> Department name :" + myEmp.FK_DeptText;
        this.Response.Write(msg);
        #endregion  Inquiry .

        #region  Delete two ways .
        //  Deletion .
        emp = new BP.Port.Emp();
        emp.No = "zhangsan";
        int delNum = emp.Delete(); //  Delete .
        if (delNum == 0)
            this.Response.Write(" Delete  zhangsan  Failure .");

        if (delNum == 1)
            this.Response.Write(" Delete  zhangsan  Success ..");
        if (delNum > 1)
            this.Response.Write(" Exception should not happen .");
        //  After the first test instances , Delete , To perform this way two sql.
        emp = new BP.Port.Emp("abc");
        emp.Delete();
        #endregion  Delete two ways .

        #region  Update .
        emp = new BP.Port.Emp("zhangyifan"); //  Examples of it .
        emp.Name = " Yifan Zhang 123"; // Change the properties .
        emp.Update();   //  Update it , This time BP Will have all the attributes to perform the update ,UPDATA  Statements related to the individual columns .

        emp = new BP.Port.Emp("fuhui"); //  Examples of it .
        emp.Update("Name", " Fu Hui 123");   // Just update this one attribute ..UPDATA  Statements related to the Name列.
        #endregion  Update .
    }
    /// <summary>
    /// Entities  The basic application .
    /// </summary>
    public void EntitiesBaseApp()
    {
        #region  Check all 
        /*  All inquiries are divided into two ways ,1  All queries from the cache .2, All queries from the database .  */
        Emps emps = new Emps();
        int num = emps.RetrieveAll(); // Discover all the data from the cache .

        this.Response.Write("RetrieveAll Check out (" + num + ")个");
        foreach (Emp emp in emps)
        {
            this.Response.Write("<hr> PERSON :" + emp.Name);
            this.Response.Write("<br> Personnel Number :" + emp.No);
            this.Response.Write("<br> Department number :" + emp.FK_Dept);
            this.Response.Write("<br> Department name :" + emp.FK_DeptText);
        }

        //把entities  Data is transferred to DataTable里.
        DataTable empsDTfield = emps.ToDataTableField(); // English field as a column name .
        DataTable empsDTDesc = emps.ToDataTableDesc(); // Field as a column name in Chinese .
       
        //  All queries from the database .
        emps = new Emps();
        num = emps.RetrieveAllFromDBSource();
        this.Response.Write("RetrieveAllFromDBSource Check out (" + num + ")个");
        foreach (Emp emp in emps)
        {
            this.Response.Write("<hr> PERSON :" + emp.Name);
            this.Response.Write("<br> Personnel Number :" + emp.No);
            this.Response.Write("<br> Department number :" + emp.FK_Dept);
            this.Response.Write("<br> Department name :" + emp.FK_DeptText);
        }
        #endregion  Check all 

        #region  Conditional Query 
        //  Single condition query .
        Emps myEmps = new Emps();
        QueryObject qo = new QueryObject(myEmps);
        qo.AddWhere(EmpAttr.FK_Dept, "01");
        qo.addOrderBy(EmpAttr.No); //  Increase collations ,Order  OrderByDesc, addOrderByDesc addOrderByRandom. 
        num = qo.DoQuery();  //  Returns the number of queries .
        this.Response.Write(" Check out (" + num + ")个, Department number =01 Staff .");
        foreach (Emp emp in myEmps)
        {
            this.Response.Write("<hr> PERSON :" + emp.Name);
            this.Response.Write("<br> Personnel Number :" + emp.No);
            this.Response.Write("<br> Department number :" + emp.FK_Dept);
            this.Response.Write("<br> Department name :" + emp.FK_DeptText);
        }

     //   DataTable mydt = qo.DoQueryToTable();  //  Check out the data is transferred to datatable里..

        Emps myEmp1s = new Emps();
        myEmp1s.Retrieve(EmpAttr.FK_Dept, "01");
        foreach (Emp  item in myEmp1s)
        {
            this.Response.Write("<hr> PERSON :" + item.Name);
            this.Response.Write("<br> Personnel Number :" + item.No);
            this.Response.Write("<br> Department number :" + item.FK_Dept);
            this.Response.Write("<br> Department name :" + item.FK_DeptText);
        }

        //  Query multiple conditions .
        myEmps = new Emps();
        qo = new QueryObject(myEmps);
        qo.AddWhere(EmpAttr.FK_Dept, "01");
        qo.addAnd();
        qo.AddWhere(EmpAttr.No, "guobaogen");
        num = qo.DoQuery();  //  Returns the number of queries .
        this.Response.Write(" Check out (" + num + ")个, Department number =01 And number =guobaogen Staff .");
        foreach (Emp emp in myEmps)
        {
            this.Response.Write("<hr> PERSON :" + emp.Name);
            this.Response.Write("<br> Personnel Number :" + emp.No);
            this.Response.Write("<br> Department number :" + emp.FK_Dept);
            this.Response.Write("<br> Department name :" + emp.FK_DeptText);
        }
        //  With a query expression in parentheses .
        myEmps = new Emps();
        qo = new QueryObject(myEmps);
        qo.addLeftBracket(); //  Plus a left parenthesis .
        qo.AddWhere(EmpAttr.FK_Dept, "01");
        qo.addAnd();
        qo.AddWhere(EmpAttr.No, "guobaogen");
        qo.addRightBracket();  //  Plus a right parenthesis .
        num = qo.DoQuery();  //  Returns the number of queries .
        this.Response.Write(" Check out (" + num + ")个, Department number =01 And number =guobaogen Staff .");
        foreach (Emp emp in myEmps)
        {
            this.Response.Write("<hr> PERSON :" + emp.Name);
            this.Response.Write("<br> Personnel Number :" + emp.No);
            this.Response.Write("<br> Department number :" + emp.FK_Dept);
            this.Response.Write("<br> Department name :" + emp.FK_DeptText);
        }
        //  Have where in  Query mode .
        myEmps = new Emps();
        qo = new QueryObject(myEmps);
        qo.AddWhereInSQL(EmpAttr.No, "SELECT No FROM Port_Emp WHERE FK_Dept='02'");
        num = qo.DoQuery();  //  Returns the number of queries .
        this.Response.Write(" Check out (" + num + ")个,WHERE IN (SELECT No FROM Port_Emp WHERE FK_Dept='02') Staff .");
        foreach (Emp emp in myEmps)
        {
            this.Response.Write("<hr> PERSON :" + emp.Name);
            this.Response.Write("<br> Personnel Number :" + emp.No);
            this.Response.Write("<br> Department number :" + emp.FK_Dept);
            this.Response.Write("<br> Department name :" + emp.FK_DeptText);
        }

        //  Have LIKE  Query mode .
        myEmps = new Emps();
        qo = new QueryObject(myEmps);
        qo.AddWhere(EmpAttr.No, " LIKE ", "guo");
        num = qo.DoQuery();  //  Returns the number of queries .
        this.Response.Write(" Check out (" + num + ")个, Personnel Number contains guo Staff .");
        foreach (Emp emp in myEmps)
        {
            this.Response.Write("<hr> PERSON :" + emp.Name);
            this.Response.Write("<br> Personnel Number :" + emp.No);
            this.Response.Write("<br> Department number :" + emp.FK_Dept);
            this.Response.Write("<br> Department name :" + emp.FK_DeptText);
        }
        #endregion  Conditional Query 

        #region  Collection of business processes .
        myEmps = new Emps();
        myEmps.RetrieveAll(); //  Discover all out .
        //  Traverse the collection is a common treatment method .
        foreach (Emp emp in myEmps)
        {
            this.Response.Write("<hr> PERSON :" + emp.Name);
            this.Response.Write("<br> Personnel Number :" + emp.No);
            this.Response.Write("<br> Department number :" + emp.FK_Dept);
            this.Response.Write("<br> Department name :" + emp.FK_DeptText);
        }
        //  Determine whether to include a designated primary key values .
        bool isHave = myEmps.Contains("Name", " Guo Baogeng "); // Determine whether the collection which contains Name= Guo Baogeng entities .
        bool isHave1 = myEmps.Contains("guobaogeng"); // Determine whether the collection inside the primary key No=guobaogeng Entity .

        //  Get Name= Guo Baogeng entities , If you do not return empty .
        Emp empFind = myEmps.GetEntityByKey("Name", " Guo Baogeng ") as Emp;
        if (empFind == null)
            this.Response.Write("<br> Not found : Name = Guo Baogeng   Entity .");
        else
            this.Response.Write("<br> Have found : Name = Guo Baogeng   Entity .  His department number ="+empFind.FK_Dept+", Department name ="+empFind.FK_DeptText);
        //  Batch Update Entity .
        myEmps.Update(); //  Is equivalent to the next cycle .
        foreach (Emp emp in myEmps)
            emp.Update();
        //  Delete Entities .
        myEmps.Delete(); //  Is equivalent to the next cycle .
        foreach (Emp emp in myEmps)
        {
            emp.Delete();
        }
        //  Perform database delete , Class in execution  DELETE Port_Emp WHERE FK_Dept='01' 的sql.
        myEmps.Delete("FK_Dept", "01");
        #endregion
    }
    /// <summary>
    ///  Show EnttiyNo Automatic Number 
    /// </summary>
    public void EnttiyNo()
    {
        //  Create an empty entity .
        BP.Demo.Student en = new BP.Demo.Student();
        //  Assigned to each property , But do not give numbers assignment .
        en.Name = " Joe Smith ";
        en.FJ_BanJi = "001";
        en.Age = 19;
        en.XB = 1;
        en.Tel = "0531-82374939";
        en.Addr = " Shandong . Jinan . High-tech Zone ";
        en.Insert(); // Here will automatically give the student ID ,从0001 Begin , Open class numbering rules Map.

        string xuehao = en.No;
        this.Response.Write(" Information has been added , The students learn to :" + xuehao);

        // Check out the entity .
        BP.Demo.Student myen = new BP.Demo.Student(xuehao);
        this.Response.Write(" Student Name :" + myen.Name);
        this.Response.Write(" Address :" + myen.Addr);
    }
    /// <summary>
    ///  Entity OID
    /// </summary>
    public void EnttiyOID()
    {
        //  Create an empty resume entity .
        BP.Demo.Resume dtl = new BP.Demo.Resume();
        dtl.FK_Emp = "zhangsan"; // Related to the primary key assignment .
        dtl.NianYue = "2014年4月";
        dtl.GongZuoDanWei = " Jinan gallop company "; //  Workplace .
        dtl.ZhengMingRen = " John Doe ";  // Certifier , John Doe .
        dtl.Insert(); // Here will automatically give the entity primary key OID Assignment ,  He is an automatic increase of the column .
        this.Response.Write(" Information has been added ,OID:" + dtl.OID);

        // Initialization of the entity , And display it .
        BP.Demo.Resume mydtl = new BP.Demo.Resume(dtl.OID);
        this.Response.Write(" Workplace :" + dtl.GongZuoDanWei + " Certifier :" + dtl.ZhengMingRen);
    }
    /// <summary>
    ///  Have MyPK Type of entity , This class is the primary key of the entity MyPK.
    ///  It is the primary key of this table 2 Months or 3 A combination of two or more fields come .
    /// </summary>
    public void EnttiyMyPK()
    {
        //  Create a staff appraisal entity .
        BP.Demo.EmpCent en = new BP.Demo.EmpCent();
        en.FK_Emp = "zhangsan";
        en.FK_NY = "2003-01";
        en.MyPK = en.FK_NY + "_" + en.FK_Emp;
        en.Cent = 100;
        en.Insert();  //  Inserted into the database .
        this.Response.Write(" Information has been added ,Cent:" + en.Cent);

        BP.Demo.EmpCent myen = new BP.Demo.EmpCent(en.MyPK);
        this.Response.Write(" Staff :" + myen.FK_Emp + ", Month :" + myen.FK_NY+",  Score :"+myen.Cent);
    }
    /// <summary>
    ///  Entity tree contains No,Name,ParentNo,Idx  Required attributes ( Field ), It is a description of the tree structure .
    /// </summary>
    public void EnttiyTree()
    {
        // Create a parent ,  Parent node number must be 1 , Parent node ParentNo  Must be  0.
        BP.WF.FlowSort en = new BP.WF.FlowSort("1");
        en.Name = " Root directory ";

        // Create a subdirectory node .
        BP.WF.FlowSort subEn = (BP.WF.FlowSort)en.DoCreateSubNode();
        subEn.Name = " Executive class ";
        subEn.Update();

        // Create a subdirectory rating node .
        BP.WF.FlowSort sameLevelSubEn = (BP.WF.FlowSort)subEn.DoCreateSameLevelNode();
        sameLevelSubEn.Name = " Business Class ";
        sameLevelSubEn.Update();

        // Create a subdirectory under a node 1.
        BP.WF.FlowSort sameLevelSubSubEn = (BP.WF.FlowSort)subEn.DoCreateSameLevelNode();
        sameLevelSubSubEn.Name = " Daily office ";
        sameLevelSubSubEn.Update();

        // Create a subdirectory under a node 1.
        BP.WF.FlowSort sameLevelSubSubEn2 = (BP.WF.FlowSort)subEn.DoCreateSameLevelNode();
        sameLevelSubSubEn2.Name = " Human Resources ";
        sameLevelSubSubEn2.Update();
        /**
         *    Root directory 
         *      Executive class 
         *         Daily office 
         *         Human Resources 
         *      Business Class 
         * 
         * 
         */
    }
    /// <summary>
    ///  Many relationship , This entity will have two columns ( Property )
    ///  Maybe all foreign key columns , And also the primary key for the table .
    /// </summary>
    public void EnttiyMM()
    {
        BP.Port.EmpStation en = new BP.Port.EmpStation();
        en.FK_Emp = "zhangsan";
        en.FK_Station = "01";
        en.Insert();
    }
}

 