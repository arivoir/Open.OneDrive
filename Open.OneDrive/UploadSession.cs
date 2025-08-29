using System;
using System.Runtime.Serialization;

namespace Open.OneDrive
{
    [DataContract]
    public class UploadSession
    {
        [DataMember(Name = "uploadUrl", EmitDefaultValue = false)]
        public string UploadUrl { get; set; }

        [DataMember(Name = "expirationDateTime", EmitDefaultValue = false)]
        public DateTime ExpirationDateTime { get; set; }

        [DataMember(Name = "nextExpectedRanges", EmitDefaultValue = false)]
        public string[] NextExpectedRanges { get; set; }
    }
}