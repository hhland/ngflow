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

namespace WorkNode
{
    public class BPRadioBtn : System.Windows.Controls.RadioButton
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
        public void SetUnSelectedState()
        {
            if (this.IsSelected)
                this.IsSelected = false;
            else
                this.IsSelected = true;
        }
        #endregion  Check processing .

        public string KeyName = null;
        public string UIBindKey = null;
        protected override void OnClick()
        {
            base.OnClick();
        }
       
        public string FK_MapData = null;
        /// <summary>
        /// BPRadioButton
        /// </summary>
        public BPRadioBtn()
        {
            this.Name = "RB" + DateTime.Now.ToString("yyMMddhhmmss");
        }
    }
}
