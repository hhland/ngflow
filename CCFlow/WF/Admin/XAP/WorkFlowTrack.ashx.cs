using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows;

using System.Xml.Linq;
using System.IO;
using BP;
using BP.WF;
using BP.DA;
using System.Data;


namespace CCFlow.WF.Admin.XAP
{
    //<img src="WorkFlowTrack.ashx" />
    /// <summary>
    /// WorkFlowTrack  The summary 
    /// </summary>
    public class WorkFlowTrack : IHttpHandler
    {

        public string FK_Flow { get; set; }
        public string WorkID { get; set; }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        System.Drawing.Graphics g;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "image/jpeg";


            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(1024, 768);
            g = System.Drawing.Graphics.FromImage(bitmap);

            // Draw    
            g.Clear(System.Drawing.Color.AntiqueWhite);

            DrawTrack("001", "101");


            g.Dispose();
            bitmap.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            bitmap.Dispose();

        }


        public void DrawTrack(string fk_flow, string workid)
        {
            dicReturnTrackTips = new Dictionary<Direction, IList<string>>();
            dicFlowNode = new Dictionary<string, FlowNode>();

            this.FK_Flow = fk_flow;
            this.WorkID = workid;
            if (!string.IsNullOrEmpty(workid))
            {
                // If you pass over workid  Note To display the rail map .

                try
                {//  Get trajectory data 
                    string sql = "";
                    string table = "ND" + int.Parse(fk_flow) + "Track";

                    sql = "SELECT NDFrom, NDTo,ActionType,Msg,RDT,EmpFrom,EmpFromT FROM " + table + " WHERE WorkID=" + workid + "  OR FID=" + workid;
                    dtTrack = BP.DA.DBAccess.RunSQLReturnTable(sql);
                    dtTrack.TableName = "WF_Track";
                }
                catch (Exception ex)
                {
                    BP.DA.Log.DefaultLogWriteLineError("GetDTOfWorkList An error has occurred  paras:" + fk_flow + "\t" + workid + ex.Message);
                    return;
                }


                this.GenerFlowChart(FK_Flow);
            }
            else
            {
                this.GenerFlowChart(FK_Flow);
            }
        }

        private double _minimalHeight = 300;


        #region Properties


        /// <summary>
        ///  To add a fallback if the current connection status 
        /// </summary>
        public bool IsReturnTypeDir { get; set; }


        public List<FlowNode> flowNodeCollections;
        public List<FlowNode> FlowNodeCollections
        {
            get
            {
                if (flowNodeCollections == null)
                {
                    flowNodeCollections = new List<FlowNode>();
                }
                return flowNodeCollections;
            }
        }
        /// <summary>
        ///  Direction 
        /// </summary>
        public List<Direction> directionCollections;
        /// <summary>
        ///  Direction 
        /// </summary>
        public List<Direction> DirectionCollections
        {
            get
            {
                if (directionCollections == null)
                {
                    directionCollections = new List<Direction>();
                }
                return directionCollections;
            }
        }

        private int nextMaxIndex = 0;
        public int NextMaxIndex
        {
            get
            {
                nextMaxIndex++;
                return nextMaxIndex;
            }
        }
        public double Left
        {
            get { return 230; }
        }
        public double Top
        {
            get { return 40; }
        }

        public int NextNewFlowNodeIndex
        {
            get { return 0; }
        }
        public int NextNewDirectionIndex
        {
            get { return 0; }
        }

        public int NextNewLabelIndex
        {
            get { return 0; }
        }


        #endregion

        #region Variables

        private DataTable dtTrack;
        private double DesignerHeight = 50;
        private double DesignerWdith = 50;
        private Dictionary<Direction, IList<string>> dicReturnTrackTips;
        private Dictionary<string, FlowNode> dicFlowNode;
        #endregion

