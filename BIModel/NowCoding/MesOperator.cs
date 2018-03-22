using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIModel.MesService;
using BIModel.Interface;
using BiInterface;

namespace BIModel.NowCoding
{
    public class MesOperator:IMesOperator
    {
        private static MesOperator _MesOperator = null;
        private  MesServiceClient _mesClient = null;
        public static MesOperator GetInstance()
        {
            if (_MesOperator == null)
            {
                _MesOperator = new MesOperator();
            }
            return _MesOperator;
        }

        private MesOperator()
        {
            if (_mesClient == null)
                _mesClient = new MesServiceClient();
        }
        
        public string GetMesStepBySn(string sn,out string message)
        {
            try
            {
                string workStep = "";
                _mesClient.GetWorkStep(sn, out workStep, out message);
                return workStep;
            }
            catch
            {
                throw new Exception("Get " + sn + " step in mes exception");

            }
        }

        public  bool Hold(string sn, string reason,out string message)
        {
            try
            {
                bool ret = _mesClient.Hold(sn, reason, out message);
                return ret;
            }
            catch
            {
                throw new Exception("Hold " +sn +" exception");
            }  
        }

        public  bool AutoMoveOut(string sn, string result,bool isHold, out int hdormv)
        {
            hdormv = 0;
            string message;
            bool retResult = true;
            try
            {
                if (result.ToUpper() == "PASS")
                {
                    if (_mesClient.MoveStandard(sn, out message))
                    {
                        hdormv = 1;
                        retResult = true;
                    }
                    else
                        retResult = false;
                   
                }
                else
                {
                    if (isHold)
                    {
                        if (_mesClient.Hold(sn, "NG Auto Hold", out message))
                        {
                            hdormv = 2;
                            retResult = true;
                        }
                        else
                            retResult = false;      
                    }
                    else
                    {
                        message = sn + " need not hold";
                        hdormv = 3;
                        retResult = false;
                    }        
                }
                return retResult;
            }
            catch
            {
                return false;
                //throw new Exception("auto move " + sn +" out exception");
            }
            
        }
        public  string CheckMesStateBySn(string sn,out string message)
        {
            try
            {
                string workStepstate = "";
                _mesClient.GetStepState(sn, out workStepstate, out message);
                return workStepstate;
            }
            catch
            {
                throw new Exception("Get " + sn + " state in mes exception");  
            }
            
        }
        public  string[] GetCocInfoBySn(string sn, out string message)
        {
            try
            {
                string[] info = new string[] { };
                _mesClient.GetCocInfoBySn(sn, out info, out message);
                return info;
            }
            catch
            {
                throw new Exception("Get " + sn + " coc info exception,please check mes system");
            } 
        }
    }
}
