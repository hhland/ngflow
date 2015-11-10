using System;
using System.Collections;
using BP.DA;
using BP.Port;
using BP.En;
using BP.Web;
using BP.Sys;

namespace BP.WF.Template.Ext
{
    /// <summary>
    ///  Process 
    /// </summary>
    public class FlowDoc : EntityNoName
    {
        #region  Constructor 
        /// <summary>
        /// UI Access control interface 
        /// </summary>
        public override UAC HisUAC
        {
            get
            {
                UAC uac = new UAC();
                if (BP.Web.WebUser.No == "admin")
                    uac.IsUpdate = true;
                return uac;
            }
        }
        /// <summary>
        ///  Process 
        /// </summary>
        public FlowDoc()
        {
        }
        /// <summary>
        ///  Process 
        /// </summary>
        /// <param name="_No"> Serial number </param>
        public FlowDoc(string _No)
        {
            this.No = _No;
            if (SystemConfig.IsDebug)
            {
                int i = this.RetrieveFromDBSources();
                if (i == 0)
                    throw new Exception(" Process number does not exist ");
            }
            else
            {
                this.Retrieve();
            }
        }
        /// <summary>
        ///  Override the base class methods 
        /// </summary>
        public override Map EnMap
        {
            get
            {
                if (this._enMap != null)
                    return this._enMap;

                Map map = new Map("WF_Flow");

                map.DepositaryOfEntity = Depositary.None;
                map.DepositaryOfMap = Depositary.Application;
                map.EnDesc = " Process ";
                map.CodeStruct = "3";

                map.AddTBStringPK(BP.WF.Template.FlowAttr.No, null, " Serial number ", true, true, 1, 10, 3);
                map.AddTBString(BP.WF.Template.FlowAttr.Name, null, " Name ", true, false, 0, 200, 10);


                map.AddDDLSysEnum(FlowAttr.FlowRunWay, (int)FlowRunWay.HandWork, " Run ", true, true, FlowAttr.FlowRunWay,
                    "@0= Manually start @1= Start time designated staff @2= Data collection started on time @3= Trigger Start ");
                map.AddTBString(FlowAttr.RunObj, null, " Run content ", true, false, 0, 100, 10);

                map.AddTBString(BP.WF.Template.FlowAttr.Note, null, " Remark ", true, false, 0, 100, 10, true);
                

                map.AddTBString(FlowAttr.StartGuidePara1, null, " Navigation Url", true, false, 0, 500, 10, true);


                // map.AddBoolean(BP.WF.FlowAttr.CCType, false, " After the process is complete Cc participants ", true, true);
                // map.AddTBString(BP.WF.FlowAttr.CCStas, null, " To Cc posts ", false, false, 0, 2000, 10);
                // map.AddTBDecimal(BP.WF.FlowAttr.AvgDay, 0, " The average running in days ", false, false);

                RefMethod rm = new RefMethod();
                rm.Title = " Design inspection report "; // " Design inspection report ";
                rm.ToolTip = " Inspection process design problems .";
                rm.Icon = "/WF/Img/Btn/Confirm.gif";
                rm.ClassMethodName = this.ToString() + ".DoCheck";
                map.AddRefMethod(rm);

                rm = new RefMethod();
                rm.Title = " View definition "; //" View definition ";
                rm.Icon = "/WF/Img/Btn/View.gif";
                rm.ClassMethodName = this.ToString() + ".DoDRpt";
                map.AddRefMethod(rm);

                rm = new RefMethod();
                rm.Title = " Reports run "; // " Reports run ";
                rm.ClassMethodName = this.ToString() + ".DoOpenRpt()";
                //rm.Icon = "/WF/Img/Btn/Table.gif";
                map.AddRefMethod(rm);

                rm = new RefMethod();
                rm.Title = " Data transferred definition ";  //" Data transferred definition ";
                //  rm.Icon = "/WF/Img/Btn/Table.gif";
                rm.ToolTip = " In the process completion time , Process data is transferred to another storage system applications .";

                rm.ClassMethodName = this.ToString() + ".DoExp";
                map.AddRefMethod(rm);

                //map.AttrsOfOneVSM.Add(new FlowStations(), new Stations(), FlowStationAttr.FK_Flow,
                //    FlowStationAttr.FK_Station, DeptAttr.Name, DeptAttr.No, " Cc jobs ");


                this._enMap = map;
                return this._enMap;
            }
        }
        #endregion

        #region   Public Methods 
        /// <summary>
        ///  Perform checks 
        /// </summary>
        /// <returns></returns>
        public string DoCheck()
        {
            Flow fl = new Flow();
            fl.No = this.No;
            fl.RetrieveFromDBSources();
            return fl.DoCheck();
        }
        /// <summary>
        ///  Design data turn out 
        /// </summary>
        /// <returns></returns>
        public string DoExp()
        {
            Flow fl = new Flow();
            fl.No = this.No;
            fl.RetrieveFromDBSources();
            return fl.DoExp();
        }
        /// <summary>
        ///  Defining Report 
        /// </summary>
        /// <returns></returns>
        public string DoDRpt()
        {
            Flow fl = new Flow();
            fl.No = this.No;
            fl.RetrieveFromDBSources();
            return fl.DoDRpt();
        }
        /// <summary>
        ///  Run the report 
        /// </summary>
        /// <returns></returns>
        public string DoOpenRpt()
        {
            Flow fl = new Flow();
            fl.No = this.No;
            fl.RetrieveFromDBSources();
            return fl.DoOpenRpt();
        }
        /// <summary>
        ///  Things after update 
        /// </summary>
        protected override void afterUpdate()
        {
            Flow fl = new Flow();
            fl.No = this.No;
            fl.RetrieveFromDBSources();
            fl.Update();
            base.afterUpdate();
        }
        #endregion
    }
    /// <summary>
    ///  Collection process 
    /// </summary>
    public class FlowDocs : EntitiesNoName
    {
        #region  Inquiry 
        /// <summary>
        ///  During check out the entire process in survival within 
        /// </summary>
        /// <param name="FlowSort"> Process Category </param>
        /// <param name="IsCountInLifeCycle"> Is not included in the period of survival  true  Check out all of the  </param>
        public void Retrieve(string FlowSort)
        {
            QueryObject qo = new QueryObject(this);
            qo.AddWhere(BP.WF.FlowAttr.FK_FlowSort, FlowSort);
            qo.addOrderBy(BP.WF.FlowAttr.No);
            qo.DoQuery();
        }
        #endregion

        #region  Constructor 
        /// <summary>
        ///  Workflow 
        /// </summary>
        public FlowDocs() { }
        /// <summary>
        ///  Workflow 
        /// </summary>
        /// <param name="fk_sort"></param>
        public FlowDocs(string fk_sort)
        {
            this.Retrieve(BP.WF.FlowAttr.FK_FlowSort, fk_sort);
        }
        #endregion

        #region  Get real 
        /// <summary>
        ///  Get it  Entity 
        /// </summary>
        public override Entity GetNewEntity
        {
            get
            {
                return new FlowDoc();
            }
        }
        #endregion
    }
}

