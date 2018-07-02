using System.Runtime.Serialization;

namespace Open.OneDrive
{
    [DataContract]
    public class Location
    {
        [DataMember(Name = "latitude", EmitDefaultValue = false)]
        public double Latitude { get; set; }
        [DataMember(Name = "longitude", EmitDefaultValue = false)]
        public double Longitude { get; set; }
        [DataMember(Name = "altitude", EmitDefaultValue = false)]
        public double Altitude { get; set; }
    }
}
