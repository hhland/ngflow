
namespace CCFlow.WF.Comm.UC
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Data.Odbc;
    using System.Drawing;
    using System.Web;
    using System.Web.SessionState;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.HtmlControls;
    using System.Linq;
    using BP.DA;
    using BP.En;
    using BP.Sys;
    using BP.Web;
    using BP.Web.Controls;
    using BP.Web.UC;
    using BP.XML;
    using BP.Sys.Xml;
    using BP.Port;
    using BP.Web;
    using BP.Web.Comm;
    using BP;
    using CCFlow.Web.Comm;
    using System.Collections.Generic;
    using System.Web.UI.DataVisualization.Charting;
    // using OWC10;
    // using Microsoft.Office.Interop.Owc11;
    /// <summary>
    ///		UCSys  The summary .
    /// </summary>
    public partial class UCSys : BP.Web.UC.UCBase
    {
        public static string FilesViewStr(string enName, object pk)
        {
            string url = "../Comm/FileManager.aspx?EnsName=" + enName + "&PK=" + pk.ToString();

            //string strs="<a href=\"javascript:WinOpen("") \" > Accessory </>";
            //string strs="<a href=\"javascript:WinOpen('"+url+"') \" > Edit attachments </>";
            string strs = "";
            SysFileManagers ens = new SysFileManagers(enName, pk.ToString());
            string path = System.Web.HttpContext.Current.Request.ApplicationPath;
            foreach (SysFileManager file in ens)
            {
                strs += "<img src='../Img/FileType/" + file.MyFileExt.Replace(".", "") + ".gif' border=0 /><a href='" + path + file.MyFilePath + "' target='_blank' >" + file.MyFileName + file.MyFileExt + "</a>&nbsp;";
                if (file.Rec == WebUser.No)
                {
                    strs += "<a title=' Open it ' href=\"javascript:DoAction('../Comm/Do.aspx?ActionType=" + (int)ActionType.DeleteFile + "&OID=" + file.OID + "&EnsName=" + enName + "&PK=" + pk + "',' Delete files 《" + file.MyFileName + file.MyFileExt + "》')\" ><img src='" + path + "../Img/Btn/delete.gif' border=0 alt=' Delete this attachment ' /></a>&nbsp;";
                }
            }
            return strs;
        }

        public static string FilesViewStr1(string enName, object pk)
        {
            string url = "../Comm/FileManager.aspx?EnsName=" + enName + "&PK=" + pk.ToString();

            //string strs="<a href=\"javascript:WinOpen("") \" > Accessory </>";
            string strs = "<a href=\"javascript:WinOpen('" + url + "') \" > Edit attachments </>";
            SysFileManagers ens = new SysFileManagers(enName, pk.ToString());
            foreach (SysFileManager file in ens)
            {
                strs += "<img src='../Img/FileType/" + file.MyFileExt.Replace(".", "") + ".gif' border=0 /><a href='../" + file.MyFilePath + "' target='_blank' >" + file.MyFileName + file.MyFileExt + "</a>&nbsp;";
            }
            return strs;
        }

        public string GenerIt()
        {
            //// Create a graphic object container 
            //OWC11.ChartSpace objCSpace = new OWC11.ChartSpaceClass();
            //// Add a graphic object in the graphic container 
            //OWC11.ChChart objChart = objCSpace.Charts.Add(0);
            //// The graph type is set as a histogram 
            //objChart.Type = OWC11.ChartChartTypeEnum.chChartTypeColumnStacked;
            //// The border color is set to white graphic container 
            //objCSpace.Border.Color = "White";

            //// Show Title 
            //objChart.HasTitle = true;
            //// Setting Title Content 
            //objChart.Title.Caption = " Chart test ";
            //// The title font size settings 
            //objChart.Title.Font.Size = 10;
            //// Set the title in bold 
            //objChart.Title.Font.Bold = true;
            //// Set the title color to red 
            //objChart.Title.Font.Color = "Red";

            //// Show legend 
            //objChart.HasLegend = true;
            //// Set the legend font size 
            //objChart.Legend.Font.Size = 10;
            //// Set the legend at the bottom position 
            //objChart.Legend.Position = OWC11.ChartLegendPositionEnum.chLegendPositionBottom;

            //// Add a series of graphical objects 
            //objChart.SeriesCollection.Add(0);
            //// Given series name 
            //objChart.SeriesCollection[0].SetData(OWC11.ChartDimensionsEnum.chDimSeriesNames,
            //    +(int)OWC11.ChartSpecialDataSourcesEnum.chDataLiteral, " Index ");
            //// Setpoint 
            //objChart.SeriesCollection[0].SetData(OWC11.ChartDimensionsEnum.chDimValues,
            //    +(int)OWC11.ChartSpecialDataSourcesEnum.chDataLiteral, "10\t40\t58\t55\t44");

            //// Display data , Create GIF Relative path to the file .
            //string FileName = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".gif";
            //objCSpace.ExportPicture(@"E:\Projects\Study\OwcImg\ChartDetail.gif", "GIF", 450, 300);


            //return FileName;
            //Image1.ImageUrl = "Http://localhost/Study/OwcImg/ChartDetail.gif";
            return null;
        }

        public static string GenerChart(DataTable dt, string colOfGroupField, string colOfGroupName,
            string colOfNumField, string colOfNumName, string title, int chartHeight, int chartWidth, ChartType ct)
        {
            string fileName = "";
            //string strCategory = "";
            //string strValue = "";
            //// Statement object 
            //ChartSpace ThisChart = new ChartSpaceClass();
            //ChChart ThisChChart = ThisChart.Charts.Add(0);
            //ChSeries ThisChSeries = ThisChChart.SeriesCollection.Add(0);

            //// Show legend 
            //ThisChChart.HasLegend = true;
            //// Title 
            //ThisChChart.HasTitle = true;
            //ThisChChart.Title.Caption = title;

            //// Given x,y Axis illustration 
            //ThisChChart.Axes[0].HasTitle = true;
            //ThisChChart.Axes[1].HasTitle = true;

            //ThisChChart.Axes[0].Title.Caption = colOfGroupName;
            //ThisChChart.Axes[1].Title.Caption = colOfNumName;

            //switch (ct)
            //{
            //    case ChartType.Histogram:
            //        foreach (DataRow dr in dt.Rows)
            //        {
            //            strCategory += dr[colOfGroupField].ToString() + '\t';
            //            strValue += dr[colOfNumField].ToString() + '\t';
            //        }
            //        ThisChChart.Type = ChartChartTypeEnum.chChartTypeColumnClustered;
            //        ThisChChart.Overlap = 50;
            //        // Rotation 
            //        ThisChChart.Rotation = 360;
            //        ThisChChart.Inclination = 10;
            //        // Background Color 
            //        ThisChChart.PlotArea.Interior.Color = "white";
            //        // Background 
            //        ThisChChart.PlotArea.Floor.Interior.Color = "green";
            //        //// Given series Name 
            //        ThisChSeries.SetData(ChartDimensionsEnum.chDimSeriesNames,
            //            ChartSpecialDataSourcesEnum.chDataLiteral.GetHashCode(), colOfGroupName);
            //        // Given classification 
            //        ThisChSeries.SetData(ChartDimensionsEnum.chDimCategories,
            //            ChartSpecialDataSourcesEnum.chDataLiteral.GetHashCode(), strCategory);
            //        // Setpoint 
            //        ThisChSeries.SetData(ChartDimensionsEnum.chDimValues,
            //            ChartSpecialDataSourcesEnum.chDataLiteral.GetHashCode(), strValue);
            //        break;
            //    case ChartType.Pie:
            //        //  Generate data 
            //        foreach (DataRow dr in dt.Rows)
            //        {
            //            strCategory += dr[colOfGroupField].ToString() + '\t';
            //            strValue += dr[colOfNumField].ToString() + '\t';
            //        }

            //        ThisChChart.Type = ChartChartTypeEnum.chChartTypePie3D;
            //        ThisChChart.SeriesCollection.Add(0);
            //        // Display the data in the chart 
            //        ThisChChart.SeriesCollection[0].DataLabelsCollection.Add();
            //        ThisChChart.SeriesCollection[0].DataLabelsCollection[0].Position = ChartDataLabelPositionEnum.chLabelPositionAutomatic;
            //        ThisChChart.SeriesCollection[0].Marker.Style = ChartMarkerStyleEnum.chMarkerStyleCircle;

            //        // Given the name of the table data Photos  
            //        ThisChChart.SeriesCollection[0].SetData(ChartDimensionsEnum.chDimSeriesNames,
            //            +(int)ChartSpecialDataSourcesEnum.chDataLiteral, "strSeriesName");

            //        // Given data classification  
            //        ThisChChart.SeriesCollection[0].SetData(ChartDimensionsEnum.chDimCategories,
            //            +(int)ChartSpecialDataSourcesEnum.chDataLiteral, strCategory);

            //        // Setpoint  
            //        ThisChChart.SeriesCollection[0].SetData(ChartDimensionsEnum.chDimValues,
            //            (int)ChartSpecialDataSourcesEnum.chDataLiteral, strValue);
            //        break;
            //    case ChartType.Line:
            //        //  Generate data 
            //        foreach (DataRow dr in dt.Rows)
            //        {
            //            strCategory += dr[colOfGroupField].ToString() + '\t';
            //            strValue += dr[colOfNumField].ToString() + '\t';
            //        }
            //        ThisChChart.Type = ChartChartTypeEnum.chChartTypeLineStacked;
            //        ThisChChart.SeriesCollection.Add(0);
            //        // Display the data in the chart 
            //        ThisChChart.SeriesCollection[0].DataLabelsCollection.Add();
            //        //ThisChChart.SeriesCollection[0].DataLabelsCollection[0].Position=ChartDataLabelPositionEnum.chLabelPositionAutomatic;
            //        //ThisChChart.SeriesCollection[0].DataLabelsCollection[0].Position=ChartDataLabelPositionEnum.chLabelPositionOutsideBase;

            //        ThisChChart.SeriesCollection[0].Marker.Style = ChartMarkerStyleEnum.chMarkerStyleCircle;

            //        // Given the name of the table data Photos  
            //        ThisChChart.SeriesCollection[0].SetData(ChartDimensionsEnum.chDimSeriesNames,
            //            +(int)ChartSpecialDataSourcesEnum.chDataLiteral, "strSeriesName");

            //        // Given data classification  
            //        ThisChChart.SeriesCollection[0].SetData(ChartDimensionsEnum.chDimCategories,
            //            +(int)ChartSpecialDataSourcesEnum.chDataLiteral, strCategory);

            //        // Setpoint  
            //        ThisChChart.SeriesCollection[0].SetData(ChartDimensionsEnum.chDimValues,
            //            (int)ChartSpecialDataSourcesEnum.chDataLiteral, strValue);
            //        break;
            //}

            //// Exporting an image file 
            ////ThisChart.ExportPicture("G:\\chart.gif","gif",600,350);

            //string fileName = ct.ToString() + WebUser.No + ".gif";
            //string strAbsolutePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\Temp\\" + fileName;
            //try
            //{
            //    ThisChart.ExportPicture(strAbsolutePath, "GIF", chartWidth, chartHeight);
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("@ You can not create file , Permission may be a problem , Please set the directory anyone can modify ." + strAbsolutePath + " Exception:" + ex.Message);
            //}
            return fileName;
        }

        /// <summary>
        ///  Generate MS Chart Histogram 
        /// </summary>
        /// <param name="dt"> Data Sources </param>
        /// <param name="xfield">X Field axis to be displayed </param>
        /// <param name="xdesc">X Axis to display the text field </param>
        /// <param name="yfields">Y Axis project to be analyzed 
        /// <remarks>Dictionary&lt;string, string&gt;
        /// <para>Key. Field bound </para>
        /// <para>Value. Display name field is bound </para>
        /// </remarks></param>
        /// <param name="title"> Chart Title </param>
        /// <param name="chartWidth"> Width of a single chart </param>
        /// <param name="chartHeight"> The height of a single chart </param>
        public void GenerateColumnChart(DataTable dt, string xfield, string xdesc, Dictionary<string, string> yfields, string title, int chartWidth = 300, int chartHeight = 300)
        {
            var legendName = "MainLegend_";
            var seriesName = "Series_";

            var chart = new Chart();
            chart.ID = "Chart_" + xfield;
            chart.Width = new Unit(chartWidth);
            chart.Height = new Unit(chartHeight);
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BackColor = Color.FromArgb(224, 236, 255);//Color.FromArgb(243, 223, 193);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.BorderlineColor = Color.FromArgb(149, 184, 231);// Color.FromArgb(181, 64, 1);
            chart.BorderSkin = new BorderSkin { SkinStyle = BorderSkinStyle.Emboss };

            var chartTitle = chart.Titles.Add(title + " - " + xdesc + " -  Histogram ");
            chartTitle.Font = new Font(new FontFamily(" Times New Roman "), 10.5f, FontStyle.Bold);
            chartTitle.ShadowColor = Color.FromArgb(32, 0, 0, 0);
            chartTitle.ShadowOffset = 3;
            chartTitle.ForeColor = Color.FromArgb(26, 59, 105);

            var legend = chart.Legends.Add(legendName + xfield);
            legend.Docking = Docking.Bottom;
            legend.TitleFont = new Font(new FontFamily("Microsoft Sans Serif"), 8, FontStyle.Bold);
            legend.BackColor = Color.Transparent;
            legend.Font = new Font(new FontFamily("Trebuchet MS"), 8.25f, FontStyle.Bold);
            legend.IsTextAutoFit = false;
            legend.LegendStyle = LegendStyle.Table;

            var chartArea = chart.ChartAreas.Add("ChartArea_" + xfield);
            chartArea.BorderColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.BackSecondaryColor = Color.White;
            chartArea.BackColor = Color.OldLace;
            chartArea.ShadowColor = Color.Transparent;
            chartArea.BackGradientStyle = GradientStyle.TopBottom;
            chartArea.Area3DStyle.Rotation = 10;
            chartArea.Area3DStyle.Perspective = 10;
            chartArea.Area3DStyle.Inclination = 15;
            chartArea.Area3DStyle.IsRightAngleAxes = false;
            chartArea.Area3DStyle.WallWidth = 0;
            chartArea.Area3DStyle.IsClustered = false;
            chartArea.AxisY.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.LabelAutoFitMaxFontSize = 8;
            chartArea.AxisY.LabelStyle.Font = new Font(new FontFamily("Trebuchet MS"), 8.25f, FontStyle.Bold);
            chartArea.AxisY.LabelStyle.IsEndLabelVisible = false;
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.Title = xdesc;
            chartArea.AxisX.TitleAlignment = StringAlignment.Far;
            chartArea.AxisX.TitleFont = new Font(new FontFamily(" Times New Roman "), 9f, FontStyle.Bold);

            Series series = null;
            foreach (var yfield in yfields)
            {
                series = chart.Series.Add(seriesName + xfield + "_" + yfield.Key);
                series.Legend = legend.Name;
                series.LegendText = yfield.Value;
                series.ChartArea = chartArea.Name;
                series.ChartType = SeriesChartType.Column;
                series.BorderColor = Color.FromArgb(180, 26, 59, 105);
                series.XValueMember = xfield;
                series.YValueMembers = yfield.Key;
                series.IsValueShownAsLabel = true;
                series.CustomProperties = "LabelStyle=Bottom";
            }

            if (yfields.Count == 1)
            {
                chartArea.AxisY.Title = yfields.Values.First();
                chartArea.AxisY.TitleAlignment = StringAlignment.Far;
                chartArea.AxisY.TitleFont = new Font(new FontFamily(" Times New Roman "), 9f, FontStyle.Bold);
            }

            chart.DataSource = dt;
            chart.DataBind();

            this.Add(chart);
        }

        /// <summary>
        ///  Generate MS Chart Pie 
        /// </summary>
        /// <param name="dt"> Data Sources </param>
        /// <param name="xfield">X Field axis to be displayed </param>
        /// <param name="xdesc">X Axis to display the text field </param>
        /// <param name="yfields">Y Axis project to be analyzed 
        /// <remarks>Dictionary&lt;string, string&gt;
        /// <para>Key. Field bound </para>
        /// <para>Value. Display name field is bound </para>
        /// </remarks></param>
        /// <param name="title"> Chart Title </param>
        /// <param name="chartWidth"> Width of a single chart </param>
        /// <param name="chartHeight"> The height of a single chart </param>
        public void GeneratePieChart(DataTable dt, string xfield, string xdesc, Dictionary<string, string> yfields, string title, int chartWidth = 300, int chartHeight = 300)
        {
            var legendName = "MainLegend_";
            var seriesName = "Series_";

            var chart = new Chart();
            chart.ID = "Chart_" + xfield;
            chart.Width = new Unit(chartWidth);
            chart.Height = new Unit(chartHeight);
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BackColor = Color.FromArgb(224, 236, 255);//Color.FromArgb(243, 223, 193);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.BorderlineColor = Color.FromArgb(149, 184, 231);//Color.FromArgb(181, 64, 1);
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;

            var chartTitle = chart.Titles.Add(title + " - " + xdesc + " -  Pie Chart ");
            chartTitle.Font = new Font(new FontFamily(" Times New Roman "), 10.5f, FontStyle.Bold);
            chartTitle.ShadowColor = Color.FromArgb(32, 0, 0, 0);
            chartTitle.ShadowOffset = 3;
            chartTitle.ForeColor = Color.FromArgb(26, 59, 105);

            var legend = chart.Legends.Add(legendName + xfield);
            legend.Docking = Docking.Bottom;
            legend.TitleFont = new Font(new FontFamily("Microsoft Sans Serif"), 8, FontStyle.Bold);
            legend.BackColor = Color.Transparent;
            legend.Alignment = StringAlignment.Center;
            legend.Font = new Font(new FontFamily("Trebuchet MS"), 8.25f, FontStyle.Bold);
            legend.IsTextAutoFit = false;
            legend.LegendStyle = LegendStyle.Table;

            Series series = null;
            ChartArea chartArea = null;
            Legend tempLegend = null;

            foreach (var yfield in yfields)
            {
                chartArea = chart.ChartAreas.Add("ChartArea_" + yfield.Key);
                chartArea.BorderColor = Color.FromArgb(64, 64, 64, 64);
                chartArea.BackSecondaryColor = Color.Transparent;
                chartArea.BackColor = Color.Transparent;
                chartArea.ShadowColor = Color.Transparent;
                chartArea.BorderWidth = 0;
                chartArea.Area3DStyle.Rotation = 0;
                chartArea.AxisY.LineColor = Color.FromArgb(64, 64, 64, 64);
                chartArea.AxisY.LabelStyle.Font = new Font(new FontFamily("Trebuchet MS"), 8.25f, FontStyle.Bold);
                chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
                chartArea.AxisX.LineColor = Color.FromArgb(64, 64, 64, 64);
                chartArea.AxisX.LabelStyle.Font = new Font(new FontFamily("Trebuchet MS"), 8.25f, FontStyle.Bold);
                chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);

                series = chart.Series.Add(seriesName + xfield + "_" + yfield.Key);
                series.Legend = legend.Name;
                series["PieLabelStyle"] = "Inside";
                series["PieDrawingStyle"] = "SoftEdge";
                series.Label = "#PERCENT{P1}";
                series.ChartArea = chartArea.Name;
                series.ChartType = SeriesChartType.Pie;
                series.BorderColor = Color.FromArgb(180, 26, 59, 105);
                series.Color = Color.FromArgb(220, 65, 140, 240);

                foreach (DataRow dr in dt.Rows)
                {
                    series.Points.Add(new DataPoint
                                          {
                                              IsVisibleInLegend = chart.Series.Count < 2,
                                              LegendText = dr[xfield].ToString(),
                                              YValues = new[] { Convert.ToDouble(dr[yfield.Key]) }
                                          });
                }

                tempLegend = chart.Legends.Add(legendName + yfield.Key);
                tempLegend.BackColor = Color.Transparent;
                tempLegend.BorderColor = Color.Transparent;
                tempLegend.Font = new Font(new FontFamily(" Times New Roman "), 9f, FontStyle.Bold);
                tempLegend.DockedToChartArea = chartArea.Name;
                tempLegend.Docking = Docking.Left;
                tempLegend.CustomItems.Add(Color.Transparent, yfield.Value);
            }

            this.Add(chart);
        }

        /// <summary>
        ///  Generate MS Chart Line chart 
        /// </summary>
        /// <param name="dt"> Data Sources </param>
        /// <param name="xfield">X Field axis to be displayed </param>
        /// <param name="xdesc">X Axis to display the text field </param>
        /// <param name="yfields">Y Axis project to be analyzed 
        /// <remarks>Dictionary&lt;string, string&gt;
        /// <para>Key. Field bound </para>
        /// <para>Value. Display name field is bound </para>
        /// </remarks></param>
        /// <param name="title"> Chart Title </param>
        /// <param name="chartWidth"> Width of a single chart </param>
        /// <param name="chartHeight"> The height of a single chart </param>
        public void GenerateLineChart(DataTable dt, string xfield, string xdesc, Dictionary<string, string> yfields, string title, int chartWidth = 300, int chartHeight = 300)
        {
            var legendName = "MainLegend_";
            var seriesName = "Series_";

            var chart = new Chart();
            chart.ID = "Chart_" + xfield;
            chart.Width = new Unit(chartWidth);
            chart.Height = new Unit(chartHeight);
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BackColor = Color.FromArgb(224, 236, 255);// Color.FromArgb(243, 223, 193);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.BorderlineColor = Color.FromArgb(149, 184, 231);//Color.FromArgb(181, 64, 1);
            chart.BorderSkin = new BorderSkin { SkinStyle = BorderSkinStyle.Emboss };

            var chartTitle = chart.Titles.Add(title + " - " + xdesc + " -  Line chart ");
            chartTitle.Font = new Font(new FontFamily(" Times New Roman "), 10.5f, FontStyle.Bold);
            chartTitle.ShadowColor = Color.FromArgb(32, 0, 0, 0);
            chartTitle.ShadowOffset = 3;
            chartTitle.ForeColor = Color.FromArgb(26, 59, 105);
            var legend = chart.Legends.Add(legendName + xfield);
            legend.Docking = Docking.Bottom;
            legend.TitleFont = new Font(new FontFamily("Microsoft Sans Serif"), 8, FontStyle.Bold);
            legend.BackColor = Color.Transparent;
            legend.Font = new Font(new FontFamily("Trebuchet MS"), 8.25f, FontStyle.Bold);
            legend.IsTextAutoFit = true;

            Series series = null;
            ChartArea chartArea = null;
            var groups = new Dictionary<string, List<DataRow>>();

            foreach (DataRow dr in dt.Rows)
            {
                if (groups.ContainsKey(dr[xfield].ToString()) == false)
                {
                    groups.Add(dr[xfield].ToString(), new List<DataRow> { dr });
                }
                else
                {
                    groups[dr[xfield].ToString()].Add(dr);
                }
            }

            foreach (var yfield in yfields)
            {
                chartArea = chart.ChartAreas.Add("ChartArea_" + yfield.Key);
                chartArea.BorderColor = Color.FromArgb(64, 64, 64, 64);
                chartArea.BackSecondaryColor = Color.White;
                chartArea.BackColor = Color.OldLace;
                chartArea.ShadowColor = Color.Transparent;
                chartArea.BackGradientStyle = GradientStyle.TopBottom;
                chartArea.Area3DStyle.Rotation = 10;
                chartArea.Area3DStyle.Perspective = 10;
                chartArea.Area3DStyle.Inclination = 15;
                chartArea.Area3DStyle.IsRightAngleAxes = false;
                chartArea.Area3DStyle.WallWidth = 0;
                chartArea.Area3DStyle.IsClustered = false;
                chartArea.AxisY.LineColor = Color.FromArgb(64, 64, 64, 64);
                chartArea.AxisY.LabelAutoFitMaxFontSize = 8;
                chartArea.AxisY.LabelStyle.Font = new Font(new FontFamily("Trebuchet MS"), 8.25f, FontStyle.Bold);
                chartArea.AxisY.LabelStyle.IsEndLabelVisible = false;
                chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
                //chartArea.AxisX.Title = xdesc;
                //chartArea.AxisX.TitleAlignment = StringAlignment.Far;
                //chartArea.AxisX.TitleFont = new Font(new FontFamily(" Times New Roman "), 9f, FontStyle.Bold);
                chartArea.AxisY.Title = yfield.Value;
                chartArea.AxisY.TitleAlignment = StringAlignment.Far;
                chartArea.AxisY.TitleFont = new Font(new FontFamily(" Times New Roman "), 9f, FontStyle.Bold);

                foreach (var de in groups)
                {
                    series = chart.Series.Add(seriesName + yfield.Key + "_" + de.Key);
                    series.Legend = legend.Name;
                    series.LegendText = de.Key;
                    series.IsVisibleInLegend = chart.ChartAreas.Count < 2;
                    series.ChartArea = chartArea.Name;
                    series.ChartType = SeriesChartType.Line;
                    series.BorderColor = Color.FromArgb(180, 26, 59, 105);
                    series.IsValueShownAsLabel = true;
                    series.MarkerStyle = MarkerStyle.Circle;

                    foreach (var dr in de.Value)
                    {
                        series.Points.AddY(dr[yfield.Key]);
                    }
                }
            }

            this.Add(chart);
        }

        public void BindMenu_Small(string enumKey, string url, string selecVal, bool IsShowAll)
        {
            SysEnums ses = new SysEnums(enumKey);
            this.Add("<Table >");
            this.AddTR();
            if (IsShowAll)
            {
                if (selecVal == "all")
                    this.Add("<TD style='font-size:12px; font-weight:bolder; ' background=Enum.gif  width='126' height='36' align=center  > <b> Whole </A> </TD>");
                else
                    this.Add("<TD style='font-size:12px; font-weight:bolder; ' background=Enum.gif  width='126' height='36' align=center  ><A href='" + url.Replace("@" + enumKey, "all") + "' > Whole </A> </TD>");
            }

            foreach (SysEnum se in ses)
            {
                if (se.IntKey.ToString() == selecVal)
                    this.Add("<TD style='font-size:12px; font-weight:bolder;  '  background=Enum.gif width='126' height='36' align=center ><b>" + se.Lab + "</b></TD>");
                else
                    this.Add("<TD style='font-size:12px; font-weight:bolder; ' background=Enum.gif  width='126' height='36' align=center  ><A href='" + url.Replace("@" + enumKey, se.IntKey.ToString()) + "' >" + se.Lab + "</A> </TD>");
            }

            this.AddTREnd();
            this.AddTableEnd();
        }
        public void BindMenu(string enumKey, string url, string selecVal, bool IsShowAll, string imgPath, string newStr)
        {
            SysEnums ses = new SysEnums(enumKey);
            this.Add("<Table >");
            this.AddTR();
            if (newStr != null)
            {
                this.Add("<TD style='font-size:12px; font-weight:bolder; ' background=Enum.gif  width='126' height='36' align=center  >" + newStr + "</TD>");
            }

            if (IsShowAll)
            {
                if (selecVal == "all")
                    this.Add("<TD style='font-size:12px; font-weight:bolder; ' background=Enum.gif  width='126' height='36' align=center  ><img src='" + imgPath + "all.gif' border=0 /> Whole </TD>");
                else
                    this.Add("<TD style='font-size:12px; font-weight:bolder; ' background=Enum.gif  width='126' height='36' align=center  ><A href='" + url.Replace("@" + enumKey, "all") + "' ><img src='" + imgPath + "all.gif' border=0 /> Whole </A> </TD>");
            }

            foreach (SysEnum se in ses)
            {
                if (se.IntKey.ToString() == selecVal)
                    this.Add("<TD style='font-size:12px; font-weight:bolder;  '  background=Enum.gif width='126' height='36' align=center ><b><img src='" + imgPath + se.IntKey + ".gif' border=0 />" + se.Lab + "</b></TD>");
                else
                    this.Add("<TD style='font-size:12px; font-weight:bolder; ' background=Enum.gif  width='126' height='36' align=center  ><A href='" + url.Replace("@" + enumKey, se.IntKey.ToString()) + "' ><img src='" + imgPath + se.IntKey + ".gif' border=0 />" + se.Lab + "</A> </TD>");
            }

            this.AddTREnd();
            this.AddTableEnd();
        }

        public void BindMenu(string enumKey, string url, string selecVal, bool IsShowAll)
        {
            SysEnums ses = new SysEnums(enumKey);
            this.Add("<Table >");
            this.AddTR();
            if (IsShowAll)
            {
                if (selecVal == "all")
                    this.Add("<TD style='font-size:12px; font-weight:bolder; ' background=Enum.gif  width='126' height='36' align=center  > <b> Whole </A> </TD>");
                else
                    this.Add("<TD style='font-size:12px; font-weight:bolder; ' background=Enum.gif  width='126' height='36' align=center  ><A href='" + url.Replace("@" + enumKey, "all") + "' > Whole </A> </TD>");
            }

            foreach (SysEnum se in ses)
            {
                if (se.IntKey.ToString() == selecVal)
                    this.Add("<TD style='font-size:12px; font-weight:bolder;  '  background=Enum.gif width='126' height='36' align=center ><b>" + se.Lab + "</b></TD>");
                else
                    this.Add("<TD style='font-size:12px; font-weight:bolder; ' background=Enum.gif  width='126' height='36' align=center  ><A href='" + url.Replace("@" + enumKey, se.IntKey.ToString()) + "' >" + se.Lab + "</A> </TD>");
            }

            this.AddTREnd();
            this.AddTableEnd();
        }

        public void BindMenuList(string enumKey, string url, string selecVal)
        {
            SysEnums ses = new SysEnums(enumKey);
            this.Add("<Table >");
            foreach (SysEnum se in ses)
            {
                this.AddTR();
                if (se.IntKey.ToString() == selecVal)
                    this.Add("<TD style='font-size:12px; font-weight:bolder;'  background=Enum.gif width='126' height='36' align=center ><b>" + se.Lab + "</b></TD>");
                else
                    this.Add("<TD style='font-size:12px; font-weight:bolder;' background=Enum.gif  width='126' height='36' align=center  ><A href='" + url.Replace("@" + enumKey, se.IntKey.ToString()) + "' >" + se.Lab + "</A> </TD>");
                this.AddTREnd();
            }
            this.AddTableEnd();
        }

        //		public void BindXmlEns(XmlEns ens)
        public void BindXmlEns(XmlEns ens)
        {
            this.Clear();
            this.AddTable();

            XmlEn myen = ens[0];
            this.Add("<TR>");
            foreach (string key in myen.Row.Keys)
                this.Add("<TD class='Title' >" + key + "</TD>");
            this.AddTREnd();

            foreach (XmlEn en in ens)
            {
                this.Add("<TR onmouseover='TROver(this)' onmouseout='TROut(this)' >");
                foreach (string key in en.Row.Keys)
                    this.AddTD(en.GetValStringByKey(key));
                this.AddTREnd();
            }
            this.Add("</Table>");

        }
        // public void GenerOutlookMenuV2(string cate)
        public void GenerOutlookMenuV2(string cate)
        {
            if (cate == null)
                cate = "01";

            this.Controls.Clear();
            DataSet ds = new DataSet();
            ds.ReadXml(SystemConfig.PathOfXML + "Menu.xml");
            DataTable dt = ds.Tables[0];
            DataTable dtl = dt.Clone();
            DataTable dtCate = dt.Clone();

            //DataTable dtl = dt.Clone();
            foreach (DataRow dr in dt.Rows)
            {
                string ForUser = dr["ForUser"].ToString().Trim();
                switch (ForUser)
                {
                    case "SysAdmin":
                        //if (WebUser.HisUserType != UserType.SysAdmin)
                        //    continue;
                        break;
                    case "AppAdmin":
                        //if (WebUser.HisUserType == UserType.AppAdmin
                        //    || WebUser.HisUserType == UserType.SysAdmin)
                        //{
                        //}
                        //else
                        //    continue;
                        break;
                    default:
                        break;
                }
                string no = dr["No"].ToString().Trim();
                if (no.Trim().Length == 2)
                {
                    DataRow dr2 = dtCate.NewRow();
                    dr2["No"] = dr["No"];
                    dr2["Name"] = dr[BP.Web.WebUser.SysLang];
                    dr2["Url"] = dr["Url"];
                    dr2["Desc"] = dr["Desc"];
                    dr2["Img"] = dr["Img"];
                    dtCate.Rows.Add(dr2);
                    continue;
                }

                if (no.Substring(0, 2) == cate)
                {
                    DataRow dr1 = dtl.NewRow();
                    dr1["No"] = dr["No"];
                    dr1["Name"] = dr[BP.Web.WebUser.SysLang];
                    dr1["Url"] = dr["Url"];
                    dr1["Desc"] = dr["Desc"];
                    dr1["Img"] = dr["Img"];
                    dtl.Rows.Add(dr1);
                }
            }


            this.Add("<TABLE   class='MainTable'  >");

            int i = 0;
            foreach (DataRow dr in dtCate.Rows)
            {
                i++;
                string no = dr["No"].ToString();
                string name = dr[BP.Web.WebUser.SysLang].ToString();
                string url = dr["Url"].ToString();
                string img = dr["Img"].ToString();
                string desc = dr["Desc"].ToString(); // Description Data 

                if (img.Trim().Length != 5)
                    name = "<img src='" + img + "' border=0 />" + name;

                string srcp = "window.location.href='LeftOutlook.aspx?cate=" + no + "'";
                /* He is a directory data .*/
                if (cate == no)
                {
                    /*  To choose his current .*/
                    this.Add("<TR  >");
                    this.Add("<TD class='TDM_Selected' nowrap=true title='" + dr["DESC"].ToString() + "' ><b>" + name + "</b></TD>");
                    this.AddTREnd();

                    /* If you have to choose the current menu .*/
                    this.Add("<TR height='100%' >");
                    this.Add("<TD calss='TDItemTable'  height='100%'  >");
                    this.Add("<Table   class='ItemTable'  cellpadding='0' cellspacing='0' style='border-collapse: collapse' >");
                    foreach (DataRow itemdr in dtl.Rows)
                    {
                        string no1 = itemdr["No"].ToString();
                        string name1 = itemdr[BP.Web.WebUser.SysLang].ToString();
                        string url1 = itemdr["Url"].ToString();
                        string img1 = itemdr["Img"].ToString();
                        string desc1 = itemdr["Desc"].ToString(); // Description Data 

                        if (img1.Trim().Length != 5)
                            name1 = "<img src='" + img1 + "' border=0 />" + name1;

                        this.Add("<TR  >");
                        this.Add("<TD onclick=\"Javascript:WinOpen('" + url1 + "','mainfrm' )\" onmouseover=\"javascript:ItemOver(this);\" onmouseout=\"javascript:ItemOut(this);\" class='Item' title='" + desc1 + "'  >");
                        this.Add(name1);
                        this.Add("</TD>");
                        this.AddTREnd();
                    }

                    this.Add("</Table>");
                    this.Add("</TD>");
                    this.AddTREnd();
                }
                else
                {
                    this.Add("<TR >");
                    this.Add("<TD class='TDM' nowrap=true title='" + dr["DESC"].ToString() + "' onclick=\"" + srcp + "\" >" + name + "</TD>");
                    this.AddTREnd();
                }
            }

            this.Add("</TABLE>");
        }
        //		public void ClearViewState()
        public void ClearViewState()
        {
            this.ViewState.Clear();
        }
        //		public void GenerOutlookMenuV2()
        public void GenerOutlookMenuV2()
        {
            this.Controls.Clear();
            DataSet ds = new DataSet();
            ds.ReadXml(SystemConfig.PathOfXML + "MenuMain.xml");
            DataTable dt = ds.Tables[0];

            this.Add("<TABLE border=-1 class='MainTable'  >");
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                i++;
                string id = "Img" + i.ToString();

                string file = dr["File"].ToString();
                string ImgOut = dr["Out"].ToString();
                string ImgOn = dr["On"].ToString();
                string Name = "&nbsp;" + dr["Name"].ToString();


                string srcp = "window.location.href='LeftOutlook.aspx?xml=" + file + "'";



                this.Add("<TR   >");
                //this.Add("<TD class='TDL'  ><Img src='./ImgOutlook/panel_left_r.gif' border=0 width=1% > </TD>");
                this.Add("<TD class='TDM' nowrap=true title='" + dr["DESC"].ToString() + "' onclick=\"" + srcp + "\" >" + Name + "</TD>");
                //this.Add("<TD class='TDR' > </TD>");
                this.AddTREnd();



                /* If you have to choose the current menu .*/
                this.Add("<TR  >");
                //this.Add("<TD ></TD>");
                this.Add("<TD calss='TDItemTable' >");

                this.Add("<Table   class='ItemTable'  cellpadding='0' cellspacing='0' style='border-collapse: collapse' >");
                ds.Tables.Clear();
                ds.ReadXml(SystemConfig.PathOfXML + file);
                DataTable items = ds.Tables["Item"];
                foreach (DataRow itemdr in items.Rows)
                {
                    string itemUrl = itemdr["URL"].ToString();
                    string itemName = itemdr["Name"].ToString();
                    string ICON = itemdr["ICON"].ToString();
                    string Desc = itemdr["Desc"].ToString();


                    this.Add("<TR  >");
                    //this.Add("<TD  nowrap=true title='"+itemdr["DESC"].ToString()+"'  >");

                    this.Add("<TD onclick=\"Javascript:WinOpen('" + itemUrl + "','mainfrm' )\" onmouseover=\"javascript:ItemOver(this);\" onmouseout=\"javascript:ItemOut(this);\" class='Item' title='" + itemdr["DESC"].ToString() + "'  >");
                    this.Add(itemName);
                    //this.Add("<img src='"+ImgOn+"' id='"+id+"' onclick=\"javascript:"+id+".src='"+ImgOut+"'; TDClick( '"+this.Request.ApplicationPath+"','"+file+"', '"+ ImgOn +"'); \"  onmouseover=\"javascript:"+id+".src='"+ImgOut+"';\"  onmouseout=\"javascript: "+id+".src='"+ImgOn+"'; \" />" );
                    this.Add("</TD>");
                    this.AddTREnd();

                }
                this.Add("</Table>");

                this.Add("</TD>");
                //this.Add("<TD  ></TD>");
                this.AddTREnd();

            }


            this.Add("</TABLE>");

        }

        //		public void  ShowTableGroupEns( DataTable dt, Map map, int top,string url,bool isShowNoCol)
        public void ShowTableGroupEns(DataTable dt, Map map, int top, string url, bool isShowNoCol)
        {
            string str = "";
            str += "<Table style='border-collapse: collapse' bordercolor='#111111' >";
            str += "<TR>";
            str += "  <TD warp=false class='Title' nowrap >";
            str += "ID";
            str += "  </TD>";
            foreach (Attr attr in map.Attrs)
            {
                if (attr.Field == null && (attr.MyFieldType == FieldType.Enum || attr.MyFieldType == FieldType.PKEnum))
                    continue;

                if (attr.MyFieldType == FieldType.RefText || attr.MyFieldType == FieldType.Normal)
                {
                    str += "  <TD warp=false class='Title' nowrap >";
                    str += attr.Desc;
                    str += "  </TD>";
                }
                else
                {
                    if (isShowNoCol)
                    {
                        str += "  <TD warp=false class='Title' nowrap >";
                        str += attr.Desc;
                        str += "  </TD>";
                    }
                }

            }
            str += "</TR>";

            int idx = 0;
            string myurl = "";
            foreach (DataRow dr in dt.Rows)
            {
                idx++;
                str += "<TR class='TR' onmouseover='TROver(this)' onmouseout='TROut(this)' >";
                str += "  <TD class='Idx' nowrap >";
                str += idx.ToString();
                str += "  </TD>";
                myurl = "";
                foreach (Attr attr in map.Attrs)
                {
                    if (attr.Field == null && (attr.MyFieldType == FieldType.Enum || attr.MyFieldType == FieldType.PKEnum))
                        continue;

                    if (attr.MyFieldType == FieldType.Normal)
                    {
                        str += "  <TD class='TDNum' nowrap >";
                        str += "<a href=\"javascript:WinOpen('" + url + myurl + "')\"  >" + dr[attr.Field] + "</a>";
                        str += "  </TD>";
                    }
                    else
                    {
                        if (attr.MyFieldType == FieldType.RefText)
                        {
                            str += "  <TD class='TD' nowrap >";
                            str += dr[attr.Key];
                            str += "  </TD>";
                        }
                        else
                        {
                            myurl += "&" + attr.Key + "=" + dr[attr.Field];
                            if (isShowNoCol)
                            {
                                str += "  <TD class='TD' nowrap >";
                                str += dr[attr.Field];
                                str += "  </TD>";
                            }
                        }
                    }
                }
                str += "</TR>";

                if (idx == top)
                    break;
            }

            str += "</Table>";
            this.Add(str);

        }


        //		public void  ShowTable( DataTable dt, Map map)
        public void ShowTable(DataTable dt, Map map)
        {
            string str = "";
            str += "<Table class='Table'  >";
            str += "<TR>";
            str += "  <TD warp=false class='Title' nowrap >";
            str += "ID";
            str += "  </TD>";
            foreach (Attr attr in map.Attrs)
            {
                if (attr.Field == null)
                    continue;

                str += "  <TD warp=false class='Title' nowrap >";
                str += attr.Desc;
                str += "  </TD>";
            }
            str += "</TR>";

            int idx = 0;
            foreach (DataRow dr in dt.Rows)
            {
                idx++;

                str += "<TR class='TR' onmouseover='TROver(this)' onmouseout='TROut(this)' >";
                str += "  <TD class='TDLeft' nowrap >";
                str += idx.ToString();
                str += "  </TD>";
                foreach (Attr attr in map.Attrs)
                {
                    if (attr.UIContralType == UIContralType.DDL)
                        continue;

                    str += "  <TD class='TD' nowrap >";
                    if (attr.MyFieldType == FieldType.RefText)
                        str += dr[attr.Key];
                    else
                        str += dr[attr.Field];

                    str += "  </TD>";
                }
                str += "</TR>";
            }

            str += "</Table>";
            this.Add(str);

        }
        public void ShowHidenMsg(string id, string title, string msg, bool isShowHelpIcon)
        {

            string appPath = this.Request.ApplicationPath;
            if (isShowHelpIcon)
                title = "<img src='../Img/btn/help.gif' border=0 />" + title;


            msg = "<table class=Table id='t" + id + "' border=0 ><TR Class=TR ><TD class=TD  bgcolor=#FFFFFF >" + msg + "</TD></TR></Table>";

            string str = "<A onclick='show" + id + "();' style='cursor:hand' > <FONT color='#008000' style='font-size:12px'  ><b>" + title + "</b><img src='../Img/downUp.gif' id=Img" + id + "' ></FONT></A><span id='" + id + "'></span>";

            string script = "\n <script language='javascript'> var mode; mode=1; ";
            script += "\n function show" + id + "() {";

            script += "\n  if (mode==0) ";
            script += "\n  {  \n";
            script += id + ".innerHTML='' \n";
            //script += "Img"+id + ".Src='/imgages/Up.gif' \n";

            script += "   mode=1 \n";

            script += "  }else{ \n";

            script += id + ".innerHTML=' " + msg + "'\n";
            // script += "Img" + id + ".Src='/imgages/Down.gif' \n";

            script += "   mode=0 \n";
            script += "  }\n";
            script += "}\n";
            script += "</script>\n";

            this.Add(str);
            this.Add(script);

        }

        public void ShowTable(string title, DataTable dt, DataTable sDT, string color, string refF)
        {

            this.AddTable();
            if (title != null)
                this.AddCaptionLeft(title);

            this.AddTR();
            this.AddTDTitle("No.");
            foreach (DataColumn dc in dt.Columns)
                this.AddTDTitle(dc.ColumnName);
            this.AddTREnd();
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                i++;
                string bg = "";
                foreach (DataRow mydr in sDT.Rows)
                {
                    if (mydr[refF].ToString() == dr[refF].ToString())
                    {
                        bg = "bgcolor=" + color;
                        break;
                    }
                }

                this.AddTR(bg);

                this.AddTDIdx(i);
                foreach (DataColumn dc in dt.Columns)
                {
                    this.AddTD(dr[dc.ColumnName].ToString());
                }
                this.AddTREnd();
            }
            this.AddTableEnd();
        }

        //		public void  ShowTable( DataTable dt)
        public void ShowTable(string title, DataTable dt, bool is_TR_TX)
        {

            this.AddTable();
            if (title != null)
                this.AddCaptionLeft(title);

            this.AddTR();
            this.AddTDTitle("No.");
            foreach (DataColumn dc in dt.Columns)
                this.AddTDTitle(dc.ColumnName);
            this.AddTREnd();
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                i++;
                if (is_TR_TX)
                    this.AddTRTX();
                else
                    this.AddTR();

                this.AddTDIdx(i);
                foreach (DataColumn dc in dt.Columns)
                {
                    this.AddTD(dr[dc.ColumnName].ToString());
                }
                this.AddTREnd();
            }
            this.AddTableEnd();
        }
        //		public void GenerOutlookMenu(string xmlFile)
        public void GenerOutlookMenu(string xmlFile)
        {
            this.Controls.Clear();
            DataSet ds = new DataSet();
            ds.ReadXml(SystemConfig.PathOfXML + "MenuMain.xml");
            DataTable dt = ds.Tables[0];


            if (xmlFile == null || xmlFile == "ss")  // If you do not find it , The first one is set .
                this.Add("<TABLE border=-1 class='MainTable'  >");
            else
                this.Add("<TABLE border=-1 class='MainTable'  height=100%  >");


            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                i++;
                string id = "Img" + i.ToString();

                string file = dr["File"].ToString();
                string ImgOut = dr["Out"].ToString();
                string ImgOn = dr["On"].ToString();
                string Name = "&nbsp;" + dr["Name"].ToString();

                string srcp = "window.location.href='LeftOutlook.aspx?xml=" + file + "'";
                if (file == xmlFile)
                {
                    this.Add("<TR   >");
                    this.Add("<TD class='TDM_Selected' nowrap=true title='" + dr["DESC"].ToString() + "' ><b>" + Name + "</b></TD>");
                    this.AddTREnd();
                }
                else
                {
                    this.Add("<TR   >");
                    this.Add("<TD class='TDM' nowrap=true title='" + dr["DESC"].ToString() + "' onclick=\"" + srcp + "\" >" + Name + "</TD>");
                    this.AddTREnd();
                }


                if (xmlFile == "RptTemplate" && file == "RptTemplate")
                {
                    /* If you have to choose the current menu .*/
                    this.Add("<TR  >");
                    this.Add("<TD calss='TDItemTable' >");
                    this.Add("<Table   class='ItemTable'  cellpadding='0' cellspacing='0' style='border-collapse: collapse' >");
                    this.Add("</Table>");
                    this.Add("</TD>");
                    this.AddTREnd();
                }
                else if (file == xmlFile)
                {
                    /* If you have to choose the current menu .*/
                    this.Add("<TR  >");
                    this.Add("<TD calss='TDItemTable' >");
                    this.Add("<Table   class='ItemTable'  cellpadding='0' cellspacing='0' style='border-collapse: collapse' >");
                    ds.Tables.Clear();
                    ds.ReadXml(SystemConfig.PathOfXML + file);
                    DataTable items = ds.Tables["Item"];
                    foreach (DataRow itemdr in items.Rows)
                    {
                        string itemUrl = itemdr["URL"].ToString();
                        string itemName = itemdr["Name"].ToString();
                        string ICON = itemdr["ICON"].ToString();
                        string Desc = itemdr["Desc"].ToString();

                        this.Add("<TR  >");
                        //this.Add("<TD  nowrap=true title='"+itemdr["DESC"].ToString()+"'  >");

                        this.Add("<TD onclick=\"Javascript:WinOpen('" + itemUrl + "','mainfrm' )\" onmouseover=\"javascript:ItemOver(this);\" onmouseout=\"javascript:ItemOut(this);\" class='Item' title='" + itemdr["DESC"].ToString() + "'  >");
                        this.Add(itemName);
                        //this.Add("<img src='"+ImgOn+"' id='"+id+"' onclick=\"javascript:"+id+".src='"+ImgOut+"'; TDClick( '"+this.Request.ApplicationPath+"','"+file+"', '"+ ImgOn +"'); \"  onmouseover=\"javascript:"+id+".src='"+ImgOut+"';\"  onmouseout=\"javascript: "+id+".src='"+ImgOn+"'; \" />" );
                        this.Add("</TD>");
                        this.AddTREnd();
                    }


                    this.Add("</Table>");
                    this.Add("</TD>");
                    //this.Add("<TD  ></TD>");
                    this.AddTREnd();
                }
            }


            this.Add("</TABLE>");

        }
        //		public void GenerOutlookMenu()
        public void GenerOutlookMenu()
        {
            this.Controls.Clear();
            DataSet ds = new DataSet();
            ds.ReadXml(SystemConfig.PathOfXML + "MenuMain.xml");
            DataTable dt = ds.Tables[0];

            this.Add("<TABLE border=-1 class='MainTable'  >");
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                i++;
                string id = "Img" + i.ToString();

                string file = dr["File"].ToString();
                string ImgOut = dr["Out"].ToString();
                string ImgOn = dr["On"].ToString();
                string Name = "&nbsp;" + dr["Name"].ToString();


                string srcp = "window.location.href='LeftOutlook.aspx?xml=" + file + "'";



                this.Add("<TR   >");
                //this.Add("<TD class='TDL'  ><Img src='./ImgOutlook/panel_left_r.gif' border=0 width=1% > </TD>");
                this.Add("<TD class='TDM' nowrap=true title='" + dr["DESC"].ToString() + "' onclick=\"" + srcp + "\" >" + Name + "</TD>");
                //this.Add("<TD class='TDR' > </TD>");
                this.AddTREnd();



                /* If you have to choose the current menu .*/
                this.Add("<TR  >");
                //this.Add("<TD ></TD>");
                this.Add("<TD calss='TDItemTable' >");

                this.Add("<Table   class='ItemTable'  cellpadding='0' cellspacing='0' style='border-collapse: collapse' >");
                ds.Tables.Clear();
                ds.ReadXml(SystemConfig.PathOfXML + file);
                DataTable items = ds.Tables["Item"];
                foreach (DataRow itemdr in items.Rows)
                {
                    string itemUrl = itemdr["URL"].ToString();
                    string itemName = itemdr["Name"].ToString();
                    string ICON = itemdr["ICON"].ToString();
                    string Desc = itemdr["Desc"].ToString();


                    this.Add("<TR  >");
                    //this.Add("<TD  nowrap=true title='"+itemdr["DESC"].ToString()+"'  >");

                    this.Add("<TD onclick=\"Javascript:WinOpen('" + itemUrl + "','mainfrm' )\" onmouseover=\"javascript:ItemOver(this);\" onmouseout=\"javascript:ItemOut(this);\" class='Item' title='" + itemdr["DESC"].ToString() + "'  >");
                    this.Add(itemName);
                    //this.Add("<img src='"+ImgOn+"' id='"+id+"' onclick=\"javascript:"+id+".src='"+ImgOut+"'; TDClick( '"+this.Request.ApplicationPath+"','"+file+"', '"+ ImgOn +"'); \"  onmouseover=\"javascript:"+id+".src='"+ImgOut+"';\"  onmouseout=\"javascript: "+id+".src='"+ImgOn+"'; \" />" );
                    this.Add("</TD>");
                    this.AddTREnd();

                }
                this.Add("</Table>");

                this.Add("</TD>");
                //this.Add("<TD  ></TD>");
                this.AddTREnd();

            }


            this.Add("</TABLE>");

        }
        //		public void GenerOutlookMenu_Img(string xmlFile)
        public void GenerOutlookMenu_Img(string xmlFile)
        {
            this.Controls.Clear();
            DataSet ds = new DataSet();
            ds.ReadXml(SystemConfig.PathOfXML + "MenuMain.xml");
            DataTable dt = ds.Tables[0];
            if (xmlFile == null || xmlFile == "")  // If you do not find it , The first one is set .
                xmlFile = dt.Rows[0]["File"].ToString();



            this.Add("<TABLE border=0 class='MainTable' >");

            //e.Item.Attributes.Add("onmouseover","DGTROn"+WebUser.Style+"(this)");
            //e.Item.Attributes.Add("onmouseout","DGTROut"+WebUser.Style+"(this)");

            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                i++;
                string id = "Img" + i.ToString();

                string file = dr["File"].ToString();
                string ImgOut = dr["Out"].ToString();
                string ImgOn = dr["On"].ToString();

                // window.location.href='MyDay.aspx?RefDate='+date;

                if (file == xmlFile)
                {
                    this.Add("<TR>");
                    this.Add("<TD   nowrap=true title='" + dr["DESC"].ToString() + "'  >");
                    this.Add("<img src='" + ImgOn + "' id='" + id + "' />");
                    this.Add("</TD>");
                    this.AddTREnd();
                }
                else
                {
                    string srcp = "window.location.href='LeftOutlook.aspx?xml=" + file + "'";

                    this.Add("<TR>");
                    this.Add("<TD   nowrap=true title='" + dr["DESC"].ToString() + "'  >");
                    this.Add("<img src='" + ImgOn + "' id='" + id + "' onclick=\"javascript:" + id + ".src='" + ImgOut + "'; " + srcp + " ; ; \"  onmouseover=\"javascript:" + id + ".src='" + ImgOut + "';\"  onmouseout=\"javascript: " + id + ".src='" + ImgOn + "'; \" />");
                    this.Add("</TD>");
                    this.AddTREnd();
                }

                if (file == xmlFile)
                {
                    /* If you have to choose the current menu .*/
                    this.Add("<TR>");
                    this.Add("<TD>");

                    this.Add("<Table border=0  class='ItemTable' >");
                    ds.Tables.Clear();
                    ds.ReadXml(SystemConfig.PathOfXML + file);
                    DataTable items = ds.Tables["Item"];
                    foreach (DataRow itemdr in items.Rows)
                    {
                        string itemUrl = itemdr["URL"].ToString();
                        string itemName = itemdr["Name"].ToString();
                        string ICON = itemdr["ICON"].ToString();
                        string Desc = itemdr["Desc"].ToString();

                        this.Add("<TR>");
                        this.Add("<TD   nowrap=true title='" + itemdr["DESC"].ToString() + "'  >");
                        this.Add("<a href='" + itemUrl + "' target='mainfrm' class='Link' >" + itemName + "</a>");
                        //this.Add("<img src='"+ImgOn+"' id='"+id+"' onclick=\"javascript:"+id+".src='"+ImgOut+"'; TDClick( '"+this.Request.ApplicationPath+"','"+file+"', '"+ ImgOn +"'); \"  onmouseover=\"javascript:"+id+".src='"+ImgOut+"';\"  onmouseout=\"javascript: "+id+".src='"+ImgOn+"'; \" />" );
                        this.Add("</TD>");
                        this.AddTREnd();
                    }
                    this.Add("</Table>");

                    this.Add("</TD>");
                    this.AddTREnd();
                }
            }
            this.Add("</TABLE>");
        }
        //		public void BindSystems()
        public void BindSystems()
        {
            this.AddTable();
            this.Add("<TR>");
            this.AddTDTitle(" System Number ");
            this.AddTDTitle(" Name ");
            this.AddTDTitle(" Version ");
            this.AddTDTitle(" Release date ");
            this.AddTREnd();
            //BPSystems ens = new BPSystems();
            //ens.RetrieveAll();
            //foreach (BPSystem en in ens)
            //{
            //    this.Add("<TR  onmouseover='TROver(this)' onmouseout='TROut(this)' >");
            //    this.AddTD(en.No);
            //    if (en.IsOk && SystemConfig.SysNo != en.No)
            //        this.AddTD("<a href='" + en.URL + "&Token=" + WebUser.Token + "&No=" + WebUser.No + "' target='_parent' >" + en.Name + "</a> ");
            //    else
            //        this.AddTD(en.Name);
            //    this.AddTD(en.Ver);
            //    this.AddTD(en.IssueDate);
            //    this.AddTREnd();
            //}
            this.Add("</Table>\n");
        }
        //		public void BindWel()
        public void BindWel()
        {
            this.Controls.Clear();
            //this.Add("<font color='#000000' size=2 > Welcome :"+WebUser.Name+", Department :"+WebUser.HisEmp.FK_DeptText+", Post :"+WebUser.HisEmp.FK_StationText+".</font>");
        }
        //		public void BindMsgInfo(string msg)
        public void BindMsgInfo(string msg)
        {
            this.Controls.Clear();
            this.Add("<Table  border='1' cellpadding='0' cellspacing='0' style='border-collapse: collapse' >");
            this.Add("<Caption align=left ><b> Message </b></Caption>");
            this.Add("<TR>");
            this.Add("<TD  bgcolor='#FFFF00' >" + msg + "</TD>");
            this.AddTREnd();
            this.Add("</Table>");
        }
        //		public void BindMsgWarning(string msg)
        public void BindMsgWarning(string msg)
        {
            this.Controls.Clear();
            this.Add("<font color='#000000' size=40 >" + msg + "</font>");
        }
        //		public void GenerMenuMain()
        public void GenerMenuMain()
        {
            this.Controls.Clear();
            DataSet ds = new DataSet();
            ds.ReadXml(SystemConfig.PathOfXML + "MenuMain.xml");
            DataTable dt = ds.Tables[0];

            this.Add("<TABLE border=0>");
            this.Add("<TR>");
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                i++;
                string id = "Img" + i.ToString();

                string file = dr["File"].ToString();
                string ImgOut = dr["Out"].ToString();
                string ImgOn = dr["On"].ToString();


                this.Add("<TD   nowrap=true title='" + dr["DESC"].ToString() + "'  >");

                this.Add("<img src='" + ImgOn + "' id='" + id + "' onclick=\"javascript:" + id + ".src='" + ImgOut + "'; TDClick( '" + this.Request.ApplicationPath + "','" + file + "', '" + ImgOn + "'); \"  onmouseover=\"javascript:" + id + ".src='" + ImgOut + "';\"  onmouseout=\"javascript: " + id + ".src='" + ImgOn + "'; \" />");

                this.Add("</TD>");
            }
            this.AddTREnd();


            this.Add("</TABLE>");


        }
        //		public void DataPanel(Entities ens, string ctrlId, string key, ShowWay sh)
        public void DataPanel(Entities ens, string ctrlId, string key, ShowWay sh)
        {
            switch (sh)
            {
                case ShowWay.Cards:
                    this.DataPanelCards(ens, ctrlId, key, true);
                    break;
                case ShowWay.List:
                    this.DataPanelCards(ens, ctrlId, key, false);
                    break;
                case ShowWay.Dtl:
                    this.DataPanelDtl(ens, ctrlId, key);
                    break;
            }

        }
        //		public void DataPanelDtl(Entities ens, string ctrlId , string colName, string urlAttrKey, string colUrl  )
        public void DataPanelDtl(Entities ens, string ctrlId, string colName, string urlAttrKey, string colUrl)
        {
            this.Controls.Clear();
            Entity myen = ens.GetNewEntity;
            string pk = myen.PK;
            string clName = myen.ToString();
            Attrs attrs = myen.EnMap.Attrs;
            Attrs selectedAttrs = myen.EnMap.GetChoseAttrs(ens);

            string appPath = this.Request.ApplicationPath;
            //  Generation title 
            this.Add("<TABLE  style='border-collapse: collapse' bordercolor='#111111' >");
            this.Add("<TR >");
            this.Add("<TH  nowrap >No.</TH>");
            this.Add("<TH nowrap >" + colName + "</TH>");

            foreach (Attr attrT in selectedAttrs)
            {
                if (attrT.UIVisible == false)
                    continue;

                this.Add("<TD  nowrap >" + attrT.Desc + "</TD>");
            }
            this.AddTREnd();

            int idx = 0;
            string style = WebUser.Style;
            foreach (Entity en in ens)
            {
                #region  Deal with keys
                string url = "";
                foreach (Attr attr in attrs)
                {
                    switch (attr.UIContralType)
                    {
                        case UIContralType.TB:
                            if (attr.IsPK)
                                url += "&" + attr.Key + "=" + en.GetValStringByKey(attr.Key);
                            break;
                        case UIContralType.DDL:
                            url += "&" + attr.Key + "=" + en.GetValStringByKey(attr.Key);
                            break;
                    }
                }
                #endregion

                this.Add("<TR  onmouseover=\"TROver(this,'" + this.Page.Request.ApplicationPath + "','" + ctrlId + "', '" + clName + "', '" + url + "');\" onmouseout='TROut(this)' ondblclick=\" WinOpen('../Comm/RefFunc/UIEn.aspx?EnsName=" + ens.ToString() + "&PK=" + en.GetValByKey(pk) + url + "')\" >");
                idx++;
                this.Add("<TD  class='Idx' nowrap >" + idx + "</TD>");
                this.Add("<TD  class='No'  nowrap ><a href='" + colUrl + en.GetValStringByKey(urlAttrKey) + "' target='_blank'> " + colName + "</a></TD>");

                foreach (Attr attr in selectedAttrs)
                {
                    if (attr.UIVisible == false)
                        continue;

                    if (attr.UIContralType == UIContralType.DDL)
                        this.Add("<TD  nowrap >" + en.GetValRefTextByKey(attr.Key) + "&nbsp;</TD>");
                    else
                    {
                        string str = en.GetValStringByKey(attr.Key);
                        switch (attr.MyDataType)
                        {
                            case DataType.AppBoolean:
                                if (str == "1")
                                    this.AddTD("是&nbsp;");
                                else
                                    this.AddTD("否&nbsp;");
                                break;
                            case DataType.AppDate:
                            case DataType.AppDateTime:
                                this.AddTD(str);
                                break;
                            case DataType.AppString:
                                if (attr.UIHeight != 0)
                                    this.AddTDDoc(str, str);
                                else
                                    this.AddTD(str);
                                break;
                            case DataType.AppDouble:
                            case DataType.AppFloat:
                            case DataType.AppMoney:
                            case DataType.AppRate:
                                this.AddTDNum(str);
                                break;
                            default:
                                throw new Exception("sdfasdfsd");
                        }
                    }
                }
                this.AddTREnd();
            }
            this.Add("</TABLE>");
        }
        public void DataPanelDtlCheckBox(Entities ens)
        {
            this.Controls.Clear();

            Entity myen = ens.GetNewEntity;
            string pk = myen.PK;
            string clName = myen.ToString();
            Attrs attrs = myen.EnMap.Attrs;
            Attrs selectedAttrs = myen.EnMap.GetChoseAttrs(ens);

            BP.Sys.Xml.Searchs cfgs = new BP.Sys.Xml.Searchs();
            cfgs.RetrieveBy(BP.Sys.Xml.SearchAttr.For, ens.ToString());

            //  Generation title 
            this.Add("<table  style=\"width:30%\" >");
            this.AddTR();

            // CheckBox cb = new CheckBox();
            // cb.Text = "序";
            // cb.ID = "CB_Idx";
            //  cb.Attributes["CheckedChanged"] = "javascript:CheckIt(this)";
            // cb.Attributes["CheckedChanged"] = "javascrip:CheckIt(this)";
            //cb.CheckedChanged ["CheckedChanged"] = "javascrip:CheckIt(this)";

            if (ens.Count > 0)
            {
                string str1 = "<INPUT id='checkedAll' onclick='selectAll()' type='checkbox' name='checkedAll'>";
                this.AddTDTitle(str1);
            }
            else
            {
                this.AddTDTitle();
            }

            foreach (Attr attrT in selectedAttrs)
            {
                if (attrT.UIVisible == false)
                    continue;

                if (attrT.Key == "MyNum")
                    continue;

                if (attrT.IsNum && attrT.IsEnum == false && attrT.MyDataType == DataType.AppBoolean == false)
                    this.AddTDTitle("<a href=\"javascript:WinOpen('Group.aspx?EnsName=" + ens.ToString() + "&NumKey=" + attrT.Key + "','sd','800','700');\" >" + attrT.Desc + "</a>");
                else
                    this.AddTDTitle(attrT.Desc);
            }
            this.AddTDTitle();
            this.AddTREnd();

            #region  User interface attribute set 
            BP.Web.Comm.UIRowStyleGlo tableStyle = UIRowStyleGlo.MouseAndAlternately;
            bool IsEnableDouclickGlo = false;
            bool IsEnableRefFunc = false;
            bool IsEnableFocusField = false;
            bool isShowOpenICON = false;
            string FocusField = null;
            int WinCardH = 600;
            int WinCardW = 500;

            try
            {
                tableStyle = (UIRowStyleGlo)ens.GetEnsAppCfgByKeyInt("UIRowStyleGlo"); //  Interface style .
                IsEnableDouclickGlo = ens.GetEnsAppCfgByKeyBoolen("IsEnableDouclickGlo"); //  Whether double-click Enable 
                IsEnableRefFunc = ens.GetEnsAppCfgByKeyBoolen("IsEnableRefFunc"); //  Whether to display related functions .
                IsEnableFocusField = ens.GetEnsAppCfgByKeyBoolen("IsEnableFocusField"); // Whether the focus is enabled field .
                isShowOpenICON = ens.GetEnsAppCfgByKeyBoolen("IsEnableOpenICON"); // Whether to enable  OpenICON .
                FocusField = null;
                if (IsEnableFocusField)
                    FocusField = ens.GetEnsAppCfgByKeyString("FocusField");

                WinCardH = ens.GetEnsAppCfgByKeyInt("WinCardH"); //  Pop-up height c
                WinCardW = ens.GetEnsAppCfgByKeyInt("WinCardW"); //  Pop-up window width 
            }
            catch
            {

            }

            bool isAddTitle = false;  // Whether to display the relevant functions are listed .
            if (isShowOpenICON)
                isAddTitle = true;
            if (IsEnableRefFunc)
                isAddTitle = true;
            #endregion  User interface attribute set 

            bool isRefFunc = true;
            int pageidx = this.PageIdx - 1;
            int idx = SystemConfig.PageSize * pageidx;

            bool is1 = false;
            string urlExt = "";
            foreach (Entity en in ens)
            {
                idx++;

                #region  Deal with keys
                string style = WebUser.Style;
                string url = this.GenerEnUrl(en, attrs);
                #endregion


                urlExt = "\"javascript:ShowEn('./RefFunc/UIEn.aspx?EnsName=" + ens.ToString() + "&PK=" + en.GetValByKey(pk) + url + "', 'cd','" + WinCardH + "','" + WinCardW + "');\"";

                // urlExt = "javascript:ShowEn('./RefFunc/UIEn.aspx?EnsName=" + ens.ToString() + "&PK=" + en.GetValByKey(pk) + url + "', 'cd');";

                switch (tableStyle)
                {
                    case UIRowStyleGlo.None:
                        if (IsEnableDouclickGlo)
                            this.AddTR("ondblclick=" + urlExt);
                        else
                            this.AddTR();
                        break;
                    case UIRowStyleGlo.Mouse:
                        if (IsEnableDouclickGlo)
                            this.AddTRTX("ondblclick=" + urlExt);
                        else
                            this.AddTRTX();
                        break;
                    case UIRowStyleGlo.Alternately:
                    case UIRowStyleGlo.MouseAndAlternately:
                        if (IsEnableDouclickGlo)
                            is1 = this.AddTR(is1, "ondblclick=" + urlExt);
                        else
                            is1 = this.AddTR(is1);
                        break;
                    default:
                        throw new Exception("@ There is no offer .");
                }


                // this.Add("<TR onmouseover=\"TROver(this);\" onmouseout='TROut(this)' ondblclick=\"WinOpen( 'UIEn.aspx?EnsName=" + ens.ToString() + "&PK=" + en.GetValByKey(pk) + url + "', 'cd' )\"   >");
                CheckBox cb = new CheckBox();
                cb.ID = "CB_" + en.PKVal;
                cb.Text = idx.ToString();
                //cb.Attributes["PK"] = en.PKVal;
                this.AddTDIdx(cb);
                string val = "";
                foreach (Attr attr in selectedAttrs)
                {
                    if (attr.UIVisible == false)
                        continue;

                    if (attr.Key == "MyNum")
                        continue;

                    this.DataPanelDtlAdd(en, attr, cfgs, url, urlExt, FocusField);
                }

                if (isRefFunc && IsEnableRefFunc)
                {
                    string str = "";

                    #region  Joined his door   Method 
                    RefMethods myreffuncs = en.EnMap.HisRefMethods;
                    foreach (RefMethod func in myreffuncs)
                    {
                        if (func.Visable == false || func.IsForEns == false)
                            continue;

                        //myurl="../Comm/RefMethod.aspx?Index="+func.Index+"&EnsName="+ens.ToString() ;
                        str += "<A onclick=\"javascript:RefMethod1('" + this.Request.ApplicationPath + "', '" + func.Index + "', '" + func.Warning + "', '" + func.Target + "', '" + ens.ToString() + "','" + url + "') \"  > " + func.GetIcon(this.Request.ApplicationPath) + " " + func.Title + "</A>";
                        // str += "<A onclick=\"javascript:RefMethod1('" + this.Request.ApplicationPath + "', '" + func.Index + "', '" + func.Warning + "', '" + func.Target + "', '" + ens.ToString() + "','" + url + "') \"  > " + func.GetIcon(this.Request.ApplicationPath) + "<font color=blue >" + func.Title + "</font></A>";
                        //this.AddItem(func.Title, "RefMethod('"+func.Index+"', '"+func.Warning+"', '"+func.Target+"', '"+this.EnsName+"')", func.Icon);
                    }
                    #endregion

                    #region  He added detail 
                    EnDtls enDtls = en.EnMap.Dtls;
                    foreach (EnDtl enDtl in enDtls)
                    {
                        str += "[<A onclick=\"javascript:EditDtl1('" + this.Request.ApplicationPath + "', '" + myen.ToString() + "',  '" + enDtl.EnsName + "', '" + enDtl.RefKey + "', '" + url + "&IsShowSum=1')\" >" + enDtl.Desc + "</A>]";
                    }
                    #endregion

                    #region  Join many of the entity editor 
                    AttrsOfOneVSM oneVsM = en.EnMap.AttrsOfOneVSM;
                    foreach (AttrOfOneVSM vsM in oneVsM)
                    {
                        str += "[<A onclick=\"javascript:EditOneVsM1('" + this.Request.ApplicationPath + "','" + en.ToString() + "','" + vsM.EnsOfMM.ToString() + "s','" + vsM.EnsOfMM + "&dt=" + DateTime.Now.ToString("hhss") + "','" + myen.ToString() + "','" + url + "'); return; \" >" + vsM.Desc + "</A>]";
                    }
                    #endregion

                    if (isShowOpenICON)
                        this.Add("<TD class='TD' style='cursor:hand;' nowrap=true  >" + str + " </TD>");
                    else
                        this.Add("<TD class='TD' style='cursor:hand;' nowrap=true  >" + str + " </TD>");
                    // this.Add("<TD class='TD' style='cursor:hand;' nowrap=true  >" + str + " <a href=\"" + urlExt + "\" ><img src='../Img/Btn/Open.gif' border=0/></a></TD>");

                }
                else
                {
                    if (isShowOpenICON)
                        this.AddTD();
                    // this.Add("<TD class='TD' style='cursor:hand;' nowrap=true><a href=\"" + urlExt + "\" ><img src='../Img/Btn/Open.gif' border=0/></a></TD>");
                    else
                        this.AddTD();
                }
                this.AddTREnd();
            }
            this.AddTableEnd();
        }
        //		public void DataPanelDtl(Entities ens, string ctrlId )
        public void DataPanelDtl(Entities ens, string ctrlId)
        {
            this.Controls.Clear();
            Entity myen = ens.GetNewEntity;
            string pk = myen.PK;
            string clName = myen.ToString();

            Attrs attrs = myen.EnMap.Attrs;
            Attrs selectedAttrs = myen.EnMap.GetChoseAttrs(ens);
            BP.Sys.Xml.Searchs cfgs = new BP.Sys.Xml.Searchs();
            cfgs.RetrieveBy(BP.Sys.Xml.SearchAttr.For, ens.ToString());

            //  Generation title 
            this.Add("<Table border='1' width='100%' align=left cellpadding='0' cellspacing='0' style='border-collapse: collapse' bordercolor='#C0C0C0'>");
            // this.AddTable("");
            this.AddTR();
            this.AddTDTitle("No.");
            foreach (Attr attrT in selectedAttrs)
            {
                if (attrT.UIVisible == false)
                    continue;
                if (attrT.Key == "MyNum")
                    continue;
                this.AddTDTitle(attrT.Desc);
            }

            bool isRefFunc = false;

            //if (myen.EnMap.HisRefMethods.CountOfVisable > 0
            //    || myen.EnMap.Dtls.Count > 0 || myen.EnMap.AttrsOfOneVSM.Count > 0)
            //{
            //    isRefFunc = true;
            //}

            isRefFunc = true;

            int pageidx = this.PageIdx - 1;
            int idx = SystemConfig.PageSize * pageidx;
            bool is1 = false;

            #region  User interface attribute set 
            BP.Web.Comm.UIRowStyleGlo tableStyle = UIRowStyleGlo.MouseAndAlternately;
            bool IsEnableDouclickGlo = true;
            bool IsEnableRefFunc = true;
            bool IsEnableFocusField = true;
            bool isShowOpenICON = true;
            string FocusField = null;
            //int WinCardH = 600;
            //int WinCardW = 500;
            int WinCardH = ens.GetEnsAppCfgByKeyInt("WinCardH", 500); //  Pop-up height 
            int WinCardW = ens.GetEnsAppCfgByKeyInt("WinCardW", 820); //  Pop-up window width 
            try
            {
                tableStyle = (UIRowStyleGlo)ens.GetEnsAppCfgByKeyInt("UIRowStyleGlo"); //  Interface style .           
                IsEnableDouclickGlo = ens.GetEnsAppCfgByKeyBoolen("IsEnableDouclickGlo"); //  Whether double-click Enable 
                IsEnableRefFunc = ens.GetEnsAppCfgByKeyBoolen("IsEnableRefFunc"); //  Whether to display related functions .
                IsEnableFocusField = ens.GetEnsAppCfgByKeyBoolen("IsEnableFocusField"); // Whether the focus is enabled field .
                isShowOpenICON = ens.GetEnsAppCfgByKeyBoolen("IsEnableOpenICON"); // Whether to enable  OpenICON .

                FocusField = null;
                if (IsEnableFocusField)
                    FocusField = ens.GetEnsAppCfgByKeyString("FocusField");

            }
            catch
            {
            }

            bool isAddTitle = false;  // Whether to display the relevant functions are listed .
            if (isShowOpenICON)
                isAddTitle = true;
            if (IsEnableRefFunc)
                isAddTitle = true;
            #endregion  User interface attribute set 

            if (isAddTitle)
                this.AddTDTitle();

            this.AddTREnd();

            string urlExt = "";
            foreach (Entity en in ens)
            {
                #region  Deal with keys
                string style = WebUser.Style;
                string url = this.GenerEnUrl(en, attrs);
                #endregion

                urlExt = "\"javascript:ShowEn('../Comm/RefFunc/UIEn.aspx?EnsName=" + ens.ToString() + "&PK=" + en.GetValByKey(pk) + url + "', 'cd','" + WinCardH + "','" + WinCardW + "');\"";
                switch (tableStyle)
                {
                    case UIRowStyleGlo.None:
                        if (IsEnableDouclickGlo)
                            this.AddTR("ondblclick=" + urlExt);
                        else
                            this.AddTR();
                        break;
                    case UIRowStyleGlo.Mouse:
                        if (IsEnableDouclickGlo)
                            this.AddTRTX("ondblclick=" + urlExt);
                        else
                            this.AddTRTX();
                        break;
                    case UIRowStyleGlo.Alternately:
                    case UIRowStyleGlo.MouseAndAlternately:
                        if (IsEnableDouclickGlo)
                            is1 = this.AddTR(is1, "ondblclick=" + urlExt);
                        else
                            is1 = this.AddTR(is1);
                        break;
                    default:
                        throw new Exception("@ There is no offer .");
                }

                idx++;
                this.AddTDIdx(idx);
                string val = "";
                foreach (Attr attr in selectedAttrs)
                {
                    if (attr.UIVisible == false)
                        continue;

                    if (attr.Key == "MyNum")
                        continue;

                    this.DataPanelDtlAdd(en, attr, cfgs, url, urlExt, FocusField);
                }

                if (IsEnableRefFunc && isRefFunc)
                {
                    string str = "";
                    // string str = "<a href=\"javascript:WinOpen('../Comm/RefFunc/UIEn.aspx?EnsName=" + ens.ToString() + "&PK=" + en.GetValByKey(pk) + url + "', 'cd')\" > Turn on </a>";
                    //<a href=\"javascript:WinOpen('../Comm/RefFunc/UIEn.aspx?EnsName=" + ens.ToString() + "&PK=" + en.GetValByKey(pk) + url + "','cd','400','600')\" > Turn on </A>

                    #region  Joined his door way 
                    RefMethods myreffuncs = en.EnMap.HisRefMethods;
                    foreach (RefMethod func in myreffuncs)
                    {
                        if (func.Visable == false || func.IsForEns == false)
                            continue;
                        str += "<A style='cursor:hand;' nowrap=true onclick=\"javascript:RefMethod1('" + this.Request.ApplicationPath + "', '" + func.Index + "', '" + func.Warning + "', '" + func.Target + "', '" + ens.ToString() + "','" + url + "') \"  > " + func.GetIcon(this.Request.ApplicationPath) + "" + func.Title + "</A>";
                    }
                    #endregion

                    #region  He added detail 
                    EnDtls enDtls = en.EnMap.Dtls;
                    foreach (EnDtl enDtl in enDtls)
                    {
                        str += "[<A onclick=\"javascript:EditDtl1('" + this.Request.ApplicationPath + "', '" + myen.ToString() + "',  '" + enDtl.EnsName + "', '" + enDtl.RefKey + "', '" + url + "&IsShowSum=1')\" >" + enDtl.Desc + "</A>]";
                    }
                    #endregion

                    #region  Join many of the entity editor 
                    AttrsOfOneVSM oneVsM = en.EnMap.AttrsOfOneVSM;
                    foreach (AttrOfOneVSM vsM in oneVsM)
                    {
                        //  str += "[<A onclick=\"javascript:EditOneVsM1('" + this.Request.ApplicationPath + "','" + en.ToString() + "','" + vsM.EnsOfMM.ToString() + "s','" + vsM.EnsOfMM + "&dt=" + DateTime.Now.ToString("hhss") + "','" + myen.ToString() + "','" + url + "'); return; \" >" + vsM.Desc + "</A>]";
                    }
                    #endregion

                    //if (isShowOpenICON)
                    //    this.Add("<TD class='TD' style='cursor:hand;' nowrap=true  >" + str + " <a href=" + urlExt + " ><img src='../Img/Btn/Open.gif' border=0/></a></TD>");
                    //else
                    this.Add("<TD class='TD' style='cursor:hand;' nowrap=true  >" + str + "</TD>");

                }
                else
                {
                    if (isShowOpenICON)
                        this.Add("<TD class='TD' style='cursor:hand;' nowrap=true><a href=" + urlExt + " ><img src='../Img/Btn/Open.gif' border=0/></a></TD>");
                }
                this.AddTREnd();
            }

            #region   Total code required to write here .
            string NoShowSum = SystemConfig.GetConfigXmlEns("NoShowSum", ens.ToString());
            if (NoShowSum == null)
                NoShowSum = "";

            bool IsHJ = false;
            foreach (Attr attr in selectedAttrs)
            {
                if (attr.MyFieldType == FieldType.RefText)
                    continue;

                if (attr.UIContralType == UIContralType.DDL)
                    continue;

                if (NoShowSum.IndexOf("@" + attr.Key + "@") != -1)
                    continue;

                if (attr.Key == "OID" || attr.Key == "MID" || attr.Key.ToUpper() == "WORKID")
                    continue;

                switch (attr.MyDataType)
                {
                    case DataType.AppDouble:
                    case DataType.AppFloat:
                    case DataType.AppInt:
                    case DataType.AppMoney:
                        IsHJ = true;
                        break;
                    default:
                        break;
                }
            }

            IsHJ = false;
            //if (ens.Count > 1 )
            //    IsHJ = true;
            //foreach (Attr attr in attrs)
            //{
            //    if (attr.IsNum  )
            //    {
            //        IsHJ = true;
            //    }
            //}



            if (IsHJ)
            {
                //  Identify the configuration is not displayed in the column total .

                if (NoShowSum == null)
                    NoShowSum = "";

                this.Add("<TR class='TRSum' >");
                this.AddTD(" Total ");
                foreach (Attr attr in selectedAttrs)
                {

                    if (attr.MyFieldType == FieldType.RefText)
                        continue;

                    if (attr.UIVisible == false)
                        continue;

                    if (attr.Key == "MyNum")
                        continue;


                    if (attr.MyDataType == DataType.AppBoolean)
                    {
                        this.AddTD();
                        continue;
                    }

                    if (attr.UIContralType == UIContralType.DDL)
                    {
                        this.AddTD();
                        continue;
                    }
                    if (attr.Key == "OID" || attr.Key == "MID" || attr.Key.ToUpper() == "WORKID")
                    {
                        this.AddTD();
                        continue;
                    }


                    if (NoShowSum.IndexOf("@" + attr.Key + "@") != -1)
                    {
                        /* It does not need to show their total .*/
                        this.AddTD();
                        continue;
                    }



                    switch (attr.MyDataType)
                    {
                        case DataType.AppDouble:
                            this.AddTDNum(ens.GetSumDecimalByKey(attr.Key));
                            break;
                        case DataType.AppFloat:
                            this.AddTDNum(ens.GetSumDecimalByKey(attr.Key));
                            break;
                        case DataType.AppInt:
                            this.AddTDNum(ens.GetSumDecimalByKey(attr.Key));
                            break;
                        case DataType.AppMoney:
                            this.AddTDJE(ens.GetSumDecimalByKey(attr.Key));
                            break;
                        default:
                            this.AddTD();
                            break;
                    }
                }
                this.AddTREnd();
            }
            #endregion
            this.AddTableEnd();
        }
        /// <summary>
        /// DataPanelDtl
        /// </summary>
        /// <param name="ens">要bind ens</param>
        /// <param name="ctrlId">webmenu id </param>
        /// <param name="groupkey">groupkey</param>
        //		public void DataPanelDtl(Entities ens, string ctrlId, string groupkey)
        public void DataPanelDtl(Entities ens, string ctrlId, string groupkey)
        {
            if (groupkey == "None")
            {
                this.DataPanelDtl(ens, ctrlId);
                return;
            }

            BP.Sys.Xml.Searchs cfgs = new BP.Sys.Xml.Searchs();
            cfgs.RetrieveBy(BP.Sys.Xml.SearchAttr.For, ens.ToString());
            //   string cfgurl = "";

            this.Controls.Clear();
            Entity myen = ens.GetNewEntity;
            string pk = myen.PK;
            string clName = myen.ToString();
            Attrs attrs = myen.EnMap.Attrs;
            Attrs selectedAttrs = myen.EnMap.GetChoseAttrs(ens);
            Attr groupAttr = myen.EnMap.GetAttrByKey(groupkey);
            if (groupAttr.MyFieldType == FieldType.Enum
                || groupAttr.MyFieldType == FieldType.PKEnum)
            {
                SysEnums ses = new SysEnums(groupAttr.Key);
                this.AddTable();
                this.AddTR();
                int num = 0;
                foreach (Attr attrT in selectedAttrs)
                {
                    if (attrT.UIVisible == false || attrT.Key == groupAttr.Key)
                        continue;
                    this.AddTDTitle(attrT.Desc);
                    num++;
                }
                this.AddTREnd();

                foreach (SysEnum se in ses)
                {
                    int gval = se.IntKey;

                    int i = 0;
                    foreach (Entity en in ens)
                    {
                        if (en.GetValIntByKey(groupAttr.Key) != gval)
                            continue;
                        i++;
                    }
                    if (i == 0)
                        continue;

                    this.AddTR();
                    this.Add("<TD colspan=" + num + " class='Bar' >&nbsp;" + se.Lab + "&nbsp;(Total " + i + " records)</TD>");
                    this.AddTREnd();

                    foreach (Entity en in ens)
                    {
                        if (en.GetValIntByKey(groupAttr.Key) != gval)
                            continue;

                        #region  Deal with  keys
                        string style = WebUser.Style;
                        string url = this.GenerEnUrl(en, attrs);
                        #endregion

                        this.AddTRTXHand(" ondblclick=\"WinOpen('../Comm/RefFunc/UIEn.aspx?EnsName=" + ens.ToString() + "&PK=" + en.GetValByKey(pk) + url + "')\"   onmousedown=\"OnDGMousedown('" + this.Page.Request.ApplicationPath + "','" + ctrlId + "', '" + clName + "', '" + url + "')\" ");
                        foreach (Attr attr in selectedAttrs)
                        {
                            if (attr.UIVisible == false || attr.Key == groupAttr.Key)
                                continue;
                            // this.DataPanelDtlAdd(en, attr, cfgs, url);
                        }
                        this.AddTREnd();
                    }
                }
                this.AddTableEnd();
            }
            else
            {
                Entities ensG = BP.En.ClassFactory.GetEns(groupAttr.UIBindKey);
                ensG.RetrieveAll();
                this.AddTable(); //("<TABLE  class='Table' id='tb1' >");
                this.AddTR();
                int num = 0;
                foreach (Attr attrT in selectedAttrs)
                {
                    if (attrT.UIVisible == false || attrT.Key == groupAttr.Key)
                        continue;
                    this.AddTDTitle(attrT.Desc);
                    num++;
                }
                this.AddTREnd();

                foreach (Entity enG in ensG)
                {
                    string gval = enG.GetValStringByKey(groupAttr.UIRefKeyValue);

                    int i = 0;
                    foreach (Entity en in ens)
                    {
                        if (en.GetValStringByKey(groupAttr.Key) != gval)
                            continue;
                        i++;
                    }
                    if (i == 0)
                        continue;

                    this.Add("<TR ><TD colspan=" + num + " class='Bar' >" + groupAttr.Desc + ":" + enG.GetValByKey(groupAttr.UIRefKeyText) + "&nbsp;(Total " + i + " records)</TD></TR>");

                    foreach (Entity en in ens)
                    {
                        if (en.GetValStringByKey(groupAttr.Key) != gval)
                            continue;

                        #region  Deal with  keys
                        string style = WebUser.Style;
                        string url = this.GenerEnUrl(en, attrs);

                        #endregion

                        this.AddTRTXHand(" ondblclick=\"WinOpen( '../Comm/RefFunc/UIEn.aspx?EnsName=" + ens.ToString() + "&PK=" + en.GetValByKey(pk) + url + "')\"  onmousedown=\"OnDGMousedown('" + this.Page.Request.ApplicationPath + "','" + ctrlId + "', '" + clName + "', '" + url + "')\" ");
                        foreach (Attr attr in selectedAttrs)
                        {
                            if (attr.UIVisible == false || attr.Key == groupAttr.Key)
                                continue;
                            //   this.DataPanelDtlAdd(en, attr, cfgs, url);
                        }
                        this.AddTREnd();
                    }
                }
                this.AddTableEnd(); //("</TABLE>");
            }
        }
        /// <summary>
        /// DataPanelDtl
        /// </summary>
        /// <param name="ens">要bind ens</param>
        /// <param name="ctrlId">webmenu id </param>
        /// <param name="groupkey">groupkey</param>
        //		public void DataPanelDtl(Entities ens, string ctrlId, string groupkey, string groupkey2)
        public void DataPanelDtl(Entities ens, string ctrlId, string groupkey, string groupkey2)
        {
            if (groupkey2 == "None" || groupkey == groupkey2)
            {
                this.DataPanelDtl(ens, ctrlId, groupkey);
                return;
            }

            Entities ensG2 = new Emps();

            this.Controls.Clear();
            Entity myen = ens.GetNewEntity;
            string pk = myen.PK;
            string clName = myen.ToString();
            Attrs attrs = myen.EnMap.Attrs;
            Attr groupAttr = myen.EnMap.GetAttrByKey(groupkey);
            Attr groupAttr2 = myen.EnMap.GetAttrByKey(groupkey2);

            BP.Sys.Xml.Searchs cfgs = new BP.Sys.Xml.Searchs();
            cfgs.RetrieveBy(BP.Sys.Xml.SearchAttr.For, ens.ToString());
            // string cfgurl = "";

            #region  Increase title 
            this.AddTable();
            this.AddTR();
            int num = 0;
            foreach (Attr attrT in myen.EnMap.Attrs)
            {
                if (attrT.UIVisible == false || attrT.Key == groupAttr.Key || attrT.Key == groupAttr2.Key)
                    continue;

                this.AddTDTitle(attrT.Desc);
                num++;
            }
            this.AddTREnd();
            #endregion

            if (groupAttr.MyFieldType == FieldType.Enum || groupAttr.MyFieldType == FieldType.PKEnum)
            {
                /*  If the first packet is an enumeration type .*/
                SysEnums ses = new SysEnums(groupAttr.Key);
                if (groupAttr2.MyFieldType == FieldType.Enum || groupAttr2.MyFieldType == FieldType.PKEnum)
                {
                    /*  Condition 1  And conditions 2  Are enumerated type  */
                    SysEnums ses2 = new SysEnums(groupAttr2.Key);
                    foreach (SysEnum se in ses)
                    {
                        string gval = se.IntKey.ToString();
                        int i = 0;
                        foreach (Entity en in ens)
                        {
                            if (en.GetValStringByKey(groupAttr.Key) != gval)
                                continue;
                            i++;
                        }
                        if (i == 0)
                            continue;

                        this.Add("<TR ><TD colspan=" + num + " class='Bar' >" + groupAttr.Desc + ":" + se.Lab + "&nbsp;(Total " + i + " records)</TD></TR>");

                        //  Convenient start 2 Packet .
                        foreach (SysEnum se2 in ses2)
                        {
                            string gval2 = se2.IntKey.ToString();  //.GetValStringByKey(groupAttr2.UIRefKeyValue);
                            i = 0;
                            foreach (Entity en in ens)
                            {
                                if (en.GetValStringByKey(groupAttr.Key) != gval || en.GetValStringByKey(groupAttr2.Key) != gval2)
                                    continue;
                                i++;
                            }
                            if (i == 0)
                                continue;

                            this.Add("<TR><TD colspan=" + num + " class='Bar' >&nbsp;&nbsp;" + groupAttr2.Desc + ":" + se2.Lab + "&nbsp;(Total " + i + " records)</TD></TR>");
                            foreach (Entity en in ens)
                            {
                                if (en.GetValStringByKey(groupAttr.Key) != gval || en.GetValStringByKey(groupAttr2.Key) != gval2)
                                    continue;

                                string style = WebUser.Style;
                                string url = this.GenerEnUrl(en, attrs);
                                this.Add("<TR class='TR' ondblclick=\"WinOpen( '../Comm/RefFunc/UIEn.aspx?EnsName=" + ens.ToString() + "&PK=" + en.GetValByKey(pk) + url + "')\"  onmousedown=\"OnDGMousedown('" + this.Page.Request.ApplicationPath + "','" + ctrlId + "', '" + clName + "', '" + url + "')\" onmouseover='TROver(this);OnDGMousedown('" + this.Page.Request.ApplicationPath + "','" + ctrlId + "', '" + clName + "', '" + url + "');' onmouseout='TROut(this)' >");
                                foreach (Attr attr in attrs)
                                {
                                    if (attr.UIVisible == false || attr.Key == groupAttr2.Key || attr.Key == groupAttr.Key)
                                        continue;
                                    //this.DataPanelDtlAdd(en, attr, cfgs, url);
                                }
                                this.AddTREnd();
                            }
                        }
                    }
                }
                else  /*  The first condition is to enumerate groups , The second condition is entities. */
                {
                    ensG2 = BP.En.ClassFactory.GetEns(groupAttr2.UIBindKey);
                    ensG2.RetrieveAll();

                    foreach (SysEnum se in ses)
                    {
                        string gval = se.IntKey.ToString();
                        int i = 0;
                        foreach (Entity en in ens)
                        {
                            if (en.GetValStringByKey(groupAttr.Key) != gval)
                                continue;
                            i++;
                        }
                        if (i == 0)
                            continue;

                        this.Add("<TR ><TD colspan=" + num + " class='Bar' >" + groupAttr.Desc + ":" + se.Lab + "&nbsp;(Total " + i + " records)</TD></TR>");

                        //  Convenient start 2 Packet .
                        foreach (Entity enG2 in ensG2)
                        {
                            string gval2 = enG2.GetValStringByKey(groupAttr2.UIRefKeyValue);
                            i = 0;
                            foreach (Entity en in ens)
                            {
                                if (en.GetValStringByKey(groupAttr.Key) != gval || en.GetValStringByKey(groupAttr2.Key) != gval2)
                                    continue;
                                i++;
                            }
                            if (i == 0)
                                continue;

                            this.Add("<TR><TD colspan=" + num + " class='Bar' >&nbsp;&nbsp;" + groupAttr2.Desc + ":" + enG2.GetValByKey(groupAttr2.UIRefKeyText) + "&nbsp;(Total " + i + " records)</TD></TR>");
                            foreach (Entity en in ens)
                            {
                                if (en.GetValStringByKey(groupAttr.Key) != gval || en.GetValStringByKey(groupAttr2.Key) != gval2)
                                    continue;

                                string style = WebUser.Style;
                                string url = this.GenerEnUrl(en, attrs);
                                this.Add("<TR class='TR' ondblclick=\"WinOpen( '../Comm/RefFunc/UIEn.aspx?EnsName=" + ens.ToString() + "&PK=" + en.GetValByKey(pk) + url + "')\"  onmouseover=\"TROver(this,'" + this.Page.Request.ApplicationPath + "','" + ctrlId + "', '" + clName + "', '" + url + "');\" onmouseout='TROut(this)' >");
                                foreach (Attr attr in attrs)
                                {
                                    if (attr.UIVisible == false || attr.Key == groupAttr2.Key || attr.Key == groupAttr.Key)
                                        continue;
                                    //this.DataPanelDtlAdd(en, attr, cfgs, url);
                                }
                                this.AddTREnd();
                            }
                        }
                    }

                    this.AddTableEnd();
                    return;
                }

            } /*  The first stage is to determine the end of the title enum type situation . */


            Entities ensG = BP.En.ClassFactory.GetEns(groupAttr.UIBindKey);
            ensG.RetrieveAll();

            if (groupAttr2.MyFieldType == FieldType.Enum || groupAttr2.MyFieldType == FieldType.PKEnum)
            {
                /* In case  2  Level   Is an enumeration type */
                SysEnums ses = new SysEnums(groupAttr2.Key);
                foreach (Entity enG in ensG)
                {
                    string gval = enG.GetValStringByKey(groupAttr.UIRefKeyValue);
                    int i = 0;
                    foreach (Entity en in ens)
                    {
                        if (en.GetValStringByKey(groupAttr.Key) != gval)
                            continue;
                        i++;
                    }
                    if (i == 0)
                        continue;

                    this.Add("<TR ><TD colspan=" + num + " class='Bar' >" + groupAttr.Desc + ":" + enG.GetValByKey(groupAttr.UIRefKeyText) + "&nbsp;(Total " + i + " records)</TD></TR>");

                    //  Convenient start 2 Packet .
                    foreach (SysEnum se in ses)
                    {
                        string gval2 = se.IntKey.ToString();  //.GetValStringByKey(groupAttr2.UIRefKeyValue);
                        i = 0;
                        foreach (Entity en in ens)
                        {
                            if (en.GetValStringByKey(groupAttr.Key) != gval || en.GetValStringByKey(groupAttr2.Key) != gval2)
                                continue;
                            i++;
                        }
                        if (i == 0)
                            continue;

                        this.Add("<TR><TD colspan=" + num + " class='Bar' >&nbsp;&nbsp;" + groupAttr2.Desc + ":" + se.Lab + "&nbsp;(Total " + i + " records)</TD></TR>");
                        foreach (Entity en in ens)
                        {
                            if (en.GetValStringByKey(groupAttr.Key) != gval || en.GetValStringByKey(groupAttr2.Key) != gval2)
                                continue;

                            string style = WebUser.Style;
                            string url = this.GenerEnUrl(en, attrs);
                            this.Add("<TR class='TR' ondblclick=\"WinOpen( '../Comm/RefFunc/UIEn.aspx?EnsName=" + ens.ToString() + "&PK=" + en.GetValByKey(pk) + url + "')\"  onmouseover=\"TROver(this,'" + this.Page.Request.ApplicationPath + "','" + ctrlId + "', '" + clName + "', '" + url + "')\" onmouseout='TROut(this)' >");
                            foreach (Attr attr in attrs)
                            {
                                if (attr.UIVisible == false || attr.Key == groupAttr2.Key || attr.Key == groupAttr.Key)
                                    continue;
                                //this.DataPanelDtlAdd(en, attr, cfgs, url);
                            }
                            this.AddTREnd();
                        }
                    }
                }
                return;
            }

            ensG2 = BP.En.ClassFactory.GetEns(groupAttr2.UIBindKey);
            ensG2.RetrieveAll();
            foreach (Entity enG in ensG)
            {
                string gval = enG.GetValStringByKey(groupAttr.UIRefKeyValue);
                int i = 0;
                foreach (Entity en in ens)
                {
                    if (en.GetValStringByKey(groupAttr.Key) != gval)
                        continue;
                    i++;
                }
                if (i == 0)
                    continue;

                this.Add("<TR ><TD colspan=" + num + " class='Bar' >" + groupAttr.Desc + ":" + enG.GetValByKey(groupAttr.UIRefKeyText) + "&nbsp;(Total " + i + " records)</TD></TR>");

                //  Convenient start 2 Packet .
                foreach (Entity enG2 in ensG2)
                {
                    string gval2 = enG2.GetValStringByKey(groupAttr2.UIRefKeyValue);
                    i = 0;
                    foreach (Entity en in ens)
                    {
                        if (en.GetValStringByKey(groupAttr.Key) != gval)
                            continue;
                        if (en.GetValStringByKey(groupAttr2.Key) != gval2)
                            continue;
                        i++;
                    }
                    if (i == 0)
                        continue;

                    this.Add("<TR><TD colspan=" + num + " class='Bar' >&nbsp;&nbsp;" + groupAttr2.Desc + ":" + enG2.GetValByKey(groupAttr2.UIRefKeyText) + "&nbsp;(Total " + i + " records)</TD></TR>");
                    foreach (Entity en in ens)
                    {
                        if (en.GetValStringByKey(groupAttr.Key) != gval || en.GetValStringByKey(groupAttr2.Key) != gval2)
                            continue;

                        string style = WebUser.Style;
                        string url = this.GenerEnUrl(en, attrs);
                        this.Add("<TR class='TR' ondblclick=\"WinOpen( '../Comm/RefFunc/UIEn.aspx?EnsName=" + ens.ToString() + "&PK=" + en.GetValByKey(pk) + url + "')\" onmouseover=\"TROver(this,'" + this.Page.Request.ApplicationPath + "','" + ctrlId + "', '" + clName + "', '" + url + "')\" onmouseout='TROut(this)' >");
                        foreach (Attr attr in attrs)
                        {
                            if (attr.UIVisible == false || attr.Key == groupAttr2.Key || attr.Key == groupAttr.Key)
                                continue;
                            // this.DataPanelDtlAdd(en, attr, cfgs, url);
                        }
                        this.AddTREnd();
                    }
                }
            }
            this.AddTableEnd(); //("</TABLE>");
        }
        private string GenerEnUrl(Entity en, Attrs attrs)
        {
            string url = "";
            foreach (Attr attr in attrs)
            {
                switch (attr.UIContralType)
                {
                    case UIContralType.TB:
                        if (attr.IsPK)
                            url += "&" + attr.Key + "=" + en.GetValStringByKey(attr.Key);
                        break;
                    case UIContralType.DDL:
                        url += "&" + attr.Key + "=" + en.GetValStringByKey(attr.Key);
                        break;
                }
            }
            return url;
        }

        private void DataPanelDtlAdd(Entity en, Attr attr, BP.Sys.Xml.Searchs cfgs, string url, string cardUrl, string focusField)
        {
            string cfgurl = "";
            if (attr.UIContralType == UIContralType.DDL)
            {
                this.AddTD(en.GetValRefTextByKey(attr.Key));
                return;
            }
            if (attr.UIHeight != 0)
            {
                this.AddTDDoc("...", "...");
                return;
            }
            string str = en.GetValStrByKey(attr.Key);

            if (focusField == attr.Key)
                str = "<a href=" + cardUrl + ">" + str + "</a>";

            switch (attr.MyDataType)
            {
                case DataType.AppDate:
                case DataType.AppDateTime:
                    if (str == "" || str == null)
                        str = "&nbsp;";
                    this.AddTD(str);
                    break;
                case DataType.AppString:
                    if (str == "" || str == null)
                        str = "&nbsp;";

                    if (attr.UIHeight != 0)
                    {
                        this.AddTDDoc(str, str);
                    }
                    else
                    {
                        if (attr.Key.IndexOf("ail") == -1)
                            this.AddTD(str);
                        else
                            this.AddTD("<a href=\"javascript:mailto:" + str + "\"' >" + str + "</a>");
                    }
                    break;
                case DataType.AppBoolean:
                    if (str == "1")
                        this.AddTD("是");
                    else
                        this.AddTD("否");
                    break;
                case DataType.AppFloat:
                case DataType.AppInt:
                case DataType.AppRate:
                case DataType.AppDouble:
                    foreach (BP.Sys.Xml.Search pe in cfgs)
                    {
                        if (pe.Attr == attr.Key)
                        {
                            cfgurl = pe.URL;
                            Attrs attrs = en.EnMap.Attrs;
                            foreach (Attr attr1 in attrs)
                                cfgurl = cfgurl.Replace("@" + attr1.Key, en.GetValStringByKey(attr1.Key));

                            break;
                        }
                    }
                    if (cfgurl == "")
                    {
                        this.AddTDNum(str);
                    }
                    else
                    {
                        cfgurl = cfgurl.Replace("@Keys", url);
                        this.AddTDNum("<a href=\"javascript:WinOpen('" + cfgurl + "','dtl1');\" >" + str + "</a>");
                    }
                    break;
                case DataType.AppMoney:
                    cfgurl = "";
                    foreach (BP.Sys.Xml.Search pe in cfgs)
                    {
                        if (pe.Attr == attr.Key)
                        {
                            cfgurl = pe.URL;
                            Attrs attrs = en.EnMap.Attrs;
                            foreach (Attr attr2 in attrs)
                                cfgurl = cfgurl.Replace("@" + attr2.Key, en.GetValStringByKey(attr2.Key));
                            break;
                        }
                    }
                    if (cfgurl == "")
                    {
                        this.AddTDNum(decimal.Parse(str).ToString("0.00"));
                    }
                    else
                    {
                        cfgurl = cfgurl.Replace("@Keys", url);
                        this.AddTDNum("<a href=\"javascript:WinOpen('" + cfgurl + "','dtl1');\" >" + decimal.Parse(str).ToString("0.00") + "</a>");
                    }
                    break;
                default:
                    throw new Exception("no this case ...");
            }
        }
        //		public void UIEn1ToMGroupKey(Entities ens, string showVal, string showText, Entities selectedEns, string selecteVal, string groupKey)
        public void UIEn1ToMGroupKey(Entities ens, string showVal, string showText, Entities selectedEns, string selecteVal, string groupKey)
        {
            this.EnableViewState = true;
            this.Controls.Clear();
            this.Add("<TABLE class='Table' cellSpacing='1' cellPadding='1'  border='1' style='width:100%'>");
            //this.AddTable(); // ("<TABLE class='Table' cellSpacing='1' cellPadding='1'  border='1'>");

            Attr attr = ens.GetNewEntity.EnMap.GetAttrByKey(groupKey);
            if (attr.MyFieldType == FieldType.Enum || attr.MyFieldType == FieldType.PKEnum) //  Check whether it is  enum  Type .
            {
                BP.Sys.SysEnums eens = new BP.Sys.SysEnums(attr.Key);
                foreach (SysEnum se in eens)
                {
                    this.Add("<TR>");
                    this.Add("<TD class='GroupTitle' colspan=3 >");

                    CheckBox cb1 = new System.Web.UI.WebControls.CheckBox();
                    cb1.Text = se.Lab;
                    cb1.ID = "CB_SE_" + se.IntKey;
                    this.Add(cb1);
                    this.Add("</TD>");
                    this.AddTREnd();

                    int i = 0;
                    bool is1 = false;
                    string ctlIDs = "";
                    foreach (Entity en in ens)
                    {
                        if (en.GetValIntByKey(attr.Key) != se.IntKey)
                            continue;

                        i++;
                        if (i == 4)
                            i = 1;
                        if (i == 1)
                            is1 = this.AddTR(is1);
                        //this.Add("<TR>");
                        CheckBox cb = new CheckBox();
                        cb.ID = "CB_" + en.GetValStringByKey(showVal);
                        ctlIDs += cb.ID + ",";

                        cb.Text = en.GetValStringByKey(showText);
                        cb.AccessKey = se.IntKey.ToString();

                        this.AddTD(cb);
                        if (i == 3)
                            this.AddTREnd();
                    }
                    cb1.Attributes["onclick"] = "SetSelected(this,'" + ctlIDs + "')";

                    // add blank
                    switch (i)
                    {
                        case 1:
                            this.Add("<TD>&nbsp;</TD>");
                            this.Add("<TD>&nbsp;</TD>");
                            this.AddTREnd();
                            break;
                        case 2:
                            this.Add("<TD>&nbsp;</TD>");
                            this.AddTREnd();
                            break;
                        default:
                            break;
                    }
                }

            }
            else
            {
                Entities groupEns = ClassFactory.GetEns(attr.UIBindKey);
                groupEns.RetrieveAll();
                foreach (Entity group in groupEns)
                {
                    this.Add("<TR>");
                    this.Add("<TD class='GroupTitle' colspan=3>");

                    CheckBox cb1 = new System.Web.UI.WebControls.CheckBox();
                    cb1.Text = group.GetValStrByKey(attr.UIRefKeyText);
                    cb1.ID = "CB_EN_" + group.GetValStrByKey(attr.UIRefKeyValue);
                    // cb1.Attributes["onclick"] = "SetSelected(this,'" + group.GetValStringByKey(attr.UIRefKeyValue) + "')";
                    this.Add(cb1);
                    this.Add("</TD>");
                    this.AddTREnd();

                    string ctlIDs = "";
                    int i = 0;
                    string gVal = group.GetValStrByKey(attr.UIRefKeyValue);
                    foreach (Entity en in ens)
                    {
                        if (en.GetValStrByKey(attr.Key) != gVal)
                            continue;
                        i++;
                        if (i == 4)
                            i = 1;
                        if (i == 1)
                            this.Add("<TR>");


                        CheckBox cb = new CheckBox();
                        cb.ID = "CB_" + en.GetValStrByKey(showVal);
                        cb.Text = en.GetValStrByKey(showText);
                        this.AddTD(cb);

                        ctlIDs += cb.ID + ",";
                        if (i == 3)
                            this.AddTREnd();
                    }

                    // add blank
                    switch (i)
                    {
                        case 1:
                            this.Add("<TD>&nbsp;</TD>");
                            this.Add("<TD>&nbsp;</TD>");
                            this.AddTREnd();
                            break;
                        case 2:
                            this.Add("<TD>&nbsp;</TD>");
                            this.AddTREnd();
                            break;
                        default:
                            break;
                    }
                    cb1.Attributes["onclick"] = "SetSelected(this,'" + ctlIDs + "')";
                }
            }

            this.AddTableEnd();

            //  Setting choice  ens .
            foreach (Entity en in selectedEns)
            {
                string key = en.GetValStringByKey(selecteVal);
                CheckBox bp = (CheckBox)this.FindControl("CB_" + key);
                if (bp == null)
                    continue;

                bp.Checked = true;
            }

        }
        //		public void UIEn1ToMGroupKey_Line(Entities ens, string showVal, string showText, Entities selectedEns, string selecteVal, string groupKey)
        public void UIEn1ToMGroupKey_Line(Entities ens, string showVal, string showText, Entities selectedEns, string selecteVal, string groupKey)
        {
            this.EnableViewState = true;
            this.Controls.Clear();
            this.Add("<TABLE class='Table' cellSpacing='1' cellPadding='1'  border='1' width='100%' >");

            Attr attr = ens.GetNewEntity.EnMap.GetAttrByKey(groupKey);
            if (attr.MyFieldType == FieldType.Enum || attr.MyFieldType == FieldType.PKEnum) //  Check whether it is  enum  Type .
            {
                BP.Sys.SysEnums eens = new BP.Sys.SysEnums(attr.Key);
                foreach (SysEnum se in eens)
                {
                    this.Add("<TR>");
                    this.Add("<TD class='GroupTitle' >" + se.Lab + "</TD>");
                    this.AddTREnd();
                    foreach (Entity en in ens)
                    {
                        if (en.GetValIntByKey(attr.Key) != se.IntKey)
                            continue;

                        this.AddTR();
                        CheckBox cb = new CheckBox();
                        cb.ID = "CB_" + en.GetValStrByKey(showVal);
                        cb.Text = en.GetValStrByKey(showText);
                        this.AddTD(cb);
                        this.AddTREnd();
                    }
                }
            }
            else
            {
                Entities groupEns = ClassFactory.GetEns(attr.UIBindKey);
                groupEns.RetrieveAll();
                foreach (Entity group in groupEns)
                {
                    this.Add("<TR>");
                    this.Add("<TD class='GroupTitle' >" + group.GetValStringByKey(attr.UIRefKeyText) + "</TD>");
                    this.AddTREnd();

                    foreach (Entity en in ens)
                    {
                        if (en.GetValStringByKey(attr.Key) != group.GetValStringByKey(attr.UIRefKeyValue))
                            continue;

                        this.Add("<TR>");

                        CheckBox cb = new CheckBox();
                        cb.ID = "CB_" + en.GetValStringByKey(showVal);
                        cb.Text = en.GetValStringByKey(showText);

                        this.Add("<TD >");
                        this.Add(cb);
                        this.Add("</TD>");
                        this.AddTREnd();
                    }
                }
            }

            this.AddTableEnd();

            //  Setting choice  ens .
            foreach (Entity en in selectedEns)
            {
                string key = en.GetValStringByKey(selecteVal);
                CheckBox bp = (CheckBox)this.FindControl("CB_" + key);
                if (bp == null)
                    continue;

                bp.Checked = true;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ens"></param>
        /// <param name="groupKey"></param>
        //		public void UIEn1ToM(Entities ens, string showVal, string showText, Entities selectedEns, string selecteVal)
        public void UIEn1ToM(Entities ens, string showVal, string showText, Entities selectedEns, string selecteVal)
        {
            this.Controls.Clear();
            this.AddTable("class='Table' cellSpacing='1' cellPadding='1'  border='1' style='width:100%'");
            int i = 0;
            bool is1 = false;
            foreach (Entity en in ens)
            {
                i++;
                if (i == 4)
                    i = 1;

                if (i == 1)
                {
                    is1 = this.AddTR(is1);
                }

                CheckBox cb = new CheckBox();
                cb.ID = "CB_" + en.GetValStringByKey(showVal);
                cb.Text = en.GetValStringByKey(showText);
                this.AddTD(cb);
                if (i == 3)
                    this.AddTREnd();
            }

            switch (i)
            {
                case 1:
                    this.AddTD();
                    this.AddTD();//"<TD>&nbsp;</TD>");
                    this.AddTREnd();//("</TR>");
                    break;
                case 2:
                    this.AddTD();
                    this.AddTREnd();
                    break;
                default:
                    break;
            }
            this.AddTableEnd();

            //  Setting choice  ens .
            foreach (Entity en in selectedEns)
            {
                string key = en.GetValStringByKey(selecteVal);
                try
                {
                    CheckBox bp = (CheckBox)this.FindControl("CB_" + key);
                    bp.Checked = true;
                }
                catch
                {
                }
            }
        }
        public void UIEn1ToM_Tree(Entities ens, string showVal, string showText, Entities selectedEns, string selecteVal)
        {
            this.Controls.Clear();
            //this.Add("<table border=0 width='500px'>");
            //this.AddTR();
            //this.AddTDBegin();
            // this.Add("<font size=12px >");
            string no = null;
            foreach (Entity en in ens)
            {
                no = en.GetValStrByKey(showVal);
                CheckBox cb = new CheckBox();
                cb.ID = "CB_" + no;
                cb.Text = no + "&nbsp;" + en.GetValStrByKey(showText);
                this.Add(DataType.GenerSpace(no.Length / 2));
                this.Add(cb);
                this.AddBR();
            }
            //this.Add("</font>");
            //this.AddTDEnd();
            //this.AddTREnd();
            //this.AddTableEnd();

            //  Setting choice  ens .
            foreach (Entity en in selectedEns)
            {
                string key = en.GetValStringByKey(selecteVal);
                CheckBox bp = (CheckBox)this.FindControl("CB_" + key);
                bp.Checked = true;
            }
        }

        //./././ public void UIEn1ToM_OneLine(Entities ens, string showVal, string showText, Entities selectedEns, string selecteVal)
        public void UIEn1ToM_OneLine(Entities ens, string showVal, string showText, Entities selectedEns, string selecteVal)
        {
            this.Controls.Clear();
            this.Add("<table border=0 width='500px'>");
            bool is1 = false;
            foreach (Entity en in ens)
            {
                is1 = this.AddTR(is1); //("<TR>");
                CheckBox cb = new CheckBox();
                cb.ID = "CB_" + en.GetValStrByKey(showVal);
                cb.Text = en.GetValStringByKey(showText);
                this.AddTD(cb);
                this.AddTREnd();
            }
            this.AddTableEnd();

            //  Setting choice  ens .
            foreach (Entity en in selectedEns)
            {
                string key = en.GetValStrByKey(selecteVal);
                CheckBox bp = (CheckBox)this.FindControl("CB_" + key);
                bp.Checked = true;
            }
        }
        /// <summary>
        /// s
        /// </summary>
        /// <param name="ens"></param>
        /// <param name="ctrlId"></param>
        /// <param name="showtext1"></param>
        /// <param name="showDtl"></param>
        private void DataPanelCards(Entities ens, string ctrlId, string showtext1, bool showDtl)
        {
            this.Controls.Clear();
            this.AddTable();
            int i = 0;
            Entity myen = ens.GetNewEntity;
            string pk = myen.PK;
            string textName1 = myen.EnMap.GetAttrByKey(showtext1).Desc;
            //	string textName2=myen.EnMap.GetAttrByKey(showtext2).Desc;
            string clName = myen.ToString();
            Attrs attrs = myen.EnMap.Attrs;

            foreach (Entity en in ens)
            {
                if (i == 0)
                    this.AddTREnd();
                i++;

                #region  Deal with keys
                string style = WebUser.Style;
                string url = "";
                foreach (Attr attr in attrs)
                {
                    switch (attr.UIContralType)
                    {
                        case UIContralType.TB:
                            if (attr.IsPK)
                                url += "&" + attr.Key + "=" + en.GetValStringByKey(attr.Key);
                            break;
                        case UIContralType.DDL:
                            url += "&" + attr.Key + "=" + en.GetValStringByKey(attr.Key);
                            break;
                    }
                }
                #endregion

                string context = "";
                if (showDtl)
                {
                    context = "<TABLE class='TableCard'  >";
                    foreach (Attr attr in attrs)
                    {
                        if (attr.Key == showtext1)
                            continue;

                        switch (attr.MyFieldType)
                        {
                            case FieldType.Normal:
                                if (attr.UIVisible == true)
                                {
                                    if (en.GetValStringByKey(attr.Key) == "")
                                        continue;
                                    context += "<TR><TD nowrap class='TDLeft' >" + attr.Desc + "</TD><TD   class='RightTD' >" + en.GetValStringByKey(attr.Key) + "</TD></TR>";
                                }
                                break;
                            case FieldType.RefText:
                                if (en.GetValStringByKey(attr.Key) == "")
                                    continue;
                                context += "<TR><TD nowrap class='TDLeft' >" + attr.Desc.Replace(" Name ", "") + "</TD><TD   class='RightTD' >" + en.GetValStringByKey(attr.Key) + "</TD></TR>";
                                //context+="<TR><TD nowrap >"+attr.Desc.Replace(" Name ","")+"</TD><TD nowrap >"+en.GetValStringByKey( attr.Key )+"</TD></TR>";
                                break;
                        }
                    }
                    context += "</TABLE>";
                }

                string img = "<img src='" + en.EnMap.Icon + "'/>";
                if (i == 3)
                {
                    i = 0;
                    this.Add("<TD   valign=top ondblclick=\"WinOpen( '../Comm/RefFunc/UIEn.aspx?EnsName=" + ens.ToString() + "&PK=" + en.GetValByKey(pk) + "')\"  onmousedown=\"OnDGMousedown('" + this.Page.Request.ApplicationPath + "','" + ctrlId + "', '" + clName + "', '" + url + "')\"  >" + img + "&nbsp;<b>" + en.GetValStringByKey(showtext1) + "</b>" + context + "</TD>");
                    this.AddTREnd();
                }
                else
                {
                    this.Add("<TD class='TD' valign=top ondblclick=\"WinOpen( '../Comm/RefFunc/UIEn.aspx?EnsName=" + ens.ToString() + "&PK=" + en.GetValByKey(pk) + "')\"  onmousedown=\"OnDGMousedown('" + this.Page.Request.ApplicationPath + "','" + ctrlId + "', '" + clName + "', '" + url + "')\"  >" + img + "&nbsp;<b>" + en.GetValStringByKey(showtext1) + "</b>" + context + "</TD>");
                }
            }

            switch (i)
            {
                case 1:
                    this.Add("<TD class='TD' >&nbsp;</TD>");
                    this.Add("<TD   >&nbsp;</TD>");
                    this.AddTREnd();
                    break;
                case 2:
                    this.Add("<TD   >&nbsp;</TD>");
                    this.AddTREnd();
                    break;
            }
            this.Add("</TABLE>");
        }
        /// <summary>
        ///  View Files 
        /// </summary>
        /// <param name="en"></param>
        //		public void FilesView(string enName, string pk)
        public void FilesView(string enName, string pk)
        {
            this.Controls.Clear();
            SysFileManagers ens = new SysFileManagers(enName, pk);
            this.Add("<TABLE BORDER=1>");
            this.Add("<TR>");
            this.Add("<TD> Serial number </TD>");
            this.Add("<TD> File name </TD>");
            this.Add("<TD> Upload people </TD>");
            this.Add("<TD> Upload time </TD>");
            this.Add("<TD> Size </TD>");
            this.Add("<TD> Operating </TD>");
            this.AddTREnd();
            foreach (SysFileManager file in ens)
            {
                this.Add("<TR>");
                this.Add("<TD>" + file.OID + "</TD>");
                this.Add("<TD><img src='../Img/FileType/" + file.MyFileExt.Replace(".", "") + ".gif' border=0 /><a href='" + this.Request.ApplicationPath + file.MyFilePath + "' target='_blank' >" + file.MyFileName + file.MyFileExt + "</a></TD>");
                this.Add("<TD>" + file.RecText + "</TD>");
                this.Add("<TD>" + file.RDT + "</TD>");
                this.Add("<TD>" + file.MyFileSize + "</TD>");
                if (file.Rec == WebUser.No)
                {
                    this.Add("<TD><a href=\"javascript:DoAction('FileManager.aspx?OID=" + file.OID + "&EnsName=" + enName + "&PK=" + pk + "',' To be deleted  《" + file.MyFileName + "》')\" > Delete </a></TD>");
                }
                else
                {
                    this.Add("<TD>无</TD>");
                }
                this.AddTREnd();
            }
            this.Add("</TABLE>");
        }
        protected void Page_Load(object sender, System.EventArgs e)
        {
            //  Put user code to initialize the page here 
        }

        #region Web  Form Designer generated code 
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN:  This call is  ASP.NET Web  Form Designer required .
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		 Required method for Designer support  -  Do not use the code editor 
        ///		 Modify the contents of this method .
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion
    }
}

