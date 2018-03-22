using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TbiesIntfDll;
using BIModel.runtime;

namespace TbiesIntfTest
{
    class Program
    {

        static void Main(string[] args)
        {
            //test_getUsers();
            test_getSpec();
            test_transformSpec();
        }

        static void test_getUsers()
        {
            TbiesIntf intf = new TbiesIntf("192.168.56.2");

            string result = intf.getUsers();
            Debug.Assert(result.Length!=0);
        }
        static void test_getSpec()
        {
            TbiesIntf intf = new TbiesIntf("192.168.56.2");

            List<Specification> specs = intf.getSpecsBrief();
            Debug.Assert(specs.Count > 0);

            Specification spec = intf.getSpec("100C_Template");
            Debug.Assert(spec!=null);
        }
        static void test_transformSpec()
        {
            TbiesIntf intf = new TbiesIntf("192.168.56.2");

            List<Specification> specs = intf.getSpecsBrief();
            List<BISpecification> biSpecs = BISpecification.transformFromApp(specs);
            Debug.Assert(biSpecs.Count > 0);

            foreach (var spec in specs)
            {
                Specification wholeSpec = intf.getSpec(spec.name);
                BISpecification biSpec = BISpecification.transformFromApp(wholeSpec);
                Debug.Assert(biSpec.Specification != null);
                Debug.Assert(biSpec.Configuration != null);
            }
        }
    }
}
