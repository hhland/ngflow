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

namespace BP
{
    public class CheckListBoxModel
    {
        private string id;
        public virtual string ID
        {
            get { return id; }
            set { id = value; }
        }

        private string modelName;
        public virtual string ModelName
        {
            get { return modelName; }
            set { modelName = value; }
        }

        private string desModel;
        public virtual string DesModel
        {
            get { return desModel; }
            set { desModel = value; }
        }
        /// <summary>
        ///  Selected for sub-pages 
        /// </summary>
        private bool isSelected;
        public virtual bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }
    }
}
