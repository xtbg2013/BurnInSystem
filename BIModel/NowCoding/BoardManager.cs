using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using BiInterface;

using BIModel.Factory;
using BIModel.Manager;
using BIModel.Interface;
using BILib;
using log4net;

namespace BIModel.NowCoding
{
    public class BoardManager
    {
        private static string targetFile = Path.Combine(Utility.RunTimeDirectory, @"BoardRuntime1.xml");
        private ILog _Logger;
        private BoardFactory _boardFactory;
        private SystemParams _param;
        private IDatabaseService _dataService;
        private List<BoardUnit> _boardSet;
        private readonly IPosMapScheme _posMapScheme;
        private void LoadBoardInfos(out List<BoardInfo> boardInfos)
        {
            try
            {
                Utility.Load(targetFile, out boardInfos);
                if (boardInfos == null)
                    boardInfos = new List<BoardInfo>();
            }
            catch (Exception)
            {
                throw new FormatException("BoardRuntime1.xml format exception");
            }
        }
        private bool IsSnExist(string sn)
        {
            foreach (var unit in this._boardSet)
            {
                if (unit.GetBoardInfo().units.FindAll(x => x.Sn == sn).Any())
                    return true;
            }
            return false;
        }
        private UnitInfo CreateUnitInfo(string boardName, int seat, string sn, string plan)
        {
            var driverType = FetchPlans.Inst(this._param.tbiesServer).GetDriverType(plan);
            Guid id = _dataService.CreateBiRecord(sn, driverType, plan, boardName, seat.ToString());
            
            UnitInfo info = new UnitInfo()
            {
                Id = id,
                Sn = sn,
                Plan = plan,
                Board = boardName,
                Seat = seat,
                Cost = 0,
                Comment = "",
                State = UnitState.READY,
                Result = UnitResult.PASS,
                CreateTime = DateTime.Now,
                CostCounter = DateTime.Now,
                ReadCounter = DateTime.Now,
                FinishTime = DateTime.Now,
                OutSpec = false,
                OutStart = DateTime.Now,
            };
            return info;
        }
        private void SaveUnitInfoToDb(UnitInfo info, double temperature)
        {
           
        }

        public EventHandler BoardStateChanged { get; set; }
        public EventHandler ProductsUpdate {
            get { return BoardUnit.ProductsUpdate; }
            set { BoardUnit.ProductsUpdate = value; }
        }

        public BoardManager(ILog logger, IDatabaseService dataService,IPosMapScheme posMapScheme, BoardFactory boardFactory, SystemParams param)
        {
            _Logger = logger;
            _param = param;
            _boardFactory = boardFactory;
            _dataService = dataService;
            _posMapScheme = posMapScheme;
            _boardSet = new List<BoardUnit>();
        }
        
        public void LoadBoardUnits()
        {
            List<BoardInfo> boardInfos;
            LoadBoardInfos(out boardInfos);
            foreach (var item in boardInfos)
            {
                if (item.Flag == BoardState.READY)
                    ChangeBoardState(item.BoardName, BoardState.LOADED);

                BoardUnit unit = new BoardUnit(this._Logger,this._dataService, this._param);
                unit.SetBoardInfo(item);
                int floor, number;
                this._posMapScheme.GetPosition(item.BoardName, out floor, out number);
                IBoard controller = this._boardFactory.GenerateBoard(item.Plan, item.BoardName, floor, number);
                if (controller != null)
                {
                    unit.SetController(controller);
                    foreach (var seat in item.units)
                    {
                        unit.GetController().AddSeat(seat.Seat, seat.Sn);
                    }
                    this._boardSet.Add(unit);
                }
                else
                {
                    foreach (var seat in item.units)
                    {
                        this._Logger.Error("empty controller: board name: " + item.BoardName + " seat = " + seat.Seat + " sn= " + seat.Sn);
                    }
                }
            }
        }
        public void SubscribeBoard(string boardName, string plan)
        {
            if (this.GetBoardUnit(boardName) != null)
            {
                this._Logger.Warn("Board " + boardName + " has been subscribed.");
                return;
            }

            int floor, number;
            this._posMapScheme.GetPosition(boardName, out floor, out number);
            IBoard controller = this._boardFactory.GenerateBoard(plan, boardName, floor,number);
            if (controller == null)
            {
                this._Logger.Error("create " + boardName + "'s controller error,subscribe board fail");
                return;
            }
            var info = new BoardInfo()
            {
                BoardName = boardName,
                Flag = BoardState.SELECTED,
                Plan = plan,
                ErrorCount = 0,
                units = new List<UnitInfo>()
            };

            BoardUnit unit = new BoardUnit(this._Logger, this._dataService, this._param);
            unit.SetBoardInfo(info);
            unit.SetController(controller);
            this._boardSet.Add(unit);
            BoardStateChanged?.Invoke(this, null);
            DumpBoardInfos();

        }
        public void UnsubscribeBoard(string boardName)
        {
            if (this.GetBoardUnit(boardName) == null)
            {
                this._Logger.Error("Board " + boardName + " hasn't been subscribed.");
                return;
            }
            this._boardSet.RemoveAll(x => x.GetBoardInfo().BoardName == boardName);
            DumpBoardInfos();
        }
        public BoardUnit GetBoardUnit(string boardName)
        {
            var result = this._boardSet.FindAll(x => x.GetBoardInfo().BoardName == boardName);
            return result.Any() ? (BoardUnit)result.First() : null;
        }