        #region  Loading process related 
        const int penWidth = 3;
        int shapeWid = 130, shapeHig = 50;
        private void GenerFlowChart(string flowID)
        {
            this.FK_Flow = flowID;
            string sqls = "";
            sqls += "SELECT NodeID,Name,X,Y,NodePosType,RunModel,HisToNDs FROM WF_Node WHERE FK_Flow='" + flowID + "' ORDER BY NodeID ASC";
            sqls += "@SELECT MyPK,Name,X,Y FROM WF_LabNote WHERE FK_Flow='" + flowID + "'";
            sqls += "@SELECT Node,ToNode,DirType,IsCanBack,Dots FROM WF_Direction WHERE FK_Flow='" + flowID + "'";
            sqls += "@SELECT Paras FROM WF_Flow WHERE No='" + flowID + "'";

            try
            {


                string[] strs = sqls.Split('@');
                DataSet ds = new DataSet();
                int i = 0;
                foreach (string sql in strs)
                {
                    if (string.IsNullOrEmpty(sql))
                        continue;
                    DataTable dt = BP.DA.DBAccess.RunSQLReturnTable(sql);
                    dt.TableName = "DT" + i;
                    ds.Tables.Add(dt);
                    i++;
                }

                Rectangle ClientRectangle;
                int x, y;
                string name;

                #region  Painting process node .

                DataTable dtNode = ds.Tables[0];
                for (int r = 0; r < dtNode.Rows.Count; r++)
                {
                    DataRow dr = dtNode.Rows[r];
                    //NodeID,Name,X,Y,NodePosType,RunModel,HisToNDs
                    //  Begin   End   Other 
                    name = dr["Name"].ToString();
                    x = int.Parse(dr["X"].ToString());
                    y = int.Parse(dr["Y"].ToString());
                    NodePosType postype = (NodePosType)int.Parse(dr["NodePosType"].ToString());
                    FlowNodeType runModel = (FlowNodeType)int.Parse(dr["RunModel"].ToString());

                    //  Always make designer width and height to the maximum node 　
                    if (y > DesignerHeight)
                        DesignerHeight = y;

                    if (x > DesignerWdith)
                        DesignerWdith = x;

                    ClientRectangle = new Rectangle() { X = x, Y = y, Width = this.shapeWid, Height = this.shapeHig };
                    switch (postype)
                    {
                        case NodePosType.Start:
                           
                            g.FillEllipse(new SolidBrush(Color.Green), ClientRectangle);

                            break;
                        case NodePosType.Mid:

                            if (runModel == FlowNodeType.Ordinary)
                            {
                                //g.DrawRectangle(Pens.Blue, ClientRectangle);
                                g.FillRectangle(new SolidBrush(Color.LightBlue), ClientRectangle);
                            }
                            else
                            {
                                GraphicsPath path = new GraphicsPath();  // Create graphics path object and add shape.
                                switch (runModel)
                                {

                                    case FlowNodeType.SubThread:

                                        path.AddPolygon(new Point[] { 
                                                new Point(){ X = x + 63, Y = y + 2 } ,
                                                new Point(){ X = x + 126,Y = y + 32 } ,
                                                new Point(){ X = x + 63, Y = y + 62 } , 
                                                new Point(){ X = x + 0,  Y = y + 32 } ,
                                                new Point(){ X = x + 63, Y = y + 2 } , 
                                                new Point(){ X = x + 63, Y = y + 2 } 
                                            });

                                        this.shapeWid = 130;
                                     
                                        ClientRectangle = new Rectangle() { X = x, Y = y, Width = this.shapeWid, Height = this.shapeHig };
                                        break;
                                    case FlowNodeType.HL:
                                       
                                        path.AddPolygon(new Point[] { 
                                                new Point(){ X = x + 0, Y = y + 2 } ,
                                                new Point(){ X = x + 120,Y = y + 2 } ,
                                                new Point(){ X = x + 100,Y = y + 42 } ,
                                                new Point(){ X = x + 20, Y = y + 42 } , 
                                                new Point(){ X = x + 0,  Y = y + 2 } 
                                              
                                            });
                                           
                                        this.shapeWid = 120;
                                    
                                        ClientRectangle = new Rectangle() { X = x, Y = y, Width = this.shapeWid, Height = this.shapeHig };
                                        break;
                                    case FlowNodeType.FL:

                                           path.AddPolygon(new Point[] { 
                                                new Point(){ X = x + 20, Y = y + 2 } ,
                                                new Point(){ X = x + 100,Y = y + 2 } ,
                                                new Point(){ X = x + 120,Y = y + 42 } ,
                                                new Point(){ X = x + 0, Y = y + 42 } , 
                                                new Point(){ X = x + 20,  Y = y + 2 } 
                                              
                                            });
                                        this.shapeWid = 120;
                                     
                                        ClientRectangle = new Rectangle() { X = x, Y = y, Width = this.shapeWid, Height = this.shapeHig };
                                      
                                        break;
                                    case FlowNodeType.FHL:
                                        path.AddPolygon(new Point[] { 
                                                new Point(){ X = x + 20, Y = y + 2 } ,
                                                new Point(){ X = x + 100,Y = y + 2 } ,
                                                new Point(){ X = x + 120,Y = y + 28 } ,
                                                new Point(){ X = x + 100,Y = y + 48 } ,
                                                new Point(){ X = x + 20, Y = y + 48 } , 
                                                new Point(){ X = x + 0,  Y = y + 28 } ,
                                                 new Point(){ X = x + 20, Y = y + 2 } 
                                              
                                            });
                                         
                                        this.shapeWid = 120;
                                     
                                        ClientRectangle = new Rectangle() { X = x, Y = y, Width = this.shapeWid, Height = this.shapeHig };
                                        break;
                                }

                                using (Pen borderPen = new Pen(Color.Blue, penWidth))
                                {
                                    g.DrawPath(borderPen, path);
                                }
                            }
                            break;
                        case NodePosType.End:
                            g.FillEllipse(new SolidBrush(Color.Red), ClientRectangle);
                            break;
                    }



                    // If text is valid, draw it
                    if (!string.IsNullOrEmpty(name))
                    {
                        Font font = new Font(FontFamily.GenericSerif, 10);
                        SizeF tSize = g.MeasureString(name, font);   // Get the width of the string

                        // Get the location to draw
                        Point DrawPoint = new Point(
                            x + (int)((ClientRectangle.Width - tSize.Width) / 2),
                            y + (int)((ClientRectangle.Height - tSize.Height) / 2));

                        using (Brush nodeNameBrush = new SolidBrush(Color.Black))// Release the brush
                        {
                            // Draw the text using the solid brush and the current font
                            g.DrawString(name, font, nodeNameBrush, DrawPoint.X, DrawPoint.Y);
                        }
                    }
                }


                #endregion



                #region  Generate labels .
                DataTable dtLabel = ds.Tables[1];
                foreach (DataRow dr in dtLabel.Rows)
                {
                    //MyPK,Name,X,Y
                    //  Begin   End   Other 
                    name = dr["Name"].ToString();
                    x = int.Parse(dr["X"].ToString());
                    y = int.Parse(dr["Y"].ToString());

                    
                    //ClientRectangle = new Rectangle() { X = x, Y = y, Width = 100, Height = 50 };

                    //// If text is valid, draw it
                    if (!string.IsNullOrEmpty(name))
                    {
                    Font font = new Font(FontFamily.GenericSerif, 8);
                    //    SizeF tSize = g.MeasureString(name, font);   // Get the width of the string

                    //    // Get the location to draw
                    //    Point DrawPoint = new Point(
                    //        x + (int)((ClientRectangle.Width - tSize.Width) / 2),
                    //        y + (int)((ClientRectangle.Height - tSize.Height) / 2));

                        using (Brush nodeNameBrush = new SolidBrush(Color.Green))// Release the brush
                        {
                            // Draw the text using the solid brush and the current font
                            g.DrawString(name, font, nodeNameBrush,x + this.shapeWid /2, y + this.shapeHig/2);
                        }
                    }
                }
                #endregion  Generate labels .

                #region  Generating direction .
                DataTable dtDir = ds.Tables[2];
                // Node,ToNode,DirType,IsCanBack,Dots
                foreach (DataRow dr in dtDir.Rows)
                {
                    string begin = dr["Node"].ToString();
                    string to = dr["ToNode"].ToString();

                    DataRow[] nodeFrom = dtNode.Select("NodeID='" + begin + "'");
                    DataRow[] nodeTo = dtNode.Select("NodeID='" + to + "'");

                    if (null != nodeFrom && 1 == nodeFrom.Length && null != nodeTo && 1 == nodeTo.Length)
                    {
                        //if (dr["Dots"] != null && !string.IsNullOrEmpty(dr["Dots"].ToString()))
                        //{
                        //    strs = dr["Dots"].ToString().Split('@');
                        //    IList<int> dots = new List<int>();

                        //    foreach (string str in strs)
                        //    {
                        //        if (str == null || str == "")
                        //            continue;
                        //        string[] mystr = str.Split(',');
                        //        if (mystr.Length == 2)
                        //        {
                        //            dots.Add(int.Parse(mystr[0]));
                        //            dots.Add(int.Parse(mystr[1]));
                        //        }
                        //    }
                        //}


                    
                        drawArrow(
                            new Point(int.Parse(nodeFrom[0]["X"].ToString()) + this.shapeWid / 2, int.Parse(nodeFrom[0]["Y"].ToString()) + this.shapeHig),
                            new Point(int.Parse(nodeTo[0]["X"].ToString()) + this.shapeWid / 2, int.Parse(nodeTo[0]["Y"].ToString())));

                    }
                }



                Dictionary<FlowNode, Direction> dicEndNodeAndDir = new Dictionary<FlowNode, Direction>();
                // There trajectory end points key,则value+=1; Rollback track there is a real end point key,则value-=1.
                // Finally, =1 The end point of the line corresponding to the direction ( Indicated that they had gone through ) Coloring .
                Dictionary<FlowNode, int> dicEndNodeAndTrackCount = new Dictionary<FlowNode, int>();

                #endregion  Generating direction .

                #region  Marker Color ,  Display track .

                if (dtTrack != null && dtTrack.Rows.Count > 0)
                {
                    /* Explained that it had to take to the track data .
                     * 1, Flow during operation WF_Track  Faithfully recorded the movement of each operation . This data sheet only increase not decrease but do not modify it .
                     * 2, Each event has an event type ActionType,  It is an enumerated type .
                     */
                    this.dicReturnTrackTips.Clear();

                    foreach (DataRow dr in dtTrack.Rows)
                    {
                        string begin = dr["NDFrom"] == null ? string.Empty : dr["NDFrom"].ToString();
                        string to = dr["NDTo"] == null ? string.Empty : dr["NDTo"].ToString();
                        string rdt = dr["RDT"] == null ? string.Empty : dr["RDT"].ToString();// RDT Record Date 

                        try
                        {
                            DateTime time = Convert.ToDateTime(rdt);
                            rdt = time.ToString("MM月dd日HH:mm");
                        }
                        catch { }

                        string msg = dr["Msg"] == null ? string.Empty : dr["Msg"].ToString();
                        string EmpFrom = dr["EmpFrom"] == null ? string.Empty : dr["EmpFrom"].ToString(); // EmpFromT The actual implementation of staff 
                        string EmpFromT = dr["EmpFromT"] == null ? string.Empty : dr["EmpFromT"].ToString();

                        //  Adding a node label 
                        foreach (FlowNode node in FlowNodeCollections)
                        {
                            if (node.FK_Flow == begin)
                            {
                                // addNodeTip(node, EmpFrom, EmpFromT, rdt);
                                break;
                            }
                        }

                        //  Event Type .
                        ActionType at = (ActionType)int.Parse(dr["ActionType"].ToString());
                        switch (at)
                        {
                            case ActionType.Forward: /* Ordinary node sends */
                            case ActionType.ForwardFL: /* Send diversion point */
                            case ActionType.ForwardHL: /* Send confluence */
                            case ActionType.SubFlowForward: /* Send the child thread point */
                            case ActionType.Skip: /* Jump */
                                #region  Painted green line represents the trajectory of a node has gone .
                                //foreach (Direction dir in DirectionCollections)
                                //{
                                //    if (dir.Node == begin && dir.ToNode == to)
                                //    {
                                //        dir.IsTrackingLine = true;
                                //        if (dicEndNodeAndTrackCount.Keys.Contains(dir.EndFlowNode))
                                //        {
                                //            dicEndNodeAndTrackCount[dir.EndFlowNode] += 1;
                                //        }
                                //        break;
                                //    }
                                //}
                                #endregion
                                break;
                            case ActionType.Return: /* Return */


                                bool isExist = false;

                                //foreach (var dir in dicReturnTrackTips.Keys)
                                //{
                                //    if (dir.BeginFlowNode.NodeID == begin && dir.EndFlowNode.NodeID == to)
                                //    {
                                //        dicReturnTrackTips[dir].Add(msg);
                                //        isExist = true;
                                //        break;
                                //    }
                                //}

                                if (!isExist)
                                {
                                    //foreach (Direction dir in DirectionCollections)
                                    //{
                                    //    if (dir.BeginFlowNode.NodeID == begin && dir.EndFlowNode.NodeID == to && dir.DirType == BP.DirType.Backward)
                                    //    {

                                    //        dir.IsTrackingLine = true;
                                    //        dicReturnTrackTips.Add(dir, new List<string>());
                                    //        dicReturnTrackTips[dir].Add(msg);


                                    //        isExist = true;
                                    //        break;
                                    //    }
                                    //}
                                }

                                // Fallback line has been removed , Still shows the trajectory has gone .
                                if (!isExist)
                                {
                                    if (dicFlowNode.ContainsKey(begin) && dicFlowNode.ContainsKey(to))
                                    {
                                        //var d = new Direction((IContainer)this);
                                        //d.FlowID = FlowID;
                                        //d.BeginFlowNode = dicFlowNode[begin]; // Start node .
                                        //d.EndFlowNode = dicFlowNode[to]; ; // End node .
                                        //d.LineType = DirectionLineType.Polyline;
                                        //d.IsTrackingLine = true;
                                        //d.DirType = DirType.Backward;
                                        //dicReturnTrackTips.Add(d, new List<string>());
                                        //dicReturnTrackTips[d].Add(msg);

                                    }
                                }

                                if (dicFlowNode.ContainsKey(begin) && dicEndNodeAndTrackCount.Keys.Contains(dicFlowNode[begin]))
                                {
                                    dicEndNodeAndTrackCount[dicFlowNode[begin]] -= 1;
                                }

                                break;
                            case ActionType.HungUp: /* Pending */
                                break;
                            default:
                                break;
                        }

                    }
                }

                //foreach (var node in dicEndNodeAndTrackCount)
                //{
                //    if (node.Value == 1)
                //    {
                //        dicEndNodeAndDir[node.Key].IsTrackingLine = true;
                //    }
                //}



                dicEndNodeAndDir.Clear();
                dicEndNodeAndTrackCount.Clear();

                #endregion  Marker Color .

                #region  Removal is no fallback trajectory direction lines 
                //List<Direction> dirToDel = new List<Direction>();

                //foreach (var dir in DirectionCollections)
                //{
                //    if (dir.IsReturnType && !dir.IsTrackingLine)
                //    {
                //        dirToDel.Add(dir);
                //    }
                //}

                //foreach (var dir in dirToDel)
                //{
                //    this.RemoveDirection(dir);
                //}

                #endregion


            }
            catch (Exception ee)
            {
                BP.DA.Log.DebugWriteInfo(ee.Message);
            }

        }

