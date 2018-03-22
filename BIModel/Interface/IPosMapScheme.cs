using BIModel.Data;
namespace BIModel.Interface
{
    public interface IPosMapScheme
    {
        string[] GetMapSchemesName();
        PosMapBlock GetDefaultPosMapBlock();
        PosMapBlock GetPosMapBlock(string schemeName);
        void InsertScheme(string schemeName, int boardRows, int boardCols, int seatRows, int seatCols,bool validation = false);
        void SubmitMap(string schemeName, string boardName, int floor, int board);
        void SubmitMap(string schemeName, int boardRow, int boardCol, int seatRow,int seatCol);
        void SubmitMap(string schemeName, bool validation);
        void RemoveMap(string schemeName, string boardName);
        
        void GetPosition(string boardName, out int floor, out int number);
        string GetBoardName(int floor, int number);
        string ExportMapScheme(string schemeName);
        void LoadMapScheme(string content);

    }
}