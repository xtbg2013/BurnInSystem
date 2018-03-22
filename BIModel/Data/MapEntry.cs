using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BIModel.Data
{
    [Serializable]
    public class MapEntry
    {
        private string _name = "";
        [XmlAttribute]
        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new Exception("MapEntry::SetName::Invalid Input");
                _name = value;
            }
        }
        [XmlAttribute]
        public int Floor { get; set; }
        [XmlAttribute]
        public int Board { get; set; }
    }
    public class PosEntry
    {
        public string BoardName { set; get; }
        public int FloorNum { set; get; }
        public int BoardNum { set; get; }
    }

    public class PosMapBlock
    {
        public string SchemeName { get; set; }
        public List<PosEntry> PosList { get; }
        public int BoardRows { get; set; }
        public int BoardCols { get; set; }
        public int SeatRows { get; set; }
        public int SeatCols { get; set; }
        public bool Validation { get; set; }
        
        public PosMapBlock()
        {
            PosList = new List<PosEntry>();
        }
    }

}
