using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace BIModel.NowCoding
{
    [Serializable]
    public class BoardInfo
    {
         
        [XmlAttribute]
        public string BoardName { get; set; }
        [XmlAttribute]
        public string Plan { get; set; }
        [XmlAttribute]
        public double temperature { get; set; }
        [XmlAttribute]
        public BoardState Flag;
        [XmlAttribute]
        public int ErrorCount { get; set; }
        [XmlElement]
        public List<UnitInfo> units { get; set; }

    }
}
