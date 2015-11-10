using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace CCForm
{
    public class BPImgAth : TextBoxExt
    {
        /// <summary>
        ///  Whether to allow editing 
        /// </summary>
        public bool IsEdit = true;
        /// <summary>
        ///  Accessory ID
        /// </summary>
        public string  CtrlID = null;
     
      
        /// <summary>
        /// BPImgAth
        /// </summary>
        public BPImgAth()
        {
            this.Name = MainPage.Instance.getElementNameFromUI(this);
            this.IsReadOnly = true;

            this.Width = 160;
            this.Height = 200;

            ImageBrush ib = new ImageBrush();
            BitmapImage png = new BitmapImage(new Uri("/CCFormDesigner;component/Img/Logo/" + Glo.CompanyID + "/LogoH.png", UriKind.Relative));

            ib.ImageSource = png;
            this.Background = ib;
            this.TextWrapping = System.Windows.TextWrapping.Wrap;
          
            this.IsSelected = false;
        }
      
    }
}
