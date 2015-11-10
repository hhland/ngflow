<%@ WebService Language="C#" Class="DataInterface" %>

using System;
using System.Web;
using System.Data;
using System.Web.Services;
using System.Web.Services.Protocols;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow the use of  ASP.NET AJAX  Call this from a script  Web  Service , Please cancel the downlink comment . 
// [System.Web.Script.Services.ScriptService]
public class DataInterface  : System.Web.Services.WebService {

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";

    }
    /// <summary>
    ///  Carried out SQL
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    public static int RunSQL(string sql)
    {
        //throw new Exception(" You need to override this method to let ccflow Calling .");
        return BP.DA.DBAccess.RunSQL(sql);
    }
    /// <summary>
    ///  Run sql Return datatable.
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    public static DataTable RunSQLReturnTable(string sql)
    {
        //throw new Exception(" You need to override this method to let ccflow Calling .");
        return BP.DA.DBAccess.RunSQLReturnTable(sql);
    }
}