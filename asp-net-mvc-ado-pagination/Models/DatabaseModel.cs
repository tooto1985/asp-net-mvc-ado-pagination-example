using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.Configuration;

namespace asp_net_mvc_ado_pagination.Models
{
    public class DatabaseModel
    {
        private SqlConnection conn;

        public DatabaseModel() {
            conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["NORTHWND"].ToString());
        }

        public DataTable Query(string sql) {
            using (SqlCommand cmd = new SqlCommand(sql, conn)) {
                if (conn.State != ConnectionState.Open) {
                    conn.Open();
                }
                using(SqlDataReader dr = cmd.ExecuteReader()) {
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    return dt;
                }
            }
        }
        
    }
}