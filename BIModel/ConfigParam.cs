using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace BIModel
{
    [DataContract]
    public class SystemParams
    {
        [DataMember]
        public string operatorId { set; get; }
        [DataMember]
        public string testStation { set; get; }
        [DataMember]
        public int comErrorTolarence { set; get; }
        [DataMember]
        public int conditionTimeout { set; get; }
        [DataMember]
        public int heatTime { set; get; }
        [DataMember]
        public string tbiesServer { set; get; }
    }
    
    [DataContract]
    public class ConfigParam
    {
        [DataMember]
        public SystemParams systemParam { set; get; }
        [DataMember]
        public Dictionary<string, string> ports { set; get; }
    }
}
