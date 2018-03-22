using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIModel.Interface
{
    public interface IMesOperator
    {
        string GetMesStepBySn(string sn, out string message);
        bool Hold(string sn, string reason, out string message);
        bool AutoMoveOut(string sn, string result, bool isHold, out int hdormv);
        string CheckMesStateBySn(string sn, out string message);
        string[] GetCocInfoBySn(string sn, out string message);
       
    }
}
