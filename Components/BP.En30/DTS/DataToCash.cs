using System;
using System.Data;
using System.Collections;
using BP;
using BP.DA;
using BP.En;

namespace BP.DTS
{
    public class DataToCash : DataIOEn
    {
        public DataToCash()
        {
            this.HisDoType = DoType.Especial;
            this.Title = " Scheduling data to  cash  Go ";
          //  this.HisUserType = Web.UserType.SysAdmin;

            this.DefaultEveryMin = "00";
            this.DefaultEveryHH = "00";
            this.DefaultEveryDay = "00";
            this.DefaultEveryMonth = "00";
            this.Note = "";
        }
        public override void Do()
        {
            Log.DebugWriteInfo(" Started  DataToCahs ");
            string sql = "";
            string str = "";

            #region  Enumerated type into cash.
            sql = "  SELECT DISTINCT ENUMKEY FROM SYS_ENUM ";
            DataTable dt = DBAccess.RunSQLReturnTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                str = dr[0].ToString();
                BP.Sys.SysEnums ses = new BP.Sys.SysEnums(str);
            }
            #endregion
          
            #region  Dispatching documents 
            //if (SystemConfig.SysNo == SysNoList.WF)
            //{
            //    Log.DefaultLogWriteLineInfo(" Document templates ");
            //    sql = "SELECT URL FROM WF_NODEREFFUNC  ";
            //    dt = DBAccess.RunSQLReturnTable(sql);
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        try
            //        {
            //            str = Cash.GetBillStr(dr[0].ToString(),false);
            //        }
            //        catch (Exception ex)
            //        {
            //            Log.DefaultLogWriteLineInfo("@ Transferred documents cash  Error :" + ex.Message);
            //        }
            //    }
            //}
            #endregion

            #region  The class of data into cash.
            // entity  Data into cash.
            ArrayList al = ClassFactory.GetObjects("BP.En.Entities");
            foreach (Entities ens in al)
            {
                Depositary where;
                try
                {
                    where = ens.GetNewEntity.EnMap.DepositaryOfEntity;
                }
                catch (Exception ex)
                {
                    Log.DefaultLogWriteLine(LogType.Info, "@ Error occurs when the data in memory :" + ex.Message + " cls=" + ens.ToString());
                    /*  Contains the user login information map  Do not take it . */
                    continue;
                }

                if (where == Depositary.None)
                    continue;

                //try
                //{
                //    ens.FlodInCash();
                //}
                //catch (Exception ex)
                //{
                //    Log.DefaultLogWriteLine(LogType.Info, "@ The data into  cash  In error .@" + ex.Message);
                //}
            }
            #endregion

            #region  °Ñxml  Data into cash.
            al = ClassFactory.GetObjects("BP.XML.XmlEns");
            foreach (BP.XML.XmlEns ens in al)
            {
                try
                {
                    dt = ens.GetTable();
                    ens.RetrieveAll();
                }
                catch (Exception ex)
                {
                    Log.DefaultLogWriteLineError("@ Dispatch  " + ens.ToString() + " Error :" + ex.Message);
                }
            }
            #endregion

            Log.DefaultLogWriteLine(LogType.Info, " End execution DataToCahs ");
        }
    }
}
