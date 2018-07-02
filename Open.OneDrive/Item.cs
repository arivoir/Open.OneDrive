using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Open.OneDrive
{
    [DataContract]
    public class Items
    {
        [DataMember(Name = "value")]
        public List<Item> Value { get; set; }
        [DataMember(Name = "@odata.nextLink")]
        public string NextLink { get; set; }
    }

    [DataContract]
    public class Item
    {
        [DataMember(Name = "id", EmitDefaultValue = false, Order = 0)]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "eTag", EmitDefaultValue = false)]
        public string ETag { get; set; }
        [DataMember(Name = "cTag", EmitDefaultValue = false)]
        public string CTag { get; set; }
        [DataMember(Name = "createdBy", EmitDefaultValue = false)]
        public IdentitySet CreatedBy { get; set; }
        [DataMember(Name = "createdDateTime", EmitDefaultValue = false)]
        public string CreatedDateTime { get; set; }
        [DataMember(Name = "lastModifiedBy", EmitDefaultValue = false)]
        public IdentitySet LastModifiedBy { get; set; }
        [DataMember(Name = "lastModifiedDateTime", EmitDefaultValue = false)]
        public string LastModifiedDateTime { get; set; }
        [DataMember(Name = "size", EmitDefaultValue = false)]
        public long Size { get; set; }
        [DataMember(Name = "webUrl", EmitDefaultValue = false)]
        public string WebUrl { get; set; }
        [DataMember(Name = "description", Order = 2, EmitDefaultValue = false)]
        public string Description { get; set; }
        [DataMember(Name = "parentReference", EmitDefaultValue = false)]
        public ItemReference ParentReference { get; set; }
        [DataMember(Name = "children", EmitDefaultValue = false)]
        public Item Children { get; set; }
        [DataMember(Name = "folder", EmitDefaultValue = false)]
        public FolderFacet Folder { get; set; }
        [DataMember(Name = "file", EmitDefaultValue = false)]
        public FileFacet File { get; set; }
        [DataMember(Name = "fileSystemInfo", EmitDefaultValue = false)]
        public FileSystemInfoFacet FileSystemInfo { get; set; }
        [DataMember(Name = "image", EmitDefaultValue = false)]
        public ImageFacet Image { get; set; }
        [DataMember(Name = "photo", EmitDefaultValue = false)]
        public PhotoFacet Photo { get; set; }
        [DataMember(Name = "audio", EmitDefaultValue = false)]
        public AudioFacet Audio { get; set; }
        [DataMember(Name = "video", EmitDefaultValue = false)]
        public VideoFacet Video { get; set; }
        [DataMember(Name = "location", EmitDefaultValue = false)]
        public LocationFacet Location { get; set; }

        [DataMember(Name = "remoteItem", EmitDefaultValue = false)]
        public RemoteItemFacet RemoteItem { get; set; }
        [DataMember(Name = "searchResult", EmitDefaultValue = false)]
        public SearchResultFacet SearchResult { get; set; }
        [DataMember(Name = "deleted", EmitDefaultValue = false)]
        public Deleted Deleted { get; set; }
        [DataMember(Name = "specialFolder", EmitDefaultValue = false)]
        public SpecialFolder SpecialFolder { get; set; }
        [DataMember(Name = "thumbnails", EmitDefaultValue = false)]
        public List<ThumbnailsSet> Thumbnails { get; set; }
        [DataMember(Name = "@content.downloadUrl", EmitDefaultValue = false)]
        public string DownloadUrl { get; set; }
        [DataMember(Name = "@content.sourceUrl", EmitDefaultValue = false)]
        public string SourceUrl { get; set; }


        [DataMember(Name = "shared", EmitDefaultValue = false)]
        public Shared Shared { get; set; }

        [DataMember(Name = "@name.conflictBehavior", EmitDefaultValue = false)]
        public ConflictBehavior ConflictBehavior { get; set; }
    }

    [DataContract(IsReference = false)]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ConflictBehavior
    {
        [EnumMember(Value = "rename")]
        Rename,
        [EnumMember(Value = "replace")]
        Replace,
        [EnumMember(Value = "fail")]
        Fail,
    }

    [DataContract]
    public class Deleted
    {
    }

    [DataContract]
    public class Shared
    {
        [DataMember(Name = "owner")]
        public IdentitySet Owner { get; set; }
        [DataMember(Name = "scope")]
        public string Scope { get; set; }
    }

    [DataContract]
    public class SpecialFolder
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }

    [DataContract]
    public class VideoFacet
    {
        [DataMember(Name = "bitrate")]
        public int Bitrate { get; set; }
        [DataMember(Name = "duration")]
        public int Duration { get; set; }
        [DataMember(Name = "height")]
        public int Height { get; set; }
        [DataMember(Name = "width")]
        public int Width { get; set; }
    }

    [DataContract]
    public class PhotoFacet
    {
        [DataMember(Name = "takenDateTime")]
        public string TakenDateTime { get; set; }
        [DataMember(Name = "cameraMake")]
        public string CameraMake { get; set; }
        [DataMember(Name = "cameraModel")]
        public string CameraModel { get; set; }
        [DataMember(Name = "fNumber")]
        public double FNumber { get; set; }
        [DataMember(Name = "exposureDenominator")]
        public double ExposureDenominator { get; set; }
        [DataMember(Name = "exposureNumerator")]
        public double ExposureNumerator { get; set; }
        [DataMember(Name = "focalLength")]
        public double FocalLength { get; set; }
        [DataMember(Name = "iso")]
        public int Iso { get; set; }
    }

    [DataContract]
    public class TimeStamp
    {
        [DataMember(Name = "dateTimeLastModified")]
        public string DateTimeLastModified { get; set; }
    }

    [DataContract]
    public class ImageFacet
    {
        [DataMember(Name = "width")]
        public int Width { get; set; }
        [DataMember(Name = "height")]
        public int Height { get; set; }
    }

    [DataContract]
    public class AudioFacet
    {
        [DataMember(Name = "album")]
        public string Album { get; set; }
        [DataMember(Name = "albumArtist")]
        public string AlbumArtist { get; set; }
        [DataMember(Name = "artist")]
        public string Artist { get; set; }
        [DataMember(Name = "bitrate")]
        public int Bitrate { get; set; }
        [DataMember(Name = "composers")]
        public string Composers { get; set; }
        [DataMember(Name = "copyright")]
        public string Copyright { get; set; }
        [DataMember(Name = "disc")]
        public int Disc { get; set; }
        [DataMember(Name = "discCount")]
        public int DiscCount { get; set; }
        [DataMember(Name = "duration")]
        public int Duration { get; set; }
        [DataMember(Name = "genre")]
        public string Genre { get; set; }
        [DataMember(Name = "hasDrm")]
        public bool HasDrm { get; set; }
        [DataMember(Name = "isVariableBitrate")]
        public bool IsVariableBitrate { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "track")]
        public int Track { get; set; }
        [DataMember(Name = "trackCount")]
        public int TrackCount { get; set; }
        [DataMember(Name = "year")]
        public int Year { get; set; }
    }

    [DataContract]
    public class FileFacet
    {
        [DataMember(Name = "mimeType", EmitDefaultValue = false)]
        public string MimeType { get; set; }
        [DataMember(Name = "hashes", EmitDefaultValue = false)]
        public HashesType Hashes { get; set; }
    }

    public class HashesType
    {
        [DataMember(Name = "crc32Hash")]
        public string Crc32Hash { get; set; }
        [DataMember(Name = "sha1Hash")]
        public string Sha1Hash { get; set; }
    }

    [DataContract]
    public class FolderFacet
    {
        [DataMember(Name = "childCount", EmitDefaultValue = false)]
        public int ChildCount { get; set; }
    }

    [DataContract]
    public class SearchResultFacet
    {
        [DataMember(Name = "onClickTelemetryUrl")]
        public string OnClickTelemetryUrl { get; set; }
    }

    [DataContract]
    public class RemoteItemFacet
    {
        [DataMember(Name = "remoteItem")]
        public Item RemoteItem { get; set; }
    }

    [DataContract]
    public class FileSystemInfoFacet
    {
        [DataMember(Name = "createdDateTime")]
        public string CreatedDateTime { get; set; }
        [DataMember(Name = "lastModifiedDateTime")]
        public string LastModifiedDateTime { get; set; }
    }

    [DataContract]
    public class LocationFacet
    {
        [DataMember(Name = "altitude", EmitDefaultValue = false)]
        public double Altitude { get; set; }
        [DataMember(Name = "latitude", EmitDefaultValue = false)]
        public double Latitude { get; set; }
        [DataMember(Name = "longitude", EmitDefaultValue = false)]
        public double Longitude { get; set; }
    }

    [DataContract]
    public class ItemReference
    {
        [DataMember(Name = "driveId", EmitDefaultValue = false)]
        public string DriveId { get; set; }
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "path", EmitDefaultValue = false)]
        public string Path { get; set; }
    }
}

