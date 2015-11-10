using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


public partial class Comm_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ////创建一个图形容器对象
        // ChartSpace objCSpace = new  ChartSpaceClass();
        ////在图形容器中增加一个图形对象
        // ChChart objChart = objCSpace.Charts.Add(0);
        ////将图形的类型设置为柱状图的一种
        //objChart.Type = ChartChartTypeEnum.chChartTypeColumnStacked;
        ////将图形容器的边框颜色设置为白色
        //objCSpace.Border.Color = "White";

        ////显示标题
        //objChart.HasTitle = true;
        ////设置标题内容
        //objChart.Title.Caption = "统计图测试";
        ////设置标题字体的大小
        //objChart.Title.Font.Size = 10;
        ////设置标题为粗体
        //objChart.Title.Font.Bold = true;
        ////设置标题颜色为红色
        //objChart.Title.Font.Color = "Red";

        ////显示图例
        //objChart.HasLegend = true;
        ////设置图例字体大小
        //objChart.Legend.Font.Size = 10;
        ////设置图例位置为底端
        //objChart.Legend.Position = ChartLegendPositionEnum.chLegendPositionBottom;

        ////在图形对象中添加一个系列
        //objChart.SeriesCollection.Add(0);
        ////给定系列的名字
        //objChart.SeriesCollection[0].SetData(ChartDimensionsEnum.chDimSeriesNames,
        //    +(int)ChartSpecialDataSourcesEnum.chDataLiteral, "指标");
        ////给定值
        //objChart.SeriesCollection[0].SetData(ChartDimensionsEnum.chDimValues,
        //    +(int)ChartSpecialDataSourcesEnum.chDataLiteral, "10\t40\t58\t55\t44");

        ////显示数据，创建GIF文件的相对路径.
        //string FileName = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".gif";
        //objCSpace.ExportPicture(BP.Sys.SystemConfig.PathOfTemp + "/Pie.gif", "GIF", 450, 300);
        //Image1.ImageUrl = "Http://localhost/PG/temp/Pie.gif";
    }
}
