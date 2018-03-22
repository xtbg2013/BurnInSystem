using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using BIModel.runtime;

namespace BIModel.Manager
{
    public class Utility
    {
        public static string Root => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"BMS");
        public static string RunTimeDirectory => Path.Combine(Root, "runtime");
        private static Dictionary<string, object> objDict = new Dictionary<string, object>();
        static Utility()
        {
            if (Directory.Exists(Root) == false)
                Directory.CreateDirectory(Root);
            if (Directory.Exists(RunTimeDirectory) == false)
                Directory.CreateDirectory(RunTimeDirectory);
        }
         
        public static string GetDefaultSnFile()
        {
            return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Configurations\ScanResult.txt");
        }
         
        public static void Load<T>(string targetFile, out T subject)
        {
            if (File.Exists(targetFile))
            {
                using (var fstream = new FileStream(targetFile, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    subject = (T)serializer.Deserialize(fstream);
                }
            }
            else
            {
                subject = default(T);
            }
        }
       
        public static void Dump<T>(string targetFile, T subject)
        {
            if(objDict.ContainsKey(targetFile)==false)
                objDict[targetFile]=new object();
            lock (objDict[targetFile])
            {
                new FileStream(targetFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None).Close();
                using (var fstream = new FileStream(targetFile, FileMode.Truncate, FileAccess.Write, FileShare.None))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(fstream, subject);
                }
            }
        }

     
    }
}
