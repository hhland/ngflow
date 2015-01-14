#region
using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BP.En;
using BP.SL;
using BP.Sys.SL;
using Liquid;
using Silverlight;
#endregion

namespace CCForm
{
    public delegate void CCBPFormClosed();
    public delegate void CCBPFormLoaded();
    public partial class MainPage:UserControl
    {
        static MainPage instance = null;
        public static MainPage Instance
        {
            get 
            {
                if (instance == null)
                    instance = new MainPage();
                return MainPage.instance;
            }
            set
            { 
                MainPage.instance = value;
            }
        }
        public event CCBPFormClosed Closed;
        /// <summary>
        ///  If you are from CCFlowDesigner Application into this page , When the form is loaded after the need to update the caller callback 
        /// </summary>
        public event CCBPFormLoaded CCBPFormLoaded;
        #region  Mobile processing variables 

        private Point pFrom;  /*  Click the mouse position    */
        public static Rectangle RectSelected;   /*   Region selection   */

        private enum StateRectangleSelected { SelectBegin, SelectComplete, SelectMoved, SelectDisposed }
        StateRectangleSelected selectState = StateRectangleSelected.SelectDisposed;
        private bool isDrawingLine;// selectedType =  Line ,leftDown-->leftUp……selected-->  selectedType !=  Line ,
        private bool isToolDraging;
        private List<FrameworkElement> selectedElements = new List<FrameworkElement>();

        #endregion

        #region  Global Variables 
        LoadingWindow loadingWindow = new LoadingWindow();

        
        public SelectM2M winSelectM2M = new SelectM2M();
        public FlowFrm winFlowFrm = new FlowFrm();
        public FrmLink winFrmLink = new FrmLink();
        public FrmLab winFrmLab = new FrmLab();
        public SelectTB winSelectTB = new SelectTB();
        public SelectDDLTable winSelectDDL = new SelectDDLTable();
        public SelectRB winSelectRB = new SelectRB();
        public FrmImp winFrmImp = new FrmImp();
        public FrmBtn winFrmBtn = new FrmBtn();

        public FrmOp winFrmOp = new FrmOp();
        public FrmImg winFrmImg = new FrmImg();
        public FrmImgAth winFrmImgAth = new FrmImgAth();
        public FrmImgSeal winFrmImgSeal = new FrmImgSeal();
        public FrmWorkCheck winWorkCheck = new FrmWorkCheck();
        public FrmEle winFrmEle = new FrmEle();

        public NodeFrms winNodeFrms = new NodeFrms();
        public SelectAttachment winSelectAttachment = new SelectAttachment();
        public FrmAttachmentM winFrmAttachmentM = new FrmAttachmentM();

        public bool IsRB ;


        string selectType = ToolBox.Mouse; //  Select the type of the current tool  hand line1 line2 label txt cannel

        BPLabel currLab; // Current  label
        BPLink currLink;  // Current  linke
        BPLine currLine;  // Current  Line
     
       

        private DataTemplate cursor;// customerCursor
        private CustomCursor cCursor = null;// which is used to replace the default cursor of UIElement

        //  Element edit form name 
        const string NameImg = "NameImg",
               NameImgAth = "NameImgAth",
               NameImgSeal = "NameImgSeal",
               NameWorkCheck = "NameWorkCheck",
               NameLab = "NameLab",
               NameM2M = "NameM2M",
               NameLink = "NameLink",
               NameTB = "NameTB",
               NameDDL = "NameDDL",
               NameRB = "NameRB",
               NameImp = "NameImp",
               NameEle = "NameEle",
               NameAttachment = "NameAttachment",
               NameOp = "NameOP",
               NameBtn = "NameBtn",
               NameFlowFrm = "NameFlowFrm",
               NameNodeFrms = "NameNodeFrms",
               NameAttachmentM = "NameAttachmentM";
        #endregion  Global Variables 

