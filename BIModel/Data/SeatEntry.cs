using System;
using System.Xml.Serialization;

namespace BIModel.Data
{
    [Serializable]
    public class SeatEntry
    {
        [XmlAttribute] public int Seat { get; set; }
        [XmlAttribute] public string Sn { get; set; }
    }
}
