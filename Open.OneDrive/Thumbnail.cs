using System.Runtime.Serialization;

namespace Open.OneDrive
{
    [DataContract]
    public class ThumbnailsSet
    {
        [DataMember(Name = "id")]
        public int id { get; set; }
        [DataMember(Name = "small")]
        public Thumbnail small { get; set; }
        [DataMember(Name = "medium")]
        public Thumbnail Medium { get; set; }
        [DataMember(Name = "large")]
        public Thumbnail Large { get; set; }
        [DataMember(Name = "source")]
        public Thumbnail source { get; set; }
    }

    [DataContract]
    public class Thumbnail
    {
        [DataMember(Name = "width")]
        public int Width { get; set; }
        [DataMember(Name = "height")]
        public int Height { get; set; }
        [DataMember(Name = "url")]
        public string Url { get; set; }
    }
}