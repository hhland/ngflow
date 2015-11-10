using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
///Event  The summary 
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow the use of  ASP.NET AJAX  Call this from a script  Web  Service , Please cancel the downlink comment . 
// [System.Web.Script.Services.ScriptService]
public class Event : System.Web.Services.WebService {

    public Event () {

        // If you are using design components , Uncomment the following line  
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }

    [WebMethod]
    public string DoEvent(int fk_node, int workid, string eventType, string paras)
    {
        switch (fk_node)
        {
            case 102:
                this.Do102(  workid,   eventType,   paras);
                break;
            default:
                break;
        }
        return "Hello World";
    }

    public string Do102(int workid, string eventType, string paras)
    {

        return null;
    }

    
      
}
