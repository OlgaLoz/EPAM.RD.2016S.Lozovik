using System.ComponentModel;

namespace Attributes
{
    public class AdvancedUser : User
    {
        private int _externalId;

        [DefaultValue(3443454)]
        public int ExternalId
        {
            get
            {
                return _externalId;
            }
        }

        [MatchParameterWithProperty("id", "Id")]
        [MatchParameterWithProperty("externalId", "ExternalId")]
        public AdvancedUser(int id, int externalId) : base(id)
        {
            _externalId = externalId;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ ExternalId;
        }

        public override bool Equals(object obj)
        {
            var advancedUser = obj as AdvancedUser;
            return base.Equals(obj) && ExternalId == advancedUser?.ExternalId;
        }
    }
}
