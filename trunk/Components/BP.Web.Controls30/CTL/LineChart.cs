using System;
using System.Web.UI.WebControls; 
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Web.UI;
using System.Collections;
using System.Data;
using BP.En;
using BP.DA;
using System.ComponentModel;


namespace  BP.Web.Controls
{
	/// <summary>
	/// LineChart The summary .
	/// </summary>
	public class LineChart : System.Web.UI.WebControls.Image
	{
		public LineChart()
		{ 
			this.PreRender += new System.EventHandler(this.LineChartPreRender);
			//ScaleX = ImageWidth ; 
			//ScaleY = ImageHeight ;
			b = new Bitmap( ImageWidth ,  ImageHeight ) ;
			g = Graphics.FromImage (b) ;			
		}
		private void LineChartPreRender( object sender, System.EventArgs e )
		{
			if (this.ImageUrl==null || this.ImageUrl=="") 
				this.ImageUrl=System.Web.HttpContext.Current.Request.ApplicationPath+"/images/sys/LineChart.gif";
			//this.BorderStyle=System.Web.UI.WebControls.BorderStyle.Double;
			this.BorderColor=Color.Black;

		}

		public Bitmap b ;
		public string Title = "BP  Data charts " ;
		public ArrayList chartValues = new ArrayList() ;
		public float Xorigin =0 , Yorigin = 0 ;
		public bool IsShowLab=false;

		private DataTable _Table ;

		/// <summary>
		///  This data table stored in the point coordinates 
		/// </summary>
		public DataTable Table 
		{
			get
			{
				return this._Table;
			}
			set
			{
				this._Table = value;
				foreach(DataRow dr in this._Table.Rows)
				{
					this.AddEntity(float.Parse(dr["Cash"].ToString()),dr["KJND"].ToString()+dr["KJNY"].ToString()) ;
				}
			}
		}
		//public float ScaleX=1000 ; 
		/// <summary>
		/// Y Top   Money 
		/// </summary>
		public float YMaxCash=500000;
		//public float Xdivs=2;
		/// <summary>
		///  To y  Group growth points .
		/// </summary>
		public float YPontNum=20;
		/// <summary>
		///  Pixel width 
		/// </summary>
		public int ImageWidth=800;
		/// <summary>
		///  Height in pixels 
		/// </summary>
		public int ImageHeight=600;
		//	    public int posX=0;
		//		public int posY=0;
		private Graphics g ;
		//private Page p ;

