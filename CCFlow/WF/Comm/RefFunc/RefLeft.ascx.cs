using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.DA;
using BP.En;

namespace CCFlow.WF.Comm.RefFunc
{
    public partial class RefLeft : BP.Web.UC.UCBase3
    {
        #region  Property 

        /// <summary>
        /// CCFlow Home directory Url
        /// <para></para>
        /// <para>added by liuxc,2014-10-23</para>
        /// </summary>
        public string CCFlowPath
        {
            get
            {
                return string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority,
                                     Request.ApplicationPath.EndsWith("/")
                                         ? Request.ApplicationPath
                                         : Request.ApplicationPath + "/");
            }
        }

        public string PK
        {
            get
            {
                if (ViewState["PK"] == null)
                {
                    string pk = this.Request.QueryString["PK"];
                    if (pk == null)
                        pk = this.Request.QueryString["No"];

                    if (pk == null)
                        pk = this.Request.QueryString["RefNo"];

                    if (pk == null)
                        pk = this.Request.QueryString["OID"];

                    if (pk == null)
                        pk = this.Request.QueryString["MyPK"];


                    if (pk != null)
                    {
                        ViewState["PK"] = pk;
                    }
                    else
                    {
                        Entity mainEn = BP.En.ClassFactory.GetEn(this.EnName);
                        ViewState["PK"] = this.Request.QueryString[mainEn.PK];
                    }
                }
                return ViewState["PK"] as string;
            }
        }

        public string AttrKey
        {
            get
            {
                return this.Request.QueryString["AttrKey"];
            }
        }

        public new string EnName
        {
            get
            {
                string enName = this.Request.QueryString["EnName"];
                string ensName = this.Request.QueryString["EnsName"];
                if (enName == null && ensName == null)
                    throw new Exception("@ Missing argument ");

                if (enName == null)
                    enName = this.ViewState["EnName"] as string;

                if (enName == null)
                {
                    Entities ens = ClassFactory.GetEns(this.EnsName);
                    this.ViewState["EnName"] = ens.GetNewEntity.ToString();
                    enName = this.ViewState["EnName"].ToString();
                }
                return enName;
            }
        }

        public new string EnsName
        {
            get
            {
                string enName = this.Request.QueryString["EnName"];
                string ensName = this.Request.QueryString["EnsName"];
                if (enName == null && ensName == null)
                    throw new Exception("@ Missing argument ");


                if (ensName == null)
                    ensName = this.ViewState["EnsName"] as string;
                if (ensName == null)
                {
                    Entity en = ClassFactory.GetEn(this.EnName);
                    this.ViewState["EnsName"] = en.GetNewEntities.ToString();
                    ensName = this.ViewState["EnsName"].ToString();
                }
                return ensName;
            }
        }

        /// <summary>
        ///  Number of functions 
        /// </summary>
        public int ItemCount { get; set; }
        #endregion

        #region Private Property,added by liuxc,2014-10-23

        /// <summary>
        ///  Node attribute default icon menu on the left of the first term 
        /// </summary>
        private const string IconFirstDefault = "WF/Img/Home.gif";

        /// <summary>
        ///  Default icon menu on the left node attribute-many 
        /// </summary>
        private const string IconM2MDefault = "WF/Img/M2M.png";

        /// <summary>
        ///  Default icon junction details of properties on the left menu 
        /// </summary>
        private const string IconDtlDefault = "WF/Img/Btn/Dtl.gif";

        /// <summary>
        ///  Whether to display the menu on the left node attributes default icon 
        /// </summary>
        private const bool ShowIconDefault = true;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            Entity en = BP.En.ClassFactory.GetEn(this.EnName);
            if (this.PK == null)
                return;

            if (en == null)
                throw new Exception(this.EnsName + " " + this.EnName);

            if (en.EnMap.AttrsOfOneVSM.Count + en.EnMap.Dtls.Count + en.EnMap.HisRefMethods.Count == 0)
                return;

            en.PKVal = this.PK;
            string keys = "&" + en.PK + "=" + this.PK + "&r=" + DateTime.Now.ToString("MMddhhmmss");

            string titleKey = "";

            if (en.EnMap.Attrs.Contains("Name"))
                titleKey = "Name";
            else if (en.EnMap.Attrs.Contains("Title"))
                titleKey = "Title";
            string desc = en.EnDesc;
            if (titleKey != "")
            {
                en.RetrieveFromDBSources();
                desc = en.GetValStrByKey(titleKey);
                if (desc.Length > 30)
                    desc = en.EnDesc;
            }

            AddUL("class='navlist'");
            AddLi(
                string.Format("<div><a href='UIEn.aspx?EnName={0}&PK={1}'>{4}<span class='nav'>{2}</span></a></div>{3}", EnName, PK, titleKey == "Title" ? " Home " : desc, Environment.NewLine, GetIcon(IconFirstDefault)));

            #region  Join many of the entity editor 
            AttrsOfOneVSM oneVsM = en.EnMap.AttrsOfOneVSM;
            string sql = "";
            int i = 0;

