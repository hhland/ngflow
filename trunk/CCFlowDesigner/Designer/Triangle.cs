
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

namespace BP
{
    public class Arrowhead : System.Windows.Controls.Canvas
    {
       
        int arrowLenght = 12;
        int arrowAngle = 30;

        /// <summary>
        ///  Length of the arrow 
        /// </summary>
        public int ArrowLenght
        {
            get
            {
                return arrowLenght;
            }
            set
            {
                arrowLenght = value;
            }
        }
        /// <summary>
        ///  The angle between the arrow and line 
        /// </summary>
        public int ArrowAngle
        {
            get
            {
                return arrowAngle;
            }
            set
            {
                arrowAngle = value;
            }
        } 
        Line lineLeft;
        Line lineRight;
        Line lineCenter;
        public int ZIndex
        {
            get
            {
                return (int)this.GetValue(Canvas.ZIndexProperty);
            }
            set
            {
                this.SetValue(Canvas.ZIndexProperty, value);
            }
        }
        public new double Opacity
        {
            get
            {
                return lineRight.Opacity;
            }
            set
            {
                lineRight.Opacity = value;
                lineLeft.Opacity = value;

            }
        }
        public Brush Stroke
        {
            get
            {
                return lineRight.Stroke; 
            }
            set
            {
                lineRight.Stroke = value;
                lineLeft.Stroke = value;
                lineCenter.Stroke = value;

            }
        }
        public double StrokeThickness
        {
            set
            {
                lineRight.StrokeThickness = value;
                lineLeft.StrokeThickness = value;
                lineCenter.StrokeThickness = value;
            }
            get
            {
                return lineLeft.StrokeThickness;
            }
        }
        void SetAngleByDegree(double degreeLeft,double degreeRight)
        {
            double angleSi = Math.PI * degreeLeft / 180.0;
            double x = System.Math.Sin(Math.PI * degreeLeft / 180.0);
            double y = System.Math.Sin(Math.PI * (90 - degreeLeft) / 180.0);  

            lineLeft.X2 = -ArrowLenght * x;
            lineLeft.Y2 = -ArrowLenght * y;
            x = System.Math.Sin(Math.PI * degreeRight / 180.0);
            y = System.Math.Sin(Math.PI * (90 - degreeRight) / 180.0);   
            lineRight.X2 = ArrowLenght * x;
            lineRight.Y2 = -ArrowLenght * y;

            lineCenter.X2 = (lineRight.X2 + lineLeft.X2) / 2 ;
            lineCenter.Y2 = (lineRight.Y2 + lineLeft.Y2) / 2 ;


        }
        /// <summary>
        ///  According to the coordinates of the rotation angle of the arrow straight set the start and end points 
        /// </summary>
        /// <param name="beginPoint"></param>
        /// <param name="endPoint"></param>
        public void SetAngleByPoint(Point beginPoint, Point endPoint)
        { 
            double x = endPoint.X - beginPoint.X;
            double y = endPoint.Y - beginPoint.Y;
            double angle = 0;
            if (y == 0)
            { 
                if(x>0)
                    angle = -90;
                else
                    angle = 90;

            }
            else
                angle = System.Math.Atan(x / y) * 180 / Math.PI; 
                

            if(endPoint.Y <= beginPoint.Y)
                SetAngleByDegree((ArrowAngle + angle) - 180, (ArrowAngle - angle) - 180);
            else
                SetAngleByDegree(ArrowAngle + angle, ArrowAngle - angle);
           

        }
        public Arrowhead()
        { 
            lineLeft = new Line();
            lineRight = new Line();
            lineCenter = new Line();
            this.Children.Add(lineLeft);
            this.Children.Add(lineRight);
            this.Children.Add(lineCenter);
            lineCenter.Opacity = 0.1;

            lineLeft.X1 = 0;
            lineLeft.Y1 = 0;
            lineRight.X1 = 0;
            lineRight.Y1 = 0; 
            lineCenter.X1 = 0;
            lineCenter.Y1 = 0;
            SetAngleByPoint(new Point(0, 0), new Point(15, 15));  
        }
    }
}