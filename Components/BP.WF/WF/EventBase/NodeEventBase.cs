using System;
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
    ///  Node event base class 
    /// </summary>
    abstract public class NodeEventBase
    {
        #region  Property .
        public Node HisNode = null;
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
            string str = this.GetValStr(key).ToString();
            return DataType.ParseSysDateTime2DateTime(str);
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
        public decimal GetValDecimal(string key)
        {
            return decimal.Parse(this.GetValStr(key));
        }
        #endregion  Get parametric method 

        #region  Constructor 
        /// <summary>
        ///  Event base class 
        /// </summary>
        public NodeEventBase()
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
        /// <summary>
        ///  Node coding s,  There can be multiple node number .
        ///  Encoding requires multiple nodes separated by commas .
        /// </summary>
        abstract public string NodeMarks
        {
            get;
        }
        #endregion  Require subclasses override properties .

        #region  Require subclasses override methods ( Form events ).
        /// <summary>
        ///  Form before loading 
        /// </summary>
        abstract public string FrmLoadBefore();
        /// <summary>
        ///  After loading the form 
        /// </summary>
        abstract public string FrmLoadAfter();
        /// <summary>
        ///  Form before saving 
        /// </summary>
        abstract public string SaveBefore();
        /// <summary>
        ///  Forms saved 
        /// </summary>
        abstract public string SaveAfter();
        #endregion  Require subclasses override methods ( Form events ).

        #region  Require subclasses override methods ( Node event ).
        /// <summary>
        ///  Node before sending 
        /// </summary>
        abstract public string SendWhen();
        /// <summary>
        ///  After the node sent successfully 
        /// </summary>
        abstract public string SendSuccess();
        /// <summary>
        ///  After sending node failure 
        /// </summary>
        abstract public string SendError();
        /// <summary>
        ///  When a node before returning 
        /// </summary>
        abstract public string ReturnBefore();
        /// <summary>
        ///  When the node back 
        /// </summary>
        abstract public string ReturnAfter();
        /// <summary>
        ///  When withdrawn before sending node 
        /// </summary>
        abstract public string UnSendBefore();
        /// <summary>
        ///  When sending node revocation 
        /// </summary>
        abstract public string UnSendAfter();
        #endregion  Require subclasses override methods ( Node event ).

        #region  Base class methods .
        /// <summary>
        ///  Execution events 
        /// </summary>
        /// <param name="eventType"> Event Type </param>
        /// <param name="en"> Entity Parameters </param>
        public string DoIt(string eventType, Node currNode, Entity en, string atPara)
        {
            // His node .
            this.HisNode = currNode;
            this.HisEn = en;

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
                foreach (string key in System.Web.HttpContext.Current.Request.QueryString)
                {
                    string val = System.Web.HttpContext.Current.Request.QueryString[key];
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
                case EventListOfNode.SendWhen:
                    return this.SendWhen();
                case EventListOfNode.SendSuccess:
                    return this.SendSuccess();
                case EventListOfNode.SendError:
                    return this.SendError();
                case EventListOfNode.ReturnBefore:
                    return this.ReturnBefore();
                case EventListOfNode.ReturnAfter:
                    return this.ReturnAfter();
                case EventListOfNode.UndoneBefore:
                    return this.UnSendBefore();
                case EventListOfNode.UndoneAfter:
                    return this.UnSendAfter();
                case EventListOfNode.SaveBefore:
                    return this.SaveBefore();
                case EventListOfNode.SaveAfter:
                    return this.SaveAfter();
                case EventListOfNode.FrmLoadBefore:
                    return this.FrmLoadBefore();
                case EventListOfNode.FrmLoadAfter:
                    return this.FrmLoadAfter();
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
