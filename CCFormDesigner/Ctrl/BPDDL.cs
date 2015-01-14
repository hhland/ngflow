using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BP.En;

namespace CCForm
{

    public class BPDDL : System.Windows.Controls.ComboBox, IElement, IRouteEvent, IDelete
    {
        #region Properties
        public bool ViewDeleted { get; set; }
        public bool TrackingMouseMove { get; set; }
       
        bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                if (value)
                {
                    Thickness d = new Thickness(1);
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
        public bool IsCanReSize
        {
            get
            {
                return false;
            }
        }
        public string KeyName = null;
        public string UIBindKey = "";
        public string _HisDataType = LGType.Enum;
        public string HisDataType
        {
            get
            {
                if (this._HisDataType == LGType.Enum)
                    return DataType.AppInt;
                else
                    return DataType.AppString;
            }
        }
        #endregion 
     
        public MouseButtonEventHandler LeftDown { get; set; }
        public MouseButtonEventHandler LeftUp { get; set; }
        public BPDDL()
        {
            this.Name = MainPage.Instance.getElementNameFromUI(this);

            this.AddHandler(ComboBox.MouseLeftButtonDownEvent, new MouseButtonEventHandler(BPDDL_MouseLeftButtonDown), true);
            this.AddHandler(ComboBox.MouseLeftButtonUpEvent, new MouseButtonEventHandler(BPDDL_MouseLeftButtonUp), true);
        }

      
        #region bind Enum
        /// <summary>
        /// enumID
        /// </summary>
        /// <param name="enumID"></param>
        public void BindEnum(string enumID)
        {

            this.UIBindKey = enumID;
            this._HisDataType = LGType.Enum;
            string sql = "SELECT IntKey as No, Lab as Name FROM Sys_Enum WHERE EnumKey='" + enumID + "'";
            this.BindSQL(sql);
        }
        /// <summary>
        /// ensName
        /// </summary>
        /// <param name="ensName"></param>
        public void BindEns(string ensName)
        {
            if (string.IsNullOrEmpty(ensName))
                throw new Exception("@ensName Can not be null .");

            this.UIBindKey = ensName;
            this._HisDataType = LGType.FK;
            FF.CCFormSoapClient da = Glo.GetCCFormSoapClientServiceInstance();
            da.RequestSFTableV1Async(ensName);
            da.RequestSFTableV1Completed += new EventHandler<FF.RequestSFTableV1CompletedEventArgs>(
                (object sender, FF.RequestSFTableV1CompletedEventArgs e)=>
                {
                    try
                    {
                        Silverlight.DataSet ds = new Silverlight.DataSet();
                        ds.FromXml(e.Result);
                        this.Items.Clear();
                        foreach (Silverlight.DataRow dr in ds.Tables[0].Rows)
                        {
                            ListBoxItem li = new ListBoxItem();
                            li.Tag = dr["No"];
                            li.Content = dr["Name"];
                            this.Items.Add(li);
                        }
                        if (this.Items.Count != 0)
                            this.SelectedIndex = 0;
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        if (null != e.Error)
                            msg += "\t\n Data :" + e.Error.Message;
                        MessageBox.Show(msg);
                    }
                });
        }
       
        public void BindSQL(string sql)
        {
            try
            {
                FF.CCFormSoapClient da = Glo.GetCCFormSoapClientServiceInstance();
                da.RunSQLReturnTableAsync(sql);
                da.RunSQLReturnTableCompleted += new EventHandler<FF.RunSQLReturnTableCompletedEventArgs>(
                    (object sender, FF.RunSQLReturnTableCompletedEventArgs e)=>
                    {
                        try
                        {
                            Silverlight.DataSet ds = new Silverlight.DataSet();
                            ds.FromXml(e.Result);

                            this.Items.Clear();
                            foreach (Silverlight.DataRow dr in ds.Tables[0].Rows)
                            {
                                this.Items.Add(dr["No"] + ":" + dr["Name"]);
                            }

                            if (this.Items.Count != 0)
                                this.SelectedIndex = 0;
                        }
                        catch(Exception ex)
                        {
                            BP.SL.LoggerHelper.Write(ex);
                            MessageBox.Show(ex.Message);
                        }
                    });
            }
            catch (Exception ex)
            {
                BP.SL.LoggerHelper.Write(ex);
                MessageBox.Show(ex.Message);
            }
        }
     
        #endregion bing Enum


        #region  Mobile Event 
      
        Point pointFrom;

        void BPDDL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.CaptureMouse();
            {
                pointFrom = e.GetPosition(null);
            }
            if (null != LeftDown)
                LeftDown(this, e);
        }
        void BPDDL_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.ReleaseMouseCapture();
            this.TrackingMouseMove = false;
            if (null != LeftUp)
                LeftUp(this, e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            Point pTo = e.GetPosition(null);
            if (TrackingMouseMove)
            {
                double moveH = pTo.Y - pointFrom.Y;
                double moveW = pTo.X - pointFrom.X;
                Canvas.SetTop(this, Canvas.GetTop(this) + moveH);
                Canvas.SetLeft(this, Canvas.GetLeft(this) + moveW);
                pointFrom = pTo;
            }
            base.OnMouseMove(e);
        }

        #endregion


        public void DeleteIt()
        {
            if (this.Name != null)
            {
                FF.CCFormSoapClient da = Glo.GetCCFormSoapClientServiceInstance();
                string sqls = "DELETE FROM Sys_MapAttr WHERE FK_MapData='" + Glo.FK_MapData + "' AND KeyOfEn='" + this.Name + "'";
                da.RunSQLAsync(sqls);
            }
            Glo.Remove( this);
        }

        public void HidIt()
        {
            FF.CCFormSoapClient hidDA = Glo.GetCCFormSoapClientServiceInstance();
            hidDA.RunSQLAsync("UPDATE Sys_MapAttr SET UIVisible=0 WHERE KeyOfEn='" + this.Name + "' AND FK_MapData='" + Glo.FK_MapData + "'");
            hidDA.RunSQLCompleted += (object sender, FF.RunSQLCompletedEventArgs e)=>
            {
                this.Visibility = System.Windows.Visibility.Collapsed;
            };
        }
        
    }
}
