using System.Runtime.Serialization;
using System.ServiceModel;

namespace Storage.Interfaces.Entities.UserInfo
{
    [DataContract]
    public enum Gender
    {
        [EnumMember]
        Male,
        [EnumMember]
        Female
    }
}