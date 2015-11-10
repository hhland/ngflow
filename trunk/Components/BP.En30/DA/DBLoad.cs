using System;
using System.IO;
using System.Data;
using System.Data.OracleClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.SqlClient;

namespace BP.DA
{
	/// <summary>
	/// DBLoad  The summary .
	/// </summary>
	public class DBLoad
	{
		static  DBLoad()
		{
		}
		public static int ImportTableInto(DataTable impTb ,string intoTb, string select ,int clear)
		{
			int count = 0;
			DataTable target = null ;

			// Whether the first empty before importing 
			if( clear==1)
				DBAccess.RunSQL( "delete from " + intoTb );

			try
			{
				target = DBAccess.RunSQLReturnTable( select );
			}
			catch(Exception ex) //select Query Error , Column may be missing 
			{
				throw new Exception(" The source table is malformed , Please check !"+ex.Message +" :"+select);
			}

			object conn = DBAccess.GetAppCenterDBConn;

			SqlDataAdapter sqlada = null;
			OracleDataAdapter oraada = null;
			DBType dbt = DBAccess.AppCenterDBType;
			if( dbt == DBType.MSSQL )
			{
				sqlada = new SqlDataAdapter( select ,(SqlConnection)DBAccess.GetAppCenterDBConn );
				SqlCommandBuilder bl = new SqlCommandBuilder( sqlada );
				sqlada.InsertCommand = bl.GetInsertCommand();

				count = ImportTable( impTb,target,sqlada );
			}
			else if( dbt == DBType.Oracle )
			{
				oraada = new OracleDataAdapter( select ,(OracleConnection)DBAccess.GetAppCenterDBConn );
				OracleCommandBuilder bl = new OracleCommandBuilder( oraada );
				oraada.InsertCommand = bl.GetInsertCommand();

				count = ImportTable( impTb,target,oraada );
			}
			else
				throw new Exception( " Did not get a database connection ! " );

			target.Dispose();
			return count;
		}
		private static int ImportTable( DataTable source ,DataTable target , SqlDataAdapter sqlada )
		{
			int count = 0;
			try
			{
				if( sqlada.InsertCommand.Connection.State!= ConnectionState.Open )
					sqlada.InsertCommand.Connection.Open();
				sqlada.InsertCommand.Transaction = sqlada.InsertCommand.Connection.BeginTransaction();
				source.Columns.Add(" Error message ",typeof( string));
				source.Columns[" Error message "].MaxLength = 1000;

				int i =0;
				while( i < source.Rows.Count ) 	//for( int i=0;i<;i++)
				{
					for( int c=0;c< target.Columns.Count ;c++)
					{
						sqlada.InsertCommand.Parameters[c].Value = source.Rows[i][c];
					}
					try// Individual record failed , Jump over 
					{
						sqlada.InsertCommand.ExecuteNonQuery();
					}
					catch( Exception ex )
					{
						source.Rows[i][" Error message "] =ex.Message;
						i++;
						continue;
					}
					count++; // The number of records have been imported 
					source.Rows.RemoveAt( i );
				}
				sqlada.InsertCommand.Transaction.Commit();
			}
			catch( Exception ex)
			{
				if( sqlada.InsertCommand.Transaction!=null)
					sqlada.InsertCommand.Transaction.Rollback();
				sqlada.InsertCommand.Connection.Close();
				throw new Exception( " Importing data failed !"+ex.Message );
			}
			return count;
		}
		private static int ImportTable( DataTable source ,DataTable target , OracleDataAdapter oraada )
		{
			int count = 0;
			try
			{
				if( oraada.InsertCommand.Connection.State!= ConnectionState.Open )
					oraada.InsertCommand.Connection.Open();
				oraada.InsertCommand.Transaction = oraada.InsertCommand.Connection.BeginTransaction();
				int i =0;
				while( i < source.Rows.Count ) 	//for( int i=0;i<;i++)
				{
					for( int c=0;c< target.Columns.Count ;c++)
					{
						oraada.InsertCommand.Parameters[c].Value = source.Rows[i][c];
					}
//					if( i>6 )
//						throw new Exception( "Test!" );
					try// Individual record failed , Jump over 
					{
						oraada.InsertCommand.ExecuteNonQuery();
					}
					catch
					{
						i++;
						continue;
					}
					count++; // The number of records have been imported 
					source.Rows.RemoveAt( i );
				}
				oraada.InsertCommand.Transaction.Commit();
			}
			catch( Exception ex)
			{
				if( oraada.InsertCommand.Transaction!=null)
					oraada.InsertCommand.Transaction.Rollback();
				oraada.InsertCommand.Connection.Close();
				throw new Exception( " Importing data failed !"+ex.Message );
			}
			return count;
		}
     
