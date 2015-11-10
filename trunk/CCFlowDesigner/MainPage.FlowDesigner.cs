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
using System.Windows.Browser;
using BP.Controls;
using WF.WS;
using BP.SL;
using Liquid;

namespace BP
{
    /// <summary>
    ///  Home Page - Process Designer 
    /// </summary>
    public partial class MainPage 
    {

        /// <summary>
        ///  Open Workflow 
        /// </summary>
        /// <param name="flowSortId"></param>
        /// <param name="flowId"></param>
        /// <param name="flowName"></param>
        internal void OpenFlow(string flowSortId, string flowId, string flowName)
        {
            OpenFlow(flowId, flowName);
        }
        /// <summary>
        ///  Open Workflow 
        /// </summary>
        /// <param name="flowid"> Workflow ID</param>
        /// <param name="title"> Workflow name </param>
        internal void OpenFlow(string flowid, string title)
        {
            foreach (TabItem t in tbDesigner.Items)
            {
                var ct = t.Content as Container;
                if (ct != null && ct.FlowID == flowid)
                {
                    tbDesigner.SelectedItem = t;
                    return;
                }
            }

            Container c = new Container()
            {
                FlowID = flowid,
                Margin = new Thickness(0)
            }.DrawFlows();

            TabItemEx ti = new TabItemEx()
            {
                Tag = title,
                Title = title,
                Content = c
            };
            Button btnClose = new Button()
            {
                Opacity = 0.2,
                Content = "╳",
                MinHeight = 15,
                MinWidth = 17,
                ClickMode = ClickMode.Release

            };
            btnClose.Click += new RoutedEventHandler(btnClose_Click);

            TextBlock tbx = new TextBlock()
            {
                Text = title,
                Width = title.Length
            };

            Canvas cs = new Canvas();
            cs.Children.Add(btnClose);
            cs.Children.Add(tbx);
            cs.Height = 20;

            ti.Width = tbx.ActualWidth + btnClose.ActualWidth + 30;
            ti.Header = cs;
            btnClose.DataContext = ti;
            btnClose.SetValue(Canvas.LeftProperty, ti.Width - btnClose.ActualWidth - 20);
            btnClose.SetValue(Canvas.TopProperty, 0.0);

            tbx.SetValue(Canvas.TopProperty, btnClose.ActualHeight);
            tbx.SetValue(Canvas.LeftProperty, 0.0);
            ti.SetValue(TabControl.WidthProperty, tbx.ActualWidth + btnClose.ActualWidth + 40);

            //ti.VerticalAlignment = VerticalAlignment.Top;
            //ti.VerticalContentAlignment = VerticalAlignment.Top;
            //ti.VerticalContentAlignment = System.Windows.VerticalAlignment.Stretch;
            //ti.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch;

            tbDesigner.Items.Add(ti);
            tbDesigner.SelectedItem = ti;

            setToolBarButtonEnableStatus(true);
        }

        /// <summary>
        ///  The current flow changes , To ensure that only the current process only display the Close button , This avoids [ The current process is A, Then click B Close button 
        ///  Indicates that the user would like to close B, But is closed A] Case .
        /// </summary>
        private void tabControlDesigner_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (TabItem t in tbDesigner.Items)
            {
                if (t.IsSelected)
                {
                    setToolBoxVisiable(false);
                    var canvas = t.Header as Canvas;
                    if (canvas != null)
                    {
                        canvas.Children[0].Visibility = Visibility.Visible;
                    }

                    if (null != t.Tag)
                    {
                        TreeNode flow = Glo.findNode(tvwFlow.Nodes[0] as TreeNode, t.Tag.ToString());

                        if (null != flow && null != flow.ParentNode)
                        {
                            Node nodeP = flow.ParentNode;
                            nodeP.Expand();
                            nodeP.IsExpanded = true;
                            tvwFlow.Selected = flow;
                        }
                    }
                }
                else
                {
                    var canvas = t.Header as Canvas;
                    if (canvas != null)
                    {
                        canvas.Children[0].Visibility = Visibility.Collapsed;
                    }
                }
            }
        }


        /// <summary>
        ///  Close tab event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedContainer != null)
            {
                // save the newest img of designer when closed
                BP.SL.Glo.SaveUIElementAsPng(SelectedContainer.cnsDesignerContainer, 1, SelectedContainer.FlowID);

                if (SelectedContainer.IsNeedSave)
                {
                    bool result = true;
                    result = HtmlPage.Window.Confirm(" Whether to save the file ");

                    if (result && save())
                    {
                        this.tbDesigner.Items.Remove(this.tbDesigner.SelectedItem);
                    }
                    else
                    {
                        tbDesigner.Items.Remove(this.tbDesigner.SelectedItem);
                    }
                }
                else
                {
                    tbDesigner.Items.Remove(this.tbDesigner.SelectedItem);

                }
            }

