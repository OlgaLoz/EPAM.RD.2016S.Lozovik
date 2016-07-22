using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlTypes;
using System.IO;
using System.Xml.Serialization;
using Storage.Entities.ServiceState;
using Storage.Entities.UserInfo;
using Storage.Interfaces;

namespace Storage.Repository
{
    [Serializable]
    public class Repository : IRepository
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
                if (stream.Length == 0)
                {
                    return new ServiceState
                    {
                        Users = new List<User>()
                    };
                }
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