        public static string GenerFirstTableName(string fileName)
        {
            return GenerTableNameByIndex(fileName, 0);
        }
        public static string GenerTableNameByIndex(string fileName,int index)
        {
            String[] excelSheets = GenerTableNames(fileName);
            if (excelSheets != null && excelSheets.Length >= index)
            {
                return excelSheets[index];
            }
            return null;
        }
        public static string[] GenerTableNames(string fileName)
        {
            
            string strConn = "Provider=Microsoft.Jet.Oledb.4.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'";
            try
            {
                if (fileName.ToLower().Contains(".xlsx"))
                {
                    // strConn = "Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
                    strConn= "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties=Excel 12.0;";
                }

                OleDbConnection con = new OleDbConnection(strConn);
                con.Open();
                // Worksheet to calculate how many there are sheet   
                DataTable dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dt == null)
                    return null;
                               
                String[] excelSheets = new String[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    excelSheets[i] = dt.Rows[i]["TABLE_NAME"].ToString();
                }

                con.Close();
                con.Dispose();
                return excelSheets;
            }
            catch (Exception ex)
            {
                throw new Exception("@ Get table An error :" + ex.Message + strConn);
            }
        }

        public static DataTable GetTableByExt(string filePath)
        {
            return GetTableByExt(filePath,null);
        }

		/// <summary>
		///  By file ,sql , Take out Table.
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="sql"></param>
		/// <returns></returns>
        public static DataTable GetTableByExt(string filePath,string sql)
        {
            DataTable Tb = new DataTable("Tb");
            Tb.Rows.Clear();

            string typ = System.IO.Path.GetExtension(filePath).ToLower();
            string strConn;
            switch (typ.ToLower() )
            {
                case ".xls":
                    if (sql==null)
                        sql = "SELECT * FROM [" + GenerFirstTableName(filePath) + "]";
                    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source =" + filePath + ";Extended Properties=Excel 8.0";
                    System.Data.OleDb.OleDbConnection conn = new OleDbConnection(strConn);
                    OleDbDataAdapter ada = new OleDbDataAdapter(sql, conn);
                    try
                    {
                        conn.Open();
                        ada.Fill(Tb);
                        Tb.TableName = Path.GetFileNameWithoutExtension(filePath);
                    }
                    catch (System.Exception ex)
                    {
                        conn.Close();
                        throw ex;//(ex.Message);
                    }
                    conn.Close();
                    break;
                case ".xlsx":
                    if (sql == null)
                        sql = "SELECT * FROM [" + GenerFirstTableName(filePath)+"]";
                    try
                    {
                        //strConn = "Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES\"";
                        strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=Excel 12.0;";
                        System.Data.OleDb.OleDbConnection conn121 = new OleDbConnection(strConn);
                        OleDbDataAdapter ada91 = new OleDbDataAdapter(sql, conn121);
                        conn121.Open();
                        ada91.Fill(Tb);
                        Tb.TableName = Path.GetFileNameWithoutExtension(filePath);
                        conn121.Close();
                        ada91.Dispose();
                    }
                    catch (System.Exception ex1)
                    {
                        try
                        {
                            strConn = "Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\"";
                            System.Data.OleDb.OleDbConnection conn1215 = new OleDbConnection(strConn);
                            OleDbDataAdapter ada919 = new OleDbDataAdapter(sql, conn1215);
                            ada919.Fill(Tb);
                            Tb.TableName = Path.GetFileNameWithoutExtension(filePath);
                            ada919.Dispose();
                            conn1215.Close();
                        }
                        catch
                        {

                        }
                        throw ex1;//(ex.Message);
                    }
                    break;
                case ".dbf":
                    strConn = "Driver={Microsoft dBASE Driver (*.DBF)};DBQ=" + System.IO.Path.GetDirectoryName(filePath) + "\\"; //+FilePath;//
                    OdbcConnection conn1 = new OdbcConnection(strConn);
                    OdbcDataAdapter ada1 = new OdbcDataAdapter(sql, conn1);
                    conn1.Open();
                    try
                    {
                        ada1.Fill(Tb);
                    }
                    catch//(System.Exception ex)
                    {
                        try
                        {
                            int sel = ada1.SelectCommand.CommandText.ToLower().IndexOf("select") + 6;
                            int from = ada1.SelectCommand.CommandText.ToLower().IndexOf("from");
                            ada1.SelectCommand.CommandText = ada1.SelectCommand.CommandText.Remove(sel, from - sel);
                            ada1.SelectCommand.CommandText = ada1.SelectCommand.CommandText.Insert(sel, " top 10 * ");
                            ada1.Fill(Tb);
                            Tb.TableName = "error";
                        }
                        catch (System.Exception ex)
                        {
                            conn1.Close();
                            throw new Exception(" Read DBF Data failed !" + ex.Message + " SQL:" + sql);
                        }
                    }
                    conn1.Close();
                    break;
                default:
                    break;
            }
            return Tb;
        }
	}
}
