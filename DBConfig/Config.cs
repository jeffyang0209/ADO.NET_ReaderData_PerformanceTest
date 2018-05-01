using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConfig
{
    public class Config
    {
        /// <summary>
        /// 連線字串
        /// </summary>
        public static readonly string ConnectionString = @"Server=.;Database=MainDB;User Id = UserID; Password=Password";

        /// <summary>
        /// SQL語法
        /// </summary>
        public static readonly string CmdText = @"[dbo].[usp_Backend_FundsManagement_GetTransactionList]";

        public static CommandType GetCommandType { get; } = CommandType.StoredProcedure;

        public static List<SqlParameter> GetSqlParameters { get; } = new List<SqlParameter>()
        {
            new SqlParameter("@intBrandID", 5),
            new SqlParameter("@dtStartTime", "2017/10/21"),
            new SqlParameter("@dtEndTime", "2018/04/25"),
            new SqlParameter("@intCurrencyID", 2),
            new SqlParameter("@strLanguageCode", "zh-tw"),
            new SqlParameter("@strMainAccountID", "wuwu66"),
            new SqlParameter("@intTransactionID", -1),
            new SqlParameter("@intStatus", -1),
            new SqlParameter("@strPlatformCode", "-1"),
            new SqlParameter("@intPlatformType", -1),
            new SqlParameter("@intBonusType", -1),
            new SqlParameter("@intPageNumber", Convert.ToInt16(0)),
            new SqlParameter("@intRecordCounts", 99999),
            new SqlParameter("@strOrderField", "LogTime"),
            new SqlParameter("@bitDesc", true)
        };
    }
}