using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Silverlight;
using System.Windows.Input;
using System;
using System.Windows.Controls.Primitives;

namespace WorkNode
{
    public class BPDtlHelper
    {
        static FF.CCFlowAPISoapClient webService;
        static List<BPDtlMeta> DtlLogic = new List<BPDtlMeta>();
        static LogicType LogicType = LogicType.None;
        static BPDtlHelper()
        {
            webService = Glo.GetCCFlowAPISoapClientServiceInstance();
            webService.GetNoNameCompleted += webService_GetNoNameCompleted;
            lb.SelectionChanged += new SelectionChangedEventHandler(lb_SelectionChanged);
        }


        /// <summary>
        ///  Add business logic 
        /// </summary>
        /// <param name="FK_MapData"></param>
        /// <param name="DtlName"></param>
        /// <param name="logic"></param>
        public static void AddLogic(string FK_MapData, string DtlName, BPDtlLogic logic)
        {
            BPDtlMeta dtl = DtlLogic.Where(p => p.FK_MapData == FK_MapData && p.DtlName == DtlName).FirstOrDefault();
            if (dtl == null)
            {
                dtl = new BPDtlMeta() { FK_MapData = FK_MapData, DtlName = DtlName };
                dtl.Logics.Add(logic);
                DtlLogic.Add(dtl);
            }
            else
            {
                if (dtl.Logics.Contains(logic) == false)
                {
                    dtl.Logics.Add(logic);
                }
            }
        }
        static DataGrid MyDataGrid;
        /////////////////////////////////////////
        public static void dg_CurrentCellChanged(string FK_MapData, string DtlName, DataGrid MyDG)
        {
            MyDataGrid = MyDG;
            BPDtlLogic dtlLogic = MyDG.Tag as BPDtlLogic;
            if (MyDG.SelectedItem != null)
            {
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
                //        if (dr["ExtType"] == "AutoFull")
                //        {

                //            // Find Price * Quantity = Together these three controls 
                //            foreach (var item in MyDG.Columns)
                //            {
                //                FrameworkElement fe2 = item.GetCellContent(MyDG.SelectedItem);
                //            }
                //        }
                //        var bpTb = fe as BPTextBox;
                //        bpTb.Tag = new object[] { MyDG, dr };
                //        bpTb.KeyUp -= (bpTb_KeyUp);
                //        bpTb.KeyUp += (bpTb_KeyUp);
                //    }
                // Find this one dtl All logical 
                var dtl = DtlLogic.Where(p => p.FK_MapData == FK_MapData && p.DtlName == DtlName).FirstOrDefault();
                // Find DataGrid Control events on the trigger 
                var fe = MyDG.CurrentColumn.GetCellContent(MyDG.SelectedItem);

                //foreach (var logic in dtl.Logics)
                //{
                //    //if (logic.AttrsOfActive.Contains(fe))
                //    //{
                //    //    if (fe is BPTextBox)
                //    //    {
                //    //        fe.Tag = logic;
                //    //        fe.KeyDown -= fe_KeyDown;
                //    //        fe.KeyDown += fe_KeyDown;
                //    //    }
                //    //    else if (fe is BPDDL)
                //    //    {

                //    //    }
                //    //    else if (fe is BPTextBox)
                //    //    {

                //    //    }
                //    //}
                //}
                if (fe is BPTextBox)
                {
                    BPTextBox tb = fe as BPTextBox;
                    // If the name of the control in attrofopername There are , Then the control is attrofoper
                    var result = dtl.Logics.Where(
                        p => p.AttrOfOperName == tb.Name ||
                            p.AttrOfOperName == tb.NameOfReal ||
                            p.Doc.Contains(tb.NameOfReal) ||
                            p.Doc.Contains(tb.Name)
                            ).FirstOrDefault();
                    if (result != null)
                    {
                        int num = result.Logics.Count(p => p.AttrOfOper == tb);
                        if (num <= 0)// If this control is not cached 
                        {
                            //result.AttrOfOper = fe;
                            if (result.ExtType == "AutoFull")
                            {
                                if (result.AttrsOfActiveNames.Count() <= 0)// If you do not AttrsOfActive From DOC Get inside to fill controls 
                                {
                                    string doc = result.Doc;//@ Unit price *@ Quantity 
                                    BPLogic logic = new BPLogic();
                                    result.Logics.Add(logic);
                                    foreach (var c in MyDG.Columns)
                                    {
                                        var temp = c.GetCellContent(MyDG.SelectedItem) as BPTextBox;
                                        if (temp != null && (doc.Contains(temp.Name) || doc.Contains(temp.NameOfReal)))
                                        {
                                            temp.Tag = result;
                                            temp.TextChanged -= AutoFull_TextChanged;
                                            temp.TextChanged += AutoFull_TextChanged;
                                            logic.AttrsOfActive.Add(temp);
                                        }
                                        if (temp != null && (result.AttrOfOperName == temp.Name || result.AttrOfOperName == temp.NameOfReal))
                                        {
                                            logic.AttrOfOper = temp;
                                        }
                                    }
                                }
                            }

                            else if (result.ExtType == "TBFullCtrl")
                            {
                                int num2 = result.Logics.Count(p => p.AttrOfOper == tb);
                                if (num <= 0)
                                {
                                    //SELECT TOP 15 No, Name,Name as renyuan, Tel as DianHua,FK_Dept as MyDept FROM WF_Emp WHERE No LIKE ~@Key%~
                                    string doc = result.Doc;
                                    BPLogic logic = new BPLogic();
                                    result.Logics.Add(logic);
                                    foreach (var c in MyDG.Columns)
                                    {
                                        var temp = c.GetCellContent(MyDG.SelectedItem) as BPTextBox;
                                        if (temp != null && (result.AttrOfOperName == temp.Name || result.AttrOfOperName == temp.NameOfReal))
                                        {
                                            temp.Tag = result;
                                            logic.AttrOfOper = temp;
                                            temp.KeyUp -= TBFullCtrl_KeyUp;
                                            temp.KeyUp += TBFullCtrl_KeyUp;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (fe is BPDDL)   //(result.ExtType == "ActiveDDL")                  
                {
                    BPDDL ddl = fe as BPDDL;
                    // If the name of the control in attrofopername There are , Then the control is attrofoper
                    var result = dtl.Logics.Where(p => p.AttrOfOperName == ddl.Name).FirstOrDefault();
                    if (result != null)
                    {
                        int num = result.Logics.Count(p => p.AttrOfOper == tb);// If this control is not cached 
                        BPLogic logic = new BPLogic();
                        result.Logics.Add(logic);
                        if (num <= 0)
                        {
                            foreach (var c in MyDG.Columns)
                            {

                                var temp = c.GetCellContent(MyDG.SelectedItem) as BPDDL;
                                if (temp != null && (result.AttrOfOperName == temp.Name || result.AttrOfOperName == temp.NameOfReal))
                                {
                                    temp.Tag = result;
                                    //result.AttrOfOper = temp;
                                    logic.AttrOfOper = temp;
                                    temp.SelectionChanged -= temp_SelectionChanged;
                                    temp.SelectionChanged += temp_SelectionChanged;
                                }
                                else if (temp != null && result.AttrsOfActiveNames.Contains(temp.Name))
                                {
                                    // result.AttrsOfActive.Add(temp);
                                    logic.AttrsOfActive.Add(temp);
                                }
                            }
                        }
                    }
                }
                //if (fe is TextBox)
                //{
                //    fe.Tag = result;
                //    fe.KeyDown -= fe_KeyDown;
                //    fe.KeyDown += fe_KeyDown;
                //}
                //    if (result.AttrsOfActiveNames.Count() <= 0)
                //    {
                //        string doc = result.Doc;//@ Unit price *@ Quantity 
                //        foreach (var c in MyDG.Columns)
                //        {
                //            var temp = c.GetCellContent(MyDG.SelectedItem) as BPTextBox;
                //            if (temp != null && (doc.Contains(temp.Name) || doc.Contains(temp.NameOfReal)))
                //            {
                //                temp.Tag = result;
                //                temp.KeyDown -= fe_KeyDown;
                //                temp.KeyDown += fe_KeyDown;
                //                result.AttrsOfActive.Add(temp);
                //            }
                //        }
                //    }
                //}

            }

        }
        static BPDDL ActiveDDL;
        static void temp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LogicType = LogicType.ActiveDDL;
            BPDDL ddl1 = sender as BPDDL;
            BPDtlLogic logic = ddl1.Tag as BPDtlLogic;
            var fes = logic.Logics.Where(p => p.AttrOfOper == ddl1).Select(p => p.AttrsOfActive).FirstOrDefault();
            if (fes.Count() <= 0)
            {
                return;
            }
            ActiveDDL = fes[0] as BPDDL;
            if (ActiveDDL == null)
            {
                return;
            }
            if (ddl1.SelectedItem == null)
            {
                return;
            }
            string Key = (ddl1.SelectedItem as ComboBoxItem).Tag.ToString();
            string strSql = logic.Doc.Replace("~@Key~", "'" + Key + "'");
            webService.GetNoNameAsync(strSql);
        }

        static void TBFullCtrl_KeyUp(object sender, KeyEventArgs e)
        {
            LogicType = LogicType.TBFullCtrl;
            BPTextBox bpTB = sender as BPTextBox;
            tb = bpTB;
            BPDtlLogic logic = bpTB.Tag as BPDtlLogic;
            string sql = logic.Doc;
            //SELECT TOP 15 No, Name,Name as renyuan, Tel as DianHua,FK_Dept as MyDept FROM WF_Emp WHERE No LIKE ~@Key%~
            sql = sql.Replace("~@Key%~", "'" + bpTB.Text + "%'");
            webService.GetNoNameAsync(sql);
        }
        //A+b=c
        static void AutoFull_TextChanged(object sender, TextChangedEventArgs e)
        {
            LogicType = LogicType.AutoFull;
            //TextBox tb = sender as TextBox;
            //DataRow dr = (tb.Tag as object[])[1] as DataRow;
            //string Doc = dr["Doc"];
            //string ExtType = dr["ExtType"];
            //string AttrOfOper = dr["AttrOfOper"];
            //string AttrOfActive = dr["AttrsOfActive"];
            //string TableName = GetTableName(Doc);
            //Doc = Doc.Replace("~@Key%~", "'" + tb.Text + "%'");
            //da.GetNoNameAsync(Doc);

            TextBox tb = sender as TextBox;
            BPDtlLogic dtlLogic = tb.Tag as BPDtlLogic;
            BPLogic logic = dtlLogic.Logics.Where(p => p.AttrsOfActive.Contains(tb)).FirstOrDefault();
            int sum = 0;

            TextBox tbActive = logic.AttrOfOper as TextBox;
            foreach (var item in logic.AttrsOfActive)
            {
                TextBox tb2 = item as TextBox;

                if (tbActive.Text.Trim() != string.Empty && tb2.Text.Trim() != string.Empty)
                {
                    //tbActive.Text = (Convert.ToInt32(tbActive.Text) + Convert.ToInt32(tb2.Text)).ToString();
                    sum += Convert.ToInt32(tb2.Text);
                }
            }
            tbActive.Text = sum.ToString();
        }

        static TextBox tb = new TextBox();
        static ListBox lb = new ListBox();
        static DataTable dtResult = new DataTable();
        static Popup popup = new Popup();

        static void webService_GetNoNameCompleted(object sender, FF.GetNoNameCompletedEventArgs e)
        {
            if (LogicType == LogicType.TBFullCtrl)
            {
                lb.Items.Clear();
                DataSet dsResult = new DataSet();
                dsResult.FromXml(e.Result);
                dtResult = dsResult.Tables[0];
                BPDtlLogic logic = tb.Tag as BPDtlLogic;
                if (logic.AttrsOfActiveNames.Count == 0)
                {
                    logic.AttrsOfActiveNames.AddRange(dtResult.Columns.Select(p => p.ColumnName));
                }
                var drs = dtResult.Rows.Where(p => p["No"].Contains(tb.Text));
                foreach (DataRow dr in drs)
                {
                    ListBoxItem li = new ListBoxItem();
                    li.Tag = dr;
                    li.Content = dr["No"] + "," + dr["Name"];
                    lb.Items.Add(li);
                }
                lb.Tag = dtResult;
                var gt = tb.TransformToVisual(null);
                Point point = gt.Transform(new Point(0, 0));
                popup.Child = lb;
                popup.SetValue(Canvas.LeftProperty, point.X);
                popup.SetValue(Canvas.TopProperty, point.Y + tb.Height);
                popup.IsOpen = true;
            }
            else if (LogicType == LogicType.ActiveDDL)
            {
                DataSet dsResult = new DataSet();
                dsResult.FromXml(e.Result);
                dtResult = dsResult.Tables[0];
                ActiveDDL.Items.Clear();
                foreach (DataRow dr in dtResult.Rows)
                {
                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = dr[1];
                    item.Tag = dr[0];
                    ActiveDDL.Items.Add(item);
                }
            }
        }

        static void lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem li = lb.SelectedItem as ListBoxItem;
            if (li == null)
            {
                return;
            }
            //DataRow dr = li.Tag as DataRow;
            //tb.Text = dr["No"].ToString();
            /////////////////////////////////////////////////////////

            /////////////////////////////////////////////////////////

            //DataGrid dg = (tb.Tag as object[])[0] as DataGrid;
            //if (dg == null)
            //{
            //    DataTable dt = lb.Tag as DataTable;
            //    if (dt != null)
            //    {
            //        foreach (var item1 in this.canvasMain.Children)
            //        {
            //            foreach (var item2 in dt.Columns)
            //            {
            //                var fe = (item1 as FrameworkElement);
            //                if (fe.Name == item2.ColumnName)
            //                {
            //                    if (fe is TextBox)
            //                    {
            //                        (fe as TextBox).Text = dr[fe.Name];
            //                    }
            //                    else if (fe is BPDDL)
            //                    {
            //                        // (fe as BPDDL).SelectedIndex = Convert.ToInt32(dr[fe.Name]);
            //                        (fe as BPDDL).SetSelectVal(dr[fe.Name]);
            //                    }
            //                }
            //            }

            //        }
            //    }
            //    popup.IsOpen = false;
            //    return;
            //}

            //DataGridCell dgc = (DataGridCell)(tb.Parent);

            //SELECT No,Name , Tel as DianHua, FK_Dept from wf_emp where no like 'zhangyifan%'

            //foreach (var item in MyDataGrid.Columns)
            //{
            //    var fe = item.GetCellContent(DataGridRow.GetRowContainingElement(dgc));
            //    foreach (var dc in dtResult.Columns)
            //    {
            //        if (dc.ColumnName == fe.Name)
            //        {
            //            if (fe is TextBox)
            //            {
            //                (fe as TextBox).Text = dr[fe.Name];
            //            }
            //            else if (fe is BPDDL)
            //            {
            //                (fe as BPDDL).SelectedIndex = Convert.ToInt32(dr[fe.Name]);
            //            }
            //        }
            //    }

            //}
            DataGridCell dgc = (DataGridCell)(tb.Parent);
            BPDtlLogic logic = tb.Tag as BPDtlLogic;
            DataGridRow dgr = DataGridRow.GetRowContainingElement(dgc);
            DataRow dr = li.Tag as DataRow;
            foreach (DataGridColumn column in MyDataGrid.Columns)
            {
                var fe = column.GetCellContent(dgr);
                foreach (var name in logic.AttrsOfActiveNames)
                {

                    if (fe is BPTextBox)
                    {
                        BPTextBox bpTb = fe as BPTextBox;
                        if (bpTb.Name.ToLower() == name.ToLower() || bpTb.NameOfReal.ToLower() == name.ToLower())
                        {
                            (fe as TextBox).Text = dr[bpTb.NameOfReal];
                        }
                    }
                    else if (fe is BPDDL)
                    {
                        BPDDL bpDdl = fe as BPDDL;
                        if (bpDdl.Name.ToLower() == name.ToLower())
                        {
                            (fe as BPDDL).SetSelectVal(dr[fe.Name]);
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
    }
    public class BPDtlMeta
    {
        /// <summary>
        ///  Form ID
        /// </summary>
        public string FK_MapData;
        /// <summary>
        ///  From the Table Name 
        /// </summary>
        public string DtlName;
        /// <summary>
        ///  From the logical table 
        /// </summary>
        public List<BPDtlLogic> Logics = new List<BPDtlLogic>();
    }
    public class BPDtlLogic
    {
        /// <summary>
        ///  Logic Type 
        /// </summary>
        public string ExtType;
        /// <summary>
        ///  Passive control change 
        /// </summary>
        public string AttrOfOperName;
        /// <summary>
        ///  Active changes 
        /// </summary>
        public List<string> AttrsOfActiveNames = new List<string>();
        /// <summary>
        /// DOC
        /// </summary>
        public string Doc;
        /// <summary>
        /// 
        /// </summary>
        public List<BPLogic> Logics = new List<BPLogic>();
    }
    public class BPLogic
    {
        /// <summary>
        /// 
        /// </summary>
        public FrameworkElement AttrOfOper;
        /// <summary>
        /// 
        /// </summary>
        public List<FrameworkElement> AttrsOfActive = new List<FrameworkElement>();
    }
    public enum LogicType
    {
        None,
        AutoFull,
        ActiveDDL,
        TBFullCtrl
    }
}
