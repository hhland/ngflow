using System; 
using System.Collections;
using BP.DA; 
using BP.Web.Controls;
using System.Reflection;
using BP.En;
namespace BP.En
{
    public enum MsgShowType
    {
        /// <summary>
        ///  The interface 
        /// </summary>
        SelfAlert,
        /// <summary>
        ///  Prompt box 
        /// </summary>
        SelfMsgWindows,
        /// <summary>
        ///  New Window 
        /// </summary>
        Blank
    }
	/// <summary>
	/// Method  The summary 
	/// </summary>
    abstract public class Method
    {
        /// <summary>
        ///  Information Display Type 
        /// </summary>
        public MsgShowType HisMsgShowType = MsgShowType.Blank;

        #region Http
        public string Request(string key)
        {
            return BP.Sys.Glo.Request.QueryString[key];
        }
        /// <summary>
        ///  Get MyPK
        /// </summary>
        public string RequestRefMyPK
        {
            get
            {
                string s = Request("RefMyPK");
                if (s == null)
                    s = Request("RefPK");

                return s;
            }
        }
        public string RequestRefNo
        {
            get
            {
                return Request("RefNo");
            }
        }
        public int RequestRefOID
        {
            get
            {
                return int.Parse(Request("RefOID"));
            }
        }
        #endregion Http

        #region ROW
        /// <summary>
        ///  Get Key值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns> The results </returns>
        public object GetValByKey(string key)
        {
            return this.Row.GetValByKey(key);
        }
        /// <summary>
        ///  Get str值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns> The results </returns>
        public string GetValStrByKey(string key)
        {
            return this.GetValByKey(key).ToString();
        }
        /// <summary>
        ///  Get int值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns> The results </returns>
        public int GetValIntByKey(string key)
        {
            return (int)this.GetValByKey(key);
        }

        /// <summary>
        ///  Get decimal值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns> The results </returns>
        public decimal GetValDecimalByKey(string key)
        {
            return (decimal)this.GetValByKey(key);
        }
        /// <summary>
        ///  Get bool值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns> The results </returns>
        public bool GetValBoolByKey(string key)
        {
            if (this.GetValIntByKey(key) == 1)
                return true;
            return false;
        }
        public void SetValByKey(string attrKey, int val)
        {
            this.Row.SetValByKey(attrKey, val);
        }
        public void SetValByKey(string attrKey, Int64 val)
        {
            this.Row.SetValByKey(attrKey, val);
        }
        public void SetValByKey(string attrKey, float val)
        {
            this.Row.SetValByKey(attrKey, val);
        }
        public void SetValByKey(string attrKey, decimal val)
        {
            this.Row.SetValByKey(attrKey, val);
        }
        public void SetValByKey(string attrKey, object val)
        {
            this.Row.SetValByKey(attrKey, val);
        }
        /// <summary>
        ///  Entity  map  Information .	
        /// </summary>		
        //public abstract void EnMap();		
        private Row _row = null;
        public Row Row
        {
            get
            {
                if (this.HisAttrs == null)
                    return null;

                if (this._row == null)
                {
                    this._row = new Row();
                    this._row.LoadAttrs(this.HisAttrs);
                }

                return this._row;
            }
            set
            {
                this._row = value;
            }
        }
        #endregion

        /// <summary>
        ///  The method of the base class 
        /// </summary>
        public Method()
        {

        }

        #region  Property 
        /// <summary>
        ///  Parameters 
        /// </summary>
        private Attrs _HisAttrs = null;
        public Attrs HisAttrs
        {
            get
            {
                if (_HisAttrs == null)
                    _HisAttrs = new Attrs();
                return _HisAttrs;
            }
        }
        /// <summary>
        ///  Title 
        /// </summary>
        public string Title = null;
        public string Help = null;

        /// <summary>
        ///  Preflight message 
        /// </summary>
        public string Warning = null;
        /// <summary>
        ///  Icon 
        /// </summary>
        public string Icon = null;
        public string GetIcon(string path)
        {
            if (this.Icon == null)
            {
                return "<img src='/WF/Img/Btn/Do.gif'  border=0 />";
            }
            else
            {
                return Icon;
                //return "<img src='" + path + Icon + "'  border=0 />";
            }
        }
        /// <summary>
        ///  Message 
        /// </summary>
        public string ToolTip = null;
        /// <summary>
        ///  The goal 
        /// </summary>
        public string Target = "OpenWin";
        /// <summary>
        ///  Height 
        /// </summary>
        public int Height = 600;
        /// <summary>
        ///  Width 
        /// </summary>
        public int Width = 800;
        /// <summary>
        ///  Carried out 
        /// </summary>
        /// <param name="paras"></param>
        /// <returns></returns>
        public abstract object Do();
        public abstract void Init();
        /// <summary>
        ///  Rights Management 
        /// </summary>
        public abstract bool IsCanDo
        {
            get;
        }
        /// <summary>
        ///  Appears in the function list 
        /// </summary>
        public bool IsVisable = true;
        #endregion
    }
}
