using System;
using System.Threading;
using System.Collections;
using System.Data;
using BP.DA;
using BP.DTS;
using BP.En;
using BP.Web.Controls;
using BP.Web;
using BP.Sys;

namespace BP.WF
{
    /// <summary>
    ///  Process Event base class 
    /// </summary>
    abstract public class FlowEventBase:EventBase
    {
        #region  Property .
        public Entity HisEn = null;
        private Row _SysPara = null;
        /// <summary>
        ///  Parameters 
        /// </summary>
        public Row SysPara
        {
            get
            {
                if (_SysPara == null)
                    _SysPara = new Row();
                return _SysPara;
            }
            set
            {
                _SysPara = value;
            }
        }
        /// <summary>
        ///  Success Information 
        /// </summary>
        public string SucessInfo = null;
        private string _title = null;
        /// <summary>
        ///  Title 
        /// </summary>
        public string Title
        {
            get
            {
                if (_title == null)
                    _title = " Unnamed ";
                return _title;
            }
            set
            {
                _title = value;
            }
        }
        #endregion  Property .

        #region  System parameters 
        /// <summary>
        ///  Form ID
        /// </summary>
        public string FK_Mapdata
        {
            get
            {
                return this.GetValStr("FK_MapData");
            }
        }
        /// <summary>
        ///  Event Type 
        /// </summary>
        public string EventType
        {
            get
            {
                return this.GetValStr("EventType");
            }
        }
        #endregion 

        #region  Common properties .
        /// <summary>
        ///  The work ID
        /// </summary>
        public int OID
        {
            get
            {
                return this.GetValInt("OID");
            }
        }
        /// <summary>
        ///  The work ID
        /// </summary>
        public Int64 WorkID
        {
            get
            {
                if (this.OID == 0)
                    return this.GetValInt64("WorkID"); /* It is possible to start node WorkID=0*/
                return this.OID;
            }
        }
        /// <summary>
        /// FID
        /// </summary>
        public Int64 FID
        {
            get
            {
                return this.GetValInt64("FID");
            }
        }
        /// <summary>
        ///  Process ID 
        /// </summary>
        public string FK_Flow
        {
            get
            {
                return this.GetValStr("FK_Flow");
            }
        }
        /// <summary>
        ///  Node number 
        /// </summary>
        public int FK_Node
        {
            get
            {
                try
                {
                    return this.GetValInt("FK_Node");
                }
                catch {
                    return 0;
                }
            }
        }
        /// <summary>
        ///  Pass over WorkIDs Set , Subprocess .
        /// </summary>
        public string WorkIDs
        {
            get
            {
                return this.GetValStr("WorkIDs");
            }
        }
        /// <summary>
        ///  Number collections s
        /// </summary>
        public string Nos
        {
            get
            {
                return this.GetValStr("Nos");
            }
        }
        #endregion  Common properties .

        /// <summary>
        ///  Process Event base class 
        /// </summary>
        public FlowEventBase()
        {
        }
        /// <summary>
        ///  Process ID 
        /// </summary>
        abstract public string FlowNo
        {
            get;
        }
    }
}
