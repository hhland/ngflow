namespace BP.Web.UC
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Data;
	using System.Data.SqlClient;
	using System.Data.Odbc ;
	using System.Drawing;
	using System.Web;
	using System.Web.SessionState;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using BP.En;
	using BP.DA;
    using BP.Sys;
    //using Microsoft.Office;
    //using Microsoft.Office.Interop;
    //using Microsoft.Web.UI.WebControls;
  //  using Microsoft.Office.Interop.Owc11;
    //using OWC10;

	/// <summary>
	///		UCGraphics  The summary .
	/// </summary>
    public partial class UCGraphics : UCBase
    {

        #region 3 d
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="colOfGroupField"></param>
        /// <param name="colOfGroupName"></param>
        /// <param name="colOfNumField"></param>
        /// <param name="colOfNumName"></param>
        /// <param name="title"></param>
        /// <param name="chartHeight"></param>
        /// <param name="chartWidth"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static string GenerChart(DataTable dt, string colOfGroupField, string colOfGroupName,
            string colOfNumField, string colOfNumName, string title, int chartHeight, int chartWidth, ChartType ct)
        {
            string strCategory = ""; // "1" + '\t' + "2" + '\t' + "3" + '\t'+"4" + '\t' + "5" + '\t' + "6" + '\t';
            string strValue = ""; // "9" + '\t' + "8" + '\t' + "4" + '\t'+"10" + '\t' + "12" + '\t' + "6" + '\t';
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
            //        ThisChChart.PlotArea.Interior.Color = "red";
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

            // Exporting an image file 
            //ThisChart.ExportPicture("G:\\chart.gif","gif",600,350);

            string fileName = ct.ToString() + PubClass.GenerTempFileName("GIF");
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
        ///  Produce 2 Latitude table 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="colOfGroupField1"></param>
        /// <param name="colOfGroupName1"></param>
        /// <param name="colOfGroupField2"></param>
        /// <param name="colOfGroupName2"></param>
        /// <param name="colOfNumField"></param>
        /// <param name="colOfNumName"></param>
        /// <param name="title"></param>
        /// <param name="chartHeight"></param>
        /// <param name="chartWidth"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public static string GenerChart2D(DataTable dt, string colOfGroupField1, string colOfGroupName1,
            string colOfGroupField2, string colOfGroupName2,
            string colOfNumField, string colOfNumName, string title, int chartHeight, int chartWidth, ChartType ct)
        {
            string strCategory = ""; // "1" + '\t' + "2" + '\t' + "3" + '\t'+"4" + '\t' + "5" + '\t' + "6" + '\t';
            string strValue = ""; // "9" + '\t' + "8" + '\t' + "4" + '\t'+"10" + '\t' + "12" + '\t' + "6" + '\t';

            //// Statement object 
            //ChartSpace ThisChart = new ChartSpaceClass();
            //ChChart ThisChChart = ThisChart.Charts.Add(0);
            ////ChSeries ThisChSeries = ThisChChart.SeriesCollection.Add(0);

            //// Show legend 
            //ThisChChart.HasLegend = true;
            //// Title 
            //ThisChChart.HasTitle = true;
            //ThisChChart.Title.Caption = title;

            //// Given x,y Axis illustration 
            //ThisChChart.Axes[0].HasTitle = true;
            //ThisChChart.Axes[1].HasTitle = true;

            ////			ThisChChart.Axes[0].Title.Caption = colOfGroupName1;
            ////			ThisChChart.Axes[1].Title.Caption = colOfNumName;

            //switch (ct)
            //{
            //    case ChartType.Histogram:
            //        ThisChChart.Type = ChartChartTypeEnum.chChartTypeColumnClustered;
            //        DataTable dtC = dt.Clone();
            //        int j = -1;
            //        foreach (DataRow dr1 in dtC.Rows)
            //        {
            //            j++;
            //            ChSeries ThisChSeries = ThisChChart.SeriesCollection.Add(j);
            //            ThisChChart.SeriesCollection[j].DataLabelsCollection.Add();
            //            // Given series Name 
            //            ThisChChart.SeriesCollection[j].SetData(ChartDimensionsEnum.chDimSeriesNames,
            //                (int)ChartSpecialDataSourcesEnum.chDataLiteral, dr1[colOfGroupField1].ToString());

            //            strCategory = "";
            //            strValue = "";
            //            foreach (DataRow dr in dt.Rows)
            //            {
            //                if (dr1[colOfGroupField1].Equals(dr[colOfGroupField1]) == false)
            //                    continue;

            //                strCategory += dr[colOfGroupField1].ToString() + '\t' + dr[colOfGroupField2].ToString() + '\t';
            //                strValue += dr[colOfNumField].ToString() + '\t';
            //            }

            //            // Given classification 
            //            ThisChSeries.SetData(ChartDimensionsEnum.chDimCategories,
            //                (int)ChartSpecialDataSourcesEnum.chDataLiteral, strCategory);
            //            // Setpoint 
            //            ThisChSeries.SetData
            //                (ChartDimensionsEnum.chDimValues,
            //                (int)ChartSpecialDataSourcesEnum.chDataLiteral, strValue);
            //        }
            //        //					ThisChChart.Overlap = 50;
            //        //					// Rotation 
            //        //					ThisChChart.Rotation  = 360;
            //        //					ThisChChart.Inclination = 10;
            //        //					// Background Color 
            //        //					ThisChChart.PlotArea.Interior.Color = "red";
            //        //					// Background 
            //        //					ThisChChart.PlotArea.Floor.Interior.Color = "green";
            //        //// Given series Name 
            //        //ThisChSeries.SetData(ChartDimensionsEnum.chDimSeriesNames,ChartSpecialDataSourcesEnum.chDataLiteral.GetHashCode(),"ssdd" );
            //        // Given classification 
            //        //ThisChSeries.SetData(ChartDimensionsEnum.chDimCategories,ChartSpecialDataSourcesEnum.chDataLiteral.GetHashCode(),strCategory);
            //        // Setpoint 
            //        //ThisChSeries.SetData(ChartDimensionsEnum.chDimValues,ChartSpecialDataSourcesEnum.chDataLiteral.GetHashCode(),strValue);
            //        break;
            //    case ChartType.Pie:
            //        //  Generate data 
            //        foreach (DataRow dr in dt.Rows)
            //        {
            //            strCategory += dr[colOfGroupField1].ToString() + '\t';
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
            //            strCategory += dr[colOfGroupField1].ToString() + '\t';
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


            // Exporting an image file 
            //ThisChart.ExportPicture("G:\\chart.gif","gif",600,350);

            string fileName = ct.ToString() + PubClass.GenerTempFileName("GIF");
            string strAbsolutePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\Temp\\" + fileName;

#warning  Commented out .
            //try
            //{
            //    ThisChart.ExportPicture(strAbsolutePath, "GIF", chartWidth, chartHeight);
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("@ You can not create file , Permission may be a problem , Please set the directory anyone can modify ." + strAbsolutePath + " Exception:" + ex.Message);
            //}
            return fileName;
            //
            //
            //			// Create GIF Relative path to the file . 
            //			string strRelativePath = "./Temp/"+fileName;
            //
            //			// Adding to the picture placeholder.  onmousedown=\"CellDown('Cell')\"
            //			//string strImageTag = "<IMG SRC='../../Temp/" + fileName + "'  />"; 
            //			return strRelativePath ;
        }
        #endregion

        #region 2 Latitude Graphics 
        #endregion

        protected void Page_Load(object sender, System.EventArgs e)
        {
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
