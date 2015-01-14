using System;
using System.Collections;
using System.Data;
using BP.DA;
using BP.En;
using BP.Web.Controls;
using BP.Web;
using BP.Sys;
using BP.WF.XML;

namespace BP.WF
{
    /// <summary>
    ///  Process Event base class 
    /// </summary>
    abstract public class FlowEventBase
    {
        #region  Property .
        /// <summary>
        ///  Send object 
        /// </summary>
        public SendReturnObjs SendReturnObjs = null;
        /// <summary>
        ///  Entity , Generally working entity 
        /// </summary>
        public Entity HisEn = null;
        /// <summary>
        ///  The current node 
        /// </summary>
        public Node HisNode = null;
        /// <summary>
        ///  Parameter Object .
        /// </summary>
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

        #region  Get parametric method 
        public DateTime GetValDateTime(string key)
        {
            try
            {
                string str = this.SysPara.GetValByKey(key).ToString();
                return DataType.ParseSysDateTime2DateTime(str);
            }
            catch (Exception ex)
            {
                throw new Exception("@ Entity error process events occur during acquisition parameters , Make sure the field (" + key + ") Is spelled correctly , Technical Information :" + ex.Message);
            }
        }
        /// <summary>
        ///  Gets a string parameter 
        /// </summary>
        /// <param name="key">key</param>
        /// <returns> If it is Nul, It does not exist or throw an exception </returns>
        public string GetValStr(string key)
        {
            try
            {
                return this.SysPara.GetValByKey(key).ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("@ Entity error process events occur during acquisition parameters , Make sure the field (" + key + ") Is spelled correctly , Technical Information :" + ex.Message);
            }
        }
        /// <summary>
        ///  Get Int64 Numerical 
        /// </summary>
        /// <param name="key"> Key </param>
        /// <returns> If it is Nul, It does not exist or throw an exception </returns>
        public Int64 GetValInt64(string key)
        {
            return Int64.Parse(this.GetValStr(key));
        }
        /// <summary>
        ///  Get int Numerical 
        /// </summary>
        /// <param name="key"> Key </param>
        /// <returns> If it is Nul, It does not exist or throw an exception </returns>
        public int GetValInt(string key)
        {
            return int.Parse(this.GetValStr(key));
        }
        public bool GetValBoolen(string key)
        {
            if (int.Parse(this.GetValStr(key)) == 0)
                return false;
            return true;
        }
        /// <summary>
        ///  Get decimal Numerical 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public decimal GetValDecimal(string key)
        {
            return decimal.Parse(this.GetValStr(key));
        }
        #endregion  Get parametric method 

        #region  Constructor 
        /// <summary>
        ///  Process Event base class 
        /// </summary>
        public FlowEventBase()
        {
        }
        #endregion  Constructor 

        #region  Require mandatory subclasses override property .
        /// <summary>
        ///  Process ID 
        /// </summary>
        abstract public string FlowMark
        {
            get;
        }
        #endregion  Require subclasses override properties .

        #region  Nodes form events 
        public virtual string FrmLoadAfter()
        {
            return null;
        }
        public virtual string FrmLoadBefore()
        {
            return null;
        }
        #endregion

        #region  Require subclasses override methods ( Process events ).
        /// <summary>
        ///  Process before completion 
        /// </summary>
        public virtual string FlowOverBefore()
        {
            return null;
        }
        /// <summary>
        ///  After 
        /// </summary>
        public virtual string FlowOverAfter()
        {
            return null;
        }
        /// <summary>
        /// Process before deleting 
        /// </summary>
        public virtual string BeforeFlowDel()
        {
            return null;
        }
        /// <summary>
        ///  Process deleted 
        /// </summary>
        public virtual string AfterFlowDel()
        {
            return null;
        }
        #endregion  Require subclasses override methods ( Process events ).


