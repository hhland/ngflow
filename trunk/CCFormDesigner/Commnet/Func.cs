﻿using System;
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
    public class Func
    {
        #region attrs
        public const string New = "New";
        public const string Open = "Open";
        public const string Save = "Save";
        public const string Copy = "Copy";
        public const string View = "View";
        public const string Exp = "Exp";
        public const string Imp = "Imp";
        public const string HiddenField = "HiddenField";

        /// <summary>
        ///  Picture 
        /// </summary>
        public const string Alignment_Left = "Alignment_Left";
        public const string Alignment_Right = "Alignment_Right";
        public const string Alignment_Top = "Alignment_Top";
        public const string Alignment_Down = "Alignment_Down";
        public const string Alignment_Center = "Alignment_Center";
        public const string Delete = "Delete";
        public const string Undo = "Undo";
        public const string ForwardDo = "ForwardDo";
        public const string Property = "Property";
        public const string Event = "Event";
        /// <summary>
        ///  Extended Settings 
        /// </summary>
        public const string MapExt = "MapExt";
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
        ///  Message 
        /// </summary>
        public string Tooltip
        {
            get { return _Tooltip; }
            set { _Tooltip = value; }
        }
        #endregion

        #region  Single Instance 
        public static readonly Func instance = new Func();
        #endregion

        #region  Public Methods 
        public List<Func> GetToolList()
        {
            List<Func> ToolList = new List<Func>()
            {
              //  new Func(){ No=Func.New, Name=" New ", Img=new BitmapImage(new Uri("/CCFormDesigner;component/Img/"+Func.Open+".png",UriKind.RelativeOrAbsolute))},
               // new Func(){ No=Func.Open, Name=" Turn on ", Img=new BitmapImage(new Uri("/CCFormDesigner;component/Img/"+Func.Open+".png",UriKind.RelativeOrAbsolute))},
                new Func(){ No=Func.Save, Name=" Save ", Tooltip="",Img=new BitmapImage(new Uri("/CCFormDesigner;component/Img/"+Func.Save+".png",UriKind.RelativeOrAbsolute))},

                new Func(){ No=Func.Property,  Name=" Property ", Tooltip=" Modify the form of basic information ", Img=new BitmapImage(new Uri("/CCFormDesigner;component/Img/"+Func.Property+".png",UriKind.RelativeOrAbsolute))},
                new Func(){ No=Func.MapExt,  Name=" Extended Settings ", Tooltip=" Forms business logic configuration ", Img=new BitmapImage(new Uri("/CCFormDesigner;component/Img/"+Func.Property+".png",UriKind.RelativeOrAbsolute))},
                new Func(){ No=Func.View,  Name=" Preview ",Tooltip="in asp Effects preview mode operation ",Img=new BitmapImage(new Uri("/CCFormDesigner;component/Img/"+Func.View+".png",UriKind.RelativeOrAbsolute))},
                //new Func(){ No=Func.Event,  Name=" Event ",Tooltip=" Form events ",Img=new BitmapImage(new Uri("/CCFormDesigner;component/Img/"+Func.Event+".png",UriKind.RelativeOrAbsolute))},
                //new Func(){ No=Func.Copy,  Name=" Copy ",Img=new BitmapImage(new Uri("/CCFormDesigner;component/Img/"+Func.Copy+".png",UriKind.RelativeOrAbsolute))},
                new Func(){ No=Func.Exp, Name=" Export ",Tooltip=" To export the current design to xml Stencil ",Img=new BitmapImage(new Uri("/CCFormDesigner;component/Img/"+Func.Exp+".png",UriKind.RelativeOrAbsolute))},
                new Func(){ No=Func.Imp,Name=" Importing ", Tooltip=" Import form templates ",Img=new BitmapImage(new Uri("/CCFormDesigner;component/Img/"+Func.Imp+".png",UriKind.RelativeOrAbsolute))}
                ,new Func(){ No=Func.HiddenField,Name=" Hidden Fields ", Tooltip=" Hidden Fields ",Img=new BitmapImage(new Uri("/CCFormDesigner;component/Img/HiddenField.png",UriKind.RelativeOrAbsolute))}

            };
            return ToolList;
        }
        #endregion
    }
}
