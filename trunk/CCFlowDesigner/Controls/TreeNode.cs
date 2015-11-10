﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Liquid;

namespace BP.Controls
{
    public class TreeNode : Node
    {
        /// <summary>
        ///  Whether the process category 
        /// </summary>
        public bool IsSort { get; set; }
        public TreeNode() { }
        /// <summary>
        ///  Department 
        /// </summary>
        public string FK_Dept { get; set; }
        /// <summary>
        ///  Post 
        /// </summary>
        public string FK_Station { get; set; }
        public bool isRoot { get; set; }
        public bool isDept { get; set; }
        public bool isStation { get; set; }
        public string EmpNo { get; set; }
    }
}
