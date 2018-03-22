using System.Linq;
using BIModel.Data;
using BILib;
using BIModel.Interface;
namespace BIModel.Manager
{
    public class PosMapScheme:IPosMapScheme
    {
        private readonly IDatabaseService _dbService;
        private PosMapBlock _posMapBlock;

        private static PosMapScheme _inst;
        public static PosMapScheme Inst(IDatabaseService service)
        {
            return _inst ?? (_inst = new PosMapScheme(service));
        }
        
        protected PosMapScheme(IDatabaseService service)
        {
            _dbService = service;
            _posMapBlock = new PosMapBlock();
        }
        public string[] GetMapSchemesName()
        {
            return _dbService.GetMapSchemsName();
        }
        public PosMapBlock GetDefaultPosMapBlock()
        {
            var ret = _dbService.GetMapScheme(true);
            if (ret.Rows.Count  > 0)
            {
                _posMapBlock =  PosMapBlockSerialize.Deserialize(ret.Rows[0]["MapContent"].ToString());
            }
            return _posMapBlock;   
        }
        public PosMapBlock GetPosMapBlock(string schemeName)
        {
            var posMap = new PosMapBlock();
            var ret = _dbService.GetMapScheme(schemeName);
            if (ret.Rows.Count > 0)
            {
                posMap = PosMapBlockSerialize.Deserialize(ret.Rows[0]["MapContent"].ToString());
            }
            return posMap;
        }

        public void InsertScheme(string schemeName, int boardRows, int boardCols, int seatRows, int seatCols,
            bool validation = false)
        {
            var posMapBlock = new PosMapBlock
            {
                SchemeName = schemeName,
                BoardRows = boardRows,
                BoardCols = boardCols,
                SeatRows =  seatRows,
                SeatCols = seatCols,
                Validation = validation
            };
            var content = PosMapBlockSerialize.Serialize(posMapBlock);
         
            _dbService.InsertMapScheme(schemeName, content,boardRows,boardCols,seatRows,seatCols);
        }
        public void SubmitMap(string schemeName,string boardName, int floor, int board)
        {
            var posMapBlock = GetPosMapBlock(schemeName);
            var entry = new PosEntry()
            {
                BoardName = boardName,
                FloorNum = floor,
                BoardNum = board,
            };
            var id = posMapBlock.PosList.FindIndex(x => x.BoardName == boardName);
            if (id < 0)
            {
                posMapBlock.PosList.Add(entry);
            }
            else
            {
                posMapBlock.PosList[id] = entry;
            }
            var content = PosMapBlockSerialize.Serialize(posMapBlock);
            _dbService.UpdateMapScheme(schemeName, content);
        }

        public void SubmitMap(string schemeName, int boardRow, int boardCol, int seatRow, int seatCol)
        {
            var posMapBlock = GetPosMapBlock(schemeName);
            posMapBlock.BoardRows = boardRow;
            posMapBlock.BoardCols = boardCol;
            posMapBlock.SeatRows = seatRow;
            posMapBlock.SeatCols = seatCol;
            var content = PosMapBlockSerialize.Serialize(posMapBlock);
            _dbService.UpdateMapScheme(schemeName,content, boardRow, boardCol, seatRow, seatCol);
        }

        public void SubmitMap(string schemeName, bool validation)
        {
            var  names = GetMapSchemesName();
            if (validation)
            {
                foreach (var name in names)
                {
                    var posMapBlock = GetPosMapBlock(name);
                    posMapBlock.Validation = false;
                    var content = PosMapBlockSerialize.Serialize(posMapBlock);
                    _dbService.UpdateMapScheme(name, content, false);
                }
            }
            var block = GetPosMapBlock(schemeName);
            block.Validation = true;
            var content1 = PosMapBlockSerialize.Serialize(block);
            _dbService.UpdateMapScheme(schemeName, content1, validation);
        }

        public void RemoveMap(string schemeName,string boardName)
        {
            var posMapBlock = GetPosMapBlock(schemeName);
            posMapBlock.PosList.RemoveAll(x => x.BoardName == boardName);
            var content = PosMapBlockSerialize.Serialize(posMapBlock);
            _dbService.UpdateMapScheme(schemeName, content);
        }
        public void GetPosition(string boardName, out int floor, out int number)
        {
            var entry = (from x in _posMapBlock.PosList where x.BoardName == boardName select x).First();
            floor = entry.FloorNum;
            number = entry.BoardNum;
        }
        public string GetBoardName(int floor, int number)
        {
            var  entry = from x in _posMapBlock.PosList where x.FloorNum == floor && x.BoardNum == number select x;
            var posEntries = entry as PosEntry[] ?? entry.ToArray();
            return posEntries.Any() ? posEntries.First().BoardName : "";
        }

        public string ExportMapScheme(string schemeName)
        {
            var posMapBlock = GetPosMapBlock(schemeName);
            var content = PosMapBlockSerialize.Serialize(posMapBlock);
            return content;
        }

        public void LoadMapScheme(string content)
        {
            var posMap = PosMapBlockSerialize.Deserialize(content);
            InsertScheme(posMap.SchemeName,posMap.BoardRows,posMap.BoardCols,posMap.SeatRows,posMap.SeatCols);
            _dbService.UpdateMapScheme(posMap.SchemeName, content);
            var ret = _dbService.GetMapScheme(true);
            if (ret.Rows.Count  <= 0)
            {
                SubmitMap(posMap.SchemeName, true);
            }
            
        }
    }
}
