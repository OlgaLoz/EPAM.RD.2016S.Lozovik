using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Storage.Interfaces.Entities.CustomSerialization;
using Storage.Interfaces.Entities.CustomSerialization.Mappers;
using Storage.Interfaces.Entities.UserInfo;

namespace Storage.Interfaces.Entities.ServiceState
{
    [Serializable]
    public class ServiceState : IXmlSerializable
    {
        public ServiceState()
        {
            Users = new List<User>();
        }

        public List<User> Users { get; set; }

        public int CurrentId { get; set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement(nameof(ServiceState));

            CurrentId = reader.ReadElementContentAsInt();

            reader.MoveToAttribute("count");
            int count = int.Parse(reader.Value);
            reader.ReadStartElement(nameof(Users));
            var userSer = new XmlSerializer(typeof(SerializableUser));
            for (int i = 0; i < count; i++)
            {
                var user = (SerializableUser)userSer.Deserialize(reader);
                Users.Add(user.ToUser());
            }

            reader.ReadEndElement();

            reader.ReadEndElement();
            }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(nameof(CurrentId), CurrentId.ToString());

            writer.WriteStartElement(nameof(Users));
            writer.WriteAttributeString("count", Users.Count.ToString());
            foreach (var user in Users)
            {
                user.ToSerializableUser().WriteXml(writer);
            }

            writer.WriteEndElement();
        }
    }
}