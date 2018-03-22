using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace SpecEditor
{
    public class ConditionItem
    {
        public string Item { get; set; }
        public string Value { get; set; }
    }

    public class SpecItem
    {
        public string Item { get; set; }
        public string Type { get; set; }
        public string LBound { get; set; }
        public string UBound { get; set; }
    }
    public class BISpecification
    {
        public string Plan { get; set; }
        public string Version { get; set; }
        public string Driver { get; set; }
        public string Span { get; set; }
        public string Interval { get; set; }
        public List<ConditionItem> Configuration { get; set; }
        public List<SpecItem> Specification { get; set; }
        
        public static BISpecification Deserialize(string content)
        {
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(BISpecification));
                return (BISpecification) json.ReadObject(stream);
            }
        }

        public static string Serialize(BISpecification obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(BISpecification));
                json.WriteObject(stream,obj);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
    }
}
