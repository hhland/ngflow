using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace CCForm
{
    public class BtnFlow
    {

        #region attrs
        public const string Send = "Send";
        public const string Save = "Save";
        public const string Return = "Return";
        public const string CC = "CC";
        public const string Shift = "Shift";
        public const string Del = "Del";
        public const string EndFlow = "EndFlow";
        public const string Rpt = "Rpt";
        public const string Ath = "Ath";
        public const string Track = "Track";
        public const string Opt = "Opt";
        #endregion

        #region  Property 
        private string _ID = null;
        private string _Lab = null;
        #endregion  Property 


        #region  Property 
        /// <summary>
        ///  Icon Name 
        /// </summary>
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        /// <summary>
        ///  Icon Text 
        /// </summary>
        public string Lab
        {
            get { return _Lab; }
            set { _Lab = value; }
        }
        #endregion

        #region  Single Instance 
        public static readonly BtnFlow instance = new BtnFlow();
        #endregion

        #region  Public Methods 

        public List<BtnFlow> getBtnFlowList()
        {
            List<BtnFlow> BtnFlowList = new List<BtnFlow>()
            {
                new BtnFlow(){ ID=BtnFlow.Send, Lab=" Send " },
                new BtnFlow(){ ID=BtnFlow.Save, Lab=" Save " },
                new BtnFlow(){ ID=BtnFlow.Return, Lab=" Return " },
                new BtnFlow(){ ID=BtnFlow.Shift, Lab=" Transfer " },
                new BtnFlow(){ ID=BtnFlow.Del, Lab=" Mouse " },
                new BtnFlow(){ ID=BtnFlow.EndFlow, Lab=" Mouse " },
                new BtnFlow(){ ID=BtnFlow.Ath, Lab=" Accessory " },
                new BtnFlow(){ ID=BtnFlow.Track, Lab=" Locus " },
                new BtnFlow(){ ID=BtnFlow.Opt, Lab=" Options " }
            };
            return BtnFlowList;
        }
        #endregion
    }
}
