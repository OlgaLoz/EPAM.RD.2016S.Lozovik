using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Storage.Interfaces.Entities.UserInfo
{
    [DataContract]
    public struct Visa 
    {
        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public DateTime Start { get; set; }

        [DataMember]
        public DateTime End { get; set; }
    }
}