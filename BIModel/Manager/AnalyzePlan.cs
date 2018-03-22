using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using BIModel.Data;
using TbiesIntfDll;
namespace BIModel.Manager
{
    class AnalyzePlan
    {
        public static BISpecification Deserialize(string content)
        {
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(BISpecification));
                return (BISpecification)json.ReadObject(stream);
            }
        }
        public static string Serialize(BISpecification obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(BISpecification));
                json.WriteObject(stream, obj);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
        private static List<ConditionItem> ExtractSpanInterval(List<ConditionItem> configs, out string span, out string interval)
        {
            List<ConditionItem> biConfigs = new List<ConditionItem>(configs.ToArray());
            span = "";
            interval = "";

            for (int i = biConfigs.Count - 1; i >= 0; i--)
            {
                if (biConfigs[i].Item == "Span")
                {
                    span = biConfigs[i].Value;
                    biConfigs.RemoveAt(i);
                }

                if (biConfigs[i].Item == "Interval")
                {
                    interval = biConfigs[i].Value;
                    biConfigs.RemoveAt(i);
                }
            }

            return biConfigs;
        }
        public static BISpecification TransformFromApp(Specification spec)
        {
            BISpecification biSpec = new BISpecification();

            biSpec.Plan = spec.name;
            biSpec.Version = spec.version;
            biSpec.Driver = spec.product;

            if (spec.content != null)
            {
                string span = "";
                string interval = "";
                var biConfigs = ExtractSpanInterval(spec.content.Configuration, out span, out interval);

                biSpec.Span = span;
                biSpec.Interval = interval;

                biSpec.Configuration = biConfigs as List<ConditionItem>;
                biSpec.Specification = spec.content.Specification as List<SpecItem>;
            }

            return biSpec;
        }
        public static List<BISpecification> TransformFromApp(List<Specification> specs)
        {
            List<BISpecification> biSpecs = new List<BISpecification>();

            foreach (var spec in specs)
            {
                BISpecification biSpec = TransformFromApp(spec);
                biSpecs.Add(biSpec);
            }

            return biSpecs;
        }
    }
}
