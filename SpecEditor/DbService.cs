using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BILib
{
    public class DbService
    {
        private SqlConnection con = null;
        private object lockObj = new object();
        public DbService(string constr)
        {
            this.con = new SqlConnection(constr);
        }

        public void Execute(string cmd)
        {
            lock (lockObj)
            {
                this.con.Open();
                try
                {
                    var SqlCmd = con.CreateCommand();
                    SqlCmd.CommandText = cmd;
                    SqlCmd.ExecuteNonQuery();
                }
                finally
                {
                    this.con.Close();
                }
            }
        }


        public DataTable Query(string cmd)
        {
            lock (lockObj)
            {
                try
                {
                    DataTable ret = new DataTable();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd, this.con);
                    adp.Fill(ret);
                    return ret;
                }
                finally
                {
                    this.con.Close();
                }
            }
        }

    }
}
