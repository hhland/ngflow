using System;
using System.Collections;
using BP.DA;
using BP.Web.Controls;
using System.Reflection;
using BP.Port;
using BP.En;
using BP.Sys;

namespace BP.WF.DTS
{
    /// <summary>
    ///  Repair illegal field name 
    /// </summary>
    public class PackAutoErrFormatFieldTable : Method
    {
        /// <summary>
        ///  Repair illegal field name 
        /// </summary>
        public PackAutoErrFormatFieldTable()
        {
            this.Title = " Repair illegal field name , Physical table name ";
            this.Help = " In previous versions , Users create a form of physical table name , An error occurred while checking the legality of the field name does not cause the system to automatically create a physical table repair physical table . This patch fixes the overall form can batch .";
            // this.Warning = " Are you sure you want to perform ?";
            // this.HisAttrs.AddTBString("Path", "C:\\ccflow.Template", " Path generation ", true, false, 1, 1900, 200);
        }
        /// <summary>
        ///  Set the execution variables 
        /// </summary>
        /// <returns></returns>
        public override void Init()
        {
        }
        /// <summary>
        ///  Whether the current operator can perform this method 
        /// </summary>
        public override bool IsCanDo
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        ///  Carried out 
        /// </summary>
        /// <returns> Return to the results </returns>
        public override object Do()
        {
            string keys = "~!@#$%^&*()+{}|:<>?`=[];,./¡«!£À£££¤£¥¡­¡­£¦¡Á£¨£©¡ª¡ª£«£û£ý£ü:[¡¶¡·?£à£­£½£Û£Ý;£§,£®£¯";
            char[] cc = keys.ToCharArray();
            foreach (char c in cc)
            {
                DBAccess.RunSQL("update sys_mapattr set keyofen=REPLACE(keyofen,'" + c + "' , '')");
            }

            BP.Sys.MapAttrs attrs = new Sys.MapAttrs();
            attrs.RetrieveAll();
            int idx = 0;
            string msg = "";
            foreach (BP.Sys.MapAttr item in attrs)
            {
                string f = item.KeyOfEn.Clone().ToString();
                try
                {
                    int i = int.Parse( item.KeyOfEn.Substring(0, 1) );
                    item.KeyOfEn = "F" + item.KeyOfEn;
                    try
                    {
                        MapAttr itemCopy = new MapAttr();
                        itemCopy.Copy(item);
                        itemCopy.Insert();
                        item.DirectDelete();
                    }
                    catch (Exception ex)
                    {
                        msg += "@" + ex.Message;
                    }
                }
                catch
                {
                    continue;
                }
                DBAccess.RunSQL("UPDATE sys_mapAttr set KeyOfEn='"+item.KeyOfEn+"', mypk=FK_MapData+'_'+keyofen where keyofen='"+item.KeyOfEn+"'");
                msg += "@No.(" + idx + ") Bug fixes success ,old£¨"+f+"£© Restored to ("+item.KeyOfEn+").";
                idx++;
            }

            BP.DA.DBAccess.RunSQL("UPDATE Sys_MapAttr SET MyPK=FK_MapData+'_'+KeyOfEn WHERE MyPK!=FK_MapData+'_'+KeyOfEn");
            return " Repair information is as follows :"+msg;
        }
    }
}
