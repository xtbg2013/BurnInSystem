using ProgressODoom;

namespace BurnInUI.Forms
{
    public class BoardCell:ProgressBarEx
    {
        private int rowId;
        private int colId;
        public BoardCell(int rowId, int colId)
        {
            this.rowId = rowId;
            this.colId = colId;
        }
        public int RowId { get { return rowId; } }
        public int ColId { get { return colId; } }
    }
}
