using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIModel
{
    public enum UnitState
    {
        DONE=0,
        PAUSE=1,
        BURNIN=2,
        READY=3,
        REWORK=4
    };

    public enum UnitResult
    {
        PASS=0,
        FAIL=1
    };
}
