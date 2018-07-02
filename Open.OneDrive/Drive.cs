using System.Runtime.Serialization;

namespace Open.OneDrive
{
    [DataContract]
    public class Drive
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "driveType")]
        public DriveType DriveType { get; set; }
        [DataMember(Name = "owner")]
        public IdentitySet Owner { get; set; }
        [DataMember(Name = "quota")]
        public Quota Quota { get; set; }
    }

    [DataContract(IsReference = false)]
    public enum DriveType
    {
        [EnumMember(Value = "personal")]
        Personal,
        [EnumMember(Value = "business")]
        Business,
    }
}
