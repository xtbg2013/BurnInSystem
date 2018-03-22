using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using BiInterface;
using BIModel.Data;

namespace BIModel.runtime
{
    [Serializable]
    public class BoardEntry:ICloneable
    {
        [XmlAttribute]
        public string BoardName { get; set; }

        [XmlAttribute]
        public string Plan { get; set; }
        [XmlAttribute]
        public BoardState Flag;
        
        [XmlAttribute]
        public int ErrorCount { get; set; }

        public List<SeatEntry> Seats = new List<SeatEntry>();

        private IBoard Controller = null;

        public IBoard GetController()
        {
            return Controller;
        }

        public void SetController(IBoard controller)
        {
            Controller = controller;
        }
        public object Clone()
        {
            return new BoardEntry()
            {
                BoardName = this.BoardName,
                Plan=this.Plan,
                Flag = this.Flag,
                ErrorCount = this.ErrorCount,
                Controller = this.Controller,
                Seats = this.Seats,
            };
        }
    }

}
