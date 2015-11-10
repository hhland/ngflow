using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BP.DA;

namespace BP.Web.UC
{
    /// <summary>
    /// Well  The summary .
    /// </summary>
    public class UCBase3 : BP.Web.UC.UCBase2
    {
        #region  Information block -  Apply mechanically 
        public void DivInfoBlockBegin()
        {
            return;

            //this.Add("<style>");
            //this.Add(".xsnazzy span {width:20px; height:10px; w\\idth:0; hei\\ght:0;}");
            //this.Add(" .xb1, .xb2, .xb3, .xb4, .xb5, .xb6, .xb7 {display:block; overflow:hidden; font-size:0;}");
            //this.Add(".xb1, .xb2, .xb3, .xb4, .xb5, .xb6 {height:1px;}");
            //this.Add(".xb4, .xb5, .xb6, .xb7 {background:#ccc; border-left:1px solid #fff; border-right:1px solid #fff;}");
            //this.Add(".xb1 {margin:0 8px; background:#fff;}");
            //this.Add(".xb2 {margin:0 6px; background:#fff;}");
            //this.Add(".xb3 {margin:0 4px; background:#fff;}");
            //this.Add(".xb4 {margin:0 3px; background:#7f7f9c; border-width:0 5px;}");
            //this.Add(".xb5 {margin:0 2px; background:#7f7f9c; border-width:0 4px;}");
            //this.Add(".xb6 {margin:0 2px; background:#7f7f9c; border-width:0 3px;} ");
            //this.Add(".xb7 {margin:0 1px; background:#7f7f9c; border-width:0 3px; height:2px;} ");
            //this.Add(".xboxcontent {display:block; background:#7f7f9c; border:3px solid #fff; border-width:0 3px;}");
            //this.Add("</style>");

            //this.Add("<div class='container'>");
            //this.Add("<div class='xsnazzy' >");
            //this.Add("<b class='xb1'></b><b class=xb2></b><b class=xb3></b><b class=xb4></b><b class=xb5></b><b class=xb6></b><b class=xb7></b>");
            //this.Add("<div class='xboxcontent' >");
            //return;
        }

        public void DivInfoBlockEnd()
        {
            return;
            //this.AddDivEnd();
            //this.Add("<b class=xb7></b><b class=xb6></b><b class=xb5></b><b class=xb4></b><b class=xb3></b><b class=xb2></b><b class=xb1></b></div>");
            //return;

            //string path = this.Request.ApplicationPath;

            //this.Add("\n</td><td width=4 class='line_l' style='border-right:1px #ccc solid;background:#f9f9f9;'></td>");
            //this.Add("\n</tr>");
            //this.Add("\n<tr class='yj_style'>");
            //this.Add("\n<td style='text-align:right'><img src='/WF/Img/Div/bl_df.jpg'></td>");
            //this.Add("\n<td style='border-bottom:1px #ccc solid;background:#f9f9f9;'></td>");
            //this.Add("\n<td>");
            //this.Add("\n<img src='/WF/Img/Div/br_df.jpg'>");
            //this.Add("\n</td></tr>");
            //this.AddTableEnd();
        }

        #endregion  Information block -  Apply mechanically 


        #region AddMsgGreen
        public void AddMsgGreen(string title, string msg)
        {
            this.AddFieldSet(title);
            this.Add(msg);
            this.AddFieldSetEnd();
            return;


            //   this.DivInfoBlock(title, msg);
            //this.AddTableGreen();
            //this.AddTableBarGreen(title, 1);
            //if (msg != null)
            //{
            //    this.AddTR();
            //    this.Add("<TD class=BigDoc >" + msg + "</TD>");
            //    this.AddTREnd();
            //}
            //this.AddTableEnd();
        }

        public void AddMsgInfo(string title, string msg)
        {

            this.DivInfoBlockRed(title, msg);

            //this.AddTable();

            //this.AddTR();
            //this.Add("<TD class=TitleDef >" + title + "</TD>");
            //this.AddTREnd();

            //this.AddTR();
            //this.Add("<TD class=BigDoc >" + msg + "</TD>");
            //this.AddTREnd();

            //this.AddTableEnd();
        }
        #endregion

