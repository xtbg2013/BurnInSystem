using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BIModel.Data;
using BIModel.runtime;
using BiInterface;

using BIModel.Interface;
using BIModel.Factory;
 
using BILib;
using log4net;

namespace BIModel.Manager
{
    class BoardManager
    {
        private ILog _Logger;
        private BoardFactory _boardFactory;
        private SystemParams _param;
        private readonly List<BoardEntry> _boardSet;
        private readonly List<BoardEntry> _validBoardSet;
        private IPosMapScheme _posMapScheme;
        
        private static string targetFile = Path.Combine(Utility.RunTimeDirectory,@"BoardRuntime.xml");
        public BoardManager(ILog logger, IDatabaseService dbService,IPosMapScheme posMapScheme, BoardFactory boardFactory, SystemParams param)
        {
            _Logger = logger;
            _param = param;
            _boardFactory = boardFactory;
            _posMapScheme = posMapScheme;
            
            Utility.Load(targetFile, out this._boardSet);
            if(this._boardSet==null)
                this._boardSet = new List<BoardEntry>();
            foreach (var entry in this._boardSet)
                if (entry.Flag == BoardState.READY)
                    ChangeBoardState(entry.BoardName, BoardState.LOADED);
            this._validBoardSet = new List<BoardEntry>();
        }


        public EventHandler BoardStateChanged { get; set; }
        public void SyncFromStore()
        {

            this._posMapScheme.GetDefaultPosMapBlock();
            foreach (var item in this._boardSet)
            {
                int floor, number;
                this._posMapScheme.GetPosition(item.BoardName, out floor, out number);
                IBoard controller = this._boardFactory.GenerateBoard(item.Plan, item.BoardName,floor,number);
                if (controller != null)
                {
                    this._validBoardSet.Add(item);
                    item.SetController(controller);
                    foreach (var seat in item.Seats)
                    {
                        item.GetController().AddSeat(seat.Seat, seat.Sn);
                    }
                }
                else
                {
                    foreach (var seat in item.Seats)
                    {
                        this._Logger.Error("empty controller: board name: " + item.BoardName + " seat = " + seat.Seat + " sn= " + seat.Sn);
                    }
                }
            }
        }

        public BoardEntry GetBoardInfo(string name)
        {
            var result = this._boardSet.FindAll(x => x.BoardName == name);
            return result.Any() ? (BoardEntry)result.First().Clone() : null;
        }
        public List<string> SelectBoardByState(BoardState state)
        {
            return (from x in this._validBoardSet where x.Flag == state select x.BoardName).ToList();
        }
        public void SubscribeBoard(string board,string plan)
        {
            if (this.GetBoardInfo(board) != null)
            {
                this._Logger.Warn("Board " + board + " has been subscribed.");
                return;
            }
            var entry = new BoardEntry()
            {
                BoardName = board,
                Flag = BoardState.SELECTED,
                Plan = plan,
                ErrorCount = 0,
            };
            int floor, number;
            this._posMapScheme.GetPosition(board, out floor, out number);
            IBoard controller = this._boardFactory.GenerateBoard(plan, board, floor, number);

            if (controller == null)
            {
                this._Logger.Error("create "+board+"'s controller error,subscribe board fail");
                return;
            }
                 
            entry.SetController(controller);
            this._boardSet.Add(entry);
            this._validBoardSet.Add(entry);
            this.BoardStateChanged?.Invoke(this, null);
            Utility.Dump(targetFile, this._boardSet);
        }

        public void UnsubscribeBoard(string board)
        {
            if (this.GetBoardInfo(board) == null)
            {
                this._Logger.Error("Board " + board + " hasn't been subscribed.");
                return;
            }
            this._boardSet.RemoveAll(x => x.BoardName==board);
            this._validBoardSet.RemoveAll(x => x.BoardName == board);
            Utility.Dump(targetFile, this._boardSet);
        }
        
        #region Board
        public void ChangeBoardState(string board, BoardState state)
        {
            var result = (from x in this._boardSet where x.BoardName == board select x);
            if (result.Any())
            {
                result.First().Flag = state;
                Utility.Dump(targetFile, this._boardSet);
                this.BoardStateChanged?.Invoke(this, null);
            }
        }

        public void AddSeatToBoard(string board, int seat, string sn)
        {
            var result = (from x in this._boardSet where x.BoardName == board select x);
            if (result.Any())
            {
                result.First().Seats.Add(new SeatEntry() {Seat = seat,Sn=sn});
                Utility.Dump(targetFile, this._boardSet);

                IBoard controller = result.First().GetController();
                if (controller != null)
                {
                   controller.AddSeat(seat, sn);
                }
                else
                {
                    this._Logger.Error("Board " + board + "'s Controller is null,add seat to boar fail.");
                }
                
                this.BoardStateChanged?.Invoke(this, null);
            }
        }

        public void RemoveSeatFromBoard(string board, int seat)
        {
            var result = (from x in this._boardSet where x.BoardName == board select x);
            if (result.Any())
            {
                result.First().Seats.RemoveAll(x=>x.Seat==seat);
                Utility.Dump(targetFile, this._boardSet);
                IBoard controller = result.First().GetController();
                if (controller != null)
                {
                    controller.RemoveSeat(seat);
                }
                this.BoardStateChanged?.Invoke(this, null);
            }
        }
        #endregion
    }
}
