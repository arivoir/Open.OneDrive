using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Open.OneDrive;

public class Items
{
    [JsonPropertyName("value")]
    public List<Item> Value { get; set; }
    [JsonPropertyName("@odata.nextLink")]
    public string NextLink { get; set; }
}

public class Item
{
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyOrder(0)]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("eTag")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string ETag { get; set; }

    [JsonPropertyName("cTag")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string CTag { get; set; }

    [JsonPropertyName("createdBy")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IdentitySet CreatedBy { get; set; }

    [JsonPropertyName("createdDateTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string CreatedDateTime { get; set; }

    [JsonPropertyName("lastModifiedBy")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IdentitySet LastModifiedBy { get; set; }

    [JsonPropertyName("lastModifiedDateTime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string LastModifiedDateTime { get; set; }

    [JsonPropertyName("size")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public long Size { get; set; }

    [JsonPropertyName("webUrl")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string WebUrl { get; set; }

    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonPropertyOrder(2)]
    public string Description { get; set; }

    [JsonPropertyName("parentReference")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ItemReference ParentReference { get; set; }

    [JsonPropertyName("children")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Item Children { get; set; }

    [JsonPropertyName("folder")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FolderFacet Folder { get; set; }

    [JsonPropertyName("file")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FileFacet File { get; set; }

    [JsonPropertyName("fileSystemInfo")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FileSystemInfoFacet FileSystemInfo { get; set; }

    [JsonPropertyName("image")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ImageFacet Image { get; set; }

    [JsonPropertyName("photo")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public PhotoFacet Photo { get; set; }

    [JsonPropertyName("audio")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public AudioFacet Audio { get; set; }

    [JsonPropertyName("video")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public VideoFacet Video { get; set; }

    [JsonPropertyName("location")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public LocationFacet Location { get; set; }

    [JsonPropertyName("remoteItem")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public RemoteItemFacet RemoteItem { get; set; }

    [JsonPropertyName("searchResult")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public SearchResultFacet SearchResult { get; set; }

    [JsonPropertyName("deleted")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Deleted Deleted { get; set; }

    [JsonPropertyName("specialFolder")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public SpecialFolder SpecialFolder { get; set; }

    [JsonPropertyName("thumbnails")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<ThumbnailsSet> Thumbnails { get; set; }

    [JsonPropertyName("@content.downloadUrl")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string DownloadUrl { get; set; }

    [JsonPropertyName("@content.sourceUrl")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string SourceUrl { get; set; }


    [JsonPropertyName("shared")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Shared Shared { get; set; }

    [JsonPropertyName("@name.conflictBehavior")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ConflictBehavior ConflictBehavior { get; set; }
}

public enum ConflictBehavior
{
    Rename,
    Replace,
    Fail,
}

public class Deleted
{
}

public class Shared
{
    [JsonPropertyName("owner")]
    public IdentitySet Owner { get; set; }
    [JsonPropertyName("scope")]
    public string Scope { get; set; }
}

public class SpecialFolder
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}

public class VideoFacet
{
    [JsonPropertyName("bitrate")]
    public int Bitrate { get; set; }
    [JsonPropertyName("duration")]
    public int Duration { get; set; }
    [JsonPropertyName("height")]
    public int Height { get; set; }
    [JsonPropertyName("width")]
    public int Width { get; set; }
}

public class PhotoFacet
{
    [JsonPropertyName("takenDateTime")]
    public string TakenDateTime { get; set; }
    [JsonPropertyName("cameraMake")]
    public string CameraMake { get; set; }
    [JsonPropertyName("cameraModel")]
    public string CameraModel { get; set; }
    [JsonPropertyName("fNumber")]
    public double FNumber { get; set; }
    [JsonPropertyName("exposureDenominator")]
    public double ExposureDenominator { get; set; }
    [JsonPropertyName("exposureNumerator")]
    public double ExposureNumerator { get; set; }
    [JsonPropertyName("focalLength")]
    public double FocalLength { get; set; }
    [JsonPropertyName("iso")]
    public int Iso { get; set; }
}

public class TimeStamp
{
    [JsonPropertyName("dateTimeLastModified")]
    public string DateTimeLastModified { get; set; }
}

public class ImageFacet
{
    [JsonPropertyName("width")]
    public int Width { get; set; }
    [JsonPropertyName("height")]
    public int Height { get; set; }
}

public class AudioFacet
{
    [JsonPropertyName("album")]
    public string Album { get; set; }
    [JsonPropertyName("albumArtist")]
    public string AlbumArtist { get; set; }
    [JsonPropertyName("artist")]
    public string Artist { get; set; }
    [JsonPropertyName("bitrate")]
    public int Bitrate { get; set; }
    [JsonPropertyName("composers")]
    public string Composers { get; set; }
    [JsonPropertyName("copyright")]
    public string Copyright { get; set; }
    [JsonPropertyName("disc")]
    public int Disc { get; set; }
    [JsonPropertyName("discCount")]
    public int DiscCount { get; set; }
    [JsonPropertyName("duration")]
    public int Duration { get; set; }
    [JsonPropertyName("genre")]
    public string Genre { get; set; }
    [JsonPropertyName("hasDrm")]
    public bool HasDrm { get; set; }
    [JsonPropertyName("isVariableBitrate")]
    public bool IsVariableBitrate { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("track")]
    public int Track { get; set; }
    [JsonPropertyName("trackCount")]
    public int TrackCount { get; set; }
    [JsonPropertyName("year")]
    public int Year { get; set; }
}

public class FileFacet
{
    [JsonPropertyName("mimeType")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string MimeType { get; set; }

    [JsonPropertyName("hashes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public HashesType Hashes { get; set; }
}

public class HashesType
{
    [JsonPropertyName("crc32Hash")]
    public string Crc32Hash { get; set; }

    [JsonPropertyName("sha1Hash")]
    public string Sha1Hash { get; set; }
}

public class FolderFacet
{
    [JsonPropertyName("childCount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int ChildCount { get; set; }
}

public class SearchResultFacet
{
    [JsonPropertyName("onClickTelemetryUrl")]
    public string OnClickTelemetryUrl { get; set; }
}

public class RemoteItemFacet
{
    [JsonPropertyName("remoteItem")]
    public Item RemoteItem { get; set; }
}

public class FileSystemInfoFacet
{
    [JsonPropertyName("createdDateTime")]
    public string CreatedDateTime { get; set; }

    [JsonPropertyName("lastModifiedDateTime")]
    public string LastModifiedDateTime { get; set; }
}

public class LocationFacet
{
    [JsonPropertyName("altitude")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]

    public double Altitude { get; set; }

    [JsonPropertyName("latitude")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public double Longitude { get; set; }
}

public class ItemReference
{
    [JsonPropertyName("driveId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string DriveId { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("path")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Path { get; set; }
}