        #region  Initialization load 
        /// <summary>
        ///  Flag this application caller  true: 由web Calling ,false: 由FlowDesigner Calling 
        /// </summary>
        bool LoadSource;
        void Content_Resized(object sender, EventArgs e)
        {
            if (!LoadSource)
            {
                
            }
            else
            {
                this.LayoutRoot.Width = Application.Current.Host.Content.ActualWidth;
                this.LayoutRoot.Height = Application.Current.Host.Content.ActualHeight;
            }
        }
        void _SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender == this.workSpace)
            {
                #region
                this.workSpace.Visibility = System.Windows.Visibility.Collapsed;

                var hostWidth = this.LayoutRoot.ActualWidth - this.lbTools.ActualWidth;
                var hostHeight = this.LayoutRoot.ActualHeight - this.toolbar1.ActualHeight;

                if (workSpace.ActualWidth > hostWidth)
                {
                    this.svWorkSpace.Width = hostWidth;
                }
                else
                {
                    this.svWorkSpace.Width = workSpace.ActualWidth;
                }

                if (hostHeight < workSpace.ActualHeight)
                {
                    svWorkSpace.Height = hostHeight;
                }
                else
                {
                    svWorkSpace.Height = workSpace.ActualHeight;
                }

                this.SetGridLines(this.workSpace, true); // Re-draw the line 
                this.workSpace.Visibility = System.Windows.Visibility.Visible;
                #endregion
            }
            else if (sender == this.LayoutRoot)
            {
                #region
                if (LayoutRoot.Height < 100) return;
                this.Visibility = System.Windows.Visibility.Collapsed;

                var containerWidth = LayoutRoot.Width;

                double left = 180;
                double width = containerWidth - left;
                if (width < workSpace.ActualWidth)
                {
                    this.svWorkSpace.Width = width > 0 ? width : this.svWorkSpace.Width;
                }
                else
                {
                    this.svWorkSpace.Width = workSpace.ActualWidth;
                }

                double containerHeight = LayoutRoot.Height;

                //double top =  this.toolbar1.ActualHeight == 0 || double.IsNaN(this.toolbar1.ActualHeight) ? 35 : this.toolbar1.ActualHeight;
                double height = containerHeight - 35;

                if (0 < height)
                {
                    this.lbTools.Height = height;

                    this.toolbar1.Height = this.toolbar1.ActualHeight;
                    this.workSpace.Height = this.workSpace.ActualHeight;
                    if (height < workSpace.ActualHeight)
                    {
                        svWorkSpace.Height = height > 0 ? height : svWorkSpace.Height;
                    }
                    else
                    {
                        svWorkSpace.Height = workSpace.ActualHeight;
                    }
                }
                this.Visibility = System.Windows.Visibility.Visible;
                #endregion
            }
            else if (sender == this)
            {
                if (LoadSource)
                {
                    this.LayoutRoot.Width = this.ActualWidth;
                    this.LayoutRoot.Height = this.ActualHeight;
                }
                else
                {
                    this.LayoutRoot.Width = this.ActualWidth;
                    this.LayoutRoot.Height = this.ActualHeight - 10;
                }
            }
        }

        private MainPage()
        {
            InitializeComponent();

            //  Obtaining property by calling the service parameters 
            if (string.IsNullOrEmpty(Glo.AppCenterDBType))
            {
                FF.CCFormSoapClient da = Glo.GetCCFormSoapClientServiceInstance();
                da.CfgKeyAsync("AppCenterDBType");
                da.CfgKeyCompleted += (object sender, FF.CfgKeyCompletedEventArgs e) =>
                {
                    if (null != e.Error || string.IsNullOrEmpty(e.Result))
                    {
                        MessageBox.Show(" Please check the configuration section AppCenterDBType");
                        Glo.AppCenterDBType = "MSSQL";
                    }
                    else
                        Glo.AppCenterDBType = e.Result;
                };
            }

            if (string.IsNullOrEmpty(Glo.CompanyID))
            {
                FF.CCFormSoapClient da = Glo.GetCCFormSoapClientServiceInstance();

                da.CfgKeyAsync("CompanyID");
                da.CfgKeyCompleted += (object s, FF.CfgKeyCompletedEventArgs ee) =>
                {
                    if (null != ee.Error || string.IsNullOrEmpty(ee.Result))
                    {
                        MessageBox.Show(" Please check the configuration section CompanyID");
                        Glo.CompanyID = "CCFlow";
                    }
                    else
                        Glo.CompanyID = ee.Result;
                };
            }

            #region toolbar 
            List<Func> ens = new List<Func>();
            ens = Func.instance.GetToolList();
            foreach (Func en in ens)
            {
                Image img = new Image()
                {
                    Width = 13,
                    Height = 13
                    ,
                    Source = new BitmapImage(new Uri("/CCFormDesigner;component/Img/" + en.No + ".png", UriKind.Relative))
                };

                TextBlock tb = new TextBlock()
                {
                    Name = "tbT" + en.No,
                    Text = en.Name + " ",
                    FontSize = 13
                };

                StackPanel mysp = new StackPanel()
                {
                    Name = "sp" + en.No,
                    Orientation = Orientation.Horizontal,
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    //RenderTransform = new CompositeTransform() { TranslateX = 0 },

                };
                mysp.Children.Add(img);
                mysp.Children.Add(tb);

                Toolbar.ToolbarButton btn = new Toolbar.ToolbarButton()
                {
                    Name = "Btn_" + en.No,
                    Tag = en.No,
                    Content = mysp
                };
                btn.Click += new RoutedEventHandler(ToolBar_Click);

                this.toolbar1.AddBtn(btn);
            }

            List<EleFunc> ensEle = new List<EleFunc>();
            ensEle = EleFunc.instance.getToolList();
            foreach (EleFunc en in ensEle)
            {

                Image img = new Image();
                BitmapImage png = new BitmapImage(new Uri("/CCFormDesigner;component/Img/" + en.No + ".png", UriKind.Relative));
                img.Source = png;

                TextBlock tb = new TextBlock();
                tb.Name = "tbT" + en.No;
                tb.Text = en.Name + " ";
                tb.FontSize = 15;
                StackPanel mysp = new StackPanel();
                mysp.Children.Add(img);
                mysp.Children.Add(tb);


                Toolbar.ToolbarBtn btn = new Toolbar.ToolbarBtn();
                btn.Name = "Btn_" + en.No;
                btn.Tag = en.No;
                btn.Click += new RoutedEventHandler(ToolBar_Click);
                btn.Content = mysp;
                this.toolbar1.AddBtn(btn);
            }


            #endregion

            #region  Toolbox 
            this.lbTools.ItemsSource = ToolBoxes.instance.GetToolBoxList();
            this.lbTools.SelectionMode = SelectionMode.Single;
            this.lbTools.AddHandler(ListBox.MouseLeftButtonDownEvent, new MouseButtonEventHandler(lbTools_MouseLeftButtonDown), true);
            this.lbTools.MouseLeftButtonUp += lbTools_MouseLeftButtonUp;
            this.lbTools.MouseRightButtonDown += UIElement_MouseRightButtonDown;
            #endregion

            #region chinwin
            winFrmImg.Name           = NameImg;
            winFrmImgAth.Name        = NameImgAth;
            winFrmImgSeal.Name       = NameImgSeal;
            winWorkCheck.Name        = NameWorkCheck;
            winFrmLab.Name           = NameLab;
            winSelectM2M.Name        = NameM2M;
            winFrmLink.Name          = NameLink;
            winSelectTB.Name         = NameTB;
            winSelectDDL.Name        = NameDDL;
            winSelectRB.Name         = NameRB;
            winFrmImp.Name           = NameImp;
            winFrmEle.Name           = NameEle;
            winSelectAttachment.Name = NameAttachment;
            winFrmOp.Name            = NameOp;
            winFrmBtn.Name           = NameBtn;
            winFlowFrm.Name          = NameFlowFrm;
            winNodeFrms.Name         = NameNodeFrms;
            winFrmAttachmentM.Name   = NameAttachmentM;

            winFrmImg.Closed           += WindowDilag_Closed;
            winFrmImgAth.Closed        += WindowDilag_Closed;
            winFrmImgSeal.Closed       += WindowDilag_Closed;
            winWorkCheck.Closed        += WindowDilag_Closed;
            winFrmLab.Closed           += WindowDilag_Closed;
            winFrmLink.Closed          += WindowDilag_Closed;
            winNodeFrms.Closed         += WindowDilag_Closed;
            winSelectTB.Closed         += WindowDilag_Closed;
            winSelectDDL.Closed        += WindowDilag_Closed;
            winSelectRB.Closed         += WindowDilag_Closed;
            winFrmImp.Closed           += WindowDilag_Closed;
            winFrmEle.Closed           += WindowDilag_Closed;
            winFrmBtn.Closed           += WindowDilag_Closed;
            winSelectAttachment.Closed += WindowDilag_Closed;
            winFrmOp.Closed            += WindowDilag_Closed;
            winFlowFrm.Closed          += WindowDilag_Closed;
            winFrmAttachmentM.Closed   += WindowDilag_Closed;
            winSelectM2M.Closed        += WindowDilag_Closed;
            #endregion chinwin.

            initPoint();

            #region eventHandler
            cCursor = new CustomCursor(this);

            this.MouseRightButtonDown += UIElement_MouseRightButtonDown;
            this.svWorkSpace.MouseRightButtonDown += UIElement_MouseRightButtonDown;
            this.workSpace.MouseLeftButtonDown += workSpace_MouseLeftButtonDown;
            this.workSpace.MouseLeftButtonUp += workSpace_MouseLeftButtonUp;
            this.workSpace.MouseMove += workSpace_MouseMove;
            this.workSpace.MouseRightButtonDown += UIElement_MouseRightButtonDown;
            this.workSpace.MouseEnter += workSpace_MouseEnter;
            this.workSpace.MouseLeave += workSpace_MouseLeave;

            this.SizeChanged += _SizeChanged;
            this.LayoutRoot.SizeChanged += _SizeChanged;
            this.workSpace.SizeChanged += _SizeChanged;
            Application.Current.Host.Content.Resized += Content_Resized;
            #endregion
          
        }


        /// <summary>
        ///  Direct access by the browser 
        /// </summary>
        public void Load(bool loadSource = false)
        {
            LoadSource = loadSource;

            if (!Glo.FK_MapDataNotFind.Equals(Glo.FK_MapData))
            {
                this.BindFrm();
            }
        }

        /// <summary>
        /// 由CCFlowDesigner Enter 
        /// </summary>
        /// <param name="fk_mapdata"> Form identification field </param>
        public void Load(string fk_mapdatam, string fk_flow)
        {
            if (string.IsNullOrEmpty(fk_mapdatam))
                throw new Exception(" Form does not allow identification field empty ");

            Glo.FK_MapData = fk_mapdatam;
            Glo.FK_Flow = fk_flow;

            Load(true);
        }
        #endregion

        void WindowDilag_Closed(object sender, EventArgs e)
        {
            this.SetSelectedTool(ToolBox.Mouse);

            ChildWindow c = sender as ChildWindow;
            if ( c.DialogResult == false)
            {
                  return;
            }

            switch (c.Name)
            {
                case NameImg:

                    Glo.currEle =this.winFrmImg.HisImg;
                    break;
                case NameImgAth:
                    Glo.currEle =this.winFrmImgAth.HisImgAth;
                    break;
                case NameM2M:
                  
                    BPM2M m2m = new BPM2M(this.winSelectM2M.IsM2M);
                    m2m.Name = Glo.TempVal.ToString();
                    if (this.workSpace.FindName(m2m.Name) != null)
                    {
                        MessageBox.Show(" Object already exists :" + m2m.Name);
                        break;
                    }

                    m2m.SetValue(Canvas.LeftProperty, Glo.X);
                    m2m.SetValue(Canvas.TopProperty, Glo.Y);

                    attachElementEvent(m2m);
                    Glo.OpenM2M(Glo.FK_MapData, m2m.Name + Glo.TimeKey);
                    break;

                case NameLab:

                    this.currLab.Content = this.winFrmLab.TB_Text.Text.Replace("@", "\n");
                    int size = this.winFrmLab.DDL_FrontSize.SelectedIndex + 6;
                    this.currLab.FontSize = double.Parse(size.ToString());
                    break;
                case NameLink:

                    this.currLink.Content = this.winFrmLink.TB_Text.Text.Replace("@", "\n");
                    this.currLink.WinTarget = this.winFrmLink.TB_WinName.Text;
                    this.currLink.URL = this.winFrmLink.TB_URL.Text;
                    size = this.winFrmLink.DDL_FrontSize.SelectedIndex + 6;
                    this.currLink.FontSize = double.Parse(size.ToString());
                    break;
                case NameTB:

                    #region  Increase audit group .
                    if (this.winSelectTB.IsCheckGroup == true)
                    {
                        if (Glo.X > 300)
                            Glo.X = 300;

                        /* If the audit is to increase the grouping .*/
                        string gName = this.winSelectTB.TB_Name.Text;
                        string gKey = this.winSelectTB.TB_KeyOfEn.Text;
                        this.SetSelectedTool(ToolBox.Mouse);
                        FF.CCFormSoapClient daCreateCheckGroup = Glo.GetCCFormSoapClientServiceInstance();
                        daCreateCheckGroup.DoTypeAsync("CreateCheckGroup", gKey, gName, Glo.FK_MapData, null, null, null);
                        daCreateCheckGroup.DoTypeCompleted += new EventHandler<FF.DoTypeCompletedEventArgs>(daCreateCheckGroup_DoTypeCompleted);
                        return;
                    }
                    #endregion

                    TBType tp = TBType.String;
                    if (winSelectTB.RB_String.IsChecked == true)
                        tp = TBType.String;

                    if (winSelectTB.RB_Money.IsChecked == true)
                        tp = TBType.Money;
                    if (winSelectTB.RB_Int.IsChecked == true)
                        tp = TBType.Int;
                    if (winSelectTB.RB_Float.IsChecked == true)
                        tp = TBType.Float;

                    if (winSelectTB.RB_DataTime.IsChecked == true)
                        tp = TBType.DateTime;

                    if (winSelectTB.RB_Data.IsChecked == true)
                        tp = TBType.Date;

                    if (winSelectTB.RB_Boolen.IsChecked == true)
                    {
                        /*  In the case of boolen  Type . */
                        BPCheckBox cb = new BPCheckBox();
                        cb.Name = this.winSelectTB.TB_KeyOfEn.Text.Trim();
                        cb.Content = this.winSelectTB.TB_Name.Text.Trim();
                        cb.KeyName = cb.Content.ToString().Trim();

                        Label cbLab = new Label();
                        cbLab.Name = "CBLab" + cb.Name;
                        cbLab.Content = this.winSelectTB.TB_Name.Text.Trim();
                        cbLab.Tag = cb.Name.Trim();
                        cb.Content = cbLab;
                        cb.SetValue(Canvas.LeftProperty, Glo.X);
                        cb.SetValue(Canvas.TopProperty, Glo.Y);


                        this.attachElementEvent(cb);
                        return;
                    }

                    BPTextBox mytb = new BPTextBox(tp, this.winSelectTB.TB_KeyOfEn.Text);
                    mytb.KeyName = this.winSelectTB.TB_Name.Text.Trim();
                    // mytb.Name = this.winSelectTB.TB_Name;
                    mytb.SetValue(Canvas.LeftProperty, Glo.X);
                    mytb.SetValue(Canvas.TopProperty, Glo.Y);


                    //  Check for generation   Label .
                    if (this.attachElementEvent(mytb) 
                        && this.winSelectTB.CB_IsGenerLabel.IsChecked == true)
                    {
                        BPLabel lab = new BPLabel();
                        lab.Content = this.winSelectTB.TB_Name.Text.Trim();
                        lab.Cursor = Cursors.Hand;
                        lab.SetValue(Canvas.LeftProperty, Glo.X - 20);
                        lab.SetValue(Canvas.TopProperty, Glo.Y);
                       
                        this.attachElementEvent(lab);
                    }

                    break;

                case NameBtn:
                    BPBtn btn = this.winFrmBtn.HisBtn;
                    if (this.workSpace.Children.Contains(btn))
                    {
                        BPBtn mybtn = (BPBtn)this.workSpace.FindName(btn.Name.Trim());
                        mybtn = btn;
                    }
                    else
                    {
                        btn.SetValue(Canvas.LeftProperty, Glo.X);
                        btn.SetValue(Canvas.TopProperty, Glo.Y);

                        this.attachElementEvent(btn);
                    }
                    break;

                case NameEle:
                    BPEle ele = this.winFrmEle.HisEle;
                    BPEle myEle = this.workSpace.FindName(ele.Name.Trim()) as BPEle;
                    if (myEle != null)
                    {
                        myEle = ele;
                    }
                    else
                    {
                        ele.SetValue(Canvas.LeftProperty, Glo.X);
                        ele.SetValue(Canvas.TopProperty, Glo.Y);
                        this.attachElementEvent(ele);
                    }
                    break;
                case NameAttachmentM:
                    BPAttachmentM atth = this.winFrmAttachmentM.HisBPAttachment;
                    atth.Label = this.winFrmAttachmentM.TB_Name.Text.Trim();
                    atth.SetValue(Canvas.LeftProperty, Glo.X);
                    atth.SetValue(Canvas.TopProperty, Glo.Y);

                    this.attachElementEvent(atth);
                    break;
                case NameAttachment:

                    #region
                    BPAttachment atthMy = new BPAttachment(
                        this.winSelectAttachment.TB_No.Text.Trim() , 
                        this.winSelectAttachment.TB_Name.Text.Trim(),
                        this.winSelectAttachment.TB_Exts.Text, 
                        70,
                        this.winSelectAttachment.TB_SaveTo.Text);
                    atthMy.SetValue(Canvas.LeftProperty, Glo.X);
                    atthMy.SetValue(Canvas.TopProperty, Glo.Y);
                    atthMy.X = Glo.X;
                    atthMy.Y = Glo.Y;
                    atthMy.IsUpload = (bool)this.winSelectAttachment.CB_IsUpload.IsChecked;
                    atthMy.IsDelete = (bool)this.winSelectAttachment.CB_IsDelete.IsChecked;
                    atthMy.IsDownload = (bool)this.winSelectAttachment.CB_IsDownload.IsChecked;


                    this.attachElementEvent(atthMy);

                    /* Generate labels */
                    BPLabel lb = new BPLabel();
                    lb.Content = this.winSelectAttachment.TB_Name.Text;
                    lb.Name = getElementNameFromUI( lb);
                    lb.Cursor = Cursors.Hand;
                    lb.SetValue(Canvas.LeftProperty, Glo.X - 20);
                    lb.SetValue(Canvas.TopProperty, Glo.Y);

                    this.attachElementEvent(lb);
                    #endregion
                    break;
                case NameDDL:

                    if (this.winSelectDDL.listBox1.SelectedIndex < 0)
                        break;

                    #region
                    ListBoxItem mylbi = this.winSelectDDL.listBox1.SelectedItem as ListBoxItem;
                    string enKey = mylbi.Content.ToString();
                    enKey = enKey.Substring(0, enKey.IndexOf(':'));

                    BPDDL myddl = new BPDDL() 
                    {
                        KeyName = this.winSelectDDL.TB_KeyOfName.Text.Trim(),
                        Name = this.winSelectDDL.TB_KeyOfEn.Text.Trim(),
                        Width = 100,
                        Height = 23
                    };
                  
                    myddl.SetValue(Canvas.LeftProperty, Glo.X);
                    myddl.SetValue(Canvas.TopProperty, Glo.Y);
                    myddl.BindEns(enKey);

                    this.attachElementEvent(myddl);

                    //  Check for generation   Label .
                    if (this.winSelectDDL.CB_IsGenerLab.IsChecked == true)
                    {
                        BPLabel lab = new BPLabel();
                        lab.Content = this.winSelectDDL.TB_KeyOfName.Text.Trim();
                        lab.SetValue(Canvas.LeftProperty, Glo.X - 20);
                        lab.SetValue(Canvas.TopProperty, Glo.Y);

                        this.attachElementEvent(lab);
                    }
                    #endregion
                    break;

                case NameRB:
                    if (this.winSelectRB.listBox1.SelectedIndex < 0)
                        break;

                    #region

                    ListBoxItem lbi = this.winSelectRB.listBox1.SelectedItem as ListBoxItem;
                    string enumKey = lbi.Content.ToString();
                    enumKey = enumKey.Substring(0, enumKey.IndexOf(':'));

                    string cfgKeys = lbi.Tag as string;
                    string[] strs = cfgKeys.Split('@');
                    if (IsRB)
                    {
                        int addX = 0;
                        int addY = 0;
                        string gName = this.winSelectRB.TB_KeyOfEn.Text.Trim(); 
                        foreach (string str in strs)
                        {
                            if (string.IsNullOrEmpty(str))
                                continue;

                            string[] mykey = str.Split('=');
                            BPRadioBtn rb = new BPRadioBtn();
                            rb.KeyName = this.winSelectRB.TB_KeyOfName.Text.Trim();
                            rb.Content = mykey[1];
                            rb.Tag = mykey[0];
                            rb.Name = Glo.FK_MapData + "_" + gName + "_" + mykey[0];
                            rb.UIBindKey = enumKey;
                            rb.SetValue(Canvas.LeftProperty, Glo.X + addX);
                            rb.SetValue(Canvas.TopProperty, Glo.Y + addY);
                            rb.GroupName = gName;
                            addY += 16;

                            this.attachElementEvent(rb);
                        }

                        //  Check for generation   Label .
                        if (this.winSelectRB.CB_IsGenerLab.IsChecked == true)
                        {
                            BPLabel lab = new BPLabel();
                            lab.Content = this.winSelectRB.TB_KeyOfName.Text.Trim();
                            lab.SetValue(Canvas.LeftProperty, Glo.X - 20);
                            lab.SetValue(Canvas.TopProperty, Glo.Y);

                            this.attachElementEvent(lab);
                        }
                    }
                    else
                    {
                        /*  In the case of ddl.*/
                        BPDDL myddlEnum = new BPDDL() 
                        {
                            Name = this.winSelectRB.TB_KeyOfEn.Text.Trim(),
                            KeyName = this.winSelectRB.TB_KeyOfName.Text.Trim(),
                            Width = 100,
                            Height = 23
                        };
                       
                        myddlEnum.SetValue(Canvas.LeftProperty, Glo.X);
                        myddlEnum.SetValue(Canvas.TopProperty, Glo.Y);
                        myddlEnum.BindEnum(enumKey);

                        this.attachElementEvent(myddlEnum);

                        //  Check for generation   Label .
                        if (this.winSelectRB.CB_IsGenerLab.IsChecked == true)
                        {
                            /* To generate a label */
                            BPLabel lab = new BPLabel();
                            lab.Content = this.winSelectRB.TB_KeyOfName.Text.Trim();
                            //lab.Width = 100;
                            //lab.Height = 23;
                            lab.SetValue(Canvas.LeftProperty, Glo.X - 20);
                            lab.SetValue(Canvas.TopProperty, Glo.Y);

                            this.attachElementEvent(lab);
                        }
                    }
                    #endregion
                    break;
                 /*  
                 * Property
                 */
                case NameFlowFrm:

                    Glo.FK_MapData = this.winFlowFrm.TB_No.Text;
                    this.BindTreeView();
                    break;
                case NameNodeFrms:

                    this.BindTreeView();
                    break;
                case NameImp:

                    this.BindFrm();
                    break;
                case NameOp:
                    this.changeFormSize(double.Parse(this.winFrmOp.TB_FrmW.Text), double.Parse(this.winFrmOp.TB_FrmH.Text));
                    break;
               
            }
        }

        //  Increase audit group 
        void daCreateCheckGroup_DoTypeCompleted(object sender, FF.DoTypeCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                MessageBox.Show(e.Result);
                return;
            }

            if (Glo.X > 300)
                Glo.X = 300;

            /* If the audit is to increase the grouping .*/
            string gName = this.winSelectTB.TB_Name.Text;
            string gKey = this.winSelectTB.TB_KeyOfEn.Text;


            BPTextBox tbNote = new BPTextBox(TBType.String, gKey + "_Note") 
            {
                KeyName = gName,
                Cursor = Cursors.Hand,
                Width = 550,
                Height = 70
            };
        
            tbNote.SetValue(Canvas.LeftProperty, Glo.X - 10);
            tbNote.SetValue(Canvas.TopProperty, Glo.Y);
       

            this.attachElementEvent(tbNote);

            BPTextBox tbChecker = new BPTextBox(TBType.String, gKey + "_Checker");
            tbChecker.KeyName = " Reviewer ";
            tbChecker.SetValue(Canvas.LeftProperty, Glo.X + 80);
            tbChecker.SetValue(Canvas.TopProperty, Glo.Y + 75);

            this.attachElementEvent(tbChecker);

            BPTextBox tbRDT = new BPTextBox(TBType.DateTime, gKey + "_RDT");
            tbRDT.KeyName = " Review time ";
            tbRDT.SetValue(Canvas.LeftProperty, Glo.X + 320);
            tbRDT.SetValue(Canvas.TopProperty, Glo.Y + 75);

            this.attachElementEvent(tbRDT);

            /* To generate a label */
            BPLabel abCheckNote = new BPLabel();
            abCheckNote.Content = gName.Replace(" Audit opinion ","@ Audit opinion "); // " Audit opinion ";
            abCheckNote.Name = "Lab" + gKey + "Note";
            abCheckNote.SetValue(Canvas.LeftProperty, Glo.X - 30);
            abCheckNote.SetValue(Canvas.TopProperty, Glo.Y);

            this.attachElementEvent(abCheckNote);

            BPLabel labChecker = new BPLabel();
            labChecker.Content = " Reviewer ";
            labChecker.Name = "Lab" + gKey + "Checker";
            labChecker.SetValue(Canvas.LeftProperty, Glo.X + 40);
            labChecker.SetValue(Canvas.TopProperty, Glo.Y + 75);

            this.attachElementEvent(labChecker);

            BPLabel abCheckRDT = new BPLabel();
            abCheckRDT.Content = " Date ";
            abCheckRDT.Name = "Lab" + gKey + "RDT";
            abCheckRDT.SetValue(Canvas.LeftProperty, Glo.X + 290);
            abCheckRDT.SetValue(Canvas.TopProperty, Glo.Y + 75);

            this.attachElementEvent(abCheckRDT);
        }

        JsonObject jsonObject = null;
      
        public void BindFrm()
        {
            this.workSpace.Children.Clear();
            this.Cursor = Cursors.Wait;
            try
            {
                FF.CCFormSoapClient da = Glo.GetCCFormSoapClientServiceInstance();
                da.GenerFrmAsync(Glo.FK_MapData, 0);
                da.GenerFrmCompleted += new EventHandler<FF.GenerFrmCompletedEventArgs>(
                (object sender, FF.GenerFrmCompletedEventArgs e) =>
                {
                    if (e.Error != null)
                    {
                        this.Cursor = Cursors.Arrow;
                        BP.SL.LoggerHelper.Write(e.Error);
                        MessageBox.Show(" Form data request service error :" + e.Error.Message);
                        return;
                    }
                    else
                    {
                        OpenFormJson(e.Result);
                    }

                    this.SetSelectedTool(ToolBox.Mouse);
                    HtmlPage.Plugin.Focus();
                    this.Focus();
                });
            }
            catch (Exception e)
            {
                MessageBox.Show(" Form data request service error :" + e.Message);
            }
        }
        void OpenFormJson(string strs)
        {

            bool toBeContinued = true;
            if (string.IsNullOrEmpty(strs) || strs.Length < 200)
            {
                loadingWindow.DialogResult = false;
                strs = string.IsNullOrEmpty(strs) ? " Data is empty " : strs;
                toBeContinued = false;
            }

            if (toBeContinued)
            {
                string table = "";
                try
                {
                    jsonObject = (JsonObject)JsonObject.Parse(strs);

                    if (jsonObject == null || jsonObject.Count == 0) toBeContinued = false;
                    #region
                   
                    if(toBeContinued)
                    foreach (KeyValuePair<string, JsonValue> item in jsonObject)
                    {
                        table = item.Key;
                        Glo.TempVal = table;

                        string tmpStr = string.Empty;
                        double tmpDouble = 0;
                        switch (table)
                        {
                            case EEleTableNames.WF_Node:
                                foreach (JsonValue dr in item.Value)
                                {
                                    if (dr.Count == 0)
                                        continue;

                                    tmpStr = dr["NODEID"].ToString();
                                    BPWorkCheck dtl = new BPWorkCheck(tmpStr);

                                    tmpDouble = dr["FWC_X"];
                                    dtl.SetValue(Canvas.LeftProperty, tmpDouble);
                                    tmpDouble = dr["FWC_Y"];
                                    dtl.SetValue(Canvas.TopProperty, tmpDouble);
                                    tmpDouble = dr["FWC_W"];
                                    dtl.Width = tmpDouble;
                                    tmpDouble = dr["FWC_H"];
                                    dtl.Height = tmpDouble;

                                    tmpStr = dr["FWCSTA"].ToString();
                                    dtl.FWC_Sta = string.IsNullOrEmpty(tmpStr) ? "0" : tmpStr;
                                    tmpStr = dr["FWCTYPE"].ToString();
                                    dtl.FWC_Type = string.IsNullOrEmpty(tmpStr) ? "0" : tmpStr;
                                    attachElementEvent(dtl);
                                }
                                break;
                            case EEleTableNames.Sys_FrmEle:
                                foreach (JsonValue dr in item.Value)
                                {
                                    if (dr.Count == 0)
                                        continue;

                                    if ((string)dr["FK_MAPDATA"] != Glo.FK_MapData)
                                        continue;

                                    BPEle bpele = new BPEle();
                                    bpele.Name = dr["MYPK"];

                                    if (string.IsNullOrEmpty((string)dr["ELETYPE"]))
                                        continue;

                                    if (string.IsNullOrEmpty((string)dr["ELEID"]))
                                        continue;

                                    if (string.IsNullOrEmpty(dr["ELENAME"]))
                                        continue;

                                    bpele.EleType = (string)dr["ELETYPE"]  ;
                                    bpele.EleName = dr["ELENAME"];
                                    bpele.EleID = (string)dr["ELEID"];

                                    tmpDouble = dr["X"];
                                    bpele.SetValue(Canvas.LeftProperty, tmpDouble);
                                    tmpDouble = dr["Y"];
                                    bpele.SetValue(Canvas.TopProperty, tmpDouble);

                                    bpele.Width = dr["W"];
                                    bpele.Height = dr["H"];

                                    attachElementEvent(bpele);
                                }
                                continue;
                            case EEleTableNames.Sys_MapData:

                                foreach (JsonValue dr in item.Value)
                                {
                                    if (dr.Count == 0)
                                        continue;

                                    if ((string)dr["NO"] != Glo.FK_MapData)
                                        continue;
                                  
                                    Glo.HisMapData = new MapData();
                                    Glo.HisMapData.FrmH = dr["FRMH"];
                                    Glo.HisMapData.FrmW = dr["FRMW"];
                                    Glo.HisMapData.No = dr["NO"];
                                    Glo.HisMapData.Name = dr["NAME"];
                                    Glo.IsDtlFrm = false;

                                    this.workSpace.Width = Glo.HisMapData.FrmW;
                                    this.workSpace.Height = Glo.HisMapData.FrmH;
                                }

                                break;
                            case EEleTableNames.Sys_FrmBtn:
                                foreach (JsonValue dr in item.Value)
                                {
                                    if (dr.Count == 0)
                                        continue;
                                    if ((string)dr["FK_MAPDATA"] != Glo.FK_MapData)
                                        continue;

                                    BPBtn btn = new BPBtn();

                                    btn.Name = dr["MYPK"];
                                    tmpStr = dr["TEXT"];
                                    btn.Content = tmpStr.Replace("&nbsp;", " ");
                                    int type = dr["BTNTYPE"];
                                    btn.HisBtnType = (BtnType)type;
                                    type = dr["EVENTTYPE"];
                                    btn.HisEventType = (EventType)type;

                                    tmpStr = dr["EVENTCONTEXT"];
                                    if (!string.IsNullOrEmpty(tmpStr))
                                        btn.EventContext = tmpStr.Replace("~", "'");

                                    tmpStr = dr["MSGERR"];
                                    if (!string.IsNullOrEmpty(tmpStr))
                                        btn.MsgErr = tmpStr.Replace("~", "'");

                                    tmpStr = dr["MSGOK"];
                                    if (!string.IsNullOrEmpty(tmpStr))
                                        btn.MsgOK = tmpStr.Replace("~", "'");

                                    tmpDouble = dr["X"];
                                    btn.SetValue(Canvas.LeftProperty, tmpDouble);
                                    tmpDouble = dr["Y"];
                                    btn.SetValue(Canvas.TopProperty, tmpDouble);
                                    attachElementEvent(btn);
                                }
                                continue;
                            case EEleTableNames.Sys_FrmLine:
                                foreach (JsonValue dr in item.Value)
                                {
                                    if (dr.Count == 0)
                                        continue;

                                    if ((string)dr["FK_MAPDATA"] != Glo.FK_MapData)
                                        continue;

                                    string color = dr["BORDERCOLOR"];
                                    if (string.IsNullOrEmpty(color))
                                        color = "Black";

                                    BPLine myline = new BPLine(color, dr["BORDERWIDTH"],
                                        dr["X1"], dr["Y1"], dr["X2"],  dr["Y2"]);
                                    myline.Name = dr["MYPK"];
                                    attachElementEvent(myline);
                                }
                                continue;
                            case EEleTableNames.Sys_FrmLab:
                                foreach (JsonValue dr in item.Value)
                                {
                                    if (dr.Count == 0)
                                        continue;

                                    if ((string)dr["FK_MAPDATA"] != Glo.FK_MapData)
                                        continue;

                                    BPLabel lab = new BPLabel();
                                    lab.Name = dr["MYPK"];

                                    tmpStr = dr["TEXT"];
                                    tmpStr = tmpStr.Replace("&nbsp;", " ").Replace("@", "\n");
                                    lab.Content = tmpStr;

                                    lab.FontSize = dr["FONTSIZE"];

                                    lab.SetValue(Canvas.LeftProperty, (double)dr["X"]);
                                    lab.SetValue(Canvas.TopProperty, (double)dr["Y"]);

                                    if (dr["ISBOLD"] == 1)
                                        lab.FontWeight = FontWeights.Bold;
                                    else
                                        lab.FontWeight = FontWeights.Normal;

                                    string color = dr["FONTCOLOR"];
                                    lab.Foreground = new SolidColorBrush(Glo.ToColor(color));

                                    attachElementEvent(lab);
                                }
                                continue;
                            case EEleTableNames.Sys_FrmLink:
                                foreach (JsonValue dr in item.Value)
                                {
                                    if (dr.Count == 0)
                                        continue;

                                 
                                    if ((string)dr["FK_MAPDATA"] != Glo.FK_MapData)
                                        continue;

                                    BPLink link = new BPLink();
                                    link.Name = dr["MYPK"];
                                    tmpStr = dr["TEXT"];
                                    link.Content = tmpStr;
                                    link.URL = dr["URL"];
                                    link.WinTarget = dr["TARGET"];
                                    link.FontSize = dr["FONTSIZE"];

                                    link.SetValue(Canvas.LeftProperty, (double)dr["X"]);
                                    link.SetValue(Canvas.TopProperty, (double)dr["Y"]);

                                    string color = dr["FONTCOLOR"];
                                    if (string.IsNullOrEmpty(color))
                                        color = "Black";

                                    link.Foreground = new SolidColorBrush(Glo.ToColor(color));

                                    attachElementEvent(link);
                                }
                                continue;
                            case EEleTableNames.Sys_FrmImg:
                                foreach (JsonValue dr in item.Value)
                                {
                                    if (dr.Count == 0)
                                        continue;

                                    if ((string)dr["FK_MAPDATA"] != Glo.FK_MapData)
                                        continue;

                                    int ImgAppType = 0;
                                    try
                                    {
                                        ImgAppType = dr["IMGAPPTYPE"];
                                    }
                                    catch (Exception)
                                    {
                                    } 

                                    switch (ImgAppType)
                                    {
                                        case 1:
                                            BPImgSeal imgSeal = new BPImgSeal();
                                            imgSeal.Name = dr["MYPK"];
                                            imgSeal.SetValue(Canvas.LeftProperty, (double)dr["X"]);
                                            imgSeal.SetValue(Canvas.TopProperty, (double)dr["Y"]);

                                            imgSeal.Width = dr["W"];
                                            imgSeal.Height = dr["H"];
                                            imgSeal.TB_CN_Name = dr["NAME"] == null ? dr["MYPK"] : dr["NAME"];
                                            imgSeal.TB_En_Name = dr["ENPK"] == null ? dr["MYPK"]: dr["ENPK"];
                                            imgSeal.Tag0 = dr["TAG0"];
                                            imgSeal.IsEdit = false;
                                           
                                            imgSeal.IsEdit = dr["ISEDIT"] == 1 ? true : false;
                                            
                                            attachElementEvent(imgSeal);
                                            break;
                                        default:
                                            BPImg img = new BPImg();
                                            img.Name = dr["MYPK"];
                                            img.SetValue(Canvas.LeftProperty, (double)dr["X"]);
                                            img.SetValue(Canvas.TopProperty, (double)dr["Y"]);
                                            img.TB_CN_Name = dr["NAME"] == null ? dr["MYPK"] : dr["NAME"];
                                            img.TB_En_Name = dr["ENPK"] == null ? dr["MYPK"] : dr["ENPK"];
                                            img.Width = dr["W"];
                                            img.Height = dr["H"];

                                            string imgPath = string.Empty;
                                            if (dr["IMGPATH"] != null)
                                            {
                                                imgPath = dr["IMGPATH"];
                                            }
                                            string imgUrl = string.Empty;
                                            if (dr["IMGURL"] != null)
                                            {
                                                imgUrl = dr["IMGURL"];
                                            }
                                            // Local Photos 
                                            if ( dr["SRCTYPE"] == 0)
                                            {
                                                img.SrcType = 0;
                                                // Determine whether to modify the image path 
                                                if (imgPath.Contains("DataUser"))
                                                {
                                                    ImageBrush ib = new ImageBrush();
                                                    imgPath = Glo.BPMHost + imgPath;
                                                    BitmapImage png = new BitmapImage(new Uri(imgPath, UriKind.RelativeOrAbsolute));
                                                    ib.ImageSource = png;
                                                    img.Background = ib;
                                                    img.HisPng = png;
                                                }
                                            }
                                            else if ( dr["SRCTYPE"] == 1)// Specify the path 
                                            {
                                                img.SrcType = 1;
                                                // Judge image path is not empty , And does not contain ccflow Expression 
                                                if (!imgUrl.Contains("@"))
                                                {
                                                    ImageBrush ib = new ImageBrush();
                                                    BitmapImage png = new BitmapImage(new Uri(imgUrl, UriKind.RelativeOrAbsolute));
                                                    ib.ImageSource = png;
                                                    img.Background = ib;
                                                    img.HisPng = png;
                                                }
                                            }

                                            img.LinkTarget = dr["LINKTARGET"];
                                            img.LinkURL = dr["LINKURL"];
                                            img.ImgURL = imgUrl;
                                            img.ImgPath = imgPath;
                                            attachElementEvent(img);
                                            break;
                                    }
                                }
                                continue;
                            case EEleTableNames.Sys_FrmImgAth:
                                foreach (JsonValue dr in item.Value)
                                {
                                    if (dr.Count == 0)
                                        continue;

                                    if ((string)dr["FK_MAPDATA"] != Glo.FK_MapData)
                                        continue;

                                    BPImgAth ath = new BPImgAth();
                                    ath.Name = dr["MYPK"];
                                    ath.CtrlID = dr["CTRLID"]; // Accessory ID.

                                    ath.SetValue(Canvas.LeftProperty, (double)dr["X"]);
                                    ath.SetValue(Canvas.TopProperty, (double)dr["Y"]);
                                    ath.IsEdit = dr["ISEDIT"] == 1 ? true : false;
                                    ath.Height = dr["H"];
                                    ath.Width = dr["W"];
                                    attachElementEvent(ath);
                                }
                                continue;
                            case EEleTableNames.Sys_FrmRB:
                                foreach (JsonValue dr in item.Value)
                                {
                                    if (dr.Count == 0)
                                        continue;
                                    if ((string)dr["FK_MAPDATA"] != Glo.FK_MapData)
                                        continue;

                                    BPRadioBtn btn = new BPRadioBtn();
                                    btn.Name = dr["MYPK"];
                                    btn.GroupName = dr["KEYOFEN"];
                                    btn.Content = (string)dr["LAB"];
                                    btn.UIBindKey = dr["ENUMKEY"];
                                    btn.Tag = dr["INTKEY"].ToString();
                                    btn.SetValue(Canvas.LeftProperty, (double)dr["X"]);
                                    btn.SetValue(Canvas.TopProperty, (double)dr["Y"]);


                                    attachElementEvent(btn);
                                }
                                continue;
                            case EEleTableNames.Sys_MapAttr:
                                foreach (JsonValue dr in item.Value)
                                {
                                    if (dr.Count == 0)
                                        continue;

                                    if (dr["UIVISIBLE"] == 0)
                                        continue;

                                    if ((string)dr["FK_MAPDATA"] != Glo.FK_MapData)
                                        continue;

                                    string myPk = dr["KEYOFEN"];
                                    string FK_MapData = dr["FK_MAPDATA"];
                                    string keyOfEn = dr["KEYOFEN"];
                                    string name = dr["NAME"];
                                    string defVal = dr["DEFVAL"];
                                    string UIContralType = dr["UICONTRALTYPE"].ToString();
                                    string MyDataType = dr["MYDATATYPE"].ToString();
                                    string lgType = dr["LGTYPE"].ToString();
                                    double X = dr["X"];
                                    double Y = dr["Y"];
                                    if (X == 0)
                                        X = 100;
                                    if (Y == 0)
                                        Y = 100;

                                    string uIBindKey = dr["UIBINDKEY"];
                                    switch (UIContralType)
                                    {
                                        case CtrlType.TextBox:
                                            TBType tp = TBType.String;
                                            switch (MyDataType)
                                            {
                                                case DataType.AppInt:
                                                    tp = TBType.Int;
                                                    break;
                                                case DataType.AppFloat:
                                                case DataType.AppDouble:
                                                    tp = TBType.Float;
                                                    break;
                                                case DataType.AppMoney:
                                                    tp = TBType.Money;
                                                    break;
                                                case DataType.AppString:
                                                    tp = TBType.String;
                                                    break;
                                                case DataType.AppDateTime:
                                                    tp = TBType.DateTime;
                                                    break;
                                                case DataType.AppDate:
                                                    tp = TBType.Date;
                                                    break;
                                                default:
                                                    break;
                                            }

                                            BPTextBox tb = new BPTextBox(tp)
                                            {
                                                NameOfReal = keyOfEn,
                                                Name = myPk,
                                                X = X,
                                                Y = Y,
                                                Width = dr["UIWIDTH"],
                                                Height = dr["UIHEIGHT"]
                                            };

                                            tb.SetValue(Canvas.LeftProperty, X);
                                            tb.SetValue(Canvas.TopProperty, Y);

                                            if (this.workSpace.FindName(tb.Name) != null)
                                            {
                                                MessageBox.Show(" Already exists " + tb.Name);
                                                continue;
                                            }
                                            attachElementEvent(tb);
                                            break;
                                        case CtrlType.DDL:
                                            BPDDL ddl = new BPDDL()
                                            {
                                                Name = myPk,
                                                UIBindKey = uIBindKey,
                                                _HisDataType = lgType,
                                                Width =dr["UIWIDTH"]
                                            };

                                            if (lgType == LGType.Enum)
                                            {
                                                ddl.BindEnum(uIBindKey);
                                            }
                                            else
                                            {
                                                ddl.BindEns(uIBindKey);
                                            }

                                            ddl.SetValue(Canvas.LeftProperty, X);
                                            ddl.SetValue(Canvas.TopProperty, Y);
                                            attachElementEvent(ddl);
                                            break;
                                        case CtrlType.CheckBox:
                                            BPCheckBox cb = new BPCheckBox();
                                            cb.Name = keyOfEn;

                                            cb.Content = new Label()
                                            {
                                                Name = myPk,
                                                Content = name,
                                                Tag = keyOfEn
                                            };

                                            if (defVal == "1")
                                                cb.IsChecked = true;
                                            else
                                                cb.IsChecked = false;

                                            cb.SetValue(Canvas.LeftProperty, X);
                                            cb.SetValue(Canvas.TopProperty, Y);

                                            attachElementEvent(cb);
                                            break;
                                        case CtrlType.RB:
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                continue;
                            case EEleTableNames.Sys_MapM2M:
                                foreach (JsonValue dr in item.Value)
                                {
                                    if (dr.Count == 0)
                                        continue;

                                    if ((string)dr["FK_MAPDATA"] != Glo.FK_MapData)
                                        continue;

                                    tmpStr = dr["MYPK"];
                                    BPM2M m2m = new BPM2M(tmpStr);
                                    tmpDouble = dr["X"];
                                    m2m.SetValue(Canvas.LeftProperty, tmpDouble);
                                    tmpDouble = dr["Y"];
                                    m2m.SetValue(Canvas.TopProperty, tmpDouble);

                                    m2m.Width = dr["W"];
                                    m2m.Height = dr["H"];

                                    attachElementEvent(m2m);
                                }
                                continue;
                            case EEleTableNames.Sys_MapDtl:
                                foreach (JsonValue dr in item.Value)
                                {
                                    if (dr.Count == 0)
                                        continue;

                                    BPDtl dtl = new BPDtl(dr["NO"]);
                                    tmpDouble = dr["X"];
                                    dtl.SetValue(Canvas.LeftProperty, tmpDouble);
                                    tmpDouble = dr["Y"];
                                    dtl.SetValue(Canvas.TopProperty, tmpDouble);
                                    dtl.Width = dr["W"];
                                    dtl.Height = dr["H"];

                                    attachElementEvent(dtl);
                                }
                                continue;
                            case EEleTableNames.Sys_FrmAttachment:
                                foreach (JsonValue dr in item.Value)
                                {
                                    if (dr.Count == 0)
                                        continue;
                                    if ((string)dr["FK_MAPDATA"] != Glo.FK_MapData)
                                        continue;

                                    int uploadTypeInt = dr["UPLOADTYPE"];
                                    AttachmentUploadType uploadType = (AttachmentUploadType)uploadTypeInt;
                                    if (uploadType == AttachmentUploadType.Single)
                                    {
                                        BPAttachment ath = new BPAttachment(dr["NOOFOBJ"],
                                            dr["NAME"], dr["EXTS"], dr["W"], dr["SAVETO"]);

                                        tmpDouble = dr["X"];
                                        ath.SetValue(Canvas.LeftProperty, tmpDouble);
                                        tmpDouble = dr["Y"];
                                        ath.SetValue(Canvas.TopProperty, tmpDouble);
                                        ath.Label = dr["NAME"];
                                        ath.Exts = dr["EXTS"];
                                        ath.SaveTo = dr["SAVETO"];

                                        ath.X = dr["X"];
                                        ath.Y = dr["Y"];

                                        ath.IsUpload = dr["ISUPLOAD"] == 1 ? true : false;
                                        ath.IsDelete = dr["ISDELETE"] == 1 ? true : false;
                                        ath.IsDownload =dr["ISDOWNLOAD"] == 1? true :false;
                                      
                                        attachElementEvent(ath);
                                    }
                                    else if (uploadType == AttachmentUploadType.Multi)
                                    {
                                        BPAttachmentM athM = new BPAttachmentM();

                                        tmpDouble = dr["X"];
                                        athM.SetValue(Canvas.LeftProperty, tmpDouble);
                                        tmpDouble = dr["Y"];
                                        athM.SetValue(Canvas.TopProperty, tmpDouble);
                                      
                                        athM.Name = dr["NOOFOBJ"];
                                        athM.Width = dr["W"];
                                        athM.Height = dr["H"];
                                        athM.X = dr["X"];
                                        athM.Y = dr["Y"];
                                        athM.SaveTo = dr["SAVETO"];
                                        athM.Label = dr["NAME"];

                                        attachElementEvent(athM);

                                    }
                                    continue;

                                }
                                continue;
                            default:
                                break;
                        }
                    }
                    #endregion

                }
                catch (Exception ex)
                {
                    toBeContinued = false;
                    BP.SL.LoggerHelper.Write(ex);
                    strs = "err:" + table + ", Error loading form ," + ex.Message;
                }
            }


            this.SetGridLines(this.workSpace, true); // Re-draw the line 

            if (!toBeContinued)
            {
                MessageBox.Show(strs);
            }
            else
            {
                if (null != CCBPFormLoaded)
                    CCBPFormLoaded();

            }
            this.Cursor = Cursors.Arrow;

        }


        static DataSet dsLatest;
        public void Save()
        {
            loadingWindow.Show();
            if (!Glo.ViewNeedSave)
            {

            }
            Glo.ViewNeedSave = false;
            try
            {
                this.SetSelectedTool(ToolBox.Mouse);

                if (dsLatest == null)
                    initDataSource();
                else
                    foreach (DataTable item in dsLatest.Tables)
                    {
                        if (item != null)
                            item.Rows.Clear();
                    }

                DataTable dtMapData = dsLatest.Tables[EEleTableNames.Sys_MapData];
                 // Form data modifications in the properties form 
                DataRow drMapDR = dtMapData.NewRow();
                drMapDR["NAME"] = Glo.HisMapData.Name;
                drMapDR["NO"] = Glo.FK_MapData;
                double tmpD = Glo.HisMapData.FrmW;
                drMapDR["FRMW"] = tmpD == double.NaN ? "900" : tmpD.ToString("0");
                tmpD = Glo.HisMapData.FrmH;
                drMapDR["FRMH"] = tmpD == double.NaN ? "900" : tmpD.ToString("0");
                dtMapData.Rows.Add(drMapDR);

                SaveJson();
            }
            catch (Exception e)
            {
                loadingWindow.DialogResult = false;
                MessageBox.Show(" Save error :" + e.Message);
            }
        }

        void initDataSource()
        {
            DataTable dtMapData = new DataTable();
            dtMapData.TableName = EEleTableNames.Sys_MapData;
            dtMapData.Columns.Add(new DataColumn("No", typeof(string)));
            dtMapData.Columns.Add(new DataColumn("Name", typeof(string)));
            dtMapData.Columns.Add(new DataColumn("FrmW", typeof(double)));
            dtMapData.Columns.Add(new DataColumn("FrmH", typeof(double)));

            #region line
            DataTable dtLine = new DataTable();
            dtLine.TableName = EEleTableNames.Sys_FrmLine;
            dtLine.Columns.Add(new DataColumn("MyPK", typeof(string)));
            dtLine.Columns.Add(new DataColumn("FK_MapData", typeof(string)));

            dtLine.Columns.Add(new DataColumn("X", typeof(double)));
            dtLine.Columns.Add(new DataColumn("Y", typeof(double)));

            dtLine.Columns.Add(new DataColumn("X1", typeof(double)));
            dtLine.Columns.Add(new DataColumn("Y1", typeof(double)));

            dtLine.Columns.Add(new DataColumn("X2", typeof(double)));
            dtLine.Columns.Add(new DataColumn("Y2", typeof(double)));

            dtLine.Columns.Add(new DataColumn("BorderWidth", typeof(string)));
            dtLine.Columns.Add(new DataColumn("BorderColor", typeof(string)));
            // lineDT.Columns.Add(new DataColumn("BorderStyle", typeof(string)));
            #endregion line

            #region btn
            DataTable dtBtn = new DataTable();
            dtBtn.TableName = EEleTableNames.Sys_FrmBtn;
            dtBtn.Columns.Add(new DataColumn("MyPK", typeof(string)));
            dtBtn.Columns.Add(new DataColumn("FK_MapData", typeof(string)));
            dtBtn.Columns.Add(new DataColumn("Text", typeof(string)));
            dtBtn.Columns.Add(new DataColumn("X", typeof(double)));
            dtBtn.Columns.Add(new DataColumn("Y", typeof(double)));
            #endregion line

            #region label
            DataTable dtLabel = new DataTable();
            dtLabel.TableName = EEleTableNames.Sys_FrmLab;
            dtLabel.Columns.Add(new DataColumn("MyPK", typeof(string)));
            dtLabel.Columns.Add(new DataColumn("FK_MapData", typeof(string)));
            dtLabel.Columns.Add(new DataColumn("X", typeof(double)));
            dtLabel.Columns.Add(new DataColumn("Y", typeof(double)));
            dtLabel.Columns.Add(new DataColumn("Text", typeof(string)));

            dtLabel.Columns.Add(new DataColumn("FontColor", typeof(string)));
            dtLabel.Columns.Add(new DataColumn("FontName", typeof(string)));
            dtLabel.Columns.Add(new DataColumn("FontStyle", typeof(string)));
            dtLabel.Columns.Add(new DataColumn("FontSize", typeof(int)));
            dtLabel.Columns.Add(new DataColumn("IsBold", typeof(int)));
            dtLabel.Columns.Add(new DataColumn("IsItalic", typeof(int)));
            #endregion label

            #region Link
            DataTable dtLikn = new DataTable();
            dtLikn.TableName = EEleTableNames.Sys_FrmLink;
            dtLikn.Columns.Add(new DataColumn("MyPK", typeof(string)));
            dtLikn.Columns.Add(new DataColumn("FK_MapData", typeof(string)));
            dtLikn.Columns.Add(new DataColumn("X", typeof(double)));
            dtLikn.Columns.Add(new DataColumn("Y", typeof(double)));
            dtLikn.Columns.Add(new DataColumn("Text", typeof(string)));
            dtLikn.Columns.Add(new DataColumn("Target", typeof(string)));
            dtLikn.Columns.Add(new DataColumn("URL", typeof(string)));

            dtLikn.Columns.Add(new DataColumn("FontColor", typeof(string)));
            dtLikn.Columns.Add(new DataColumn("FontName", typeof(string)));
            dtLikn.Columns.Add(new DataColumn("FontStyle", typeof(string)));
            dtLikn.Columns.Add(new DataColumn("FontSize", typeof(int)));

            dtLikn.Columns.Add(new DataColumn("IsBold", typeof(int)));
            dtLikn.Columns.Add(new DataColumn("IsItalic", typeof(int)));
            #endregion Link

            #region img  ImgSeal
            DataTable dtImg = new DataTable();
            dtImg.TableName = EEleTableNames.Sys_FrmImg;
            dtImg.Columns.Add(new DataColumn("MyPK", typeof(string)));
            dtImg.Columns.Add(new DataColumn("FK_MapData", typeof(string)));
            dtImg.Columns.Add(new DataColumn("ImgAppType", typeof(int)));
            dtImg.Columns.Add(new DataColumn("X", typeof(double)));
            dtImg.Columns.Add(new DataColumn("Y", typeof(double)));
            dtImg.Columns.Add(new DataColumn("W", typeof(double)));
            dtImg.Columns.Add(new DataColumn("H", typeof(double)));

            dtImg.Columns.Add(new DataColumn("ImgURL", typeof(string)));
            dtImg.Columns.Add(new DataColumn("ImgPath", typeof(string))); // Application Type  0= Picture ,1 Signature ..

            dtImg.Columns.Add(new DataColumn("LinkURL", typeof(string)));
            dtImg.Columns.Add(new DataColumn("LinkTarget", typeof(string)));
            dtImg.Columns.Add(new DataColumn("SrcType", typeof(int))); // Image Source Type .
            dtImg.Columns.Add(new DataColumn("ImgAppType", typeof(int))); // Application Type  0= Picture ,1 Signature ..
            dtImg.Columns.Add(new DataColumn("Tag0", typeof(string)));
            dtImg.Columns.Add(new DataColumn("IsEdit", typeof(int)));
            dtImg.Columns.Add(new DataColumn("Name", typeof(string)));// Chinese name 
            dtImg.Columns.Add(new DataColumn("EnPK", typeof(string)));// English 
            #endregion img

            #region eleDT
            DataTable dtEle = new DataTable();
            dtEle.TableName = EEleTableNames.Sys_FrmEle;
            dtEle.Columns.Add(new DataColumn("MyPK", typeof(string)));
            dtEle.Columns.Add(new DataColumn("FK_MapData", typeof(string)));

            //eleDT.Columns.Add(new DataColumn("EleType", typeof(string)));
            //eleDT.Columns.Add(new DataColumn("EleID", typeof(string)));
            //eleDT.Columns.Add(new DataColumn("EleName", typeof(string)));

            dtEle.Columns.Add(new DataColumn("X", typeof(double)));
            dtEle.Columns.Add(new DataColumn("Y", typeof(double)));
            dtEle.Columns.Add(new DataColumn("W", typeof(double)));
            dtEle.Columns.Add(new DataColumn("H", typeof(double)));
            #endregion eleDT

            #region Sys_FrmImgAth
            DataTable imgAthDT = new DataTable();
            imgAthDT.TableName = EEleTableNames.Sys_FrmImgAth;
            imgAthDT.Columns.Add(new DataColumn("MyPK", typeof(string)));
            imgAthDT.Columns.Add(new DataColumn("CtrlID", typeof(string)));
            imgAthDT.Columns.Add(new DataColumn("FK_MapData", typeof(string)));
            imgAthDT.Columns.Add(new DataColumn("IsEdit", typeof(double)));
            imgAthDT.Columns.Add(new DataColumn("X", typeof(double)));
            imgAthDT.Columns.Add(new DataColumn("Y", typeof(double)));
            imgAthDT.Columns.Add(new DataColumn("W", typeof(double)));
            imgAthDT.Columns.Add(new DataColumn("H", typeof(double)));
            #endregion Sys_FrmImgAth

            #region mapAttrDT
            DataTable mapAttrDT = new DataTable();
            mapAttrDT.TableName = EEleTableNames.Sys_MapAttr;
            mapAttrDT.Columns.Add(new DataColumn("MyPK", typeof(string)));
            mapAttrDT.Columns.Add(new DataColumn("FK_MapData", typeof(string)));
            mapAttrDT.Columns.Add(new DataColumn("KeyOfEn", typeof(string)));
            mapAttrDT.Columns.Add(new DataColumn("UIContralType", typeof(string)));
            mapAttrDT.Columns.Add(new DataColumn("MyDataType", typeof(string)));
            mapAttrDT.Columns.Add(new DataColumn("LGType", typeof(string)));

            mapAttrDT.Columns.Add(new DataColumn("UIWidth", typeof(double)));
            mapAttrDT.Columns.Add(new DataColumn("UIHeight", typeof(double)));

            mapAttrDT.Columns.Add(new DataColumn("UIBindKey", typeof(string)));
            mapAttrDT.Columns.Add(new DataColumn("UIRefKey", typeof(string)));
            mapAttrDT.Columns.Add(new DataColumn("UIRefKeyText", typeof(string)));
            //   mapAttrDT.Columns.Add(new DataColumn("UIVisible", typeof(string)));
            mapAttrDT.Columns.Add(new DataColumn("X", typeof(double)));
            mapAttrDT.Columns.Add(new DataColumn("Y", typeof(double)));
            #endregion mapAttrDT

            #region frmRBDT
            DataTable frmRBDT = new DataTable();
            frmRBDT.TableName = EEleTableNames.Sys_FrmRB;
            frmRBDT.Columns.Add(new DataColumn("MyPK", typeof(string)));
            frmRBDT.Columns.Add(new DataColumn("FK_MapData", typeof(string)));
            frmRBDT.Columns.Add(new DataColumn("KeyOfEn", typeof(string)));
            frmRBDT.Columns.Add(new DataColumn("EnumKey", typeof(string)));
            frmRBDT.Columns.Add(new DataColumn("IntKey", typeof(int)));
            frmRBDT.Columns.Add(new DataColumn("Lab", typeof(string)));
            frmRBDT.Columns.Add(new DataColumn("X", typeof(double)));
            frmRBDT.Columns.Add(new DataColumn("Y", typeof(double)));
            #endregion frmRBDT

            #region Dtl
            DataTable dtlDT = new DataTable();

            dtlDT.TableName = EEleTableNames.Sys_MapDtl;
            dtlDT.Columns.Add(new DataColumn("No", typeof(string)));
            dtlDT.Columns.Add(new DataColumn("FK_MapData", typeof(string)));

            dtlDT.Columns.Add(new DataColumn("X", typeof(double)));
            dtlDT.Columns.Add(new DataColumn("Y", typeof(double)));

            dtlDT.Columns.Add(new DataColumn("H", typeof(double)));
            dtlDT.Columns.Add(new DataColumn("W", typeof(double)));
            #endregion Dtl

            // BPWorkCheck
            DataTable dtWorkCheck = new DataTable();
            dtWorkCheck.TableName = EEleTableNames.WF_Node;
            dtWorkCheck.Columns.Add(new DataColumn("NodeID", typeof(string)));
            dtWorkCheck.Columns.Add(new DataColumn("FWCSta", typeof(int)));
            dtWorkCheck.Columns.Add(new DataColumn("FWCType", typeof(int)));
            dtWorkCheck.Columns.Add(new DataColumn("FWC_X", typeof(double)));
            dtWorkCheck.Columns.Add(new DataColumn("FWC_Y", typeof(double)));
            dtWorkCheck.Columns.Add(new DataColumn("FWC_H", typeof(double)));
            dtWorkCheck.Columns.Add(new DataColumn("FWC_W", typeof(double)));
          
            #region m2mDT
            DataTable m2mDT = new DataTable();
            m2mDT.TableName = EEleTableNames.Sys_MapM2M;
            m2mDT.Columns.Add(new DataColumn("MyPK", typeof(string)));
            m2mDT.Columns.Add(new DataColumn("NoOfObj", typeof(string)));
            m2mDT.Columns.Add(new DataColumn("FK_MapData", typeof(string)));

            m2mDT.Columns.Add(new DataColumn("X", typeof(double)));
            m2mDT.Columns.Add(new DataColumn("Y", typeof(double)));

            m2mDT.Columns.Add(new DataColumn("H", typeof(string)));
            m2mDT.Columns.Add(new DataColumn("W", typeof(string)));
            #endregion m2mDT

            #region athDT
            DataTable athDT = new DataTable();
            athDT.TableName = EEleTableNames.Sys_FrmAttachment;
            athDT.Columns.Add(new DataColumn("MyPK", typeof(string)));
            athDT.Columns.Add(new DataColumn("FK_MapData", typeof(string)));
            athDT.Columns.Add(new DataColumn("NoOfObj", typeof(string)));

            //athDT.Columns.Add(new DataColumn("Name", typeof(string)));
            //athDT.Columns.Add(new DataColumn("Exts", typeof(string)));
            //athDT.Columns.Add(new DataColumn("SaveTo", typeof(string)));
            athDT.Columns.Add(new DataColumn("UploadType", typeof(int)));

            athDT.Columns.Add(new DataColumn("X", typeof(double)));
            athDT.Columns.Add(new DataColumn("Y", typeof(double)));
            athDT.Columns.Add(new DataColumn("W", typeof(double)));
            athDT.Columns.Add(new DataColumn("H", typeof(double)));
            #endregion athDT

            dsLatest = new DataSet();

            dsLatest.Tables.Add(dtWorkCheck);
            dsLatest.Tables.Add(dtLabel);
            dsLatest.Tables.Add(dtLikn);
            dsLatest.Tables.Add(dtImg);
            dsLatest.Tables.Add(dtEle);
            dsLatest.Tables.Add(dtBtn);
            dsLatest.Tables.Add(imgAthDT);
            dsLatest.Tables.Add(mapAttrDT);
            dsLatest.Tables.Add(frmRBDT);
            dsLatest.Tables.Add(dtLine);
            dsLatest.Tables.Add(dtlDT);
            dsLatest.Tables.Add(athDT);
            dsLatest.Tables.Add(dtMapData);
            dsLatest.Tables.Add(m2mDT);
        }

        public void SaveJson()
        {
            DataTable 
            dtLine = dsLatest.Tables[EEleTableNames.Sys_FrmLine],
            dtBtn = dsLatest.Tables[EEleTableNames.Sys_FrmBtn],
            dtLabel = dsLatest.Tables[EEleTableNames.Sys_FrmLab],
            dtLink = dsLatest.Tables[EEleTableNames.Sys_FrmLink],
            dtImg = dsLatest.Tables[EEleTableNames.Sys_FrmImg],
            dtEle = dsLatest.Tables[EEleTableNames.Sys_FrmEle],
            dtImgAth = dsLatest.Tables[EEleTableNames.Sys_FrmImgAth],
            dtMapAttr = dsLatest.Tables[EEleTableNames.Sys_MapAttr],
            dtRDB = dsLatest.Tables[EEleTableNames.Sys_FrmRB],
            dtlDT = dsLatest.Tables[EEleTableNames.Sys_MapDtl],
            dtWorkCheck = dsLatest.Tables[EEleTableNames.WF_Node],
            dtM2M = dsLatest.Tables[EEleTableNames.Sys_MapM2M],
            dtAth = dsLatest.Tables[EEleTableNames.Sys_FrmAttachment];
           
            #region
            foreach (UIElement ctl in this.workSpace.Children)
            {
                if (!(ctl is IElement)) continue;
                if((ctl as IElement).ViewDeleted) continue;

                double dX = Canvas.GetLeft(ctl);
                double dY = Canvas.GetTop(ctl);

                if (ctl is BPLine)
                {
                    #region
                    BPLine line = ctl as BPLine;
                    if (line != null)
                    {
                        DataRow drline = dtLine.NewRow();
                        string myPk = line.Name.Contains(Glo.FK_MapData) ? line.Name : Glo.FK_MapData + "_" + line.Name;

                        drline["MYPK"] = myPk;
                        drline["FK_MAPDATA"] = Glo.FK_MapData;

                        drline["X"] = dX.ToString("0.00");
                        drline["Y"] = dY.ToString("0.00");

                        drline["X1"] = line.MyLine.X1.ToString("0.00");
                        drline["X2"] = line.MyLine.X2.ToString("0.00");
                        drline["Y1"] = line.MyLine.Y1.ToString("0.00");
                        drline["Y2"] = line.MyLine.Y2.ToString("0.00");
                        drline["BORDERWIDTH"] = line.MyLine.StrokeThickness.ToString("0.00");
                        drline["BORDERCOLOR"] = line.Color;
                        dtLine.Rows.Add(drline);
                    }
                    #endregion
                }
                else if (ctl is TextBoxExt)
                {
                    if (ctl is BPEle)
                    {
                        #region
                        BPEle ele = ctl as BPEle;
                        if (ele != null)
                        {
                            DataRow drImg = dtEle.NewRow();

                            string myPk = ele.Name.Contains(Glo.FK_MapData) ? ele.Name : Glo.FK_MapData + "_" + ele.Name;

                            drImg["MYPK"] = myPk;
                            drImg["FK_MAPDATA"] = Glo.FK_MapData;

                            //drImg["ELETYPE"] = ele.EleType;
                            //drImg["ELENAME"] = ele.EleName;
                            //drImg["ELEID"] = ele.EleID;

                            //eleDT.Columns.Add(new DataColumn("EleType", typeof(string)));
                            //eleDT.Columns.Add(new DataColumn("EleID", typeof(string)));
                            //eleDT.Columns.Add(new DataColumn("EleName", typeof(string)));

                            MatrixTransform transform = ctl.TransformToVisual(this.workSpace) as MatrixTransform;
                            double x = transform.Matrix.OffsetX;
                            double y = transform.Matrix.OffsetY;

                            if (x <= 0)
                                x = 0;
                            if (y == 0)
                                y = 0;
                            if (y.ToString() == "NaN")
                            {
                                x = Canvas.GetLeft(ctl);
                                y = Canvas.GetTop(ctl);
                            }

                            drImg["X"] = x.ToString("0.00");
                            drImg["Y"] = y.ToString("0.00");

                            drImg["W"] = ele.Width.ToString("0.00");
                            drImg["H"] = ele.Height.ToString("0.00");

                            dtEle.Rows.Add(drImg);

                        }
                        continue;
                        #endregion
                    }
                    else if (ctl is BPTextBox)
                    {
                        #region
                        BPTextBox tb = ctl as BPTextBox;
                        if (tb != null)
                        {

                            DataRow mapAttrDR = dtMapAttr.NewRow();
                            string myPk = tb.Name.Trim().Contains(Glo.FK_MapData) ? tb.Name.Trim() : Glo.FK_MapData + "_" + tb.Name.Trim();

                            mapAttrDR["MYPK"] = myPk;
                            mapAttrDR["MYPK"] = Glo.FK_MapData + "_" + tb.Name.Trim();
                            mapAttrDR["FK_MAPDATA"] = Glo.FK_MapData;
                            mapAttrDR["KEYOFEN"] = tb.Name.Trim();

                            mapAttrDR["UICONTRALTYPE"] = CtrlType.TextBox;
                            mapAttrDR["MYDATATYPE"] = tb.HisDataType;

                            mapAttrDR["UIWIDTH"] = tb.Width.ToString("0.00");
                            mapAttrDR["UIHEIGHT"] = tb.Height.ToString("0.00");
                            mapAttrDR["LGTYPE"] = LGType.Normal;


                            MatrixTransform transform = ctl.TransformToVisual(this.workSpace) as MatrixTransform;
                            double x = transform.Matrix.OffsetX;
                            double y = transform.Matrix.OffsetY;

                            if (y.ToString() == "NaN")
                            {
                                x = Canvas.GetLeft(ctl);
                                y = Canvas.GetTop(ctl);
                            }

                            mapAttrDR["X"] = x.ToString("0.00");
                            mapAttrDR["Y"] = y.ToString("0.00");
                            // mapAttrDR["UIVISIBLE"] = "1";
                            dtMapAttr.Rows.Add(mapAttrDR);

                        }
                        continue;
                        #endregion
                    }
                    else if (ctl is BPImg)
                    {
                        #region
                        BPImg img = ctl as BPImg;
                        if (img != null)
                        {
                            DataRow drImg = dtImg.NewRow();

                            string myPk = img.Name.Contains(Glo.FK_MapData) ? img.Name : Glo.FK_MapData + "_" + img.Name;
                            drImg["MYPK"] = myPk;
                            drImg["FK_MAPDATA"] = Glo.FK_MapData;

                            MatrixTransform transform = ctl.TransformToVisual(this.workSpace) as MatrixTransform;
                            double x = transform.Matrix.OffsetX;
                            double y = transform.Matrix.OffsetY;

                            if (x <= 0)
                                x = 0;
                            if (y == 0)
                                y = 0;
                            if (y.ToString() == "NaN")
                            {
                                x = Canvas.GetLeft(ctl);
                                y = Canvas.GetTop(ctl);
                            }

                            drImg["X"] = x.ToString("0.00"); // Canvas.GetLeft(ctl).ToString("0.00");
                            drImg["Y"] = y.ToString("0.00"); // Canvas.GetTop(ctl).ToString("0.00");

                            drImg["W"] = img.Width.ToString("0.00");
                            drImg["H"] = img.Height.ToString("0.00");

                            BitmapImage png = img.HisPng;

                            drImg["LINKURL"] = img.LinkURL;
                            drImg["LINKTARGET"] = img.LinkTarget;
                            drImg["SRCTYPE"] = img.SrcType.ToString();

                            drImg["IMGPATH"] = png.UriSource.ToString().Contains("DataUser") ? png.UriSource.ToString().Replace(Glo.BPMHost, "") : png.UriSource.ToString();
                            drImg["IMGURL"] = img.ImgURL;

                            drImg["IMGAPPTYPE"] = "0";
                            drImg["ISEDIT"] = "1";
                            drImg["NAME"] = img.TB_CN_Name;
                            drImg["ENPK"] = img.TB_En_Name;
                            dtImg.Rows.Add(drImg);

                        }
                        #endregion
                    }
                    else if (ctl is BPImgAth)
                    {
                        #region
                        BPImgAth imgAth = ctl as BPImgAth;
                        if (imgAth != null)
                        {
                            DataRow mapAth = dtImgAth.NewRow();
                            string myPk = imgAth.Name.Contains(Glo.FK_MapData) ? imgAth.Name : Glo.FK_MapData + "_" + imgAth.Name;
                            mapAth["MYPK"] = myPk;
                            mapAth["CTRLID"] = imgAth.CtrlID; // Accessory ID.
                            mapAth["FK_MAPDATA"] = Glo.FK_MapData;
                            mapAth["ISEDIT"] = imgAth.IsEdit ? "1" : "0";
                            MatrixTransform transform = imgAth.TransformToVisual(this.workSpace) as MatrixTransform;

                            mapAth["X"] = transform.Matrix.OffsetX.ToString("0.00");
                            mapAth["Y"] = transform.Matrix.OffsetY.ToString("0.00");

                            mapAth["W"] = imgAth.Width.ToString("0.00");
                            mapAth["H"] = imgAth.Height.ToString("0.00");
                            dtImgAth.Rows.Add(mapAth);

                        }
                        #endregion
                    }
                    else if (ctl is BPImgSeal)
                    {
                        #region
                        BPImgSeal imgSeal = ctl as BPImgSeal;
                        if (imgSeal != null)
                        {
                            DataRow drImgSeal = dtImg.NewRow();
                            string myPk = imgSeal.Name.Contains(Glo.FK_MapData) ? imgSeal.Name : Glo.FK_MapData + "_" + imgSeal.Name;

                            drImgSeal["MYPK"] = myPk;
                            drImgSeal["FK_MAPDATA"] = Glo.FK_MapData;
                            drImgSeal["IMGAPPTYPE"] = "1";
                            MatrixTransform transform = ctl.TransformToVisual(this.workSpace)
                                as MatrixTransform;
                            double x = transform.Matrix.OffsetX;
                            double y = transform.Matrix.OffsetY;

                            if (x <= 0)
                                x = 0;
                            if (y == 0)
                                y = 0;
                            if (y.ToString() == "NaN")
                            {
                                x = Canvas.GetLeft(ctl);
                                y = Canvas.GetTop(ctl);
                            }

                            drImgSeal["X"] = x.ToString("0.00");
                            drImgSeal["Y"] = y.ToString("0.00");

                            drImgSeal["W"] = imgSeal.Width.ToString("0.00");
                            drImgSeal["H"] = imgSeal.Height.ToString("0.00");

                            BitmapImage png = imgSeal.HisPng;
                            drImgSeal["IMGURL"] = png.UriSource.ToString();
                            drImgSeal["TAG0"] = imgSeal.Tag0;
                            drImgSeal["NAME"] = imgSeal.TB_CN_Name;
                            drImgSeal["ENPK"] = imgSeal.TB_En_Name;
                            drImgSeal["ISEDIT"] = imgSeal.IsEdit ? "1" : "0";
                            dtImg.Rows.Add(drImgSeal);

                        }
                        #endregion
                    }
                }
                else if (ctl is LabelExt)
                {
                    if (ctl is BPLabel)
                    {
                        #region
                        BPLabel lab = ctl as BPLabel;
                        if (lab != null)
                        {
                            DataRow drLab = dtLabel.NewRow();
                            string myPk = lab.Name.Contains(Glo.FK_MapData) ? lab.Name : Glo.FK_MapData + "_" + lab.Name;

                            drLab["MYPK"] = myPk;
                            drLab["TEXT"] = lab.Content.ToString().Replace(" ", "&nbsp;").Replace("\n", "@");
                            drLab["FK_MAPDATA"] = Glo.FK_MapData;

                            drLab["X"] = dX.ToString("0.00");
                            drLab["Y"] = dY.ToString("0.00");

                            // drLab["FONTCOLOR"] = lab.GetValue( lapp ).ToString();
#warning  How to get the font color  ? .

                            SolidColorBrush d = (SolidColorBrush)lab.Foreground;
                            drLab["FONTCOLOR"] = d.Color.ToString();
                            // Glo.PreaseColorToName(d.Color.ToString());
                            drLab["FONTNAME"] = lab.FontFamily.ToString();
                            drLab["FONTSTYLE"] = lab.FontStyle.ToString();
                            drLab["FONTSIZE"] = lab.FontSize.ToString();

                            if (lab.FontWeight == FontWeights.Normal)
                                drLab["ISBOLD"] = "0";
                            else
                                drLab["ISBOLD"] = "1";

                            if (lab.FontStyle.ToString() == "Italic")
                                drLab["ISITALIC"] = "1";
                            else
                                drLab["ISITALIC"] = "0";

                            dtLabel.Rows.Add(drLab);

                        }
                        #endregion
                    }
                    else if (ctl is BPLink)
                    {
                        #region
                        BPLink link = ctl as BPLink;
                        if (link != null)
                        {
                            DataRow drLink = dtLink.NewRow();
                            string myPk = link.Name.Contains(Glo.FK_MapData) ? link.Name : Glo.FK_MapData + "_" + link.Name;
                            drLink["MYPK"] = myPk;

                            drLink["TEXT"] = link.Content.ToString();
                            drLink["FK_MAPDATA"] = Glo.FK_MapData;

                            drLink["X"] = dX.ToString("0.00");
                            drLink["Y"] = dY.ToString("0.00");

                            SolidColorBrush d = (SolidColorBrush)link.Foreground;
                            drLink["FONTCOLOR"] = Glo.PreaseColorToName(d.Color.ToString());
                            drLink["FONTNAME"] = link.FontFamily.ToString();
                            drLink["FONTSTYLE"] = link.FontStyle.ToString();
                            drLink["FONTSIZE"] = link.FontSize.ToString();
                            drLink["URL"] = link.URL;
                            drLink["TARGET"] = link.WinTarget;

                            if (link.FontWeight == FontWeights.Normal)
                                drLink["ISBOLD"] = "0";
                            else
                                drLink["ISBOLD"] = "1";

                            if (link.FontStyle.ToString() == "Italic")
                                drLink["ISITALIC"] = "1";
                            else
                                drLink["ISITALIC"] = "0";

                            dtLink.Rows.Add(drLink);

                        }
                        #endregion
                    }
                }
                else if (ctl is UCExt)
                {
                    if (ctl is BPAttachment)
                    {
                        #region
                        BPAttachment athCtl = ctl as BPAttachment;
                        if (athCtl != null)
                        {

                            DataRow mapAth = dtAth.NewRow();
                            string myPk = athCtl.Name.Contains(Glo.FK_MapData) ? athCtl.Name : Glo.FK_MapData + "_" + athCtl.Name;

                            mapAth["MYPK"] = myPk;
                            mapAth["FK_MAPDATA"] = Glo.FK_MapData;
                            mapAth["NOOFOBJ"] = athCtl.Name;
                            mapAth["UPLOADTYPE"] = "0";

                            MatrixTransform transform = athCtl.TransformToVisual(this.workSpace) as MatrixTransform;
                            mapAth["X"] = transform.Matrix.OffsetX.ToString("0.00");
                            mapAth["Y"] = transform.Matrix.OffsetY.ToString("0.00");
                            mapAth["W"] = athCtl.HisTB.Width.ToString("0.00");
                            dtAth.Rows.Add(mapAth);
                        }
                        #endregion
                    }
                    else if (ctl is BPAttachmentM)
                    {
                        #region
                        BPAttachmentM athM = ctl as BPAttachmentM;
                        if (athM != null)
                        {
                            DataRow mapAth = dtAth.NewRow();
                            string myPk = athM.Name.Contains(Glo.FK_MapData) ? athM.Name : Glo.FK_MapData + "_" + athM.Name;

                            mapAth["MYPK"] = myPk;
                            mapAth["FK_MAPDATA"] = Glo.FK_MapData;
                            mapAth["NOOFOBJ"] = athM.Name;
                            mapAth["UPLOADTYPE"] = "1";

                            MatrixTransform transform = athM.TransformToVisual(this.workSpace) as MatrixTransform;
                            mapAth["X"] = transform.Matrix.OffsetX.ToString("0.00");
                            mapAth["Y"] = transform.Matrix.OffsetY.ToString("0.00");

                            mapAth["W"] = athM.Width.ToString("0.00");
                            mapAth["H"] = athM.Height.ToString("0.00");
                            dtAth.Rows.Add(mapAth);
                        }
                        #endregion
                    }
                    else if (ctl is BPDtl)
                    {
                        #region
                        BPDtl dtlCtl = ctl as BPDtl;
                        if (dtlCtl != null)
                        {
                            DataRow mapDtl = dtlDT.NewRow();
                            string myPk = dtlCtl.Name.Contains(Glo.FK_MapData) ? dtlCtl.Name : Glo.FK_MapData + "_" + dtlCtl.Name;
                            mapDtl["NO"] = myPk;
                        
                            mapDtl["FK_MAPDATA"] = Glo.FK_MapData;
                            MatrixTransform transform = dtlCtl.TransformToVisual(this.workSpace) as MatrixTransform;

                            mapDtl["X"] = transform.Matrix.OffsetX.ToString("0.00");
                            mapDtl["Y"] = transform.Matrix.OffsetY.ToString("0.00");
                            mapDtl["W"] = dtlCtl.Width.ToString("0.00");
                            mapDtl["H"] = dtlCtl.Height.ToString("0.00");
                            dtlDT.Rows.Add(mapDtl);

                        }
                        #endregion
                    }
                    else if (ctl is BPWorkCheck)
                    {
                        #region   Audit Components 
                        BPWorkCheck wkCheck = ctl as BPWorkCheck;
                        if (wkCheck != null)
                        {
                            DataRow workCheckDt = dtWorkCheck.NewRow();
                            workCheckDt["NODEID"] = Glo.FK_MapData.Replace("ND", "");

                            MatrixTransform transform = wkCheck.TransformToVisual(this.workSpace) as MatrixTransform;

                            workCheckDt["FWCSTA"] = wkCheck.FWC_Sta;
                            workCheckDt["FWCTYPE"] = wkCheck.FWC_Type;

                            workCheckDt["FWC_X"] = transform.Matrix.OffsetX.ToString("0.00");
                            workCheckDt["FWC_Y"] = transform.Matrix.OffsetY.ToString("0.00");

                            workCheckDt["FWC_W"] = wkCheck.Width.ToString("0.00");
                            workCheckDt["FWC_H"] = wkCheck.Height.ToString("0.00");
                            dtWorkCheck.Rows.Add(workCheckDt);

                        }
                        #endregion
                    }
                    else if (ctl is BPM2M)
                    {
                        #region
                        BPM2M m2mCtl = ctl as BPM2M;
                        if (m2mCtl != null)
                        {
                            DataRow rowM2M = dtM2M.NewRow();
                            rowM2M["NOOFOBJ"] = m2mCtl.Name;
                            rowM2M["FK_MAPDATA"] = Glo.FK_MapData;
                            string myPk = m2mCtl.Name.Contains(Glo.FK_MapData) ? m2mCtl.Name : Glo.FK_MapData + "_" + m2mCtl.Name;
                            rowM2M["MYPK"] = myPk;

                            MatrixTransform transform = m2mCtl.TransformToVisual(this.workSpace) as MatrixTransform;

                            rowM2M["X"] = transform.Matrix.OffsetX.ToString("0.00");
                            rowM2M["Y"] = transform.Matrix.OffsetY.ToString("0.00");

                            rowM2M["W"] = m2mCtl.Width.ToString("0.00");
                            rowM2M["H"] = m2mCtl.Height.ToString("0.00");

                            dtM2M.Rows.Add(rowM2M);
                        }
                        #endregion
                    }
                }
                else if (ctl is BPDatePicker)
                {
                    #region
                    BPDatePicker dp = ctl as BPDatePicker;
                    if (dp != null)
                    {
                        DataRow mapAttrDR = dtMapAttr.NewRow();
                        string myPk = dp.Name.Contains(Glo.FK_MapData) ? dp.Name : Glo.FK_MapData + "_" + dp.Name;
                        mapAttrDR["MYPK"] = myPk;
                        mapAttrDR["FK_MAPDATA"] = Glo.FK_MapData;
                        mapAttrDR["KEYOFEN"] = dp.Name;

                        mapAttrDR["UICONTRALTYPE"] = CtrlType.TextBox;
                        mapAttrDR["MYDATATYPE"] = dp.HisDateType;
                        mapAttrDR["LGTYPE"] = LGType.Normal;

                        mapAttrDR["X"] = dX.ToString("0.00");
                        mapAttrDR["Y"] = dY.ToString("0.00");

                        // mapAttrDR["UIVISIBLE"] = "1";
                        mapAttrDR["UIWIDTH"] = "50";
                        mapAttrDR["UIHEIGHT"] = "23";

                        dtMapAttr.Rows.Add(mapAttrDR);

                    }
                    continue;
                    #endregion
                }
                else if (ctl is BPBtn)
                {
                    #region
                    BPBtn btn = ctl as BPBtn;
                    if (btn != null)
                    {
                        DataRow drBtn = dtBtn.NewRow();
                        string myPk = btn.Name.Contains(Glo.FK_MapData) ? btn.Name : Glo.FK_MapData + "_" + btn.Name;
                        drBtn["MYPK"] = myPk;
                        drBtn["TEXT"] = btn.Content.ToString().Replace(" ", "&nbsp;").Replace("\n", "@");
                        drBtn["FK_MAPDATA"] = Glo.FK_MapData;

                        drBtn["X"] = dX.ToString("0.00");
                        drBtn["Y"] = dY.ToString("0.00");

                        dtBtn.Rows.Add(drBtn);
                    }
                    #endregion
                }

                else if (ctl is BPDDL)
                {
                    #region
                    BPDDL ddl = ctl as BPDDL;
                    if (ddl != null)
                    {
                      
                        DataRow mapAttrDR = dtMapAttr.NewRow();
                        string myPk = ddl.Name.Contains(Glo.FK_MapData) ? ddl.Name : Glo.FK_MapData + "_" + ddl.Name;
                        mapAttrDR["MYPK"] = myPk;
                        mapAttrDR["FK_MAPDATA"] = Glo.FK_MapData;
                        mapAttrDR["KEYOFEN"] = ddl.Name;

                        mapAttrDR["UICONTRALTYPE"] = CtrlType.DDL;
                        mapAttrDR["MYDATATYPE"] = ddl.HisDataType;
                        mapAttrDR["LGTYPE"] = ddl._HisDataType;



                        mapAttrDR["UIWIDTH"] = ddl.Width.ToString("0.00");
                        mapAttrDR["UIHEIGHT"] = "23";

                        mapAttrDR["X"] = dX.ToString("0.00");
                        mapAttrDR["Y"] = dY.ToString("0.00");

                        mapAttrDR["UIBINDKEY"] = ddl.UIBindKey;
                        mapAttrDR["UIREFKEY"] = "No";
                        mapAttrDR["UIREFKEYTEXT"] = "Name";
                        //     mapAttrDR["UIVISIBLE"] = "1";
                        dtMapAttr.Rows.Add(mapAttrDR);

                    }
                    #endregion
                }
                else if (ctl is BPCheckBox)
                {
                    #region
                    BPCheckBox cb = ctl as BPCheckBox;
                    if (cb != null)
                    {
                        DataRow mapAttrDR = dtMapAttr.NewRow();
                        string myPk = cb.Name.Contains(Glo.FK_MapData) ? cb.Name : Glo.FK_MapData + "_" + cb.Name;
                        mapAttrDR["MYPK"] = myPk;
                        mapAttrDR["FK_MAPDATA"] = Glo.FK_MapData;
                        mapAttrDR["KEYOFEN"] = cb.Name;
                        mapAttrDR["UICONTRALTYPE"] = CtrlType.CheckBox;
                        mapAttrDR["MYDATATYPE"] = DataType.AppBoolean;
                        mapAttrDR["LGTYPE"] = LGType.Normal;
                        mapAttrDR["X"] = dX.ToString("0.00");
                        mapAttrDR["Y"] = dY.ToString("0.00");
                        mapAttrDR["UIWIDTH"] = "100";
                        mapAttrDR["UIHEIGHT"] = "23";


                        dtMapAttr.Rows.Add(mapAttrDR);

                    }
                    #endregion
                }
                else if (ctl is BPRadioBtn)
                {
                    #region
                    BPRadioBtn rb = ctl as BPRadioBtn;
                    if (rb != null)
                    {
                        DataRow mapAttrRB = dtRDB.NewRow();
                        string myPk = rb.Name.Contains(Glo.FK_MapData) ? rb.Name : Glo.FK_MapData + "_" + rb.Name;
                        mapAttrRB["MYPK"] = myPk;
                        mapAttrRB["FK_MAPDATA"] = Glo.FK_MapData;
                        mapAttrRB["KEYOFEN"] = rb.GroupName;
                        mapAttrRB["INTKEY"] = rb.Tag.ToString();
                        mapAttrRB["LAB"] = rb.Content as string;
                        mapAttrRB["ENUMKEY"] = rb.UIBindKey;
                        mapAttrRB["X"] = dX.ToString("0.00");
                        mapAttrRB["Y"] = dY.ToString("0.00");
                        dtRDB.Rows.Add(mapAttrRB);

                    }
                    #endregion
                }
            }
            #endregion

            #region  Deal with  RB  Enum value 
            string keys = "";
            foreach (DataRow dr in dtRDB.Rows)
            {
                string keyOfEn = dr["KEYOFEN"];
                if (keys.Contains("@" + keyOfEn + "@"))
                    continue;
                else
                    keys += "@" + keyOfEn + "@";
                

                string enumKey = dr["ENUMKEY"];
                DataRow mapAttrDR = dtMapAttr.NewRow();
                mapAttrDR["MYPK"] = Glo.FK_MapData + "_" + keyOfEn;
                mapAttrDR["FK_MAPDATA"] = Glo.FK_MapData;
                mapAttrDR["KEYOFEN"] = keyOfEn;

                mapAttrDR["UICONTRALTYPE"] = CtrlType.RB;
                mapAttrDR["MYDATATYPE"] = DataType.AppInt;
                mapAttrDR["LGTYPE"] = LGType.Enum;
                mapAttrDR["X"] = "0";
                mapAttrDR["Y"] = "0";

                mapAttrDR["UIBINDKEY"] = enumKey;
                mapAttrDR["UIREFKEY"] = "No";
                mapAttrDR["UIREFKEYTEXT"] = "Name";
                mapAttrDR["UIWIDTH"] = "30";
                mapAttrDR["UIHEIGHT"] = "23";
                dtMapAttr.Rows.Add(mapAttrDR);
            }
            #endregion 

            #region DELETE
            string sqls = "", table = string.Empty;
            foreach (KeyValuePair<string, JsonValue> item in jsonObject)
            {
                if (item.Value.Count <= 0) continue;

                table = item.Key;
                string pk = "";
                string tmpStr = string.Empty;

                switch (table)
                {
                    case "WF_Node":
                        break;
                }
                DataTable newDt = dsLatest.Tables[table];
                if (newDt == null)
                    continue;


                foreach (JsonValue dr in item.Value)
                {
                    if (dr.Count == 0)
                        continue;

                    #region 求pK
                    if (dr.ContainsKey("MYPK") || dr.ContainsKey("MyPK"))
                    {
                        pk = "MYPK";
                    }
                    else if (dr.ContainsKey("NO") || dr.ContainsKey("No"))
                    {
                        pk = "NO";
                    }
                    else if (dr.ContainsKey("OID") || dr.ContainsKey("OID"))
                    {
                        pk = "OID";
                    }
                    else if (dr.ContainsKey("NODEID") || dr.ContainsKey("NodeID"))
                    {
                        pk = "NODEID";
                    }
                    #endregion 求pK

                    if (!string.IsNullOrEmpty(pk))
                        break;
                }

                foreach (JsonValue dr in item.Value)
                {
                    if (dr.Count == 0)
                        continue;

                    bool isStillExit = false;
                    string pkVal = dr[pk].ToString().Replace("\"", "");
                    if (table == EEleTableNames.WF_Node)
                    {
                        #region WF_Node, Does not contain FK_MapData Temporarily placed in front .
                        if (dr["NODEID"].ToString() != Glo.FK_MapData.Replace("ND", ""))
                            continue;
                      
                        //  Audit component judgment  
                        isStillExit = false;
                        foreach (DataRow newDr in newDt.Rows)
                        {
                            if ((string)newDr[pk] == pkVal)
                            {
                                isStillExit = true;
                                break;
                            }
                        }

                        if(!isStillExit)
                            sqls += "@UPDATE WF_Node SET FWCSta=0 WHERE " + pk + "='" + pkVal + "'";


                        break;
                        #endregion
                    }

                    if (table == EEleTableNames.Sys_MapData)
                    {
                        #region Sys_MapData
                        if (isDesignerSizeChanged)
                        {

                            double
                                heigh = double.NaN,
                                wid = double.NaN;
                            string no;

                            object tmp = newDt.Rows[0]["NO"];
                            no = tmp == null ? null : tmp.ToString();

                            tmp = newDt.Rows[0]["FRMW"];
                            if (tmp != null)
                                double.TryParse(tmp.ToString(), out wid);

                            tmp = newDt.Rows[0]["FRMH"];
                            if (null != tmp)
                                double.TryParse(tmp.ToString(), out heigh);
                            if (!string.IsNullOrEmpty(no) && heigh != double.NaN && wid != double.NaN)
                                sqls += "@UPDATE Sys_MapData SET FrmW=" + wid + ", FrmH=" + heigh + " WHERE No='" + no + "'";

                        }
                        #endregion
                        continue;
                    }

                    if ((string)dr["FK_MAPDATA"] != Glo.FK_MapData)
                        continue;

                    isStillExit = false;
                    if (table == EEleTableNames.Sys_MapAttr) /*  If the field control  .. */
                    {
                        foreach (DataRow newDr in newDt.Rows)
                        {
                            if ( dr["UIVISIBLE"] == 0)
                            {
                                isStillExit = true;
                                break;
                            }
                            if (newDr[pk].ToString() == pkVal)
                            {
                                isStillExit = true;
                                break;
                            }
                        }

                        //if (!isStillExit)
                        //{
                        //    if (dr["UIVISIBLE"] == 0 || dr["EDITTYPE"] != 0)
                        //        isStillExit = true;
                        //}
                    }
                    else
                    {
                        foreach (DataRow newDr in newDt.Rows)
                        {
                            if (newDr[pk].ToString() == pkVal)
                            {
                                isStillExit = true;
                                break;
                            }
                        }
                    }

                    if (!isStillExit)
                        sqls += "@DELETE FROM " + table + " WHERE " + pk + "='" + pkVal + "'";
                }
            }

            #endregion

            #region UPDATE
            string len = Glo.LEN_Function;
            foreach (UIElement ctl in this.workSpace.Children)
            {
                if (!(ctl is IElement)) continue;

                if (ctl is BPCheckBox)
                {
                    BPCheckBox cb = ctl as BPCheckBox;
                    if (null == cb || string.IsNullOrEmpty(cb.KeyName)) continue;

                    Label mylab = cb.Content as Label;
                    sqls += "@UPDATE Sys_MapAttr SET Name='" + mylab.Content + "'  WHERE MyPK='" + Glo.FK_MapData + "_" + cb.Name + "' AND " + len + "(Name)=0";
                    continue;

                }
                else if (ctl is BPTextBox)
                {
                    BPTextBox tb = ctl as BPTextBox;
                    if (tb == null || string.IsNullOrEmpty(tb.KeyName))
                        continue;

                    sqls += "@UPDATE Sys_MapAttr SET Name='" + tb.KeyName + "' WHERE MyPK='" + Glo.FK_MapData + "_" + tb.Name + "' AND ( " + len + "(Name)=0 OR KeyOfEn=Name )";
                    continue;

                }
                else if (ctl is BPDDL)
                {
                    BPDDL ddl = ctl as BPDDL;

                    if (ddl == null || string.IsNullOrEmpty(ddl.KeyName))
                        continue;

                    sqls += "@UPDATE Sys_MapAttr SET Name='" + ddl.KeyName + "' WHERE MyPK='" + Glo.FK_MapData + "_" + ddl.Name + "' AND " + len + "(Name)=0";
                    continue;

                }
                else if (ctl is BPRadioBtn)
                {
                    BPRadioBtn rb = ctl as BPRadioBtn;
                    if (rb == null || string.IsNullOrEmpty(rb.KeyName) || sqls.Contains("_" + rb.GroupName))
                        continue;

                    sqls += "@UPDATE Sys_MapAttr SET Name='" + rb.KeyName + "' WHERE MyPK='" + Glo.FK_MapData + "_" + rb.GroupName + "' AND " + len + "(Name)=0";
                    continue;
                }

            }
            #endregion

            try
            {
                string xml = Glo.ToJson(dsLatest);
                FF.CCFormSoapClient da = Glo.GetCCFormSoapClientServiceInstance();
                da.SaveFrmAsync(Glo.FK_MapData, xml, sqls, null);
                da.SaveFrmCompleted += ((object senders, FF.SaveFrmCompletedEventArgs ee) =>
                {
                    isDesignerSizeChanged = false;
                    loadingWindow.DialogResult = true;
                    #region
                    if (ee.Error != null)
                    {
                        BP.SL.LoggerHelper.Write(ee.Error);
                        MessageBox.Show(ee.Result, " Save error ", MessageBoxButton.OK);
                        return;
                    }

                    if (Keyboard.Modifiers == ModifierKeys.Windows)
                    {
                        string url1 = null;
                        if (Glo.IsDtlFrm == false)
                            url1 = Glo.BPMHost + "/WF/CCForm/Frm.aspx?FK_MapData=" + Glo.FK_MapData + "&IsTest=1&WorkID=0&FK_Node=" + Glo.FK_Node + "&sd=s" + Glo.TimeKey;
                        else
                            url1 = Glo.BPMHost + "/WF/CCForm/FrmCard.aspx?EnsName=" + Glo.FK_MapData + "&RefPKVal=0&OID=0" + Glo.TimeKey;

                        Glo.WinOpen(url1, (int)Glo.HisMapData.FrmH, (int)Glo.HisMapData.FrmW);
                    }

                    #endregion
                });
            }
            catch (Exception e)
            {
                loadingWindow.DialogResult = false;
                MessageBox.Show(" Save error :"+ e.Message);
            }
        }

        DateTime _lastTime;
        void UIElement_LeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (null != e)
                e.Handled = true;

            FrameworkElement element = sender as FrameworkElement;
         
            IElement ele = element as IElement;
            bool dbClicked = false;
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {//  Multiple choice 
                bool op;
                if (op=selectedElements.Contains(element))//  The same element   Selected  -->  Uncheck 
                {
                    selectedElements.Remove(element);
                }
                else
                {
                    selectedElements.Add(element);
                }

                if (ele  != null)
                {
                    ele.IsSelected = !op;
                }
            }
            else
            {
                if (element is BPLine)
                {

                }
                else
                {
                    //  With an element within the specified time, click twice to double-click 
                    if (Glo.currEle == element && DateTime.Now.Subtract(_lastTime).TotalMilliseconds < 300)
                    {
                        dbClicked = true;
                    }
                    else
                    {
                        Glo.currEle = element;
                        _lastTime = DateTime.Now;
                        dbClicked = false;
                    }
                }

                ////  Radio 
                //if (selectedElements.Contains(element))
                //{
                //    if ((ele = element as IElement) != null)
                //    {
                //        ele.IsSelected = false;
                //    }
                //}
                //else
                {
                    foreach (UIElement en in this.selectedElements)
                    {
                        if (en is IElement && (ele = en as IElement) != null)
                        {
                            ele.IsSelected = false;
                        }
                    }

                    this.selectedElements.Clear();
                    if ((ele = element as IElement) != null)
                    {
                        ele.IsSelected = true;
                    }
                    this.selectedElements.Add(element);
                }
            }


            Glo.SetTracking(ele, !dbClicked);
            if (dbClicked ||　 ele is BPLine)
            {
                UIElementDbClickEdit(sender);
            }
            else
            {
                MouseEventHandlers.pointFrom = e.GetPosition(null);
            }
        }
        void UIElement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {

            }
        }
        void UIElement_MouseLeave(object sender, MouseEventArgs e)
        {
            if (null != cursor && (this.isToolDraging || this.selectType.Equals(ToolBox.Line)))
            {
                cCursor.SetCursorTemplate(cursor);
            }
            else cCursor.SetCursorDefault(Cursors.Arrow);
        }
        void UIElement_MouseEnter(object sender, MouseEventArgs e)
        {
            cCursor.SetCursorDefault(Cursors.None);
        }
        void UIElementDbClickEdit(object sender)
        {//  Double-click on the element  或  Shortcut menu   Call editor 

            Glo.currEle = sender as UIElement;

            if (sender is BPBtn)
            {
                BPBtn btn = sender as BPBtn;
                if (btn != null)
                {
                    this.winFrmBtn.HisBtn = btn;
                    this.winFrmBtn.Show();
                }
            }
            else if (sender is BPLine)
            {
                BPLine line = sender as BPLine;
                if (line != null)
                {
                    this.currLine = line;
                    lineEdit(line);
                }
            }
            else if (sender is BPLabel)
            {
                BPLabel lab = sender as BPLabel;
                if (lab != null)
                {
                    this.currLab = lab;
                    this.winFrmLab.BindIt(this.currLab);
                }
            }
            else if (sender is BPLink)
            {
                BPLink link = sender as BPLink;
                if (link != null)
                {
                    this.currLink = link;
                    this.winFrmLink.BindIt(this.currLink);
                }
            }
            else if (sender is BPImg)
            {
                BPImg img = sender as BPImg;
                if (img != null)
                    this.winFrmImg.BindIt(img);
            }
            else if (sender is BPImgAth)
            {
                BPImgAth imgAth = sender as BPImgAth;
                if (imgAth != null)
                    this.winFrmImgAth.BindIt(imgAth);
            }
            else if (sender is BPImgSeal)
            {
                BPImgSeal imgSeal = sender as BPImgSeal;
                if (imgSeal != null)
                    this.winFrmImgSeal.BindIt(imgSeal);
            }
            else if (sender is BPEle)
            {
                BPEle el = sender as BPEle;
                if (el != null)
                {
                    this.winFrmEle.BindData(el.Name);
                    this.winFrmEle.Show();
                }
            }
            else if (sender is BPWorkCheck)
            {
                //BPWorkCheck workCheck = sender as BPWorkCheck;
                //if (workCheck != null)
                //    this.winWorkCheck.BindIt(workCheck);

                string url = Glo.BPMHost + @"/WF/Comm/RefFunc/UIEn.aspx?EnName=BP.Sys.FrmWorkCheck&PK=" + Glo.FK_MapData.Replace("ND", "");
                Glo.WinOpenDialog(url);
            }
            else
            {
                string host = Glo.BPMHost + "/WF/MapDef/Do.aspx?DoType=CCForm";

                if (sender is BPTextBox)
                {
                    BPTextBox tb = sender as BPTextBox;
                    if (tb != null)
                    {
                        if (tb.NameOfReal == null)
                            return;
                        string keyName = HttpUtility.UrlEncode(tb.KeyName);
                        string url = host + "&FK_MapData=" + Glo.FK_MapData + "&MyPK=" + Glo.FK_MapData + "_" + tb.Name + "&DataType=" + tb.HisDataType + "&GroupField=0&LGType=" + LGType.Normal + "&KeyOfEn=" + tb.Name + "&UIContralType=" + CtrlType.TextBox + "&KeyName=" + keyName + Glo.TimeKey;
                        Glo.WinOpenDialog(url, 500, 600);
                    }
                }
                else if (sender is BPCheckBox)
                {
                    BPCheckBox cb = sender as BPCheckBox;
                    if (cb != null)
                    {
                        string keyName = HttpUtility.UrlEncode(cb.KeyName);
                        string url = host + "&FK_MapData=" + Glo.FK_MapData + "&MyPK=" + Glo.FK_MapData + "_" + cb.Name + "&DataType=" + DataType.AppBoolean + "&GroupField=0&LGType=" + LGType.Normal + "&KeyOfEn=" + cb.Name + "&UIContralType=" + CtrlType.CheckBox + "&KeyName=" + keyName;
                        Glo.WinOpenDialog(url, 500, 600);
                    }
                }
                else if (sender is BPDatePicker)
                {
                    BPDatePicker dp = sender as BPDatePicker;
                    if (dp != null)
                    {
                        string keyName = HttpUtility.UrlEncode(dp.KeyName);
                        string url = host + "&FK_MapData=" + Glo.FK_MapData + "&MyPK=" + Glo.FK_MapData + "_" + dp.Name + "&DataType=" + dp.HisDateType + "&GroupField=0&LGType=" + LGType.Normal + "&KeyOfEn=" + dp.Name + "&UIContralType=" + CtrlType.TextBox + "&KeyName=" + keyName;
                        Glo.WinOpenDialog(url, 500, 600);
                    }
                }
                else if (sender is BPDDL)
                {
                    BPDDL ddl = sender as BPDDL;
                    if (ddl != null)
                    {
                        string keyName = HttpUtility.UrlEncode(ddl.KeyName);
                        string url = host + "&FK_MapData=" + Glo.FK_MapData + "&MyPK=" + Glo.FK_MapData + "_" + ddl.Name + "&DataType=" + ddl.HisDataType + "&GroupField=0&LGType=" + ddl._HisDataType + "&KeyOfEn=" + ddl.Name + "&UIBindKey=" + ddl.UIBindKey + "&UIContralType=" + CtrlType.DDL + "&KeyName=" + keyName;
                        Glo.WinOpenDialog(url, 500, 600);
                    }
                }
                else if (sender is BPRadioBtn)
                {
                    BPRadioBtn rb = sender as BPRadioBtn;
                    if (rb != null)
                    {
                        string keyName = HttpUtility.UrlEncode(rb.KeyName);
                        string url = host + "&FK_MapData=" + Glo.FK_MapData + "&MyPK=" + Glo.FK_MapData + "_" + rb.GroupName + "&DataType=" + DataType.AppInt + "&GroupField=0&LGType=" + LGType.Enum + "&KeyOfEn=" + rb.GroupName + "&UIBindKey=" + rb.UIBindKey + "&UIContralType=" + CtrlType.RB;
                        Glo.WinOpenDialog(url, 500, 600);
                    }
                }
                else if (sender is BPDtl)
                {
                    BPDtl dtl = sender as BPDtl;
                    if (dtl != null)
                    {
                        Glo.OpenDtl(Glo.FK_MapData, dtl.Name);
                    }
                }
                else if (sender is BPM2M)
                {
                    BPM2M m2m = sender as BPM2M;
                    if (m2m != null)
                    {
                        Glo.OpenM2M(Glo.FK_MapData, m2m.Name + Glo.TimeKey);
                    }
                }
                else if (sender is BPAttachment)
                {
                    BPAttachment ath = sender as BPAttachment;
                    if (ath != null)
                    {
                        //this.winSelectAttachment.BindIt(ath);
                        string url = Glo.BPMHost + "/WF/MapDef/Attachment.aspx?FK_MapData=" + Glo.FK_MapData + "&Ath=" + ath.Name + Glo.TimeKey;
                        Glo.WinOpen(url, 600, 800);

                    }
                }
                else if (sender is BPAttachmentM)
                {
                    BPAttachmentM athm = sender as BPAttachmentM;
                    if (athm != null)
                    {
                        //this.winFrmAttachmentM.BindIt(athm);
                        //this.winFrmAttachmentM.Show();

                        string url = Glo.BPMHost + "/WF/MapDef/Attachment.aspx?FK_MapData=" + Glo.FK_MapData + "&Ath=" + athm.Name + Glo.TimeKey;
                        Glo.WinOpen(url, 600, 800);
                    }
                }
            }
        }

        private void lbTools_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string toolName = string.Empty;
            e.Handled = true;

            FrameworkElement ele = e.OriginalSource as FrameworkElement;
            ele = sender as FrameworkElement;
            if (ele is ListBox)
            {
                CCForm.ToolBox o = lbTools.SelectedValue as CCForm.ToolBox;
                toolName = o.IcoName;
            }
            else return;

            this.SetSelectedTool(toolName);

            //  Draw the line   And marquee   Need to manually cancel 
            if (ToolBox.Line.Equals(toolName) || ToolBox.Selected.Equals(toolName))
                return;

            isToolDraging = true;
            if (null != cursor)
            {
                if (this.isToolDraging)
                    cCursor.SetCursorTemplate(cursor);
                else
                    cCursor.SetCursorDefault(Cursors.Arrow);
            }
            else
            {
                if (this.isToolDraging)
                    cCursor.SetCursorDefault(Cursors.Hand);
                else
                    cCursor.SetCursorDefault(Cursors.Arrow);
            }


            // Other elements 
            if ( Glo.IsDbClick)
            {// ToolboxItem Double-click Add a new element 
               
                addNewElementToWorkSpace(null);
            }
            else
            {// ToolboxItem  Drag and drop elements 
               
            }
        }

        private void lbTools_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            isToolDraging = false;
            cCursor.SetCursorDefault(Cursors.Arrow);
        }

        private void workSpace_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pFrom = e.GetPosition(workSpace);
            delPoint();
            this.SetSelectedState(false);

            //  Cancel rectangular selection 
            if (selectState != StateRectangleSelected.SelectDisposed)
            {
                selectState = StateRectangleSelected.SelectDisposed;
                RemoveRect();
            }

            if (selectType.Equals(ToolBox.Line))
            {
                try
                {
                    this.SetSelectedState(false);
                    this.isDrawingLine = true;
                    this.isPointSelected = false;

                    currLine = new BPLine("Black", 2, pFrom.X, pFrom.Y, pFrom.X, pFrom.Y);
                    string name = getElementNameFromUI(currLine);
                    name = name.Contains(Glo.FK_MapData) ? name : Glo.FK_MapData +"_"+ name;
                    currLine.Name = name;
                    this.selectedElements.Add(currLine);
                    Glo.currEle = currLine;

                    attachElementEvent(currLine);
                }
                catch (Exception ex)
                {
                    BP.SL.LoggerHelper.Write(ex);
                    currLine = null;
                    Glo.currEle = null;
                }
            }
            else if (selectType.Equals(ToolBox.Selected))
            {

                //  Rectangular Selection 
                //if (selectState != StateRectangleSelected.SelectDisposed )
                //{
                //    selectState = StateRectangleSelected.SelectDisposed;
                //    RemoveRect();
                //}
                //else
                if (e.OriginalSource is Canvas)
                {
                    selectState = StateRectangleSelected.SelectBegin;
                    RectSelected = new Rectangle();

                    RectSelected.BindDrag();
                    RectSelected.MouseLeftButtonDown += new MouseButtonEventHandler(rectSelected_MouseLeftButtonDown);
                    RectSelected.MouseMove += rectSelected_MouseMove;
                    RectSelected.MouseEnter += rectSelected_MouseEnter;
                    RectSelected.MouseLeave += rectSelected_MouseLeave;
                    RectSelected.SetValue(Canvas.LeftProperty, pFrom.X);
                    RectSelected.SetValue(Canvas.TopProperty, pFrom.Y);
                    RectSelected.Fill = new SolidColorBrush(Color.FromArgb(255,201, 224, 252));//r.FromArgb(201,224,252)
                    RectSelected.Stroke = new SolidColorBrush(Color.FromArgb(255, 36, 93, 219));
                    RectSelected.StrokeThickness = 1;
                    RectSelected.Opacity = 0.3;
                    workSpace.Children.Add(RectSelected);

                }
            }
            else if (selectType.Equals(ToolBox.Mouse))
            {//  In the work area to detect whether or not the top level element 
                #region
                //foreach (var item in this.workSpace.Children)
                //{
                //    if (item is IElement)
                //    {
                //        FrameworkElement element = item as FrameworkElement;
                //        double zIndex = (double)element.GetValue(Canvas.ZIndexProperty),
                //            left = (double)element.GetValue(Canvas.LeftProperty),
                //            top = (double)element.GetValue(Canvas.TopProperty),
                //            width = element.ActualWidth,
                //            height = element.ActualHeight;
                //        int band = 5;
                //        if (left - band < pFrom.X && pFrom.X < left + width + band &&
                //            top - band < pFrom.Y && pFrom.Y < top + height + band)
                //        {
                //            if (null == Glo.currEle)
                //                Glo.currEle = element;
                //            else if ((double)Glo.currEle.GetValue(Canvas.ZIndexProperty) < zIndex)
                //            {
                //                Glo.currEle = element;
                //            }
                //        }
                //    }
                //}
                #endregion
            }
            else
            {

            }
        }

        private void workSpace_MouseMove(object sender, MouseEventArgs e)
        {
          
            #region  Draw the line 
            if (this.isDrawingLine && this.currLine != null && this.selectType == ToolBox.Line)
            {
                currLine.MyLine.X2 = e.GetPosition(this.workSpace).X;
                currLine.MyLine.Y2 = e.GetPosition(this.workSpace).Y;
                double x = currLine.MyLine.X1 - currLine.MyLine.X2;
                double y = currLine.MyLine.Y1 - currLine.MyLine.Y2;
                if (Math.Abs(x) > Math.Abs(y))
                {
                    /* A horizontal line  */
                    currLine.MyLine.Y2 = currLine.MyLine.Y1;
                }
                else
                {
                    currLine.MyLine.X2 = currLine.MyLine.X1;
                }
                return;
            }
            #endregion  Draw the line 

            #region  Change the length of the line 
            if (selectType == ToolBox.Mouse && isPointSelected == true)
            {
                lineSizeChange(e.GetPosition(this.workSpace));
                return;
            }
            #endregion

            #region  Rectangular Selection .
            if (this.selectType == ToolBox.Selected && selectState == StateRectangleSelected.SelectBegin && RectSelected != null) /*   Update rect  Size   */
            {
                cCursor.SetCursorTemplate(Resources[this.selectType] as DataTemplate);
                Point curPoint = e.GetPosition(workSpace);
                if (curPoint.X > pFrom.X)
                {
                    RectSelected.Width = curPoint.X - pFrom.X;
                }
                if (curPoint.X < pFrom.X)
                {
                    RectSelected.SetValue(Canvas.LeftProperty, curPoint.X);
                    RectSelected.Width = pFrom.X - curPoint.X;
                }
                if (curPoint.Y > pFrom.Y)
                {
                    RectSelected.Height = curPoint.Y - pFrom.Y;
                }
                if (curPoint.Y < pFrom.Y)
                {
                    RectSelected.SetValue(Canvas.TopProperty, curPoint.Y);
                    RectSelected.Height = pFrom.Y - curPoint.Y;
                }

                return;
            }
            #endregion  Rectangular Selection .


            if ( null != Glo.currEle &&  Glo.currEle is BPLine )
            {
                //IElement ie = Glo.currEle as IElement;
                //if (ie.TrackingMouseMove)
                //{
                //    currLine = Glo.currEle as BPLine;
                  
                //    Point curPoint = e.GetPosition(workSpace);

                //    double deltaV = curPoint.Y - pFrom.Y;
                //    double deltaH = curPoint.X - pFrom.X;
                //    currLine.UpdatePos(deltaH, deltaV);
                //    Glo.ViewNeedSave = true;

                //    pFrom = curPoint;

                //}
                return;
            }
        }

        private void workSpace_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CloseMenu();
           
            this.setUnTracking();
           
            this.isPointSelected = false;
            this.isDrawingLine = false;
          

            if (Keyboard.Modifiers == ModifierKeys.Control && MouseEventHandlers.IsCopy)
            {// Copy 
                this.Paste();
                return;
            }

            if (isToolDraging)
            {//  Drag and drop controls 
                addNewElementToWorkSpace(e);
                return;
            }

           
            if (eCurrent != null)
            { //  Current point selection .
                eCurrent.Fill = new SolidColorBrush(Colors.Green);
                return;
            }


            //  Rectangular Selection 
            if (selectState == StateRectangleSelected.SelectBegin)
            {
                selectState = StateRectangleSelected.SelectComplete;
            
                SelectUIElement();
                cCursor.SetCursorDefault(Cursors.Arrow);
            }
            //else if (!(e.OriginalSource is Rectangle))
            //    RemoveRect();

        }

        private void workSpace_MouseEnter(object sender, MouseEventArgs e)
        {
            if (null != cursor && (this.isToolDraging || this.selectType.Equals(ToolBox.Line)))
            {
                cCursor.SetCursorTemplate(cursor);
            }
        }

        private void workSpace_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!this.isToolDraging)
                cCursor.SetCursorDefault(Cursors.Arrow);
        }

        int positionOffset = 0;
        private void addNewElementToWorkSpace(MouseButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(selectType))
            {
                this.SetSelectedTool(ToolBox.Mouse);
                return;
            }

            if (ToolBox.Line.Equals(selectType) || ToolBox.Selected.Equals(selectType))
                return;

            Point point = new Point(50, 50);
            if (null != e)
            {
                point = e.GetPosition(this.workSpace);
                positionOffset = 0;
            }
            else
            {
                positionOffset += 10;
                point = new Point(point.X + positionOffset, point.Y + positionOffset);
            }
            Glo.X = point.X;
            Glo.Y = point.Y;

          
            #region
            try
            {
                switch (selectType)
                {
                    case ToolBox.Mouse:
                        break;

                    case ToolBox.Btn:
                        BPBtn btn = new BPBtn();
                        this.winFrmBtn.HisBtn = btn;
                        Glo.currEle = btn;
                        this.winFrmBtn.Show();

                        break;
                    case ToolBox.Label: /*  Label . */
                        BPLabel lab = new BPLabel();
                        Glo.currEle = lab;
                        lab.SetValue(Canvas.LeftProperty, point.X);
                        lab.SetValue(Canvas.TopProperty, point.Y);

                        attachElementEvent(lab);

                        break;
                    case ToolBox.Link: /* Link. */
                        BPLink link = new BPLink();
                        Glo.currEle = link;
                        link.SetValue(Canvas.LeftProperty, point.X);
                        link.SetValue(Canvas.TopProperty, point.Y);

                        attachElementEvent(link);
                        break;
                 
                    case ToolBox.TextBox:  //  Textbox 
                        this.winSelectTB.Show();
                        this.winSelectTB.RB_String.IsChecked = true;

                        break;
                    case ToolBox.DateCtl:  
                        this.winSelectTB.RB_Data.IsChecked = true;
                        this.winSelectTB.Show();

                        break;
                    case ToolBox.CheckBox:
                        this.winSelectTB.RB_Boolen.IsChecked = true;
                        this.winSelectTB.CB_IsGenerLabel.IsChecked = false;
                        this.winSelectTB.Show();

                        break;
                    case ToolBox.RBS:  
                        this.winSelectRB.Show();
                        this.IsRB = true;

                        break;
                    case ToolBox.DDLEnum:  
                        this.winSelectRB.Show();
                        this.IsRB = false;

                        break;
                    case ToolBox.DDLTable:  // DDL.
                        this.winSelectDDL.Show();

                        break;
                    case ToolBox.Img:// Decorative Image 
                        BPImg bpImg = new BPImg();
                        Glo.currEle = bpImg;
                        string name = getElementNameFromUI(bpImg);

                        bpImg.Name = name;
                        bpImg.TB_CN_Name = name;
                        bpImg.TB_En_Name = name;
                        bpImg.SetValue(Canvas.LeftProperty, point.X);
                        bpImg.SetValue(Canvas.TopProperty, point.Y);

                        attachElementEvent(bpImg);
                        break;
                    case ToolBox.SealImg:// Signature 
                        BPImgSeal bpImgSeal = new BPImgSeal();
                        Glo.currEle = bpImgSeal;
                        name = getElementNameFromUI(bpImgSeal);

                        bpImgSeal.Name = name;
                        bpImgSeal.TB_CN_Name = name;
                        bpImgSeal.TB_En_Name = name;
                        bpImgSeal.SetValue(Canvas.LeftProperty, point.X);
                        bpImgSeal.SetValue(Canvas.TopProperty, point.Y);

                        attachElementEvent(bpImgSeal);

                        break;
                    case ToolBox.FrmEle:
                        this.winFrmEle.SetBlank();
                        this.winFrmEle.Show();

                        break;
                    case ToolBox.Attachment:  //  Accessory 
                        BPAttachment bpAth = new BPAttachment() { X = Glo.X, Y = Glo.Y };
                        Glo.currEle = bpAth;
                        this.winSelectAttachment.BindIt(bpAth);
                        this.winSelectAttachment.Show();

                        break;
                    case ToolBox.AttachmentM:  //  More Accessories 
                        BPAttachmentM myAthM = new BPAttachmentM();
                        Glo.currEle = myAthM;
                        name = getElementNameFromUI( myAthM);

                        myAthM.X = Glo.X;
                        myAthM.Y = Glo.Y;
                        myAthM.Name = name;
                        myAthM.Label = "";
                        myAthM.SaveTo = @"/DataUser/UploadFile/";
                        myAthM.IsDelete = true;
                        myAthM.IsDownload = true;
                        myAthM.IsUpload = true;
                    
                        this.winFrmAttachmentM.BindIt(myAthM);
                        this.winFrmAttachmentM.Show();

                        break;
                    case ToolBox.Dtl:

                        BPDtl newDtl = new BPDtl();
                        Glo.currEle = newDtl;
                        name = getElementNameFromUI(newDtl);
                        name = name.Contains(Glo.FK_MapData) ? name : Glo.FK_MapData + "_" + name;
                        newDtl.Name = name ;
                        newDtl.NewDtl();

                        newDtl.SetValue(Canvas.LeftProperty, point.X);
                        newDtl.SetValue(Canvas.TopProperty, point.Y);

                        attachElementEvent(newDtl);

                        break;
                    case ToolBox.WorkCheck:
                        #region
                        if (!Glo.FK_MapData.Contains("ND"))
                        {
                            MessageBox.Show(" Only nodes form support auditing components ", "ERROR", MessageBoxButton.OK);
                            break;
                        }

                        // Forms can only increase an audit component 
                        bool find = false;
                        foreach (UIElement ctl in this.workSpace.Children)
                        {
                            if(ctl is IElement )
                                if (ctl is BPWorkCheck)
                                {
                                    if ((ctl as BPWorkCheck) == null)
                                        continue;

                                    find = true;
                                    MessageBox.Show(" The same form is not allowed to add two audit components ", "ERROR", MessageBoxButton.OK);
                                    break;
                                }
                        }

                        if (!find)
                        {
                            BPWorkCheck wkCheck = new BPWorkCheck();
                            Glo.currEle = wkCheck;
                            wkCheck.SetValue(Canvas.LeftProperty, point.X);
                            wkCheck.SetValue(Canvas.TopProperty, point.Y);
                            attachElementEvent(wkCheck);
                        }
                        #endregion

                        break;
                    case ToolBox.M2M:
                        this.winSelectM2M.IsM2M = 0;
                        this.winSelectM2M.Show();

                        break;
                    case ToolBox.M2MM:

                        this.winSelectM2M.IsM2M = 1;
                        this.winSelectM2M.X = Glo.X;
                        this.winSelectM2M.Y = Glo.Y;
                        this.winSelectM2M.Show();
                        break;
                    case ToolBox.ImgAth:

                        BPImgAth ath = new BPImgAth();
                        Glo.currEle = ath;
                        name = getElementNameFromUI( ath);

                        ath.Name = name;
                        ath.CtrlID = name; //  Accessory ID.
                        ath.SetValue(Canvas.LeftProperty, point.X);
                        ath.SetValue(Canvas.TopProperty, point.Y);
                        attachElementEvent(ath);
                        break;

                    default:
                        MessageBox.Show(" Function is not complete :" + selectType, " Please look forward to ", MessageBoxButton.OK);
                        break;
                }
            }
            catch (Exception ee) { BP.SL.LoggerHelper.Write(ee); }
            #endregion

            if (!this.selectType.Equals(ToolBox.Line))
                this.SetSelectedTool(ToolBox.Mouse);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="isAdded"> Are Added </param>
        /// <returns></returns>
        bool attachElementEvent(FrameworkElement element)
        {

            bool flag = false;
            if (null == element) return flag;

            Glo.currEle = element;

            try
            {
                flag = !IsExist(element.Name);

                if (flag)
                {
                    if (!this.workSpace.Children.Contains(element))
                        this.workSpace.Children.Add(element);
                    
                    if (element is IRouteEvent)
                    {
                        IRouteEvent route = element as IRouteEvent;
                        if (null != route)
                        {
                            route.LeftDown += UIElement_LeftButtonDown;
                            route.LeftUp += UIElement_MouseLeftButtonUp;
                        }
                    }
                    else
                    {
                        element.MouseLeftButtonDown += UIElement_LeftButtonDown;
                        element.MouseLeftButtonUp += UIElement_MouseLeftButtonUp;
                    }
                    element.MouseRightButtonDown += UIElement_MouseRightButtonDown;
                    element.MouseEnter += UIElement_MouseEnter;
                    element.MouseLeave += UIElement_MouseLeave;

                    if( element is BPLine)
                        (element as BPLine).Moved += new LineMoved(BPLine_MouseMove);
                    element.Cursor = Cursors.Hand;

                    Glo.ViewNeedSave = true;
                }
                else
                {
                    MessageBox.Show(" Already exists ID为" + element.Name + " Elements , Not allowed to add elements of the same name !", "CCForm Prompt :", MessageBoxButton.OK);
                }

            }
            catch (Exception ex) 
            {
                flag = false;
                BP.SL.LoggerHelper.Write(ex);
                MessageBox.Show(" Controls ID:" + element.Name + " Add error . Error Info:" + ex.Message+"\n trace:"+ex.StackTrace);
            }
            return flag;
        }

        public bool IsExist(string name)
        {
            bool flag = false;
            foreach (FrameworkElement ele in this.workSpace.Children)
            {
                if (ele.Name == name)
                {
                    flag = true ;
                    break;
                }
            }
            return flag;
        }

        private void HidCurrSelectUI()
        {
            if (MessageBox.Show(" Are you sure you want to hide selected elements ?", " Perform validation ", MessageBoxButton.OKCancel)
                == MessageBoxResult.No)
                return;

            BPRadioBtn rb = Glo.currEle as BPRadioBtn;
            if (rb != null)
            {
                rb.DeleteIt();
                return;
            }

            if (this.workSpace.Children.Contains(Glo.currEle))
            {
                BPTextBox tb = Glo.currEle as BPTextBox;
                if (tb != null)
                {
                    tb.HidIt();
                    return;
                }

                BPDDL ddl = Glo.currEle as BPDDL;
                if (ddl != null)
                {
                    ddl.HidIt();
                    return;
                }

                BPDtl dtl = Glo.currEle as BPDtl;
                if (dtl != null)
                {
                    dtl.DeleteIt();
                    return;
                }

                BPM2M m2m = Glo.currEle as BPM2M;
                if (m2m != null)
                {
                    m2m.DeleteIt();
                    return;
                }

                MessageBox.Show(" Elements that you selected does not support hidden ", " Execution error ", MessageBoxButton.OK);
                return;
            }
            Glo.currEle = null;
        }
        private void DeleteCurrSelectUI()
        {
            int number = this.selectedElements.Count;
            if (number == 0)
            {
                MessageBox.Show(" You did not choose to delete the object , Prompt : Press ctrl Can achieve multiple choice .", " Batch delete tips ", MessageBoxButton.OK);
                return;
            }
            else if (number > 2)
            {
                string alter = " Total  (" + number + ")  Objects are selected , Are you sure you want to delete them ?\n\n Prompt : Press ctrl Can achieve multiple choice .";
                if (MessageBox.Show(alter, " Batch delete tips ", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                    return;
            }

            for (int i = 0; i < number; i++)
            {
                FrameworkElement item = this.selectedElements[i];
                if (item is IElement && this.workSpace.Children.Contains(item))
                {
                    if (item is IDelete)
                    {
                        (item as IDelete).DeleteIt();
                        continue;
                    }
                    else
                        this.workSpace.Children.Remove(item);
                }
            }

            this.selectedElements.Clear();

            if (Glo.currEle != null)
            {
                Glo.currEle = null;
                this.delPoint();
            }

            this.SetGridLines(this.workSpace, true);
        }
      
        private void Paste()
        {
            if (!MouseEventHandlers.IsCopy)
                return;

            List<FrameworkElement> copyEles = new List<FrameworkElement>();
            string name = "";
            foreach (FrameworkElement item in this.selectedElements)
            {
                if (item is BPLine)
                {
                    BPLine line = item as BPLine;
                    if (line != null)
                    {
                        BPLine lineN = new BPLine( line.Color, line.MyLine.StrokeThickness,
                            line.MyLine.X1 + 10, line.MyLine.Y1 + 10, line.MyLine.X2 + 10, line.MyLine.Y2 + 10);
                        lineN.Name = getElementNameFromUI(lineN);
                        copyEles.Add(lineN);
                        attachElementEvent(lineN);
                    }
                }
                else if (item is BPLabel)
                {
                    BPLabel lab = item as BPLabel;
                    if (lab != null)
                    {
                        name = getElementNameFromUI( lab);
                        BPLabel labN = new BPLabel()
                        {
                            Name = name,
                            Content = lab.Content,
                            Foreground = lab.Foreground,
                            FontSize = lab.FontSize,
                            FontWeight = lab.FontWeight
                        };

                        labN.SetValue(Canvas.LeftProperty, (double)lab.GetValue(Canvas.LeftProperty) + 16);
                        labN.SetValue(Canvas.TopProperty, (double)lab.GetValue(Canvas.TopProperty) + 16);

                        attachElementEvent(labN);
                        copyEles.Add(labN);
                    }
                }
                else if (item is BPLink)
                {
                    BPLink link = item as BPLink;
                    if (link != null)
                    {
                        name = getElementNameFromUI( link);
                        BPLink labN = new BPLink()
                        {
                            Name = name,
                            Content = link.Content,
                            Foreground = link.Foreground,
                            FontSize = link.FontSize,
                            FontWeight = link.FontWeight
                        };
                        labN.SetValue(Canvas.LeftProperty, (double)link.GetValue(Canvas.LeftProperty) + 16);
                        labN.SetValue(Canvas.TopProperty, (double)link.GetValue(Canvas.TopProperty) + 16);

                        attachElementEvent(labN);
                        copyEles.Add(labN);
                    }
                }
                else if (item is BPTextBox)
                {
                    BPTextBox tb = item as BPTextBox;
                    if (tb != null )// Was never set up conditions ,textbox Copy suspended .
                    {
                        name = getElementNameFromUI(tb);
                        BPTextBox tbN = new BPTextBox()
                        {
                            TextAlignment = tb.TextAlignment,
                            Text = tb.Text,
                            Name = name,
                            Width = tb.Width,
                            Height = tb.Height,
                            HisTBType = tb.HisTBType,
                            IsReadOnly = tb.IsReadOnly,

                            Background = new SolidColorBrush(Colors.Orange)
                        };
                        tbN.SetValue(Canvas.LeftProperty, (double)tb.GetValue(Canvas.LeftProperty) + 16);
                        tbN.SetValue(Canvas.TopProperty, (double)tb.GetValue(Canvas.TopProperty) + 16);

                        attachElementEvent(tbN);
                        copyEles.Add(tbN);
                    }
                }
                else if (item is BPImg)
                {
                    BPImg img = item as BPImg;
                    if (img != null)
                    {
                        MessageBox.Show(" Decorative image copying functions unfinished .");
                        return;

                        //  name = "Img" + timeKey + "_" + idx.ToString();
                        //  BPImg labN = new BPImg();
                        //  labN.Name = name;
                        ////  labN.Content = lab.Content;
                        //  labN.SetValue(Canvas.LeftProperty, (double)lab.GetValue(Canvas.LeftProperty) + 16);
                        //  labN.SetValue(Canvas.TopProperty, (double)lab.GetValue(Canvas.TopProperty) + 16);
                        //  labN.Foreground = lab.Foreground;
                        //  this.workSpace.Children.Add(labN);

                        //  labN.MouseLeftButtonDown += new MouseButtonEventHandler(UIElement_Click);
                        //  labN.MouseRightButtonDown += new MouseButtonEventHandler(UIElement_MouseRightButtonDown);

                        //  copyEles.Add(labN);
                    }
                }
            }
            this.SetSelectedState(false);
            this.selectedElements = copyEles;
            this.SetSelectedState(true);

        }

        private void ToolBar_Click(object sender, RoutedEventArgs e)
        {
            string id = "";
            FrameworkElement ele = sender as FrameworkElement;
            if( ele != null)
                id = ele.Tag.ToString();
            else return;

            switch (id)
            {
                case EleFunc.SelectAll:

                    selectAll();
                    break;
                case EleFunc.CopyEle:

                    MouseEventHandlers.IsCopy = true;
                    break;
                case EleFunc.Paste:

                    this.Paste();
                    break;
                case EleFunc.FontSizeAdd:

                    foreach (FrameworkElement item in this.selectedElements)
                    {
                        BPLabel lab = item as BPLabel;
                        if (lab != null)
                        {
                            lab.FontSize = lab.FontSize + 1;
                        }

                        BPLink link = item as BPLink;
                        if (link != null )
                        {
                            link.FontSize = link.FontSize + 1;
                        }

                        BPLine line = item as BPLine;
                        if (line != null )
                        {
                            line.MyLine.StrokeThickness = line.MyLine.StrokeThickness + 2;//re,1
                        }
                    }
                    break;
                case EleFunc.FontSizeCut:
                    foreach (FrameworkElement item in this.selectedElements)
                    {
                        BPLabel lab = item as BPLabel;
                        if (lab != null)
                        {
                            if (lab.FontSize < 8)
                                continue;

                            lab.FontSize = Math.Abs(lab.FontSize - 1);
                        }

                        BPLink link = item as BPLink;
                        if (link != null )
                        {
                            if (link.FontSize < 8)
                                continue;

                            link.FontSize = Math.Abs(link.FontSize - 1);
                        }

                        BPLine line = item as BPLine;
                        if (line != null )
                        {
                            if (line.MyLine.StrokeThickness < 3)//re,0.5
                                continue;
                            line.MyLine.StrokeThickness = Math.Abs(line.MyLine.StrokeThickness - 2);//re,1
                        }
                    }
                    break;
                case EleFunc.Colorpicker:

                    ColorPickerWin cw = new ColorPickerWin();
                    cw._ColorChanged += new ColorPickerWin.ColorChanged(ColorChanged);
                    cw.Show();
                    break;
                case EleFunc.Bold:
                    foreach (FrameworkElement item in this.selectedElements)
                    {
                        BPLabel lab = item as BPLabel;
                        if (lab != null)
                        {
                            if (lab.FontWeight == FontWeights.Bold)
                                lab.FontWeight = FontWeights.Normal;
                            else
                                lab.FontWeight = FontWeights.Bold;
                        }

                        BPLink link = item as BPLink;
                        if (link != null)
                        {
                            if (link.FontWeight == FontWeights.Bold)
                                link.FontWeight = FontWeights.Normal;
                            else
                                link.FontWeight = FontWeights.Bold;
                        }
                    }
                    break;
                case Func.Property:

                    this.winFrmOp.Show();
                    break;
                case Func.Alignment_Down:// Align Bottom 
                    if (this.selectedElements.Count == 0)
                    {
                        MessageBox.Show(" You must select two or more controls to perform the alignment .");
                        return;
                    }

                    double maxY = 0;
                    foreach (FrameworkElement item in this.selectedElements)
                    {
                        double dY = Canvas.GetTop(item) + item.ActualHeight;
                        if (maxY < dY)
                        {
                            maxY = dY;
                        }
                    }

                    foreach (FrameworkElement item in this.selectedElements)
                    {
                        item.SetValue(Canvas.TopProperty, maxY - item.ActualHeight);
                    }
                    break;
                case Func.Alignment_Top:
                    if (this.selectedElements.Count == 0)
                    {
                        MessageBox.Show(" You must select two or more controls to perform the alignment .");
                        return;
                    }
                    double minY = 1000;
                    foreach (FrameworkElement item in this.selectedElements)
                    {
                        //MatrixTransform transform = item.TransformToVisual(this.workSpace) as MatrixTransform;
                        //double y = transform.Matrix.OffsetY;
                        double dY = Canvas.GetTop(item);
                        if (minY > dY)
                            minY = dY;
                    }
                    foreach (FrameworkElement item in this.selectedElements)
                    {
                        item.SetValue(Canvas.TopProperty, minY);
                    }
                    break;
                case Func.Alignment_Left:
                    if (this.selectedElements.Count == 0)
                    {
                        MessageBox.Show(" You must select two or more controls to perform the alignment .");
                        return;
                    }

                    double minX = 1000;
                    foreach (FrameworkElement item in this.selectedElements)
                    {
                        MatrixTransform transform = item.TransformToVisual(this.workSpace) as MatrixTransform;
                        double x = transform.Matrix.OffsetX;
                        if (x <= 0)
                            continue;

                        if (minX > x)
                            minX = x;
                    }

                    foreach (FrameworkElement item in this.selectedElements)
                    {
                        item.SetValue(Canvas.LeftProperty, minX);
                    }
                    break;
                case Func.Alignment_Right:
                    if (this.selectedElements.Count == 0)
                    {
                        MessageBox.Show(" You must select two or more controls to perform the alignment .");
                        return;
                    }
                    double maxX = 0;
                    foreach (FrameworkElement item in this.selectedElements)
                    {
                        MatrixTransform transform = item.TransformToVisual(this.workSpace) as MatrixTransform;
                        double x = transform.Matrix.OffsetX + item.ActualWidth;
                        if (maxX < x)
                        {
                            maxX = x;
                        }
                    }
                    foreach (FrameworkElement item in this.selectedElements)
                    {
                        MatrixTransform transform = item.TransformToVisual(this.workSpace) as MatrixTransform;
                        item.SetValue(Canvas.LeftProperty, maxX - item.ActualWidth);
                    }
                    break;
                case Func.Alignment_Center:
                    if (this.selectedElements.Count == 0)
                    {
                        MessageBox.Show(" You must select two or more controls to perform the alignment .");
                        return;
                    }
                    double miX = 1000, maX = 0; /*  Seeking maximum ,  Least */
                    foreach (FrameworkElement item in this.selectedElements)
                    {
                        MatrixTransform transform = item.TransformToVisual(this.workSpace) as MatrixTransform;
                        double x = transform.Matrix.OffsetX + item.ActualWidth;
                        if (maX < x)
                            maX = x;
                        if (miX > transform.Matrix.OffsetX)
                            miX = x;
                    }
                    double miudeLine = (maX - miX) / 2 + miX;
                    foreach (FrameworkElement item in this.selectedElements)
                    {
                        item.SetValue(Canvas.LeftProperty, miudeLine - item.ActualWidth / 2);
                    }
                    break;
                
                case Func.View:

                    try
                    {
                        string url1 = null;
                        if (Glo.IsDtlFrm == false)
                            url1 = Glo.BPMHost + "/WF/CCForm/Frm.aspx?FK_MapData=" + Glo.FK_MapData + "&IsTest=1&WorkID=0&FK_Node=" + Glo.FK_Node + "&s=2" + Glo.TimeKey;
                        else
                            url1 = Glo.BPMHost + "/WF/CCForm/FrmCard.aspx?EnsName=" + Glo.FK_MapData + "&RefPKVal=0&OID=0" + Glo.TimeKey;

                        Glo.WinOpen(url1, (int)Glo.HisMapData.FrmH, (int)Glo.HisMapData.FrmW);

                    }
                    catch (Exception ee)
                    {
                        BP.SL.LoggerHelper.Write(ee);
                    }

                    break;
                case Func.Exp:
                    Glo.WinOpen(Glo.BPMHost + "/WF/Admin/XAP/DoPort.aspx?DoType=DownFormTemplete&FK_MapData=" + Glo.FK_MapData + Glo.TimeKey, 100, 100);
                    if (MessageBox.Show(" Has started export of , If your browser can not normally be downloaded , Click the OK button to directly download the template .", " You see the exported file yet ?", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        Glo.WinOpen(Glo.BPMHost + "/DataUser/Temp/" + Glo.FK_MapData + ".xml", 100, 200);
                    return;
                case Func.MapExt: //  Extended Settings .
                    Glo.WinOpen(Glo.BPMHost + "/WF/MapDef/MapExt.aspx?FK_MapData=" + Glo.FK_MapData + Glo.TimeKey);
                    return;
                case Func.Imp:
                    winFrmImp.Show();
                    break;
                case Func.Delete:
                    this.DeleteCurrSelectUI();
                    break;
                case Func.Save:
                    this.Save();
                    break;
              
                case Func.Copy:
                    this.winFrmImp.Show();
                    break;
                case Func.Event:
                    FrmEvent fa = new FrmEvent();
                    fa.Show();
                    break;
                case Func.HiddenField:
                    new FrmHiddenField().Show();
                    break;
              
                case "Btn_Glo":
                    MessageBox.Show(Glo.currEle.ToString());
                    break;
                case "Btn_Impdddd": // del.
                    this.menuItem_MouseLeftButtonDown(null, null);
                    OpenFileDialog myOpenFileDialog = new OpenFileDialog();
                    myOpenFileDialog.Filter = " Gallop workflow form templates (*.xml)|*.xml|All Files (*.*)|*.*";  //SL Currently only supports jpg和png Format of the image display 
                    myOpenFileDialog.Multiselect = false;// Only allows you to select a picture 
                    if (myOpenFileDialog.ShowDialog() == false)
                        return;

                    string mapImageName = myOpenFileDialog.File.Name.ToString();
                    // Obtaining image flow information , With image Control is bound 
                    FileInfo aFileInfo = myOpenFileDialog.File;
                    FileInfo fileInfoOfMapImage = myOpenFileDialog.File;

                    if (aFileInfo == null)
                        return;
                    Stream mapImageStream = aFileInfo.OpenRead();
                    // Upload Photos 
                    mapImageStream.Position = 0;
                    byte[] buffer = new byte[mapImageStream.Length + 1];
                    mapImageStream.Read(buffer, 0, buffer.Length);
                    String fileName = fileInfoOfMapImage.Name;
                    FF.CCFormSoapClient da = Glo.GetCCFormSoapClientServiceInstance();
                    da.UploadFileAsync(buffer, "\\Temp\\s.xml");
                    da.UploadFileCompleted +=(object senders, FF.UploadFileCompletedEventArgs ee)=>
                    {
                        if( ee.Error == null)
                            this.OpenFormJson(ee.Result);
                    };
                    break;
                default:
                    MessageBox.Show(sender.ToString() + " ID=" + id + "  Function not implemented .");
                    break;
            }
        }

        bool isDesignerSizeChanged;
        void changeFormSize(double width, double height)
        {
            this.workSpace.Width = width;
            this.workSpace.Height = height;
            Glo.HisMapData.FrmH = height;
            Glo.HisMapData.FrmW = width;
            this.isDesignerSizeChanged = true;
        }

        #region BPLine 点
        Ellipse e1, e2;// Select the line after Green Point 
        Ellipse eCurrent;// Selected Green Point 
        bool isPointSelected;// Green Point is on to determine the current state of the mouse pressed .

        void initPoint()
        {
            e1 = new Ellipse();
            e1.Tag = "e1";
            e1.Cursor = Cursors.Hand;
            e1.MouseLeftButtonDown += e_MouseLeftButtonDown;
            e1.Width = 9;
            e1.Height = 9;
            e1.Fill = new SolidColorBrush(Colors.Green);

            e2 = new Ellipse();
            e2.Tag = "e2";
            e2.Cursor = Cursors.Hand;
            e2.MouseLeftButtonDown += e_MouseLeftButtonDown;
            e2.Width = 9;
            e2.Height = 9;
            e2.Fill = new SolidColorBrush(Colors.Green);

        }


        void BPLine_MouseMove(BPLine line)
        {

            e1.SetValue(Canvas.LeftProperty, line.MyLine.X1 - 4);
            e1.SetValue(Canvas.TopProperty, line.MyLine.Y1 - 4);

            e2.SetValue(Canvas.LeftProperty, line.MyLine.X2 - 4);
            e2.SetValue(Canvas.TopProperty, line.MyLine.Y2 - 4);
        }
       
        void e_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            isPointSelected = true;
            eCurrent = sender as Ellipse;
            eCurrent.Fill = new SolidColorBrush(Colors.Red);
        }

        void lineSizeChange(Point p)
        {
            #region  Change the length of the line 

            if (eCurrent.Tag.ToString() == "e1")
            {
                double x = p.X - currLine.MyLine.X2;
                double y = p.Y - currLine.MyLine.Y2;
                if (Math.Abs(x) > Math.Abs(y))
                {
                    currLine.MyLine.X1 = p.X;
                    currLine.MyLine.Y1 = currLine.MyLine.Y2;
                    eCurrent.SetValue(Canvas.LeftProperty, p.X - 4);
                    eCurrent.SetValue(Canvas.TopProperty, currLine.MyLine.Y2 - 4);
                }
                else
                {
                    currLine.MyLine.X1 = currLine.MyLine.X2;
                    currLine.MyLine.Y1 = p.Y;
                    eCurrent.SetValue(Canvas.LeftProperty, currLine.MyLine.X2 - 4);
                    eCurrent.SetValue(Canvas.TopProperty, p.Y - 4);
                }
            }
            else //if (eCurrent.Tag.ToString() == "e2")
            {
                double x = p.X - currLine.MyLine.X1;
                double y = p.Y - currLine.MyLine.Y1;
                if (Math.Abs(x) > Math.Abs(y))
                {
                    currLine.MyLine.X2 = p.X;
                    currLine.MyLine.Y2 = currLine.MyLine.Y1;
                    eCurrent.SetValue(Canvas.LeftProperty, p.X - 4);
                    eCurrent.SetValue(Canvas.TopProperty, currLine.MyLine.Y1 - 4);
                }
                else
                {
                    currLine.MyLine.X2 = currLine.MyLine.X1;
                    currLine.MyLine.Y2 = p.Y;
                    eCurrent.SetValue(Canvas.LeftProperty, currLine.MyLine.X1 - 4);
                    eCurrent.SetValue(Canvas.TopProperty, p.Y - 4);
                }
            }

            #endregion
        }
        void lineEdit(BPLine line)
        {
            if (selectType == ToolBox.Mouse)
            {
                if (!workSpace.Children.Contains(e1) )
                    this.workSpace.Children.Add(e1);
                if (!workSpace.Children.Contains(e2))
                    this.workSpace.Children.Add(e2);
               

                e1.SetValue(Canvas.LeftProperty, line.MyLine.X1 - 4);
                e1.SetValue(Canvas.TopProperty, line.MyLine.Y1 - 4);

                e2.SetValue(Canvas.LeftProperty, line.MyLine.X2 - 4);
                e2.SetValue(Canvas.TopProperty, line.MyLine.Y2 - 4);
            }
        }

        // Point on the main panel to delete the line 
        void delPoint()
        {
            if (workSpace.Children.Contains(e1))
                this.workSpace.Children.Remove(e1);

            if (workSpace.Children.Contains(e2))
                this.workSpace.Children.Remove(e2);
        }
        #endregion

        #region  Shortcut menu 

        private void UIElement_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            if (sender == this.workSpace)
            {
                ShowMenu(this.muFrm, e);
            }
            else if (sender is IElement)
            {
                UIElement ele = sender as UIElement;
               
                if (Glo.currEle != ele)
                {
                    if (Glo.currEle != null)
                    {
                        (Glo.currEle as IElement).IsSelected = false;
                    }

                    (ele as IElement).IsSelected = true;
                    (ele as IElement).TrackingMouseMove = false;
                    Glo.currEle = ele;
                }
              
                ShowMenu(this.muElePanel,e);
            }
          
            else if( sender == this.lbTools)
            {

            }
            else
            {
            }
        }

        static List<Menu> menus = new List<Menu>();
        void CloseMenu()
        {
            foreach (var item in menus)
            {
               item.Hide();
            }
        }
        void ShowMenu(Menu menu, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(this.workSpace);
            if (!this.workSpace.Children.Contains(menu))
                this.workSpace.Children.Add(menu);

            if (!menus.Contains(menu))
                menus.Add(menu);
            CloseMenu();
            //  Adjustment x,y 值 , To prevent the menu is obscured .
            var x = p.X;
            var y = p.Y;

            double menuHeight = 200, menuWidth = 200;
            if (menu == this.muElePanel || menu == this.muFrm)
            {//  Form shortcuts 
                if (menu == this.muElePanel)
                {
                    menuHeight = 180;
                }
                else if (menu == this.muFrm)
                {
                    menuHeight = 270;
                }

                var hostWidth = this.svWorkSpace.ActualWidth;// Application.Current.Host.Content.ActualWidth - 180;
                var hostHeight = this.svWorkSpace.ActualHeight;// Application.Current.Host.Content.ActualHeight - 35;
                
                if (x + menuWidth > hostWidth)
                {
                    x = x - (x + menuWidth - hostWidth);

                    if (!double.IsNaN(this.svWorkSpace.HorizontalOffset))
                        x += this.svWorkSpace.HorizontalOffset;
                }

                double menuTop = y + menuHeight;

                if (double.IsNaN(this.svWorkSpace.VerticalOffset) || this.svWorkSpace.VerticalOffset ==0)
                {
                    if (menuTop > hostHeight)
                    {
                        y = y - (menuTop - hostHeight);
                    }
                }
                else 
                {
                    if (menuTop - this.svWorkSpace.VerticalOffset > hostHeight)
                    {
                        y = y - (menuTop - hostHeight) + this.svWorkSpace.VerticalOffset;
                    }
                }
            }
            else
            {
            
            }


            menu.SetValue(Canvas.LeftProperty, x);
            menu.SetValue(Canvas.TopProperty, y);

            //  Resetting the shortcut menu ZIndex Prevent being covered 
            if (null != Glo.currEle)
            {
                int d = Canvas.GetZIndex(Glo.currEle);
                menu.SetValue(Canvas.ZIndexProperty, ++d);
            }
            menu.Show();
           
        }

        //  Close the shortcut menu in the main menu 
        private void Menu_MouseLeave(object sender, MouseEventArgs e)
        {
            Menu menu = sender as Menu;
            if( null != menu)
                menu.Hide();
        }
       
        private void menuItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            MenuItem tb = sender as MenuItem;

            Menu menu = tb.ParentMenu;
            if (null != menu)
                menu.Hide();

            switch (tb.Name)
            {
                case "FrmTempleteShareIt": // Shared Templates .
                    FrmShare fss = new FrmShare();
                    fss.Show();
                    break;
                case "AdvAction": // Event .
                case "AdvActionExt":
                    FrmEvent fa = new FrmEvent();
                    fa.Show();
                    break;
                case "AdvUAC": // Access Control .
                    FrmUAC fuac = new FrmUAC();
                    fuac.Show();
                    break;
                case "GradeLine":
                case "GradeLine_Ext":
                    this.GradeLine.IsChecked = !this.GradeLine.IsChecked;
                    this.SetGridLines( this.workSpace,this.GradeLine.IsChecked); // Re-draw the line 
                    break;
                case "FullScreen": // Full screen 
                case "FullScreen_Ext": // Full screen 
                    Application.Current.Host.Content.IsFullScreen = !Application.Current.Host.Content.IsFullScreen;
                    break;
                case "FrmTempleteShare":
                    FrmImpFromInternet impFrmI = new FrmImpFromInternet();
                    impFrmI.HisMainPage = this;
                    impFrmI.Show();
                    break;
                case "FrmTempleteExp": // Export Form Template .
                case "FrmTempleteExp_Ext": // Export Form Template .
                    Glo.WinOpen(Glo.BPMHost + "/WF/Admin/XAP/DoPort.aspx?DoType=DownFormTemplete&FK_MapData=" + Glo.FK_MapData + Glo.TimeKey,
                        100, 100);
                    return;
                case "FrmTempleteImp": // Import form templates 
                case "FrmTempleteImp_Ext": // Import form templates 
                    winFrmImp.Show();
                    break;
                case "eleDel":
                    this.DeleteCurrSelectUI();
                    break;
                case "eleCopyTo": // Copy to other sheets 
                    FrmCopyEleTo copyIt = new FrmCopyEleTo();
                    copyIt.Tag = sender;
                    copyIt.Show();
                    break;
                case "eleHid":
                    this.HidCurrSelectUI();
                    break;
                case "eleCancel":
                    break;
                case "eleDtlFrm":
                    BPDtl dtlFrm = Glo.currEle as BPDtl;
                    if (dtlFrm != null)
                    {
                        string url = Glo.BPMHost + "/WF/MapDef/CCForm/Frm.aspx?FK_MapData=" + dtlFrm.Name + "&FK_Node=" + Glo.FK_Node + "&S=2" + Glo.TimeKey;
                        HtmlPage.Window.Eval("window.open('" + url + "','_blank')");
                        return;
                    }
                    else
                    {
                        MessageBox.Show(" The currently selected element is not a list ", " Prompt ", MessageBoxButton.OK);
                    }
                    break;
                case "eleTabIdx":
                case "eleTabIdx_Ext":
                    string url1 = Glo.BPMHost + "/WF/MapDef/TabIdx.aspx?FK_MapData=" + Glo.FK_MapData + Glo.TimeKey;
                    HtmlPage.Window.Eval("window.showModalDialog('" + url1 + "',window,'dialogHeight:500px;dialogWidth:700px;center:Yes;help:No;scroll:auto;resizable:1;status:No;');");
                    return;
                case "eleEdit":

                    UIElementDbClickEdit(Glo.currEle);

                    break;
                case "sysErrorLog":
                    BP.SL.OutputChildWindow.ShowException();
                    break;
                case "refresh":
                    this.BindFrm();
                    break;
                default:
                    MessageBox.Show(tb.Text + "  Function is not complete .", " Stay tuned ", MessageBoxButton.OK);
                    break;
            }
        }
      
        #endregion

        #region  Selected 
        void SetSelectedTool(string id)
        {  //  Setting choice ToolBox. And configure the mouse style 
            this.selectType = id;

            switch (this.selectType)
            {
                case ToolBox.Mouse:

                    MouseEventHandlers.IsCopy = false;
                    this.isDrawingLine = false;
                    this.isToolDraging = false;
                    this.lbTools.SelectedIndex = 0;
                    this.isPointSelected = false;
                    cursor = null;
                    cCursor.SetCursorDefault(Cursors.Arrow);
                    selectState = StateRectangleSelected.SelectDisposed;
                    this.RemoveRect();
                    this.setUnTracking();
                    this.SetSelectedState(false);
                    break;
                default:
                    cursor = null;     
                    cursor = Resources[id] as DataTemplate;
                    break;
            }
        }
        void ColorChanged(Color strColor)
        {
            foreach (FrameworkElement item in this.selectedElements)
            {
                BPLabel lab = item as BPLabel;
                if (lab != null)
                    lab.Foreground = new SolidColorBrush(strColor);

                BPLink link = item as BPLink;
                if (link != null)
                    link.Foreground = new SolidColorBrush(strColor);

                BPLine line = item as BPLine;
                if (line != null)
                {
                    line.Color = Glo.PreaseColorToName(strColor.ToString());
                }
            }
        }
        void setUnTracking()
        {
            Glo.SetTracking(Glo.currEle as IElement, false);
        }
      
        #endregion

        #region   Selection rectangle deal 
        double left = 0.0, top = 0.0;
        void rectSelected_MouseMove(object sender, MouseEventArgs e)
        {

            if (selectState == StateRectangleSelected.SelectComplete)
            {
                left = Convert.ToDouble(RectSelected.GetValue(Canvas.LeftProperty));
                top = Convert.ToDouble(RectSelected.GetValue(Canvas.TopProperty));
                selectState = StateRectangleSelected.SelectMoved;
            }
            foreach (var item in this.selectedElements)
            {
                BPLine line = item as BPLine;
                if (line != null)
                {
                    line.UpdatePos(Convert.ToDouble(RectSelected.GetValue(Canvas.LeftProperty)) - left, Convert.ToDouble(RectSelected.GetValue(Canvas.TopProperty)) - top);
                }
                else
                {
                    item.SetValue(Canvas.LeftProperty, Convert.ToDouble(item.GetValue(Canvas.LeftProperty)) + Convert.ToDouble(RectSelected.GetValue(Canvas.LeftProperty)) - left);
                    item.SetValue(Canvas.TopProperty, Convert.ToDouble(item.GetValue(Canvas.TopProperty)) + Convert.ToDouble(RectSelected.GetValue(Canvas.TopProperty)) - top);

                }
            }
            left = Convert.ToDouble(RectSelected.GetValue(Canvas.LeftProperty));
            top = Convert.ToDouble(RectSelected.GetValue(Canvas.TopProperty));

        }
        void rectSelected_MouseLeave(object sender, MouseEventArgs e)
        {
            cCursor.SetCursorDefault(Cursors.Arrow);
        }

        void rectSelected_MouseEnter(object sender, MouseEventArgs e)
        {
            DataTemplate cursor = Resources["MOVE"] as DataTemplate;
            if (null != cursor)
            {
                cCursor.SetCursorTemplate(cursor);
            }
            else 
                cCursor.SetCursorDefault(Cursors.Hand);
        }

        void rectSelected_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
       
        private void SelectUIElement()
        {
            if (RectSelected == null) return;
           
            this.selectedElements.Clear();
            // Automatic variable adjustment marquee area  
            bool isHaveLine = false;// There is no line judge 
            double finalLeft = double.MaxValue;
            double finalRight = double.MinValue;
            double finalTop = double.MaxValue;
            double finalBottom = double.MinValue;

         
            double Left = Convert.ToDouble(RectSelected.GetValue(Canvas.LeftProperty));
            double Top = Convert.ToDouble(RectSelected.GetValue(Canvas.TopProperty));
            double Right = Left + RectSelected.ActualWidth;
            double Bottom = Top + RectSelected.ActualHeight;

            double left = 0.0;
            double top = 0.0;
            double right = 0.0;
            double bottom = 0.0;

            foreach (UIElement ue in workSpace.Children)
            {
                if (ue is IElement && !object.ReferenceEquals(ue, RectSelected))
                {
                    FrameworkElement c = ue as FrameworkElement;

                    if (c is BPLine)
                    {//  There is no line judge 
                        BPLine line = c as BPLine;
                        isHaveLine = true;
                        left = line.MyLine.X1;
                        top = line.MyLine.Y1;
                        right = line.MyLine.X2;
                        bottom = line.MyLine.Y2;
                    }
                    else
                    {
                        left = Convert.ToDouble(c.GetValue(Canvas.LeftProperty));
                        top = Convert.ToDouble(c.GetValue(Canvas.TopProperty));
                        right = left + c.ActualWidth;
                        bottom = top + c.ActualHeight;
                    }

                    if (Left < right &&left < Right  && Top < bottom && top < Bottom )
                    //if (!(right < Left || bottom < Top || Right < left  ||  Bottom < top ))
                    {
                        this.selectedElements.Add(c);

                        if (finalLeft > left) finalLeft = left;
                        if (finalTop > top) finalTop = top;

                        if (finalRight < right) finalRight = right;
                        if (finalBottom < bottom) finalBottom = bottom;
                    }

                }
            }

            // If there is no line for automatic adjustment marquee area 
            if (!isHaveLine & this.selectedElements.Count > 0)
            {
                RectSelected.SetValue(Canvas.LeftProperty, finalLeft);
                RectSelected.SetValue(Canvas.TopProperty, finalTop);
                RectSelected.Width = Math.Abs(finalRight - finalLeft);
                RectSelected.Height = Math.Abs(finalBottom - finalTop);
            }
            else if (this.selectedElements.Count <= 0)
            {  // Marquee blank area 
                RemoveRect();
            }
            else
                this.SetSelectedState(true);
        }
        void RemoveRect()
        {
            this.SetSelectedState(false);
            if (RectSelected != null)
            {
                RectSelected.MouseLeftButtonDown -= rectSelected_MouseLeftButtonDown;
                RectSelected.MouseMove -= rectSelected_MouseMove;
                RectSelected.MouseEnter -= rectSelected_MouseEnter;
                RectSelected.MouseLeave -= rectSelected_MouseLeave;
                workSpace.Children.Remove(RectSelected);
                selectState = StateRectangleSelected.SelectDisposed;
                RectSelected = null;
            }
        }
        void selectAll()
        {
            this.selectedElements.Clear();
            foreach (FrameworkElement item in this.workSpace.Children)
            {
                if (item is IElement)
                    this.selectedElements.Add(item);
            }
            this.SetSelectedState(true);
        }

        void SetSelectedState(bool isSelected)
        {
            IElement e = null;
            if (isSelected)
            {
                foreach (UIElement en in this.selectedElements)
                {
                    if (en is IElement && (e = en as IElement) != null)
                    {
                        e.IsSelected = isSelected;
                    }
                }
            }
            else
            {
                foreach (UIElement en in this.workSpace.Children)
                {
                    if (en is IElement && (e = en as IElement) != null)
                    {
                        e.IsSelected = isSelected;
                    }
                }

                this.selectedElements.Clear();
            }
        }

        #endregion

        #region  Gets the element default name 
      
        public string getElementNameFromUI( object obj )
        {
            string prefix = string.Empty;
            if (obj is BPAttachment)        prefix = "Attach";
            else if (obj is BPAttachmentM)  prefix = "AttachM";
            else if (obj is BPBtn)          prefix = "Btn";
            else if (obj is BPCheckBox)   { prefix = "Ckb"; }
            else if (obj is BPDatePicker) { prefix = "Date"; }
            else if (obj is BPDDL)        { prefix = "Ddl"; }
            else if (obj is BPDir)        { prefix = "Dir"; }
        
            else if (obj is BPEle)        { prefix = "Ele"; }
            else if (obj is BPLabel)      { prefix = "LB"; }
            else if (obj is BPLine)       { prefix = "LE"; }
            else if (obj is BPLink) { prefix = "LK"; }
            else if (obj is BPM2M) { prefix = "M2M"; }
            else if (obj is BPRadioBtn) { prefix = "Rdb"; }
          
            else if (obj is BPImg) { prefix = "Img"; }
            else if (obj is BPImgAth) { prefix = "ImgAth"; }
            else if (obj is BPImgSeal) { prefix = "ImgSeal"; }
            else if (obj is BPTextBox) { prefix = "TB"; }
            else if (obj is BPDtl) { prefix = "Dtl"; }

             if (obj is BPWorkCheck)
            {
                prefix = "Wc" + Glo.FK_MapData.Replace("ND", "");
                return prefix ;
            }
            else
            {
                Type type = obj.GetType();
                int maxSuffix = 0;
                foreach (FrameworkElement item in this.workSpace.Children)
                {
                    if ((item is IElement) == false) continue;

                    if (item.GetType() == type)
                    {
                        bool flag = true;
                        int numEle = 0;
                        string num = item.Name;
                        if (!string.IsNullOrEmpty(num) && num.Contains(prefix) && num.Length > prefix.Length)
                        {
                            num = num.Substring(prefix.Length, num.Length - prefix.Length);
                            if (!int.TryParse(num, out numEle))
                                flag = false;
                        }

                        if (flag)
                        {
                            if (numEle >= maxSuffix)
                                maxSuffix = numEle + 1;
                        }
                        else
                            maxSuffix++;
                    }
                }

                //  Element names define rules :

             
                prefix = prefix  + Glo.FK_Flow +maxSuffix.ToString();
                //suffix = DateTime.Now.ToString("yyMMddhhmmss");

                if (obj is BPLine || obj is BPLabel)
                {
                    prefix = prefix.Contains(Glo.FK_MapData) ? prefix : Glo.FK_MapData + "_" + prefix ;
                }

                return prefix;
            }
        }
        #endregion

        #region  Draw grid lines 
        List<string> gridLineNames = new List<string>();
        public void SetGridLines(Canvas workSpace, bool isShow)
        {
            Color col = Color.FromArgb(255, 160, 160, 160);
            SetGridLines(workSpace, col, isShow, false);
        }

        public void SetGridLines(Canvas workSpace, Color bursh, bool isShow, bool isVirtualLine)
        {
            #region  Remove 
            foreach (string id in gridLineNames)
            {
                Line mylin = workSpace.FindName(id) as Line;
                if (mylin == null)
                    continue;
                if (workSpace.Children.Contains(mylin))
                    workSpace.Children.Remove(mylin);
            }
            gridLineNames.Clear();

            #endregion

            if (!isShow)
                return;

            #region  Show 
            SolidColorBrush brush = new SolidColorBrush(bursh);
           
            double thickness = 0.3;
            double top = 0,left = 0;
            double width = workSpace.Width;
            double height = workSpace.Height;
            double stepLength = 40;
            double x, y;
            x = left + stepLength;
            y = top;

            while (x < width + left)
            {
                Line line = new Line();
                line.Name = "GLine" + x + "_" + y;
                line.X1 = x;
                line.Y1 = y;
                line.X2 = x;
                line.Y2 = y + height;

                line.Stroke = brush;
                line.StrokeThickness = thickness;
                if (isVirtualLine)  // Set dashed methods 
                    line.StrokeDashArray = new DoubleCollection() { thickness, thickness }; ;

                line.Stretch = Stretch.Fill;
                workSpace.Children.Add(line);
                x += stepLength;
                gridLineNames.Add(line.Name);
            }

            x = left;
            y = top + stepLength;

            while (y < height + top)
            {
                Line line = new Line();
                line.Name = "GLine" + x + "_" + y;
                line.X1 = x;
                line.Y1 = y;
                line.X2 = x + width;
                line.Y2 = y;

                line.Stroke = brush;
                line.Stretch = Stretch.Fill;
                line.StrokeThickness = thickness;
                if (isVirtualLine)  // Set dashed methods 
                    line.StrokeDashArray = new DoubleCollection() { thickness, thickness }; ;

                workSpace.Children.Add(line);
                y += stepLength;
                gridLineNames.Add(line.Name);
            }
            #endregion
        }
        #endregion

        #region  Treatment and recovery of revocation . Unrealized 
        public void DoRecStep(string doType, UIElement obj, double x1, double y1, double x2, double y2)
        {
            Glo.CurrOpStep = Glo.CurrOpStep + 1;
            FuncStep en = new FuncStep();
            en.DoType = doType;
            en.Ele = obj;
            en.X1 = x1;
            en.X2 = x2;
            en.Y1 = y1;
            en.Y2 = y2;
            Glo.FuncSteps.Add(en);
        }
        public void DoRecStep(string doType, UIElement obj)
        {
            DoRecStep(doType, obj, 0, 0, 0, 0);
        }
        #endregion  Treatment and recovery of revocation .

        protected override void OnKeyDown(KeyEventArgs e)
        {
            e.Handled = true;
            base.OnKeyDown(e);
            this.delPoint();

            switch (e.Key)
            {
                case Key.C:
                    if(Keyboard.Modifiers == ModifierKeys.Control)
                         MouseEventHandlers.IsCopy = true;
                    break;
              
                case Key.Escape:
                    //  Deselect 
                    if (this.selectedElements != null && this.selectedElements.Count > 0)
                    {
                        SetSelectedTool(ToolBox.Mouse);
                    }//  Exit the current window 
                    else if (this.LoadSource)
                    {
                        if (MessageBox.Show(" Whether to exit the current form ", " Shut down ", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            Canvas canvas = new Canvas();
                            canvas = this.workSpace;
                            BP.SL.Glo.SaveUIElementAsPng(canvas, 2, Glo.FK_MapData);

                            if (Closed != null)
                                Closed();
                        }
                    }
                    break;
              
                //  Up 
                case Key.W:
                case Key.Up:
                    foreach (FrameworkElement item in selectedElements)
                    {
                        if (item is BPLine)
                        {
                            BPLine line = item as BPLine;
                            if (line != null)
                            {
                                line.MyLine.Y1 += -1;
                                line.MyLine.Y2 += -1;
                            }
                            continue ;
                        }
                        else if (item is BPLabel)
                        {
                            BPLabel lab = item as BPLabel;
                            if (lab != null)
                            {
                                lab.ToUp();
                            }
                            continue;
                        }
                        if (Keyboard.Modifiers == ModifierKeys.Shift)
                        {
                            if (item is IElement && (item as IElement).IsCanReSize)
                                if (item.Height > 18)
                                item.Height += -1;
                        }
                        else
                        {
                            item.SetValue(Canvas.TopProperty, Canvas.GetTop(item) - 1);
                        }
                    }
                    break;

                //  Down 
                case Key.S: case Key.Down:
                    if (e.Key == Key.S)
                    {
                        //  Save 
                        if (Keyboard.Modifiers == ModifierKeys.Control || Keyboard.Modifiers == ModifierKeys.Windows)
                        {
                            this.Save();
                            break;
                        }
                    }
                    foreach (FrameworkElement item in selectedElements)
                    {
                        if (item is BPLine)
                        {
                            BPLine line = item as BPLine;
                            if (line != null)
                            {
                                line.MyLine.Y1 += 1;
                                line.MyLine.Y2 += 1;
                            }
                            continue;
                        }
                        else if (item is BPLabel)
                        {
                            BPLabel lab = item as BPLabel;
                            if (lab != null)
                            {
                                lab.ToDown();
                            }
                            continue;
                        }

                        if (Keyboard.Modifiers == ModifierKeys.Shift)
                        {
                            if (item is IElement && ( item as IElement).IsCanReSize)
                            {
                                item.Height += 1;
                            }
                        }
                        else
                        {
                            item.SetValue(Canvas.TopProperty, Canvas.GetTop(item) + 1);
                        }
                    }
                    break;

                //  To the right 
                case Key.D:
                case Key.Right:
                    foreach (FrameworkElement item in selectedElements)
                    {
                        if (item is BPLine)
                        {
                            BPLine line = item as BPLine;
                            if (line != null)
                            {
                                line.MyLine.X1 += 1;
                                line.MyLine.X2 += 1;
                                continue;
                            }
                        }
                        else if (item is BPLabel)
                        {

                            BPLabel lab = item as BPLabel;
                            if (lab != null)
                            {
                                lab.ToRight();
                                continue;
                            }
                        }
                        if (Keyboard.Modifiers == ModifierKeys.Shift)
                        {
                            if (item is IElement && ( item as IElement).IsCanReSize)
                            {
                                item.Width += 1;
                            }
                        }
                        else
                        {
                            item.SetValue(Canvas.LeftProperty, Canvas.GetLeft(item) + 1);
                        }
                    }
                    break;

                //  To the left .
                case Key.A:
                case Key.Left:

                    if (e.Key == Key.A && Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        selectAll();
                        break;
                    }
                    foreach (FrameworkElement item in selectedElements)
                    {
                        if (item is BPLine)
                        {
                            BPLine line = item as BPLine;
                            if (line != null)
                            {
                                line.MyLine.X1 += -1;
                                line.MyLine.X2 += -1;

                                continue;
                            }
                        }
                        else if (item is BPLabel)
                        {
                            BPLabel lab = item as BPLabel;
                            if (lab != null)
                            {
                                lab.ToLeft();
                                continue;
                            }
                        }
                        if (Keyboard.Modifiers == ModifierKeys.Shift)
                        {
                            if (item is IElement && (item as IElement).IsCanReSize)
                                if (item.Height > 18)
                                item.Width += -1;
                        }
                        else
                        {
                            item.SetValue(Canvas.LeftProperty, Canvas.GetLeft(item) - 1);
                        }
                    }
                    break;
                case Key.Delete: // Delete .
                    this.DeleteCurrSelectUI();
                    break;
                default:
                    break;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            e.Handled = true;
            base.OnKeyUp(e);
            //check for the specific 'v' key, then check modifiers
            if (e.Key == Key.V)
            {
                if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    //specific Ctrl+V action here
                    this.Paste();
                }
            } 
        }

        #region
        DataSet dsOldest = null;
        void OpenFormXml(string strs)
        {
            this.dsOldest = new DataSet();

            bool toBeContinued = true;
            if (string.IsNullOrEmpty(strs) || strs.Length < 200)
            {
                loadingWindow.DialogResult = false;
                strs = string.IsNullOrEmpty(strs) ? " Data is empty " : strs;
                toBeContinued = false;
            }

            if (toBeContinued)
            {
                try
                {
                    this.dsOldest.FromXml(strs);
                }
                catch (Exception ex)
                {
                    strs = " Read xml Failure " + ex.Message + "\t\n" + strs;
                    toBeContinued = false;
                }
            }

            if (toBeContinued)
            {
                string table = "";
                try
                {
                    #region
                    foreach (DataTable dt in this.dsOldest.Tables)
                    {
                        Glo.TempVal = dt.TableName;
                        table = dt.TableName;

                        switch (dt.TableName)
                        {
                            case "WF_Node":
                                foreach (DataRow dr in dt.Rows)
                                {
                                    BPWorkCheck dtl = new BPWorkCheck(dr["NODEID"]);

                                    dtl.SetValue(Canvas.LeftProperty, double.Parse(dr["FWC_X"]));
                                    dtl.SetValue(Canvas.TopProperty, double.Parse(dr["FWC_Y"]));
                                    dtl.Width = double.Parse(dr["FWC_W"]);
                                    dtl.Height = double.Parse(dr["FWC_H"]);
                                    dtl.FWC_Sta = dr["FWCSTA"] == null ? "0" : dr["FWCSTA"];
                                    dtl.FWC_Type = dr["FWCTYPE"] == null ? "0" : dr["FWCTYPE"];
                                    attachElementEvent(dtl);
                                }
                                break;
                            case "Sys_FrmEle":
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (dr["FK_MAPDATA"] != Glo.FK_MapData)
                                        continue;

                                    BPEle bpele = new BPEle();
                                    bpele.Name = dr["MYPK"].ToString();

                                    if (string.IsNullOrEmpty(dr["ELETYPE"]))
                                        continue;

                                    if (string.IsNullOrEmpty(dr["ELEID"]))
                                        continue;

                                    if (string.IsNullOrEmpty(dr["ELENAME"]))
                                        continue;

                                    bpele.EleType = dr["ELETYPE"].ToString();
                                    bpele.EleName = dr["ELENAME"].ToString();
                                    bpele.EleID = dr["ELEID"].ToString();

                                    bpele.SetValue(Canvas.LeftProperty, double.Parse(dr["X"].ToString()));
                                    bpele.SetValue(Canvas.TopProperty, double.Parse(dr["Y"].ToString()));

                                    bpele.Width = double.Parse(dr["W"].ToString());
                                    bpele.Height = double.Parse(dr["H"].ToString());


                                    attachElementEvent(bpele);
                                }
                                continue;
                            case "Sys_MapData":
                                if (dt.Rows.Count == 0)
                                    continue;
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (dr["NO"] != Glo.FK_MapData)
                                        continue;

                                    Glo.HisMapData = new MapData();
                                    Glo.HisMapData.FrmH = double.Parse(dt.Rows[0]["FRMH"]);
                                    Glo.HisMapData.FrmW = double.Parse(dt.Rows[0]["FRMW"]);
                                    Glo.HisMapData.No = (string)dt.Rows[0]["NO"];
                                    Glo.HisMapData.Name = (string)dt.Rows[0]["NAME"];
                                    Glo.IsDtlFrm = false;

                                    this.workSpace.Width = Glo.HisMapData.FrmW;
                                    this.workSpace.Height = Glo.HisMapData.FrmH;
                                }
                                break;
                            case "Sys_FrmBtn":
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (dr["FK_MAPDATA"] != Glo.FK_MapData)
                                        continue;

                                    BPBtn btn = new BPBtn();
                                    btn.Name = dr["MYPK"];
                                    btn.Content = dr["TEXT"].Replace("&nbsp;", " ");
                                    btn.HisBtnType = (BtnType)int.Parse(dr["BTNTYPE"]);
                                    btn.HisEventType = (EventType)int.Parse(dr["EVENTTYPE"]);

                                    if (dr["EVENTCONTEXT"] != null)
                                        btn.EventContext = dr["EVENTCONTEXT"].Replace("~", "'");

                                    if (dr["MSGERR"] != null)
                                        btn.MsgErr = dr["MSGERR"].Replace("~", "'");

                                    if (dr["MSGOK"] != null)
                                        btn.MsgOK = dr["MSGOK"].Replace("~", "'");

                                    btn.SetValue(Canvas.LeftProperty, double.Parse(dr["X"]));
                                    btn.SetValue(Canvas.TopProperty, double.Parse(dr["Y"]));
                                    attachElementEvent(btn);
                                }
                                continue;
                            case "Sys_FrmLine":
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (dr["FK_MAPDATA"] != Glo.FK_MapData)
                                        continue;

                                    string color = dr["BORDERCOLOR"];
                                    if (string.IsNullOrEmpty(color))
                                        color = "Black";

                                    BPLine myline = new BPLine( color, double.Parse(dr["BORDERWIDTH"]),
                                        double.Parse(dr["X1"]), double.Parse(dr["Y1"]), double.Parse(dr["X2"]),
                                        double.Parse(dr["Y2"]));
                                    myline.Name = dr["MYPK"];
                                    attachElementEvent(myline);
                                }
                                continue;
                            case "Sys_FrmLab":
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (dr["FK_MAPDATA"] != Glo.FK_MapData)
                                        continue;

                                    BPLabel lab = new BPLabel();
                                    lab.Name = dr["MYPK"];
                                    string text = dr["TEXT"].Replace("&nbsp;", " ");
                                    text = text.Replace("@", "\n");
                                    lab.Content = text;
                                    lab.FontSize = double.Parse(dr["FONTSIZE"]);

                                    lab.SetValue(Canvas.LeftProperty, double.Parse(dr["X"]));
                                    lab.SetValue(Canvas.TopProperty, double.Parse(dr["Y"]));

                                    if (dr["ISBOLD"] == "1")
                                        lab.FontWeight = FontWeights.Bold;
                                    else
                                        lab.FontWeight = FontWeights.Normal;


                                    string color = dr["FONTCOLOR"];
                                    lab.Foreground = new SolidColorBrush(Glo.ToColor(color));

                                    attachElementEvent(lab);
                                }
                                continue;
                            case "Sys_FrmLink":
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (dr["FK_MAPDATA"] != Glo.FK_MapData)
                                        continue;

                                    BPLink link = new BPLink();
                                    link.Name = dr["MYPK"];
                                    link.Content = dr["TEXT"];
                                    link.URL = dr["URL"];
                                    link.WinTarget = dr["TARGET"];
                                    link.FontSize = double.Parse(dr["FONTSIZE"]);

                                    link.SetValue(Canvas.LeftProperty, double.Parse(dr["X"]));
                                    link.SetValue(Canvas.TopProperty, double.Parse(dr["Y"]));

                                    string color = dr["FONTCOLOR"];
                                    if (string.IsNullOrEmpty(color))
                                        color = "Black";

                                    link.Foreground = new SolidColorBrush(Glo.ToColor(color));

                                    attachElementEvent(link);
                                }
                                continue;
                            case "Sys_FrmImg":
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (dr["FK_MAPDATA"] != Glo.FK_MapData)
                                        continue;

                                    string ImgAppType = dr["IMGAPPTYPE"];
                                    switch (ImgAppType)
                                    {
                                        case "1":
                                            BPImgSeal imgSeal = new BPImgSeal();
                                            imgSeal.Name = dr["MYPK"];
                                            imgSeal.SetValue(Canvas.LeftProperty, double.Parse(dr["X"].ToString()));
                                            imgSeal.SetValue(Canvas.TopProperty, double.Parse(dr["Y"].ToString()));

                                            imgSeal.Width = double.Parse(dr["W"].ToString());
                                            imgSeal.Height = double.Parse(dr["H"].ToString());
                                            imgSeal.TB_CN_Name = dr["NAME"] == null ? dr["MYPK"] : dr["NAME"].ToString();
                                            imgSeal.TB_En_Name = dr["ENPK"] == null ? dr["MYPK"] : dr["ENPK"].ToString();
                                            imgSeal.Tag0 = dr["TAG0"].ToString();
                                            imgSeal.IsEdit = false;
                                            if (dr["ISEDIT"] != null && !string.IsNullOrEmpty(dr["ISEDIT"].ToString()))
                                            {
                                                imgSeal.IsEdit = dr["ISEDIT"].ToString() == "1" ? true : false;
                                            }
                                            attachElementEvent(imgSeal);
                                            break;
                                        default:
                                            BPImg img = new BPImg();
                                            img.Name = dr["MYPK"];
                                            img.SetValue(Canvas.LeftProperty, double.Parse(dr["X"].ToString()));
                                            img.SetValue(Canvas.TopProperty, double.Parse(dr["Y"].ToString()));
                                            img.TB_CN_Name = dr["NAME"] == null ? dr["MYPK"] : dr["NAME"].ToString();
                                            img.TB_En_Name = dr["ENPK"] == null ? dr["MYPK"] : dr["ENPK"].ToString();
                                            img.Width = double.Parse(dr["W"].ToString());
                                            img.Height = double.Parse(dr["H"].ToString());

                                            // Local Photos 
                                            if (dr["SRCTYPE"] != null && dr["SRCTYPE"].ToString() == "0")
                                            {
                                                img.SrcType = 0;
                                                // Determine whether to modify the image path 
                                                if (dr["IMGPATH"] != null && dr["IMGPATH"].Contains("DataUser"))
                                                {
                                                    ImageBrush ib = new ImageBrush();
                                                    BitmapImage png = new BitmapImage(new Uri(Glo.BPMHost + dr["IMGPATH"], UriKind.RelativeOrAbsolute));
                                                    ib.ImageSource = png;
                                                    img.Background = ib;
                                                    img.HisPng = png;
                                                }
                                            }
                                            else if (dr["SRCTYPE"] != null && dr["SRCTYPE"].ToString() == "1")// Specify the path 
                                            {
                                                img.SrcType = 1;
                                                // Judge image path is not empty , And does not contain ccflow Expression 
                                                if (dr["IMGURL"] != null && !dr["IMGURL"].ToString().Contains("@"))
                                                {
                                                    ImageBrush ib = new ImageBrush();
                                                    BitmapImage png = new BitmapImage(new Uri(dr["IMGURL"], UriKind.RelativeOrAbsolute));
                                                    ib.ImageSource = png;
                                                    img.Background = ib;
                                                    img.HisPng = png;
                                                }
                                            }

                                            img.LinkTarget = dr["LINKTARGET"];
                                            img.LinkURL = dr["LINKURL"];
                                            img.ImgURL = dr["IMGURL"];
                                            img.ImgPath = dr["IMGPATH"];
                                            attachElementEvent(img);
                                            break;
                                    }
                                }
                                continue;
                            case "Sys_FrmImgAth":
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (dr["FK_MAPDATA"] != Glo.FK_MapData)
                                        continue;

                                    BPImgAth ath = new BPImgAth();
                                    ath.Name = dr["MYPK"];
                                    ath.CtrlID = dr["CTRLID"]; // Accessory ID.

                                    ath.SetValue(Canvas.LeftProperty, double.Parse(dr["X"].ToString()));
                                    ath.SetValue(Canvas.TopProperty, double.Parse(dr["Y"].ToString()));
                                    ath.IsEdit = dr["ISEDIT"].ToString() == "1" ? true : false;
                                    ath.Height = double.Parse(dr["H"].ToString());
                                    ath.Width = double.Parse(dr["W"].ToString());
                                    attachElementEvent(ath);
                                }
                                continue;
                            case "Sys_FrmRB":
                                DataTable dtRB = this.dsOldest.Tables["SYS_FRMRB"];
                                foreach (DataRow dr in dtRB.Rows)
                                {
                                    if (dr["FK_MAPDATA"] != Glo.FK_MapData)
                                        continue;

                                    BPRadioBtn btn = new BPRadioBtn();
                                    btn.Name = dr["MYPK"];
                                    btn.GroupName = dr["KEYOFEN"];
                                    btn.Content = dr["LAB"];
                                    btn.UIBindKey = dr["ENUMKEY"];
                                    btn.Tag = dr["INTKEY"];
                                    btn.SetValue(Canvas.LeftProperty, double.Parse(dr["X"].ToString()));
                                    btn.SetValue(Canvas.TopProperty, double.Parse(dr["Y"].ToString()));


                                    attachElementEvent(btn);
                                }
                                continue;
                            case "Sys_MapAttr":
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (dr["UIVISIBLE"] == "0")
                                        continue;

                                    if (dr["FK_MAPDATA"] != Glo.FK_MapData)
                                        continue;

                                    string myPk = dr["MYPK"];
                                    string FK_MapData = dr["FK_MAPDATA"];
                                    string keyOfEn = dr["KEYOFEN"];
                                    string name = dr["NAME"];
                                    string defVal = dr["DEFVAL"];
                                    string UIContralType = dr["UICONTRALTYPE"];
                                    string MyDataType = dr["MYDATATYPE"];
                                    string lgType = dr["LGTYPE"];
                                    double X = double.Parse(dr["X"]);
                                    double Y = double.Parse(dr["Y"]);
                                    if (X == 0)
                                        X = 100;
                                    if (Y == 0)
                                        Y = 100;

                                    string uIBindKey = dr["UIBINDKEY"];
                                    switch (UIContralType)
                                    {
                                        case CtrlType.TextBox:
                                            TBType tp = TBType.String;
                                            switch (MyDataType)
                                            {
                                                case DataType.AppInt:
                                                    tp = TBType.Int;
                                                    break;
                                                case DataType.AppFloat:
                                                case DataType.AppDouble:
                                                    tp = TBType.Float;
                                                    break;
                                                case DataType.AppMoney:
                                                    tp = TBType.Money;
                                                    break;
                                                case DataType.AppString:
                                                    tp = TBType.String;
                                                    break;
                                                case DataType.AppDateTime:
                                                    tp = TBType.DateTime;
                                                    break;
                                                case DataType.AppDate:
                                                    tp = TBType.Date;
                                                    break;
                                                default:
                                                    break;
                                            }

                                            BPTextBox tb = new BPTextBox(tp)
                                            {
                                                NameOfReal = keyOfEn,
                                                Name = keyOfEn,
                                                X = X,
                                                Y = Y,
                                                Width = double.Parse(dr["UIWIDTH"]),
                                                Height = double.Parse(dr["UIHEIGHT"])
                                            };

                                            tb.SetValue(Canvas.LeftProperty, X);
                                            tb.SetValue(Canvas.TopProperty, Y);

                                            if (this.workSpace.FindName(tb.Name) != null)
                                            {
                                                MessageBox.Show(" Already exists " + tb.Name);
                                                continue;
                                            }
                                            attachElementEvent(tb);
                                            break;
                                        case CtrlType.DDL:
                                            BPDDL ddl = new BPDDL()
                                            {
                                                Name = keyOfEn,
                                                UIBindKey = uIBindKey,
                                                _HisDataType = lgType,
                                                Width = double.Parse(dr["UIWIDTH"]),

                                            };

                                            if (lgType == LGType.Enum)
                                            {
                                                ddl.BindEnum(uIBindKey);
                                            }
                                            else
                                            {
                                                ddl.BindEns(uIBindKey);
                                            }

                                            ddl.SetValue(Canvas.LeftProperty, X);
                                            ddl.SetValue(Canvas.TopProperty, Y);
                                            attachElementEvent(ddl);
                                            break;
                                        case CtrlType.CheckBox:
                                            BPCheckBox cb = new BPCheckBox();
                                            cb.Name = keyOfEn;

                                            cb.Content = new Label()
                                            {
                                                Name = "CBLab" + cb.Name,
                                                Content = name,
                                                Tag = keyOfEn
                                            };

                                            if (defVal == "1")
                                                cb.IsChecked = true;
                                            else
                                                cb.IsChecked = false;

                                            cb.SetValue(Canvas.LeftProperty, X);
                                            cb.SetValue(Canvas.TopProperty, Y);

                                            attachElementEvent(cb);
                                            break;
                                        case CtrlType.RB:
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                continue;
                            case "Sys_MapM2M":
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (dr["FK_MAPDATA"] != Glo.FK_MapData)
                                        continue;

                                    BPM2M m2m = new BPM2M(dr["NOOFOBJ"]);
                                    m2m.SetValue(Canvas.LeftProperty, double.Parse(dr["X"]));
                                    m2m.SetValue(Canvas.TopProperty, double.Parse(dr["Y"]));

                                    m2m.Width = double.Parse(dr["W"]);
                                    m2m.Height = double.Parse(dr["H"]);

                                    attachElementEvent(m2m);
                                }
                                continue;
                            case "Sys_MapDtl":
                                foreach (DataRow dr in dt.Rows)
                                {
                                    BPDtl dtl = new BPDtl(dr["NO"]);
                                    dtl.SetValue(Canvas.LeftProperty, double.Parse(dr["X"]));
                                    dtl.SetValue(Canvas.TopProperty, double.Parse(dr["Y"]));
                                    dtl.Width = double.Parse(dr["W"]);
                                    dtl.Height = double.Parse(dr["H"]);

                                    attachElementEvent(dtl);
                                }
                                continue;
                            case "Sys_FrmAttachment":
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (dr["FK_MAPDATA"] != Glo.FK_MapData)
                                        continue;

                                    string uploadTypeInt = dr["UPLOADTYPE"];
                                    if (string.IsNullOrEmpty(uploadTypeInt))
                                        uploadTypeInt = "0";

                                    AttachmentUploadType uploadType = (AttachmentUploadType)int.Parse(uploadTypeInt);
                                    if (uploadType == AttachmentUploadType.Single)
                                    {
                                        BPAttachment ath = new BPAttachment(dr["NOOFOBJ"],
                                            dr["NAME"], dr["EXTS"],
                                            double.Parse(dr["W"]), dr["SAVETO"].ToString());

                                        ath.SetValue(Canvas.LeftProperty, double.Parse(dr["X"]));
                                        ath.SetValue(Canvas.TopProperty, double.Parse(dr["Y"]));

                                        ath.Label = dr["NAME"] as string;
                                        ath.Exts = dr["EXTS"] as string;
                                        ath.SaveTo = dr["SAVETO"] as string;

                                        ath.X = double.Parse(dr["X"]);
                                        ath.Y = double.Parse(dr["Y"]);

                                        if (dr["ISUPLOAD"] == "1")
                                            ath.IsUpload = true;
                                        else
                                            ath.IsUpload = false;

                                        if (dr["ISDELETE"] == "1")
                                            ath.IsDelete = true;
                                        else
                                            ath.IsDelete = false;

                                        if (dr["ISDOWNLOAD"] == "1")
                                            ath.IsDownload = true;
                                        else
                                            ath.IsDownload = false;

                                        attachElementEvent(ath);
                                    }
                                    else if (uploadType == AttachmentUploadType.Multi)
                                    {
                                        BPAttachmentM athM = new BPAttachmentM();
                                        athM.SetValue(Canvas.LeftProperty, double.Parse(dr["X"]));
                                        athM.SetValue(Canvas.TopProperty, double.Parse(dr["Y"]));
                                        athM.Name = dr["NOOFOBJ"];
                                        athM.Width = double.Parse(dr["W"]);
                                        athM.Height = double.Parse(dr["H"]);
                                        athM.X = double.Parse(dr["X"]);
                                        athM.Y = double.Parse(dr["Y"]);
                                        athM.SaveTo = dr["SAVETO"];
                                        athM.Label = dr["NAME"];

                                        attachElementEvent(athM);

                                    }
                                    continue;

                                }
                                continue;
                            default:
                                break;
                        }
                    }
                    #endregion

                }
                catch (Exception ex)
                {
                    toBeContinued = false;
                    BP.SL.LoggerHelper.Write(ex);
                    strs = "err:" + table + ", Error loading form .\t\n May be due to system upgrades , Please try to run with the upgrade mode to process designer ,http://yourserver:portNum/Default.htm";
                }
            }


            this.SetGridLines(this.workSpace, true); // Re-draw the line 

            if (!toBeContinued)
            {
                MessageBox.Show(strs);
                this.Cursor = Cursors.Arrow;
            }
            else
            {
                this.Cursor = Cursors.Arrow;
                if (null != CCBPFormLoaded)
                    CCBPFormLoaded();
            }
        }
