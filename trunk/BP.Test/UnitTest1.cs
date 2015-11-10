using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using BP.Sys;

namespace BP.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            
         //   string addr = "249750689@qq.com", title = "deal";
         //   PubClass.SendMail(addr, title, title);
        }

        [TestMethod]
        public void TestReplace() {
            string mailDocTmp = "test table NPO TT_@WF.WorkID have been created with below information:            Project Type : @Project_Type Project Trigger: "
         +"   @Project_Trigger   #ddltable(npo_tt_projecttrigger,Project_Trigger) #dtl(ND16701Dtl1) ";
            string ddltable_prefix = "#ddltable(";
            while (mailDocTmp.Contains(ddltable_prefix))
            {
                while (mailDocTmp.Contains(ddltable_prefix))
                {
                    int si = mailDocTmp.IndexOf(ddltable_prefix), ei = mailDocTmp.Substring(si).IndexOf(")");
                    string pattern = mailDocTmp.Substring(si, ei + 1);
                    string[] vals = pattern.Substring(ddltable_prefix.Length).TrimEnd(',').Split(',');
                    string tablename = vals[0], enname = vals[1];
                   
                }
            }
        }

        [TestMethod]
        public void TestAjax() {
            string url = "http://188.75.104.92:9780/WF/CCForm/HanderMapExt.ashx?Key=E4959&FK_MapExt=ND16701Dtl1_TBFullCtrl_MainSiteID&DoType=ReqCtrl&KVs=";
        }
    }
}
