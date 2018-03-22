using System;
using System.Xml;

namespace BurnInUI.ConfigReader
{
    public class ConfigReader
    {
        private XmlDocument xml = null;
        private static ConfigReader instance = null;
        private string _filePath;
        private ConfigReader(string fileName)
        {
            xml = new XmlDocument();
            this._filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            xml.Load(this._filePath);
        }
        public static ConfigReader GetInstance(string fileName)
        {
            if (instance == null)
            {
                instance = new ConfigReader(fileName);
            }
            return instance;
        }

        public string GetItem(string item, string subItem)
        {

            foreach (XmlElement element in xml.DocumentElement.ChildNodes)
            {
                if (element.Name == item)
                {
                    try
                    {
                        return element.SelectSingleNode(subItem).InnerText;
                    }
                    catch
                    {
                        return "";
                    }
                }
            }     
            return "";
        }
        public string GetItem(string item)
        {
            foreach (XmlElement element in xml.DocumentElement.ChildNodes)
                if (element.Name == item)
                {
                    return element.InnerText;
                    
                }

            return "";
        }
        public void WriteItem(string item, string subItem)
        {
            foreach (XmlElement element in xml.DocumentElement.ChildNodes)
                foreach (XmlElement ele in element.ChildNodes)
                {
                    if (ele.Name == item)
                    {
                        ele.InnerText = subItem;
                    }
                }
            xml.Save(this._filePath);    
        }


    }
}
