using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIModel
{
    [Serializable]
    public enum BoardState
    {
        UNSELECTED = 0,
        SELECTED = 1,
        LOADED = 2,
        READY = 3,
        RUNNING = 4,
        DONE=5,
        CONFLICT
    };
}
