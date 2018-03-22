using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
namespace BIModel.Data
{
    [Serializable]
    public class UnitEntry
    {
        [XmlAttribute]
        public Guid Id { get; set; }
        [XmlAttribute]
        public string Sn { get; set; }
        [XmlAttribute]
        public string Plan { get; set; }
        [XmlAttribute]
        public string Board { get; set; }
        [XmlAttribute]
        public int Seat { get; set; }
        [XmlAttribute]
        public double Cost { get; set; }
        [XmlAttribute]
        public UnitState State { get; set; }
        [XmlAttribute]
        public UnitResult Result { get; set; }
        [XmlAttribute]
        public string Comment { get; set; }
        [XmlAttribute]
        public DateTime CreateTime { get; set; }
        [XmlAttribute]
        public DateTime CostCounter { get; set; }
        [XmlAttribute]
        public DateTime ReadCounter { get; set; }
        [XmlAttribute]
        public DateTime FinishTime { get; set; }
        [XmlAttribute]
        public bool OutSpec { get; set; }
        [XmlAttribute]
        public DateTime OutStart { get; set; }
    }
}
