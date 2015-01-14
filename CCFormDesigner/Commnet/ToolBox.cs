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
    public class ToolBox
    {
        #region attrs
        ///// <summary>
        /////  Hidden Fields 
        ///// </summary>
        //public const string HiddenField = "HiddenField";
        public const string Mouse = "Mouse";
        public const string Selected = "Selected";
        public const string Line = "Line";
        /// <summary>
        ///  Image Attachment 
        /// </summary>
        public const string ImgAth = "ImgAth";
        /// <summary>
        ///  Label 
        /// </summary>
        public const string Label = "Label";
        /// <summary>
        ///  Connection 
        /// </summary>
        public const string Link = "Link";
        /// <summary>
        ///  Textbox 
        /// </summary>
        public const string TextBox = "TextBox";
        /// <summary>
        /// DateCtl
        /// </summary>
        public const string DateCtl = "DateCtl";
        /// <summary>
        ///  Drop-down box 
        /// </summary>
        public const string DDLEnum = "DDLEnum";
        /// <summary>
        ///  Data sheet 
        /// </summary>
        public const string DDLTable = "DDLTable";
        /// <summary>
        ///  Radio buttons 
        /// </summary>
        public const string RBS = "RBS";
        /// <summary>
        ///  Selection box 
        /// </summary>
        public const string CheckBox = "CheckBox";
        /// <summary>
        ///  Picture 
        /// </summary>
        public const string Img = "Img";
        /// <summary>
        ///  Signature Pictures 
        /// </summary>
        public const string SealImg = "SealImg";

        public const string Dtl = "Dtl";
        public const string M2M = "M2M";
        public const string M2MM = "M2MM";
        /// <summary>
        ///  Audit Components 
        /// </summary>
        public const string WorkCheck = "WorkCheck";

        public const string FrmEle = "FrmEle";
        public const string Btn = "Btn";
        public const string Attachment = "Attachment";
        /// <summary>
        ///  Multiple file upload control 
        /// </summary>
        public const string AttachmentM = "AttachmentM";
        #endregion

        #region  Field 
        string icoName;
        string icoNameText;
        BitmapImage icoImage;
        #endregion

        #region  Property 
        /// <summary>
        ///  Icon Name 
        /// </summary>
        public string IcoName
        {
            get { return icoName; }
            set { icoName = value; }
        }
        /// <summary>
        ///  Icon image 
        /// </summary>
        public BitmapImage IcoImage
        {
            get { return icoImage; }
            set { icoImage = value; }
        }
        /// <summary>
        ///  Icon Text 
        /// </summary>
        public string IcoNameText
        {
            get { return icoNameText; }
            set { icoNameText = value; }
        }
        #endregion

      
    }

    public class ToolBoxes
    {
        #region  Single Instance 
        public static readonly ToolBoxes instance = new ToolBoxes();
        #endregion

        public List<ToolBox> GetToolBoxList()
        {
            List<ToolBox> ToolBoxList = new List<ToolBox>()
            {
                new ToolBox(){ IcoName=ToolBox.Mouse, IcoNameText=" Mouse ", IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/Mouse.png",UriKind.RelativeOrAbsolute))},
                new ToolBox(){ IcoName=ToolBox.Selected,  IcoNameText=" Choose ",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/Selected.png",UriKind.RelativeOrAbsolute))},
                new ToolBox(){ IcoName=ToolBox.Line,  IcoNameText=" Draw the line ",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/Line.png",UriKind.RelativeOrAbsolute))},
                new ToolBox(){ IcoName=ToolBox.Label,  IcoNameText=" Label ",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/Label.png",UriKind.RelativeOrAbsolute))},
                new ToolBox(){ IcoName=ToolBox.Link,  IcoNameText=" Hyperlinks ",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/Link.png",UriKind.RelativeOrAbsolute))},
                new ToolBox(){ IcoName=ToolBox.Btn, IcoNameText=" Push button ",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/Btn.png",UriKind.RelativeOrAbsolute))},

                new ToolBox(){ IcoName=ToolBox.Img, IcoNameText=" Decorative Picture ",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/Img.png",UriKind.RelativeOrAbsolute))},
                new ToolBox(){ IcoName=ToolBox.SealImg, IcoNameText=" Signature ",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/seal.png",UriKind.RelativeOrAbsolute))},

                new ToolBox(){ IcoName=ToolBox.TextBox,IcoNameText=" Textbox ", IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/TextBox.png",UriKind.RelativeOrAbsolute))},
                new ToolBox(){ IcoName=ToolBox.DateCtl,IcoNameText=" Date / Time ", IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/Calendar.png",UriKind.RelativeOrAbsolute))},
                new ToolBox(){ IcoName=ToolBox.CheckBox,IcoNameText=" Selection box ", IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/CheckBox.png",UriKind.RelativeOrAbsolute))},
                new ToolBox(){ IcoName=ToolBox.DDLEnum, IcoNameText=" Drop-down box ( Enumerate )",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/DDLEnum.png",UriKind.RelativeOrAbsolute))},
                new ToolBox(){ IcoName=ToolBox.DDLTable, IcoNameText=" Drop-down box (表/ View )",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/DDLEnum.png",UriKind.RelativeOrAbsolute))},
                new ToolBox(){ IcoName=ToolBox.RBS,  IcoNameText=" Radio buttons ",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/RB.png",UriKind.RelativeOrAbsolute))},
                new ToolBox(){ IcoName=ToolBox.FrmEle, IcoNameText=" Extended Controls ",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/FrmEle.png",UriKind.RelativeOrAbsolute))},
                new ToolBox(){ IcoName=ToolBox.Attachment, IcoNameText=" Single attachment ",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/Attachment.png",UriKind.RelativeOrAbsolute))},
                new ToolBox(){ IcoName=ToolBox.AttachmentM, IcoNameText=" More Accessories ",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/AttachmentM.png",UriKind.RelativeOrAbsolute))},
                new ToolBox(){ IcoName=ToolBox.ImgAth, IcoNameText=" Image Attachment ",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/ImgAth.png",UriKind.RelativeOrAbsolute))},
                new ToolBox(){ IcoName=ToolBox.Dtl, IcoNameText=" List ( From Table )",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/Dtl.png",UriKind.RelativeOrAbsolute))},
                new ToolBox(){ IcoName=ToolBox.M2M, IcoNameText=" Many ",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/M2M.png",UriKind.RelativeOrAbsolute))},
                new ToolBox(){ IcoName=ToolBox.M2MM, IcoNameText=" Many more ",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/M2M.png",UriKind.RelativeOrAbsolute))},
                new ToolBox(){ IcoName=ToolBox.WorkCheck, IcoNameText=" Audit Components ",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/M2M.png",UriKind.RelativeOrAbsolute))}
                //, new ToolBox(){ IcoName=ToolBox.HiddenField, IcoNameText=" Hidden Fields ",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/HiddenField.png",UriKind.RelativeOrAbsolute))}
            };
            return ToolBoxList;
        }
      
    }
}
