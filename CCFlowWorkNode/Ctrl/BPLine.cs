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
namespace WorkNode
{
    public class BPLine : System.Windows.Controls.Label
    {
        public string FK_MapData = null;
        /// <summary>
        /// BPLine
        /// </summary>
        public BPLine()
        {
            this.Name = "Line" + DateTime.Now.ToString("yyMMhhddhhss");
        }
        public string Color = null;
        public BPLine(string name, string color, double borderW,
            double x1, double y1, double x2, double y2)
        {
            this.Name = name;
            this.MyLine = new Line();
            this.MyLine.Name = "lo" + name;
            this.MyLine.X1 = x1;
            this.MyLine.Y1 = y1;
            this.MyLine.X2 = x2;
            this.MyLine.Y2 = y2;
            this.MyLine.StrokeThickness = borderW;
            this.MyLine.Cursor = Cursors.Hand;
            this.Color = color;
            this.MyLine.Stroke = new SolidColorBrush(Glo.ToColor(color));
            this.Content = this.MyLine;
        }
        public Line MyLine = null;
        public Point MoveFrom = new Point(0,0);
        public Point MoveTo = new Point(0,0);
    }
}