		struct DataPoint 
		{
			public float x ;
			public float y ;
			public bool valid ;
			public float Cash;
			public string KJNY;
			 
		}		 
		/// <summary>
		///  Add a point 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void AddPoint( float x , float y, string kjny, float val ) 
		{
			DataPoint myPoint;
			myPoint.x = x ;
			myPoint.y = y ;
			myPoint.valid = true ;
			myPoint.KJNY=kjny;
			myPoint.Cash=val;
			chartValues.Add( myPoint ) ;
		}
		/// <summary>
		///  Add an entity 
		/// </summary>
		/// <param name="val"> Generally the amount </param>
		/// <param name="kjny"> General accounting years  format yyyym</param>
		public void AddEntity(float val , string kjny )
		{
			 
			if ( val > this.YMaxCash ) 
				throw new Exception("@Y  Maximum coordinate set ["+this.YMaxCash.ToString()+"] Lower than the amount shown in ["+val.ToString()+"].");
			float y= val/this.YMaxCash * this.ChartWidth-this.ChartInset;
		}
		/// <summary>
		///  Both sides of the border defined by the width .
		/// </summary>
		public int ChartInset
		{
			get
			{
				return 50;
			}
		}
		/// <summary>
		///  Width 
		/// </summary>
		public int ChartWidth 
		{
			get
			{
				return ImageWidth - ( 2 * ChartInset );
			}
		}
		/// <summary>
		///  Height 
		/// </summary>
		public int ChartHeight 
		{
			get
			{
				return ImageHeight - ( 2 * ChartInset );
			}
		}
		/// <summary>
		///  Fonts 
		/// </summary>
		public Font axesFont
		{
			get
			{
				return new Font ( "arial" , 10);
			}
		}
		public Pen RedPen
		{
			get
			{
				return new Pen(Color.Red, 3 ) ;
			}
		}
		public Pen BlackPen
		{
			get
			{
				return new Pen(Color.Black, 3 ) ;
			}
		}
		public Pen GradePen
		{
			get
			{
				//float i =float.Parse( "1");
				return new Pen(Color.DarkBlue,1 ) ;
			}
		}
		/// <summary>
		///  Title 
		/// </summary>
		public Brush BlackBrush 
		{
			get
			{
				return new SolidBrush ( Color.Black );	
			}
		} 
		public bool IsShowGirde=true;
		/// <summary>
		///  Whether to display gride . 
		/// </summary>
		public void Draw() 
		{
			int i ;
			float x , y , x0 , y0 ; 	 

			// First, the size of the image to be created 
			//p.Response.ContentType = "image/jpeg" ;
			//p.Response.ContentType = "image/jpeg" ;
			g.FillRectangle ( new SolidBrush ( Color.White ) , 0 , 0 , ImageWidth  , ImageHeight  ) ;
			 
			/// Draw a rectangle  
			g.DrawRectangle ( new Pen( Color.Black , 1) , ChartInset , ChartInset , ChartWidth , ChartHeight );
			// Write the contents of the text above picture picture  
			Font fon= new Font( " Times New Roman " , 14 );
			g.DrawString( Title , fon , BlackBrush , ImageWidth/4, 10 );	

			#region ÑØY Coordinate write Y Label 	
			 
			for (  i = 0 ; i <= YPontNum ; i++ )
			{
				x = ChartInset ;
				y = ChartHeight + ChartInset - ( i * ChartHeight / YPontNum ) ;
				string myLabel = ( Yorigin + ( YMaxCash * i / YPontNum ) ).ToString ( ) ;
				g.DrawString(myLabel , axesFont , BlackBrush , 5 , y - 6 ) ;
				g.DrawLine ( BlackPen , x + 2 , y , x - 2 , y ) ;  ///  Allowed to display a  dot  In Fig. .  There are two basic points combined into close .
				if (IsShowGirde && i > 0 )
				{
					g.DrawLine ( this.GradePen , ChartInset , y   , ChartInset +  ChartWidth  ,  y) ;
				}
			}
			g.RotateTransform ( 180 ) ;
			g.TranslateTransform ( 0 , - ChartHeight ) ;
			g.TranslateTransform ( - ChartInset , ChartInset ) ;
			g.ScaleTransform ( - 1 , 1);
			///  If you want to display gride .
			#endregion

			#region  Draw the data in the chart  
			DataPoint prevPoint = new DataPoint();
			prevPoint.valid = false ;
			foreach ( DataPoint myPoint in chartValues ) 
			{
				if ( prevPoint.valid == true ) 
				{
//					Xorigin = 2;
//					Yorigin=2;

//					x0 = ChartWidth * ( prevPoint.x - Xorigin ) /ImageWidth;
//					y0 = ChartHeight*( prevPoint.y - Yorigin )/ImageHeight;
//
//					x = ChartWidth * ( myPoint.x - Xorigin ) / ImageWidth ;
//					y = ChartHeight * ( myPoint.y - Yorigin ) / ImageHeight ;



										x0 =  prevPoint.x ;
										y0 =  prevPoint.y ;					
										x =   myPoint.x ;
										y =  myPoint.y ;

					
					g.DrawLine ( RedPen , x0 , y0 , x , y );
					//g.FillEllipse(BlackBrush , x0 - 2 , y0 - 2, 2 , 2 ) ;
					//g.FillEllipse( BlackBrush , x - 2 , y - 2 , 2 , 2 );

					g.FillEllipse(BlackBrush , x0-2, y0-2, 4, 4 ) ;
					g.FillEllipse( BlackBrush , x-2 , y-2, 4 , 4);

					//g.FillEllipse( BlackBrush , x0  , y0  , 1 , 1 ) ;
					//g.FillEllipse( BlackBrush , x  , y , 1 , 1 );

					if (this.IsShowLab)
					{
						g.DrawString( myPoint.KJNY+";"+myPoint.Cash.ToString(), new Font(" Times New Roman ",10), BlackBrush , x,y  );	
						//g.DrawString( "abc", new Font(" Times New Roman ",10), BlackBrush , x,y  );	
						//g.RotateTransform ( 180 ) ;
						//g.TranslateTransform ( 0 , - ImageHeight ) ;
						//g.TranslateTransform ( - ChartInset , ChartInset ) ;
						//g.ScaleTransform ( - 1 , 1);
					}
				}
				prevPoint = myPoint ;
			}
			#endregion	

            #region  Finally, in picture form to browse 
			string fileName=DBAccess.GenerOID().ToString()+".Jpeg";
			b.Save( ExportFilePath+ fileName, ImageFormat.Jpeg);
			this.ImageUrl=System.Web.HttpContext.Current.Request.ApplicationPath+"/Temp/" + fileName;
			#endregion
		}
	
		/// <summary>
		///  The path of the export file 
		/// </summary>
		protected string ExportFilePath
		{
			get
			{
				return this.Page.Request.PhysicalApplicationPath + "Temp\\";
			}
		}
		 
		/*
		LineChart() 
		{
			g.Dispose();
			b.Dispose();
		}*/
		 
	
	}
}
