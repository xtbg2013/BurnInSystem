
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TbiesIntfDll
{
    [DataContract]
    public class ConditionItem
    {

        [DataMember]
        public string Item { get; set; }
        [DataMember]
        public string Value { get; set; }
    }
    [DataContract]
    public class SpecItem
    {
        [DataMember]
        public string Item { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string LBound { get; set; }
        [DataMember]
        public string UBound { get; set; }
    }
    [DataContract]
    public class SpecContent
    {
        [DataMember]
        public List<ConditionItem> Configuration { get; set; }
        [DataMember]
        public List<SpecItem> Specification { get; set; }
    }
    [DataContract]
    public class Specification
    {
        [DataMember]
        public string _id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string product { get; set; }
        [DataMember]
        public string version { get; set; }
        [DataMember]
        public string userId { get; set; }
        [DataMember]
        public string time { get; set; }
        [DataMember]
        public SpecContent content { get; set; }
        
    }
}
