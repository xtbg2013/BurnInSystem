using System.Data;
using System.Data.SqlClient;
using System;
using log4net;
namespace BILib
{
    public class DbService
    {
        private readonly SqlConnection _con;
        private readonly object _lockObj = new object();
        private readonly ILog _log;
        public DbService(ILog log,string constr)
        {
            _con = new SqlConnection(constr);
            _log = log;
        }

        public bool Execute(string cmd)
        {
            var res = true;
            lock (_lockObj)
            {
                _con.Open();
                try
                {
                    var sqlCmd = _con.CreateCommand();
                    sqlCmd.CommandText = cmd;
                    sqlCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    _log.Error($"Execute sql error ,message = {ex.Message} , cmd = {cmd}");
                    res = false;
                }
                finally
                {
                    _con.Close();
                }
            }
            return res;
        }
       
        public DataTable Query(string cmd)
        {
            var ret = new DataTable();
            lock (_lockObj)
            {
                try
                {
                   
                    var adp = new SqlDataAdapter(cmd, _con);
                    adp.Fill(ret);
                    
                }
                catch (Exception ex)
                {
                    _log.Error($"Query sql error ,message = {ex.Message} , cmd = {cmd}");

                }
                finally
                {
                    _con.Close();
                }
            }
            return ret;
        }

    }
}
