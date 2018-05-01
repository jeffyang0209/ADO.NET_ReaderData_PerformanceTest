using DBConfig;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTable.Load
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
                    Stopwatch sw = new Stopwatch();
                    //CommandBehavior behavior = CommandBehavior.SequentialAccess;
                    //IDataReader reader = cmd.ExecuteReader(behavior);
                    sw.Start();
                    IDataReader reader = cmd.ExecuteReader();

                    // 效能測試開始
                    using (DataSet ds = new DataSet())
                    {
                        do
                        {
                            var table = new System.Data.DataTable();
                            table.Load(reader);
                            ds.Tables.Add(table);
                        } while (!reader.IsClosed);

                    }
                    reader.Close();
                    sw.Stop();
                    var time = sw.Elapsed;

                    Console.WriteLine("DataTable Load Elapsed={0}", time);
                    cmd.Parameters.Clear();
                }
            }
        }
    }
}
