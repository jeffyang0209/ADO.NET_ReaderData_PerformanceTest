using DBConfig;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAdapter.Fill
{
    class Program
    {
        static void Main(string[] args)
        {
            for(int i = 0; i < 50; i++)
            {
                Go();
            }
        }

        private static void Go()
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
                    using (SqlDataAdapter sa = new SqlDataAdapter(cmd))
                    {
                        using (DataSet ds = new DataSet())
                        {
                            sw.Start();
                            sa.Fill(ds);
                            sw.Stop();
                            var time = sw.Elapsed;
                            Console.WriteLine("DataAdapter.Fill={0}", time);

                        }
                    }

                    cmd.Parameters.Clear();
                }
            }
        }
    }
}