        #region  Require subclasses override methods ( Node event ).
        /// <summary>
        ///  Saved 
        /// </summary>
        public virtual string SaveAfter()
        {
            return null;
        }
        /// <summary>
        ///  Save ago 
        /// </summary>
        public virtual string SaveBefore()
        {
            return null;
        }
        /// <summary>
        /// Send ago 
        /// </summary>
        public virtual string SendWhen()
        {
            return null;
        }
        /// <summary>
        ///  When sent successfully 
        /// </summary>
        public virtual string SendSuccess()
        {
            return null;
        }
        /// <summary>
        ///  Failed to send 
        /// </summary>
        /// <returns></returns>
        public virtual string SendError() { return null; }
        public virtual string ReturnBefore() { return null; }
        public virtual string ReturnAfter() { return null; }
        public virtual string UndoneBefore() { return null; }
        public virtual string UndoneAfter() { return null; }
        /// <summary>
        ///  After transfer 
        /// </summary>
        /// <returns></returns>
        public virtual string ShiftAfter()
        {
            return null;
        }
        /// <summary>
        ///  Plus sign after 
        /// </summary>
        /// <returns></returns>
        public virtual string AskerAfter()
        {
            return null;
        }
        /// <summary>
        ///  After endorsement reply 
        /// </summary>
        /// <returns></returns>
        public virtual string AskerReAfter()
        {
            return null;
        }
        #endregion  Require subclasses override methods ( Node event ).


        #region  Base class methods .
        /// <summary>
        ///  Execution events 
        /// </summary>
        /// <param name="eventType"> Event Type </param>
        /// <param name="en"> Entity Parameters </param>
        public string DoIt(string eventType, Node currNode, Entity en, string atPara)
        {
            this.HisEn = en;
            this.HisNode = currNode;

            #region  Processing parameters .
            Row r = en.Row;
            try
            {
                // System parameters .
                r.Add("FK_MapData", en.ClassID);
            }
            catch
            {
                r["FK_MapData"] = en.ClassID;
            }

            if (atPara != null)
            {
                AtPara ap = new AtPara(atPara);
                foreach (string s in ap.HisHT.Keys)
                {
                    try
                    {
                        r.Add(s, ap.GetValStrByKey(s));
                    }
                    catch
                    {
                        r[s] = ap.GetValStrByKey(s);
                    }
                }
            }

            if (SystemConfig.IsBSsystem == true)
            {
                /* In the case of bs System ,  Join the external url Variables .*/
                foreach (string key in BP.Sys.Glo.Request.QueryString)
                {
                    string val = BP.Sys.Glo.Request.QueryString[key];
                    try
                    {
                        r.Add(key, val);
                    }
                    catch
                    {
                        r[key] = val;
                    }
                }
            }
            this.SysPara = r;
            #endregion  Processing parameters .

            #region  Execution events .
            switch (eventType)
            {
                case EventListOfNode.FrmLoadAfter: //  Nodes form events .
                    return this.FrmLoadAfter();
                case EventListOfNode.FrmLoadBefore: //  Nodes form events .
                    return this.FrmLoadBefore();

                case EventListOfNode.SaveAfter: //  Node event   Saved .
                    return this.SaveAfter();
                case EventListOfNode.SaveBefore: //  Node event  -  Save ago ..
                    return this.SaveBefore();
                case EventListOfNode.SendWhen: //  Node event  -  Send ago .
                    return this.SendWhen();
                case EventListOfNode.SendSuccess: //  Node event  -  When sent successfully .
                    return this.SendSuccess();
                case EventListOfNode.SendError: //  Node event  -  Failed to send .
                    return this.SendError();
                case EventListOfNode.ReturnBefore: //  Node event  -  Return ago .
                    return this.ReturnBefore();
                case EventListOfNode.ReturnAfter: //  Node event  -  After returning .
                    return this.ReturnAfter();
                case EventListOfNode.UndoneBefore: //  Node event  -  Revocation ago .
                    return this.UndoneBefore();
                case EventListOfNode.UndoneAfter: //  Node event  -  Revocation .
                    return this.UndoneAfter();
                case EventListOfNode.ShitAfter://  Node event - After transfer 
                    return this.ShiftAfter();
                case EventListOfNode.AskerAfter:// Node event   Plus sign after 
                    return this.AskerAfter();
                case EventListOfNode.AskerReAfter:// After the incident node endorsement Reply 
                    return this.AskerReAfter();
                case EventListOfNode.FlowOverBefore: //  Process events  -------------------------------------------.
                    return this.FlowOverBefore();
                case EventListOfNode.FlowOverAfter: //  Process events .
                    return this.FlowOverAfter();
                case EventListOfNode.BeforeFlowDel: //  Process events .
                    return this.BeforeFlowDel();
                case EventListOfNode.AfterFlowDel: //  Process events .
                    return this.AfterFlowDel();
                default:
                    throw new Exception("@ Event types are not judged :" + eventType);
                    break;
            }
            #endregion  Execution events .
            return null;
        }
        #endregion  Base class methods .
    }
}