//        public void SaveXml()
//        {
//            DataTable
//            dtLine = dsLatest.Tables[ElementTable.Sys_FrmLine],
//            dtBtn = dsLatest.Tables[ElementTable.Sys_FrmBtn],
//            dtLabel = dsLatest.Tables[ElementTable.Sys_FrmLab],
//            dtLikn = dsLatest.Tables[ElementTable.Sys_FrmLink],
//            dtImg = dsLatest.Tables[ElementTable.Sys_FrmImg],
//            dtEle = dsLatest.Tables[ElementTable.Sys_FrmEle],
//            dtImgAth = dsLatest.Tables[ElementTable.Sys_FrmImgAth],
//            dtMapAttr = dsLatest.Tables[ElementTable.Sys_MapAttr],
//            dtRDB = dsLatest.Tables[ElementTable.Sys_FrmRB],
//            dtlDT = dsLatest.Tables[ElementTable.Sys_MapDtl],
//            dtWorkCheck = dsLatest.Tables[ElementTable.WF_Node],
//            dtM2M = dsLatest.Tables[ElementTable.Sys_MapM2M],
//            dtAth = dsLatest.Tables[ElementTable.Sys_FrmAttachment];

//            #region
//            foreach (UIElement ctl in this.workSpace.Children)
//            {
//                if (!(ctl is IElement)) continue;
//                if ((ctl as IElement).ViewDeleted) continue;