            if (oneVsM.Count > 0)
            {
                foreach (AttrOfOneVSM vsM in oneVsM)
                {
                    string url = "Dot2Dot.aspx?EnsName=" + en.GetNewEntities.ToString() + "&EnName=" + this.EnName + "&AttrKey=" + vsM.EnsOfMM.ToString() + keys;
                    try
                    {
                        sql = "SELECT COUNT(*) as NUM FROM " + vsM.EnsOfMM.GetNewEntity.EnMap.PhysicsTable + " WHERE " + vsM.AttrOfOneInMM + "='" + en.PKVal + "'";
                        i = DBAccess.RunSQLReturnValInt(sql);
                    }
                    catch
                    {
                        sql = "SELECT COUNT(*) as NUM FROM " + vsM.EnsOfMM.GetNewEntity.EnMap.PhysicsTable + " WHERE " + vsM.AttrOfOneInMM + "=" + en.PKVal;
                        try
                        {
                            i = DBAccess.RunSQLReturnValInt(sql);
                        }
                        catch
                        {
                            vsM.EnsOfMM.GetNewEntity.CheckPhysicsTable();
                        }
                    }
                    if (i == 0)
                    {
                        if (this.AttrKey == vsM.EnsOfMM.ToString())
                        {
                            AddLi(string.Format(
                                "<div style='font-weight:bold'><a href='{0}'>{3}<span class='nav'>{1}</span></a></div>{2}",
                                url, vsM.Desc, Environment.NewLine, GetIcon(IconM2MDefault)));
                            ItemCount++;
                        }
                        else
                        {
                            AddLi(string.Format("<div><a href='{0}'>{3}<span class='nav'>{1}</span></a></div>{2}", url, vsM.Desc, Environment.NewLine, GetIcon(IconM2MDefault)));
                            ItemCount++;
                        }
                    }
                    else
                    {
                        if (this.AttrKey == vsM.EnsOfMM.ToString())
                        {
                            AddLi(string.Format(
                                "<div style='font-weight:bold'><a href='{0}'>{4}<span class='nav'>{1}[{2}]</span></a></div>{3}",
                                url, vsM.Desc, i, Environment.NewLine, GetIcon(IconM2MDefault)));
                            ItemCount++;
                        }
                        else
                        {
                            AddLi(string.Format("<div><a href='{0}'>{4}<span class='nav'>{1}[{2}]</span></a></div>{3}", url, vsM.Desc, i, Environment.NewLine, GetIcon(IconM2MDefault)));
                            ItemCount++;
                        }
                    }
                }
            }
            #endregion

            #region  Joined his door   Method 
            RefMethods myreffuncs = en.EnMap.HisRefMethods;
            string path = this.Request.ApplicationPath;
            foreach (RefMethod func in myreffuncs)
            {
                if (func.Visable == false || func.RefAttrKey != null)
                    continue;

                if (func.RefMethodType != RefMethodType.Func)
                {
                    string myurl = func.Do(null) as string;
                    int h = func.Height;

                    if (func.RefMethodType == RefMethodType.RightFrameOpen)
                    {
                        AddLi(string.Format(
                                "<div><a href='javascript:void(0)' onclick=\"javascript:OpenUrlInRightFrame(this,'{0}')\" title='{1}'>{4}<span class='nav'>{2}</span></a></div>{3}",
                                myurl, func.ToolTip, func.Title, Environment.NewLine, GetIcon(func.Icon)));
                        ItemCount++;
                        continue;
                    }

                    if (func.Target == null)
                    {
                        //this.AddLi(func.GetIcon(path) + "<a href='" + myurl + "' ToolTip='" + func.ToolTip + "' >" + func.Title + "</a>");
                        AddLi(string.Format("<div><a href='{0}' title='{1}'>{4}<span class='nav'>{2}</span></a></div>{3}", myurl, func.ToolTip, func.Title, Environment.NewLine, GetIcon(func.Icon)));
                        ItemCount++;
                    }
                    else
                    {
                        //this.AddLi(func.GetIcon(path) + "<a href=\"javascript:WinOpen('" + myurl + "','" + func.Target + "')\" ToolTip='" + func.ToolTip + "' >" + func.Title + "</a>");
                        AddLi(string.Format("<div><a href=\"javascript:WinOpen('{0}','{1}')\" title='{2}'>{5}<span class='nav'>{3}</span></a></div>{4}", myurl, func.Target, func.ToolTip, func.Title, Environment.NewLine, GetIcon(func.Icon)));
                        ItemCount++;
                    }
                    continue;
                }

                // string url = path + "/Comm/RefMethod.aspx?Index=" + func.Index + "&EnsName=" + hisens.ToString() + keys;
                string url = "../RefMethod.aspx?Index=" + func.Index + "&EnsName=" + en.GetNewEntities.ToString() + keys;

                //  string urlRefFunc = "RefMethod.aspx?Index=" + func.Index + "&EnsName=" + en.GetNewEntities.ToString() + keys;
                if (func.Warning == null)
                {
                    if (func.Target == null)
                    {
                        //this.AddLi(func.GetIcon(path) + "<a href='" + url + "' ToolTip='" + func.ToolTip + "' >" + func.Title + "</a>");
                        AddLi(string.Format("<div><a href='{0}' title='{1}'>{4}<span class='nav'>{2}</span></a></div>{3}", url, func.ToolTip, func.Title, Environment.NewLine, GetIcon(func.Icon)));
                        ItemCount++;
                    }
                    else
                    {
                        //this.AddLi(func.GetIcon(path) + "<a href=\"javascript:WinOpen('" + url + "','" + func.Target + "')\" ToolTip='" + func.ToolTip + "' >" + func.Title + "</a>");
                        AddLi(string.Format("<div><a href=\"javascript:WinOpen('{0}','{1}')\" title='{2}'>{5}<span class='nav'>{3}</span></a></div>{4}", url, func.Target, func.ToolTip, func.Title, Environment.NewLine, GetIcon(func.Icon)));
                        ItemCount++;
                    }
                }
                else
                {
                    if (func.Target == null)
                    {
                        //this.AddLi(func.GetIcon(path) + "<a href=\"javascript: if ( confirm('" + func.Warning + "') ) { window.location.href='" + url + "' }\" ToolTip='" + func.ToolTip + "' >" + func.Title + "</a>");
                        AddLi(string.Format(
                            "<div><a href=\"javascript: if ( confirm('{0}')){{ window.location.href='{1}' }}\" title='{2}'>{5}<span class='nav'>{3}</span></a></div>{4}",
                            func.Warning, url, func.ToolTip, func.Title, Environment.NewLine, GetIcon(func.Icon)));
                        ItemCount++;
                    }
                    else
                    {
                        //this.AddLi(func.GetIcon(path) + "<a href=\"javascript: if ( confirm('" + func.Warning + "') ) { WinOpen('" + url + "','" + func.Target + "') }\" ToolTip='" + func.ToolTip + "' >" + func.Title + "</a>");
                        AddLi(string.Format(
                            "<div><a href=\"javascript: if ( confirm('{0}')){{ WinOpen('{1}','{2}') }}\" title='{3}'>{6}<span class='nav'>{4}</span></a></div>{5}",
                            func.Warning, url, func.Target, func.ToolTip, func.Title, Environment.NewLine, GetIcon(func.Icon)));
                        ItemCount++;
                    }
                }
            }
            #endregion

