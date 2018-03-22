using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Net;

namespace TbiesIntfDll
{
    class Utils
    {
        public class Json
        {
            public static string ExtractList(string jsonStr)
            {
                int head = jsonStr.IndexOf('[');
                int tail = jsonStr.LastIndexOf(']');
                return jsonStr.Substring(head, tail - head + 1);
            }
            public static string ToJson(object item)
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(item.GetType());
                using (MemoryStream ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, item);
                    StringBuilder sb = new StringBuilder();
                    sb.Append(Encoding.UTF8.GetString(ms.ToArray()));
                    return sb.ToString();
                }
            }

            public static T ToObject<T>(string jsonString)
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
                {
                    T jsonObject = (T)ser.ReadObject(ms);
                    return jsonObject;
                }
            }
        }
        public class Http
        {
            public static string Get(string Url, string postDataStr = "")
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
                    request.Method = "GET";
                    request.ContentType = "text/html;charset=UTF-8";

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream myResponseStream = response.GetResponseStream();
                    StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                    string retString = myStreamReader.ReadToEnd();
                    myStreamReader.Close();
                    myResponseStream.Close();

                    Console.Write(retString);

                    return retString;
                }
                catch (Exception ex)
                {
                    throw new Exception("Http get error : " + ex.ToString());
                }
            }
        }
    }
}
