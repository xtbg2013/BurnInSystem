using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TbiesIntfDll
{
    public class TbiesIntf
    {
        #region configurations
        private string mServerIp = null;
        readonly Dictionary<string, string> APP_PORT = new Dictionary<string, string> {
            { "whoami",       "3000" },
            { "conframe",     "3001" },
        };
        private string serverPort(string appName)
        {
            return "http://" + mServerIp + ":" + APP_PORT[appName] + "/publications/";
        }
        #endregion
        public TbiesIntf(string serverIp)
        {
            mServerIp = serverIp;
        }
        public string getUsers()
        {
            return Utils.Http.Get(serverPort("whoami") + "users.list");
        }
        public List<Specification> getSpecsBrief()
        {
            string result = Utils.Http.Get(serverPort("conframe") + "specifications.brief");
            result = Utils.Json.ExtractList(result);
            return Utils.Json.ToObject<List<Specification>>(result);
        }
        public Specification getSpec(string name)
        {
            List<Specification> specs = getSpecsBrief();

            foreach (var spec in specs)
            {
                if (spec.name == name)
                {
                    string result = Utils.Http.Get(serverPort("conframe") + "specification-with-id/" + spec._id);
                    result = Utils.Json.ExtractList(result);
                    return Utils.Json.ToObject<List<Specification>>(result)[0];
                }
            }
            return null;
        }
    }
}
