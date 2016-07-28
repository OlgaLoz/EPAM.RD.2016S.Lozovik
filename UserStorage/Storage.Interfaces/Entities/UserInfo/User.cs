using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Storage.Interfaces.Entities.UserInfo
{
    [Serializable]
    public class User : IEquatable<User>, IXmlSerializable
    {
        public int PersonalId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirdth { get; set; }

        public Gender Gender { get; set; }

        public Visa[] Visas { get; set; }

        public bool Equals(User other)
        {
            if (other == null)
            {
                return false;
            }

            return PersonalId == other.PersonalId &&
                FirstName == other.FirstName &&
                LastName == other.LastName;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return obj.GetType() == typeof(User) && Equals((User)obj);
        }

        public override int GetHashCode()
        {
            int hash = PersonalId.GetHashCode();
            hash ^= FirstName?.GetHashCode() ?? 0;
            hash ^= LastName?.GetHashCode() ?? 0;
            hash ^= DateOfBirdth.GetHashCode();
            hash ^= Gender.GetHashCode();

            return hash;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement(nameof(User));

            PersonalId = reader.ReadElementContentAsInt();
            FirstName = reader.ReadElementContentAsString();
            LastName = reader.ReadElementContentAsString();
            Gender = (Gender)reader.ReadElementContentAsInt();
            DateOfBirdth = reader.ReadElementContentAsDateTime();

            reader.MoveToAttribute("count");
            int count = int.Parse(reader.Value);
            Visas = new Visa[count];

            reader.ReadStartElement(nameof(Visas));
            var visaSer = new XmlSerializer(typeof(Visa));
            for (int i = 0; i < count; i++)
            {
                var visa = (Visa)visaSer.Deserialize(reader);
                Visas[i] = visa;
            }

            reader.ReadEndElement();

            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement(nameof(User));

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