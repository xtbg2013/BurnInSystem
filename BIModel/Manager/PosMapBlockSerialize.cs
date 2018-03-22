using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using BIModel.Data;
namespace BIModel.Manager
{

    public class PosMapBlockSerialize
    {
        public static PosMapBlock Deserialize(string content)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                var json = new DataContractJsonSerializer(typeof(PosMapBlock));
                return (PosMapBlock)json.ReadObject(stream);
            }
        }
        public static string Serialize(PosMapBlock obj)
        {
            using (var stream = new MemoryStream())
            {
                var json = new DataContractJsonSerializer(typeof(PosMapBlock));
                json.WriteObject(stream, obj);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
    }


   
}
