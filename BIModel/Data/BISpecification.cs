using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

using TbiesIntfDll;

namespace BIModel.Data
{
    public class BISpecification
    {
        public string Plan { get; set; }
        public string Version { get; set; }
        public string Driver { get; set; }
        public string Span { get; set; }
        public string Interval { get; set; }
        public List<ConditionItem> Configuration { get; set; }
        public List<SpecItem> Specification { get; set; }
    }
}