        public void DumpBoardInfos()
        {
            List<BoardInfo> boardInfos = new List<BoardInfo>();
            foreach (var unit in this._boardSet)
            {
                boardInfos.Add(unit.GetBoardInfo());
            }
            Utility.Dump(targetFile, boardInfos);
        }
        public List<string> SelectBoardByState(BoardState state)
        {
            return (from x in this._boardSet where x.GetBoardInfo().Flag == state select x.GetBoardInfo().BoardName).ToList();
        }
        public void ChangeBoardState(string boardName, BoardState state)
        {
            var result = (from x in this._boardSet where x.GetBoardInfo().BoardName == boardName select x);
            if (result.Any())
            {
                result.First().GetBoardInfo().Flag = state;
                DumpBoardInfos();
                BoardStateChanged?.Invoke(this, null);
            }
        }
        public void AddSeatToBoard(string boardName, int seat, string sn)
        {
            if (IsSnExist(sn))
            {
                this._Logger.Error(sn + " exists in current system.");
                return;
            }
            this._Logger.Info("add >>>>\t" + boardName+"_"+seat+"\t"+sn );
            var result = (from x in this._boardSet where x.GetBoardInfo().BoardName == boardName select x);
            if (result.Any())
            {
                BoardUnit boardUnit = result.First();
                BoardInfo boardInfo = boardUnit.GetBoardInfo();
                IBoard controller = boardUnit.GetController();
                if (controller != null)
                {
                    controller.AddSeat(seat, sn);
                    UnitInfo unitInfo = CreateUnitInfo(boardName, seat, sn, boardInfo.Plan);
                    boardInfo.units.Add(unitInfo);
                    DumpBoardInfos();
                    SaveUnitInfoToDb(unitInfo, boardInfo.temperature);
                    BoardUnit.ProductsUpdate?.Invoke(this, null);
                }
                else
                {
                    this._Logger.Error("Board " + boardName + "'s Controller is null,add seat to board fail.");
                }
                BoardStateChanged?.Invoke(this, null);
            }
        }
        public void RemoveSeatFromBoard(string boardName, int seat)
        {
            var result = (from x in this._boardSet where x.GetBoardInfo().BoardName == boardName select x);
            if (result.Any())
            {
                BoardUnit boardUnit = result.First();
                BoardInfo boardInfo = boardUnit.GetBoardInfo();
                var unit = (from y in boardInfo.units where y.Seat == seat select y);
                if (!unit.Any())
                {
                    this._Logger.Error("seat = " + seat + "is not exist in unit set,unsubscrible unit fail");
                    return;
                }
                boardInfo.units.RemoveAll(x => x.Seat == seat);
                IBoard controller = result.First().GetController();
                if (controller != null)
                {
                    controller.RemoveSeat(seat);
                }
                DumpBoardInfos();
                BoardStateChanged?.Invoke(this, null);
                BoardUnit.ProductsUpdate?.Invoke(this, null);
            }
        }
        public List<string> GetAllSnSet()
        {
            List<string> snSet = new List<string>();
            foreach (var unit in this._boardSet)
            {
                BoardInfo board = unit.GetBoardInfo();
                List<string> sns= (from x in board.units select x.Sn).ToList();
                snSet = snSet.Concat(sns).ToList<string>();
            }
            return snSet;  
        }
    }

}
