<%@ WebService Language="C#" Class="CCFlowWebService" %>
using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

/*
  Explanation : Such a method is used for the user to rewrite , And achieve their business logic .
 */

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow the use of  ASP.NET AJAX  Call this from a script  Web  Service , Please cancel the downlink comment . 
// [System.Web.Script.Services.ScriptService]
public class CCFlowWebService : System.Web.Services.WebService
{
    /// <summary>
    ///  The method uses : Check the username and password are correct .
    ///  Call location :D:\ccflow\VisualFlow\WF\UC\Login.ascx.cs
    ///  Significance achieved : Meaning override this method is that you can modify to implement logic , Implementing your own check mode .
    ///  Such as :
    /// 1, User does not exist how do ?
    /// 2, How do users post insufficiency ?
    /// 3,  Users no department or division numbers do not how ?
    /// 4, Need to how to deal with secondary encryption password ?
    /// </summary>
    /// <param name="userNo"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public bool CheckUserPass(string userNo, string password)
    {
        BP.Port.Emp emp = new BP.Port.Emp();
        emp.No = userNo;
        if (emp.RetrieveFromDBSources() == 0)
            return false; /* Does not exist */
        

        #region  Check whether the user is disabled .
        /*  Check if the user is disabled */
        BP.WF.Port.WFEmp wfemp = new BP.WF.Port.WFEmp();
        wfemp.No = emp.No;
        if (wfemp.RetrieveFromDBSources() == 0)
        {
            wfemp.UseSta = 1;
            wfemp.Copy(emp);
            wfemp.Insert();
        }
        else
        {
            if (wfemp.UseSta == 0)
                throw new Exception(" Change user has been disabled ");
        }
        #endregion  Check whether the user is disabled .

        #region  Check password .
        if (emp.Pass == password)
            return true; /* The password is correct : Here is done with plain-text password check , You can modify the encryption into their own */
        return false;
        #endregion  Check password .

    }
}