            #region  He added detail 
            EnDtls enDtls = en.EnMap.Dtls;
            foreach (EnDtl enDtl in enDtls)
            {
                string url = "Dtl.aspx?EnName=" + this.EnName + "&PK=" + this.PK + "&EnsName=" + enDtl.EnsName + "&RefKey=" + enDtl.RefKey + "&RefVal=" + en.PKVal.ToString() + "&MainEnsName=" + en.ToString() + keys;

                try
                {
                    i = DBAccess.RunSQLReturnValInt("SELECT COUNT(*) FROM " + enDtl.Ens.GetNewEntity.EnMap.PhysicsTable + " WHERE " + enDtl.RefKey + "='" + en.PKVal + "'");
                }
                catch
                {
                    try
                    {
                        i = DBAccess.RunSQLReturnValInt("SELECT COUNT(*) FROM " + enDtl.Ens.GetNewEntity.EnMap.PhysicsTable + " WHERE " + enDtl.RefKey + "=" + en.PKVal);
                    }
                    catch
                    {
                        enDtl.Ens.GetNewEntity.CheckPhysicsTable();
                    }
                }

                if (i == 0)
                {
                    //this.AddLi("<a href=\"" + url + "\"  >" + enDtl.Desc + "</a>");
                    AddLi(string.Format("<div><a href='{0}'>{3}<span class='nav'>{1}</span></a></div>{2}", url, enDtl.Desc, Environment.NewLine, GetIcon(IconDtlDefault)));
                    ItemCount++;
                }
                else
                {
                    //this.AddLi("<a href=\"" + url + "\"   >" + enDtl.Desc + "-" + i + "</a>");
                    AddLi(string.Format("<div><a href='{0}'>{4}<span class='nav'>{1} [{2}]</span></a></div>{3}", url, enDtl.Desc, i, Environment.NewLine, GetIcon(IconDtlDefault)));
                    ItemCount++;
                }
            }
            #endregion

            AddULEnd();
        }

        /// <summary>
        ///  Get the junction properties on the left menu item Default Front icon 
        /// <para></para>
        /// <para> According to page set ShowIconDefault与IconXXXDefault To generate </para>
        /// </summary>
        /// <param name="imgPath"> Relative path icon , Empty or the default icon for details </param>
        /// <returns></returns>
        private string GetIcon(string imgPath)
        {
            if (!ShowIconDefault) return string.Empty;

            return "<img src='" + CCFlowPath + (string.IsNullOrWhiteSpace(imgPath) ? IconDtlDefault : imgPath).Replace("//", "/").TrimStart('/') + "' width='16' border='0' />";
        }
    }
}