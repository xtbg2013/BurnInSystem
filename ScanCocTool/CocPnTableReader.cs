using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GlobalFiles
{
    class CocPnTableReader
    {
        private static CocPnTableReader inst = null;
        private XmlDocument xml = null;

        Dictionary<string, List<string>> bh1Table;
        Dictionary<string, List<string>> bh2Table;

        public static CocPnTableReader GetInstance(string fileName)
        {
            if (inst == null)
                inst = new CocPnTableReader(fileName);
            return inst;
        }

        public Dictionary<string, List<string>> GetBh1Table()
        {
            return bh1Table;
        }
        public Dictionary<string, List<string>> GetBh2Table()
        {
            return bh2Table;
        }

        public void ReadPnTable()
        {
            GetItem("BH1", out bh1Table);
            GetItem("BH2", out bh2Table);
        }

        private CocPnTableReader(string fileName)
        {
            this.xml = new XmlDocument();
            xml.Load(System.IO.Path.Combine(Environment.CurrentDirectory, fileName));
        }

        private void GetItem(string item, out Dictionary<string, List<string>> items)
        {
            items = new Dictionary<string, List<string>>();
            XmlNodeList nodes = xml.SelectSingleNode("Settings").SelectSingleNode(item).ChildNodes;
            foreach (XmlNode node in nodes)
            {
                List<string> ls = new List<string>();
                foreach (XmlNode nd in node.ChildNodes)
                {
                    ls.Add(nd.InnerText);
                }
                items[node.Name] = ls;
            }
        }

    }
}
