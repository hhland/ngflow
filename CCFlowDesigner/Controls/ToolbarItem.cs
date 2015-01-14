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
using System.Windows.Media.Imaging;

namespace BP.Controls
{
    public class ToolbarItem
    {

        public string No { get; set; }

        public string Name { get; set; }

        public bool IsEnable { get; set; }

        #region  Single Instance 
        public static readonly ToolbarItem Instance = new ToolbarItem();
        #endregion

        #region  Public Methods 
        public List<ToolbarItem> GetLists()
        {
            List<ToolbarItem> ToolList = new List<ToolbarItem>()
            {
                new ToolbarItem(){No="ToolBarLogin", Name = "  Log in ", IsEnable = true},
                new ToolbarItem(){No="ToolBarToolBox", Name=" Toolbox " },
                new ToolbarItem(){No="ToolBarSave", Name=" Save "},
                new ToolbarItem(){No="ToolBarDesignReport", Name=" Design Reports "},
                new ToolbarItem(){No="ToolBarCheck", Name=" Inspection "},
                new ToolbarItem(){No="ToolBarRun", Name=" Run "},
                new ToolbarItem(){No="ToolBarEditFlow", Name=" Property "},
                new ToolbarItem(){No="ToolBarDeleteFlow", Name=" Delete "},
                new ToolbarItem(){No="ToolBarGenerateModel", Name=" Export "},
                new ToolbarItem(){No="ToolBarShareModel", Name=" Template Library ", IsEnable = true},
                //new ToolbarItem(){No="ToolBarFrmLab", Name=" Form Library "},
                //new ToolbarItem(){No="ToolBarReleaseToFTP", Name=" Shared processes "}, 
                new ToolbarItem(){No="ToolBarFlowUI", Name=" Node Style "},

                new ToolbarItem(){No="ToolBarHelp", Name=" Help ", IsEnable = true},
                
            };
            return ToolList;
        }
        #endregion
    }
}

