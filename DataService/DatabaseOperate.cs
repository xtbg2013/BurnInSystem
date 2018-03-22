using System.Data;
using System.Data.SqlClient;
using System;
using log4net;

namespace BILib
{
    internal class DatabaseOperate
    {
        private readonly SqlConnection _con;
        private readonly object _lockObj = new object();
        private readonly ILog _log;
        public DatabaseOperate(ILog log, string constr)
        {
            _con = new SqlConnection(constr);
            _log = log;
        }

        public bool Execute(string cmdText)
        {
            var res = true;
            lock (_lockObj)
            {
                SqlTransaction transaction = null;
                try
                {
                    _con.Open();
                    transaction = _con.BeginTransaction();
                    var sqlCmd = _con.CreateCommand();
                    sqlCmd.Transaction = transaction;
                   
                    sqlCmd.CommandText = cmdText;
                    sqlCmd.ExecuteNonQuery();
                     
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    _log?.Error($"Execute sql error ,message = {ex.Message}");
                    res = false;
                }
                finally
                {
                    _con.Close();
                }
            }
            return res;
        }
        public bool Execute(string[] cmdText)
        {
            var res = true;
            lock (_lockObj)
            {
                SqlTransaction transaction = null;
                try
                {
                    _con.Open();
                    transaction = _con.BeginTransaction();
                    var sqlCmd = _con.CreateCommand();
                    sqlCmd.Transaction = transaction;
                    foreach (var cmd in cmdText)
                    {
                        sqlCmd.CommandText = cmd;
                        sqlCmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    _log?.Error($"Execute sql error ,message = {ex.Message}");
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
                    _log?.Error($"Query sql error ,message = {ex.Message} , cmd = {cmd}");

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

