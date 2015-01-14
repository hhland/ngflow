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
using System.Windows.Data;

namespace BP
{
    public struct SEnum
    {
        public string Name;
        public int Value;
    }

    /// <summary>
    ///  Organizations / Mode  
    /// </summary>
    public enum OSModel
    {
        /// <summary>
        ///  Integrated (WorkFlow) Embedded mode 
        /// </summary>
        WorkFlow = 0,
        /// <summary>
        ///  Independent operation (BPM) Mode 
        /// </summary>
        BPM =1
    }
    /// <summary>
    ///  Application Type 
    /// </summary>
    public enum AppType
    {
        /// <summary>
        ///  Process Form 
        /// </summary>
        Application = 0,
        /// <summary>
        ///  Node Form 
        /// </summary>
        Node = 1
    }
    public enum FlowChartType
    {
        Shape = 0,
        UserIcon = 1,  
        UnKnown

    }
    /// <summary>
    ///  Node positions 
    /// </summary>
    public enum NodePosType
    {
        Start,
        Mid,
        End
    }
    public enum MergePictureRepeatDirection
    {
        Vertical = 0,
        Horizontal,
        None
    }
    /// <summary>
    ///  Node Type 
    /// </summary>
    public enum FlowNodeType_del
    {
        INITIAL,
        INTERACTION,
        COMPLETION,
        //AND_MERGE, ccflow  Without these types of .
        //AND_BRANCH,
        STATIONODE,
        AUTOMATION,
        DUMMY,
        //OR_BRANCH,
        //OR_MERGE,
        SUBPROCESS,
        VOTE_MERGE,
    }
    public enum FlowNodeType
    {
        /// <summary>
        ///  General 
        /// </summary>
        Ordinary = 0,
        /// <summary>
        ///  Confluence 
        /// </summary>
        HL = 1,
        /// <summary>
        ///  Bypass 
        /// </summary>
        FL = 2,
        /// <summary>
        ///  Confluence points 
        /// </summary>
        FHL,
        /// <summary>
        ///  Child thread .
        /// </summary>
        SubThread,
        /// <summary>
        ///  Virtual node 
        /// </summary>
        VirtualStart,
        VirtualEnd,
        //INITIAL,
        //INTERACTION,
        //COMPLETION,
        ////AND_MERGE, ccflow  Without these types of .
        ////AND_BRANCH,
        //STATIONODE,
        //AUTOMATION,
        //DUMMY,
        ////OR_BRANCH,
        ////OR_MERGE,
        //SUBPROCESS,
        //VOTE_MERGE,

        UnKnown
    }
    /// <summary>
    ///  Run mode 
    /// </summary>
    public enum RunModel
    {
        /// <summary>
        ///  General 
        /// </summary>
        Ordinary = 0,
        /// <summary>
        ///  Confluence 
        /// </summary>
        HL = 1,
        /// <summary>
        ///  Bypass 
        /// </summary>
        FL = 2,
        /// <summary>
        ///  Confluence points 
        /// </summary>
        FHL,
        /// <summary>
        ///  Child thread .
        /// </summary>
        SubThread
    }
    /// <summary>
    ///  Node Location Type 
    /// </summary>
    public enum FlowNodePosType
    {
        /// <summary>
        ///  Start node 
        /// </summary>
        Start,
        /// <summary>
        ///  Intermediate point 
        /// </summary>
        Mid,
        /// <summary>
        ///  End point 
        /// </summary>
        End
    }
    public enum WorkFlowElementType
    {
        FlowNode = 0,
        Direction,
        Label
    }

    public enum PageEditType
    {
        Add = 0,
        Modify,
        None
    }

    public enum DirectionLineType
    {
        Line = 0,
        Polyline
    }

    public enum HistoryType
    {
        New,
        Next,
        Previous
    } 

    public enum DirectionMove { Begin = 0, Line, End }

    public enum DirType
    {
        // Forward line 
        Forward = 0,
        // Fallback line 
        Backward = 1,
        // Virtual Line 
        Virtual = 2
    }

    /// <summary>
    ///  Ordinary working node processing mode 
    /// </summary>
    public enum TodolistModel
    {
        /// <summary>
        ///  Grab Office ( Who grabbed who handled , After handling other people can not handle .)
        /// </summary>
        QiangBan,
        /// <summary>
        ///  Cooperation ( No processing order , The person must accept to deal , Sent by the last person to the next node )
        /// </summary>
        Teamup,
        /// <summary>
        ///  Queue ( According to the order processing , There are the last person to send to the next node )
        /// </summary>
        Order,
        /// <summary>
        ///  Sharing Mode ( Need to apply , After the application to perform )
        /// </summary>
        Sharing
    }

    public enum BPFormType
    {
       
        FormNode,
        FormFlow
    }
    //public enum FrmType
    //{
    //     Fool form  = 0,
    //     Freedom Form  = 1,
    //    SL Form  = 2,
    //     Custom Form  = 3,
    //    Word Form  = 4,
    //    Excel Form  = 5
    //}


    //public class EnumConverter : IValueConverter 
    //{ 
    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) 
    //    { 
    //        if (value == null || parameter == null)   
    //            return value; 
    //        return value.ToString() == parameter.ToString(); 
    //    } 
    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    { 
    //        if (value == null || parameter == null)  
    //            return value; 
    //        return Enum.Parse(targetType, (String)parameter, true); 
    //    } 
    //}
}
