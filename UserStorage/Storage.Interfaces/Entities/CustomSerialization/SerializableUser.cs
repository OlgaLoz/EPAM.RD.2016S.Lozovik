using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Storage.Interfaces.Entities.UserInfo;

namespace Storage.Interfaces.Entities.CustomSerialization
{
    public class SerializableUser : IXmlSerializable
    {
        public int PersonalId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirdth { get; set; }

        public Gender Gender { get; set; }

        public SerializableVisa[] Visas { get; set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement(nameof(SerializableUser));

            PersonalId = reader.ReadElementContentAsInt();
            FirstName = reader.ReadElementContentAsString();
            LastName = reader.ReadElementContentAsString();
            Gender = (Gender)reader.ReadElementContentAsInt();
            DateOfBirdth = reader.ReadElementContentAsDateTime();

            reader.MoveToAttribute("count");
            int count = int.Parse(reader.Value);
            Visas = new SerializableVisa[count];

            reader.ReadStartElement(nameof(Visas));
            var visaSer = new XmlSerializer(typeof(SerializableVisa));
            for (int i = 0; i < count; i++)
            {
                var visa = (SerializableVisa)visaSer.Deserialize(reader);
                Visas[i] = visa;
            }

            reader.ReadEndElement();

      ////      reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(nameof(SerializableUser));

            writer.WriteElementString(nameof(PersonalId), PersonalId.ToString());
            writer.WriteElementString(nameof(FirstName), FirstName);
            writer.WriteElementString(nameof(LastName), LastName);
            writer.WriteElementString(nameof(Gender), ((int)Gender).ToString());
            writer.WriteElementString(nameof(DateOfBirdth), DateOfBirdth.ToString("yyyy-MM-dd"));

            writer.WriteStartElement(nameof(Visas));
            writer.WriteAttributeString("count", Visas.Length.ToString());
            for (int i = 0; i < Visas.Length; i++)
            {
                Visas[i].WriteXml(writer);
            }

            writer.WriteEndElement();

            writer.WriteEndElement();
        }
    }
}