//                double dX = Canvas.GetLeft(ctl);
//                double dY = Canvas.GetTop(ctl);


//                if (ctl is BPLine)
//                {
//                    #region
//                    BPLine line = ctl as BPLine;
//                    if (line != null)
//                    {
//                        DataRow drline = dtLine.NewRow();
//                        drline["MYPK"] = line.Name;
//                        drline["FK_MAPDATA"] = Glo.FK_MapData;

//                        drline["X"] = dX.ToString("0.00");
//                        drline["Y"] = dY.ToString("0.00");

//                        drline["X1"] = line.MyLine.X1.ToString("0.00");
//                        drline["X2"] = line.MyLine.X2.ToString("0.00");
//                        drline["Y1"] = line.MyLine.Y1.ToString("0.00");
//                        drline["Y2"] = line.MyLine.Y2.ToString("0.00");
//                        drline["BORDERWIDTH"] = line.MyLine.StrokeThickness.ToString("0.00");

//                        SolidColorBrush d = (SolidColorBrush)line.MyLine.Stroke;
//                        drline["BORDERCOLOR"] = Glo.PreaseColorToName(d.Color.ToString());
//                        dtLine.Rows.Add(drline);
//                    }
//                    #endregion
//                }
//                else if (ctl is TextBoxExt)
//                {
//                    if (ctl is BPEle)
//                    {
//                        #region
//                        BPEle ele = ctl as BPEle;
//                        if (ele != null)
//                        {
//                            DataRow drImg = dtEle.NewRow();
//                            drImg["MYPK"] = ele.Name;
//                            drImg["FK_MAPDATA"] = Glo.FK_MapData;

