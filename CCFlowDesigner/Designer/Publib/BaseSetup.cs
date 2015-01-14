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

namespace BP
{
    public class BaseSetup
    {
        #region  Starting point correlation 
        
        /// <summary>
        ///  Starting color 
        /// </summary>
        public static Brush StartColor
        {
            get
            {
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Color.FromArgb(255, 0, 80, 0);
                return brush;
            }
        }
        /// <summary>
        ///  Starting diameter 
        /// </summary>
        public static double StartDia
        {
            get { return 50.0; }
        }
        /// <summary>
        ///  Starting opacity 
        /// </summary>
        public static double StartOpacity
        {
            get { return 1.0; }
        }

        #endregion

        #region  Drag point correlation rules 

        /// <summary>
        ///  Dragging a little color 
        /// </summary>
        public static Brush RuleDragColor
        {
            get
            {
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Color.FromArgb(255, 160, 160, 160);
                return brush;
            }
        }
        /// <summary>
        ///  Drag the dot diameter 
        /// </summary>
        public static double RuleDragDia
        {
            get { return 20.0; }
        }
        /// <summary>
        ///  Drag point Opacity 
        /// </summary>
        public static double RuleDragOpacity
        {
            get { return 0.8; }
        }
        /// <summary>
        ///  Drag the line color 
        /// </summary>
        public static Brush RuleDragThicknessColor
        {
            get
            {
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Color.FromArgb(255, 160, 160, 160);
                return brush;
            }
        }
        /// <summary>
        ///  Drag the line width 
        /// </summary>
        public static double RuleDragThickness
        {
            get { return 5.0; }
        }
        /// <summary>
        ///  Opacity drag line 
        /// </summary>
        public static double RuleDragThicknessOpacity
        {
            get { return 0.8; }
        }

        #endregion

        #region  Activities related node 

        /// <summary>
        ///  Active node background color 
        /// </summary>
        public static Brush ActivityBackgroundColor
        {
            get
            {
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Color.FromArgb(255, 255, 255, 255);
                return brush;
            }
        }
        /// <summary>
        ///  Active node border color 
        /// </summary>
        public static Brush ActivityStrokeColor
        {
            get
            {
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Color.FromArgb(255, 0, 80, 0);
                return brush;
            }
        }
        /// <summary>
        ///  Active node border width 
        /// </summary>
        public static double ActivityStrokeThickness
        {
            get { return 2.0; }
        }
        /// <summary>
        ///  Active node width 
        /// </summary>
        public static double ActivityWidth
        {
            get { return 120.0; }
        }
        /// <summary>
        ///  Highly active node 
        /// </summary>
        public static double ActivityHeight
        {
            get { return 60.0; }
        }
        /// <summary>
        ///  Active node fillet X Axis Radius 
        /// </summary>
        public static double ActivityRadiusX
        {
            get { return 10.0; }
        }
        /// <summary>
        ///  Active node fillet Y Axis Radius 
        /// </summary>
        public static double ActivityRadiusY
        {
            get { return 10.0; }
        }
        /// <summary>
        ///  Active node opacity 
        /// </summary>
        public static double ActivityOpacity
        {
            get { return 1.0; }
        }

        /// <summary>
        ///  Title Color 
        /// </summary>
        public static Brush ActivityTitleColor
        {
            get
            {
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Color.FromArgb(255, 0, 0, 255);
                return brush;
            }
        }

        /// <summary>
        ///  The title font size 
        /// </summary>
        public static double ActivityTitleSize
        {
            get { return 11.0; }
        }

        #endregion

        #region  Routing lines related 

        /// <summary>
        ///  Rules of line color 
        /// </summary>
        public static Brush RuleColor
        {
            get
            {
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Color.FromArgb(255, 0, 80, 0);
                return brush;
            }
        }

        /// <summary>
        ///  Rules Line opacity 
        /// </summary>
        public static double RuleOpacity
        {
            get { return 0.8; }
        }

        /// <summary>
        ///  Rules of the line width 
        /// </summary>
        public static double RuleThickness
        {
            get { return 8.0; }
        }

        /// <summary>
        ///  Arrow side 
        /// </summary>
        public static double RuleArrowPar
        {
            get { return 20.0; }
        }

        /// <summary>
        ///  Title Color 
        /// </summary>
        public static Brush RuleTitleColor
        {
            get
            {
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Color.FromArgb(255, 180, 40, 0);
                return brush;
            }
        }

        /// <summary>
        ///  The title font size 
        /// </summary>
        public static double RuleTitleSize
        {
            get { return 11.0; }
        }

        /// <summary>
        ///  The title of the play at the line rate routing ,1 To center 
        /// </summary>
        public static double RuleTitleRatio
        {
            get { return 0.7; }
        }

        #endregion

        #region  End point related 

        /// <summary>
        ///  End point color 
        /// </summary>
        public static Brush EndColor
        {
            get
            {
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Color.FromArgb(255, 255, 0, 0);
                return brush;
            }
        }
        /// <summary>
        ///  The diameter of the end point 
        /// </summary>
        public static double EndDia
        {
            get { return 50.0; }
        }
        /// <summary>
        ///  End point Opacity 
        /// </summary>
        public static double EndOpacity
        {
            get { return 1.0; }
        }

        #endregion

        #region  Arrow Related 

        /// <summary>
        ///  Arrow Color 
        /// </summary>
        public static Brush ArrowColor
        {
            get
            {
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Color.FromArgb(255, 160, 160, 160);
                return brush;
            }
        }
        /// <summary>
        ///  Arrow side 
        /// </summary>
        public static double ArrowPar
        {
            get { return 20.0; }
        }
        /// <summary>
        ///  Arrow opacity 
        /// </summary>
        public static double ArrowOpacity
        {
            get { return 0.8; }
        }

        #endregion

        #region  And from the relevant arrow 

        /// <summary>
        ///  Radius of the round-trip between the arrows 
        /// </summary>
        public static double BidRad
        {
            get { return 10.0; }
        }

        #endregion

        /// <summary>
        ///  Warning color 
        /// </summary>
        public static Brush WarningColor
        {
            get
            {
                return new SolidColorBrush(Color.FromArgb(255, 232, 112, 2));
            }
        }

        public static Brush DefaultSelectColor
        { 
            get
            {
                return new SolidColorBrush(Color.FromArgb(255, 160, 171, 193));
            }
        }
    }
}
