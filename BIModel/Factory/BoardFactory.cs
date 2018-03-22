
using System;
using System.Collections.Generic;
using System.Reflection;
using BiInterface;
using TbiesIntfDll;
using BIModel.Manager;
using log4net;

namespace BIModel.Factory
{
    public class BoardFactory
    {
        private static BoardFactory factory = null;
        private static IDriverFactory driverFactory = null;
        private ILog _logger;
       
        private FetchPlans _fetchPlans;
        private Dictionary<string, string> _ports;
        public static BoardFactory Instance(ILog log,FetchPlans fetchObj, Dictionary<string, string> ports)
        {
            if (factory == null)
                factory = new BoardFactory(log, fetchObj, ports);
            return factory;
        }
        public BoardFactory(ILog log, FetchPlans fetchObj, Dictionary<string, string> ports)
        {
            this._logger = log;
            this._ports = ports;
            this._fetchPlans = fetchObj;
        }
        public IDriverFactory CreateDriverFactory()
        {
            if (driverFactory == null)
            {
                string asmPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"lib\BiBsps.dll");
                if (!System.IO.File.Exists(asmPath))
                {
                    this._logger.Error("BisBsps.dll is not exist");
                    return null;
                }
                Assembly ass = System.Reflection.Assembly.LoadFile(asmPath);
                Type type = ass.GetType("BiBsps.DriverFactory");
                driverFactory = (IDriverFactory)Activator.CreateInstance(type);   
            }
            return driverFactory;
        }
        public IBoard GenerateBoard(string plan, string boardName,int floor,int number)
        {
            try
            {
                string product = this._fetchPlans.GetDriverType(plan);
                var config = this._fetchPlans.GetConfigItems(plan);
                Dictionary<string,string> para = new Dictionary<string, string>
                {
                    { "Floor", floor.ToString()},
                    { "Locate", number.ToString() },
                    { "Name", boardName },
                };
                
                string key = "Floor" + floor.ToString();
                if (this._ports.ContainsKey(key))
                {
                    para["ControlPort"] = this._ports[key];
                }
                key = "OvenPort";
                if (this._ports.ContainsKey(key))
                {
                    para["OvenPort"] = this._ports[key];
                }
            
                foreach (ConditionItem entry in config)
                    para[entry.Item] = entry.Value;

                IDriverFactory factory = CreateDriverFactory();
                if (factory != null)
                    return factory.CreateDriver(product, this._logger, para);
                else
                    return null;
            }
            catch (Exception ex)
            {
                this._logger.Error("Fail to generate board "+boardName+":"+ex.Message);
                return null;
            }
        }
       
    }
}