//                            //drImg["ELETYPE"] = ele.EleType;
//                            //drImg["ELENAME"] = ele.EleName;
//                            //drImg["ELEID"] = ele.EleID;

//                            //eleDT.Columns.Add(new DataColumn("EleType", typeof(string)));
//                            //eleDT.Columns.Add(new DataColumn("EleID", typeof(string)));
//                            //eleDT.Columns.Add(new DataColumn("EleName", typeof(string)));

//                            MatrixTransform transform = ctl.TransformToVisual(this.workSpace) as MatrixTransform;
//                            double x = transform.Matrix.OffsetX;
//                            double y = transform.Matrix.OffsetY;

//                            if (x <= 0)
//                                x = 0;
//                            if (y == 0)
//                                y = 0;
//                            if (y.ToString() == "NaN")
//                            {
//                                x = Canvas.GetLeft(ctl);
//                                y = Canvas.GetTop(ctl);
//                            }

//                            drImg["X"] = x.ToString("0.00");
//                            drImg["Y"] = y.ToString("0.00");

//                            drImg["W"] = ele.Width.ToString("0.00");
//                            drImg["H"] = ele.Height.ToString("0.00");

//                            dtEle.Rows.Add(drImg);

//                        }
//                        continue;
//                        #endregion
//                    }
//                    else if (ctl is BPTextBox)
//                    {
//                        #region
//                        BPTextBox tb = ctl as BPTextBox;
//                        if (tb != null)
//                        {
//                            DataRow mapAttrDR = dtMapAttr.NewRow();
//                            mapAttrDR["MYPK"] = Glo.FK_MapData + "_" + tb.Name.Trim();
//                            mapAttrDR["FK_MAPDATA"] = Glo.FK_MapData;
//                            mapAttrDR["KEYOFEN"] = tb.Name.Trim();

