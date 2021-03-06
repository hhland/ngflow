﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

namespace CCFlow.WF.Admin
{
    public partial class SimulationRunEUI : System.Web.UI.Page
    {
        #region  Property .
        /// <summary>
        ///  Staff ID
        /// </summary>
        public string IDs
        {
            get
            {
                return this.Request.QueryString["IDs"];
            }
        }
        /// <summary>
        ///  Process ID 
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.Request.QueryString["FK_Flow"];
            }
        }
        #endregion  Property .
        public string getUTF8ToString(string param)
        {
            return HttpUtility.UrlDecode(Request[param], System.Text.Encoding.UTF8);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
          
         //   ArrayList htmlArr = new ArrayList();
         ////   htmlArr = BP.WF.Glo.Simulation_RunOne(this.FK_Flow, "1922", "");
         //   this.Response.Write(htmlArr[0]);
            //return "";
       

         




            string method = string.Empty;
            // The return value 
            string s_responsetext = string.Empty;
            if (string.IsNullOrEmpty(Request["method"]))
                return;

            method = Request["method"].ToString();
            switch (method)
            {
                //case "getMyData":
                //    s_responsetext = getMyData();
                //    break;
                //case "insertSameNodeMet":
                //    s_responsetext = insertSameNodeMet();
                //    break;
                //case "insertSonNodeMet":
                //    s_responsetext = insertSonNodeMet();
                //    break;
                //case "editNodeMet":
                //    s_responsetext = editNodeMet();
                //    break;
                //case "delNodeMet":
                //    s_responsetext = delNodeMet();
                //    break;
            }
            if (string.IsNullOrEmpty(s_responsetext))
                s_responsetext = "";
            // Assembly ajax String format , Return to the calling client 
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "text/html";
            Response.Expires = 0;
            Response.Write(s_responsetext);
            Response.End();
            //string[] emps = this.IDs.Split(',');
        }
        private string getData()
        {
            //ArrayList htmlArr = new ArrayList();
            //htmlArr = BP.WF.Glo.Simulation_RunOne(this.FK_Flow, "1922", "");
            //this.Response.Write(htmlArr[0]);
            return "";
        }
    }
}