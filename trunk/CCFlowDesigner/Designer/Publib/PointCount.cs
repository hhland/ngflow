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
    public class PointCount
    {
        /// <summary>
        ///  Return to the circle outside the points projected onto the edge of the center circle coordinates 
        /// </summary>
        /// <param name="star"> The circle center coordinates </param>
        /// <param name="end"> Outside the circle point coordinates </param>
        /// <param name="r"> Radius </param>
        /// <returns></returns>
        public static Point GetPointAtEllipse(Point star, Point end, double r)
        {
            Point returnPoint = new Point();
            if (star.X == end.X)
            {
                returnPoint.X = star.X;
                if (end.Y <= star.Y)
                    returnPoint.Y = star.Y - r;
                else
                    returnPoint.Y = star.Y + r;
            }
            else if (star.Y == end.Y)
            {
                returnPoint.Y = star.Y;
                if (end.X <= star.X)
                    returnPoint.X = star.X - r;
                else
                    returnPoint.X = star.X + r;
            }
            else
            {
                returnPoint.Y = r * (end.Y - star.Y) / Math.Sqrt(Math.Pow(end.Y - star.Y, 2) + Math.Pow(end.X - star.X, 2)) + star.Y;
                returnPoint.X = (returnPoint.Y - star.Y) * (end.X - star.X) / (end.Y - star.Y) + star.X;
            }
            return (returnPoint);
        }

        /// <summary>
        ///  Returns the rectangle outside the points projected onto the side of the center coordinates of the rectangle 
        /// </summary>
        /// <param name="star"> The center point of the rectangle coordinates </param>
        /// <param name="end"> Outside of the rectangle coordinates </param>
        /// <param name="width"> Rectangular wide </param>
        /// <param name="height"> Rectangular High </param>
        /// <returns></returns>
        public static Point GetPointAtRectangle(Point star, Point end, double width, double height)
        {
            // Need to determine the ratio of the width and height issue 
            Point returnPoint = new Point();
            if (star.X == end.X)
            {
                returnPoint.X = star.X;
                if (end.Y <= star.Y)
                    returnPoint.Y = star.Y - height/2;
                else
                    returnPoint.Y = star.Y + height / 2;
            }
            else if (star.Y == end.Y)
            {
                returnPoint.Y = star.Y;
                if (end.X <= star.X)
                    returnPoint.X = star.X - width / 2;
                else
                    returnPoint.X = star.X + width / 2;
            }
            else
            {
                if (height / width >= Math.Abs(end.Y - star.Y) / Math.Abs(end.X - star.X))
                {
                    if (end.X - star.X <= 0)
                    {
                        returnPoint.X = star.X - width / 2;
                        returnPoint.Y = star.Y + (end.Y - star.Y) * (-width / 2) / (end.X - star.X);
                    }
                    else
                    {
                        returnPoint.X = star.X + width / 2;
                        returnPoint.Y = star.Y + (end.Y - star.Y) * (width / 2) / (end.X - star.X);
                    }
                }
                else
                {
                    if (end.Y - star.Y <= 0)
                    {
                        returnPoint.Y = star.Y - height / 2;
                        returnPoint.X = (-height / 2) * (end.X - star.X) / (end.Y - star.Y) + star.X;
                    }
                    else
                    {
                        returnPoint.Y = star.Y + height / 2;
                        returnPoint.X = (height / 2) * (end.X - star.X) / (end.Y - star.Y) + star.X;
                    }
                }
            }
            return (returnPoint);
        }

        /// <summary>
        ///  According to the center of the two activities , Returns the coordinates and from the edge 
        /// </summary>
        /// <param name="star"> Starting activity center point </param>
        /// <param name="end"> The center of the event </param>
        /// <param name="r"> Round-trip between the radius </param>
        /// <param name="bid"> Round-trip type </param>
        /// <param name="width"> Wide activities </param>
        /// <param name="height"> High activity </param>
        /// <returns></returns>
        public static Point[] IntPoint(Point star, Point end, double r, BP.DirType dirType, double width, double height)
        {
            // Get a new starting point 
            Point newStart = GetBidPoint(star, end, r, dirType, true);
            // Get new end 
            Point newEnd = GetBidPoint(star, end, r, dirType, false);

            Point[] intersection = new Point[2];
            if (newStart.X == newEnd.X)
            {
                if (newStart.Y >= newEnd.Y)
                {
                    intersection[0].X = newStart.X;
                    intersection[0].Y = newStart.Y - height / 2;
                    intersection[1].X = newEnd.X;
                    intersection[1].Y = newEnd.Y + height / 2;
                }
                else
                {
                    intersection[0].X = newStart.X;
                    intersection[0].Y = newStart.Y + height / 2;
                    intersection[1].X = newEnd.X;
                    intersection[1].Y = newEnd.Y - height / 2;
                }
            }
            else if (newStart.Y == newEnd.Y)
            {
                if (newStart.X >= newEnd.X)
                {
                    intersection[0].X = newStart.X - width / 2;
                    intersection[0].Y = newStart.Y;
                    intersection[1].X = newEnd.X + width / 2;
                    intersection[1].Y = newEnd.Y;
                }
                else
                {
                    intersection[0].X = newStart.X + width / 2;
                    intersection[0].Y = newStart.Y;
                    intersection[1].X = newEnd.X - width / 2;
                    intersection[1].Y = newEnd.Y;
                }
            }
            else
            {
                // Define the starting point of the coordinates of the four corners 
                Point Srec1 = new Point(star.X - width / 2, star.Y - height / 2);// The upper left corner 
                Point Srec2 = new Point(star.X + width / 2, star.Y - height / 2);// Top right corner 
                Point Srec3 = new Point(star.X + width / 2, star.Y + height / 2);// Bottom right corner 
                Point Srec4 = new Point(star.X - width / 2, star.Y + height / 2);// Bottom left corner 

                // Which definition of crossover points fall on the edge 
                int Lin1 = 0; //0 To the top ,1 To the right of the ,2 As below ,3 To the left 

                // Define the four corners of the end of the little coordinates 
                Point Erec1 = new Point(end.X - width / 2, end.Y - height / 2);// The upper left corner 
                Point Erec2 = new Point(end.X + width / 2, end.Y - height / 2);// Top right corner 
                Point Erec3 = new Point(end.X + width / 2, end.Y + height / 2);// Bottom right corner 
                Point Erec4 = new Point(end.X - width / 2, end.Y + height / 2);// Bottom left corner 

                // Which definition of crossover points fall on the edge 
                int Lin2 = 0; //0 To the top ,1 To the right of the ,2 As below ,3 To the left 

                #region  Computing the intersection of the edge of the piece falls 
                if (newStart.Y < newEnd.Y)
                {
                    if (newStart.X > newEnd.X)
                        if (Math.Abs((newStart.Y - newEnd.Y) / (newStart.X - newEnd.X)) >= Math.Abs((newStart.Y - Srec4.Y) / (newStart.X - Srec4.X)))
                            Lin1 = 2;
                        else
                            Lin1 = 3;
                    else
                        if (Math.Abs((newStart.Y - newEnd.Y) / (newStart.X - newEnd.X)) >= Math.Abs((newStart.Y - Srec3.Y) / (newStart.X - Srec3.X)))
                            Lin1 = 2;
                        else
                            Lin1 = 1;

                    if (newStart.X > newEnd.X)
                        if (Math.Abs((newEnd.Y - newStart.Y) / (newEnd.X - newStart.X)) >= Math.Abs((newEnd.Y - Erec2.Y) / (newEnd.X - Erec2.X)))
                            Lin2 = 0;
                        else
                            Lin2 = 1;
                    else
                        if (Math.Abs((newEnd.Y - newStart.Y) / (newEnd.X - newStart.X)) >= Math.Abs((newEnd.Y - Erec1.Y) / (newEnd.X - Erec1.X)))
                            Lin2 = 0;
                        else
                            Lin2 = 3;
                }
                else
                {
                    if (newStart.X > newEnd.X)
                        if (Math.Abs((newEnd.Y - newStart.Y) / (newEnd.X - newStart.X)) >= Math.Abs((newStart.Y - Srec1.Y) / (newStart.X - Srec1.X)))
                            Lin1 = 0;
                        else
                            Lin1 = 3;
                    else
                        if (Math.Abs((newEnd.Y - newStart.Y) / (newEnd.X - newStart.X)) >= Math.Abs((newStart.Y - Srec2.Y) / (newStart.X - Srec2.X)))
                            Lin1 = 0;
                        else
                            Lin1 = 1;

                    if (newStart.X > newEnd.X)
                        if (Math.Abs((newStart.Y - newEnd.Y) / (newStart.X - newEnd.X)) >= Math.Abs((newEnd.Y - Erec3.Y) / (newEnd.X - Erec3.X)))
                            Lin2 = 2;
                        else
                            Lin2 = 1;
                    else
                        if (Math.Abs((newStart.Y - newEnd.Y) / (newStart.X - newEnd.X)) >= Math.Abs((newEnd.Y - Erec4.Y) / (newEnd.X - Erec4.X)))
                            Lin2 = 2;
                        else
                            Lin2 = 3;
                }
                #endregion

                #region  Calculate intersection 
                Point point1 = newStart;
                Point point2 = newEnd;
                Point point3 = new Point();
                Point point4 = new Point();
                switch (Lin1)
                {
                    case 0:
                        {
                            point3 = Srec1;
                            point4 = Srec2;
                            break;
                        }
                    case 1:
                        {
                            point3 = Srec2;
                            point4 = Srec3;
                            break;
                        }
                    case 2:
                        {
                            point3 = Srec3;
                            point4 = Srec4;
                            break;
                        }
                    case 3:
                        {
                            point3 = Srec4;
                            point4 = Srec1;
                            break;
                        }
                }
                intersection[0] = Interaction(point1, point2, point3, point4);
                if (intersection[0].Equals(new Point()))
                    intersection[0] = newStart;
                switch (Lin2)
                {
                    case 0:
                        {
                            point3 = Erec1;
                            point4 = Erec2;
                            break;
                        }
                    case 1:
                        {
                            point3 = Erec2;
                            point4 = Erec3;
                            break;
                        }
                    case 2:
                        {
                            point3 = Erec3;
                            point4 = Erec4;
                            break;
                        }
                    case 3:
                        {
                            point3 = Erec4;
                            point4 = Erec1;
                            break;
                        }
                }
                intersection[1] = Interaction(point1, point2, point3, point4);
                if (intersection[1].Equals(new Point()))
                    intersection[1] = newEnd;
                #endregion
            }
            return (intersection);
        }

        /// <summary>
        ///  Return  P1 P2 与 P3 P4  Crosspoint , If you do not  P3 P4  Within the range , Returns  new point()
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="point3"></param>
        /// <param name="point4"></param>
        /// <returns></returns>
        private static Point Interaction(Point point1, Point point2, Point point3, Point point4)
        {
            Point intersection = new Point();
            //k为intersection( Point of intersection )到point1 The distance divided point1到point2 Distance ,k<0 Show intersection在point1 Addition ,k>1 Show intersection在point2 Addition 
            double k = ((point1.Y - point4.Y) * (point3.X - point4.X) - (point1.X - point4.X) * (point3.Y - point4.Y)) / ((point2.X - point1.X) * (point3.Y - point4.Y) - (point2.Y - point1.Y) * (point3.X - point4.X));
            if (k >= 0 && k <= 1)
            {
                intersection.X = point1.X + (point2.X - point1.X) * k;
                intersection.Y = point1.Y + (point2.Y - point1.Y) * k;
            }
            return (intersection);
        }

        /// <summary>
        ///  Returns a new two-way route point 
        /// </summary>
        /// <param name="star"> Starting point </param>
        /// <param name="end"> End </param>
        /// <param name="r"> Radius between the double arrow </param>
        /// <param name="bid"> Strike </param>
        /// <param name="SorE"> Returns the starting point or endpoint </param>
        /// <returns></returns>
        private static Point GetBidPoint(Point star, Point end, double r, BP.DirType dirType, bool SorE)
        {
            Point returnPoint = new Point();
            Point beginPoint = new Point();
            if (SorE)
                beginPoint = star;
            else
                beginPoint = end;
            if (star.X == end.X && star.Y == end.Y)
            {
                returnPoint.X = star.X + r;
                returnPoint.Y = star.Y;
            }
            else
            {
                if (dirType == DirType.Forward)
                {
                    if (star.X >= end.X && star.Y > end.Y)
                    {
                        returnPoint.X = beginPoint.X + Math.Sqrt(Math.Pow(r, 2) * Math.Pow(star.Y - end.Y, 2) / (Math.Pow(star.Y - end.Y, 2) + Math.Pow(star.X - end.X, 2)));
                        returnPoint.Y = beginPoint.Y - Math.Sqrt(Math.Pow(r, 2) * Math.Pow(star.X - end.X, 2) / (Math.Pow(star.Y - end.Y, 2) + Math.Pow(star.X - end.X, 2)));
                    }
                    else if (star.X < end.X && star.Y > end.Y)
                    {
                        returnPoint.X = beginPoint.X + Math.Sqrt(Math.Pow(r, 2) * Math.Pow(star.Y - end.Y, 2) / (Math.Pow(star.Y - end.Y, 2) + Math.Pow(star.X - end.X, 2)));
                        returnPoint.Y = beginPoint.Y + Math.Sqrt(Math.Pow(r, 2) * Math.Pow(star.X - end.X, 2) / (Math.Pow(star.Y - end.Y, 2) + Math.Pow(star.X - end.X, 2)));
                    }
                    else if (star.X < end.X && star.Y <= end.Y)
                    {
                        returnPoint.X = beginPoint.X - Math.Sqrt(Math.Pow(r, 2) * Math.Pow(star.Y - end.Y, 2) / (Math.Pow(star.Y - end.Y, 2) + Math.Pow(star.X - end.X, 2)));
                        returnPoint.Y = beginPoint.Y + Math.Sqrt(Math.Pow(r, 2) * Math.Pow(star.X - end.X, 2) / (Math.Pow(star.Y - end.Y, 2) + Math.Pow(star.X - end.X, 2)));
                    }
                    else
                    {
                        returnPoint.X = beginPoint.X - Math.Sqrt(Math.Pow(r, 2) * Math.Pow(star.Y - end.Y, 2) / (Math.Pow(star.Y - end.Y, 2) + Math.Pow(star.X - end.X, 2)));
                        returnPoint.Y = beginPoint.Y - Math.Sqrt(Math.Pow(r, 2) * Math.Pow(star.X - end.X, 2) / (Math.Pow(star.Y - end.Y, 2) + Math.Pow(star.X - end.X, 2)));
                    }
                }
                else
                {
                    if (star.X <= end.X && star.Y <= end.Y)
                    {
                        returnPoint.X = beginPoint.X - Math.Sqrt(Math.Pow(r, 2) * Math.Pow(end.Y - star.Y, 2) / (Math.Pow(star.Y - end.Y, 2) + Math.Pow(end.X - star.X, 2)));
                        returnPoint.Y = beginPoint.Y + Math.Sqrt(Math.Pow(r, 2) * Math.Pow(end.X - star.X, 2) / (Math.Pow(star.Y - end.Y, 2) + Math.Pow(end.X - star.X, 2)));
                    }
                    else if (star.X > end.X && star.Y <= end.Y)
                    {
                        returnPoint.X = beginPoint.X - Math.Sqrt(Math.Pow(r, 2) * Math.Pow(end.Y - star.Y, 2) / (Math.Pow(star.Y - end.Y, 2) + Math.Pow(end.X - star.X, 2)));
                        returnPoint.Y = beginPoint.Y - Math.Sqrt(Math.Pow(r, 2) * Math.Pow(end.X - star.X, 2) / (Math.Pow(star.Y - end.Y, 2) + Math.Pow(end.X - star.X, 2)));
                    }
                    else if (star.X > end.X && star.Y > end.Y)
                    {
                        returnPoint.X = beginPoint.X + Math.Sqrt(Math.Pow(r, 2) * Math.Pow(end.Y - star.Y, 2) / (Math.Pow(star.Y - end.Y, 2) + Math.Pow(end.X - star.X, 2)));
                        returnPoint.Y = beginPoint.Y - Math.Sqrt(Math.Pow(r, 2) * Math.Pow(end.X - star.X, 2) / (Math.Pow(star.Y - end.Y, 2) + Math.Pow(end.X - star.X, 2)));
                    }
                    else
                    {
                        returnPoint.X = beginPoint.X + Math.Sqrt(Math.Pow(r, 2) * Math.Pow(end.Y - star.Y, 2) / (Math.Pow(star.Y - end.Y, 2) + Math.Pow(end.X - star.X, 2)));
                        returnPoint.Y = beginPoint.Y + Math.Sqrt(Math.Pow(r, 2) * Math.Pow(end.X - star.X, 2) / (Math.Pow(star.Y - end.Y, 2) + Math.Pow(end.X - star.X, 2)));
                    }
                }
            }

            return (returnPoint);
        }
    }
}
