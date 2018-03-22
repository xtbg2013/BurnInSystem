using System;

namespace BurnInUI
{
    public class BoardLocateEventArgs:EventArgs
    {
        public BoardLocateEventArgs(int row, int col, string boardName)
        {
            Row = row;
            Column = col;
            BoardName = boardName;
        }

        public BoardLocateEventArgs()
        {
        }

        public int Row { set; get; }
        public int Column { set; get; }
        public string BoardName { set; get; }
    }
}
