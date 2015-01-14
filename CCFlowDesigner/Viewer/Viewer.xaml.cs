using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Input;
using BP.WF;
using Silverlight;
using WF.WS;


namespace BP
{

    /// <summary>
    ///  Mainly used to display the process track 
    /// </summary>
    public partial class Viewer : IContainer
    {
        #region FlowChartType Change : General designer has been loaded 

        FlowChartType flow_ChartType = FlowChartType.UnKnown;
        /// <summary>
        ///  Flow chart style 
        ///  Changes :
        /// 1） Update Grid Style , Including grid , Designer background 
        /// 2） Redraw process 
        /// </summary>
        public FlowChartType Flow_ChartType
        {
            get
            {
                return flow_ChartType;
            }
            set
            {
                if (value == flow_ChartType) return;
                flow_ChartType = value;
            }
        }
        void setContainerStyle()
        {
            Content_Resized(this, null);
            this.Visibility = System.Windows.Visibility.Visible;
        }
        /// <summary>
        ///  Switching flow pattern 
        /// </summary>
        /// <param name="value"></param>
        void SetChartType(FlowChartType value)
        {
            if (value != flow_ChartType)
            {
                string msg = " Determine the current flow from " + flow_ChartType.ToString() + " Switch to the " + value.ToString() + " Style ";
                if (MessageBox.Show(msg, " Determine the replacement process Style ?", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    this.flow_ChartType = value;

                    //  Update the database 
                    //  Reload the data 
                    //this.DrawFlows();
                }
            }
        }

       
        #endregion

        #region Constructs
        public Viewer()
        {
            InitializeComponent();
            dicReturnTrackTips = new Dictionary<Direction, IList<string>>();
            dicFlowNode = new Dictionary<string, FlowNode>();

            Application.Current.Host.Content.Resized += Content_Resized;
            this.UcRoot.MouseRightButtonDown += Disable_MouseRightButtonDown;
            this.LayoutRoot.LayoutUpdated += Track_LayoutUpdated;
        }

        const double _minimalHeight = 300;
        void Track_LayoutUpdated(object sender, EventArgs e)
        {
            Size size = this.paint.DesiredSize;
            if (size.Height < _minimalHeight)
                size.Height = _minimalHeight;
            String heightInPixel = String.Format("{0}px", size.Height);
            String containerElementId = "silverlightControlHost";
            HtmlElement element = HtmlPage.Document.GetElementById(containerElementId);
            element.SetStyleAttribute("height", heightInPixel);

        }

       
        private void Disable_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        { //  Shield SL The default context menu 
            e.Handled = true;
        }


        /// <summary>
        /// Content Reset when changing the size of the design ,  Take the size of the design Content And the maximum value of the maximum node  
        /// 20 To assume that the width of the scroll bar .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Content_Resized(object sender, EventArgs e)
        {

            var contentWidth = Application.Current.Host.Content.ActualWidth -100;
            paint.Width = DesignerWdith  > contentWidth    ? DesignerWdith : contentWidth;

            var contentHeight = Application.Current.Host.Content.ActualHeight  ;
            paint.Height = DesignerHeight  > contentHeight  ? DesignerHeight  : contentHeight;
        }


        /// <summary>
        ///  Trajectories 
        /// </summary>
        /// <param name="fk_flow"></param>
        /// <param name="workid"></param>
        public Viewer(string fk_flow, string workid) : this()
        {
            this.FK_Flow = fk_flow;
            this.WorkID = workid;
            try
            {
                if (!string.IsNullOrEmpty(workid))
                {
                    // If you pass over workid  Note To display the rail map .
                    WSDesignerSoapClient ws = BP.Glo.GetDesignerServiceInstance();
                    ws.GetDTOfWorkListAsync(this.FK_Flow, workid);
                    ws.GetDTOfWorkListCompleted += new EventHandler<GetDTOfWorkListCompletedEventArgs>(
                        (object sender, GetDTOfWorkListCompletedEventArgs e)=>
                        {////  Get trajectory data 
                            // 给track dataset  Assignment .
                            trackDataSet = new DataSet();
                            trackDataSet.FromXml(e.Result);

                            // Generate trajectories .
                            this.GenerFlowChart(FK_Flow);
                        });
                              
                }
                else
                {
                    this.GenerFlowChart(FK_Flow);
                }

            }
            catch (Exception e)
            {
                BP.SL.LoggerHelper.Write(e);
            }
        }
        #endregion


        public string GetNewElementName(WorkFlowElementType type)
        {
            return "";
        }
        #region Properties
        public bool IsNeedSave
        {
            get;
            set;
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
      
        private DataSet trackDataSet;
      
        private Dictionary<Direction, IList<string>> dicReturnTrackTips;
        private Dictionary<string, FlowNode> dicFlowNode;

        private WSDesignerSoapClient _service = Glo.GetDesignerServiceInstance();
        public WSDesignerSoapClient _Service
        {
            get { return _service; }
            set { _service = value; }
        }
        public string FK_Flow { get; set; }
        public string WorkID { get; set; }
        public string FlowID { get; set; }
        public int NodeID;
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

        public List<Direction> directionCollections;
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

        public List<NodeLabel> lableCollections;
        public List<NodeLabel> LableCollections
        {
            get
            {
                if (lableCollections == null)
                {
                    lableCollections = new List<NodeLabel>();
                }
                return lableCollections;
            }
        }


        private double DesignerHeight;
        private double DesignerWdith;
        public Double ContainerWidth
        {
            get { return paint.Width; }
            set { paint.Width = value; }
        }

        public Double ContainerHeight
        {
            get { return paint.Height; }
            set { paint.Height = value; }
        }
       
        #endregion

      
        private void GenerFlowChart(string flowID)
        {
            this.FlowID = flowID;
            string sqls = "";
            sqls += "SELECT NodeID,Name,Icon,X,Y,NodePosType,RunModel,HisToNDs,TodolistModel FROM WF_Node WHERE FK_Flow='" + flowID + "'";
            sqls += "@SELECT MyPK,Name,X,Y FROM WF_LabNote WHERE FK_Flow='" + flowID + "'";
            sqls += "@SELECT Node,ToNode,DirType,IsCanBack,Dots FROM WF_Direction WHERE FK_Flow='" + flowID + "'";
            sqls += "@SELECT Paras,ChartType FROM WF_Flow WHERE No='" + flowID + "'";
            try
            {
                WSDesignerSoapClient ws = BP.Glo.GetDesignerServiceInstance();
                ws.RunSQLReturnTableSAsync(sqls);
                ws.RunSQLReturnTableSCompleted += new EventHandler<RunSQLReturnTableSCompletedEventArgs>(ws_RunSQLReturnTableSCompleted);
            }
            catch (Exception e)
            {
                setContainerStyle();
                BP.SL.LoggerHelper.Write(e);
            }
        }
        bool isAddNodeTip = false;
        void ws_RunSQLReturnTableSCompleted(object sender, RunSQLReturnTableSCompletedEventArgs e)
        {
            if (null != e.Error)
            {
                setContainerStyle();
                BP.SL.LoggerHelper.Write(e.Error);
                return;
            }

            try
            {
                var ds = new DataSet();
                ds.FromXml(e.Result);

                DataTable dtFlow = ds.Tables[3];
                if (null != dtFlow && 1 == dtFlow.Rows.Count)
                {
                    //  Style setup process , Change the background color when switching , Gridlines , Process node style 
                    string chartType = dtFlow.Rows[0]["ChartType"];
                    if (chartType == "1")
                    {
                        this.Flow_ChartType = FlowChartType.UserIcon;
                    }
                    else //if (chartType == "0")
                    {
                        this.Flow_ChartType = FlowChartType.Shape;
                    }
                }

                #region  Painting node .
                DataTable dtNode = ds.Tables[0];
                foreach (DataRow rowNode in dtNode.Rows)
                {
                    string icon = string.Empty;
                    if (this.flow_ChartType == FlowChartType.UserIcon)
                    {
                        icon = rowNode["Icon"];
                    }

                    FlowNodeType nodeType = (FlowNodeType)int.Parse(rowNode["RunModel"]);
                    NodePosType postype = (NodePosType)int.Parse(rowNode["NodePosType"]);
                    double x = double.Parse(rowNode["X"]);
                    double y = double.Parse(rowNode["Y"]);

                    FlowNode flowNode = new FlowNode((IContainer)this);
                    flowNode.NodeType = nodeType;
                    flowNode.HisPosType = postype;
                    flowNode.Icon = icon;
                    flowNode.SetValue(Canvas.ZIndexProperty, NextMaxIndex);
                    flowNode.FK_Flow = FlowID;
                    flowNode.NodeID = rowNode["NodeID"];
                    flowNode.NodeName = rowNode["Name"];

                    if (x < 50)
                        x = 50;
                    else if (x > 1190)
                        x = 1190;

                    if (y < 30)
                        y = 30;
                    else if (y > 770)
                        y = 770;

                    flowNode.CenterPoint = new Point(x, y);

                    //  Always make designer width and height to the maximum node 　
                    if (y > DesignerHeight)
                        DesignerHeight = y + 10;

                    if (x > DesignerWdith)
                        DesignerWdith = x + 10;

                    AddUI(flowNode);
                }

                #endregion  Painting node .

                #region  Generate labels .
                DataTable dtLabel = ds.Tables[1];
                foreach (DataRow dr in dtLabel.Rows)
                {
                    var nodeLabel = new NodeLabel((IContainer)this);
                    nodeLabel.LabelName = dr["Name"].ToString();
                    nodeLabel.Position = new Point(double.Parse(dr["X"].ToString()), double.Parse(dr["Y"].ToString()));
                    nodeLabel.LableID = dr["MyPK"].ToString();
                    this.AddUI(nodeLabel);
                }
                #endregion  Generate labels .

                #region  Generating direction .
                DataTable dtDir = ds.Tables[2];

                foreach (DataRow dr in dtDir.Rows)
                {
                    string begin = dr["Node"].ToString();
                    string to = dr["ToNode"].ToString();

                    if (dicFlowNode.ContainsKey(begin) && dicFlowNode.ContainsKey(to))
                    {
                        var d = new Direction((IContainer)this);
                        d.DirType = (DirType)Enum.Parse(typeof(DirType), dr["DirType"].ToString(), false);
                        d.IsCanBack = (int.Parse(dr["IsCanBack"]) == 0) ? false : true;

                        if (dr["Dots"] != null
                            && string.IsNullOrEmpty(dr["Dots"].ToString()) == false)
                        {
                            string[] strs = dr["Dots"].ToString().Split('@');
                            IList<double> dots = new List<double>();

                            foreach (string str in strs)
                            {
                                if (str == null || str == "")
                                    continue;
                                string[] mystr = str.Split(',');
                                if (mystr.Length == 2)
                                {
                                    dots.Add(double.Parse(mystr[0]));
                                    dots.Add(double.Parse(mystr[1]));
                                }
                            }

                            d.LineType = DirectionLineType.Polyline;
                            d.PointTurn1.CenterPosition = new Point(dots[0], dots[1]);
                            d.PointTurn2.CenterPosition = new Point(dots[2], dots[3]);
                        }

                        d.FlowID = FlowID;
                        d.BeginFlowNode = dicFlowNode[begin]; // Start node .
                        d.EndFlowNode = dicFlowNode[to]; // End node .

                        // Increasing direction .
                        this.AddUI(d);
                    }
                }

                //Direction da = new Direction((IContainer)this);
                //da.FlowID = FlowID;
                //da.BeginFlowNode = node0;
                //da.EndFlowNode = GetBeginFlowNode();
                //da.IsTrackingLine = true;
                //AddDirection(da);

                Dictionary<FlowNode, Direction> dicEndNodeAndDir = new Dictionary<FlowNode, Direction>();
                // There trajectory end points key,则value+=1; Rollback track there is a real end point key,则value-=1.
                // Finally, =1 The end point of the line corresponding to the direction ( Indicated that they had gone through ) Coloring .
                Dictionary<FlowNode, int> dicEndNodeAndTrackCount = new Dictionary<FlowNode, int>();

                //IList<FlowNode> endNodes = GetEndFlowNodes();
                //foreach (FlowNode item in endNodes)
                //{
                //    Direction db = new Direction((IContainer)this);
                //    db.FlowID = FlowID;
                //    db.BeginFlowNode = item;
                //    db.EndFlowNode = node1;
                //    AddDirection(db);
                //    dicEndNodeAndDir.Add(item, db);
                //    dicEndNodeAndTrackCount.Add(item, 0);
                //}
                #endregion  Generating direction .

                #region  Marker Color ,  Display track .

                if (trackDataSet != null && null != trackDataSet.Tables["WF_Track"] && trackDataSet.Tables["WF_Track"].Rows.Count > 0)
                {
                    /*
                     *  Explained that it had to take to the track data .
                     * 1, Flow during operation WF_Track  Faithfully recorded the movement of each operation . Data in the table only increase not decrease even without modifying it .
                     * 2, Each event has an event type ActionType,  It is an enumerated type .
                     */
                    this.dicReturnTrackTips.Clear();

                    foreach (DataRow dr in trackDataSet.Tables["WF_Track"].Rows)
                    {
                        string begin = dr["NDFrom"] == null ? string.Empty : dr["NDFrom"].ToString();
                        string to = dr["NDTo"] == null ? string.Empty : dr["NDTo"].ToString();
                        string rdt = dr["RDT"] == null ? string.Empty : dr["RDT"].ToString();// RDT Record Date 
                        string msg = dr["Msg"] == null ? string.Empty : dr["Msg"].ToString();

                        if (isAddNodeTip)
                        {
                            #region  Adding a node label 
                          
                            try
                            {
                                DateTime time = Convert.ToDateTime(rdt);
                                rdt = time.ToString("MM月dd日HH:mm");
                            }
                            catch { }

                            string EmpFrom = dr["EmpFrom"] == null ? string.Empty : dr["EmpFrom"].ToString(); // EmpFromT The actual implementation of staff 
                            string EmpFromT = dr["EmpFromT"] == null ? string.Empty : dr["EmpFromT"].ToString();

                            FlowNode node = dicFlowNode[begin];
                            if (null != node && !node.IsTrackDealed)
                            {
                                //  Calculation process node is done in collaboration multiplayer 
                                if (node.TodolistModel == TodolistModel.QiangBan)
                                    addNodeTip(node, EmpFrom, EmpFromT, rdt);
                                else
                                {
                                    addNodeTip(node, "", " Multiplayer done in collaboration ", !string.IsNullOrEmpty(rdt) && rdt.Length > 5 ? rdt.Substring(0, rdt.Length - 5) : "");
                                }
                                node.IsTrackDealed = true;
                            }
                            #endregion
                        }

                        #region  Painted green line represents the trajectory of a node has gone .
                        foreach (Direction dir in DirectionCollections)
                        {
                            if (dir.BeginFlowNode.NodeID == begin && dir.EndFlowNode.NodeID == to)
                            {
                                dir.IsTrackingLine = true;

                                if (dicEndNodeAndTrackCount.Keys.Contains(dir.EndFlowNode))
                                {
                                    dicEndNodeAndTrackCount[dir.EndFlowNode] += 1;
                                }
                                break;
                            }
                        }
                        #endregion

                     
                        ActionType at = (ActionType)int.Parse(dr["ActionType"].ToString());
                        switch (at)
                        {
                            case ActionType.Forward: /* Ordinary node sends */
                            case ActionType.ForwardFL: /* Send diversion point */
                            case ActionType.ForwardHL: /* Send confluence */
                            case ActionType.SubFlowForward: /* Send the child thread point */
                            case ActionType.HungUp: /* Pending */
                                break;
                            case ActionType.Skip: /* Jump */
                               
                                break;
                            case ActionType.Return: /* Return */

                                bool isExist = false;

                                foreach (var dir in dicReturnTrackTips.Keys)
                                {
                                    if (dir.BeginFlowNode.NodeID == begin && dir.EndFlowNode.NodeID == to)
                                    {
                                        dicReturnTrackTips[dir].Add(msg);
                                        isExist = true;
                                        break;
                                    }
                                }

                                if (!isExist)
                                {
                                    foreach (Direction dir in DirectionCollections)
                                    {
                                        if (dir.BeginFlowNode.NodeID == begin && dir.EndFlowNode.NodeID == to && dir.DirType == BP.DirType.Backward)
                                        {

                                            dir.IsTrackingLine = true;
                                            dicReturnTrackTips.Add(dir, new List<string>());
                                            dicReturnTrackTips[dir].Add(msg);

                                            isExist = true;
                                            break;
                                        }
                                    }
                                }

                                // Fallback line has been removed , Still shows the trajectory has gone .
                                if (!isExist)
                                {
                                    if (dicFlowNode.ContainsKey(begin) && dicFlowNode.ContainsKey(to))
                                    {
                                        var d = new Direction((IContainer)this);
                                        d.FlowID = FlowID;
                                        d.BeginFlowNode = dicFlowNode[begin]; // Start node .
                                        d.EndFlowNode = dicFlowNode[to]; ; // End node .
                                        d.LineType = DirectionLineType.Polyline;
                                        d.IsTrackingLine = true;
                                        d.DirType = DirType.Backward;
                                        dicReturnTrackTips.Add(d, new List<string>());
                                        dicReturnTrackTips[d].Add(msg);

                                        if (paint.Children.Contains(d) == false)
                                        {
                                            paint.Children.Add(d);
                                            d.Container = this;
                                        }
                                    }
                                }

                                if (dicFlowNode.ContainsKey(begin) && dicEndNodeAndTrackCount.Keys.Contains(dicFlowNode[begin]))
                                {
                                    dicEndNodeAndTrackCount[dicFlowNode[begin]] -= 1;
                                }

                                break;
                          
                            default:
                                break;
                        }

                    }
                }

                foreach (var node in dicEndNodeAndTrackCount)
                {
                    if (node.Value == 1)
                    {
                        dicEndNodeAndDir[node.Key].IsTrackingLine = true;
                    }
                }


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

                #region  Disable to disable all the elements of the workflow workflow elements all events , View only 
                foreach (UIElement ui in paint.Children)
                {
                    if (ui is IElement)
                    {
                        UserControl uc = ui as UserControl;
                        if (uc != null)
                            uc.IsEnabled = false;
                    }
                }
                #endregion

                setContainerStyle();
            }
            catch (Exception ee)
            {
                BP.SL.LoggerHelper.Write(ee);
            }
        }

        public void AddUI(Direction dir)
        {
           
            if (paint.Children.Contains(dir) == false)
            {
                paint.Children.Add(dir);
                dir.Container = this;
                //SolidColorBrush brush = new SolidColorBrush();
                //brush = SystemConst.ColorConst.UnTrackingColor.DirectionColor as SolidColorBrush;//.Color = Colors.Gray;
                //if (dir.BeginFlowNode.NodeID == "0")
                //{
                //    brush = SystemConst.ColorConst.TrackingColor.DirectionColor as SolidColorBrush;//Colors.Green;
                //}
                ////dir.begin.Fill = brush;
                ////dir.endArrow.Stroke = brush;
                ////dir.line.Stroke = brush;
                ////if (dir.LineType == DirectionLineType.Polyline)
                ////{
                ////    dir.DirectionTurnPoint1.Fill = brush;
                ////    dir.DirectionTurnPoint2.Fill = brush;
                ////}

                //dir.begin.Fill = brush;
                //dir.begin.Width = 4;
                //dir.begin.Height = 4;
                //dir.endArrow.Stroke = brush;
                //dir.endArrow.StrokeThickness = 2;
                //dir.line.Stroke = brush;
                //dir.line.StrokeThickness = 2;
                //if (dir.LineType == DirectionLineType.Polyline)
                //{
                //    dir.DirectionTurnPoint1.Fill = brush;
                //    dir.DirectionTurnPoint2.Fill = brush;
                //    dir.DirectionTurnPoint1.eliTurnPoint.Width = 4;
                //    dir.DirectionTurnPoint1.eliTurnPoint.Height = 4;
                //    dir.DirectionTurnPoint2.eliTurnPoint.Width = 4;
                //    dir.DirectionTurnPoint2.eliTurnPoint.Height = 4;
                //}
            }

            if (DirectionCollections.Contains(dir) == false)
                DirectionCollections.Add(dir);        
        }

        public void AddUI(NodeLabel l)
        {
            if (!paint.Children.Contains(l) )
                paint.Children.Add(l);

        }

        public void AddUI(FlowNode a)
        {
            if (!paint.Children.Contains(a))
            {
                paint.Children.Add(a);
                a.Container = this;
                a.FK_Flow = FlowID;
            }

            if (!FlowNodeCollections.Contains(a) )
            {
                FlowNodeCollections.Add(a);

                if (!dicFlowNode.ContainsKey(a.NodeID))
                {
                    dicFlowNode.Add(a.NodeID, a);
                }
            }
        }

        private void addNodeTip(FlowNode node, string empFrom, string empFromT, string receiveTime)
        {
            string imageSource = "{0}.png";

            if (string.IsNullOrEmpty(empFrom))
                imageSource = string.Format(imageSource, "Default");
            else
                imageSource = string.Format(imageSource, empFrom);

            NodeTip tip = new NodeTip(empFromT, receiveTime);
            setImage(tip.bg, imageSource);
            if (this.flow_ChartType == FlowChartType.UserIcon)
            {
                tip.SetValue(Canvas.LeftProperty, Canvas.GetLeft(node) + node.UIWidth);
            }
            else
            {
                tip.SetValue(Canvas.LeftProperty, Canvas.GetLeft(node) + node.UIWidth/2 + 10);
            }
            tip.SetValue(Canvas.TopProperty, Canvas.GetTop(node) + node.UIHeight / 2 - tip.Height / 2);

            tip.SetValue(Canvas.ZIndexProperty, 1000);
            paint.Children.Add(tip);
        }

        public void setImage(System.Windows.Controls.Image image, String file)
        {
            string uri = Application.Current.Host.Source.AbsoluteUri;// Server Path 
            int index = uri.IndexOf("/ClientBin");
            uri = uri.Substring(0, index) + "/DataUser/UserIcon/" + file;
            Uri u = new Uri(uri);
            System.Net.WebClient wc = new System.Net.WebClient();

            wc.OpenReadAsync(u);
            wc.OpenReadCompleted += delegate(object sender, System.Net.OpenReadCompletedEventArgs e)
            {
                System.Windows.Media.Imaging.BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
                if (null == e.Error)
                {
                    bitmapImage.SetSource(e.Result);
                    image.Source = bitmapImage;
                }
            };
        }

        #region
        public bool CtrlKeyIsPress
        {
            get { throw new NotImplementedException(); }
        }

        public double ScrollViewerHorizontalOffset
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double ScrollViewerVerticalOffset
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsSomeChildEditing
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsContainerRefresh
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool MouseIsInContainer
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsMouseSelecting
        {
            get { throw new NotImplementedException(); }
        }

        public Direction CurrentTemporaryDirection
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public List<IElement> SelectedWFElements
        {
            get { throw new NotImplementedException(); }
        }

        public PageEditType EditType
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Canvas GridLinesContainer
        {
            get { throw new NotImplementedException(); }
        }

        public void RemoveUI(FlowNode a)
        {
            throw new NotImplementedException();
        }

        public void RemoveUI(NodeLabel l)
        {
            throw new NotImplementedException();
        }

        public void CreateLabel(Point p)
        {
            throw new NotImplementedException();
        }

        public void RemoveUI(Direction r)
        {
            throw new NotImplementedException();
        }

        public void SelectDirection(Direction r, bool isSelected)
        {
            throw new NotImplementedException();
        }

        public void SelectFlowNode(FlowNode a, bool isSelected)
        {
            throw new NotImplementedException();
        }

        public void ShowMessage(string message)
        {
            throw new NotImplementedException();
        }

        public void ShowSetting(FlowNode ac)
        {
            throw new NotImplementedException();
        }

        public void ShowSetting(Direction rc)
        {
            throw new NotImplementedException();
        }

        public void ShowContentMenu(FlowNode a, object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void ShowContentMenu(NodeLabel l, object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void ShowContentMenu(Direction r, object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void AddSelectedControl(IElement uc)
        {
            throw new NotImplementedException();
        }

        public void RemoveSelectedControl(IElement uc)
        {
            throw new NotImplementedException();
        }

        public void SelectedWorkFlowElement(IElement uc, bool isSelect)
        {
            throw new NotImplementedException();
        }

        public void MoveControlCollectionByDisplacement(double x, double y, IElement uc)
        {
            throw new NotImplementedException();
        }

        public void ClearSelected(IElement uc)
        {
            throw new NotImplementedException();
        }

        public void CopySelectedToMemory(IElement currentControl)
        {
            throw new NotImplementedException();
        }

        public void UpdateSelectedToMemory(IElement currentControl)
        {
            throw new NotImplementedException();
        }

        public void UpdateSelectedNode(IElement currentControl)
        {
            throw new NotImplementedException();
        }

        public void DeleteSeleted()
        {
            throw new NotImplementedException();
        }

        public void PastMemoryToContainer()
        {
            throw new NotImplementedException();
        }

        public void ActionPrevious()
        {
            throw new NotImplementedException();
        }

        public void ActionNext()
        {
            throw new NotImplementedException();
        }

        public bool Contains(UIElement uiel)
        {
            throw new NotImplementedException();
        }

        public CheckResult CheckSave()
        {
            throw new NotImplementedException();
        }

        public void SaveChange(HistoryType action)
        {
            throw new NotImplementedException();
        }

        public void SetProper(string lang, string dotype, string fk_flow, string node1, string node2, string title)
        {
            throw new NotImplementedException();
        }

        public void SetGridLines(bool isShow)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

}