//                            mapAttrDR["UICONTRALTYPE"] = CtrlType.TextBox;
//                            mapAttrDR["MYDATATYPE"] = tb.HisDataType;

//                            mapAttrDR["UIWIDTH"] = tb.Width.ToString("0.00");
//                            mapAttrDR["UIHEIGHT"] = tb.Height.ToString("0.00");
//                            mapAttrDR["LGTYPE"] = LGType.Normal;


//                            MatrixTransform transform = ctl.TransformToVisual(this.workSpace) as MatrixTransform;
//                            double x = transform.Matrix.OffsetX;
//                            double y = transform.Matrix.OffsetY;

//                            if (y.ToString() == "NaN")
//                            {
//                                x = Canvas.GetLeft(ctl);
//                                y = Canvas.GetTop(ctl);
//                            }

//                            mapAttrDR["X"] = x.ToString("0.00");
//                            mapAttrDR["Y"] = y.ToString("0.00");
//                            // mapAttrDR["UIVISIBLE"] = "1";
//                            dtMapAttr.Rows.Add(mapAttrDR);

//                        }
//                        continue;
//                        #endregion
//                    }
//                    else if (ctl is BPImg)
//                    {
//                        #region
//                        BPImg img = ctl as BPImg;
//                        if (img != null)
//                        {
//                            DataRow drImg = dtImg.NewRow();
//                            drImg["MYPK"] = img.Name;
//                            drImg["FK_MAPDATA"] = Glo.FK_MapData;

