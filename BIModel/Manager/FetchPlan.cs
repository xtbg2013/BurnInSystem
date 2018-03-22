using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using TbiesIntfDll;
using BILib;
using BIModel.Data;
using BIModel.runtime;
namespace BIModel.Manager
{
    public class FetchPlans
    {
        private Dictionary<string, BISpecification> _specSet = new Dictionary<string, BISpecification>();
        private static FetchPlans _inst = null;
        private string _serverIp;
      
        public static FetchPlans Inst(string remoteServerIp)
        {
            return _inst ?? (_inst = new FetchPlans(remoteServerIp));
        }

        private FetchPlans(string remoteServerIp)
        {
            this._serverIp = remoteServerIp;
           
        }
        public void FetchPlansList(IDatabaseService service)
        {
            if (this._serverIp.ToUpper() == "")
            {
                var specTable = service.GetValidSpecificationTable();
                foreach (DataRow row in specTable.Rows)
                    this._specSet[row["Plan"].ToString()] = AnalyzePlan.Deserialize(row["Content"].ToString());
            }
            else
            {
                TbiesIntf intf = new TbiesIntf(this._serverIp);
                List<Specification> specs = intf.getSpecsBrief();
                List<BISpecification> biSpecs = AnalyzePlan.TransformFromApp(specs);
                foreach (BISpecification spec in biSpecs)
                {
                    this._specSet[spec.Plan] = spec;
                }
            }
            
        }
        private BISpecification FetchSpecificationSet(string plan)
        {
            if (this._serverIp.ToUpper() == "")
            {
                return this._specSet[plan];
            }
            else
            {
                TbiesIntf intf = new TbiesIntf(_serverIp);
                return AnalyzePlan.TransformFromApp(intf.getSpec(plan));
            }
        }
        
        public string GetDriverType(string plan)
        {
            BISpecification spec = FetchSpecificationSet(plan);
            return spec.Driver;
        }
        public double GetSpan(string plan)
        {
            return double.Parse(this._specSet[plan].Span);
        }
        public double GetInterval(string plan)
        {
            return double.Parse(this._specSet[plan].Interval);
        }
        public string[] GetSupportPlanTable(string[] driverType)
        {
            return (from x in this._specSet where driverType.Contains(x.Value.Driver) select x.Key).ToArray();
        }
        public SpecItem[] GetSpecification(string plan)
        {
            return this._specSet[plan].Specification.ToArray();
        }
        public ConditionItem[] GetConfigItems(string plan)
        {
            return this._specSet[plan].Configuration.ToArray();
        }
    }
}
