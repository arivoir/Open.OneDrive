using System.Runtime.Serialization;

namespace Open.OneDrive
{
    [DataContract]
    public class IdentitySet
    {
        [DataMember(Name = "user")]
        public Identity User { get; set; }
        [DataMember(Name = "application")]
        public Identity Application { get; set; }
        [DataMember(Name = "device")]
        public Identity Device { get; set; }
    }

    public class Identity
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "displayName")]
        public string DisplayName { get; set; }

    }
}
