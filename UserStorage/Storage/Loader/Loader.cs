using System.Configuration;
using System.IO;
using System.Xml.Serialization;
using Storage.Service;

namespace Storage.Loader
{
    public class Loader : ILoader
    {
        public void Save(ServiceState state)
        {
            var formatter = new XmlSerializer(typeof(ServiceState));

            using (var stream = new FileStream(GetFileName(), FileMode.OpenOrCreate))
            {
                formatter.Serialize(stream, state);
            }
        }

        public ServiceState Load()
        {
            var formatter = new XmlSerializer(typeof(ServiceState));

            using (var stream = new FileStream(GetFileName(), FileMode.OpenOrCreate))
            {
                return (ServiceState)formatter.Deserialize(stream);
            }
        }

        private string GetFileName()
        {
            var value = ConfigurationManager.AppSettings["fileName"];
            if (value == null)
            {
                throw new FileNotFoundException();
            }
            return value;
        }
    }
}