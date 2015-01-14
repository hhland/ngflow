using System; 
using System.Collections;
using BP.DA; 
using BP.Web.Controls;
using System.Reflection;

namespace BP.En
{
    /// <summary>
    ///  Related functions Types 
    /// </summary>
    public enum RefMethodType
    {
        /// <summary>
        ///  Function 
        /// </summary>
        Func,
        /// <summary>
        ///  Modal window opens 
        /// </summary>
        LinkModel,
        /// <summary>
        ///  Open a new window 
        /// </summary>
        LinkeWinOpen,
        /// <summary>
        ///  Right window open 
        /// </summary>
        RightFrameOpen
    }
    /// <summary>
    /// RefMethod  The summary .
    /// </summary>
    public class RefMethod
    {
        #region  Methods and window-related .
        /// <summary>
        ///  Height 
        /// </summary>
        public int Height = 600;
        /// <summary>
        ///  Width 
        /// </summary>
        public int Width = 800;
        public string Target = "_B123";
        #endregion

        /// <summary>
        ///  Function 
        /// </summary>
        public RefMethodType RefMethodType = RefMethodType.Func;
        /// <summary>
        ///  Related Fields 
        /// </summary>
        public string RefAttrKey = null;
        /// <summary>
        ///  Connections tab 
        /// </summary>
        public string RefAttrLinkLabel = null;

        /// <summary>
        ///  Is displayed in Ens÷–?
        /// </summary>
        public bool IsForEns = true;
        /// <summary>
        ///  Related functions 
        /// </summary>
        public RefMethod()
        {
        }
        /// <summary>
        ///  Parameters 
        /// </summary>
        private Attrs _HisAttrs = null;
        /// <summary>
        ///  Parameters 
        /// </summary>
        public Attrs HisAttrs
        {
            get
            {
                if (_HisAttrs == null)
                    _HisAttrs = new Attrs();
                return _HisAttrs;
            }
            set
            {
                _HisAttrs = value;
            }
        }
        /// <summary>
        ///  The index position , Use it to distinguish between entities .
        /// </summary>
        public int Index = 0;
        /// <summary>
        ///  Whether to display 
        /// </summary>
        public bool Visable = true;
        /// <summary>
        ///  Can batch 
        /// </summary>
        public bool IsCanBatch = false;
        /// <summary>
        ///  Title 
        /// </summary>
        public string Title = null;
        /// <summary>
        ///  Preflight message 
        /// </summary>
        public string Warning = null;
        /// <summary>
        ///  Connection 
        /// </summary>
        public string ClassMethodName = null;
        /// <summary>
        ///  Icon 
        /// </summary>
        public string Icon = null;
        public string GetIcon(string path)
        {
            if (this.Icon == null)
            {
                return null;
                return "<img src='/WF/Img/Btn/Do.gif'  border=0 />";
            }
            else
            {
                string url = path + Icon;
                url = url.Replace("//", "/");
                return "<img src='" + url + "'  border=0 />";
            }
        }
        /// <summary>
        ///  Message 
        /// </summary>
        public string ToolTip = null;
       
        /// <summary>
        /// PKVal
        /// </summary>
        public object PKVal = "PKVal";
        /// <summary>
        /// 
        /// </summary>
        public Entity HisEn = null;
        /// <summary>
        ///  Entity PK
        /// </summary>
        public string[] PKs = "".Split('.');
        /// <summary>
        ///  Carried out 
        /// </summary>
        /// <param name="paras"></param>
        /// <returns></returns>
        public object Do(object[] paras)
        {
            string str = this.ClassMethodName.Trim(' ', ';', '.');
            int pos = str.LastIndexOf(".");
            string clas = str.Substring(0, pos);
            string meth = str.Substring(pos, str.Length - pos).Trim('.', ' ', '(', ')');
            if (this.HisEn == null)
            {
                this.HisEn = BP.En.ClassFactory.GetEn(clas);
                Attrs attrs = this.HisEn.EnMap.Attrs;

                //if (SystemConfig.IsBSsystem)
                //{
                //    //string val = BP.Sys.Glo.Request.QueryString["No"];
                //    //if (val == null)
                //    //{
                //    //    val = BP.Sys.Glo.Request.QueryString["PK"];
                //    //}
                //    this.HisEn.PKVal = BP.Sys.Glo.Request.QueryString[this.HisEn.PK];
                //}
                //else
                //    this.HisEn.PKVal = this.PKVal;
                //this.HisEn.Retrieve();
            }

            Type tp = this.HisEn.GetType();
            MethodInfo mp = tp.GetMethod(meth);
            if (mp == null)
                throw new Exception("@ Object instance [" + tp.FullName + "] Does not find a way [" + meth + "]!");

            try
            {
                return mp.Invoke(this.HisEn, paras); // Thus calling  MethodInfo  Examples of reflection method or constructor .
            }
            catch (System.Reflection.TargetException ex)
            {
                string strs = "";
                if (paras == null)
                {
                    throw new Exception(ex.Message);
                }
                else
                {
                    foreach (object obj in paras)
                    {
                        strs += "para= " + obj.ToString() + " type=" + obj.GetType().ToString() + "\n<br>";
                    }
                }
                throw new Exception(ex.Message + "  more info:" + strs);
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class RefMethods : CollectionBase
    {
        /// <summary>
        ///  Join 
        /// </summary>
        /// <param name="attr">attr</param>
        public void Add(RefMethod en)
        {
            if (this.IsExits(en))
                return;
            en.Index = this.InnerList.Count;
            this.InnerList.Add(en);
        }
        /// <summary>
        ///  Is there a collection of inside 
        /// </summary>
        /// <param name="en"> To check the RefMethod</param>
        /// <returns>true/false</returns>
        public bool IsExits(RefMethod en)
        {
            foreach (RefMethod dtl in this)
            {
                if (dtl.ClassMethodName == en.ClassMethodName)
                    return true;
            }
            return false;
        }
        /// <summary>
        ///  Attributes can be seen 
        /// </summary>
        public int CountOfVisable
        {
            get
            {
                int i = 0;
                foreach (RefMethod rm in this)
                {
                    if (rm.Visable)
                        i++;
                }
                return i;
            }
        }
        /// <summary>
        ///  According to the index to access elements within the collection Attr.
        /// </summary>
        public RefMethod this[int index]
        {
            get
            {
                return (RefMethod)this.InnerList[index];
            }
        }
    }
}