//                            MatrixTransform transform = ctl.TransformToVisual(this.workSpace) as MatrixTransform;
//                            double x = transform.Matrix.OffsetX;
//                            double y = transform.Matrix.OffsetY;

//                            if (x <= 0)
//                                x = 0;
//                            if (y == 0)
//                                y = 0;
//                            if (y.ToString() == "NaN")
//                            {
//                                x = Canvas.GetLeft(ctl);
//                                y = Canvas.GetTop(ctl);
//                            }

//                            drImg["X"] = x.ToString("0.00"); // Canvas.GetLeft(ctl).ToString("0.00");
//                            drImg["Y"] = y.ToString("0.00"); // Canvas.GetTop(ctl).ToString("0.00");

//                            drImg["W"] = img.Width.ToString("0.00");
//                            drImg["H"] = img.Height.ToString("0.00");

//                            BitmapImage png = img.HisPng;

//                            drImg["LINKURL"] = img.LinkURL;
//                            drImg["LINKTARGET"] = img.LinkTarget;
//                            drImg["SRCTYPE"] = img.SrcType.ToString();

//                            drImg["IMGPATH"] = png.UriSource.ToString().Contains("DataUser") ? png.UriSource.ToString().Replace(Glo.BPMHost, "") : png.UriSource.ToString();
//                            drImg["IMGURL"] = img.ImgURL;

//                            drImg["IMGAPPTYPE"] = "0";
//                            drImg["ISEDIT"] = "1";
//                            drImg["NAME"] = img.TB_CN_Name;
//                            drImg["ENPK"] = img.TB_En_Name;
//                            dtImg.Rows.Add(drImg);

//                        }
//                        #endregion
//                    }
//                    else if (ctl is BPImgAth)
//                    {
//                        #region
//                        BPImgAth imgAth = ctl as BPImgAth;
//                        if (imgAth != null)
//                        {
//                            DataRow mapAth = dtImgAth.NewRow();
//                            mapAth["MYPK"] = imgAth.Name;
//                            mapAth["CTRLID"] = imgAth.CtrlID; // Accessory ID.
//                            mapAth["FK_MAPDATA"] = Glo.FK_MapData;
//                            mapAth["ISEDIT"] = imgAth.IsEdit ? "1" : "0";
//                            MatrixTransform transform = imgAth.TransformToVisual(this.workSpace) as MatrixTransform;

//                            mapAth["X"] = transform.Matrix.OffsetX.ToString("0.00");
//                            mapAth["Y"] = transform.Matrix.OffsetY.ToString("0.00");

//                            mapAth["W"] = imgAth.Width.ToString("0.00");
//                            mapAth["H"] = imgAth.Height.ToString("0.00");
//                            dtImgAth.Rows.Add(mapAth);

//                        }
//                        #endregion
//                    }
//                    else if (ctl is BPImgSeal)
//                    {
//                        #region
//                        BPImgSeal imgSeal = ctl as BPImgSeal;
//                        if (imgSeal != null)
//                        {
//                            DataRow drImgSeal = dtImg.NewRow();
//                            drImgSeal["MYPK"] = imgSeal.Name;
//                            drImgSeal["FK_MAPDATA"] = Glo.FK_MapData;
//                            drImgSeal["IMGAPPTYPE"] = "1";
//                            MatrixTransform transform = ctl.TransformToVisual(this.workSpace)
//                                as MatrixTransform;
//                            double x = transform.Matrix.OffsetX;
//                            double y = transform.Matrix.OffsetY;

//                            if (x <= 0)
//                                x = 0;
//                            if (y == 0)
//                                y = 0;
//                            if (y.ToString() == "NaN")
//                            {
//                                x = Canvas.GetLeft(ctl);
//                                y = Canvas.GetTop(ctl);
//                            }

//                            drImgSeal["X"] = x.ToString("0.00");
//                            drImgSeal["Y"] = y.ToString("0.00");

//                            drImgSeal["W"] = imgSeal.Width.ToString("0.00");
//                            drImgSeal["H"] = imgSeal.Height.ToString("0.00");

//                            BitmapImage png = imgSeal.HisPng;
//                            drImgSeal["IMGURL"] = png.UriSource.ToString();
//                            drImgSeal["TAG0"] = imgSeal.Tag0;
//                            drImgSeal["NAME"] = imgSeal.TB_CN_Name;
//                            drImgSeal["ENPK"] = imgSeal.TB_En_Name;
//                            drImgSeal["ISEDIT"] = imgSeal.IsEdit ? "1" : "0";
//                            dtImg.Rows.Add(drImgSeal);

//                        }
//                        #endregion
//                    }
//                }
//                else if (ctl is LabelExt)
//                {
//                    if (ctl is BPLabel)
//                    {
//                        #region
//                        BPLabel lab = ctl as BPLabel;
//                        if (lab != null)
//                        {
//                            DataRow drLab = dtLabel.NewRow();
//                            drLab["MYPK"] = lab.Name;
//                            drLab["TEXT"] = lab.Content.ToString().Replace(" ", "&nbsp;").Replace("\n", "@");
//                            drLab["FK_MAPDATA"] = Glo.FK_MapData;

//                            drLab["X"] = dX.ToString("0.00");
//                            drLab["Y"] = dY.ToString("0.00");

//                            // drLab["FONTCOLOR"] = lab.GetValue( lapp ).ToString();
//#warning  How to get the font color  ? .

//                            SolidColorBrush d = (SolidColorBrush)lab.Foreground;
//                            drLab["FONTCOLOR"] = d.Color.ToString();
//                            // Glo.PreaseColorToName(d.Color.ToString());
//                            drLab["FONTNAME"] = lab.FontFamily.ToString();
//                            drLab["FONTSTYLE"] = lab.FontStyle.ToString();
//                            drLab["FONTSIZE"] = lab.FontSize.ToString();

//                            if (lab.FontWeight == FontWeights.Normal)
//                                drLab["ISBOLD"] = "0";
//                            else
//                                drLab["ISBOLD"] = "1";

//                            if (lab.FontStyle.ToString() == "Italic")
//                                drLab["ISITALIC"] = "1";
//                            else
//                                drLab["ISITALIC"] = "0";

//                            dtLabel.Rows.Add(drLab);

//                        }
//                        #endregion
//                    }
//                    else if (ctl is BPLink)
//                    {
//                        #region
//                        BPLink link = ctl as BPLink;
//                        if (link != null)
//                        {
//                            DataRow drLink = dtLikn.NewRow();
//                            drLink["MYPK"] = link.Name;

//                            drLink["TEXT"] = link.Content.ToString();
//                            drLink["FK_MAPDATA"] = Glo.FK_MapData;

//                            drLink["X"] = dX.ToString("0.00");
//                            drLink["Y"] = dY.ToString("0.00");

//                            SolidColorBrush d = (SolidColorBrush)link.Foreground;
//                            drLink["FONTCOLOR"] = Glo.PreaseColorToName(d.Color.ToString());
//                            drLink["FONTNAME"] = link.FontFamily.ToString();
//                            drLink["FONTSTYLE"] = link.FontStyle.ToString();
//                            drLink["FONTSIZE"] = link.FontSize.ToString();
//                            drLink["URL"] = link.URL;
//                            drLink["TARGET"] = link.WinTarget;

//                            if (link.FontWeight == FontWeights.Normal)
//                                drLink["ISBOLD"] = "0";
//                            else
//                                drLink["ISBOLD"] = "1";

//                            if (link.FontStyle.ToString() == "Italic")
//                                drLink["ISITALIC"] = "1";
//                            else
//                                drLink["ISITALIC"] = "0";

//                            dtLikn.Rows.Add(drLink);

//                        }
//                        #endregion
//                    }
//                }
//                else if (ctl is UCExt)
//                {
//                    if (ctl is BPAttachment)
//                    {
//                        #region
//                        BPAttachment athCtl = ctl as BPAttachment;
//                        if (athCtl != null)
//                        {

//                            DataRow mapAth = dtAth.NewRow();
//                            mapAth["MYPK"] = Glo.FK_MapData + "_" + athCtl.Name;
//                            mapAth["FK_MAPDATA"] = Glo.FK_MapData;
//                            mapAth["NOOFOBJ"] = athCtl.Name;
//                            mapAth["UPLOADTYPE"] = "0";

//                            MatrixTransform transform = athCtl.TransformToVisual(this.workSpace) as MatrixTransform;
//                            mapAth["X"] = transform.Matrix.OffsetX.ToString("0.00");
//                            mapAth["Y"] = transform.Matrix.OffsetY.ToString("0.00");
//                            mapAth["W"] = athCtl.HisTB.Width.ToString("0.00");
//                            dtAth.Rows.Add(mapAth);
//                        }
//                        #endregion
//                    }
//                    else if (ctl is BPAttachmentM)
//                    {
//                        #region
//                        BPAttachmentM athM = ctl as BPAttachmentM;
//                        if (athM != null)
//                        {
//                            DataRow mapAth = dtAth.NewRow();
//                            mapAth["MYPK"] = Glo.FK_MapData + "_" + athM.Name;
//                            mapAth["FK_MAPDATA"] = Glo.FK_MapData;
//                            mapAth["NOOFOBJ"] = athM.Name;
//                            mapAth["UPLOADTYPE"] = "1";

