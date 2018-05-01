using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRecord
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SqlConnection conn = new SqlConnection(@"Server=10.101.6.228;Database=MainDB;User Id=kevinlimantoro; Password=kevinlimantoro"))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("[dbo].[usp_Backend_FundsManagement_GetTransactionList]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(new SqlParameter("@intBrandID", 5));
                    parameters.Add(new SqlParameter("@dtStartTime", "2018/01/21"));
                    parameters.Add(new SqlParameter("@dtEndTime", "2018/04/25"));
                    parameters.Add(new SqlParameter("@intCurrencyID", 2));
                    parameters.Add(new SqlParameter("@strLanguageCode", "zh-tw"));
                    parameters.Add(new SqlParameter("@strMainAccountID", "wuwu66"));
                    parameters.Add(new SqlParameter("@intTransactionID", -1));
                    parameters.Add(new SqlParameter("@intStatus", -1));
                    parameters.Add(new SqlParameter("@strPlatformCode", "-1"));
                    parameters.Add(new SqlParameter("@intPlatformType", -1));
                    parameters.Add(new SqlParameter("@intBonusType", -1));
                    parameters.Add(new SqlParameter("@intPageNumber", Convert.ToInt16(0)));
                    parameters.Add(new SqlParameter("@intRecordCounts", 99999));
                    parameters.Add(new SqlParameter("@strOrderField", "LogTime"));
                    parameters.Add(new SqlParameter("@bitDesc", true));
                    SqlParameter outs = new SqlParameter("@strResult", SqlDbType.NVarChar, 10);
                    outs.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outs);
                    cmd.CommandTimeout = 60;
                    CommandBehavior behavior = CommandBehavior.SequentialAccess;
                    IDataReader reader = cmd.ExecuteReader(behavior);

                    using (DataSet ds = new DataSet())
                    {
                        do
                        {
                            DataTable dtSchema = reader.GetSchemaTable();
                            DataTable dt = new DataTable();
                            List<DataColumn> listCols = new List<DataColumn>();

                            if (dtSchema != null)
                            {
                                foreach (DataRow drow in dtSchema.Rows)
                                {
                                    string columnName = System.Convert.ToString(drow["ColumnName"]);
                                    DataColumn column = new DataColumn(columnName, (Type)(drow["DataType"]));
                                    column.Unique = (bool)drow["IsUnique"];
                                    column.AllowDBNull = (bool)drow["AllowDBNull"];
                                    column.AutoIncrement = (bool)drow["IsAutoIncrement"];
                                    listCols.Add(column);
                                    dt.Columns.Add(column);
                                }
                            }

                            while (reader.Read())
                            {
                                IDataRecord record = reader;
                                DataRow dataRow = dt.NewRow();
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    dataRow[i] = record[i];
                                }
                                dt.Rows.Add(dataRow);
                            }
                            ds.Tables.Add(dt);
                        } while (reader.NextResult());
                        reader.Close();
                    }
                }
            }
        }
    }
}
