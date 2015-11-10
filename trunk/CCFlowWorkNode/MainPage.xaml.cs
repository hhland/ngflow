using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using Silverlight;
using BP.En;
using BP.Sys;
using BP.Sys.SL;
using BP.WF;
using System.Linq;
using System.Windows.Controls.Primitives;
using WorkNode.FF;
using System.Windows.Resources;

namespace WorkNode
{
    public partial class MainPage : UserControl
    {
        #region zhoupeng add  Global Variables 
        LoadingWindow loadingWindow = new LoadingWindow();
        #endregion  Global Variables 

        public void SetGridLines()
        {
            #region  Determine whether there .
            int mynum = this.canvasMain.Children.Count;
            string ids = "";
            for (int i = 0; i < mynum; i++)
            {
                Line mylin = this.canvasMain.Children[i] as Line;
                if (mylin == null)
                    continue;

                if (mylin.Name == null)
                    continue;

                if (mylin.Name.Contains("GLine"))
                {
                    ids += "@" + mylin.Name;
                }
            }

            if (ids != "")
            {
                string[] myids = ids.Split('@');
                foreach (string id in myids)
                {
                    if (string.IsNullOrEmpty(id))
                        continue;

                    Line mylin = this.canvasMain.FindName(id) as Line;
                    if (mylin == null)
                        continue;
                    this.canvasMain.Children.Remove(mylin);
                }
                return;
            }
            #endregion  Determine whether there .

            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Color.FromArgb(255, 160, 160, 160);
            //  brush.Color = Color.FromArgb(255, 255, 255, 255);
            double thickness = 0.3;
            double top = 0;
            double left = 0;
            double width = canvasMain.Width;
            double height = canvasMain.Height;
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
                line.Stretch = Stretch.Fill;
                canvasMain.Children.Add(line);
                x += stepLength;
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
                canvasMain.Children.Add(line);
                y += stepLength;
            }
        }
        FF.CCFlowAPISoapClient da;
        /// <summary>
        ///  Entry point 
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            #region  Get parameters 
            da = Glo.GetCCFlowAPISoapClientServiceInstance();
            da.GenerWorkNode_SLAsync(Glo.FK_Flow, Glo.FK_Node, Glo.WorkID, Glo.FID, BP.Port.WebUser.No);
            da.GenerWorkNode_SLCompleted += new EventHandler<FF.GenerWorkNode_SLCompletedEventArgs>(BindFrm);
            da.GetNoNameCompleted += new EventHandler<FF.GetNoNameCompletedEventArgs>(da_GetNoNameCompleted);
            #endregion  Get parameters 

