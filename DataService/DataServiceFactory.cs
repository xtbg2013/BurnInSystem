using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILib
{
    public class DataServiceFactory
    {
        public static IDataService CreateDataService(string constr)
        {
            return new DtService36(constr);
        }


        public static IDataService CreateDataService(string dataSource, string dataBaseName, string userId, string pwd)
        {
            return new DtService36(dataSource, dataBaseName, userId, pwd); 
        }

        public static IDatabaseService CreateDatabaseService(string constr)
        {
            return new DataService37(constr);
        }

    }
}
