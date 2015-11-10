using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace CCForm
{
    public class BPImg : TextBoxExt
    {
        public string LinkTarget = "_blank";
        public string LinkURL = "";
        /// <summary>
        ///  Picture Path 
        /// </summary>
        public string ImgURL = "";
        /// <summary>
        ///  File Path 
        /// </summary>
        public string ImgPath = "";
        /// <summary>
        ///  Chinese name 
        /// </summary>
        public string TB_CN_Name = "";
        /// <summary>
        ///  English 
        /// </summary>
        public string TB_En_Name = "";
        /// <summary>
        ///  Source Type 
        /// </summary>
        public int SrcType = 0;
        public BitmapImage HisPng = null;

      
        /// <summary>
        /// BPImg
        /// </summary>
        public BPImg()
        {
            this.Name = MainPage.Instance.getElementNameFromUI(this);
            this.IsReadOnly = true;

            this.Width = 200;
            this.Height = 120;

            ImageBrush ib = new ImageBrush();
            BitmapImage png = new BitmapImage(new Uri("/CCFormDesigner;component/Img/Logo/"+Glo.CompanyID+"/LogoBig.png", UriKind.Relative));
            ib.ImageSource = png;
            this.Background = ib;

            this.HisPng = png;
            this.TextWrapping = System.Windows.TextWrapping.Wrap;
         
            this.IsSelected = false;
        }

    }
}