        #region  Information block -  Function 
        /// <summary>
        ///  Information block default  
        /// </summary>
        /// <param name="html">html Information </param>
        public void DivInfoBlock(string html)
        {
            this.DivInfoBlockBegin();
            this.Add(html);
            this.DivInfoBlockEnd();
        }
        /// <summary>
        ///  Information block default 
        /// </summary>
        /// <param name="title"> Title </param>
        /// <param name="html"> Content </param>
        public void DivInfoBlock(string title, string html)
        {
            this.DivInfoBlockBegin();
            this.Add("<b>" + title + "</b><br>");
            this.Add(html);
            this.DivInfoBlockEnd();
        }
        public void DivInfoBlockRed(string html)
        {
            DivInfoBlockRed(null, html);
        }
        /// <summary>
        ///  Red block information  
        /// </summary>
        /// <param name="html">html Information </param>
        public void DivInfoBlockRed(string title, string html)
        {
            string path = this.Request.ApplicationPath;
            this.Add("\n<table  cellspacing='0'>");
            this.Add("\n<tr>");
            this.Add("\n<td style='text-align:right'><img src='/WF/Img/Div/tl_red.jpg'></td>");
            this.Add("\n<td style='border-top:1px #ffb9b6 solid;background:#ffebea;'></td>");
            this.Add("\n<td>");
            this.Add("\n<img src='/WF/Img/Div/tr_red.jpg'>");
            this.Add("\n</td></tr>");
            this.Add("\n<tr><td width=4 class='line_l' style='border-left:1px #ffb9b6 solid;background:#ffebea;'></td><td width='100%' style='background:#ffebea;'>");

            if (title != null)
                this.Add("<b>" + title + "</b><br>");

            this.Add(html);

            this.Add("\n</td><td width=4 class='line_l' style='border-right:1px #ffb9b6 solid;background:#ffebea;'></td>");
            this.Add("\n</tr>");
            this.Add("\n<tr>");
            this.Add("\n<td style='text-align:right'><img src='/WF/Img/Div/bl_red.jpg'></td>");
            this.Add("\n<td style='border-bottom:1px #ffb9b6 solid;background:#ffebea;'></td>");
            this.Add("\n<td>");
            this.Add("\n<img src='/WF/Img/Div/br_red.jpg'>");
            this.Add("\n</td></tr>");
            this.AddTableEnd();
        }
        /// <summary>
        ///  Green block information  
        /// </summary>
        /// <param name="html">html Information </param>
        public void DivInfoBlockGreen(string html)
        {
            string path = this.Request.ApplicationPath;
            this.Add("\n<table cellspacing='0'>");
            this.Add("\n<tr>");
            this.Add("\n<td style='text-align:right'><img src='/WF/Img/Div/tl_green.jpg'></td>");
            this.Add("\n<td style='border-top:1px #b5d95e solid;background:#efffc9;'></td>");
            this.Add("\n<td>");
            this.Add("\n<img src='/WF/Img/Div/tr_green.jpg'>");
            this.Add("\n</td></tr>");
            this.Add("\n<tr><td width=4 class='line_l' style='border-left:1px #b5d95e solid;background:#efffc9;'></td><td width='100%' style='background:#efffc9;'>");
            this.Add(html);
            this.Add("\n</td><td width=4 class='line_l' style='border-right:1px #b5d95e solid;background:#efffc9;'></td>");
            this.Add("\n</tr>");
            this.Add("\n<tr>");
            this.Add("\n<td style='text-align:right'><img src='/WF/Img/Div/bl_green.jpg'></td>");
            this.Add("\n<td style='border-bottom:1px #b5d95e solid;background:#efffc9;'></td>");
            this.Add("\n<td>");
            this.Add("\n<img src='/WF/Img/Div/br_green.jpg'>");
            this.Add("\n</td></tr>");
            this.AddTableEnd();
        }
        /// <summary>
        ///  Blue block information  
        /// </summary>
        /// <param name="html">html Information </param>
        public void DivInfoBlockBlue(string html)
        {
            string path = this.Request.ApplicationPath;
            this.Add("\n<table  cellspacing='0'>");
            this.Add("\n<tr>");
            this.Add("\n<td style='text-align:right'><img src='/WF/Img/Div/tl_Blue.jpg'></td>");
            this.Add("\n<td style='border-top:1px #b5e8fa solid;background:#f0fbff;'></td>");
            this.Add("\n<td>");
            this.Add("\n<img src='/WF/Img/Div/tr_Blue.jpg'>");
            this.Add("\n</td></tr>");
            this.Add("\n<tr><td width=4 class='line_l' style='border-left:1px #b5e8fa solid;background:#f0fbff;'></td><td width='100%' style='background:#f0fbff;'>");
            this.Add(html);
            this.Add("\n</td><td width=4 class='line_l' style='border-right:1px #b5e8fa solid;background:#f0fbff;'></td>");
            this.Add("\n</tr>");
            this.Add("\n<tr>");
            this.Add("\n<td style='text-align:right'><img src='/WF/Img/Div/bl_Blue.jpg'></td>");
            this.Add("\n<td style='border-bottom:1px #b5e8fa solid;background:#f0fbff;'></td>");
            this.Add("\n<td>");
            this.Add("\n<img src='/WF/Img/Div/br_Blue.jpg'>");
            this.Add("\n</td></tr>");
            this.AddTableEnd();
        }
        /// <summary>
        ///  Yellow information block  
        /// </summary>
        /// <param name="html">html Information </param>
        public void DivInfoBlockYellow(string html)
        {
            string path = this.Request.ApplicationPath;
            this.Add("\n<table  cellspacing='0'>");
            this.Add("\n<tr>");
            this.Add("\n<td style='text-align:right'><img src='/WF/Img/Div/tl_yellow.jpg'></td>");
            this.Add("\n<td style='border-top:1px #f1e167 solid;background:#fffce5;'></td>");
            this.Add("\n<td>");
            this.Add("\n<img src='/WF/Img/Div/tr_yellow.jpg'>");
            this.Add("\n</td></tr>");
            this.Add("\n<tr><td width=4 class='line_l' style='border-left:1px #f1e167 solid;background:#fffce5;'></td><td width='100%' style='background:#fffce5;'>");
            this.Add(html);
            this.Add("\n</td><td width=4 class='line_l' style='border-right:1px #f1e167 solid;background:#fffce5;'></td>");
            this.Add("\n</tr>");
            this.Add("\n<tr>");
            this.Add("\n<td style='text-align:right'><img src='/WF/Img/Div/bl_yellow.jpg'></td>");
            this.Add("\n<td style='border-bottom:1px #f1e167 solid;background:#fffce5;'></td>");
            this.Add("\n<td>");
            this.Add("\n<img src='/WF/Img/Div/br_yellow.jpg'>");
            this.Add("\n</td></tr>");
            this.AddTableEnd();

        }
        #endregion  Information block 

