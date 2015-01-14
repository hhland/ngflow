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
    public class EleFunc
    {
        #region attrs
        public const string SelectAll = "SelectAll";
        public const string CopyEle = "CopyEle";
        public const string Paste = "Paste";
        public const string Bold = "Bold";
        public const string Italic = "Italic";
        public const string Strike = "Strike";
        public const string FontSizeAdd = "FontSizeAdd";
        public const string FontSizeCut = "FontSizeCut";
        public const string Colorpicker = "Colorpicker";
        #endregion

        #region  Field 
        string _No;
        string _Name;
        BitmapImage _Img;
        string _Tooltip;
        #endregion

        #region  Property 
        /// <summary>
        ///  Icon Name 
        /// </summary>
        public string No
        {
            get { return _No; }
            set { _No = value; }
        }
        /// <summary>
        ///  Icon image 
        /// </summary>
        public BitmapImage Img
        {
            get { return _Img; }
            set { _Img = value; }
        }
        /// <summary>
        ///  Icon Text 
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        /// <summary>
        ///  Prompt 
        /// </summary>
        public string Tooltip
        {
            get { return _Tooltip; }
            set { _Tooltip = value; }
        }
        #endregion

        #region  Single Instance 
        public static readonly EleFunc instance = new EleFunc();
        #endregion

        #region  Public Methods 
        public List<EleFunc> getToolList()
        {
            List<EleFunc> ToolList = new List<EleFunc>()
            {
                //new EleFunc(){ No=EleFunc.SelectAll,  Name=" Select ", Tooltip=" Select all of the elements , You can bulk delete , Mobile and other operations ,  You can also ctrl+ Multi-mode control selection .",
                  //  Img=new BitmapImage(new Uri("/CCFormDesigner;component/Img/"+EleFunc.SelectAll+".png",UriKind.RelativeOrAbsolute))},
             //   new EleFunc(){ No=EleFunc.CopyEle, Tooltip=" Copy the element has been selected ", Name=" Copy ",Img=new BitmapImage(new Uri("/CCFormDesigner;component/Img/"+EleFunc.CopyEle+".png",UriKind.RelativeOrAbsolute))},
              //  new EleFunc(){ No=EleFunc.Paste, Tooltip="", Name=" Paste ",Img=new BitmapImage(new Uri("/CCFormDesigner;component/Img/"+EleFunc.Paste+".png",UriKind.RelativeOrAbsolute))},

                new EleFunc(){ No=Func.Alignment_Left,Name=" Left ", Tooltip=" Can be performed on one or more selected Chak controls ", Img=new BitmapImage(new Uri("/CCFormDesigner;component/Img/"+Func.Alignment_Left+".png",UriKind.RelativeOrAbsolute))},
                new EleFunc(){ No=Func.Alignment_Center,  Name=" Center ",Tooltip=" Can be performed on one or more selected Chak controls ", Img=new BitmapImage(new Uri("/CCFormDesigner;component/Img/"+Func.Alignment_Center+".png",UriKind.RelativeOrAbsolute))},
                new EleFunc(){ No=Func.Alignment_Right,Name=" Align Right ", Tooltip=" Can be performed on one or more selected Chak controls ", Img=new BitmapImage(new Uri("/CCFormDesigner;component/Img/"+Func.Alignment_Right+".png",UriKind.RelativeOrAbsolute))},
                new EleFunc(){ No=Func.Alignment_Top, Name=" Align the top ",Tooltip=" Can be performed on one or more selected Chak controls ", Img=new BitmapImage(new Uri("/CCFormDesigner;component/Img/"+Func.Alignment_Top+".png",UriKind.RelativeOrAbsolute))},
                new EleFunc(){ No=Func.Alignment_Down, Name=" Align bottom ",Tooltip=" Can be performed on one or more selected Chak controls ", Img=new BitmapImage(new Uri("/CCFormDesigner;component/Img/"+Func.Alignment_Down+".png",UriKind.RelativeOrAbsolute))},
                new EleFunc(){ No=Func.Delete,  Name=" Delete ",Tooltip=" Delete one or more elements selected ", Img=new BitmapImage(new Uri("/CCFormDesigner;component/Img/"+Func.Delete+".png",UriKind.RelativeOrAbsolute))},
                //new EleFunc(){ No=Func.Undo,  Name=" Revocation ",Tooltip=" This feature is not yet implemented ", Img=new BitmapImage(new Uri("/CCFormDesigner;component/Img/"+Func.Undo+".png",UriKind.RelativeOrAbsolute))},
                //new EleFunc(){ No=Func.ForwardDo,  Name=" Recovery ",Tooltip=" This feature is not yet implemented ",Img=new BitmapImage(new Uri("/CCFormDesigner;component/Img/"+Func.ForwardDo+".png",UriKind.RelativeOrAbsolute))},
                
                new EleFunc(){ No=EleFunc.Bold, Name=" Bold face ",Tooltip=" For one or more elements selected valid tag ", Img=new BitmapImage(new Uri("/CCFormDesigner;component/Img/"+EleFunc.Bold+".png",UriKind.RelativeOrAbsolute))},
                //new EleFunc(){ No=EleFunc.Italic, Name=" Italics ",Tooltip=" For one or more elements selected valid tag ",  Img=new BitmapImage(new Uri("/CCFormDesigner;component/Img/"+EleFunc.Italic+".png",UriKind.RelativeOrAbsolute))},
                //new EleFunc(){ No=EleFunc.Strike,  Name=" Delete line ",Tooltip=" For one or more elements selected valid tag ", Img=new BitmapImage(new Uri("/CCFormDesigner;component/Img/"+EleFunc.Strike+".png",UriKind.RelativeOrAbsolute))},
                new EleFunc(){ No=EleFunc.FontSizeAdd,  Name=" Increases ",Tooltip=" For a selected one or more tags , Line elements effectively ",  Img=new BitmapImage(new Uri("/CCFormDesigner;component/Img/"+EleFunc.FontSizeAdd+".png",UriKind.RelativeOrAbsolute))},
                new EleFunc(){ No=EleFunc.FontSizeCut,  Name=" Decreases ",Tooltip=" For a selected one or more tags , Line elements effectively ", Img=new BitmapImage(new Uri("/CCFormDesigner;component/Img/"+EleFunc.FontSizeCut+".png",UriKind.RelativeOrAbsolute))},
                new EleFunc(){ No=EleFunc.Colorpicker,  Name=" Color ",Tooltip=" For a selected one or more tags , Line elements effectively ", Img=new BitmapImage(new Uri("/CCFormDesigner;component/Img/"+EleFunc.Colorpicker+".png",UriKind.RelativeOrAbsolute))}
            };
            return ToolList;
        }
        #endregion
    }
}