            if (tbDesigner.Items.Count == 0)
            {
                setToolBarButtonEnableStatus(false);
                tvwFlow.Selected = null;
            }
        }

        bool save()
        {
            bool result = true;
            if (null != SelectedContainer)
            {
                result = SelectedContainer.Save(null);
            }
            return result;
        }

      
        public void SetSelectedTool(string id)
        {  //  Setting choice ToolBox. And configure the mouse style 

            CustomCursor CCursor = Glo.CCursor;
            switch (id)
            {
                case "Arrow":
                    CCursor.SetCursorDefault(Cursors.Arrow);
                    break;
                case "Hand":
                    CCursor.SetCursorDefault(Cursors.Hand);
                    break;
                case "Wait":
                    CCursor.SetCursorDefault(Cursors.Wait);
                    break;
                default:
                    DataTemplate cursor = null;
                    cursor = ToolBoxNodeIcons.Instance.GetCursorTemplate(id);
                    if (cursor == null )
                        CCursor.SetCursorDefault(Cursors.Hand);
                    else
                        CCursor.SetCursorTemplate(cursor);
                    break;
            }
        }

        //  Toolbox button to switch status 
        void setToolBoxVisiable(bool isShow)
        {
            if (isShow && null != SelectedContainer)
            {
                if (SelectedContainer.Flow_ChartType == FlowChartType.UserIcon)
                {
                    flowToolBox.SetVisiable(false);
                    flowToolBoxIcon.SetVisiable(!flowToolBoxIcon.isOpened);
                }
                else
                {
                    flowToolBoxIcon.SetVisiable(false);
                    flowToolBox.SetVisiable(!this.flowToolBox.isOpened);
                }
            }
            else
            {
                flowToolBox.SetVisiable(false);
                flowToolBoxIcon.SetVisiable(false);
            }
        }
      
        private void workSpace_MouseMove(object sender, MouseEventArgs e)
        {
        }
        private void workSpace_MouseEnter(object sender, MouseEventArgs e)
        {
            if (ToolBoxNodeIcons.IsToolDraging && !string.IsNullOrEmpty(NewElementNameOrIcon))
            {
                MainPage.Instance.SetSelectedTool(NewElementNameOrIcon);

            }
            else if (ToolBox.isToolDraging)
            {
                if (string.IsNullOrEmpty(NewElementNameOrIcon))
                    Instance.SetSelectedTool("Arrow");
                else
                    Instance.SetSelectedTool("Hand");
            }
            else
                Instance.SetSelectedTool("Arrow");
            
        }
        private void workSpace_MouseLeave(object sender, MouseEventArgs e)
        {
            Instance.SetSelectedTool("Arrow");
        }
        private void workSpace_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SelectedContainer == null)
                return;

            this.SelectedContainer.ClearSelectFlowElement();
            this.flowToolBox.isToolBoxCanDrag = false;
            this.flowToolBoxIcon.isToolBoxCanDrag = false;

            string name = NewElementNameOrIcon;
            if ( !string.IsNullOrEmpty(name))
            {
                if (ToolBoxNodeIcons.IsToolDraging || ToolBox.isToolDraging)
                {
                    if (ToolBoxNodeIcons.IsToolDraging)
                    {
                        ToolBoxNodeIcons.Instance.SetDefault();
                        Point p = e.GetPosition(workSpace);
                        CreateAcitivty(FlowNodeType.Ordinary, name, p);
                    }
                    else if (ToolBox.isToolDraging)
                    {
                        ToolBox.SetDefault();
                        Point p = e.GetPosition(workSpace);
                        CreateElement(name, p);
                    }
                }
                NewElementNameOrIcon = string.Empty;
                positionDbClick = 0;
                pDbClick = new Point(260, 260);
            }
        }

        /// <summary>
        /// 在ToolBox  When you select an assignment ,在MainPage  Left mouse button Up When you use  
        /// Shape Toolbox 
        /// </summary>
        public static string NewElementNameOrIcon = string.Empty;
        Point pDbClick = new Point(260, 260);
        int positionDbClick = 0;

        /// 在Shape Double-click the toolbox when you call 
        public void AddElement(string name)
        {
            if (null == SelectedContainer)
            {
                MessageBox.Show(" Please open a design interface ");
            }
            else
            {
                positionDbClick += 10;
                Point p = new Point(pDbClick.X + positionDbClick, pDbClick.Y + positionDbClick);
                CreateElement(name, p);
            }
        }
        //  Double-click the icon in the toolbox to add call 
        public void CreateAcitivty(FlowNodeType type, string icon)
        {
            ToolBoxNodeIcons.Instance.SetDefault();
            if (null == SelectedContainer)
            {
                MessageBox.Show(" Please open a design interface ");
            }
            else
            {
                positionDbClick += 10;
                Point p  = new Point(pDbClick.X + positionDbClick, pDbClick.Y + positionDbClick);
                CreateAcitivty(type, icon, p);
            }
        }

        public void CreateElement(string name, Point p)
        {
            WorkFlowElementType type = WorkFlowElementType.Label;
            switch (name)
            {
                case "label":
                    type = WorkFlowElementType.Label;
                    break;
                case "line":
                    type = WorkFlowElementType.Direction;
                    break;
                case "lineReturn":
                    type = WorkFlowElementType.Direction;
                    break;
                case "nodeOrdinary":
                case "nodeFL":
                case "nodeHL":
                case "nodeFHL":
                case "subThread":
                    type = WorkFlowElementType.FlowNode;
                    break;
            }

            switch (type)
            {
                case WorkFlowElementType.Direction:
                    switch (name)
                    {
                        case "line":
                            AddLine( p, DirType.Forward);
                            break;
                        case "lineReturn":
                            AddLine( p, DirType.Backward);
                            break;
                    }
                    break;
                case WorkFlowElementType.FlowNode:
                    switch (name)
                    {
                        case "nodeOrdinary":
                            CreateAcitivty(FlowNodeType.Ordinary, name, p);
                            break;
                        case "nodeFL":
                            CreateAcitivty(FlowNodeType.FL, name, p);
                            break;
                        case "nodeHL":
                            CreateAcitivty(FlowNodeType.HL, name, p);
                            break;
                        case "nodeFHL":
                            CreateAcitivty(FlowNodeType.FHL, name, p);
                            break;
                        case "subThread":
                            CreateAcitivty(FlowNodeType.SubThread, name, p);
                            break;
                    }
                    break;
                case WorkFlowElementType.Label:
                    this.SelectedContainer.CreateLabel(p);
                    break;
            }
        }

        public void CreateAcitivty(FlowNodeType type, string icon, Point point)
        {
            this.SelectedContainer.CreateFlowNode(type, point, icon);
        }

        void AddLine(Point point, DirType type)
        {
            this.SelectedContainer.CreateDirection(point, type);
        }


        void ChageFlowUI(ToolbarButton btn)
        {
            if (this.SelectedContainer == null) return;

            this.save();

            FlowChartType type = FlowChartType.UnKnown;
            if (this.SelectedContainer.Flow_ChartType == FlowChartType.Shape)
            {
                type = FlowChartType.UserIcon;
            }
            else
            {
                type = FlowChartType.Shape;
            }
            this.SelectedContainer.UpdateFlow(type);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            //原因是: MainPage把keydown给拦截了,所以无法输入
            if (SelectedContainer != null)
            {
                if (SelectedContainer.IsSomeChildEditing)
                {
                    base.OnKeyDown(e);
                    return;
                }
            }
            e.Handled = true;
            base.OnKeyDown(e);

            switch (e.Key)
            {
                case Key.H:
                    if (Keyboard.Modifiers == ModifierKeys.Control || Keyboard.Modifiers == ModifierKeys.Windows)
                    {
                        BP.SL.OutputChildWindow.ShowException();
                        return;
                    }
                    break;
                case Key.S: //  Save 
                    if (Keyboard.Modifiers == ModifierKeys.Control || Keyboard.Modifiers == ModifierKeys.Windows)
                    {
                        this.save();
                        return;
                    }
                    break;
                case Key.T: if (Keyboard.Modifiers == ModifierKeys.Control || Keyboard.Modifiers == ModifierKeys.Windows)
                    {
                        new FtpFileExplorer().Show();
                        return;
                    }
                    break;
            }
           
            switch (e.Key)
            {
                case Key.Escape:
                  
                    break;
                case Key.Delete: // Delete .

                    if (MessageBox.Show(" OK to delete the selected element ", " Remove elements ", MessageBoxButton.OKCancel) == MessageBoxResult.OK
                         && this.SelectedContainer != null)
                        this.SelectedContainer.DeleteSeleted();

                    break;
                //  Up 
                case Key.W:  case Key.Up:
                   
                    break;
                //  Down 
                case Key.S:  case Key.Down:
                   
                    break;
                //  To the right 
                case Key.D:  case Key.Right:
                   
                    break;
                //  To the left .
                case Key.A:  case Key.Left:
                   
                    break;
                default:
                    break;
            }
        }

        #region  Open Form 
        CCForm.MainPage main = null;
        ContainerContent ccFormContainer = null;
        /// <summary>
        ///  Form Designer 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="para">Title,Url/Fk_MapData</param>
        public void OpenBPForm(BPFormType type, params string[] para)
        {
            if (null == para || 3 != para.Length)
                return;
            string title = para[0];

            if (Glo.UrlOrForm)
            {
                string url = para[1];

                url = Glo.BPMHost + url;
                Glo.OpenMax(url, title);
            }
            else
            {
                try
                {
                  

                    string fk_MapData = para[1], fk_Flow = para[2];
                  
                    if (main == null)
                    {
                        main = CCForm.MainPage.Instance;
                        main.Margin = new Thickness(0);
                        main.CCBPFormLoaded += () =>                        //  After loading the form callback 
                        {
                            main.Focus();
                        };
                        main.Closed += () =>
                        {
                           ccFormContainer.Close();
                        };
                    }

                    main.Load(fk_MapData, fk_Flow);

                    if (null == ccFormContainer)
                    {
                        ccFormContainer = new ContainerContent();
                        ccFormContainer.VerticalAlignment = VerticalAlignment.Top;
                        ccFormContainer.VerticalContentAlignment = System.Windows.VerticalAlignment.Top;
                        ccFormContainer.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                        ccFormContainer.Margin = new Thickness() { Left = 0, Right = 0, Bottom = 0, Top = 0 };
                        ccFormContainer.content.SizeChanged += (object sender, SizeChangedEventArgs e) =>
                        {
                            main.Width = ccFormContainer.content.ActualWidth - 10;
                            main.Height = ccFormContainer.content.ActualHeight - 35;// TitleBar
                        };
                    }
                   
                    ccFormContainer.content.Width = App.Current.Host.Content.ActualWidth;
                    ccFormContainer.content.Height = App.Current.Host.Content.ActualHeight;
                    ccFormContainer.Title = title;
                    ccFormContainer.content.Content = main;
                    ccFormContainer.Show();
                }
                catch (Exception e)
                {
                    BP.Glo.ShowException(e);
                }
            }
        }

        #endregion

        /// <summary>
        /// Asp.net Event to be executed when you close the page 
        /// </summary>
        [ScriptableMember]
        public void OnAspNetPageClosed(string type, string para)
        {
            switch (type)
            {
                case "FlowNodeProperty":
                    var paras = para.Split(','); //id,namevalue

                    foreach (TabItem t in tbDesigner.Items)
                    {
                        Container ct = t.Content as Container;

                        foreach (var flowNode in ct.FlowNodeCollections)
                        {
                            if (paras[0] == flowNode.NodeID && paras[0] != flowNode.NodeName)
                            {
                                ct.IsNeedSave = true;
                                flowNode.NodeName = paras[1];
                                break;
                            }
                        }
                    }

                    break;

                case "FlowProperty":
                    paras = para.Split(','); //id,namevalue
                    var oldValue = string.Empty;
                    int index = 0;

                    foreach (TabItem t in tbDesigner.Items)
                    {
                        Container ct = t.Content as Container;
                        oldValue = (t as TabItemEx).Title;
                        if (paras[0] == ct.FlowID && paras[1] != oldValue)
                        {
                            var tabItem = tbDesigner.Items[index] as TabItemEx;
                            Button btnClose = new Button();
                            btnClose.Opacity = 0.2;
                            btnClose.Content = "╳";
                            btnClose.MinHeight = 15;
                            btnClose.MinWidth = 17;
                            btnClose.Click += new RoutedEventHandler(btnClose_Click);
                            btnClose.ClickMode = ClickMode.Release;
                            TextBlock tbx = new TextBlock();
                            tbx.Text = paras[1];
                            tbx.Width = paras[1].Length;
                            Canvas cs = new Canvas();
                            cs.Children.Add(btnClose);
                            cs.Children.Add(tbx);
                            cs.Height = 20;
                            tabItem.Title = paras[1];
                            tabItem.Width = tbx.ActualWidth + btnClose.ActualWidth + 30;
                            tabItem.Header = cs;
                            btnClose.DataContext = tabItem;
                            btnClose.SetValue(Canvas.LeftProperty, tabItem.Width - btnClose.ActualWidth - 20);
                            btnClose.SetValue(Canvas.TopProperty, 0.0);
                            tbx.SetValue(Canvas.TopProperty, btnClose.ActualHeight);
                            tbx.SetValue(Canvas.LeftProperty, 0.0);
                            tabItem.SetValue(TabControl.WidthProperty, tbx.ActualWidth + btnClose.ActualWidth + 40);

                            tabItem.VerticalAlignment = VerticalAlignment.Top;
                            tabItem.VerticalContentAlignment = VerticalAlignment.Top;
                            break;

                        }
                        index++;
                    }

                    this.BindFlowAndFlowSort();
                    break;

            }
        }

        [ScriptableMember]
        public void OnShortKey(string type)
        {
            switch (type)
            {
                case "Save":
                        if (main != null)
                            main.Save();
                        else
                            this.save();
                    break;
                default:
                    break;
            }

        }

    }
}