        #region  Menu 
        /// <summary>
        ///  Display the menu 
        /// </summary>
        /// <param name="ens"></param>
        /// <param name="selectVal"></param>
        public void Menu(BP.XML.XmlMenus ens, string selectVal)
        {
            this.Add("\n<Table style='border-bottom:1px #96c1cc solid;border-collapse:collapse;' cellpadding='0' cellspacing='1' >");
            this.Add("\n<TR>");
            this.Add("\n<TD width='2%' ></TD>");
            foreach (BP.XML.XmlMenu en in ens)
            {
                if (selectVal == en.No)
                    this.Add("\n<TD class=MenuS><a href='" + en.Url + "' target=" + en.Target + ">" + en.Name + "</a></TD>");
                else
                    this.Add("\n<TD class=Menu ><a href='" + en.Url + "' target=" + en.Target + ">" + en.Name + "</a></TD>");
            }
            this.Add("\n<TD ></TD>");
            this.Add("\n</TR>");
            this.AddTableEnd();
        }

        /// <summary>
        ///  Display the menu Red
        /// </summary>
        /// <param name="ens"></param>
        /// <param name="selectVal"></param>
        public void MenuRed(BP.XML.XmlMenus ens, string selectVal)
        {
            this.Add("\n<Table style='border-bottom:1px #75001b solid;' cellpadding='0' cellspacing='0' >");
            this.Add("\n<TR>");
            this.Add("\n<TD width='2%' ></TD>");
            foreach (BP.XML.XmlMenu en in ens)
            {
                if (selectVal == en.No)
                    this.Add("\n<TD class=MenuS><a href='" + en.Url + "' target=" + en.Target + ">" + en.Name + "</a></TD>");
                else
                    this.Add("\n<TD class=MenuRed><a href='" + en.Url + "' target=" + en.Target + ">" + en.Name + "</a></TD>");
            }
            this.Add("\n<TD  ></TD>");
            this.Add("\n</TR>");
            this.AddTableEnd();
        }

