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
    public class AdvTool
    {

        #region attrs
        /// <summary>
        ///  Hidden Fields 
        /// </summary>
        public const string HiddenField = "HiddenField";
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
        public const string Dtl = "Dtl";
        public const string M2M = "M2M";

        public const string Attachment = "Attachment";
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

        #region  Single Instance 

        public static readonly AdvTool instance = new AdvTool();

        #endregion

        #region  Public Methods 

        public List<AdvTool> getAdvToolList()
        {
            List<AdvTool> AdvToolList = new List<AdvTool>()
            {
                new AdvTool(){ IcoName=AdvTool.Mouse, IcoNameText=" Form events ", IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/Mouse.png",UriKind.RelativeOrAbsolute))},
                new AdvTool(){ IcoName=AdvTool.Selected,  IcoNameText=" Form events ",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/Selected.png",UriKind.RelativeOrAbsolute))},
                new AdvTool(){ IcoName=AdvTool.Label,  IcoNameText=" Access Control ",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/Label.png",UriKind.RelativeOrAbsolute))},
                new AdvTool(){ IcoName=AdvTool.Line,  IcoNameText=" Draw the line ",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/Line.png",UriKind.RelativeOrAbsolute))},
                new AdvTool(){ IcoName=AdvTool.Link,  IcoNameText=" Hyperlinks ",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/Link.png",UriKind.RelativeOrAbsolute))},
                new AdvTool(){ IcoName=AdvTool.Img, IcoNameText=" Decorative Picture ",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/Img.png",UriKind.RelativeOrAbsolute))},

                new AdvTool(){ IcoName=AdvTool.TextBox,IcoNameText=" Textbox ", IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/TextBox.png",UriKind.RelativeOrAbsolute))},
                new AdvTool(){ IcoName=AdvTool.DateCtl,IcoNameText=" Date / Time ", IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/calendar.png",UriKind.RelativeOrAbsolute))},
                new AdvTool(){ IcoName=AdvTool.CheckBox,IcoNameText=" Selection box ", IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/CheckBox.png",UriKind.RelativeOrAbsolute))},
                new AdvTool(){ IcoName=AdvTool.DDLEnum, IcoNameText=" Drop-down box ( Enumerate )",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/DDLEnum.png",UriKind.RelativeOrAbsolute))},
                new AdvTool(){ IcoName=AdvTool.DDLTable, IcoNameText=" Drop-down box (表/ View )",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/DDLEnum.png",UriKind.RelativeOrAbsolute))},
                new AdvTool(){ IcoName=AdvTool.RBS,  IcoNameText=" Radio buttons ",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/RB.png",UriKind.RelativeOrAbsolute))},
                new AdvTool(){ IcoName=AdvTool.Attachment, IcoNameText=" Form attachments ",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/Attachment.png",UriKind.RelativeOrAbsolute))},
                new AdvTool(){ IcoName=AdvTool.ImgAth, IcoNameText=" Image Attachment ",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/ImgAth.png",UriKind.RelativeOrAbsolute))},
                new AdvTool(){ IcoName=AdvTool.Dtl, IcoNameText=" List ( From Table )",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/Dtl.png",UriKind.RelativeOrAbsolute))},
                new AdvTool(){ IcoName=AdvTool.M2M, IcoNameText=" Many relationship ",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/M2M.png",UriKind.RelativeOrAbsolute))},
                new AdvTool(){ IcoName=AdvTool.HiddenField, IcoNameText=" Hidden Fields ",IcoImage=new BitmapImage(new Uri("/CCFormDesigner;component/Img/HiddenField.png",UriKind.RelativeOrAbsolute))}
            };
            return AdvToolList;
        }
        #endregion
    }
}
