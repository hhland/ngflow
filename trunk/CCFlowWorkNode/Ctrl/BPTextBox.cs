using System;
using System.Windows;
using System.Windows.Media;
using BP.En;

namespace WorkNode
{
    /// <summary>
    ///  Type 
    /// </summary>
    public enum TBType
    {
        String,
        Int,
        Float,
        Money,
        DateTime,
        Date
    }
    public class BPTextBox : System.Windows.Controls.TextBox
    {
        #region  Check processing .
        /*
        private string _NameOfReal = null;
        public string NameOfReal
        {
            get { return _NameOfReal; }
            set { _NameOfReal = value; }
        }
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
        public void SetUnSelectedState()
        {
            if (this.IsSelected)
                this.IsSelected = false;
            else
                this.IsSelected = true;
        }
       

        public double X = 0;
        public double Y = 0;


        private TBType _HisTBType = TBType.String;
        /// <summary>
        ///  Type 
        /// </summary>
        public TBType HisTBType
        {
            get { return _HisTBType; }
            set { _HisTBType = value; }
        }
        public string HisDataType
        {
            get
            {
                switch (this.HisTBType)
                {
                    case TBType.Float:
                        return DataType.AppFloat;
                    case TBType.Money:
                        return DataType.AppMoney;
                    case TBType.Int:
                        return DataType.AppInt;
                    case TBType.Date:
                        return DataType.AppDate;
                    case TBType.DateTime:
                        return DataType.AppDateTime;
                    case TBType.String:
                    default:
                        return DataType.AppString;
                }
            }
            set
            {
                switch (value)
                {

                    case DataType.AppInt:
                        _HisTBType = TBType.Int;
                        break;
                    case DataType.AppFloat:
                    case DataType.AppDouble:
                        _HisTBType = TBType.Float;
                        break;
                    case DataType.AppMoney:
                        _HisTBType = TBType.Money;
                        break;
                    case DataType.AppString:
                        _HisTBType = TBType.String;
                        break;
                    case DataType.AppDateTime:
                        _HisTBType = TBType.DateTime;
                        break;
                    case DataType.AppDate:
                        _HisTBType = TBType.Date;
                        break;
                    default:
                        break;
                }
            }
        }


        public string KeyName = null;
        /// <summary>
        /// BPTextBox
        /// </summary>
        public BPTextBox()
        {
            this.Loaded += new RoutedEventHandler(BPTextBox_Loaded);
        }
        void BPTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            this.InitType();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ty"></param>
        //public BPTextBox(TBType ty)
        //{
        //    this.Name = "TB" + DateTime.Now.ToString("yyMMddhhmmss");
        //    this.HisTBType = ty;
        //    this.InitType();
        //}
        private string TBVal = null;
        public BPTextBox(TBType ty, string tbName)
        {
            this.NameOfReal = tbName;
            this.Name = tbName;
            // "TB" + DateTime.Now.ToString("yyMMddhhmmss");
            this.HisTBType = ty;
            this.InitType();
        }
        public void InitType()
        {
            Style style = new Style();
            this.Background = new SolidColorBrush(Colors.White);

            switch (this.HisTBType)
            {
                case TBType.Date:
                    this.Width = 100;
                    this.Height = 23;
                    break;
                case TBType.DateTime:
                    this.Width = 120;
                    this.Height = 23;
                    break;
                case TBType.String:
                    this.Width = 100;
                    this.Height = 23;
                    break;
                case TBType.Money:
                    this.Width = 100;
                    this.Height = 23;
                   // this.Text = "0.00";
                    this.TextAlignment = System.Windows.TextAlignment.Right;
                    break;
                case TBType.Int:
                    this.Width = 100;
                    this.Height = 23;
                    //this.Text = "0";
                    this.TextAlignment = System.Windows.TextAlignment.Right;
                    break;
                case TBType.Float:
                    this.Width = 100;
                    this.Height = 23;
                    //this.Text = "0";
                    this.TextAlignment = System.Windows.TextAlignment.Right;
                    break;
                default:
                    break;
            }
        }
        protected override void OnDrop(DragEventArgs e)
        {
            MessageBox.Show(e.ToString());
            base.OnDrop(e);
        }
         * */
        #endregion  Check processing .
        private string _NameOfReal = null;
        public string NameOfReal
        {
            get { return _NameOfReal; }
            set { _NameOfReal = value; }
        }
        private string StrRegex = string.Empty;
        private string OldText = string.Empty;
        private TBType _TBType = TBType.String;
        public TBType TBType
        {
            get
            {
                return _TBType;
            }
            set
            {
                _TBType = value;
            }
        }
        public BPTextBox()
            : base()
        {
            this.TextChanged += BPTextBox_TextChanged;
            this.Loaded += BPTextBox_Loaded;
        }

        void BPTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            OldText = this.Text;
            InitType();
        }

        public string HisDataType
        {
            get
            {
                switch (this._TBType)
                {
                    case TBType.Float:
                        return DataType.AppFloat;
                    case TBType.Money:
                        return DataType.AppMoney;
                    case TBType.Int:
                        return DataType.AppInt;
                    case TBType.Date:
                        return DataType.AppDate;
                    case TBType.DateTime:
                        return DataType.AppDateTime;
                    case TBType.String:
                        return DataType.AppString;
                    default:
                        return DataType.AppString;
                }
            }
            set
            {
                switch (value)
                {
                    case DataType.AppInt:
                        _TBType = TBType.Int;
                        break;
                    case DataType.AppFloat:
                        _TBType = TBType.Float;
                        break;
                    case DataType.AppDouble:
                        _TBType = TBType.Float;
                        break;
                    case DataType.AppMoney:
                        _TBType = TBType.Money;
                        break;
                    case DataType.AppString:
                        _TBType = TBType.String;
                        break;
                    case DataType.AppDateTime:
                        _TBType = TBType.DateTime;
                        break;
                    case DataType.AppDate:
                        _TBType = TBType.Date;
                        break;
                }
            }
        }

        private void InitType()
        {
            switch (this._TBType)
            {
                case TBType.Date:
                    this.Width = 100;
                    this.Height = 23;
                    break;
                case TBType.DateTime:
                    this.Width = 120;
                    this.Height = 23;
                    break;
                case TBType.String:
                    this.Width = 100;
                    this.Height = 23;
                    break;
                case TBType.Money:
                    this.Width = 100;
                    this.Height = 23;
                    this.TextAlignment = System.Windows.TextAlignment.Right;
                    this.StrRegex = @"^(-?\d+)(\.\d+)?$"; // Float  
                    break;
                case TBType.Int:
                    this.Width = 100;
                    this.Height = 23;
                    this.TextAlignment = System.Windows.TextAlignment.Right;
                    this.StrRegex = @"^-?\d+$";
                    break;
                case TBType.Float:
                    this.Width = 100;
                    this.Height = 23;
                    this.TextAlignment = System.Windows.TextAlignment.Right;
                    this.StrRegex = @"^(-?\d+)(\.\d+)?$"; // Float  
                    break;
                default:
                    break;
            }
        }

        void BPTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (StrRegex != string.Empty)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(this.Text, StrRegex))
                {
                    this.OldText = this.Text;
                }
                else
                {
                    this.Text = this.OldText;
                    this.SelectionStart = this.Text.Length;
                }
            }
        }
    }
}