        /// <summary>
        ///  Display the menu  Green
        /// </summary>
        /// <param name="ens"></param>
        /// <param name="selectVal"></param>
        public void MenuGreen(BP.XML.XmlMenus ens, string selectVal)
        {
            this.Add("\n<Table width='100%' style='border-bottom:1px #5c8a0b solid;' cellpadding='0' cellspacing='0' >");
            this.Add("\n<TR>");
            this.Add("\n<TD width='24%' ></TD>");
            foreach (BP.XML.XmlMenu en in ens)
            {
                if (selectVal == en.No)
                    this.Add("\n<TD class=MenuS><a href='" + en.Url + "' target=" + en.Target + ">" + en.Name + "</a></TD>");
                else
                    this.Add("\n<TD class=MenuGreen><a href='" + en.Url + "' target=" + en.Target + ">" + en.Name + "</a></TD>");
            }
            this.Add("\n<TD   ></TD>");
            this.Add("\n</TR>");
            this.AddTableEnd();
        }

        /// <summary>
        ///  Display the menu  Blue
        /// </summary>
        /// <param name="ens"></param>
        /// <param name="selectVal"></param>
        public void MenuBlue(BP.XML.XmlMenus ens, string selectVal)
        {
            this.Add("\n<Table style='border-bottom:1px #4d71c3 solid;' cellpadding='0' cellspacing='0' >");
            this.Add("\n<TR>");
            this.Add("\n<TD width='2%' ></TD>");
            foreach (BP.XML.XmlMenu en in ens)
            {
                if (selectVal == en.No)
                    this.Add("\n<TD class=MenuS><a href='" + en.Url + "' target=" + en.Target + ">" + en.Name + "</a></TD>");
                else
                    this.Add("\n<TD class=MenuBlue><a href='" + en.Url + "' target=" + en.Target + ">" + en.Name + "</a></TD>");
            }
            this.Add("\n<TD ></TD>");
            this.Add("\n</TR>");
            this.AddTableEnd();
        }

        /// <summary>
        ///  Display the menu  Yellow
        /// </summary>
        /// <param name="ens"></param>
        /// <param name="selectVal"></param>
        public void MenuYellow(BP.XML.XmlMenus ens, string selectVal)
        {
            this.Add("\n<Table style='border-bottom:1px #ffcc00 solid;' cellpadding='0' cellspacing='0' >");
            this.Add("\n<TR>");
            this.Add("\n<TD width='2%' ></TD>");
            foreach (BP.XML.XmlMenu en in ens)
            {
                if (selectVal == en.No)
                    this.Add("\n<TD class=MenuS><a href='" + en.Url + "' target=" + en.Target + ">" + en.Name + "</a></TD>");
                else
                    this.Add("\n<TD class=MenuYellow><a href='" + en.Url + "' target=" + en.Target + ">" + en.Name + "</a></TD>");
            }
            this.Add("\n<TD ></TD>");
            this.Add("\n</TR>");
            this.AddTableEnd();
        }



        /// <summary>
        ///  Display the menu  Win7
        /// </summary>
        /// <param name="ens"></param>
        /// <param name="selectVal"></param>
        public void MenuWin7(BP.XML.XmlMenus ens, string selectVal)
        {
            this.Add("\n<Table  cellpadding='0' cellspacing='0' >");
            this.Add("\n<TR>");
            this.Add("\n<TD width='2%' ></TD>");
            foreach (BP.XML.XmlMenu en in ens)
            {
                if (selectVal == en.No)
                    this.Add("\n<TD class=MenuWin7S><a href='" + en.Url + "' target=" + en.Target + ">" + en.Name + "</a></TD>");
                else
                    this.Add("\n<TD class=MenuWin7><a href='" + en.Url + "' target=" + en.Target + ">" + en.Name + "</a></TD>");
            }
            this.Add("\n<TD  ></TD>");
            this.Add("\n</TR>");
            this.AddTableEnd();
        }

        #endregion


        #region  Method to generate the menu   Significant .
        public void MenuSelfVerticalBegin()
        {
            this.Add("<ul class=MenuSelfVertical >");
        }
        public void MenuSelfVerticalItem(string url, string lab, string target)
        {
            if (target == null)
                target = "_self";
            this.Add("\t\n<li class=MenuSelfVerticalItem ><a href=\"" + url + "\" target=" + target + " >" + lab + "</li>");
        }
        public void MenuSelfVerticalItemS(string url, string lab, string target)
        {
            if (target == null)
                target = "_self";
            this.Add("\t\n<li class=MenuSelfVerticalItemS ><a href=\"" + url + "\" target=" + target + " >" + lab + "</li>");
        }
        public void MenuSelfVerticalEnd()
        {
            this.Add("</ul>");
        }
        #endregion  Method to generate the menu   Sideways .