//                            MatrixTransform transform = athM.TransformToVisual(this.workSpace) as MatrixTransform;
//                            mapAth["X"] = transform.Matrix.OffsetX.ToString("0.00");
//                            mapAth["Y"] = transform.Matrix.OffsetY.ToString("0.00");

//                            mapAth["W"] = athM.Width.ToString("0.00");
//                            mapAth["H"] = athM.Height.ToString("0.00");
//                            dtAth.Rows.Add(mapAth);
//                        }
//                        #endregion
//                    }
//                    else if (ctl is BPDtl)
//                    {
//                        #region
//                        BPDtl dtlCtl = ctl as BPDtl;
//                        if (dtlCtl != null)
//                        {
//                            DataRow mapDtl = dtlDT.NewRow();
//                            mapDtl["NO"] = dtlCtl.Name;
//                            mapDtl["FK_MAPDATA"] = Glo.FK_MapData;

//                            MatrixTransform transform = dtlCtl.TransformToVisual(this.workSpace) as MatrixTransform;

//                            mapDtl["X"] = transform.Matrix.OffsetX.ToString("0.00");
//                            mapDtl["Y"] = transform.Matrix.OffsetY.ToString("0.00");
//                            mapDtl["W"] = dtlCtl.Width.ToString("0.00");
//                            mapDtl["H"] = dtlCtl.Height.ToString("0.00");
//                            dtlDT.Rows.Add(mapDtl);

//                        }
//                        #endregion
//                    }
//                    else if (ctl is BPWorkCheck)
//                    {
//                        #region   Audit Components 
//                        BPWorkCheck wkCheck = ctl as BPWorkCheck;
//                        if (wkCheck != null)
//                        {
//                            DataRow workCheckDt = dtWorkCheck.NewRow();
//                            workCheckDt["NODEID"] = Glo.FK_MapData.Replace("ND", "");

//                            MatrixTransform transform = wkCheck.TransformToVisual(this.workSpace) as MatrixTransform;

//                            workCheckDt["FWCSTA"] = wkCheck.FWC_Sta;
//                            workCheckDt["FWCTYPE"] = wkCheck.FWC_Type;

//                            workCheckDt["FWC_X"] = transform.Matrix.OffsetX.ToString("0.00");
//                            workCheckDt["FWC_Y"] = transform.Matrix.OffsetY.ToString("0.00");

//                            workCheckDt["FWC_W"] = wkCheck.Width.ToString("0.00");
//                            workCheckDt["FWC_H"] = wkCheck.Height.ToString("0.00");
//                            dtWorkCheck.Rows.Add(workCheckDt);

//                        }
//                        #endregion
//                    }
//                    else if (ctl is BPM2M)
//                    {
//                        #region
//                        BPM2M m2mCtl = ctl as BPM2M;
//                        if (m2mCtl != null)
//                        {
//                            DataRow rowM2M = dtM2M.NewRow();
//                            rowM2M["NOOFOBJ"] = m2mCtl.Name;
//                            rowM2M["FK_MAPDATA"] = Glo.FK_MapData;
//                            rowM2M["MYPK"] = Glo.FK_MapData + "_" + m2mCtl.Name;

//                            MatrixTransform transform = m2mCtl.TransformToVisual(this.workSpace) as MatrixTransform;

//                            rowM2M["X"] = transform.Matrix.OffsetX.ToString("0.00");
//                            rowM2M["Y"] = transform.Matrix.OffsetY.ToString("0.00");

//                            rowM2M["W"] = m2mCtl.Width.ToString("0.00");
//                            rowM2M["H"] = m2mCtl.Height.ToString("0.00");

//                            dtM2M.Rows.Add(rowM2M);
//                        }
//                        #endregion
//                    }
//                }
//                else if (ctl is BPDatePicker)
//                {
//                    #region
//                    BPDatePicker dp = ctl as BPDatePicker;
//                    if (dp != null)
//                    {
//                        DataRow mapAttrDR = dtMapAttr.NewRow();
//                        mapAttrDR["MYPK"] = Glo.FK_MapData + "_" + dp.Name;
//                        mapAttrDR["FK_MAPDATA"] = Glo.FK_MapData;
//                        mapAttrDR["KEYOFEN"] = dp.Name;

//                        mapAttrDR["UICONTRALTYPE"] = CtrlType.TextBox;
//                        mapAttrDR["MYDATATYPE"] = dp.HisDateType;
//                        mapAttrDR["LGTYPE"] = LGType.Normal;

//                        mapAttrDR["X"] = dX.ToString("0.00");
//                        mapAttrDR["Y"] = dY.ToString("0.00");

//                        // mapAttrDR["UIVISIBLE"] = "1";
//                        mapAttrDR["UIWIDTH"] = "50";
//                        mapAttrDR["UIHEIGHT"] = "23";

//                        dtMapAttr.Rows.Add(mapAttrDR);

//                    }
//                    continue;
//                    #endregion
//                }
//                else if (ctl is BPBtn)
//                {
//                    #region
//                    BPBtn btn = ctl as BPBtn;
//                    if (btn != null)
//                    {
//                        DataRow drBtn = dtBtn.NewRow();
//                        drBtn["MYPK"] = btn.Name;
//                        drBtn["TEXT"] = btn.Content.ToString().Replace(" ", "&nbsp;").Replace("\n", "@");
//                        drBtn["FK_MAPDATA"] = Glo.FK_MapData;

//                        drBtn["X"] = dX.ToString("0.00");
//                        drBtn["Y"] = dY.ToString("0.00");

//                        dtBtn.Rows.Add(drBtn);
//                    }
//                    #endregion
//                }

//                else if (ctl is BPDDL)
//                {
//                    #region
//                    BPDDL ddl = ctl as BPDDL;
//                    if (ddl != null)
//                    {

//                        DataRow mapAttrDR = dtMapAttr.NewRow();
//                        mapAttrDR["MYPK"] = Glo.FK_MapData + "_" + ddl.Name;
//                        mapAttrDR["FK_MAPDATA"] = Glo.FK_MapData;
//                        mapAttrDR["KEYOFEN"] = ddl.Name;

//                        mapAttrDR["UICONTRALTYPE"] = CtrlType.DDL;
//                        mapAttrDR["MYDATATYPE"] = ddl.HisDataType;
//                        mapAttrDR["LGTYPE"] = ddl._HisDataType;

//                        mapAttrDR["UIWIDTH"] = ddl.Width.ToString("0.00");
//                        mapAttrDR["UIHEIGHT"] = "23";

//                        mapAttrDR["X"] = dX.ToString("0.00");
//                        mapAttrDR["Y"] = dY.ToString("0.00");

//                        mapAttrDR["UIBINDKEY"] = ddl.UIBindKey;
//                        mapAttrDR["UIREFKEY"] = "No";
//                        mapAttrDR["UIREFKEYTEXT"] = "Name";
//                        //     mapAttrDR["UIVISIBLE"] = "1";
//                        dtMapAttr.Rows.Add(mapAttrDR);

//                    }
//                    #endregion
//                }
//                else if (ctl is BPCheckBox)
//                {
//                    #region
//                    BPCheckBox cb = ctl as BPCheckBox;
//                    if (cb != null)
//                    {

//                        DataRow mapAttrDR = dtMapAttr.NewRow();
//                        mapAttrDR["MYPK"] = Glo.FK_MapData + "_" + cb.Name;
//                        mapAttrDR["FK_MAPDATA"] = Glo.FK_MapData;
//                        mapAttrDR["KEYOFEN"] = cb.Name;
//                        mapAttrDR["UICONTRALTYPE"] = CtrlType.CheckBox;
//                        mapAttrDR["MYDATATYPE"] = DataType.AppBoolean;
//                        mapAttrDR["LGTYPE"] = LGType.Normal;
//                        mapAttrDR["X"] = dX.ToString("0.00");
//                        mapAttrDR["Y"] = dY.ToString("0.00");
//                        mapAttrDR["UIWIDTH"] = "100";
//                        mapAttrDR["UIHEIGHT"] = "23";


//                        dtMapAttr.Rows.Add(mapAttrDR);

//                    }
//                    #endregion
//                }
//                else if (ctl is BPRadioBtn)
//                {
//                    #region
//                    BPRadioBtn rb = ctl as BPRadioBtn;
//                    if (rb != null)
//                    {
//                        DataRow mapAttrRB = dtRDB.NewRow();
//                        mapAttrRB["MYPK"] = rb.Name;
//                        mapAttrRB["FK_MAPDATA"] = Glo.FK_MapData;
//                        mapAttrRB["KEYOFEN"] = rb.GroupName;
//                        mapAttrRB["INTKEY"] = rb.Tag as string;
//                        mapAttrRB["LAB"] = rb.Content as string;
//                        mapAttrRB["ENUMKEY"] = rb.UIBindKey;
//                        mapAttrRB["X"] = dX.ToString("0.00");
//                        mapAttrRB["Y"] = dY.ToString("0.00");
//                        dtRDB.Rows.Add(mapAttrRB);

//                    }
//                    #endregion
//                }
//            }
//            #endregion

//            #region  Deal with  RB  Enum value 
//            string keys = "";
//            foreach (DataRow dr in dtRDB.Rows)
//            {
//                string keyOfEn = dr["KEYOFEN"];
//                if (keys.Contains("@" + keyOfEn + "@"))
//                {
//                    continue;
//                }
//                else
//                {
//                    keys += "@" + keyOfEn + "@";
//                }

//                string enumKey = dr["ENUMKEY"];
//                DataRow mapAttrDR = dtMapAttr.NewRow();
//                mapAttrDR["MYPK"] = Glo.FK_MapData + "_" + keyOfEn;
//                mapAttrDR["FK_MAPDATA"] = Glo.FK_MapData;
//                mapAttrDR["KEYOFEN"] = keyOfEn;

//                mapAttrDR["UICONTRALTYPE"] = CtrlType.RB;
//                mapAttrDR["MYDATATYPE"] = DataType.AppInt;
//                mapAttrDR["LGTYPE"] = LGType.Enum;
//                mapAttrDR["INTKEY"] = dr["INTKEY"];

//                mapAttrDR["X"] = "0";
//                mapAttrDR["Y"] = "0";

//                mapAttrDR["UIBINDKEY"] = enumKey;
//                mapAttrDR["UIREFKEY"] = "No";
//                mapAttrDR["UIREFKEYTEXT"] = "Name";
//                //      mapAttrDR["UIVISIBLE"] = "1";
//                mapAttrDR["UIWIDTH"] = "30";
//                mapAttrDR["UIHEIGHT"] = "23";
//                dtMapAttr.Rows.Add(mapAttrDR);
//            }
//            #endregion  Deal with  RB  Enum value 

//            #region deleted.
//            string sqls = "";
//            foreach (DataTable ysdt in this.dsOldest.Tables)
//            {

//                DataTable newDt = dsLatest.Tables[ysdt.TableName];
//                if (newDt == null)
//                    continue;

//                string pk = "";
//                #region 求pK
//                foreach (DataColumn dc in ysdt.Columns)
//                {
//                    switch (dc.ColumnName.ToLower())
//                    {
//                        case "mypk":
//                            pk = "MyPK";
//                            break;
//                        case "no":
//                            pk = "No";
//                            break;
//                        case "oid":
//                            pk = "OID";
//                            break;
//                        case "nodeid":
//                            pk = "NodeID";
//                            break;
//                        default:
//                            break;
//                    }
//                }
//                #endregion 求pK

//                foreach (DataRow dr in ysdt.Rows)
//                {
//                    string pkVal = dr[pk].ToString();
//                    bool isHave = false;


//                    #region WF_Node, Does not contain FK_MapData Temporarily placed in front .
//                    if (ysdt.TableName == "WF_Node")
//                    {
//                        if (dr["NODEID"] != Glo.FK_MapData.Replace("ND", ""))
//                            continue;
//                        /*  If the judge is to examine the components of  .. */
//                        foreach (DataRow newDr in newDt.Rows)
//                        {
//                            if (newDr[pk].ToString() == pkVal)
//                            {
//                                isHave = true;
//                                break;
//                            }
//                        }
//                        if (isHave == false)
//                        {
//                            sqls += "@UPDATE WF_Node SET FWCSta=0 WHERE " + pk + "='" + pkVal + "'";
//                        }
//                        break;
//                    }
//                    #endregion


//                    if (ysdt.TableName == "Sys_MapData")
//                    {
//                        #region Sys_MapData
//                        if (isDesignerSizeChanged)
//                        {

//                            double
//                                heigh = double.NaN,
//                                wid = double.NaN;
//                            string no;

//                            object tmp = newDt.Rows[0]["NO"];
//                            no = tmp == null ? null : tmp.ToString();

//                            tmp = newDt.Rows[0]["FRMW"];
//                            if (null != tmp)
//                                double.TryParse(tmp.ToString(), out wid);


//                            tmp = newDt.Rows[0]["FRMH"];
//                            if (null != tmp)
//                                double.TryParse(tmp.ToString(), out heigh);
//                            if (!string.IsNullOrEmpty(no) && heigh != double.NaN && wid != double.NaN)
//                                sqls += "@UPDATE Sys_MapData SET FrmW=" + wid + ", FrmH=" + heigh + " WHERE No='" + no + "'";

//                            //wid = Glo.HisMapData.FrmW;
//                            //heigh = Glo.HisMapData.FrmH;
//                            //no = Glo.FK_MapData;

//                            //if (!string.IsNullOrEmpty(no) && heigh != double.NaN && wid != double.NaN)
//                            //    sqls += "@UPDATE Sys_MapData SET FrmW=" + wid + ", FrmH=" + heigh + " WHERE No='" + no + "'";
//                        }
//                        #endregion
//                        continue;
//                    }

//                    try
//                    {
//                        string id = dr["NODEID"] as string;
//                        continue;
//                    }
//                    catch (Exception) { }


//                    if ((dr["FK_MAPDATA"] as string) != Glo.FK_MapData)
//                        continue;


//                    if (ysdt.TableName == "Sys_MapAttr")
//                    {
//                        /*  If the field control  .. */
//                        isHave = false;
//                        foreach (DataRow newDr in newDt.Rows)
//                        {
//                            if (dr["FK_MAPDATA"] != Glo.FK_MapData || dr["UIVISIBLE"] == "0")
//                            {
//                                isHave = true;
//                                break;
//                            }
//                            if (newDr[pk].ToString() == pkVal)
//                            {
//                                isHave = true;
//                                break;
//                            }
//                        }

//                        if (isHave == false)
//                        {
//                            if (dr["UIVISIBLE"] == "0" || dr["EDITTYPE"] != "0")
//                                isHave = true;
//                        }
//                    }
//                    else
//                    {
//                        foreach (DataRow newDr in newDt.Rows)
//                        {
//                            if (newDr[pk].ToString() == pkVal)
//                            {
//                                isHave = true;
//                                break;
//                            }
//                        }
//                    }
//                    if (isHave == false)
//                        sqls += "@DELETE FROM " + ysdt.TableName + " WHERE " + pk + "='" + pkVal + "'";
//                }
//            }
//            #endregion

//            #region update.
//            string len = Glo.LEN_Function;
//            foreach (UIElement ctl in this.workSpace.Children)
//            {
//                if (!(ctl is IElement)) continue;

//                if (ctl is BPCheckBox)
//                {
//                    BPCheckBox cb = ctl as BPCheckBox;
//                    if (null == cb || string.IsNullOrEmpty(cb.KeyName)) continue;

//                    Label mylab = cb.Content as Label;
//                    sqls += "@UPDATE Sys_MapAttr SET Name='" + mylab.Content + "'  WHERE MyPK='" + Glo.FK_MapData + "_" + cb.Name + "' AND " + len + "(Name)=0";
//                    continue;

//                }
//                else if (ctl is BPTextBox)
//                {
//                    BPTextBox tb = ctl as BPTextBox;
//                    if (tb == null || string.IsNullOrEmpty(tb.KeyName))
//                        continue;

//                    sqls += "@UPDATE Sys_MapAttr SET Name='" + tb.KeyName + "' WHERE MyPK='" + Glo.FK_MapData + "_" + tb.Name + "' AND ( " + len + "(Name)=0 OR KeyOfEn=Name )";
//                    continue;

//                }
//                else if (ctl is BPDDL)
//                {
//                    BPDDL ddl = ctl as BPDDL;

//                    if (ddl == null || string.IsNullOrEmpty(ddl.KeyName))
//                        continue;

//                    sqls += "@UPDATE Sys_MapAttr SET Name='" + ddl.KeyName + "' WHERE MyPK='" + Glo.FK_MapData + "_" + ddl.Name + "' AND " + len + "(Name)=0";
//                    continue;

//                }
//                else if (ctl is BPRadioBtn)
//                {
//                    BPRadioBtn rb = ctl as BPRadioBtn;
//                    if (rb == null || string.IsNullOrEmpty(rb.KeyName) || sqls.Contains("_" + rb.GroupName))
//                        continue;

//                    sqls += "@UPDATE Sys_MapAttr SET Name='" + rb.KeyName + "' WHERE MyPK='" + Glo.FK_MapData + "_" + rb.GroupName + "' AND " + len + "(Name)=0";
//                    continue;

//                }
//                else if (ctl is BPLine)
//                {
//                    BPLine line = ctl as BPLine;
//                    if (line == null || string.IsNullOrEmpty(line.Name))
//                        continue;

//                    #region line.


//                    double
//                        BorderWidth = double.NaN,

//                        x1 = double.NaN,
//                        x2 = double.NaN,
//                        y1 = double.NaN,
//                        y2 = double.NaN;


//                    string MyPK = line.Name;
//                    string BorderColor = line.MyLine.StrokeThickness.ToString();
//                    string FK_MapData = Glo.FK_MapData;
//                    string tmp = Glo.PreaseColorToName(((SolidColorBrush)line.MyLine.Stroke).Color.ToString());
//                    if (null != tmp)
//                        double.TryParse(tmp.ToString(), out BorderWidth);


//                    tmp = line.MyLine.X1.ToString("0.00");
//                    if (null != tmp)
//                        double.TryParse(tmp.ToString(), out x1);

//                    tmp = line.MyLine.X2.ToString("0.00");
//                    if (null != tmp)
//                        double.TryParse(tmp.ToString(), out x2);

//                    tmp = line.MyLine.Y1.ToString("0.00");
//                    if (null != tmp)
//                        double.TryParse(tmp.ToString(), out y1);
//                    tmp = line.MyLine.Y2.ToString("0.00");
//                    if (null != tmp)
//                        double.TryParse(tmp.ToString(), out y2);

//                    string sqlTmp = "@UPDATE Sys_FrmLine SET  X1={1},Y1={2},X2={3},Y2={4} WHERE MyPK='{0}'";
//                    if (!string.IsNullOrEmpty(MyPK) && x1 != double.NaN && y1 != double.NaN && x2 != double.NaN && y2 != double.NaN)
//                        sqls += string.Format(sqlTmp, MyPK, x1, y1, x2, y2);


//                    continue;
//                    #endregion
//                }
//            }
//            #endregion

//            string xml = dsLatest.ToXml(true, false);
//            FF.CCFormSoapClient da = Glo.GetCCFormSoapClientServiceInstance();
//            da.SaveFrmAsync(Glo.FK_MapData, xml, sqls, null);
//            da.SaveFrmCompleted += ((object senders, FF.SaveFrmCompletedEventArgs ee) =>
//            {
//                isDesignerSizeChanged = false;
//                loadingWindow.DialogResult = true;
//                #region
//                if (ee.Error != null)
//                {
//                    BP.SL.LoggerHelper.Write(ee.Error);
//                    MessageBox.Show(ee.Result, " Save error ", MessageBoxButton.OK);
//                    return;
//                }

//                if (Keyboard.Modifiers == ModifierKeys.Windows)
//                {
//                    string url1 = null;
//                    if (Glo.IsDtlFrm == false)
//                        url1 = Glo.BPMHost + "/WF/CCForm/Frm.aspx?FK_MapData=" + Glo.FK_MapData + "&IsTest=1&WorkID=0&FK_Node=" + Glo.FK_Node + "&sd=s" + Glo.TimeKey;
//                    else
//                        url1 = Glo.BPMHost + "/WF/CCForm/FrmCard.aspx?EnsName=" + Glo.FK_MapData + "&RefPKVal=0&OID=0" + Glo.TimeKey;

//                    Glo.WinOpen(url1, (int)Glo.HisMapData.FrmH, (int)Glo.HisMapData.FrmW);
//                }
//                else
//                {
//                    //  MessageBox.Show("ccform  Saved successfully .", " Saving tips ", MessageBoxButton.OK);
//                }


//                #endregion
//            });
//        }
        #endregion

        #region UnUsed

        void treeViewItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //e.Handled = true;
            //if (_doubleClickTimer.IsEnabled)
            //{
            //    _doubleClickTimer.Stop();
            //    TreeViewItem tvItem = this.tvNode.SelectedItem as TreeViewItem;
            //    if (tvItem == null)
            //        return;

            //    if (tvItem.Tag == null)
            //    {
            //        Glo.FK_MapData = tvItem.Name.ToString();
            //        Glo.FK_Node = int.Parse(tvItem.Name.Replace("ND", ""));
            //    }
            //    else
            //    {
            //        string[] strs = tvItem.Name.Split('_');
            //        Glo.FK_Node = int.Parse(strs[0]);
            //        if (strs.Length == 3)
            //        {
            //            Glo.IsDtlFrm = true;
            //            Glo.FK_MapData = strs[2];
            //        }
            //        else
            //        {
            //            Glo.IsDtlFrm = false;
            //            Glo.FK_MapData = strs[1];
            //        }
            //    }
            //    this.BindFrm();
            //}
            //else
            //{
            //    _doubleClickTimer.Start();
            //}
        }
        private void tvmi_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //e.Handled = true;
            //MenuItem item = sender as MenuItem;
            //TreeViewItem li = this.tvNode.SelectedItem as TreeViewItem;
            //if (li == null)
            //    return;

            //switch (item.Name)
            //{
            //    case "FrmBill":
            //        MessageBox.Show(" Setting mode , Please see the configuration section details the operating instructions , Visual design in the development process .");
            //        break;
            //    case "FrmRef":
            //        this.BindTreeView();
            //        break;
            //    case "FrmAction":
            //        //FrmEvent frm = new FrmEvent();
            //        //frm.Show();
            //        string host = Glo.BPMHost + "/WF/MapDef/FrmEvent.aspx?FK_MapData=" + Glo.FK_MapData;
            //        HtmlPage.Window.Eval("window.showModalDialog('" + host + "',window,'dialogHeight:600px;dialogWidth:450px;center:Yes;help:No;scroll:auto;resizable:1;status:No;');");
            //        return;
            //    case "RefFrm":
            //        if (li.Tag != null)
            //        {
            //            BP.WF.FrmNode myfn = new BP.WF.FrmNode(li.Tag.ToString());
            //            this.winNodeFrms.FK_Node = myfn.FK_Node;
            //        }
            //        else
            //        {
            //            this.winNodeFrms.FK_Node = int.Parse(li.Name.Replace("ND", ""));
            //        }
            //        this.winNodeFrms.listBox1.Items.Clear();
            //        this.winNodeFrms.Show();
            //        break;
            //    case "NewFrm":
            //        // BP.WF.FrmNode fn = new BP.WF.FrmNode(li.Tag.ToString());

            //        this.winFlowFrm.TB_No.Text = "";
            //        this.winFlowFrm.TB_No.IsEnabled = true;
            //        this.winFlowFrm.TB_Name.Text = "";
            //        this.winFlowFrm.TB_PTable.Text = "";
            //        this.winFlowFrm.TB_URL.Text = "";
            //        this.winFlowFrm.NodeID = int.Parse(li.Name.Replace("ND", ""));
            //        this.winFlowFrm.Show();
            //        break;
            //    case "DeleteFrm":
            //        if (li.Tag == null)
            //            return;

            //        if (Glo.IsDtlFrm == true)
            //        {
            //            if (MessageBox.Show(" Single list can not be deleted .",
            //           " Prompt ", MessageBoxButton.OK)
            //           == MessageBoxResult.No)
            //                return;
            //            return;
            //        }

            //        if (MessageBox.Show(" Are you sure you want to delete it ? If you delete all associated nodes form will be deleted !!!",
            //            " Delete Tip ", MessageBoxButton.OKCancel)
            //            == MessageBoxResult.No)
            //            return;

            //        string[] strs = li.Name.Split('_');
            //        string fk_frm = strs[1];
            //        Glo.FK_MapData = "ND" + strs[0];
            //        Glo.FK_Node = int.Parse(strs[0]);
            //        this.DoTypeName = "DeleteFrm";
            //        this.DoType(this.DoTypeName, fk_frm, null, null, null, null);
            //        break;
            //    case "EditFrm":
            //        if (li.Tag == null)
            //            return;

            //        if (Glo.IsDtlFrm)
            //            return;

            //        BP.WF.FrmNode fn = new BP.WF.FrmNode(li.Tag.ToString());
            //        this.winFlowFrm.TB_No.Text = fn.No;
            //        this.winFlowFrm.TB_Name.Text = fn.Name;
            //        this.winFlowFrm.DDL_FrmType.SelectedIndex = fn.FormType;
            //        this.winFlowFrm.TB_URL.Text = fn.URL;
            //        this.winFlowFrm.TB_PTable.Text = fn.PTable;
            //        this.winFlowFrm.CB_IsReadonly.IsChecked = fn.IsReadonly;
            //        this.winFlowFrm.NodeID = fn.FK_Node;
            //        this.winFlowFrm.Show();
            //        break;
            //    case "DeFrm": //  Design a Form .
            //        Glo.IsDtlFrm = false;
            //        if (li.Tag == null)
            //        {
            //            Glo.FK_MapData = li.Name as string;
            //            if (Glo.FK_MapData.Contains("ND"))
            //                Glo.FK_Node = int.Parse(Glo.FK_MapData.Replace("ND", ""));
            //        }
            //        else
            //        {
            //            string[] str = li.Name.Split('_');
            //            Glo.FK_Node = int.Parse(str[0]);
            //            if (str.Length == 3)
            //            {
            //                Glo.IsDtlFrm = true;
            //                Glo.FK_MapData = str[2];
            //            }
            //            else
            //            {
            //                Glo.FK_MapData = str[1];
            //                Glo.IsDtlFrm = true;
            //            }
            //        }
            //        this.BindFrm();
            //        break;
            //    case "FrmUp":   //  Move .
            //    case "FrmDown": //  Move .
            //        if (li.Name.ToString().Contains("ND"))
            //            return;

            //        string[] strs1 = li.Name.Split('_');
            //        Glo.FK_MapData = strs1[1];
            //        Glo.FK_Node = int.Parse(strs1[0]);
            //        this.DoType(item.Name, Glo.FK_Node.ToString(), Glo.FK_MapData, null, null, null);
            //        break;
            //    default:
            //        break;
            //}
        }


        public void BindTreeView()
        {
            BindTreeView(Glo.FK_Flow);
        }
        public void BindTreeView(string FK_Flow)
        {
            string
            sqls = "SELECT NodeID, Name,Step FROM WF_Node WHERE FK_Flow='" + FK_Flow + "'";
            sqls += "@SELECT * FROM WF_FrmNode WHERE FK_Flow='" + FK_Flow + "' AND FK_Frm IN (SELECT No FROM Sys_MapData ) ORDER BY FK_Node, Idx";
            sqls += "@SELECT * FROM Sys_MapData ";
            sqls += "@SELECT * FROM Sys_MapDtl WHERE  FK_MapData IN( SELECT No FROM Sys_MapData WHERE FK_Flow='" + FK_Flow + "') AND DtlShowModel=1";
            FF.CCFormSoapClient da = Glo.GetCCFormSoapClientServiceInstance();
            da.RunSQLReturnTableSAsync(sqls);
            da.RunSQLReturnTableSCompleted += (object sender, FF.RunSQLReturnTableSCompletedEventArgs e) =>
            {
                if (null != e.Error)
                {
                    BP.SL.LoggerHelper.Write(e.Error);
                    MessageBox.Show(e.Error.Message);
                    return;
                }
                try
                {
                    DataSet ds = new DataSet();
                    ds.FromXml(e.Result);

                }
                catch (Exception ee)
                {
                    BP.SL.LoggerHelper.Write(ee);
                    MessageBox.Show(ee.Message);
                }
            };
        }
        //private string DoTypeName = null;
        //public void RunSQL(string sql)
        //{
        //    FF.CCFormSoapClient da = Glo.GetCCFormSoapClientServiceInstance();
        //    da.RunSQLsAsync(sql);
        //    da.RunSQLsCompleted += new EventHandler<FF.RunSQLsCompletedEventArgs>(
        //        (object sender, FF.RunSQLsCompletedEventArgs e) =>
        //        {
        //            if (e.Error != null)
        //            {
        //                MessageBox.Show(e.Error.Message, " Execution information ", MessageBoxButton.OK);
        //                return;
        //            }

        //            DoTypeCompleted(this.DoTypeName);
        //        });
        //}
        //public void DoType(string doType, string v1, string v2, string v3, string v4, string v5)
        //{
        //    this.DoTypeName = doType;
        //    FF.CCFormSoapClient da = Glo.GetCCFormSoapClientServiceInstance();
        //    da.DoTypeAsync(doType, v1, v2, v3, v4, v5);
        //    da.DoTypeCompleted += new EventHandler<FF.DoTypeCompletedEventArgs>((object sender, FF.DoTypeCompletedEventArgs e)=>
        //    {
        //        if (e.Error != null)
        //        {
        //            MessageBox.Show(e.Error.Message, " Execution information ", MessageBoxButton.OK);
        //            return;
        //        }
        //        if (e.Result != null)
        //        {
        //            MessageBox.Show(e.Result, " Execution information ", MessageBoxButton.OK); 
        //            return;
        //        }

        //        DoTypeCompleted(doType);
        //    });
        //}
        //void DoTypeCompleted(string doType)
        //{
        //    switch (doType)
        //    {
        //        case "FrmUp":
        //        case "FrmDown":
        //        case "DeleteFrm":
        //            this.BindTreeView();
        //            break;
        //        default:
        //              MessageBox.Show(" Write achieve "+ doType);
        //            break;
        //    }
        //}

        #endregion 
    }
}
