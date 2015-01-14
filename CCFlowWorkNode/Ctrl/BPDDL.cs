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
using BP.En;
using Silverlight;


namespace WorkNode
{
    public class BPDDL : System.Windows.Controls.ComboBox
    {
        #region  Check processing .
        private bool _IsSelected = false;
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
                if (value == true)
                {
                    Thickness d = new Thickness(0.5);
                    this.BorderThickness = d;
                    this.BorderBrush = new SolidColorBrush(Colors.Blue);
                }
                else
                {
                    Thickness d1 = new Thickness(0.5);
                    this.BorderThickness = d1;
                    this.BorderBrush = new SolidColorBrush(Colors.Black);
                }
            }
        }
        public string _NameOfReal = string.Empty;
        public string NameOfReal
        {
            get { return _NameOfReal; }
            set { _NameOfReal = value; }
        }
        /// <summary>
        ///  Set selected value 
        /// </summary>
        /// <param name="val"> Value to be selected </param>
        public bool SetSelectVal(string val)
        {
            bool result = false;
            foreach (ComboBoxItem item in this.Items)
            {
                if (item.Tag.ToString() == val)
                {
                    item.IsSelected = true;
                    this.SelectedItem = item;
                    this.SelectedValue = item;
                    result = true;
                }
                else
                {
                    item.IsSelected = false;
                }
            }
            return result;
        }

        public static readonly DependencyProperty SelectedContentProperty = DependencyProperty.Register("SelectedContent", typeof(object), typeof(BPDDL), null);
        public object SelectedContent
        {
            get { return (object)GetValue(SelectedContentProperty); }
            set { SetValue(SelectedContentProperty, value); }
        }

        #endregion  Check processing .

        #region bind Enum

        private string _HisLgType = LGType.Enum;
        public string HisLGType
        {
            get { return _HisLgType; }
            set
            {
                _HisLgType = value;
                if (_HisLgType != LGType.Enum && !string.IsNullOrEmpty(_UIBindKey))
                {
                    BindEns(_UIBindKey);
                }
            }
        }

        public string HisDataType
        {
            get
            {
                if (this.HisLGType == LGType.Enum)
                    return DataType.AppInt;
                else
                    return DataType.AppString;
            }
        }
        public string KeyName { get; set; }

        public string _UIBindKey = string.Empty;
        public string UIBindKey
        {
            get { return _UIBindKey; }
            set
            {
                _UIBindKey = value;
            }
        }

        /// <summary>
        /// BPDDL
        /// </summary>
        public BPDDL()
        {
            this.Name = "DDL" + DateTime.Now.ToString("yyMMddhhmmss");
            this.Loaded += new RoutedEventHandler(BPDDL_Loaded);
        }

        void BPDDL_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (object.Equals((this.Items[i] as ComboBoxItem).Content, SelectedContent))
                {
                    this.SelectedIndex = i;
                    break;
                }
            }
        }
        #endregion bind Enum

        #region bind Enum
        /// <summary>
        /// enumID
        /// </summary>
        /// <param name="enumID"></param>
        public void BindEnum(string enumID)
        {
            this.UIBindKey = enumID;
            this._HisLgType = LGType.Enum;
            string sql = "SELECT IntKey as No, Lab as Name FROM Sys_Enum WHERE EnumKey='" + enumID + "'";
            this.BindSQL(sql);
        }
        /// <summary>
        ///  Foreign key bindings 
        /// </summary>
        /// <param name="ensName"> For example, the table name or class name : CN_City / BP.Port.Depts</param>
        public void BindEns(string ensName)
        {
            this.UIBindKey = ensName;
            this._HisLgType = LGType.FK;
            FF.CCFlowAPISoapClient da = Glo.GetCCFlowAPISoapClientServiceInstance();
            da.RequestSFTableV1Completed += new EventHandler<FF.RequestSFTableV1CompletedEventArgs>(da_RequestSFTableV1Completed);
            da.RequestSFTableV1Async(ensName);
        }

        void da_RequestSFTableV1Completed(object sender, FF.RequestSFTableV1CompletedEventArgs e)
        {
            try
            {
                Silverlight.DataSet ds = new Silverlight.DataSet();
                ds.FromXml(e.Result);
                this.Items.Clear();
                foreach (Silverlight.DataRow dr in ds.Tables[0].Rows)
                {
                    ComboBoxItem li = new ComboBoxItem();
                    li.Tag = dr["No"];
                    li.Content = dr["Name"];
                    this.Items.Add(li);
                }
                if (this.Items.Count != 0)
                    this.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\t\n Data :" + e.Error.Message);
            }
        }
        #endregion bing Enum

        #region bind table
        public void BindSQL(string sql)
        {
            try
            {
                FF.CCFlowAPISoapClient da = Glo.GetCCFlowAPISoapClientServiceInstance();
                da.RunSQLReturnTableAsync(sql);
                da.RunSQLReturnTableCompleted += new EventHandler<FF.RunSQLReturnTableCompletedEventArgs>(da_RunSQLReturnTableCompleted);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void da_RunSQLReturnTableCompleted(object sender, FF.RunSQLReturnTableCompletedEventArgs e)
        {
            try
            {
                Silverlight.DataSet ds = new Silverlight.DataSet();
                ds.FromXml(e.Result);

                this.Items.Clear();
                foreach (Silverlight.DataRow dr in ds.Tables[0].Rows)
                {
                    ComboBoxItem li = new ComboBoxItem();
                    li.Tag = dr["No"];
                    li.Content = dr["Name"];
                    this.Items.Add(li);
                }
                if (this.Items.Count != 0)
                    this.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion bing Enum
    }
}
