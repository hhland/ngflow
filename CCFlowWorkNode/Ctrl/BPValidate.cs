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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Windows.Controls.Primitives;

namespace WorkNode
{
    public class BPValidate
    {
        List<ValidateControl> ValidateControls = new List<ValidateControl>();

        public void AddValidateControl(TextBox textBox, ValidateType validateType, string strReg, string errorMessage)
        {
            var vc = ValidateControls.Where(p => p.TextBoxToValidate == textBox).FirstOrDefault();
            if (vc == null)
            {
                ValidateControl validate = new ValidateControl();
                validate.OnValidate += new OnValidateHandler(validate_OnValidate);
                validate.BindValidate(textBox, validateType, strReg, errorMessage);
            }
            else
            {
                vc.BindValidate(textBox, validateType, strReg, errorMessage);
            }
        }

        void validate_OnValidate(object sender, ValidateEventArgs arg)
        {

        }

        public void AddValidateControl(TextBox textBox, string validateType, string strRegex, string errorMessage)
        {
            ValidateType type = ValidateType.Null;
            if (validateType.ToLower() == "onblur")
            {
                type = ValidateType.OnLostFocus;
            }
            else if (validateType.ToLower() == "onkeypress")
            {
                type = ValidateType.OnKeyDown;
            }
            AddValidateControl(textBox, type, strRegex, errorMessage);
        }

    }
    public class ValidateControl
    {
        private List<ValidateType> ValidateTypes = new List<ValidateType>();
        public TextBox TextBoxToValidate;
        private string ErrorMsg;
        private string StrReg;
        Popup popup = new Popup();
        public void BindValidate(TextBox textBox, ValidateType validateType, string strReg, string errorMessage)
        {
            this.StrReg = strReg;
            this.ErrorMsg = errorMessage;
            this.TextBoxToValidate = textBox;

            if (ValidateTypes.Contains(validateType) == true)
            {
                return;
            }
            if (validateType == ValidateType.OnKeyDown)
            {
                textBox.KeyDown += new KeyEventHandler(textBox_KeyDown);
            }
            else if (validateType == ValidateType.OnKeyUp)
            {
                textBox.KeyUp += new KeyEventHandler(textBox_KeyUp);
            }
            else if (validateType == ValidateType.OnLostFocus)
            {
                textBox.LostFocus += new RoutedEventHandler(textBox_LostFocus);
            }
        }

        void textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckValidate();
        }

        void textBox_KeyUp(object sender, KeyEventArgs e)
        {
            CheckValidate();
        }

        void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            CheckValidate();
        }
        private void CheckValidate()
        {
            Regex reg = new Regex(StrReg);
            bool result = reg.IsMatch(this.TextBoxToValidate.Text);
            Validated(result);
            //if (result == false)
            //{
            //    this.TextBoxToValidate.Focus();
            //}
            if (OnValidate != null)
            {
                OnValidate.Invoke(this, new ValidateEventArgs() { Result = result });
            }
        }
        void Validated(bool result)
        {
            if (result == false)
            {
                var gt = TextBoxToValidate.TransformToVisual(null);
                TextBoxToValidate.BorderBrush = new SolidColorBrush(Colors.Red);
                TextBoxToValidate.BorderThickness = new Thickness(1);
                Point point = gt.Transform(new Point(0, 0));
                Label lbl = new Label();
                lbl.Background = new SolidColorBrush(Colors.Red);
                lbl.Padding = new Thickness(5, 2, 5, 2);
                lbl.Content = ErrorMsg;
                lbl.Height = TextBoxToValidate.ActualHeight;
                popup.Child = lbl;
                popup.SetValue(Canvas.LeftProperty, point.X + TextBoxToValidate.Width);
                popup.SetValue(Canvas.TopProperty, point.Y);
                popup.IsOpen = true;
            }
            else
            {
                TextBoxToValidate.BorderThickness = new Thickness(0);
                popup.IsOpen = false;
            }
        }
        public event OnValidateHandler OnValidate;
    }

    public enum ValidateType
    {
        Null,
        OnLostFocus,
        OnKeyUp,
        OnKeyDown
    }

    public delegate void OnValidateHandler(object sender, ValidateEventArgs e);
}

public class ValidateEventArgs : EventArgs
{
    public bool Result;
}

//onblur-onblur
//onchange-onchange
//ondblclick-ondblclick
//onkeypress-onkeypress
//onkeyup-onkeyup
//onsubmit-onsubmit