        #region  Method to generate the menu   Sideways .
        /// <summary>
        ///  Began to increase menu 
        /// </summary>
        public void MenuSelfBegin()
        {
            this.Add("\n<Table style='width:100%;' cellpadding='5' cellspacing='5'>");
            this.Add("\n<TR>");
        }
        /// <summary>
        ///  Adding a lab
        /// </summary>
        /// <param name="attr">TD Inside the property </param>
        /// <param name="lab"> Label </param>
        public void MenuSelfLab(string attr, string lab)
        {
            this.Add("\n<TD " + attr + ">" + lab + "</TD>");
        }

        public void MenuSelfItem(string url, string lab, string target)
        {
            this.Add("\n<TD class=Menu><a href=\"" + url + "\" target=" + target + ">" + lab + "</a></TD>");
        }

        public void MenuSelfItemLab(string lab)
        {
            this.Add("\n<TD class=Menu>" + lab + "</TD>");
        }

        public void MenuSelfItem(string url, string lab, string target, bool selected)
        {
            if (selected == false)
                MenuSelfItem(url, lab, target);
            else
                MenuSelfItemS(url, lab, target);
        }
        public void MenuSelfItemS(string url, string lab, string target)
        {
            this.Add("\n<TD class=MenuS >" + lab + "</TD>");
        }
        /// <summary>
        ///  End menu 
        /// </summary>
        public void MenuSelfEnd(int perBlankLeft)
        {
            this.Add("\n<TD width='" + perBlankLeft + "%' ></TD>");
            this.Add("\n</TR>");
            this.AddTableEnd();
        }
        /// <summary>
        ///  End menu 
        /// </summary>
        public void MenuSelfEnd()
        {
            this.Add("\n</TR>");
            this.AddTableEnd();
        }
        #endregion  Menu 

        #region EasyUI Style Panel Information display methods  added by liuxc,2014-10-22,edited by liuxc,2014-11-28

        /// <summary>
        ///  Adding a EasyUi的Panel, Display a short message , With title 
        /// </summary>
        /// <param name="title"> Title </param>
        /// <param name="msg"> To show </param>
        /// <param name="iconCls"> In front of the icon title , Must be EasyUi中icon.css Defined class </param>
        /// <param name="padding">Panel Internal margins ( Unit :px)</param>
        public void AddEasyUiPanelInfo(string title, string msg, string iconCls = "icon-tip", int padding = 10)
        {
            AddEasyUiPanelInfoBegin(title, iconCls, padding);
            Add(msg);
            AddEasyUiPanelInfoEnd();
        }

        /// <summary>
        ///  Began to increase a EasyUi的Panel, With title 
        /// <remarks>
        /// <para> Watch out : For AddEasyUiPanelInfoEnd Prior methods , Both must be used in conjunction with </para>
        /// </remarks>
        /// </summary>
        /// <param name="title"> Title </param>
        /// <param name="iconCls"> In front of the icon title , Must be EasyUi中icon.css Defined class </param>
        /// <param name="padding">Panel Internal margins ( Unit :px)</param>
        public void AddEasyUiPanelInfoBegin(string title, string iconCls = "icon-tip", int padding = 10)
        {
            Add("<div style='width:100%'>");
            Add(string.Format("<div class='easyui-panel' title='{0}' data-options=\"iconCls:'{1}',fit:true\" style='height:auto;padding:{2}px'>", title, iconCls, padding));
        }

        /// <summary>
        ///  Add an end EasyUi的Panel, With title 
        /// <remarks>
        /// <para> Watch out : For AddEasyUiPanelInfoBegin After method , Both must be used in conjunction with </para>
        /// </remarks>
        /// </summary>
        public void AddEasyUiPanelInfoEnd()
        {
            Add("</div>");
            Add("</div>");
        }
        #endregion

        #region  Client EasyUi Style bomb box method  added by liuxc,2014-10-22

        //public void AddEasyUiMessager(string msg, string title = " Prompt ", int autoCloseMillionSeconds = 2000)
        //{
        //    this.Page.ScriptManager.RegisterClientScriptBlock(this, typeof(string), "msg", "showInfo('" + title + "','" + msg + "'," + autoCloseMillionSeconds + ");", true);
        //}
        #endregion
    }
}
