using DBConfig;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataReader
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 50; i++)
            {
                GO();
            }
        }

        static void GO()
        {
            using (SqlConnection conn = new SqlConnection(Config.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(Config.CmdText, conn))
                {
                    cmd.CommandType = Config.GetCommandType;
                    List<SqlParameter> parameters = Config.GetSqlParameters;
                    cmd.Parameters.AddRange(parameters.ToArray());

                    SqlParameter outs = new SqlParameter("@strResult", SqlDbType.NVarChar, 10);
                    outs.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outs);
                    cmd.CommandTimeout = 60;

                    Stopwatch sw = new Stopwatch();
                    //CommandBehavior behavior = CommandBehavior.SequentialAccess;
                    //IDataReader reader = cmd.ExecuteReader(behavior);
                    IDataReader reader = cmd.ExecuteReader();
                    sw.Start();
                    using (DataSet ds = new DataSet())
                    {
                        int index = 0;
                        do
                        {
                            DataTable dtSchema = reader.GetSchemaTable();

                            DataTable dt = new DataTable();
                            dt.TableName = string.Format("Table{0}", index == 0 ? string.Empty : index.ToString());
                            index++;
                            if (dtSchema != null)
                            {
                                foreach (DataRow drow in dtSchema.Rows)
                                {
                                    string columnName = System.Convert.ToString(drow["ColumnName"]);
                                    DataColumn column = new DataColumn(columnName, (Type)(drow["DataType"]));
                                    dt.Columns.Add(column);
                                }
                            }

                            while (reader.Read())
                            {
                                DataRow dataRow = dt.NewRow();
                                for (int i = 0; i <= reader.FieldCount - 1; i++)
                                {
                                    dataRow[i] = reader[i];
                                }
                                dt.Rows.Add(dataRow);
                            }
                            ds.Tables.Add(dt);
                        } while (reader.NextResult());
                        reader.Close();
                    }
                    sw.Stop();
                    var time = sw.Elapsed;

                    Console.WriteLine("DataReader Elapsed={0}", time);
                    cmd.Parameters.Clear();
                }
            }
        }
    }
}