        void drawArrow(Point pFrom, Point pTo)
        {
            Pen pen = new Pen(Color.Blue, 3f);
            System.Drawing.Drawing2D.AdjustableArrowCap lineCap = new System.Drawing.Drawing2D.AdjustableArrowCap(3, 3, true);
            pen.CustomEndCap = lineCap;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.DrawLine(pen, pFrom, pTo);
        }

        private void addNodeTip(string empFrom, string empFromT, string receiveTime)
        {
            string imageSource = "{0}.png";

            if (string.IsNullOrEmpty(empFrom))
                imageSource = string.Format(imageSource, "Default");
            else
                imageSource = string.Format(imageSource, empFrom);


            //setImage(tip.bg, imageSource);
            //string uri = Application.Current.Host.Source.AbsoluteUri;// Server Path 
            //int index = uri.IndexOf("/ClientBin");
            //uri = uri.Substring(0, index) + "/DataUser/UserIcon/" + file;
            //tip.SetValue(Canvas.TopProperty, Canvas.GetTop(node) + node.Width / 2 - tip.Height - 20);
            //tip.SetValue(Canvas.LeftProperty, Canvas.GetLeft(node) + tip.Width - 20);
            //tip.SetValue(Canvas.ZIndexProperty, 1000);
        }

        #endregion
    }
    public enum FlowNodeType
    {
        /// <summary>
        ///  General 
        /// </summary>
        Ordinary = 0,
        /// <summary>
        ///  Confluence 
        /// </summary>
        HL = 1,
        /// <summary>
        ///  Bypass 
        /// </summary>
        FL = 2,
        /// <summary>
        ///  Confluence points 
        /// </summary>
        FHL,
        /// <summary>
        ///  Child thread .
        /// </summary>
        SubThread,
        /// <summary>
        ///  Virtual node 
        /// </summary>
        VirtualStart,
        VirtualEnd
        //INITIAL,
        //INTERACTION,
        //COMPLETION,
        ////AND_MERGE, ccflow  Without these types of .
        ////AND_BRANCH,
        //STATIONODE,
        //AUTOMATION,
        //DUMMY,
        ////OR_BRANCH,
        ////OR_MERGE,
        //SUBPROCESS,
        //VOTE_MERGE,
    }


    /// <summary>
    ///  Node Location Type 
    /// </summary>
    public enum FlowNodePosType
    {
        /// <summary>
        ///  Start node 
        /// </summary>
        Start,
        /// <summary>
        ///  Intermediate point 
        /// </summary>
        Mid,
        /// <summary>
        ///  End point 
        /// </summary>
        End
    }

}