            lb.SelectionChanged += new SelectionChangedEventHandler(lb_SelectionChanged);
        }

        void lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem li = lb.SelectedItem as ListBoxItem;
            if (li == null)
            {
                return;
            }
            DataRow dr = li.Tag as DataRow;
            tb.Text = dr["No"].ToString();
            ///////////////////////////////////////////////////////

            ///////////////////////////////////////////////////////

            DataGrid dg = (tb.Tag as object[])[0] as DataGrid;
            if (dg == null)
            {
                DataTable dt = lb.Tag as DataTable;
                if (dt != null)
                {
                    foreach (var item1 in this.canvasMain.Children)
                    {
                        foreach (var item2 in dt.Columns)
                        {
                            var fe = (item1 as FrameworkElement);
                            if (fe.Name == item2.ColumnName)
                            {
                                if (fe is TextBox)
                                {
                                    (fe as TextBox).Text = dr[fe.Name];
                                }
                                else if (fe is BPDDL)
                                {
                                    // (fe as BPDDL).SelectedIndex = Convert.ToInt32(dr[fe.Name]);
                                    (fe as BPDDL).SetSelectVal(dr[fe.Name]);
                                }
                            }
                        }

                    }
                }
                popup.IsOpen = false;
                return;
            }

            DataGridCell dgc = (DataGridCell)(tb.Parent);

            //SELECT No,Name , Tel as DianHua, FK_Dept from wf_emp where no like 'zhangyifan%'

            foreach (var item in dg.Columns)
            {
                var fe = item.GetCellContent(DataGridRow.GetRowContainingElement(dgc));
                foreach (var dc in dtResult.Columns)
                {
                    if (dc.ColumnName == fe.Name)
                    {
                        if (fe is TextBox)
                        {
                            (fe as TextBox).Text = dr[fe.Name];
                        }
                        else if (fe is BPDDL)
                        {
                            (fe as BPDDL).SelectedIndex = Convert.ToInt32(dr[fe.Name]);
                        }

                    }
                }

            }

            //    if (fe.Name == dr["AttrOfOper"])
            //    {
            //        //fe.Tag = dr;
            //        //TextBox tb = fe as TextBox;
            //        //tb.KeyDown += new KeyEventHandler(tb_KeyDown);

            //        //tb.KeyDown +=new KeyEventHandler(tb_KeyDown);
            //        //string sql = dr["Doc"];
            //        //// SELECT No,Name , Tel as DianHua, FK_Dept from wf_emp where no like ~@Key%~
            popup.IsOpen = false;
        }


        /// <summary>
        ///  Unbound form 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BindFrm(object sender, FF.GenerWorkNode_SLCompletedEventArgs e)
        {
            #region  Initialization data .
            this.canvasMain.Children.Clear();
            this.FrmDS = new DataSet();
            try
            {
                if (e.Result.Length < 200)
                    throw new Exception(e.Result);
                this.FrmDS.FromXml(e.Result);
                loadingWindow.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Err", MessageBoxButton.OK);
                loadingWindow.DialogResult = true;
                return;
            }
            Glo.FrmDS = this.FrmDS;
            #endregion  Initialization data .

            //  Show Process Control .
            this.InitToolbar();

            string table = "";
            try
            {
                this.dtMapAttrs = this.FrmDS.Tables["Sys_MapAttr"];

                // Through all the tables .
                foreach (DataTable dt in this.FrmDS.Tables)
                {
                    Glo.TempVal = dt.TableName;
                    table = dt.TableName;

                    switch (dt.TableName)
                    {
                        case "Sys_MapAttr":
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (dr["FK_MapData"] != Glo.FK_MapData)
                                    continue; // Field of non-primary table .

                                if (dr["UIVisible"] == "0")
                                    continue;

                                string myPk = dr["MyPK"];
                                string keyOfEn = dr["KeyOfEn"]; //  Field name .
                                string defVal = dr["DefVal"];
                                string UIContralType = dr["UIContralType"]; // Control Types . 0=textbox, 1=ddl, 2=checkbox, 3=radiobutton
                                string MyDataType = dr["MyDataType"]; // Data Types . string, int ,float.
                                string lgType = dr["LGType"]; // Logic Type . 0 General ,1 Enumeration ,2 Foreign keys .
                                bool isEnable = false;
                                if (dr["UIIsEnable"] == null || dr["UIIsEnable"].ToString() == "1")
                                    isEnable = true; // Is available .

                                double X = double.Parse(dr["X"]);
                                double Y = double.Parse(dr["Y"]);
                                if (X == 0)
                                    X = 100;
                                if (Y == 0)
                                    Y = 100;

                                string UIBindKey = dr["UIBindKey"];
                                /*
                                  With respect to UIBindKey.
                                 *  // Binding outside or enumeration values .   If the enumeration values in  Sys_Enum, 
                                 *  // If it is a foreign key to the foreign key table to find .  Such as :CN_City
                                 */


                                switch (UIContralType)
                                {
                                    case CtrlType.TextBox:
                                        TBType tp = TBType.String;
                                        //  In the case of textbox  Type of control , It is necessary to determine the type of data .
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

                                        BPTextBox tb = new BPTextBox();
                                        tb.TBType = tp;
                                        tb.Name = keyOfEn;
                                        tb.NameOfReal = dr["Name"];
                                        tb.SetValue(Canvas.LeftProperty, X);
                                        tb.SetValue(Canvas.TopProperty, Y);
                                        tb.Text = this.GetValByKey(keyOfEn); // Assigned to the control .
                                        tb.Width = double.Parse(dr["UIWidth"]);
                                        if (tb.Height > 24)
                                            tb.TextWrapping = TextWrapping.Wrap;
                                        tb.Height = double.Parse(dr["UIHeight"]);
                                        if (isEnable)
                                            tb.IsEnabled = true;
                                        else
                                            tb.IsEnabled = false;

                                        this.canvasMain.Children.Add(tb);
                                        break;
                                    case CtrlType.DDL:
                                        BPDDL ddl = new BPDDL();
                                        ddl.Name = keyOfEn;
                                        ddl.HisLGType = lgType;
                                        ddl.Width = double.Parse(dr["UIWidth"]);
                                        ddl.UIBindKey = UIBindKey;
                                        ddl.HisLGType = lgType;
                                        if (lgType == LGType.Enum)
                                        {
                                            DataTable dtEnum = this.FrmDS.Tables["Sys_Enum"];
                                            foreach (DataRow drEnum in dtEnum.Rows)
                                            {
                                                if (drEnum["EnumKey"].ToString() != UIBindKey)
                                                    continue;
                                                ComboBoxItem li = new ComboBoxItem();
                                                li.Tag = drEnum["IntKey"].ToString();
                                                li.Content = drEnum["Lab"].ToString();
                                                ddl.Items.Add(li);
                                            }
                                            if (ddl.Items.Count == 0)
                                                throw new Exception("@ No from Sys_Enum Found in numbered (" + UIBindKey + ") The enumeration value .");
                                        }
                                        else
                                        {
                                            ddl.BindEns(UIBindKey);
                                        }

                                        ddl.SetValue(Canvas.LeftProperty, X);
                                        ddl.SetValue(Canvas.TopProperty, Y);

                                        // Assigned to the control .
                                        ddl.SetSelectVal(this.GetValByKey(keyOfEn));

                                        this.canvasMain.Children.Add(ddl);
                                        break;
                                    case CtrlType.CheckBox:
                                        BPCheckBox cb = new BPCheckBox();
                                        cb.Name = keyOfEn;

                                        cb.SetValue(Canvas.LeftProperty, X);
                                        cb.SetValue(Canvas.TopProperty, Y);

                                        if (this.GetValByKey(keyOfEn) == "1")
                                            cb.IsChecked = true;
                                        else
                                            cb.IsChecked = false;

                                        this.canvasMain.Children.Add(cb);
                                        break;
                                    case CtrlType.RB:
                                        break;
                                    default:
                                        break;
                                }
                            }
                            continue;

                        case "Sys_FrmRB":
                            DataTable dtRB = this.FrmDS.Tables["Sys_FrmRB"];
                            foreach (DataRow dr in dtRB.Rows)
                            {
                                if (dr["FK_MapData"] != Glo.FK_MapData)
                                    continue;

                                BPRadioBtn btn = new BPRadioBtn();
                                btn.Name = dr["MyPK"];
                                btn.GroupName = dr["KeyOfEn"];
                                btn.Content = dr["Lab"];
                                btn.UIBindKey = dr["EnumKey"];
                                btn.Tag = dr["IntKey"];
                                btn.SetValue(Canvas.LeftProperty, double.Parse(dr["X"].ToString()));
                                btn.SetValue(Canvas.TopProperty, double.Parse(dr["Y"].ToString()));
                                this.canvasMain.Children.Add(btn);
                            }
                            continue;
                        case "Sys_MapDtl": // From the description of the table .
                            foreach (DataRow dr in dt.Rows)
                            {
                                //BP.En.MapDtl dtlEn = new MapDtl();
                                //dtlEn.No=dr["No"].ToString();
                                //dtlEn.Name = dr["Name"].ToString();
                                //dtlEn.W = dr["W"].ToString();
                                //dtlEn.H = dr["H"].ToString();

                                BPDtl dtl = new BPDtl(dr, this.FrmDS);


                                dtl.SetValue(Canvas.LeftProperty, double.Parse(dr["X"]));
                                dtl.SetValue(Canvas.TopProperty, double.Parse(dr["Y"]));

                                dtl.Width = double.Parse(dr["W"]);
                                dtl.Height = double.Parse(dr["H"]);

                                this.canvasMain.Children.Add(dtl);

                            }
                            continue;
                        case "Sys_FrmEle": // Other elements of the table .
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (dr["FK_MapData"] != Glo.FK_MapData)
                                    continue;

                                BPEle img = new BPEle();
                                img.Name = dr["MyPK"].ToString();
                                img.EleType = dr["EleType"].ToString();
                                img.EleName = dr["EleName"].ToString();
                                img.EleID = dr["EleID"].ToString();

                                img.Cursor = Cursors.Hand;
                                img.SetValue(Canvas.LeftProperty, double.Parse(dr["X"].ToString()));
                                img.SetValue(Canvas.TopProperty, double.Parse(dr["Y"].ToString()));

                                img.Width = double.Parse(dr["W"].ToString());
                                img.Height = double.Parse(dr["H"].ToString());
                                this.canvasMain.Children.Add(img);
                            }
                            continue;
                        case "Sys_MapData":
                            if (dt.Rows.Count == 0)
                                continue;
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (dr["No"] != Glo.FK_MapData)
                                    continue;

                                Glo.HisMapData = new MapData();
                                Glo.HisMapData.FrmH = double.Parse(dt.Rows[0]["FrmH"]);
                                Glo.HisMapData.FrmW = double.Parse(dt.Rows[0]["FrmW"]);
                                Glo.HisMapData.No = (string)dt.Rows[0]["No"];
                                Glo.HisMapData.Name = (string)dt.Rows[0]["Name"];
                                // Glo.IsDtlFrm = false;
                                this.canvasMain.Width = Glo.HisMapData.FrmW;
                                this.canvasMain.Height = Glo.HisMapData.FrmH;
                                this.scrollViewer1.Width = Glo.HisMapData.FrmW;
                            }
                            break;
                        case "Sys_FrmBtn": // Push button .
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (dr["FK_MapData"] != Glo.FK_MapData)
                                    continue;

                                BPBtn btn = new BPBtn();
                                btn.Name = dr["MyPK"];
                                btn.Content = dr["Text"].Replace("&nbsp;", " ");
                                btn.HisBtnType = (BtnType)int.Parse(dr["BtnType"]);
                                btn.HisEventType = (EventType)int.Parse(dr["EventType"]);

                                if (dr["EventContext"] != null)
                                    btn.EventContext = dr["EventContext"].Replace("~", "'");

                                if (dr["MsgErr"] != null)
                                    btn.MsgErr = dr["MsgErr"].Replace("~", "'");

                                if (dr["MsgOK"] != null)
                                    btn.MsgOK = dr["MsgOK"].Replace("~", "'");

                                btn.SetValue(Canvas.LeftProperty, double.Parse(dr["X"]));
                                btn.SetValue(Canvas.TopProperty, double.Parse(dr["Y"]));
                                this.canvasMain.Children.Add(btn);
                            }
                            continue;
                        case "Sys_FrmLine": //线.
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (dr["FK_MapData"] != Glo.FK_MapData)
                                    continue;

                                string color = dr["BorderColor"];
                                if (string.IsNullOrEmpty(color))
                                    color = "Black";

                                BPLine myline = new BPLine(dr["MyPK"], color, double.Parse(dr["BorderWidth"]),
                                    double.Parse(dr["X1"]), double.Parse(dr["Y1"]), double.Parse(dr["X2"]),
                                    double.Parse(dr["Y2"]));

                                myline.SetValue(Canvas.LeftProperty, double.Parse(dr["X"]));
                                myline.SetValue(Canvas.TopProperty, double.Parse(dr["Y"]));
                                this.canvasMain.Children.Add(myline);
                            }
                            continue;
                        case "Sys_FrmLab": // Label .
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (dr["FK_MapData"] != Glo.FK_MapData)
                                    continue;

                                BPLabel lab = new BPLabel();
                                lab.Name = dr["MyPK"];
                                string text = dr["Text"].Replace("&nbsp;", " ");
                                text = text.Replace("@", "\n");
                                lab.Content = text;
                                lab.FontSize = double.Parse(dr["FontSize"]);
                                lab.Cursor = Cursors.Hand;
                                lab.SetValue(Canvas.LeftProperty, double.Parse(dr["X"]));
                                lab.SetValue(Canvas.TopProperty, double.Parse(dr["Y"]));

                                if (dr["IsBold"] == "1")
                                    lab.FontWeight = FontWeights.Bold;
                                else
                                    lab.FontWeight = FontWeights.Normal;

                                string color = dr["FontColor"];
                                lab.Foreground = new SolidColorBrush(Glo.ToColor(color));
                                this.canvasMain.Children.Add(lab);
                            }
                            continue;
                        case "Sys_FrmLink": // Hyperlinks .
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (dr["FK_MapData"] != Glo.FK_MapData)
                                    continue;

                                BPLink link = new BPLink();
                                link.Name = dr["MyPK"];
                                link.Content = dr["Text"];
                                link.URL = dr["URL"];

                                link.WinTarget = dr["Target"];

                                link.FontSize = double.Parse(dr["FontSize"]);
                                link.Cursor = Cursors.Hand;
                                link.SetValue(Canvas.LeftProperty, double.Parse(dr["X"]));
                                link.SetValue(Canvas.TopProperty, double.Parse(dr["Y"]));

                                string color = dr["FontColor"];
                                if (string.IsNullOrEmpty(color))
                                    color = "Black";

                                link.Foreground = new SolidColorBrush(Glo.ToColor(color));
                                this.canvasMain.Children.Add(link);
                            }
                            continue;
                        case "Sys_FrmImg": // Picture .
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (dr["FK_MapData"] != Glo.FK_MapData)
                                    continue;

                                BPImg img = new BPImg();
                                img.Name = dr["MyPK"];
                                img.Cursor = Cursors.Hand;
                                img.SetValue(Canvas.LeftProperty, double.Parse(dr["X"].ToString()));
                                img.SetValue(Canvas.TopProperty, double.Parse(dr["Y"].ToString()));

                                img.Width = double.Parse(dr["W"].ToString());
                                img.Height = double.Parse(dr["H"].ToString());
                                this.canvasMain.Children.Add(img);
                            }
                            continue;
                        case "Sys_FrmImgAth": // Image Attachment 
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (dr["FK_MapData"] != Glo.FK_MapData)
                                    continue;

                                Border border = new Border();
                                border.BorderBrush = new SolidColorBrush(Colors.LightGray);
                                border.BorderThickness = new Thickness(1);
                                Image ath = new Image();
                                border.Child = ath;
                                ath.Name = dr["MyPK"];
                                ath.Cursor = Cursors.Hand;
                                border.SetValue(Canvas.LeftProperty, double.Parse(dr["X"].ToString()));
                                border.SetValue(Canvas.TopProperty, double.Parse(dr["Y"].ToString()));
                                border.Height = double.Parse(dr["H"].ToString());
                                border.Width = double.Parse(dr["W"].ToString());
                                string asmName = System.Reflection.Assembly.GetExecutingAssembly().FullName.Split(',')[0];
                                string fileName = "/" + asmName + ";component/Img/Logo/CCFlow/LogoH.png";
                                ath.Source = new BitmapImage(new Uri(fileName, UriKind.Relative));
                                ath.Stretch = Stretch.Fill;
                                ath.MouseLeftButtonDown += (sender2, e2) =>
                                {
                                    BPClipImg img = BPClipImg.Instance;
                                    img.Closed += (sender3, e3) =>
                                    {
                                        if (img.ClipImage != null)
                                        {
                                            ath.Source = img.ClipImage;
                                        }
                                    };
                                    img.Show();
                                };
                                this.canvasMain.Children.Add(border);
                            }
                            continue;

                        case "Sys_MapM2M":
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (dr["FK_MapData"] != Glo.FK_MapData)
                                    continue;
                                string Tag2 = dr["DBOfObjs"];
                                string Tag1 = dr["DBOfGroups"];
                                BPM2M m2m = new BPM2M(Tag1, Tag2);
                                m2m.SetValue(Canvas.LeftProperty, double.Parse(dr["X"]));
                                m2m.SetValue(Canvas.TopProperty, double.Parse(dr["Y"]));
                                m2m.Width = double.Parse(dr["W"]);
                                m2m.Height = double.Parse(dr["H"]);
                                this.canvasMain.Children.Add(m2m);
                            }
                            continue;
                        case "Sys_FrmAttachment":
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (dr["FK_MapData"] != Glo.FK_MapData)
                                    continue;

                                string uploadTypeInt = dr["UploadType"].ToString();
                                if (uploadTypeInt == null)
                                    uploadTypeInt = "0";

                                AttachmentUploadType uploadType = (AttachmentUploadType)int.Parse(uploadTypeInt);

                                if (uploadType == AttachmentUploadType.Single)
                                {
                                    BPUpload upload = new BPUpload();
                                    upload.Height = double.Parse(dr["H"]);
                                    upload.Height = upload.Height > 46 ? upload.Height : 46;
                                    upload.Width = double.Parse(dr["W"]);

                                    upload.SetValue(Canvas.LeftProperty, double.Parse(dr["X"]));
                                    upload.SetValue(Canvas.TopProperty, double.Parse(dr["Y"]));


                                    string str = dr["Name"] as string;
                                    upload.extension = dr["Exts"] as string;
                                    upload.SaveAs = dr["SaveTo"] as string;

                                    if (dr["IsUpload"] == "1")
                                        upload.IsCanUpload = true;
                                    else
                                        upload.IsCanUpload = false;

                                    if (dr["IsDelete"] == "1")
                                        upload.IsCanDelete = true;
                                    else
                                        upload.IsCanDelete = false;

                                    if (dr["IsDownload"] == "1")
                                        upload.IsCanDownload = true;
                                    else
                                        upload.IsCanDownload = false;

                                    this.canvasMain.Children.Add(upload);
                                    return;

                                    //BPAttachment ath = new BPAttachment(dr["NoOfObj"],
                                    //    dr["Name"], dr["Exts"],
                                    //    double.Parse(dr["W"]), dr["SaveTo"].ToString());
                                    //BPUpload ath = new BPUpload(dr["NoOfObj"], dr["Exts"], dr["SaveTo"]);
                                    PageSingle ath = new PageSingle();
                                    ath.SetValue(Canvas.LeftProperty, double.Parse(dr["X"]));
                                    ath.SetValue(Canvas.TopProperty, double.Parse(dr["Y"]));

                                    ath.Label = dr["Name"] as string;
                                    ath.FileFilter = dr["Exts"] as string;
                                    ath.SaveTo = dr["SaveTo"] as string;

                                    if (dr["IsUpload"] == "1")
                                        ath.IsUpload = true;
                                    else
                                        ath.IsUpload = false;

                                    if (dr["IsDelete"] == "1")
                                        ath.IsDelete = true;
                                    else
                                        ath.IsDelete = false;

                                    if (dr["IsDownload"] == "1")
                                        ath.IsDownload = true;
                                    else
                                        ath.IsDownload = false;

                                    this.canvasMain.Children.Add(ath);
                                    continue;
                                }

                                if (uploadType == AttachmentUploadType.Multi)
                                {
                                    // BPAttachmentM athM = new BPAttachmentM();
                                    //WorkNode.Page athM = new Page();
                                    //athM.SetValue(Canvas.LeftProperty, double.Parse(dr["X"]));
                                    //athM.SetValue(Canvas.TopProperty, double.Parse(dr["Y"]));
                                    //athM.Name = dr["NoOfObj"];
                                    //athM.Width = double.Parse(dr["W"]);
                                    //athM.Height = double.Parse(dr["H"]);
                                    ////athM.X = double.Parse(dr["X"]);
                                    ////athM.Y = double.Parse(dr["Y"]);
                                    ////athM.SaveTo = dr["SaveTo"];
                                    ////athM.Text = dr["Name"];
                                    ////athM.Label = dr["Name"];
                                    //this.canvasMain.Children.Add(athM);
                                    BPMultiUpload athM = new BPMultiUpload();                                
                                    athM.SetValue(Canvas.LeftProperty, double.Parse(dr["X"]));
                                    athM.SetValue(Canvas.TopProperty, double.Parse(dr["Y"]));
                                    athM.Name = dr["NoOfObj"];
                                    athM.Width = double.Parse(dr["W"]);
                                    athM.Height = double.Parse(dr["H"]);
                                    //athM.X = double.Parse(dr["X"]);
                                    //athM.Y = double.Parse(dr["Y"]);
                                    //athM.SaveTo = dr["SaveTo"];
                                    //athM.Text = dr["Name"];
                                    //athM.Label = dr["Name"];
                                    this.canvasMain.Children.Add(athM);
                                    continue;
                                }
                            }
                            continue;
                        default:
                            break;
                    }
                }
                loadingWindow.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("err:" + table, ex.Message + " " + ex.StackTrace,
                    MessageBoxButton.OK);
            }
            BindLogic();
            this.SetGridLines();
        }

        void BindLogic()
        {
            DataTable dt = this.FrmDS.Tables["Sys_MapExt"];
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {

                    string ExtType = dr["ExtType"];
                    string AttrOfOper = dr["AttrOfOper"];
                    string FK_MapData = dr["FK_MapData"];
                    if (FK_MapData.ToLower().Contains("dtl"))
                    {
                        BindDtlLogic(dr);
                    }
                    else
                    {
                        BindMainFormLogic(dr);
                    }
                }
            }
        }
        void BindMainFormLogic(DataRow dr)
        {
            string ExtType = dr["ExtType"];
            string AttrOfOper = dr["AttrOfOper"];
            switch (ExtType)
            {
                case "ActiveDDL":
                    ComboBox cb = this.FindName(AttrOfOper) as ComboBox;
                    cb.Tag = dr;
                    cb.SelectionChanged += new SelectionChangedEventHandler(cb_SelectionChanged);
                    break;
                case "DDLFullCtrl":
                    ComboBox cbFull = this.FindName(AttrOfOper) as ComboBox;
                    cbFull.Tag = dr;
                    cbFull.SelectionChanged += new SelectionChangedEventHandler(cbFull_SelectionChanged);
                    break;
                case "TBFullCtrl":
                    TextBox tb = this.FindName(AttrOfOper) as TextBox;
                    //if (tb != null)
                    //{
                    tb.Tag = new object[] { null, dr };
                    tb.KeyUp += new KeyEventHandler(tb_KeyDown);
                    //}
                    //else
                    //{
                    //    BindDtlLog(dr);
                    //}
                    break;
                case "PopVal":
                    TextBox tbpop = this.FindName(AttrOfOper) as TextBox;
                    tbpop.Background = new SolidColorBrush(Colors.White);
                    tbpop.Tag = dr;
                    tbpop.MouseLeftButtonDown += new MouseButtonEventHandler(tbpop_MouseLeftButtonDown);
                    break;
                case "AutoFull":
                    string str = dr["Doc"];
                    string str1 = str.Replace("@", string.Empty);
                    BindAutoFullLogic(str1, dr["AttrOfOper"]);
                    break;
                case "RegularExpression":// Regex 
                    string strName = dr["AttrOfOper"];
                    string strReg = dr["Doc"];
                    string validateType = dr["Tag"];
                    string errorMessage = dr["Tag1"];
                    TextBox textbox = this.canvasMain.FindName(strName) as TextBox;
                    validate.AddValidateControl(textbox, validateType, strReg, errorMessage);
                    break;
            }
        }
        BPValidate validate = new BPValidate();
        string[] Symbles = new string[] { "+", "-", "*", "/" };
        void BindAutoFullLogic(string strValue, string AttrOfOper)
        {
            string strSymble = string.Empty;
            foreach (string item in Symbles)
            {
                if (strValue.Contains(item))
                {
                    strSymble = item;
                    break;
                }
            }
            if (strSymble != string.Empty)
            {
                string[] strs = strValue.Split(strSymble.ToCharArray());
                List<BPTextBox> tbs = new List<BPTextBox>();
                BPTextBox textbox = this.canvasMain.FindName(AttrOfOper) as BPTextBox;
                textbox.Tag = strSymble;
                tbs.Add(textbox);
                foreach (string str in strs)
                {
                    foreach (var item in this.canvasMain.Children)
                    {
                        if (item is BPTextBox)
                        {
                            BPTextBox tb = item as BPTextBox;
                            if (tb.Name.ToLower() == str.ToLower() || tb.NameOfReal.ToLower() == str.ToLower())
                            {
                                tbs.Add(item as BPTextBox);
                                break;
                            }
                        }
                    }
                }
                for (int i = 1; i < tbs.Count; i++)
                {
                    tbs[i].KeyUp += new KeyEventHandler(fe_KeyUp);
                    tbs[i].Tag = tbs;
                }
            }
        }

        void fe_KeyUp(object sender, KeyEventArgs e)
        {
            double sum = 1;
            string strSymble = string.Empty;
            List<BPTextBox> fes = (sender as BPTextBox).Tag as List<BPTextBox>;
            switch (strSymble)
            {
                case "*":
                    sum = 1;
                    break;
                case "-":
                    sum = 0;
                    break;
                case "+":
                    sum = 0;
                    break;
                case "/":
                    sum = 0;
                    break;
            }
            if (fes != null)
            {
                strSymble = fes[0].Tag as string;
                for (int i = 1; i < fes.Count; i++)
                {
                    switch (strSymble)
                    {
                        case "*":
                            sum = sum * Convert.ToDouble((fes[i] as TextBox).Text);
                            break;
                        case "-":
                            sum = sum - Convert.ToDouble((fes[i] as TextBox).Text);
                            break;
                        case "+":
                            sum = sum + Convert.ToDouble((fes[i] as TextBox).Text);
                            break;
                        case "/":
                            sum = sum / Convert.ToDouble((fes[i] as TextBox).Text);
                            break;
                    }

                }
            }
            (fes[0] as TextBox).Text = sum.ToString();
        }

        DataRow _DataRow;
        void cbFull_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //SELECT TEL AS DianHua , fk_dept as MyDept from WF_Emp where No = ~@Key~
            ComboBox comboBox = sender as ComboBox;
            if (comboBox.SelectedIndex >= 0)
            {
                OPType = 2;
                _DataRow = comboBox.Tag as DataRow;
                string sql = _DataRow["Doc"];
                sql = sql.Replace("~@Key~", "'" + (comboBox.SelectedItem as ComboBoxItem).Tag.ToString() + "'");
                da.GetNoNameAsync(sql);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        void BindDtlLogic(DataRow dr)
        {
            //var dtls = this.canvasMain.Children.Where(p => p.GetType().Name == "BPDtl");// All bpdtl
            //var dtl = dtls.Where(p => (p as FrameworkElement).Name == dr["Fk_MapData"]).FirstOrDefault();// Find name 
            var dtl = this.canvasMain.Children.Where(p => (p as FrameworkElement).Name == dr["Fk_MapData"]).FirstOrDefault();// Find by name BPDtl
            BPDtl bpdtl = dtl as BPDtl;
            DataGrid dg = bpdtl.Content as DataGrid;
            DataTable dt = FrmDS.Tables["Sys_MapExt"];
            foreach (DataRow dataRow in dt.Rows)
            {
                if (dataRow["FK_MapData"] != bpdtl.Name)
                {
                    continue;
                }
                BPDtlLogic logic = new BPDtlLogic();
                logic.AttrOfOperName = dataRow["AttrOfOper"];
                if (!string.IsNullOrEmpty(dataRow["AttrsOfActive"]))
                {
                    logic.AttrsOfActiveNames.Add(dataRow["AttrsOfActive"]);
                }
                logic.ExtType = dataRow["ExtType"];
                logic.Doc = dataRow["Doc"];
                BPDtlHelper.AddLogic(dr["FK_MapData"], bpdtl.Name, logic);
            }
            // dg.Tag = logic;
            dg.CurrentCellChanged += (sender2, e2) =>
            {
                BPDtlHelper.dg_CurrentCellChanged(dr["FK_MapData"], bpdtl.Name, sender2 as DataGrid);
            };
            //string pName = dr["FK_MapData"];
            //string ctlName = dr["AttrOfOper"];
            ////SELECT No,Name , Tel as DianHua, FK_Dept from wf_emp where no like ~@Key%~
            //BPDtl dtl = this.FindName(pName) as BPDtl;
            //DataGrid dg = dtl.Content as DataGrid;
            //foreach (var item in dg.Columns)
            //{
            //}
        }
        void dg_CurrentCellChanged(object sender, EventArgs e)
        {
            // BPDtlLogic logic =(sender as DataGrid ).Tag as BPDtlLogic ;
            //BPDtlHelper.dg_CurrentCellChanged (


            //DataGrid MyDG = sender as DataGrid;
            //DataRow dr = MyDG.Tag as DataRow;
            //if (MyDG.SelectedItem != null)
            //{
            //    FrameworkElement fe = MyDG.CurrentColumn.GetCellContent(MyDG.SelectedItem);
            //    if (fe is BPTextBox && fe.Name == dr["AttrOfOper"])
            //    {
            //        fe.Tag = new object[] { MyDG, dr };
            //        fe.KeyDown -= new KeyEventHandler(ctl_KeyDown);
            //        fe.KeyDown += new KeyEventHandler(ctl_KeyDown);
            //    }
            //    else if (fe is BPDDL) //&& dr["ExtType"] == "ActiveDDL")
            //    {
            //        var ctl = fe as BPDDL;
            //        ctl.Tag = new object[] { MyDG, dr };
            //        cbActive = GetDtlAttrsOfActive(ctl) as ComboBox;
            //        ctl.SelectionChanged -= new SelectionChangedEventHandler(ctl_SelectionChanged);
            //        ctl.SelectionChanged += new SelectionChangedEventHandler(ctl_SelectionChanged);
            //    }
            //    else if (fe is BPTextBox)//bptextbox  Calculate the amount and automatically populate 
            //    {
            //        if(dr["ExtType"]=="AutoFull")
            //        {

            //            // Find Price * Quantity = Together these three controls 
            //            foreach (var item in MyDG.Columns )
            //            {
            //                FrameworkElement fe2 = item.GetCellContent(MyDG.SelectedItem);
            //            }
            //        }

            //    }

            //}
        }

        void ctl_KeyDown(object sender, KeyEventArgs e)
        {
            tb = sender as TextBox;
            DataRow dr = (tb.Tag as object[])[1] as DataRow;
            string Doc = dr["Doc"];
            string ExtType = dr["ExtType"];
            string AttrOfOper = dr["AttrOfOper"];
            string AttrOfActive = dr["AttrsOfActive"];
            string TableName = GetTableName(Doc);
            Doc = Doc.Replace("~@Key%~", "'" + tb.Text + "%'");
            da.GetNoNameAsync(Doc);
        }

        void ctl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OPType = 1;
            var ctl = sender as BPDDL;
            DataRow dr = (ctl.Tag as object[])[1] as DataRow;
            string Doc = dr["Doc"];
            string ExtType = dr["ExtType"];
            string AttrOfOper = dr["AttrOfOper"];
            string AttrOfActive = dr["AttrsOfActive"];
            string TableName = GetTableName(Doc);
            if (ctl.SelectedItem == null)
            {
                return;
            }
            string Key = (ctl.SelectedItem as ComboBoxItem).Tag.ToString();
            Doc = Doc.Replace("~@Key~", "'" + Key + "'");
            da.GetNoNameAsync(Doc);
        }

        void ctl_TextChanged(object sender, TextChangedEventArgs e)
        {


            return;

            //DataGrid dg = ((sender as TextBox).Tag as object[])[0] as DataGrid;
            //DataRow dr = ((sender as TextBox).Tag as object[])[1] as DataRow;
            //DataGridCell dgc = (DataGridCell)((sender as FrameworkElement).Parent);
            //foreach (var item in dg.Columns)
            //{
            //    FrameworkElement fe = item.GetCellContent(DataGridRow.GetRowContainingElement(dgc));
            //    if (fe.Name == dr["AttrOfOper"])
            //    {
            //        //fe.Tag = dr;
            //        //TextBox tb = fe as TextBox;
            //        //tb.KeyDown += new KeyEventHandler(tb_KeyDown);

            //        //tb.KeyDown +=new KeyEventHandler(tb_KeyDown);
            //        //string sql = dr["Doc"];
            //        //// SELECT No,Name , Tel as DianHua, FK_Dept from wf_emp where no like ~@Key%~

            //    }
            //}
        }

        object GetDtlAttrsOfActive(object sender)
        {
            DataGrid dg = ((sender as FrameworkElement).Tag as object[])[0] as DataGrid;
            DataRow dr = ((sender as FrameworkElement).Tag as object[])[1] as DataRow;
            DataGridCell dgc = (DataGridCell)((sender as FrameworkElement).Parent);
            foreach (var item in dg.Columns)
            {
                FrameworkElement fe = item.GetCellContent(DataGridRow.GetRowContainingElement(dgc));
                if (fe.Name == dr["AttrsOfActive"])
                {
                    return fe;
                }
            }
            return null;
        }
        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tbpop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //@PopValSelectModel=1
            //@PopValFormat=1
            //@PopValWorkModel=1
            TextBox tb = sender as TextBox;
            DataRow dr = tb.Tag as DataRow;
            string Doc = dr["Doc"];
            string Tag1 = dr["Tag1"];
            string Tag2 = dr["Tag2"];
            ChildPop pop = ChildPop.CreateInstance(Tag1, Tag2);
            string strpara = dr["AtPara"];
            string[] paras = strpara.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string para in paras)
            {
                //@PopValSelectModel=1@PopValFormat=1@PopValWorkModel=1
                if (para.StartsWith("PopValSelectModel"))
                {
                    pop.SelectionMode = Convert.ToInt32(para.Substring(18, para.Length - 18));
                }
                else if (para.StartsWith("PopValFormat"))
                {
                    pop.PopValFormat = Convert.ToInt32(para.Substring(13, para.Length - 13));
                }
            }
            ChildPop.ReturnValue = tb.Text;
            pop.Closed += (sender2, e2) =>
            {
                tb.Text = ChildPop.ReturnValue;
            };
            pop.Show();
        }
        TextBox tb = new TextBox();
        Popup popup = new Popup();
        DataSet ds = new DataSet();
        ListBox lb = new ListBox();
        void tb_KeyDown(object sender, KeyEventArgs e)
        {
            tb = sender as TextBox;
            DataRow dr = (tb.Tag as object[])[1] as DataRow;
            string Doc = dr["Doc"];
            string ExtType = dr["ExtType"];
            string AttrOfOper = dr["AttrOfOper"];
            string AttrOfActive = dr["AttrsOfActive"];
            string TableName = GetTableName(Doc);
            Doc = Doc.Replace("~@Key%~", "'" + tb.Text + "%'");
            da.GetNoNameAsync(Doc);
        }
        DataTable dtResult = new DataTable();
        void da_GetNoNameCompleted(object sender, GetNoNameCompletedEventArgs e)
        {
            HandleMainFormDDL(e);
        }
        void HandleMainFormDDL(GetNoNameCompletedEventArgs e)
        {
            if (OPType == 1)// Cascading drop-down 
            {
                DataSet dsResult = new DataSet();
                dsResult.FromXml(e.Result);
                dtResult = dsResult.Tables[0];
                cbActive.Items.Clear();
                foreach (DataRow dr in dtResult.Rows)
                {
                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = dr[1];
                    item.Tag = dr[0];
                    cbActive.Items.Add(item);
                }
            }
            else if (OPType == 2)//ddlfull
            {
                DataSet dsResult = new DataSet();
                dsResult.FromXml(e.Result);
                dtResult = dsResult.Tables[0];
                foreach (var dc in dtResult.Columns)
                {
                    foreach (var item in this.canvasMain.Children)
                    {
                        var fe = item as FrameworkElement;
                        if (fe.Name == dc.ColumnName)
                        {
                            if (fe is TextBox)
                            {
                                (fe as TextBox).Text = dtResult.Rows[0][fe.Name];
                            }
                            else if (fe is BPDDL)
                            {
                                (fe as BPDDL).SetSelectVal(dtResult.Rows[0][fe.Name]);
                            }
                        }
                    }
                }
            }
            else if (OPType == 3)// From the drop-down list 
            {

            }
            else// Automatic filling 
            {
                lb.Items.Clear();
                DataSet dsResult = new DataSet();
                dsResult.FromXml(e.Result);
                dtResult = dsResult.Tables[0];
                var drs = dtResult.Rows.Where(p => p["No"].Contains(tb.Text));
                foreach (DataRow dr in drs)
                {
                    ListBoxItem li = new ListBoxItem();
                    li.Tag = dr;
                    li.Content = dr["No"] + "," + dr["Name"];
                    lb.Items.Add(li);
                }
                lb.Tag = dtResult;
                var gt = tb.TransformToVisual(LayoutRoot);
                Point point = gt.Transform(new Point(0, 0));
                popup.Child = lb;
                popup.SetValue(Canvas.LeftProperty, point.X);
                popup.SetValue(Canvas.TopProperty, point.Y + tb.Height);
                popup.IsOpen = true;
            }
        }
        ComboBox cbActive;
        /// <summary>
        ///  Main table   Cascading drop-down 1, Area , Provinces dropdown 
        /// </summary>
        int OPType = 0;
        void cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedIndex < 0) return;
            ComboBoxItem item = cb.SelectedItem as ComboBoxItem;
            string SF = item.Tag.ToString();
            DataRow dr = cb.Tag as DataRow;
            string Doc = dr["Doc"];
            string ExtType = dr["ExtType"];
            string AttrOfOper = dr["AttrOfOper"];
            string AttrOfActive = dr["AttrsOfActive"];
            string TableName = GetTableName(Doc);
            cbActive = this.FindName(AttrOfActive) as ComboBox;
            //SELECT No, Name FROM CN_City WHERE FK_SF = ~@Key~
            Doc = Doc.Replace("~@Key~", "'" + SF + "'");
            //cbActive.Items.Clear();
            //DataTable dt = this.FrmDS.Tables[TableName];
            //foreach (DataRow datarow in dt.Rows)
            //{
            //    if (datarow[0].ToString().StartsWith(SF))
            //    {
            //        ComboBoxItem subItem = new ComboBoxItem();
            //        subItem.Tag = datarow[0];
            //        subItem.Content = datarow[1];
            //        subItem.MouseLeftButtonDown += new MouseButtonEventHandler(subItem_MouseLeftButtonDown);
            //        cbActive.Items.Add(subItem);
            //    }
            //}

            OPType = 1;
            da.GetNoNameAsync(Doc);
        }

        void subItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
        string GetTableName(string Doc)
        {
            string[] strs = Doc.Split(' ');
            for (int i = 0; i < strs.Length; i++)
            {
                if (strs[i].ToLower() == "from")
                {
                    return strs[i + 1];
                }
            }
            return string.Empty;
        }

        /// <summary>
        ///  Generate toolbar
        /// </summary>
        public void InitToolbar()
        {
        //    DataTable dt = this.FrmDS.Tables["WF_BtnLab"];

            DataTable dt = this.FrmDS.Tables["WF_Node"];

            #region  Generate toolbar .
            /*  Send  */
            List<Func> ens = new List<Func>();

            Func enAdd = new Func();
            enAdd.No = BtnAttr.SendLab;
            enAdd.Name = dt.Rows[0][BtnAttr.SendLab];
            ens.Add(enAdd);

            // To test the use of .
            int i = 0;

            /* Save */
            if (dt.Rows[0][BtnAttr.SaveEnable].ToString() != "0")
            {
                enAdd = new Func();
                enAdd.No = BtnAttr.SaveLab;
                enAdd.Name = dt.Rows[0][BtnAttr.SaveLab];
                ens.Add(enAdd);
            }

            /* Return */
            if (dt.Rows[0][BtnAttr.ReturnRole].ToString() != "0" || i == 0)
            {
                enAdd = new Func();
                enAdd.No = BtnAttr.ReturnLab;
                enAdd.Name = dt.Rows[0][BtnAttr.ReturnLab];
                ens.Add(enAdd);
            }

            /* Jump */
            if (dt.Rows[0][NodeAttr.JumpWay].ToString() != "0" || i == 0)
            {
                enAdd = new Func();
                enAdd.No = BtnAttr.JumpWayLab;
                enAdd.Name = dt.Rows[0][BtnAttr.JumpWayLab];
                ens.Add(enAdd);
            }

            /* Cc */
            if (dt.Rows[0][NodeAttr.CCRole].ToString() != "0" || i == 0)
            {
                enAdd = new Func();
                enAdd.No = BtnAttr.CCLab;
                enAdd.Name = dt.Rows[0][BtnAttr.CCLab];
                ens.Add(enAdd);
            }

            /* Pending */
            if (dt.Rows[0][BtnAttr.HungEnable].ToString() != "0" || i == 0)
            {
                enAdd = new Func();
                enAdd.No = BtnAttr.HungLab;
                enAdd.Name = dt.Rows[0][BtnAttr.HungLab];
                ens.Add(enAdd);
            }

            /* Transfer */
            if (dt.Rows[0][BtnAttr.ShiftEnable].ToString() != "0" || i == 0)
            {
                enAdd = new Func();
                enAdd.No = BtnAttr.ShiftLab;
                enAdd.Name = dt.Rows[0][BtnAttr.ShiftLab];
                ens.Add(enAdd);
            }

            /* Delete */
            if (dt.Rows[0][BtnAttr.DelEnable].ToString() != "0" || i == 0)
            {
                enAdd = new Func();
                enAdd.No = BtnAttr.DelLab;
                enAdd.Name = dt.Rows[0][BtnAttr.DelLab];
                ens.Add(enAdd);
            }

            /* End */
            if (dt.Rows[0][BtnAttr.EndFlowEnable].ToString() != "0" || i == 0)
            {
                enAdd = new Func();
                enAdd.No = BtnAttr.EndFlowLab;
                enAdd.Name = dt.Rows[0][BtnAttr.EndFlowLab];
                ens.Add(enAdd);
            }

            /* Printing documents */
            if (dt.Rows[0][BtnAttr.PrintDocEnable].ToString() != "0" || i == 0)
            {
                enAdd = new Func();
                enAdd.No = BtnAttr.PrintDocLab;
                enAdd.Name = dt.Rows[0][BtnAttr.PrintDocLab];
                ens.Add(enAdd);
            }

            /* Locus */
            if (dt.Rows[0][BtnAttr.TrackEnable].ToString() != "0" || i == 0)
            {
                enAdd = new Func();
                enAdd.No = BtnAttr.TrackLab;
                enAdd.Name = dt.Rows[0][BtnAttr.TrackLab];
                ens.Add(enAdd);
            }

            /* Recipient */
            if (dt.Rows[0][BtnAttr.SelectAccepterEnable].ToString() != "0" || i == 0)
            {
                enAdd = new Func();
                enAdd.No = BtnAttr.SelectAccepterLab;
                enAdd.Name = dt.Rows[0][BtnAttr.SelectAccepterLab];
                ens.Add(enAdd);
            }

            /* Options */
            //if (dt.Rows[0][BtnAttr.OptEnable].ToString() != "0" || i == 0)
            //{
            //    enAdd = new Func();
            //    enAdd.No = BtnAttr.OptLab;
            //    enAdd.Name = dt.Rows[0][BtnAttr.OptLab];
            //    ens.Add(enAdd);
            //}

            /* Inquiry */
            if (dt.Rows[0][BtnAttr.SearchEnable].ToString() != "0" || i == 0)
            {
                enAdd = new Func();
                enAdd.No = BtnAttr.SearchLab;
                enAdd.Name = dt.Rows[0][BtnAttr.SearchLab];
                ens.Add(enAdd);
            }


            // The button to tool bar .
            foreach (Func en in ens)
            {
                Toolbar.ToolbarButton btn = new Toolbar.ToolbarButton();
                btn.Name = "Btn_" + en.No;
                btn.Click += new RoutedEventHandler(ToolBar_Click);

                StackPanel mysp = new StackPanel();
                mysp.Orientation = Orientation.Horizontal;
                mysp.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                mysp.Name = "sp" + en.No;

                Image img = new Image();
                BitmapImage png = new BitmapImage(new Uri("/WorkNode;component/Img/" + Func.Save + ".png", UriKind.Relative));
                img.Source = png;
                img.Width = 13;
                img.Height = 13;
                mysp.Children.Add(img);

                TextBlock tb = new TextBlock();
                tb.Name = "tbT" + en.No;
                tb.Text = en.Name + " ";
                tb.FontSize = 13;
                mysp.Children.Add(tb);
                btn.Content = mysp;
                this.toolbar1.AddBtn(btn);
            }
            #endregion
        }
        void daAppCenter_CfgKeyCompleted(object sender, FF.CfgKeyCompletedEventArgs e)
        {
            Glo.AppCenterDBType = e.Result;
        }
        public DataSet FrmDS = null;
        private DataTable dtND = null;
        private DataTable dtMapAttrs = null;
        public string GetValByKey(string key)
        {
            if (dtND == null)
                dtND = this.FrmDS.Tables[Glo.FK_MapData];
            try
            {
                return dtND.Rows[0][key].ToString();
            }
            catch
            {
                //if ( dtND.Rows[0][key]==null)
                return "";
            }
        }
        /// <summary>
        ///  Implementation of the lock screen 
        /// </summary>
        public void DoLockPage()
        {
            this.toolbar1.IsEnabled = false;
            foreach (UIElement ele in this.canvasMain.Children)
            {
                BPTextBox tb = ele as BPTextBox;
                if (tb != null)
                    tb.IsEnabled = false;

                BPDDL ddl = ele as BPDDL;
                if (ddl != null)
                    ddl.IsEnabled = false;

                BPDtl dtl = ele as BPDtl;
                if (dtl != null)
                    dtl.IsEnabled = false;

                CheckBox cb = ele as CheckBox;
                if (cb != null)
                    cb.IsEnabled = false;

                RadioButton rb = ele as RadioButton;
                if (rb != null)
                    rb.IsEnabled = false;
            }
        }
        /// <summary>
        ///  Event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolBar_Click(object sender, RoutedEventArgs e)
        {
            #region  Get ID
            string id = "";
            Button btn = sender as Button;
            if (btn == null)
            {
                Toolbar.ToolbarButton mybtn = sender as Toolbar.ToolbarButton;
                id = mybtn.Name;
            }
            else
            {
                id = btn.Name;
            }
            #endregion  Get id

            id = id.Replace("Btn_", "");
            switch (id)
            {
                case BP.WF.BtnAttr.SendLab: //  Send .
                    this.loadingWindow.Title = " Saving form data and performs transmission ...";
                    this.loadingWindow.Show();
                    FF.CCFlowAPISoapClient sendWorkNode = Glo.GetCCFlowAPISoapClientServiceInstance();

                    string xmlSend =this.GenerFrmDataSet().ToXml(true, false);

                    sendWorkNode.Node_SendWorkAsync(Glo.FK_Flow, Glo.FK_Node, Glo.WorkID,xmlSend, BP.Port.WebUser.No);
                    sendWorkNode.Node_SendWorkCompleted += new EventHandler<FF.Node_SendWorkCompletedEventArgs>(SendWorkNode_Node_SendWorkCompleted);
                    break;
                case BP.WF.BtnAttr.SaveLab: //  Save .
                    this.loadingWindow.Title = " Saving form data ...";
                    this.loadingWindow.Show();
                    FF.CCFlowAPISoapClient saveWorkNode = Glo.GetCCFlowAPISoapClientServiceInstance();
                   // string xml = this.GenerFrmDataSet().ToXml(true, false);
                 //   string xm1l = this.GenerFrmDataSet().ToXml(false, true);

                    string xml = this.GenerFrmDataSet().ToXml(true, true);
                    saveWorkNode.Node_SaveWorkAsync(Glo.FK_Flow, Glo.FK_Node, Glo.WorkID, BP.Port.WebUser.No, xml);
                    saveWorkNode.Node_SaveWorkCompleted += new EventHandler<FF.Node_SaveWorkCompletedEventArgs>(SaveWorkNode_Node_SaveWorkCompleted);
                    return;
                case BP.WF.BtnAttr.ReturnLab: // Return .
                    ReturnWork rw = new ReturnWork();
                    rw.Closed += new EventHandler(PopupWindow_Closed);
                    rw.Show();
                    return;
                case BP.WF.BtnAttr.CCLab: // Cc .
                    Glo.OpenWindowOrDialog("/WF/WorkOpt/CC.aspx?FK_Flow=" + Glo.FK_Flow + "&NodeID=" + Glo.FK_Node + "&WorkID=" + Glo.WorkID,
                        " Cc ", 500, 800, BP.WindowModelEnum.Dialog);
                    return;
                case BP.WF.BtnAttr.PrintDocLab: // Printing documents .
                    string urlr = "/WF/WorkOpt/PrintDoc.aspx?FK_Node=" + Glo.FK_Node + "&FID=" + Glo.FID + "&WorkID=" + Glo.WorkID + "&FK_Flow=" + Glo.FK_Flow;
                    Glo.OpenWindowOrDialog(urlr,
                        " Printing documents ", 400, 700, BP.WindowModelEnum.Dialog);
                    return;
                case BP.WF.BtnAttr.SelectAccepterLab: // Select recipient .
                    Glo.OpenWindowOrDialog("/WF/Accepter.aspx?FK_Flow=" + Glo.FK_Flow + "&FK_Node=" + Glo.FK_Node + "&WorkID=" + Glo.WorkID,
                        " Select recipient ", 500, 600, BP.WindowModelEnum.Dialog);
                    return;
                case BP.WF.BtnAttr.ShiftLab: // Transfer .
                    ShiftWork sw = new ShiftWork();
                    sw.Closed += new EventHandler(PopupWindow_Closed);
                    sw.Show();
                    return;
                case BP.WF.BtnAttr.HungLab: // Pending .
                    Glo.OpenWindowOrDialog("/WF/WorkOpt/HungUp.aspx?FK_Flow=" + Glo.FK_Flow + "&FK_Node=" + Glo.FK_Node + "&WorkID=" + Glo.WorkID,
                      " Pending ", 300, 600, BP.WindowModelEnum.Dialog);
                    return;
                case BP.WF.BtnAttr.TrackLab: // Locus .
                    Glo.OpenWindowOrDialog("/WF/WorkOpt/OneWork/Track.aspx?FK_Flow=" + Glo.FK_Flow + "&FK_Node=" + Glo.FK_Node + "&WorkID=" + Glo.WorkID,
                     " Locus ", 500, 800, BP.WindowModelEnum.Dialog);
                    return;
                case BP.WF.BtnAttr.JumpWayLab: // Jump .
                    Jump j = new Jump();
                    j.Closed += new EventHandler(PopupWindow_Closed);
                    j.Show();
                    return;
                case BP.WF.BtnAttr.SearchLab: // Inquiry .
                    Search s = new Search();
                    s.Closed += new EventHandler(PopupWindow_Closed);

                    s.Show();
                    return;
                case BP.WF.BtnAttr.EndFlowLab: // End Process .
                    WorkNode.WorkRefFunc.EndWorkFlow end = new WorkRefFunc.EndWorkFlow();
                    end.Closed += new EventHandler(PopupWindow_Closed);
                    end.Show();
                    return;
                case BP.WF.BtnAttr.DelLab: // Delete Process .
                    WorkNode.WorkRefFunc.DelWorkFlow del = new WorkRefFunc.DelWorkFlow();
                    del.Closed += new EventHandler(PopupWindow_Closed);
                    del.Show();
                    return;
                default:
                    MessageBox.Show(sender.ToString() + " ID=" + id + "  Function not implemented .");
                    break;
            }
        }
        private void PopupWindow_Closed(object sender, EventArgs e)
        {
            ChildWindow cw = sender as ChildWindow;
            if ((bool)cw.DialogResult == true)
            {
                /* If you need to lock the screen .*/
                this.DoLockPage();
            }
        }
        /// <summary>
        ///  Access to data pages 
        /// </summary>
        /// <returns></returns>
        public DataSet GenerFrmDataSet()
        {
            // Master table data .
            DataTable dtMain = new DataTable();
            dtMain.TableName = Glo.FK_MapData;
            dtMain.Columns.Add(new DataColumn("KeyOfEn", typeof(string)));
            dtMain.Columns.Add(new DataColumn("Val", typeof(string)));
            
            #region  Gets the value of the main table .
            foreach (DataRow dr in dtMapAttrs.Rows)
            {
                if (dr["UIVisible"] == "0")
                    continue;

                if (dr["FK_MapData"] != Glo.FK_MapData)
                    continue;
                DataRow drNew = dtMain.NewRow();

                string keyOfEn = dr["KeyOfEn"];

                UIElement ctl = this.canvasMain.FindName(keyOfEn) as UIElement;
                BPTextBox tb = ctl as BPTextBox;
                if (tb != null)
                {
                    drNew[0] = keyOfEn;
                    drNew[1] = tb.Text;
                    dtMain.Rows.Add(drNew);
                    continue;
                }

                BPDDL ddl = ctl as BPDDL;
                if (ddl != null)
                {
                    ComboBoxItem li = ddl.SelectedItem as ComboBoxItem;
                    if (li == null)
                        continue;
                    drNew[0] = keyOfEn;
                    drNew[1] = li.Tag.ToString();
                    dtMain.Rows.Add(drNew);
                    continue;
                }

                BPCheckBox cb = ctl as BPCheckBox;
                if (cb != null)
                {
                    drNew[0] = keyOfEn;
                    if (cb.IsChecked == true)
                        drNew[1] = "1";
                    else
                        drNew[1] = "0";
                    dtMain.Rows.Add(drNew);
                    continue;
                }
            }
            #endregion  Gets the value of the main table .


            #region  Get values from a table inside .
            DataSet dsNodeData = new DataSet();
            dsNodeData.Tables.Add(dtMain);

            // Here are just a deal to save the main table , Multi-Table ,多m2m, Accessory , Picture . No deal .
            //从DataSet All queries from the table 
            var BPDtls = this.canvasMain.Children.Where(p => p is BPDtl);
            foreach (var item in BPDtls)
            {
                BPDtl dtl = item as BPDtl;
                DataTable dt = dtl.GetDataTable();
                dsNodeData.Tables.Add(dt);
            }
            #endregion  Get values from a table inside .

            return dsNodeData;
        }
        void SendWorkNode_Node_SendWorkCompleted(object sender, FF.Node_SendWorkCompletedEventArgs e)
        {
            /*  After successfully sent 
            * 1,  Tip Send message 
            * 2,
            */
            this.loadingWindow.DialogResult = true;
            WorkNode.WorkRefFunc.SendObjsAlert alert = new WorkRefFunc.SendObjsAlert();
            alert.Closed += new EventHandler(PopupWindow_Closed);
            alert.BindIt(e.Result);
            alert.Show();
        }
        /// <summary>
        ///  Save results returned .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SaveWorkNode_Node_SaveWorkCompleted(object sender, FF.Node_SaveWorkCompletedEventArgs e)
        {
            /* After successfully saved 
           * 1,  May return an exception 
           * 2,
           */
            this.loadingWindow.DialogResult = true;
        }
    }
}
