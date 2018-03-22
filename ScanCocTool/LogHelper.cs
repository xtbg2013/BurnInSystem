using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GlobalFiles
{
    public class LogHelper  
    {
        public static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("RunLogger");
        public static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("ErrorLogger");
        public static readonly log4net.ILog logeData = log4net.LogManager.GetLogger("DataLogger");

        public static void WriteDataLog(string info)
        {
            if (logeData.IsInfoEnabled)
            {
                logeData.Info(info);
            }
        }
        public static void Info(string info)
        {
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(info);
            }
        }

        public static void Debug(string info)
        {
            if (loginfo.IsDebugEnabled)
            {
                loginfo.Debug(info);
            }
        }

        public static void Warn(string info)
        {
            if (loginfo.IsWarnEnabled)
            {
                loginfo.Warn(info);
            }
        }
        public static void Error(string info)
        {
            if (loginfo.IsErrorEnabled)
            {
                loginfo.Error(info);
            }

            if (logerror.IsErrorEnabled)
            {
                logerror.Error(info);
            }
        }
       
        public static void Fetal(string info)
        {
            if (loginfo.IsFatalEnabled)
            {
                loginfo.Fatal(info);
            }

            if (logerror.IsFatalEnabled)
            {
                logerror.Fatal(info);
            }
        }
        


    }
}
