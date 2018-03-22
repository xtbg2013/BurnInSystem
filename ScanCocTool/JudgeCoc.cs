using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalFiles;
using ScanCocTool.MesService;
using log4net;

namespace ScanCocTool
{
    class JudgeCoc
    {
        private MesServiceClient _mesClient = null;
        private CocPnTableReader _cocPnTableReader = null;
        public JudgeCoc(MesServiceClient client, CocPnTableReader cocPnTableReader)
        {
            _mesClient = client;
            _cocPnTableReader = cocPnTableReader;
            _cocPnTableReader.ReadPnTable();
        }
       
        private bool GetCocInfo(string sn, out Dictionary<string, Dictionary<string, string>> cocInfo)
        {
            bool state = false;
            Dictionary<string, List<string>> bh1PnTable = _cocPnTableReader.GetBh1Table();
            Dictionary<string, List<string>> bh2PnTable = _cocPnTableReader.GetBh2Table();
            cocInfo = new Dictionary<string, Dictionary<string, string>>();

            string msg;
            string[] retInfo = new string[] { };
            try
            {
                _mesClient.GetCocInfoBySn(sn, out retInfo, out msg);
            }
            catch
            {
                LogHelper.Error(sn + " get coc info from mes  error");
                return false;
            }
            

            int count = retInfo.Length / 4;

            LogHelper.Info("Get component info: sn = " + sn);
            for (int i = 0; i < count; i++)
            {
                string cocSN = retInfo[i * 4];
                string cocPN = retInfo[i * 4 + 1];

                Dictionary<string, string> info = new Dictionary<string, string>();
                //4 channels
                //ch1
                if (bh1PnTable.ContainsKey("ch1") && bh1PnTable["ch1"].Contains(cocPN))
                {
                    info["type"] = "BH1";
                    info["cocSn"] = cocSN;
                    info["cocPN"] = cocPN;
                    cocInfo["ch1"] = info;
                    LogHelper.Info("Channel1:sn = " + cocInfo["ch1"]["cocSn"] + " pn = " + cocInfo["ch1"]["cocPN"]);

                }
                else if (bh2PnTable.ContainsKey("ch1") && bh2PnTable["ch1"].Contains(cocPN))
                {
                    info["type"] = "BH2";
                    info["cocSn"] = cocSN;
                    info["cocPN"] = cocPN;
                    cocInfo["ch1"] = info;
                    LogHelper.Info("Channel1:sn = " + cocInfo["ch1"]["cocSn"] + " pn = " + cocInfo["ch1"]["cocPN"]);
                }
                //ch2
                if (bh1PnTable.ContainsKey("ch2") && bh1PnTable["ch2"].Contains(cocPN))
                {
                    info["type"] = "BH1";
                    info["cocSn"] = cocSN;
                    info["cocPN"] = cocPN;
                    cocInfo["ch2"] = info;
                    LogHelper.Info("Channel2:sn = " + cocInfo["ch2"]["cocSn"] + " pn = " + cocInfo["ch2"]["cocPN"]);

                }
                else if (bh2PnTable.ContainsKey("ch2") && bh2PnTable["ch2"].Contains(cocPN))
                {
                    info["type"] = "BH2";
                    info["cocSn"] = cocSN;
                    info["cocPN"] = cocPN;
                    cocInfo["ch2"] = info;
                    LogHelper.Info("Channel2:sn = " + cocInfo["ch2"]["cocSn"] + " pn = " + cocInfo["ch2"]["cocPN"]);
                }
                //ch3
                if (bh1PnTable.ContainsKey("ch3") && bh1PnTable["ch3"].Contains(cocPN))
                {
                    info["type"] = "BH1";
                    info["cocSn"] = cocSN;
                    info["cocPN"] = cocPN;
                    cocInfo["ch3"] = info;
                    LogHelper.Info("Channel3:sn = " + cocInfo["ch3"]["cocSn"] + " pn = " + cocInfo["ch3"]["cocPN"]);

                }
                else if (bh2PnTable.ContainsKey("ch3") && bh2PnTable["ch3"].Contains(cocPN))
                {
                    info["type"] = "BH2";
                    info["cocSn"] = cocSN;
                    info["cocPN"] = cocPN;
                    cocInfo["ch3"] = info;
                    LogHelper.Info("Channel3:sn = " + cocInfo["ch3"]["cocSn"] + " pn = " + cocInfo["ch3"]["cocPN"]);
                }
                if (bh1PnTable.ContainsKey("ch4") && bh1PnTable["ch4"].Contains(cocPN))
                {
                    info["type"] = "BH1";
                    info["cocSn"] = cocSN;
                    info["cocPN"] = cocPN;
                    cocInfo["ch4"] = info;
                    LogHelper.Info("Channel4:sn = " + cocInfo["ch4"]["cocSn"] + " pn = " + cocInfo["ch4"]["cocPN"]);

                }
                else if (bh2PnTable.ContainsKey("ch4") && bh2PnTable["ch4"].Contains(cocPN))
                {
                    info["type"] = "BH2";
                    info["cocSn"] = cocSN;
                    info["cocPN"] = cocPN;
                    cocInfo["ch4"] = info;
                    LogHelper.Info("Channel4:sn = " + cocInfo["ch4"]["cocSn"] + " pn = " + cocInfo["ch4"]["cocPN"]);
                }
            }


            if (!cocInfo.ContainsKey("ch1") || !cocInfo.ContainsKey("ch2") || !cocInfo.ContainsKey("ch3") || !cocInfo.ContainsKey("ch4"))
            {

                LogHelper.Info("Sn = " + sn + "  get coc info from camstar :empty");
                state = false;
            }
            else
                state = true;
            return state;
        }

        public void JudgeCocTypeInfo(string sn,out Dictionary<string,string> info)
        {
            Dictionary<string, Dictionary<string, string>> cocInfo;
            info = new Dictionary<string, string>();
            bool state = GetCocInfo(sn, out cocInfo);
            if (state)
            {
                if (cocInfo["ch1"].ContainsValue("BH2") && cocInfo["ch2"].ContainsValue("BH2") &&
                    cocInfo["ch3"].ContainsValue("BH2") && cocInfo["ch3"].ContainsValue("BH2"))
                {
                    info["COC"] = "BH2";
                    info["HOURS"] = "92";
                    info["CURRENT"] = "90";
                }
                else if (cocInfo["ch1"].ContainsValue("BH1") && cocInfo["ch2"].ContainsValue("BH1") &&
                    cocInfo["ch3"].ContainsValue("BH1") && cocInfo["ch3"].ContainsValue("BH1"))
                {
                    info["COC"] = "BH1";
                    info["HOURS"] = "20";
                    info["CURRENT"] = "80";
                }

                else
                {
                    info["COC"] = "BH1&BH2";
                    info["HOURS"] = "92";
                    info["CURRENT"] = "90";
                }
            }
            else
            {
                info["COC"] = "empty";
                info["HOURS"] = "0";
                info["CURRENT"] = "0";
            }
             

        }
    }
}
